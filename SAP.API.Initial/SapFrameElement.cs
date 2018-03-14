using SAP2000v20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.API.Initial
{
    class SapFrameElement
    {
        #region MembVariables
        SapPoint point1;
        SapPoint point2;
        SapRecangularSection rectsection;
        string label;
        string name;
        List<SapFrameDistLoad> distibutedLoads;
        List<SapFrameResult> frameResults;
        cSapModel mymodel;
        #endregion
        #region Prop
        public SapPoint Point1 { get => point1; set => point1 = value; }
        public SapPoint Point2 { get => point2; set => point2 = value; }
        public string Label { get => label; set => label = value; }
        public string Name { get => name; set => name = value; }
        internal SapRecangularSection Rectsection { get => rectsection; set => rectsection = value; }
        
        public cSapModel Mymodel { get => mymodel; set => mymodel = value; }
        internal List<SapFrameDistLoad> DistibutedLoads { get => distibutedLoads; set => distibutedLoads = value; }
        internal List<SapFrameResult> FrameResults { get => frameResults; set => frameResults = value; }
        #endregion
        #region Constructors
        public SapFrameElement(cSapModel mymodel ,SapPoint point1,SapPoint point2,SapRecangularSection rectsection,string label,string framename)
        {
            this.mymodel = mymodel;
            this.point1 = point1;
            this.point2 = point2;
            this.rectsection = rectsection;
            this.label = label;
            this.name = framename;
            this.distibutedLoads = new List<SapFrameDistLoad>();
            this.frameResults = new List<SapFrameResult>();
            this.mymodel.FrameObj.AddByPoint(this.point1.Name, this.point2.Name,ref this.label, this.rectsection.Name, this.name);
        }
        public SapFrameElement(cSapModel mymodel, SapPoint point1, SapPoint point2, string label, string framename)
        {
            this.mymodel = mymodel;
            this.point1 = point1;
            this.point2 = point2;
            this.label = label;
            this.name = framename;
            this.distibutedLoads = new List<SapFrameDistLoad>();
            this.frameResults = new List<SapFrameResult>();
            this.mymodel.FrameObj.AddByPoint(this.point1.Name, this.point2.Name, ref this.label, this.name);
        }
        public SapFrameElement(cSapModel mymodel,double x11,double y11,double z11,double x22,double y22,double z22,string framename,SapRecangularSection rectsection,string label)
        {
            this.mymodel = mymodel;
            point1.X = x11;
            point1.Y = y11;
            point1.Z = z11;
            point2.X = x22;
            point2.Y = y22;
            point2.Z = z22;
            this.rectsection = rectsection;
            this.label = label;
            this.name = framename;
            this.distibutedLoads = new List<SapFrameDistLoad>();
            this.frameResults = new List<SapFrameResult>();
            this.mymodel.FrameObj.AddByCoord(x11, y11, z11, x22, y22, z22, ref this.label, this.rectsection.Name, this.name);
            string temp1="", temp2="";
            this.mymodel.FrameObj.GetPoints(this.name, ref temp1, ref temp2);
            point1.Name = temp1;
            point2.Name = temp2;
        }
        public SapFrameElement(cSapModel mymodel, double x11, double y11, double z11, double x22, double y22, double z22, string framename, string label)
        {
            this.mymodel = mymodel;
            point1 = new SapPoint(this.mymodel,x11,y11,z11);
            point2 = new SapPoint(this.mymodel,x22,y22,z22);
            this.label = label;
            this.name = framename;
            this.distibutedLoads = new List<SapFrameDistLoad>();
            this.frameResults = new List<SapFrameResult>();
            this.mymodel.FrameObj.AddByCoord(x11, y11, z11, x22, y22, z22, ref this.label, this.name);
            string temp1 = "", temp2 = "";
            this.mymodel.FrameObj.GetPoints(this.name, ref temp1, ref temp2);
            point1.Name = temp1;
            point2.Name = temp2;
        }
        #endregion
        #region Methods
        public void AddDistributedLoad(SapFrameDistLoad distibutedload)
        {
            this.distibutedLoads.Add(distibutedload);
           int check= this.mymodel.FrameObj.SetLoadDistributed(this.label, distibutedload.LoadPattern.Name, distibutedload.Type, distibutedload.Direction, distibutedload.Distance1, distibutedload.Distance2, distibutedload.Value1, distibutedload.Value2,"Local",System.Convert.ToBoolean(-1),System.Convert.ToBoolean(-1),0);

        }

        #endregion
    }
}
