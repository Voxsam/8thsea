using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicController : MonoBehaviour {

    public class ResearchProtocol
    {
        public string researchStation;
        public bool completed;

        public ResearchProtocol ( string _researchStation )
        {
            researchStation = _researchStation;
            completed = false;
        }
    };

    //Fish details.
    public class FishParameters
    {
        public string name;
        public ResearchProtocol[] researchProtocols;
        public int currentResearchProtocol;

        public FishParameters ( string _name )
        {
            name = _name;
            currentResearchProtocol = 0;
        }
    };

    public static FishParameters [] AllFishParameters;

    // Use this for initialization
    void Start () {
        AllFishParameters = new FishParameters[1];
        AllFishParameters[0] = new FishParameters( "Clownfish" );
        AllFishParameters[0].researchProtocols = new ResearchProtocol[1];
        AllFishParameters[0].researchProtocols[0] = new ResearchProtocol("Extraction Station");
	}
	
	// Update is called once per frame
	void Update () {
    }
}
