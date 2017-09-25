using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This shit is in a script for now but can be moved to the actual GameController script or something.
//It's just stuff to keep track of the fish details like name and research protocols lol.
public class GameLogicController : MonoBehaviour {

    public struct ResearchProtocol
    {
        public string researchStation;

        public ResearchProtocol ( string _researchStation )
        {
            researchStation = _researchStation;
        }
    };

    public enum FishType
    {
        ClownFish = 0,
        PufferFish
    }

    //Fish details.
    public class FishParameters
    {
        public string name;
        public ResearchProtocol[] researchProtocols;
        public float panicTimerLength;
        public int currentResearchProtocol;

        public FishParameters ( string _name, float _panicTimerLength )
        {
            name = _name;
            panicTimerLength = _panicTimerLength;
            currentResearchProtocol = 0;
        }
    };

    public static FishParameters [] AllFishParameters;

    // Use this for initialization
    void Awake () {
        AllFishParameters = new FishParameters[2];
        AllFishParameters[(int)FishType.ClownFish] = new FishParameters( "Clownfish", 40 );
        AllFishParameters[(int)FishType.ClownFish].researchProtocols = new ResearchProtocol[2];
        AllFishParameters[(int)FishType.ClownFish].researchProtocols[0] = new ResearchProtocol("A");
        AllFishParameters[(int)FishType.ClownFish].researchProtocols[1] = new ResearchProtocol("B");
        AllFishParameters[(int)FishType.PufferFish] = new FishParameters( "Pufferfish", 50 );
        AllFishParameters[(int)FishType.PufferFish].researchProtocols = new ResearchProtocol[2];
        AllFishParameters[(int)FishType.PufferFish].researchProtocols[0] = new ResearchProtocol("B");
        AllFishParameters[(int)FishType.PufferFish].researchProtocols[1] = new ResearchProtocol("A");
    }
	
	// Update is called once per frame
	void Update () {
    }
}
