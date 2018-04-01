using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP2000v20;

namespace SAP.API.Initial
{
    public enum LoadPatternType
    {
        Dead = 1,
        SuperDead = 2,
        Live = 3,
        ReduceLive = 4,
        Quake = 5,
        Wind = 6,
        Snow = 7,
        Other = 8,
        Move = 9,
        Temperature = 10,
        Rooflive = 11,
        Notional = 12,
        PatternLive = 13,
        Wave = 14,
        Braking = 15,
        Centrifugal = 16,
        Friction = 17,
        Ice = 18,
        WindOnLiveLoad = 19,
        HorizontalEarthPressure = 20,
        VerticalEarthPressure = 21,
        EarthSurcharge = 22,
        DownDrag = 23,
        VehicleCollision = 24,
        VesselCollision = 25,
        TemperatureGradient = 26,
        Settlement = 27,
        Shrinkage = 28,
        Creep = 29,
        WaterloadPressure = 30,
        LiveLoadSurcharge = 31,
        LockedInForces = 32,
        PedestrianLL = 33,
        Prestress = 34,
        Hyperstatic = 35,
        Bouyancy = 36,
        StreamFlow = 37,
        Impact = 38,
        Construction = 39,
        DeadWearing = 40,
        DeadWater = 41,
        DeadManufacture = 42,
        EarthHydrostatic = 43,
        PassiveEarthPressure = 44,
        ActiveEarthPressure = 45,
        PedestrianLLReduced = 46,
        SnowHighAltitude = 47,
        EuroLm1Char = 48,
        EuroLm1Freq = 49,
        EuroLm2 = 50,
        EuroLm3 = 51,
        EuroLm4 = 52,
        SeaState = 53,
        Permit = 54,
        MoveFatigue = 55,
        MoveFatiguePermit = 56,
        MoveDeflection = 57
    }
   public class SapLoadPattern
    {
        #region Member Variables

        string name;
        double selfWtMultiplier;
        bool addAnalysisCase;
        cSapModel sapModel;


        #endregion

        #region Properties
        public string Name { get => name; set => name = value; }
        public double SelfWtMultiplier { get => selfWtMultiplier; set => selfWtMultiplier = value; }
        public bool AddAnalysisCase { get => addAnalysisCase; set => addAnalysisCase = value; }

        #endregion

        #region Constructors
        public SapLoadPattern(cSapModel _sapModel,string _name, LoadPatternType _loadPatternType, double _selfWtMultiplier,bool _addAnalysisCase)
        {
            sapModel = _sapModel;
            selfWtMultiplier = _selfWtMultiplier;
            addAnalysisCase = _addAnalysisCase;
            name = _name;
            sapModel.LoadPatterns.Add(name, (eLoadPatternType)_loadPatternType, selfWtMultiplier, addAnalysisCase);
        }

        #endregion

        #region Methods


        #endregion

        #region Static Methods


        #endregion
    }
}
