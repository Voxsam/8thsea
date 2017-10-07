using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    #region Structures
    //Fish details.
    public class FishParameters
    {
        private FishType type;
        public StationType[] ResearchProtocols {// The order of Stations to follow
            get; private set;
        }

        public float panicTimerLength;
        public int currentResearchProtocol;
        public int totalResearched;
        public int totalToResearch;

        public float minSpeed;
        public float maxSpeed;
        public float minRotationSpeed;
        public float maxRotationSpeed;
        public float minNeighbourDistance;

        public int minSchoolSize;
        public int maxSchoolSize;
        public FishParameters(FishType _type, float _panicTimerLength, int _totalToResearch, StationType[] _researchProtocol,
                                float _minSpeed = 1f, float _maxSpeed = 3f,
                                float _minRotationSpeed = 1.0f, float _maxRotationSpeed = 4.0f,
                                int _minSchoolSize = 5, int _maxSchoolSize = 15,
                                float _minNeighbourDistance = 30.0f)
        {
            type = _type;
            panicTimerLength = _panicTimerLength;

            ResearchProtocols = _researchProtocol;
            currentResearchProtocol = totalResearched = 0;
            totalToResearch = _totalToResearch;

            minSpeed = _minSpeed;
            maxSpeed = _maxSpeed;
            minRotationSpeed = _minRotationSpeed;
            maxRotationSpeed = _maxRotationSpeed;
            minNeighbourDistance = _minNeighbourDistance;
            minSchoolSize = _minSchoolSize;
            maxSchoolSize = _maxSchoolSize;
        }

        // Getters
        public string Name
        {
            get { return type.ToString(); }
        }

        public int TotalNumberOfStations
        {
            get
            {
                if (ResearchProtocols == null)
                {
                    return -1;
                }
                else
                {
                    return ResearchProtocols.Length;
                }
            }
        }
    };

    public struct ResearchStationParameters
    {
        public GameData.StationType researchStation;
        public string Name
        {
            get
            {
                return researchStation.ToString();
            }
        }

        public ResearchStationParameters(GameData.StationType _researchStation)
        {
            researchStation = _researchStation;
        }
    };
    #endregion

    public enum ControlType
    {
        CHARACTER,
        SUBMARINE,
        STATION,
    };

    public const int TOTAL_NUMBER_OF_FISHTYPES = 2;
    public enum FishType
    {
        ClownFish = 0,
        PufferFish
    };

    public enum StationType
    {
        None = -1,
        Massage,
        Clean
    };

    public const float PAYMENT_INTERVAL = 60f;
    public const int PAYMENT_AMOUNT = 100;

    // Fish management
    private static FishParameters[] AllFishParameters = // Contains details on all variants of fishes
    {
        new FishParameters(FishType.ClownFish, 40, 1, new StationType[] {
            StationType.Clean, StationType.Massage
        }),
        new FishParameters(FishType.PufferFish, 50, 1, new StationType[] {
            StationType.Massage, StationType.Clean
        }),
    };

    // Research Station management
    private static ResearchStationParameters[] AllResearchStationParameters = // Contains details on all variants of research stations
    {
        new ResearchStationParameters(StationType.Massage),
        new ResearchStationParameters(StationType.Clean)
    };

    #region Getter and setters

    public static FishParameters GetFishParameters(FishType fish)
    {
        return AllFishParameters[(int)fish];
    }

    public static ResearchStationParameters GetResearchStationParameters(StationType station)
    {
        return AllResearchStationParameters[(int)station];
    }

    public static void AddResearchedFish (FishType fish)
    {

        if (AllFishParameters[(int)fish].totalResearched < AllFishParameters[(int)fish].totalToResearch)
        {
            AllFishParameters[(int)fish].totalResearched++;
        }
    }
    #endregion
}
