using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP2000v20;

namespace SAP.API.Initial
{
    class SapFrameResult
    {

        #region Member Variables
        cSapModel sapModel;
        string frameName;
        int numberOfResults = 0;
        string[] frameObjName = new string[0];
        double[] frameObjStation = new double[0];
        string[] frameElmName = new string[0];
        double[] frameElmStation = new double[0];
        string[] loadCase = new string[0];
        string[] stepType = new string[0];
        double[] stepNum = new double[0];
        double[] p = new double[0];
        double[] v2 = new double[0];
        double[] v3 = new double[0];
        double[] t = new double[0];
        double[] m2 = new double[0];
        double[] m3 = new double[0];
        
        #endregion

        #region Properties
        public int NumberOfResults { get => numberOfResults; set => numberOfResults = value; }
        public string[] FrameObjName { get => frameObjName; set => frameObjName = value; }
        public string[] FrameElmName { get => frameElmName; set => frameElmName = value; }
        public string[] LoadCase { get => loadCase; set => loadCase = value; }
        public string[] StepType { get => stepType; set => stepType = value; }
        public double[] StepNum { get => stepNum; set => stepNum = value; }
        
        public cSapModel SapModel { get => sapModel; set => sapModel = value; }
        public string FrameName { get => frameName; set => frameName = value; }
        public double[] P { get => p; set => p = value; }
        public double[] V2 { get => v2; set => v2 = value; }
        public double[] V3 { get => v3; set => v3 = value; }
        public double[] T { get => t; set => t = value; }
        public double[] M2 { get => m2; set => m2 = value; }
        public double[] M3 { get => m3; set => m3 = value; }
        #endregion

        #region Constructors
        public SapFrameResult(cSapModel _sapModel, string _frameName)
        {
            sapModel = _sapModel;
            frameName = _frameName;
            int check = SapModel.Results.FrameForce(frameName, eItemTypeElm.ObjectElm, ref numberOfResults, ref frameObjName, ref frameObjStation, ref frameElmName, ref frameElmStation, ref loadCase, ref stepType, ref stepNum, ref p, ref v2, ref v3, ref t, ref m2, ref m3);
        }


        #endregion

        #region Methods


        #endregion

        #region Static Methods


        #endregion
    }
}
