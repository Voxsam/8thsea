using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour {
    //X
    [SerializeField]
    public int zoneWidth;
    //Y
    [SerializeField]
    public int zoneHeight;
    //Z
    [SerializeField]
    public int zoneLength;

    [SerializeField]
    public int[] fishTypes;

    [SerializeField]
    public int maxSchools;
    [SerializeField]
    public int minSchools;

    [SerializeField]
    public GameObject fishSchoolTemplate;
    [SerializeField]
    public GameObject fishTemplate;

    private GameObject zoneBoundaryXPos;
    private GameObject zoneBoundaryXNeg;
    private GameObject zoneBoundaryYPos;
    private GameObject zoneBoundaryYNeg;
    private GameObject zoneBoundaryZPos;
    private GameObject zoneBoundaryZNeg;

    private int numSchools;

    private Dictionary<int, List<GameObject>> fishSchools;

    // Use this for initialization
    void Start () {
        zoneBoundaryXPos = gameObject.transform.Find("ZoneColliderXPos").gameObject;
        zoneBoundaryXNeg = gameObject.transform.Find("ZoneColliderXNeg").gameObject;
        zoneBoundaryYPos = gameObject.transform.Find("ZoneColliderYPos").gameObject;
        zoneBoundaryYNeg = gameObject.transform.Find("ZoneColliderYNeg").gameObject;
        zoneBoundaryZPos = gameObject.transform.Find("ZoneColliderZPos").gameObject;
        zoneBoundaryZNeg = gameObject.transform.Find("ZoneColliderZNeg").gameObject;

        zoneBoundaryXPos.transform.localScale = new Vector3 (1, zoneHeight * 2, zoneLength * 2);
        zoneBoundaryXNeg.transform.localScale = new Vector3(1, zoneHeight * 2, zoneLength * 2);
        zoneBoundaryYPos.transform.localScale = new Vector3(zoneWidth * 2, 1, zoneLength * 2);
        zoneBoundaryYNeg.transform.localScale = new Vector3(zoneWidth * 2, 1, zoneLength * 2);
        zoneBoundaryZPos.transform.localScale = new Vector3(zoneWidth * 2, zoneHeight * 2, 1);
        zoneBoundaryZNeg.transform.localScale = new Vector3(zoneWidth * 2, zoneHeight * 2, 1);

        zoneBoundaryXPos.transform.localPosition = new Vector3(zoneWidth, 0, 0);
        zoneBoundaryXNeg.transform.localPosition = new Vector3(-zoneWidth, 0, 0);
        zoneBoundaryYPos.transform.localPosition = new Vector3(0, zoneHeight, 0);
        zoneBoundaryYNeg.transform.localPosition = new Vector3(0, -zoneHeight, 0);
        zoneBoundaryZPos.transform.localPosition = new Vector3(0, 0, zoneLength);
        zoneBoundaryZNeg.transform.localPosition = new Vector3(0, 0, -zoneLength);


        fishSchools = new Dictionary<int, List<GameObject>>();
        foreach (int fishType in fishTypes)
        {
            fishSchools.Add(fishType, new List<GameObject>());
        }

        numSchools = Random.Range(minSchools, maxSchools);
        for (int i = 0; i < numSchools; i++)
        {
            int fishTypeIndex = Random.Range(0, fishTypes.Length);

            GameObject newFishSchool = (GameObject)Instantiate(fishSchoolTemplate);
            newFishSchool.transform.position = transform.position;
            FishSchoolController fishSchoolController = newFishSchool.GetComponent<FishSchoolController>();
            fishSchoolController.zoneWidth = zoneWidth;
            fishSchoolController.zoneHeight = zoneHeight;
            fishSchoolController.zoneLength = zoneLength;

            int schoolSize = Random.Range ( GameLogicController.AllFishParameters[fishTypes[fishTypeIndex]].minSchoolSize,
                                            GameLogicController.AllFishParameters[fishTypes[fishTypeIndex]].maxSchoolSize);
            for (int j = 0; j < schoolSize; j++)
            {
                GameObject newFish = (GameObject)Instantiate(fishTemplate);
                newFish.transform.position = transform.position;
                FishMovementController fishMovementController = newFish.GetComponent<FishMovementController>();
                fishMovementController.Initialise
                (
                    GameLogicController.AllFishParameters[fishTypes[fishTypeIndex]].minSpeed,
                    GameLogicController.AllFishParameters[fishTypes[fishTypeIndex]].maxSpeed,
                    GameLogicController.AllFishParameters[fishTypes[fishTypeIndex]].minRotationSpeed,
                    GameLogicController.AllFishParameters[fishTypes[fishTypeIndex]].maxRotationSpeed,
                    GameLogicController.AllFishParameters[fishTypes[fishTypeIndex]].minNeighbourDistance
                );
                fishMovementController.SetEnabled(true);
                FishController fishController = newFish.GetComponent<FishController>();
                fishController.fishType = fishTypes[fishTypeIndex];
                fishController.SetEnabled (false);
                fishSchoolController.AddFishToSchool(newFish);
            }


            fishSchools[fishTypes[fishTypeIndex]].Add(newFishSchool);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
