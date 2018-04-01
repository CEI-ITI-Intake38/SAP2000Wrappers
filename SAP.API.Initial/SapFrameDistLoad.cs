using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.API.Initial
{
    class SapFrameDistLoad
    {
        #region Member Variables

        SapLoadPattern loadPattern;
        int type;
        int direction;
        double distance1;
        double distance2;
        double value1;
        double value2;



        #endregion

        #region Properties
        internal SapLoadPattern LoadPattern { get => loadPattern; set => loadPattern = value; }
        public int Type { get => type; set => type = value; }
        public int Direction { get => direction; set => direction = value; }
        public double Distance1 { get => distance1; set => distance1 = value; }
        public double Distance2 { get => distance2; set => distance2 = value; }
        public double Value1 { get => value1; set => value1 = value; }
        public double Value2 { get => value2; set => value2 = value; }

        #endregion

        #region Constructors
        public SapFrameDistLoad(SapLoadPattern _loadPattern,int _type,int _direction,double _distance1,double _distance2,double _value1,double _value2)
        {
            loadPattern = _loadPattern;
            type = _type;
            direction = _direction;
            distance1 = _distance1;
            distance2 = _distance2;
            value1 = _value1;
            value2 = _value2;
        }
        #endregion

        #region Methods


        #endregion

        #region Static Methods


        #endregion
    }
}
