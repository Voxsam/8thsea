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
        public float panicTimerLength
        {
            get; private set;
        }

        public FishParameters(FishType _type, float _panicTimerLength, StationType[] _researchProtocol)
        {
            type = _type;
            panicTimerLength = _panicTimerLength;
            ResearchProtocols = _researchProtocol;
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
    #endregion

    public enum ControlType
    {
        CHARACTER,
        SUBMARINE,
        STATION,
    };

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
        new FishParameters(FishType.ClownFish, 40, new StationType[] {
            StationType.Clean, StationType.Massage
        }),
        new FishParameters(FishType.PufferFish, 50, new StationType[] {
            StationType.Massage, StationType.Clean
        }),
    };

    #region Getter and setters

    public static FishParameters GetFishParameter(FishType fish)
    {
        return AllFishParameters[(int)fish];
    }
    #endregion
}
