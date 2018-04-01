using SAP2000v20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.API.Initial
{
  public  enum MaterialType
    {
        STEEL = 1,

        CONCRETE = 2,

        NODESIGN = 3,

        ALUMINUM = 4,

        COLDFORMED = 5,

        REBAR = 6,

        TENDON = 7


    }
    enum SapColor
    {
        Default = -1,
    }
    class SapMaterial
    {
        #region Member Veriabels

           
            private string name;
            private  SapColor color;

        #endregion

        #region Properties


        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public SapColor Color { get => color; set => color = value; }
        public cSapModel SapModel { get; set; }


        #endregion

        #region Constructors


        public SapMaterial(cSapModel sapModel, string name, MaterialType type, SapColor color = SapColor.Default)
        {
            this.SapModel = sapModel;
            this.name = name;
            SetMaterial(name, type, color);
        }
        
        #endregion

        #region Methods

        public void SetMaterial(string name, MaterialType type, SapColor color = SapColor.Default)
        {
            SapModel.PropMaterial.SetMaterial(name, (eMatType)type, (int)color);
        }
        public void SetSteelMaterial(double Fy, double Fu, double EFy, double EFu)
        {

            SapModel.PropMaterial.SetOSteel_1(name, Fy, Fu, EFy, EFu, 1, 2, 0.02, 0.1, 0.2, -0.1);


        }
        public void SetConcreteMaterial(double Fc, bool IsLightWeight = false, double FcsFactor = 0)
        {
            int check= SapModel.PropMaterial.SetOConcrete_1(name, Fc, IsLightWeight, FcsFactor, 1, 2, 0.0022, 0.0052, -0.1);
        }

        public void SetIsotropic(double E, double U, double A)
        {
            int check = SapModel.PropMaterial.SetMPIsotropic(name, E, U, A);
        }
        public void SetWeight(double value )
        {
            int check = SapModel.PropMaterial.SetWeightAndMass(name, 1, value);
        }
        #endregion
    }
}
