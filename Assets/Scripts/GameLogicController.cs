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
        public int totalResearched;
        public int totalToResearch;

        public float minSpeed;
        public float maxSpeed;
        public float minRotationSpeed;
        public float maxRotationSpeed;
        public float minNeighbourDistance;

        public int minSchoolSize;
        public int maxSchoolSize;

        public FishParameters(string _name, float _panicTimerLength, int _totalToResearch,
                                float _minSpeed = 1f, float _maxSpeed = 3f,
                                float _minRotationSpeed = 1.0f, float _maxRotationSpeed = 4.0f,
                                int _minSchoolSize = 5, int _maxSchoolSize = 15,
                                float _minNeighbourDistance = 30.0f )
        {
            name = _name;
            panicTimerLength = _panicTimerLength;
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
    };

    public static FishParameters [] AllFishParameters;

    // Use this for initialization
    void Awake () {
        AllFishParameters = new FishParameters[2];
        AllFishParameters[0] = new FishParameters( "Clownfish", 40, 1 );
        AllFishParameters[0].researchProtocols = new ResearchProtocol[2];
        AllFishParameters[0].researchProtocols[0] = new ResearchProtocol("A");
        AllFishParameters[0].researchProtocols[1] = new ResearchProtocol("B");
        AllFishParameters[1] = new FishParameters( "Pufferfish", 50, 1 );
        AllFishParameters[1].researchProtocols = new ResearchProtocol[2];
        AllFishParameters[1].researchProtocols[0] = new ResearchProtocol("B");
        AllFishParameters[1].researchProtocols[1] = new ResearchProtocol("A");
    }
	
	// Update is called once per frame
	void Update () {
    }
}
