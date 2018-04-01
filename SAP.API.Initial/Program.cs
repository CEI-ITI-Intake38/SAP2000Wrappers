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
            int choise;
            Console.WriteLine("For SapAPI tutorial type: 1 ");
            Console.WriteLine("For SapAPI Wrapper  type: 2 ");
            Console.WriteLine("type (1 or 2) :  ");
            choise = Convert.ToInt32(Console.ReadLine().ToString());

            switch (choise)
            {
                case 1:
                    RunSapTutorial();
                    break;
                case 2:
                    RunWrapperCode();
                    break;
                default:
                    break;
            }
            
        }

        static void RunWrapperCode()
        {
            #region Wrapper Code



            Console.Write("Enter the number of spans in X direction : ");
            int nSpanX = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the value of span in X direction : ");
            double spanX = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter the number of spans in Y direction : ");
            int nSpanY = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the value of span in Y direction : ");
            double spanY = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter the number of stories : ");
            int nStory = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the value of story height : ");
            double storyHeight = Convert.ToDouble(Console.ReadLine());

            Console.Write("Do you want to display sap windows (0 For No) (any integer for Yes): ");
            bool showSap = Convert.ToBoolean(Convert.ToInt32(Console.ReadLine()));


            //setting model origin
            double originX = 0;
            double originY = 0;
            double originZ = 0;


            Console.WriteLine("Opening Sap Application ...... ");
            Console.WriteLine(".............................. ");


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
            Console.WriteLine("Running Analysis ....");
            model.RunAnalysis();


            Console.WriteLine("Reading Results ....");
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



            Console.WriteLine("Finished , press any key to exit ");
            Console.ReadLine();

            model.Close(true);

            #endregion
        }

        static void RunSapTutorial()
        {
            #region SapTutorial

            //set the following flag to true to attach to an existing instance of the program

            //otherwise a new instance of the program will be started

            bool AttachToInstance;

            AttachToInstance = false;



            //set the following flag to true to manually specify the path to SAP2000.exe

            //this allows for a connection to a version of SAP2000 other than the latest installation

            //otherwise the latest installed version of SAP2000 will be launched

            bool SpecifyPath;

            SpecifyPath = false;



            //if the above flag is set to true, specify the path to SAP2000 below

            string ProgramPath;

            ProgramPath = "C:\\Program Files (x86)\\Computers and Structures\\SAP2000 19\\SAP2000.exe";



            //full path to the model

            //set it to the desired path of your model

            string ModelDirectory = "C:\\CSiAPIexample";

            try
            {
                System.IO.Directory.CreateDirectory(ModelDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not create directory: " + ModelDirectory);
            }

            string ModelName = "API_1-001.sdb";
            string ModelPath = ModelDirectory + System.IO.Path.DirectorySeparatorChar + ModelName;



            //dimension the SapObject as cOAPI type
            cOAPI mySapObject = null;


            //Use ret to check if functions return successfully (ret = 0) or fail (ret = nonzero)
            int ret = 0;


            // AttachToInstance -- initialized to False
            if (AttachToInstance)
            {
                //attach to a running instance of SAP2000

                try
                {
                    //get the active SapObject
                    mySapObject = (cOAPI)System.Runtime.InteropServices.Marshal.GetActiveObject("CSI.SAP2000.API.SapObject");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("No running instance of the program found or failed to attach.");
                    return;
                }

            }
            else
            {
                //create API helper object
                cHelper myHelper;

                try
                {
                    myHelper = new Helper();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cannot create an instance of the Helper object");
                    return;
                }


                // SpecifyPath -- initialized to False
                if (SpecifyPath)
                {
                    //'create an instance of the SapObject from the specified path
                    try
                    {
                        //create SapObject
                        mySapObject = myHelper.CreateObject(ProgramPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cannot start a new instance of the program from " + ProgramPath);
                        return;
                    }

                }
                else
                {
                    //'create an instance of the SapObject from the latest installed SAP2000
                    try
                    {
                        //create SapObject
                        mySapObject = myHelper.CreateObjectProgID("CSI.SAP2000.API.SapObject");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cannot start a new instance of the program.");
                        return;
                    }

                }

                //start SAP2000 application
                ret = mySapObject.ApplicationStart();

            }



            //create SapModel object
            cSapModel mySapModel;
            mySapModel = mySapObject.SapModel;



            //initialize model
            //ret = mySapModel.InitializeNewModel((eUnits.kN_mm_C));
            ret = mySapModel.InitializeNewModel((eUnits.kip_in_F));



            //create new blank model
            ret = mySapModel.File.NewBlank();



            //define material property
            ret = mySapModel.PropMaterial.SetMaterial("CONC", eMatType.Concrete, -1, "", "");



            //assign isotropic mechanical properties to material
            ret = mySapModel.PropMaterial.SetMPIsotropic("CONC", 3600, 0.2, 0.0000055, 0);



            //define rectangular frame section property
            ret = mySapModel.PropFrame.SetRectangle("R1", "CONC", 12, 12, -1, "", "");



            //define frame section property modifiers
            double[] ModValue = new double[8];
            int i;
            for (i = 0; i <= 7; i++)
            {
                ModValue[i] = 1;
            }

            ModValue[0] = 1000;
            ModValue[1] = 0;
            ModValue[2] = 0;

            ret = mySapModel.PropFrame.SetModifiers("R1", ref ModValue);



            //switch to k-ft units
            ret = mySapModel.SetPresentUnits(eUnits.kip_ft_F);



            //add frame object by coordinates
            string[] FrameName = new string[3];
            string temp_string1 = FrameName[0];
            string temp_string2 = FrameName[0];

            // ret = mySapModel.FrameObj.AddByCoord(0, 0, 0, 0, 0, 10, ref temp_string1, "R1"/*Property*/, "1"/*Label*/, "Global");
            FrameName[0] = temp_string1;

            //  ret = mySapModel.FrameObj.AddByCoord(0, 0, 10, 8, 0, 16, ref temp_string1, "R1", "2", "Global");
            FrameName[1] = temp_string1;

            ret = mySapModel.FrameObj.AddByCoord(-4, 0, 10, 0, 0, 10, ref temp_string1, "R1", "3", "Global");
            FrameName[2] = temp_string1;
            string framename = "";
            SapMaterial s = new SapMaterial(mySapModel, "ff", MaterialType.CONCRETE);
            SapRecangularSection srs = new SapRecangularSection(mySapModel, s, "fouad", 12, 25, "", "", -1);
            SapFrameElement sfm = new SapFrameElement(mySapModel, 0, 0, 0, 0, 0, 10, "ah", "");
            SapPoint point1 = new SapPoint(mySapModel, 0, 0, 10);
            SapPoint point2 = new SapPoint(mySapModel, 8, 0, 16);
            SapFrameElement sfm2 = new SapFrameElement(mySapModel, point1, point2, "", "fo2sh");
            SapLoadPattern loadpattern = new SapLoadPattern(mySapModel, "m", LoadPatternType.Other, 0, true);
            SapFrameDistLoad distload = new SapFrameDistLoad(loadpattern, 1, 2, 0, 1, 2, 2);
            sfm2.AddDistributedLoad(distload);
            point2.SetRestraint(Restrains.Fixed);
            SapPointLoad pload = new SapPointLoad(loadpattern, new double[] { 1, 2, 3, 4, 5, 6 }, true);
            point2.AddPointLoad(pload);
            //sfm.Point1.SetRestraint(Restrains.Fixed);
            //assign point object restraint at base
            string[] PointName = new string[2];
            bool[] Restraint = new bool[6];
            for (i = 0; i <= 3; i++)
            {
                Restraint[i] = true;
            }
            for (i = 4; i <= 5; i++)
            {
                Restraint[i] = false;
            }

            ret = mySapModel.FrameObj.GetPoints(FrameName[0], ref temp_string1, ref temp_string2);
            PointName[0] = temp_string1;
            PointName[1] = temp_string2;

            ret = mySapModel.PointObj.SetRestraint(PointName[0], ref Restraint, 0);



            //assign point object restraint at top
            for (i = 0; i <= 1; i++)
            {
                Restraint[i] = true;
            }
            for (i = 2; i <= 5; i++)
            {
                Restraint[i] = false;
            }

            ret = mySapModel.FrameObj.GetPoints(FrameName[1], ref temp_string1, ref temp_string2);
            PointName[0] = temp_string1;
            PointName[1] = temp_string2;

            ret = mySapModel.PointObj.SetRestraint(PointName[1], ref Restraint, 0);



            //refresh view, update (initialize) zoom
            bool temp_bool = false;
            ret = mySapModel.View.RefreshView(0, temp_bool);



            //add load patterns
            temp_bool = true;
            ret = mySapModel.LoadPatterns.Add("1", eLoadPatternType.Other, 1, temp_bool);
            ret = mySapModel.LoadPatterns.Add("2", eLoadPatternType.Other, 0, temp_bool);
            ret = mySapModel.LoadPatterns.Add("3", eLoadPatternType.Other, 0, temp_bool);
            ret = mySapModel.LoadPatterns.Add("4", eLoadPatternType.Other, 0, temp_bool);
            ret = mySapModel.LoadPatterns.Add("5", eLoadPatternType.Other, 0, temp_bool);
            ret = mySapModel.LoadPatterns.Add("6", eLoadPatternType.Other, 0, temp_bool);
            ret = mySapModel.LoadPatterns.Add("7", eLoadPatternType.Other, 0, temp_bool);



            //assign loading for load pattern 2
            ret = mySapModel.FrameObj.GetPoints(FrameName[2], ref temp_string1, ref temp_string2);
            PointName[0] = temp_string1;
            PointName[1] = temp_string2;
            double[] PointLoadValue = new double[6];
            PointLoadValue[2] = -10;
            ret = mySapModel.PointObj.SetLoadForce(PointName[0], "2", ref PointLoadValue, false, "Global", 0);
            ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[2], "2", 1, 10, 0, 1, 1.8, 1.8, "Global", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);



            //assign loading for load pattern 3
            ret = mySapModel.FrameObj.GetPoints(FrameName[2], ref temp_string1, ref temp_string2);
            PointName[0] = temp_string1;
            PointName[1] = temp_string2;
            PointLoadValue = new double[6];
            PointLoadValue[2] = -17.2;
            PointLoadValue[4] = -54.4;
            ret = mySapModel.PointObj.SetLoadForce(PointName[1], "3", ref PointLoadValue, false, "Global", 0);




            //assign loading for load pattern 4
            ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[1], "4", 1, 11, 0, 1, 2, 2, "Global", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);



            //assign loading for load pattern 5
            ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[0], "5", 1, 2, 0, 1, 2, 2, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);
            ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[1], "5", 1, 2, 0, 1, -2, -2, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);



            //assign loading for load pattern 6
            ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[0], "6", 1, 2, 0, 1, 0.9984, 0.3744, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);
            ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[1], "6", 1, 2, 0, 1, -0.3744, 0, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);



            //assign loading for load pattern 7
            ret = mySapModel.FrameObj.SetLoadPoint(FrameName[1], "7", 1, 2, 0.5, -15, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);



            //switch to k-in units
            ret = mySapModel.SetPresentUnits(eUnits.kip_in_F);



            //save model
            ret = mySapModel.File.Save(ModelPath);



            //run model (this will create the analysis model)
            ret = mySapModel.Analyze.RunAnalysis();



            //initialize for SAP2000 results
            double[] SapResult = new double[7];
            ret = mySapModel.FrameObj.GetPoints(FrameName[1], ref temp_string1, ref temp_string2);

            PointName[0] = temp_string1;
            PointName[1] = temp_string2;



            //get SAP2000 results for load patterns 1 through 7           
            int NumberResults = 0;
            string[] Obj = new string[1];
            string[] Elm = new string[1];
            string[] LoadCase = new string[1];
            string[] StepType = new string[1];
            double[] StepNum = new double[1];

            double[] U1 = new double[1];
            double[] U2 = new double[1];
            double[] U3 = new double[1];
            double[] R1 = new double[1];
            double[] R2 = new double[1];
            double[] R3 = new double[1];

            for (i = 0; i <= 6; i++)
            {
                ret = mySapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();
                ret = mySapModel.Results.Setup.SetCaseSelectedForOutput(System.Convert.ToString(i + 1), System.Convert.ToBoolean(-1));

                if (i <= 3)
                {
                    ret = mySapModel.Results.JointDispl(PointName[1], eItemTypeElm.ObjectElm, ref NumberResults, ref Obj, ref Elm, ref LoadCase, ref StepType, ref StepNum, ref U1, ref U2, ref U3, ref R1, ref R2, ref R3);
                    U3.CopyTo(U3, 0);
                    SapResult[i] = U3[0];
                }
                else
                {
                    ret = mySapModel.Results.JointDispl(PointName[0], eItemTypeElm.ObjectElm, ref NumberResults, ref Obj, ref Elm, ref LoadCase, ref StepType, ref StepNum, ref U1, ref U2, ref U3, ref R1, ref R2, ref R3);
                    U1.CopyTo(U1, 0);
                    SapResult[i] = U1[0];
                }

            }



            //close SAP2000

            mySapObject.ApplicationExit(false);
            mySapModel = null;
            mySapObject = null;



            //fill SAP2000 result strings
            string[] SapResultString = new string[7];
            for (i = 0; i <= 6; i++)
            {
                SapResultString[i] = string.Format("{0:0.00000}", SapResult[i]);
                ret = (string.Compare(SapResultString[i], 1, "-", 1, 1, true));
                if (ret != 0)
                {
                    SapResultString[i] = " " + SapResultString[i];
                }

            }



            //fill independent results
            double[] IndResult = new double[7];
            string[] IndResultString = new string[7];
            IndResult[0] = -0.02639;
            IndResult[1] = 0.06296;
            IndResult[2] = 0.06296;
            IndResult[3] = -0.2963;
            IndResult[4] = 0.3125;
            IndResult[5] = 0.11556;
            IndResult[6] = 0.00651;
            for (i = 0; i <= 6; i++)
            {

                IndResultString[i] = string.Format("{0:0.00000}", IndResult[i]);
                ret = (string.Compare(IndResultString[i], 1, "-", 1, 1, true));
                if (ret != 0)
                {
                    IndResultString[i] = " " + IndResultString[i];
                }

            }



            //fill percent difference
            double[] PercentDiff = new double[7];
            string[] PercentDiffString = new string[7];
            for (i = 0; i <= 6; i++)
            {
                PercentDiff[i] = (SapResult[i] / IndResult[i]) - 1;
                PercentDiffString[i] = string.Format("{0:0%}", PercentDiff[i]);
                ret = (string.Compare(PercentDiffString[i], 1, "-", 1, 1, true));
                if (ret != 0)
                {
                    PercentDiffString[i] = " " + PercentDiffString[i];
                }

            }



            //display message box comparing results
            string msg = "";
            msg = msg + "LC  Sap2000  Independent  %Diff\r\n";
            for (i = 0; i <= 5; i++)
            {
                msg = msg + string.Format("{0:0}", i + 1) + "    " + SapResultString[i] + "   " + IndResultString[i] + "       " + PercentDiffString[i] + "\r\n";
            }


            msg = msg + string.Format("{0:0}", i + 1) + "    " + SapResultString[i] + "   " + IndResultString[i] + "       " + PercentDiffString[i];
            Console.WriteLine(msg);
            Console.ReadKey();

            #endregion
        }
    }
}
