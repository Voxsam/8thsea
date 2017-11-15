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
    
    public const float PAYMENT_INTERVAL = 60f;
    public const int STARTING_MONEY = 30000;
    public const int MONEY_DEPLETE_RATE = 100;

    #region Structures
    //Fish details.
    public class FishParameters
    {
        private FishType type;
        public StationType[] ResearchProtocols
        {// The order of Stations to follow
            get; private set;
        }

		public string printableName;

        public float panicTimerLength;
        public float researchPanicRate;
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
		public FishParameters(FishType _type, string _printableName, float _panicTimerLength, int _totalToResearch, StationType[] _researchProtocol,
                                float _researchPanicRate = 2f,
                                float _minSpeed = 1f, float _maxSpeed = 3f,
                                float _minRotationSpeed = 1.0f, float _maxRotationSpeed = 4.0f,
                                int _minSchoolSize = 5, int _maxSchoolSize = 15,
                                float _minNeighbourDistance = 30.0f)
        {
            type = _type;
            panicTimerLength = _panicTimerLength;
            researchPanicRate = _researchPanicRate;

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
			printableName = _printableName;
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
        Penguin,
        Octi,
        Shark,
        Seahorse,
        // Tutorial fishes should not be included in the normal spawning
		TutorialClownFish
    };
    public static int TOTAL_NUMBER_OF_FISHTYPES {
        get { return AllFishParameters.Length - 1; } // Exclude TutorialClownFish
    }

    public static string GetFishName(FishType fish)
    {
		return GetFishParameters (fish).printableName;
    }

    public enum StationType
    {
        None = -1,
        Research,
        Sample,
        Photograph
    };

    #region Unity objects
    [SerializeField] private Transform DefaultEmptyFishPrefab;

    // Specific meshes
    [SerializeField] private Transform ClownFishPrefab;
    [SerializeField] private Transform PufferFishPrefab;
    [SerializeField] private Transform UnicornFishPrefab;
    [SerializeField] private Transform WhalePrefab;
    [SerializeField] private Transform FlounderPrefab;
    [SerializeField] private Transform PenguinPrefab;
    [SerializeField] private Transform OctiPrefab;
    [SerializeField] private Transform SharkPrefab;
    [SerializeField] private Transform SeahorsePrefab;

    [SerializeField] private Transform ClownFishSelectablePrefab;
    [SerializeField] private Transform PufferFishSelectablePrefab;
    [SerializeField] private Transform UnicornFishSelectablePrefab;
    [SerializeField] private Transform WhaleSelectablePrefab;
    [SerializeField] private Transform FlounderSelectablePrefab;
    [SerializeField] private Transform PenguinSelectablePrefab;
    [SerializeField] private Transform OctiSelectablePrefab;
    [SerializeField] private Transform SharkSelectablePrefab;
    [SerializeField] private Transform SeahorseSelectablePrefab;
    #endregion

    // Fish management
    private static FishParameters[] AllFishParameters = // Contains details on all variants of fishes
    {
        new FishParameters(FishType.ClownFish, "Clownfish", 40, 1, new StationType[] {
            StationType.Sample, StationType.Photograph, StationType.Research
        }, 2.5f),
		new FishParameters(FishType.PufferFish, "Pufferfish", 50, 1, new StationType[] {
            StationType.Research, StationType.Sample
        }, 3.5f),
		new FishParameters(FishType.UnicornFish, "Unicorn Fish", 45, 1, new StationType[] {
            StationType.Research, StationType.Photograph
        }, 2.5f, 1, 3, 1, 4, 5, 10, 30),
		new FishParameters(FishType.Whale, "Small Whale", 60, 1, new StationType[] {
            StationType.Research, StationType.Sample, StationType.Research
        }, 3.5f, 1, 4, 1, 4, 1, 3, 50),
		new FishParameters(FishType.Flounder, "Flounder", 45, 1, new StationType[] {
            StationType.Photograph, StationType.Sample, StationType.Research
        }, 2),
		new FishParameters(FishType.Penguin, "Angry Penguin", 45, 1, new StationType[] {
            StationType.Research, StationType.Photograph, StationType.Sample
        }, 3, 1, 3, 1, 4, 3, 6, 30),
		new FishParameters(FishType.Octi, "Octopus", 45, 1, new StationType[] {
            StationType.Photograph, StationType.Sample
        }, 3, 1, 3, 1, 4, 3, 2, 30),
		new FishParameters(FishType.Shark, "Shark", 40, 1, new StationType[] {
            StationType.Photograph, StationType.Research, StationType.Sample
		}, 2.5f, 1, 4, 1, 4, 3, 5, 30),
		new FishParameters(FishType.Seahorse, "Seahorse", 40, 1, new StationType[] {
            StationType.Research, StationType.Sample, StationType.Photograph
		}, 3f, 1, 4, 1, 4, 3, 10, 30),
		new FishParameters(FishType.TutorialClownFish, "Clownfish", 100, 1, new StationType[] {
			StationType.Sample, StationType.Photograph, StationType.Research
		}, 2f, 1, 2, 1, 2, 10, 20, 30)
    };

    // Research Station management
    private static ResearchStationParameters[] AllResearchStationParameters = // Contains details on all variants of research stations
    {
        new ResearchStationParameters(StationType.Research),
        new ResearchStationParameters(StationType.Sample),
        new ResearchStationParameters(StationType.Photograph)
    };

    // Level management
    [SerializeField] private static FishType[][] levels =
    {
        new FishType[] {FishType.TutorialClownFish},
        new FishType[] { FishType.ClownFish, FishType.PufferFish, FishType.Flounder },
        new FishType[] { FishType.Whale, FishType.Shark, FishType.Octi, },
        new FishType[] { FishType.Penguin, FishType.UnicornFish, FishType.Seahorse},
    };
    public static int TOTAL_NUMBER_OF_LEVELS
    {
        get { return levels.Length; }
    }

    /// <summary>
    /// Gets the fish types required for the level in ResearchRequirement type. Level is expected to start from 1, not 0.
    /// </summary>
    public static FishType[] GetResearchRequirementsForLevel(int level)
    {
        if (level < 0 || level > TOTAL_NUMBER_OF_LEVELS)
        {
            return null;
        }

        return levels[level];
    }

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
		if (GameController.Obj.isTutorial && fish == FishType.TutorialClownFish) {
			if (TutorialManager.Obj.currentStep == 10) {
				TutorialManager.Obj.hasCompletedFish = true;
			}
		}

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
                case FishType.Penguin:
                    prefab = Obj.PenguinPrefab;
                    break;
                case FishType.Octi:
                    prefab = Obj.OctiPrefab;
                    break;
                case FishType.Shark:
                    prefab = Obj.SharkPrefab;
                    break;
				case FishType.TutorialClownFish:
					prefab = Obj.ClownFishPrefab;
					break;
                case FishType.Seahorse:
                    prefab = Obj.SeahorsePrefab;
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

    public static Transform GetSelectableFish(FishType fish)
    {
        Transform prefab;
        switch (fish)
        {
            case FishType.ClownFish:
                prefab = Obj.ClownFishSelectablePrefab;
                break;
			case FishType.TutorialClownFish:
				prefab = Obj.ClownFishSelectablePrefab;
				break;
            case FishType.UnicornFish:
                prefab = Obj.UnicornFishSelectablePrefab;
                break;
            case FishType.Whale:
                prefab = Obj.WhaleSelectablePrefab;
                break;
            case FishType.Flounder:
                prefab = Obj.FlounderSelectablePrefab;
                break;
            case FishType.Penguin:
                prefab = Obj.PenguinSelectablePrefab;
                break;
            case FishType.Octi:
                prefab = Obj.OctiSelectablePrefab;
                break;
            case FishType.Shark:
                prefab = Obj.SharkSelectablePrefab;
                break;
            case FishType.PufferFish:
            default:
                prefab = Obj.PufferFishSelectablePrefab;
                break;
        }

        return prefab;
    }
    #endregion
}