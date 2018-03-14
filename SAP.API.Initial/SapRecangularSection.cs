using SAP2000v20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.API.Initial
{
    
    class SapRecangularSection
    {
        #region MembVariables
        string name;
        SapMaterial material;
        double t2;
        double t3;
        string note;
        string guid;
        double[] modifiers;
        int color;
        cSapModel mysapmodel;
        #endregion
        #region Prop
        public string Name { get => name; set => name = value; }
        internal SapMaterial Material { get => material; set => material = value; }
        public double T2 { get => t2; set => t2 = value; }
        public double T3 { get => t3; set => t3 = value; }
        public string Note { get => note; set => note = value; }
        public string Guid { get => guid; set => guid = value; }
        public double[] Modifiers { get => modifiers; set => modifiers = value; }
        public int Color { get => color; set => color = value; }
        public cSapModel Mysapmodel { get => mysapmodel; set => mysapmodel = value; }
        #endregion
        #region constructor
        public SapRecangularSection(cSapModel my_model,SapMaterial material,string name,double t2,double t3,string note,string guid, int color, double[] modifiers)
        {
            this.mysapmodel = my_model;
            this.material = material;
            this.name = name;
            this.t2 = t2;
            this.t3 = t3;
            this.note = note;
            this.guid = guid;
            this.color = color;
            mysapmodel.PropFrame.SetRectangle(this.name, this.material.Name, this.t3, this.t2, this.color, this.note, this.guid);
            this.modifiers = new double[0];
            this.modifiers = modifiers;
            mysapmodel.PropFrame.SetModifiers(this.name, ref this.modifiers);
        }
        public SapRecangularSection(cSapModel my_model, SapMaterial material, string name, double t2, double t3, string note, string guid, int color)
        {
            this.mysapmodel = my_model;
            this.material = material;
            this.name = name;
            this.t2 = t2;
            this.t3 = t3;
            this.note = note;
            this.guid = guid;
            this.color = color;
            this.modifiers = new double[0];
            int check = mysapmodel.PropFrame.SetRectangle(this.name, this.material.Name, this.t3, this.t2, this.color, this.note, this.guid);
        }
        #endregion
        #region Methods
        public void SetModifiers(double[] Modifiers)
        {
            this.modifiers = Modifiers;
            mysapmodel.PropFrame.SetModifiers(name,ref modifiers);
        }
        #endregion


    }
}
