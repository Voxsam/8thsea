using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AquariumStationController : StationControllerInterface
{
    public override GameData.ControlType ControlMode
    {
        get { return GameData.ControlType.STATION; }
    }

    public bool isTutorial = false;

    [SerializeField]
    public GameObject fishSchool;
    private Dictionary<GameData.FishType, FishSchoolController> currentLevelSchools;
    private List<FishSchoolController> previousSchools;

    [SerializeField] public GameData.FishType[] researchRequirementsForLevel;

    [SerializeField] private GameObject holdSlot;
    [SerializeField] private GameObject playerAnchor;
    [SerializeField] private GameObject worldspaceCanvas;
    [SerializeField] private GameObject warningText;
    [SerializeField] private GameObject researchReqAnchor;
    private float warningTextLifespan = 0;
    private float selectInterval = 0;

    // Camera things
    public const float AQUARIUM_CAMERA_FIELD_OF_VIEW = 60f;
    protected float cameraOriginalFov;

    public Vector3 AQUARIUM_CAMERA_DISTANCE_FROM_TARGET = new Vector3(0, 0, 0f);
    protected Vector3 cameraOriginalDistance;

    private Dictionary<GameData.FishType, GameObject> fishResearchRequirements;
    private int selectedResearchRequirementIndex = -1;

    private void OnDestroy()
    {
        //Save the fish in the aquarium on destroy for persistence.
        for (int i = 0; i < researchRequirementsForLevel.Length; i++)
        {
            if (currentLevelSchools[researchRequirementsForLevel[i]].FishInSchool.Count > 0)
                PersistentData.Obj.AddSchool(currentLevelSchools[researchRequirementsForLevel[i]]);
        }
    }

    // Use this for initialization
    void Start()
    {
        warningText.SetActive(false);

        previousSchools = new List<FishSchoolController>();
        for (int i = 0; i < PersistentData.Obj.savedSchools.Count; i++)
        {
            PersistentData.AquariumSchoolData persistentData = PersistentData.Obj.savedSchools[i];
            GameData.FishType fishTypeToSpawn = persistentData.fishType;
            GameData.FishParameters fishParameters = GameData.GetFishParameters(fishTypeToSpawn);

            GameObject newFishSchool = (GameObject)Instantiate(fishSchool);
            newFishSchool.transform.position = holdSlot.transform.position;
            newFishSchool.transform.SetParent(gameObject.transform);
            FishSchoolController fishSchoolController = newFishSchool.GetComponent<FishSchoolController>();
            fishSchoolController.zoneX = 20;
            fishSchoolController.zoneY = 10;
            fishSchoolController.zoneZ = 5;

            for (int j = 0; j < persistentData.numberOfFishes; j++)
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
                newFish.gameObject.transform.position = holdSlot.transform.position
                                                        + new Vector3(Random.Range(-2, 2),
                                                                        Random.Range(-2, 2),
                                                                        Random.Range(-2, 2));
                fishSchoolController.AddFishToSchool(newFish.gameObject);
            }
            previousSchools.Add(fishSchoolController);
        }

        currentLevelSchools = new Dictionary<GameData.FishType, FishSchoolController>();
        fishResearchRequirements = new Dictionary<GameData.FishType, GameObject>();
        if (isTutorial)
        {
            researchRequirementsForLevel = GameData.GetResearchRequirementsForLevel(0);
        }
        else
        {
            researchRequirementsForLevel = GameData.GetResearchRequirementsForLevel(GameController.Obj.CurrentLevel);
        }

        for (int i = 0; i < researchRequirementsForLevel.Length; i++)
        {
            GameObject newFishSchool = (GameObject)Instantiate(fishSchool);
            newFishSchool.transform.position = holdSlot.transform.position;
            newFishSchool.transform.SetParent(GameController.Obj.AquariumHolder);
            FishSchoolController fishSchoolController = newFishSchool.GetComponent<FishSchoolController>();
            fishSchoolController.zoneX = 10;
            fishSchoolController.zoneY = 5;
            fishSchoolController.zoneZ = 10;
            currentLevelSchools.Add(researchRequirementsForLevel[i], fishSchoolController);

            Transform newFishResearchRequirements = Instantiate(GameData.GetSelectableFish(researchRequirementsForLevel[i]));
            newFishResearchRequirements.SetParent(researchReqAnchor.transform, false);
            newFishResearchRequirements.Translate(0 + (i * 5), 0, 0);
            newFishResearchRequirements.GetComponent<ResearchRequirementsController>().Init(researchRequirementsForLevel[i]);
            fishResearchRequirements.Add(researchRequirementsForLevel[i], newFishResearchRequirements.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (warningText.activeSelf)
        {
            if (warningTextLifespan < 3)
            {
                warningTextLifespan += Time.deltaTime;
            }
            else
            {
                warningTextLifespan = 0;
                warningText.SetActive(false);
            }
        }

        if (this.IsActivated && playerInStation != null)
        {
            if (selectInterval > 0.2f)
            {
                if (playerInStation.controls.GetHorizontalAxis() > 0)
                {
                    selectInterval = 0;
					
                    if (selectedResearchRequirementIndex == -1)
                    {
                        selectedResearchRequirementIndex = 0;
                        fishResearchRequirements[researchRequirementsForLevel[selectedResearchRequirementIndex]].GetComponent<ResearchRequirementsController>().Select();
                    }
                    else if (selectedResearchRequirementIndex < (researchRequirementsForLevel.Length - 1))
                    {
                        GameData.FishType fishType = researchRequirementsForLevel[selectedResearchRequirementIndex];
                        fishResearchRequirements[fishType].GetComponent<ResearchRequirementsController>().Deselect();
                        selectedResearchRequirementIndex++;
                        fishType = researchRequirementsForLevel[selectedResearchRequirementIndex];
                        fishResearchRequirements[fishType].GetComponent<ResearchRequirementsController>().Select();
                    }
                }
                else if (playerInStation.controls.GetHorizontalAxis() < 0)
                {
                    selectInterval = 0;
                    if (selectedResearchRequirementIndex == -1)
                    {
                        selectedResearchRequirementIndex = 0;
                        fishResearchRequirements[researchRequirementsForLevel[selectedResearchRequirementIndex]].GetComponent<ResearchRequirementsController>().Select();
                    }
                    else if (selectedResearchRequirementIndex > 0)
                    {
                        GameData.FishType fishType = researchRequirementsForLevel[selectedResearchRequirementIndex];
                        fishResearchRequirements[fishType].GetComponent<ResearchRequirementsController>().Deselect();
                        selectedResearchRequirementIndex--;
                        fishType = researchRequirementsForLevel[selectedResearchRequirementIndex];
                        fishResearchRequirements[fishType].GetComponent<ResearchRequirementsController>().Select();
                    }
                }
            }
            else
            {
                selectInterval += Time.deltaTime;
            }

            if (playerInStation.controls.GetCancelKeyDown())
            {
                PlayerController playerControllerScript = this.playerInStation.GetComponent<PlayerController>();
                if (playerControllerScript != null)
                {
                    DisengagePlayer();
                }
            }
            else if (playerInStation.controls.GetActionKeyDown())
            {
                PlayerInteractionController playerControllerScript = this.playerInStation.GetComponent<PlayerInteractionController>();
                if (playerControllerScript != null)
                {
                    if (selectedResearchRequirementIndex >= 0 && selectedResearchRequirementIndex < (researchRequirementsForLevel.Length - 1))
                    {
                        if (RemoveFish(playerControllerScript, researchRequirementsForLevel[selectedResearchRequirementIndex]))
                        {
                            DisengagePlayer();
                        }
                        else
                        {
                            ShowWarningText("No fish of this type is in the aquarium!");
                        }
                    }
                }
            }
        }
    }

    #region Inherited from IInteractables interface
    override public void Interact()
    {
    }

    override public void Interact(GameObject otherActor)
    {
        if (otherActor.tag == "Player")
        {
            //Make sure the station is not currently controlled by another player.
            if (!this.IsActivated)
            {

				if (GameController.Obj.isTutorial) {
					TutorialManager.Obj.hasActivatedAquarium = true;
				}

                PlayerInteractionController playerInteractionControllerScript = otherActor.GetComponent<PlayerInteractionController>();
                if (playerInteractionControllerScript != null)
                {
                    //Get the object held by the player.
                    GameObject objectToHold = playerInteractionControllerScript.GetHeldObject();
                    //Attempt to deposit the fish into the aquarium if the player is holding one.
                    if (objectToHold != null)
                    {
                        FishController heldObjectControllerScript = objectToHold.GetComponent<FishController>();
                        if (heldObjectControllerScript != null)
                        {
                            if (heldObjectControllerScript.IsDead())
                            {
                                ShowWarningText("This fish is dead!");
                            }
                            else if (!IsFishTypeStoreable(heldObjectControllerScript.fishType))
                            {
                                ShowWarningText("This fish is not part of the list!");
                            }
                            else
                            {
                                //Only allow the player to put a fish into the aquarium if they have researched the required amount for the species.
                                if (GameData.GetFishParameters(heldObjectControllerScript.fishType).totalResearched == GameData.GetFishParameters(heldObjectControllerScript.fishType).totalToResearch)
                                {
                                    playerInteractionControllerScript.DropObject();
                                    StoreFish(objectToHold, heldObjectControllerScript.fishType);
                                }
                                else
                                {
                                    ShowWarningText("Please complete the required number of researches for this species first!");
                                }
                            }
                        }
                    }
                    //Man the aquarium station otherwise.
                    else
                    {
                        EngagePlayer(otherActor);
                    }
                }
            }
        }
    }

    override public void ToggleHighlight(PlayerController otherPlayerController, bool toggle = true)
    {
    }
    #endregion
    public override void SetPlayerToStation(PlayerController player)
    {
        base.SetPlayerToStation(player);
        stationCamera.SetCameraToObject(playerAnchor);
    }

    public override void WhenActivated()
    {
        selectedResearchRequirementIndex = -1;

        cameraOriginalFov = stationCamera.GetCamera.fieldOfView;
        stationCamera.GetCamera.fieldOfView = AQUARIUM_CAMERA_FIELD_OF_VIEW;

        cameraOriginalDistance = stationCamera.CameraOffset;
        stationCamera.CameraOffset = AQUARIUM_CAMERA_DISTANCE_FROM_TARGET;
    }

    public override void WhenDeactivated()
    {
        foreach (KeyValuePair<GameData.FishType, GameObject> researchRequirement in fishResearchRequirements)
        {
            researchRequirement.Value.GetComponent<ResearchRequirementsController>().Deselect();
        }
        stationCamera.GetCamera.fieldOfView = cameraOriginalFov;
        stationCamera.CameraOffset = cameraOriginalDistance;

    }

    public override bool SwitchCondition()
    {
        return true;
    }

    #region Main Functions
    private void ShowWarningText(string text)
    {
        warningText.GetComponent<Text>().text = text;
        warningText.SetActive(true);
        warningTextLifespan = 0;
    }

    private bool EngagePlayer(GameObject player)
    {
        if (this.playerInStation == null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (player != null)
            {
                // If the player is in Character control mode
                if (playerController.ControlMode == GameData.ControlType.CHARACTER)
                {
                    this.SetPlayerToStation(playerController);
                    this.WhenActivated();
                    return true;
                }
            }
        }
        return false;
    }

    private bool DisengagePlayer()
    {
        PlayerController playerControllerScript = this.playerInStation.GetComponent<PlayerController>();
        if (playerControllerScript != null)
        {
            this.IsActivated = false;
            this.WhenDeactivated();
            this.ReleasePlayerFromStation();
            return true;
        }
        return false;
    }

    private bool IsFishTypeStoreable (GameData.FishType fishType)
    {
        for (int i = 0; i < researchRequirementsForLevel.Length; i++)
        {
            if (fishType == researchRequirementsForLevel[i])
            {
                return true;
            }
        }
        return false;
    }

    public void StoreFish(GameObject fishToStore, GameData.FishType fishType)
    {
        if (fishToStore != null)
        {
            FishSchoolController schoolController = currentLevelSchools[fishType];
            if (schoolController != null)
            {
                FishController fishController = fishToStore.GetComponent<FishController>();
                if (fishController != null)
                {
                    fishController.StartSlowPanic();
                    fishController.SetEnabled(false);
                    FishMovementController fishMovementController = fishToStore.GetComponent<FishMovementController>();
                    if (fishMovementController != null)
                    {
                        fishMovementController.SetEnabled(true);
                        fishMovementController.Initialise();
                        schoolController.AddFishToSchool(fishToStore);
                        fishToStore.transform.position = holdSlot.transform.position;
                    }

                    // I don't know why I must do this, but I must delay setting the parent else it will force set itself to no parent
                    StartCoroutine(GameController.ActivateCallbackAfterDelayCoroutine(1f, () =>
                    {
                        fishController.transform.SetParent(schoolController.transform);
                    }));
                }

            }
        }
    }

    public bool RemoveFish(PlayerInteractionController playerControllerScript, GameData.FishType fishType)
    {
        if (currentLevelSchools.ContainsKey(fishType))
        {
            FishSchoolController schoolController = currentLevelSchools[fishType];
            if (schoolController != null)
            {
                if (schoolController.FishInSchool.Count > 0)
                {
                    GameObject fishToRemove = schoolController.RemoveFishFromSchool(0);
                    FishMovementController fishMovementController = fishToRemove.GetComponent<FishMovementController>();
                    if (fishMovementController != null)
                    {
                        fishMovementController.SetEnabled(false);
                        FishController fishController = fishToRemove.GetComponent<FishController>();
                        if (fishController != null)
                        {
                            fishController.SetEnabled(true);
                            playerControllerScript.PickUpObject(fishToRemove);
                            
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    #endregion
}
