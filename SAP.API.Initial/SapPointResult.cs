using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP2000v20;

namespace SAP.API.Initial
{
    class SapPointResult
    {

        #region Member Variables
        cSapModel sapModel;
        string pointName;
        int numberOfResults = 0;
        string[] pointObjName = new string[0];
        string[] pointElmName = new string[0];
        string[] loadCase = new string[0];
        string[] stepType = new string[0];
        double[] stepNum = new double[0];
        double[] u1 = new double[0];
        double[] u2 = new double[0];
        double[] u3 = new double[0];
        double[] r1 = new double[0];
        double[] r2 = new double[0];
        double[] r3 = new double[0];
        double[] f1 = new double[0];
        double[] f2 = new double[0];
        double[] f3 = new double[0];
        double[] m1 = new double[0];
        double[] m2 = new double[0];
        double[] m3 = new double[0];
        #endregion

        #region Properties
        public int NumberOfResults { get => numberOfResults; set => numberOfResults = value; }
        public string[] PointObjName { get => pointObjName; set => pointObjName = value; }
        public string[] PointElmName { get => pointElmName; set => pointElmName = value; }
        public string[] LoadCase { get => loadCase; set => loadCase = value; }
        public string[] StepType { get => stepType; set => stepType = value; }
        public double[] StepNum { get => stepNum; set => stepNum = value; }
        public double[] U1 { get => u1; set => u1 = value; }
        public double[] U2 { get => u2; set => u2 = value; }
        public double[] U3 { get => u3; set => u3 = value; }
        public double[] R1 { get => r1; set => r1 = value; }
        public double[] R2 { get => r2; set => r2 = value; }
        public double[] R3 { get => r3; set => r3 = value; }
        public double[] F1 { get => f1; set => f1 = value; }
        public double[] F2 { get => f2; set => f2 = value; }
        public double[] F3 { get => f3; set => f3 = value; }
        public double[] M1 { get => m1; set => m1 = value; }
        public double[] M2 { get => m2; set => m2 = value; }
        public double[] M3 { get => m3; set => m3 = value; }
        public cSapModel SapModel { get => sapModel; set => sapModel = value; }
        public string PointName { get => pointName; set => pointName = value; }
        #endregion

        #region Constructors
        public SapPointResult(cSapModel _sapModel, string _pointName)
        {
            sapModel = _sapModel;
            pointName = _pointName;
            sapModel.Results.JointDispl(pointName, eItemTypeElm.ObjectElm, ref numberOfResults, ref pointObjName, ref pointElmName, ref loadCase, ref stepType, ref stepNum, ref u1, ref u2, ref u3, ref r1, ref r2, ref r3);
            sapModel.Results.JointReact(pointName, eItemTypeElm.ObjectElm, ref numberOfResults, ref pointObjName, ref pointElmName, ref loadCase, ref stepType, ref stepNum, ref f1, ref f2, ref f3, ref m1, ref m2, ref m3);
           
        }


        #endregion

        #region Methods


        #endregion

        #region Static Methods


        #endregion
    }
}
