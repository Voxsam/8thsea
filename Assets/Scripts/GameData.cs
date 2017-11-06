using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    public static GameData Obj;

    void Awake()
    {
        if (Obj == null)
        {
            Obj = this;
        }
    }

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

    public enum FishType
    {
        None = -1, // Default value
        ClownFish = 0,
        PufferFish,
        UnicornFish,
        Whale,
        Flounder,
    };
    public static int TOTAL_NUMBER_OF_FISHTYPES {
        get { return AllFishParameters.Length; }
    }

    public enum StationType
    {
        None = -1,
        Research,
        Sample,
        Photograph
    };

    public const float PAYMENT_INTERVAL = 60f;
    public const int STARTING_MONEY = 50000;
    public const int MONEY_DEPLETE_RATE = 100;
    
    [SerializeField] private Transform DefaultEmptyFishPrefab;
    [SerializeField] private Transform ClownFishPrefab;
    [SerializeField] private Transform PufferFishPrefab;
    [SerializeField] private Transform UnicornFishPrefab;
    [SerializeField] private Transform WhalePrefab;
    [SerializeField] private Transform FlounderPrefab;


    // Fish management
    private static FishParameters[] AllFishParameters = // Contains details on all variants of fishes
    {
        new FishParameters(FishType.ClownFish, 40, 1, new StationType[] {
            StationType.Sample, StationType.Photograph, StationType.Research
        }),
        new FishParameters(FishType.PufferFish, 50, 1, new StationType[] {
            StationType.Research, StationType.Sample
        }),
        new FishParameters(FishType.UnicornFish, 45, 1, new StationType[] {
            StationType.Research, StationType.Photograph
        }, 1, 3, 1, 4, 5, 10, 30),
        new FishParameters(FishType.Whale, 60, 1, new StationType[] {
            StationType.Research, StationType.Sample, StationType.Research
        }, 1, 4, 1, 4, 1, 3, 50),
        new FishParameters(FishType.Flounder, 45, 1, new StationType[] {
            StationType.Photograph, StationType.Sample, StationType.Research
        })
    };

    // Research Station management
    private static ResearchStationParameters[] AllResearchStationParameters = // Contains details on all variants of research stations
    {
        new ResearchStationParameters(StationType.Research),
        new ResearchStationParameters(StationType.Sample),
        new ResearchStationParameters(StationType.Photograph)
    };

    //Easing functions.
    public static float QuadEaseInOut (float currentTime, float initialValue, float changeValue, float duration)
    {
        if ((currentTime /= duration / 2) < 1)
        {
            return changeValue / 2 * currentTime * currentTime + initialValue;
        }
	    return -changeValue / 2 * ((--currentTime) *(currentTime - 2) - 1) + initialValue;
    }

    public static float QuadEaseOut (float currentTime, float initialValue, float changeValue, float duration)
    {
        currentTime /= duration;
        return -changeValue * currentTime * (currentTime - 2) + initialValue;
    }

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

    /// <summary>
    /// Creates a new Fish with that FishType at location
    /// </summary>
    public static FishController CreateNewFish(FishType type, Vector3 locationToSpawn)
    {
        FishController fish = null;
        try
        {
            fish = Instantiate(Obj.DefaultEmptyFishPrefab, locationToSpawn, Quaternion.identity).GetComponent<FishController>();
        }
        catch
        {
            Debug.Log("Error instantiating fish");
            return null;
        }

        if (fish != null)
        {
            Transform prefab;

            switch(type)
            {
                case FishType.ClownFish:
                    prefab = Obj.ClownFishPrefab;
                    break;
                case FishType.UnicornFish:
                    prefab = Obj.UnicornFishPrefab;
                    break;
                case FishType.Whale:
                    prefab = Obj.WhalePrefab;
                    break;
                case FishType.Flounder:
                    prefab = Obj.FlounderPrefab;
                    break;
                case FishType.PufferFish:
                default:
                    prefab = Obj.PufferFishPrefab;
                    break;
            }

            try
            {
                Transform model = Instantiate(prefab, fish.transform);
                fish.Setup(type, model.GetComponentInChildren<SkinnedMeshRenderer>(), model.GetComponentInChildren<Animator>());
            }
            catch
            {
                Debug.Log("Error instantiating fish mesh");
            }
        }

        return fish;
    }

    /// <summary>
    /// Creates a new Fish at the transform location with the transform as parent
    /// </summary>
    /// <param name="type"></param>
    /// <param name="locationToSpawn"></param>
    /// <returns></returns>
    public static FishController CreateNewFish(FishType type, Transform parent)
    {
        FishController fish = CreateNewFish(type, parent.position);
        fish.transform.SetParent(parent);
        return fish;
    }
    #endregion
}