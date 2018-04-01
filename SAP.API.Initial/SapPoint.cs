using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP2000v20;

namespace SAP.API.Initial
{
   public class SapPoint
    {

        #region Member Variables

        
        private string name;
        private double x;
        private double y;
        private double z;

        SapJointRestraint jointRestraint;
        public SapJointRestraint JointRestraint
        {
            get { return jointRestraint; }
            set { jointRestraint = value;UpdateRestrains(); }
        }

        List<SapPointLoad> pointloads;
        List<SapPointResult> pointResults;
        #endregion

        #region Properties
        public cSapModel SapModel { get; set; }
        public double X
        {
            get { return x; }
            set { x = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        public string Name
        {
            get { return name; }
            set {
                // changeing the name of point
                SapModel.PointObj.ChangeName(name, value);
                name = value;
            }
        }

        public List<SapPointLoad> Pointloads { get => pointloads; set => pointloads = value; }
        internal List<SapPointResult> PointResults { get => pointResults; set => pointResults = value; }
        #endregion

        #region Constructors
        public SapPoint(cSapModel model,double x,double y,double z)
        {
            this.SapModel = model;
            this.x = x;
            this.y = y;
            this.z = z;
            model.PointObj.AddCartesian(x, y, z, ref name);
            //Iniitialize the point with no restraints;
            jointRestraint = new SapJointRestraint(Restrains.NoRestraint);
            pointloads = new List<SapPointLoad>();
            pointResults = new List<SapPointResult>();
        }

        public SapPoint(cSapModel model):this(model,0,0,0)
        {
        }
        #endregion


        #region Methods

        public void SetRestraint(Restrains restrains)
        {
            jointRestraint = new SapJointRestraint(restrains);
            bool[] tempRestrains = jointRestraint.Restrains;
           int r= SapModel.PointObj.SetRestraint(name, ref tempRestrains,0);
            
        }
        public void SetRestraint(bool U1, bool U2, bool U3, bool R1, bool R2, bool R3)
        {
            jointRestraint.SetRestraint(U1, U2, U3, R1, R2, R3);
            bool[] tempRestrains = jointRestraint.Restrains;
            SapModel.PointObj.SetRestraint(name, ref tempRestrains,0);
            
        }
        public void DeleteRestraint()
        {
            SapModel.PointObj.DeleteRestraint(name);
        }
        public void AddPointLoad(SapPointLoad pointload)
        {
            double[] temp = new double[6];
            temp = pointload.Values;
            this.SapModel.PointObj.SetLoadForce(this.name, pointload.LoadPattern.Name, ref temp, true, "Global", 0);
            pointload.Values = temp;
            pointloads.Add(pointload);
        }

        #endregion

        #region private Method
        private void UpdateRestrains()
        {
            bool[] tempRestrains = jointRestraint.Restrains;
            SapModel.PointObj.SetRestraint(name, ref tempRestrains);

        }
        #endregion


        #region Static Methods
        public static long Count(cSapModel SapModel)
        {
             return SapModel.PointObj.Count();
        }

        public static string[] GetNameList(cSapModel sapModel)
        {
            int NumberNames=0;
            //get the count of the point to initialise the array size first
            long PointCount=Count(sapModel);
            string[] PointNames=new string[PointCount];

            sapModel.PointObj.GetNameList(ref NumberNames, ref PointNames);
            return PointNames;

        }
       
        #endregion


    }
}
