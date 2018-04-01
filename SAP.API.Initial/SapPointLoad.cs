using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP2000v20;

namespace SAP.API.Initial
{
   public class SapPointLoad
    {
        #region Member Variables

        SapLoadPattern loadPattern;
        double[] values ;
        #endregion

        #region Properties
        public double[] Values { get => values; set => values = value; }
        internal SapLoadPattern LoadPattern { get => loadPattern; set => loadPattern = value; }
        #endregion

        #region Constructors
        public SapPointLoad(SapLoadPattern _loadPattern, double[] _sixLoadValues, bool replace)
        {
            loadPattern = _loadPattern;
            try
            {
                if (_sixLoadValues.Length==6 && replace)
                {
                    values = _sixLoadValues;
                }
                else if (_sixLoadValues.Length == 6 && !replace)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        values[i] += _sixLoadValues[i];
                    }
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            
          
        }

        #endregion


        #region Methods


        #endregion

        #region Static Methods


        #endregion
    }
}
