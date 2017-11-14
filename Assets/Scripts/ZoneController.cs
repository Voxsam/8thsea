using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{

    //X
    [SerializeField] public int zoneX;
    //Y
    [SerializeField] public int zoneY;
    //Z
    [SerializeField] public int zoneZ;

    [SerializeField] public Transform Sea;

    [SerializeField] public int maxSchools = 10;
    [SerializeField] public int minSchools = 20;
    [SerializeField] public float fishSchoolSpawnDelay = 5f;

    [SerializeField] public GameObject zoneCollider;

	[Tooltip("Which fish types to spawn, look up numbers in GameData.")]
	public GameData.FishType[] fishTypesToSpawn; 

    [SerializeField] public Transform SpawnPoint;

    [SerializeField] public GameObject fishSchoolTemplate;

    private GameObject zoneBoundaryXPos;
    private GameObject zoneBoundaryXNeg;
    private GameObject zoneBoundaryYPos;
    private GameObject zoneBoundaryYNeg;
    private GameObject zoneBoundaryZPos;
    private GameObject zoneBoundaryZNeg;

    private int numSchools;

    private Dictionary<GameData.FishType, List<GameObject>> fishSchools;

    private const int MINIMUM_SCHOOL_SIZE = 3;
    private const int MAXIMUM_SCHOOL_SIZE = 5;

    private bool finishSpawning = false;
    private int schoolsDone = 0;
    public float percentageSpawned
    {
        get { return (finishSpawning) ? 1f : schoolsDone / (1f * numSchools * MAXIMUM_SCHOOL_SIZE); }
    }

    // Use this for initialization
    void Start()
    {
        /*
        zoneBoundaryXPos = gameObject.transform.Find("ZoneColliderXPos").gameObject;
        zoneBoundaryXNeg = gameObject.transform.Find("ZoneColliderXNeg").gameObject;
        zoneBoundaryYPos = gameObject.transform.Find("ZoneColliderYPos").gameObject;
        zoneBoundaryYNeg = gameObject.transform.Find("ZoneColliderYNeg").gameObject;
        zoneBoundaryZPos = gameObject.transform.Find("ZoneColliderZPos").gameObject;
        zoneBoundaryZNeg = gameObject.transform.Find("ZoneColliderZNeg").gameObject;

        zoneBoundaryXPos.transform.localScale = new Vector3(1, zoneHeight * 2, zoneLength * 2);
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
        */
        zoneCollider.transform.localScale = new Vector3(zoneX * 2, zoneY * 2, zoneZ * 2);
        zoneCollider.name = gameObject.ToString();

        // Set Zone as a child of Sea
        this.transform.SetParent(Sea);

        fishSchools = new Dictionary<GameData.FishType, List<GameObject>>();
        for (int i = 0; i < GameData.TOTAL_NUMBER_OF_FISHTYPES; i++)
        {
            GameData.FishType fishType = (GameData.FishType)i;
            fishSchools.Add(fishType, new List<GameObject>());
        }

        numSchools = Random.Range(minSchools, maxSchools);
        StartCoroutine(SpawnSchools());
    }

    protected IEnumerator SpawnSchools()
    {
		
        while (schoolsDone < numSchools)

        {
            int rand = Random.Range(MINIMUM_SCHOOL_SIZE, MAXIMUM_SCHOOL_SIZE);
            for (int i = 0; i < rand; i++)
            {
                if (schoolsDone >= numSchools)
                {
                    break;
                }

				GameData.FishType fishTypeToSpawn;
				if (fishTypesToSpawn.Length > 0) {
					fishTypeToSpawn = fishTypesToSpawn [Random.Range (0, fishTypesToSpawn.Length)];
				} else {
					fishTypeToSpawn = (GameData.FishType) Random.Range (0, GameData.TOTAL_NUMBER_OF_FISHTYPES);
				}

				GameData.FishParameters fishParameters = GameData.GetFishParameters(fishTypeToSpawn);

                GameObject newFishSchool = (GameObject)Instantiate(fishSchoolTemplate, SpawnPoint);
                newFishSchool.transform.position = transform.position;
                FishSchoolController fishSchoolController = newFishSchool.GetComponent<FishSchoolController>();
                fishSchoolController.zoneX = zoneX;
                fishSchoolController.zoneY = zoneY;
                fishSchoolController.zoneZ = zoneZ;
                fishSchoolController.zoneName = zoneCollider.name;

                int schoolSize = Random.Range(fishParameters.minSchoolSize,
                                                fishParameters.maxSchoolSize);
                Vector3 schoolPos = gameObject.transform.position
                                    + new Vector3(Random.Range(-zoneX, zoneX),
                                                    Random.Range(-zoneY, zoneY),
                                                    Random.Range(-zoneZ, zoneZ));
                for (int j = 0; j < schoolSize; j++)
                {
					FishController newFish = GameData.CreateNewFish(fishTypeToSpawn, newFishSchool.transform);
                    FishMovementController fishMovementController = newFish.GetComponent<FishMovementController>();
                    fishMovementController.Initialise
                    (
                        fishParameters.minSpeed,
                        fishParameters.maxSpeed,
                        fishParameters.minRotationSpeed,
                        fishParameters.maxRotationSpeed,
                        fishParameters.minNeighbourDistance
                    );
                    fishMovementController.SetEnabled(true);
                    newFish.SetRigidbody(false);
                    newFish.gameObject.transform.position = schoolPos
                                                            + new Vector3(Random.Range(-5, 5),
                                                                          Random.Range(-5, 5),
                                                                          Random.Range(-5, 5));
                    fishSchoolController.AddFishToSchool(newFish.gameObject);
                }
				fishSchools[fishTypeToSpawn].Add(newFishSchool);
                schoolsDone++;
            }

            yield return new WaitForEndOfFrame();
        }

        finishSpawning = true;
        yield return null;
    }

    /*private void SpawnSchools()
    {
        for (int i = 0; i < numSchools; i++)
        {
            int fishTypeIndex = Random.Range(0, GameData.TOTAL_NUMBER_OF_FISHTYPES);
            GameData.FishParameters fishParameters = GameData.GetFishParameters((GameData.FishType)fishTypeIndex);

            GameObject newFishSchool = (GameObject)Instantiate(fishSchoolTemplate, SpawnPoint);
            newFishSchool.transform.position = transform.position;
            FishSchoolController fishSchoolController = newFishSchool.GetComponent<FishSchoolController>();
            fishSchoolController.zoneWidth = zoneWidth;
            fishSchoolController.zoneHeight = zoneHeight;
            fishSchoolController.zoneLength = zoneLength;

            int schoolSize = Random.Range(fishParameters.minSchoolSize,
                                           fishParameters.maxSchoolSize);
            for (int j = 0; j < schoolSize; j++)
            {
                FishController newFish = GameData.CreateNewFish((GameData.FishType)fishTypeIndex, newFishSchool.transform);
                FishMovementController fishMovementController = newFish.GetComponent<FishMovementController>();
                fishMovementController.Initialise
                (
                    fishParameters.minSpeed,
                    fishParameters.maxSpeed,
                    fishParameters.minRotationSpeed,
                    fishParameters.maxRotationSpeed,
                    fishParameters.minNeighbourDistance
                );
                fishMovementController.SetEnabled(true);
                newFish.SetRigidbody(false);
                fishSchoolController.AddFishToSchool(newFish.gameObject);
            }

            fishSchools[(GameData.FishType)fishTypeIndex].Add(newFishSchool);
        }
    }*/

    // Update is called once per frame
    void Update()
    {

    }
}
