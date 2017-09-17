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
    void Start () {
        AllFishParameters = new FishParameters[2];
        AllFishParameters[0] = new FishParameters( "Clownfish", 40 );
        AllFishParameters[0].researchProtocols = new ResearchProtocol[2];
        AllFishParameters[0].researchProtocols[0] = new ResearchProtocol("A");
        AllFishParameters[0].researchProtocols[1] = new ResearchProtocol("B");
        AllFishParameters[1] = new FishParameters( "Pufferfish", 50 );
        AllFishParameters[1].researchProtocols = new ResearchProtocol[2];
        AllFishParameters[1].researchProtocols[0] = new ResearchProtocol("B");
        AllFishParameters[1].researchProtocols[1] = new ResearchProtocol("A");
    }
	
	// Update is called once per frame
	void Update () {
    }
}
