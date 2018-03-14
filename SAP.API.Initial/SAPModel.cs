using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP2000v20;
namespace SAP.API.Initial
{
    public enum SapUnits
    {
        lb_in_F = 1,
        lb_ft_F = 2,
        kip_in_F = 3,
        kip_ft_F = 4,
        kN_mm_C = 5,
        kN_m_C = 6,
        kgf_mm_C = 7,
        kgf_m_C = 8,
        N_mm_C = 9,
        N_m_C = 10,
        Ton_mm_C = 11,
        Ton_m_C = 12,
        kN_cm_C = 13,
        kgf_cm_C = 14,
        N_cm_C = 15,
        Ton_cm_C = 16
    }
    class SapModel
    {
        public bool AttachToInstance { get; set; }

        public bool SpecifyPath { get; set; }
        public string ModelDirectory { get; set; }
        public string ModelName { get; set; }
        public string ModelPath { get; set; }
        public cSapModel SapObjectModel { get; set; }
        private cOAPI sapObjectAPI;

        #region Constructors
        public SapModel(string ModelDirectory, bool visible = false,string ModelName = "Untitled-API")
        {
            this.ModelDirectory = ModelDirectory;
            this.ModelName = ModelName;

            //set the following flag to true to attach to an existing instance of the program
            //otherwise a new instance of the program will be started

            AttachToInstance = false;

            //set the following flag to true to manually specify the path to SAP2000.exe

            //this allows for a connection to a version of SAP2000 other than the latest installation

            //otherwise the latest installed version of SAP2000 will be launched

            SpecifyPath = false;

            //if the above flag is set to true, specify the path to SAP2000 below

            string ProgramPath;

            ProgramPath = "C:\\Program Files (x86)\\Computers and Structures\\SAP2000 20\\SAP2000.exe";

            //full path to the model

            //set it to the desired path of your model

            try

            {

                System.IO.Directory.CreateDirectory(ModelDirectory);

            }

            catch (Exception ex)

            {

                Console.WriteLine("Could not create directory: " + ModelDirectory);

            }



             ModelPath = ModelDirectory + System.IO.Path.DirectorySeparatorChar + ModelName;



            //dimension the SapObject as cOAPI type

             sapObjectAPI = null;



            //Use ret to check if functions return successfully (ret = 0) or fail (ret = nonzero)

            int ret = 0;



            if (AttachToInstance)

            {
                //attach to a running instance of SAP2000
                try

                {
                    //get the active SapObject
                    sapObjectAPI = (cOAPI)System.Runtime.InteropServices.Marshal.GetActiveObject("CSI.SAP2000.API.SapObject");
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



                if (SpecifyPath)

                {
                    //'create an instance of the SapObject from the specified path
                    try

                    {
                        //create SapObject

                        sapObjectAPI = myHelper.CreateObject(ProgramPath);
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

                        sapObjectAPI = myHelper.CreateObjectProgID("CSI.SAP2000.API.SapObject");

                    }

                    catch (Exception ex)

                    {
                        Console.WriteLine("Cannot start a new instance of the program.");

                        return;
                    }
                }
                //start SAP2000 application
                
                ret = sapObjectAPI.ApplicationStart((eUnits)SapUnits.N_m_C,visible,"FileName");
               // mySapObject.Hide();
            }

            //create SapModel object
            SapObjectModel = sapObjectAPI.SapModel;
        }


        #endregion
        #region Methods
        
        public int Initialize(SapUnits sapUnits=SapUnits.N_m_C)
        {   
            //initialize model
            SapObjectModel.InitializeNewModel((eUnits)sapUnits);
            //create new blank model
           return  SapObjectModel.File.NewBlank();
            
        }
        public int RefreshView(int window=0,bool zoom=false)
        {
            return SapObjectModel.View.RefreshView(window, zoom);
        }

        public void RunAnalysis()
        {
            SapObjectModel.File.Save(ModelPath);
            SapObjectModel.Analyze.RunAnalysis();
        }

        public void Close(bool FileSave=true)
        {
            if (FileSave)
            {
                SapObjectModel.File.Save(ModelPath);
            }
            
            sapObjectAPI.ApplicationExit(FileSave);
            SapObjectModel = null;
            sapObjectAPI = null;
        }
        #endregion
    }
}
