using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP.API.Initial;
namespace SAP.API.Initial
{
    public enum Restrains
    {
        Fixed,
        Pinned,
        Roller,
        NoRestraint
    }
    /// <summary>
    /// 
    /// </summary>
  public  class SapJointRestraint
    {
        private bool[] restrains;

        public bool[] Restrains
        {
            get { return restrains; }
            set { restrains= value; }
        }

        #region Constructors
        /// <summary>
        /// assigns the restraint assignments for a point object. 
        /// The restraint assignments are always set in the point local coordinate system.
        /// </summary>
        /// <param name="RestrainType">default restraint types ex:fixed,roller</param>
        public SapJointRestraint(Restrains RestrainType)
        {
            restrains = new bool[6];
            switch (RestrainType)
            {
                case Initial.Restrains.Fixed:
                    for (int i = 0; i <6; i++)
                    {
                        restrains[i] = true;
                    }
                    break;

                case Initial.Restrains.Pinned:
                    for (int i = 0; i < 3; i++)
                    {
                        restrains[i] = true;
                    }
                    break;
                case Initial.Restrains.Roller:
                    //set constrains to Z-Axis
                    restrains[2] = true;
                    break;
                case Initial.Restrains.NoRestraint:

                    break;
                default:
                    break;
            }
            

        }
        /// <summary>
        /// assigns the restraint assignments for a point object. 
        /// The restraint assignments are always set in the point local coordinate system.        /// </summary>
        /// <param name="U1"></param>
        /// <param name="U2"></param>
        /// <param name="U3"></param>
        /// <param name="R1"></param>
        /// <param name="R2"></param>
        /// <param name="R3"></param>
        public void SetRestraint(bool U1,bool U2,bool U3,bool R1,bool R2,bool R3)
        {
            restrains[0] = U1;
            restrains[1] = U2;
            restrains[2] = U3;
            restrains[3] = R1;
            restrains[4] = R2;
            restrains[5] = R3;
        }
        #endregion


       
    }
}
