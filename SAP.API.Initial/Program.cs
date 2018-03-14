using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP2000v20;


namespace SAP.API.Initial
{
    class Program
    {
        static void Main(string[] args)
        {

            "SAP API Wrappers with C#".Header(ConsoleColor.Yellow,ConsoleColor.Black);

            "Enter the number of spans in X direction : ".Print(ConsoleColor.Cyan);
            int nSpanX = Convert.ToInt32(Console.ReadLine());
            "Enter the value of span in X direction : ".Print(ConsoleColor.Cyan);
            double spanX = Convert.ToDouble(Console.ReadLine());

            "Enter the number of spans in Y direction : ".Print(ConsoleColor.Cyan);
            int nSpanY = Convert.ToInt32(Console.ReadLine());
            "Enter the value of span in Y direction : ".Print(ConsoleColor.Cyan);
            double spanY = Convert.ToDouble(Console.ReadLine());

            "Enter the number of stories : ".Print(ConsoleColor.Cyan);
            int nStory = Convert.ToInt32(Console.ReadLine());
            "Enter the value of story height : ".Print(ConsoleColor.Cyan);
            double storyHeight = Convert.ToDouble(Console.ReadLine());

            "Do you want to display sap windows (0 For No) (any integer for Yes): ".PrintCenter(ConsoleColor.Red);
            bool showSap = Convert.ToBoolean(Convert.ToInt32(Console.ReadLine()));


            //setting model origin
            double originX = 0;
            double originY = 0;
            double originZ = 0;


            "Opening Sap Application ...... ".Print(ConsoleColor.Green);
            "------------------------------------ ".PrintCenter(ConsoleColor.Yellow);


            //setting file path and file name
            string filePath = "C:\\CSiAPIexample";
            string fileName = "trial1";

            //initializing the model
            SapModel model = new SapModel(filePath, showSap, fileName);
            model.Initialize();

            //setting Sap units
            model.SapObjectModel.SetPresentUnits(eUnits.Ton_m_C);


            //creating material
            SapMaterial material1 = new SapMaterial(model.SapObjectModel, "FCU25", MaterialType.CONCRETE);
            material1.SetConcreteMaterial(2500);
            material1.SetIsotropic(2200000, 0.2, 9.900E-06);
            material1.SetWeight(2.5);

            //creating two sections
            SapRecangularSection B30x60 = new SapRecangularSection(model.SapObjectModel, material1, "B30x60", 0.30, 0.60, "this is a new rec section", "", -1);
            SapRecangularSection C30x30 = new SapRecangularSection(model.SapObjectModel, material1, "C30x30", 0.30, 0.30, "this is a new rec section", "", -1);

            //creating points 
            SapPoint[,,] points = new SapPoint[nSpanX + 1, nSpanY + 1, nStory + 1];
            for (int zStep = 0; zStep < nStory + 1; zStep++)
            {
                for (int xStep = 0; xStep < nSpanX + 1; xStep++)
                {
                    for (int yStep = 0; yStep < nSpanY + 1; yStep++)
                    {
                        points[xStep, yStep, zStep] = new SapPoint(model.SapObjectModel, originX + xStep * spanX, originY + yStep * spanY, originZ + zStep * storyHeight);
                    }
                }
            }


            //creating columns
            SapFrameElement[,,] columns = new SapFrameElement[nSpanX + 1, nSpanY + 1, nStory];
            for (int zStep = 0; zStep < nStory; zStep++)
            {
                for (int xStep = 0; xStep < nSpanX + 1; xStep++)
                {
                    for (int yStep = 0; yStep < nSpanY + 1; yStep++)
                    {
                        columns[xStep, yStep, zStep] = new SapFrameElement(model.SapObjectModel, points[xStep, yStep, zStep], points[xStep, yStep, zStep + 1], C30x30, $"col{xStep},{yStep},{zStep}", $"col{xStep},{yStep},{zStep}");
                    }
                }
            }

            //creating beams in X direction
            SapFrameElement[,,] xBeams = new SapFrameElement[nSpanX, nSpanY + 1, nStory];
            for (int zStep = 0; zStep < nStory; zStep++)
            {
                for (int xStep = 0; xStep < nSpanX; xStep++)
                {
                    for (int yStep = 0; yStep < nSpanY + 1; yStep++)
                    {
                        xBeams[xStep, yStep, zStep] = new SapFrameElement(model.SapObjectModel, points[xStep, yStep, zStep + 1], points[xStep + 1, yStep, zStep + 1], B30x60, $"xBeam{xStep},{yStep},{zStep}", $"xBeam{xStep},{yStep},{zStep}");
                    }
                }
            }

            //creating beams in Y direction
            SapFrameElement[,,] yBeams = new SapFrameElement[nSpanX + 1, nSpanY, nStory];
            for (int zStep = 0; zStep < nStory; zStep++)
            {
                for (int xStep = 0; xStep < nSpanX + 1; xStep++)
                {
                    for (int yStep = 0; yStep < nSpanY; yStep++)
                    {
                        yBeams[xStep, yStep, zStep] = new SapFrameElement(model.SapObjectModel, points[xStep, yStep, zStep + 1], points[xStep, yStep + 1, zStep + 1], B30x60, $"yBeam{xStep},{yStep},{zStep}", $"yBeam{xStep},{yStep},{zStep}");
                    }
                }
            }

            //setting base points as hinged
            for (int xStep = 0; xStep < nSpanX + 1; xStep++)
            {
                for (int yStep = 0; yStep < nSpanY + 1; yStep++)
                {
                    points[xStep, yStep, 0].SetRestraint(Restrains.Pinned);
                }
            }



            //adding load patterns D and L and setting last element as true to create corresponding load case
            List<SapLoadPattern> loadPatternlist = new List<SapLoadPattern>();
            SapLoadPattern deadLoadPattern = new SapLoadPattern(model.SapObjectModel, "D", LoadPatternType.Dead, 1, true);
            loadPatternlist.Add(deadLoadPattern);
            SapLoadPattern liveLoadPattern = new SapLoadPattern(model.SapObjectModel, "L", LoadPatternType.Live, 0, true);
            loadPatternlist.Add(liveLoadPattern);

            //creating distributed loads
            SapFrameDistLoad deadDistLoad = new SapFrameDistLoad(deadLoadPattern, 1, 2, 0, 1, -2, -2);
            SapFrameDistLoad liveDistLoad = new SapFrameDistLoad(liveLoadPattern, 1, 2, 0, 1, -1.5, -1.5);
            
            //assigning loads to beams
            foreach (var beam in xBeams)
            {
                beam.AddDistributedLoad(deadDistLoad);
                beam.AddDistributedLoad(liveDistLoad);
            }
            foreach (var beam in yBeams)
            {
                beam.AddDistributedLoad(deadDistLoad);
                beam.AddDistributedLoad(liveDistLoad);
            }

            //adding earth quake load pattern
            SapLoadPattern QuakeLoadPattern = new SapLoadPattern(model.SapObjectModel, "Qx", LoadPatternType.Quake, 0, true);
            loadPatternlist.Add(QuakeLoadPattern);
            
            //creating point load in x direction
            SapPointLoad QxPointLoad = new SapPointLoad(QuakeLoadPattern, new double[] { 10, 0, 0, 0, 0, 0 }, true);

            //assigning point load to points on the left of the building
            for (int zStep = 1; zStep < nStory + 1; zStep++)
            {
                for (int yStep = 0; yStep < nSpanY + 1; yStep++)
                {
                    points[0, yStep, zStep].AddPointLoad(QxPointLoad);
                }
            }



            model.RefreshView(0, false);
            "Running Analysis ....".Print(ConsoleColor.Green);
            model.RunAnalysis();


            "Reading Results ....".Print(ConsoleColor.Yellow); ;
            //reading the results 
            foreach (var loadpattern in loadPatternlist)
            {
                model.SapObjectModel.Results.Setup.DeselectAllCasesAndCombosForOutput();
                model.SapObjectModel.Results.Setup.SetCaseSelectedForOutput(loadpattern.Name, true);

                foreach (var column in columns)
                {
                    column.FrameResults.Add(new SapFrameResult(model.SapObjectModel, column.Label));
                }
                foreach (var beam in xBeams)
                {
                    beam.FrameResults.Add(new SapFrameResult(model.SapObjectModel, beam.Label));
                }
                foreach (var beam in yBeams)
                {
                    beam.FrameResults.Add(new SapFrameResult(model.SapObjectModel, beam.Label));
                }
                foreach (var point in points)
                {
                    point.PointResults.Add(new SapPointResult(model.SapObjectModel, point.Name));
                }

            }


            "Finished , press any key to close SAP and exit ".PrintCenter(ConsoleColor.DarkBlue);
            Console.ReadLine();
            
            model.Close(true);


        }
    }
}
