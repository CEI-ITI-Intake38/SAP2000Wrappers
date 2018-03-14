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
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// set new material  of default Material types.
        /// </summary>
        /// <param name="sapModel"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        public SapMaterial(cSapModel sapModel, string name, MaterialType type, SapColor color = SapColor.Default)
        {
            this.SapModel = sapModel;
            this.name = name;
            SetMaterial(name, type, color);
        }

        #endregion

        #region Methods
        /// <summary>
        /// define new material
        /// </summary>
        /// <param name="name">The name of an existing or new material property. 
        /// If this is an existing property, that property is modified; 
        /// otherwise, a new property is added.</param>


        /// <param name="type">This is one of the standard material type</param>
        /// <param name="color"></param>
        
        public void SetMaterial(string name, MaterialType type, SapColor color = SapColor.Default)
        {
            SapModel.PropMaterial.SetMaterial(name, (eMatType)type, (int)color);
        }
        /// <summary>
        /// Set existing material properties to 'Steel Material'
        /// </summary>
        /// <param name="Fy">The minimum yield stress. [F/L2]</param>
        /// <param name="Fu">The minimum tensile stress. [F/L2]</param>
        /// <param name="EFy">The expected yield stress. [F/L2]</param>
        /// <param name="EFu">The expected tensile stress. [F/L2]</param>
        public void SetSteelMaterial(double Fy, double Fu, double EFy, double EFu)
        {

            SapModel.PropMaterial.SetOSteel_1(name, Fy, Fu, EFy, EFu, 1, 2, 0.02, 0.1, 0.2, -0.1);


        }
        /// <summary>
        /// Set existing material properties to 'Concrete Material'
        /// </summary>
        /// <param name="Fc">The concrete compressive strength. [F/L2]</param>
        /// <param name="IsLightWeight">If this item is True,
        /// the concrete is assumed to be lightweight concrete.</param>
        /// <param name="FcsFactor">The shear strength reduction factor 
        /// for lightweight concrete.</param>
        public void SetConcreteMaterial(double Fc, bool IsLightWeight = false, double FcsFactor = 0)
        {


            int check = SapModel.PropMaterial.SetOConcrete_1(name, Fc, IsLightWeight, FcsFactor, 1, 2, 0.0022, 0.0052, -0.1);
        }

        /// <summary>
        ///Set mechanical properties for a material with an isotropic directional symmetry type
        /// </summary>
        /// <param name="E"> The modulus of elasticity. [F/L2]</param>
        /// <param name="U">Poisson’s ratio.range from[0:0.5]</param>
        /// <param name="A">The thermal coefficient. [1/T]</param>
        public void SetIsotropic(double E, double U, double A)
        {
            int check = SapModel.PropMaterial.SetMPIsotropic(name, E, U, A);
        }
        /// <summary>
        /// This function assigns weight per unit volume to a material property.
        /// </summary>
        /// <param name="value"></param>
        public void SetWeight(double value )
        {
            int check = SapModel.PropMaterial.SetWeightAndMass(name, 1, value);
        }
        #endregion
    }
}
