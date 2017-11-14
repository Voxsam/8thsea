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

    [SerializeField]
    public GameObject fishSchool;
    [System.Serializable]
    public struct ResearchRequirements
    {
        public GameData.FishType fishType;
        public GameObject templateObject;
    }
    [SerializeField]
    public ResearchRequirements[] researchRequirementTemplates;

    [SerializeField]
    private GameObject holdSlot;
    [SerializeField]
    private GameObject playerAnchor;
    [SerializeField]
    private GameObject worldspaceCanvas;
    [SerializeField]
    private GameObject warningText;
    [SerializeField]
    private GameObject researchReqAnchor;
    private float warningTextLifespan = 0;
    private float selectInterval = 0;

    // Camera things
    public const float AQUARIUM_CAMERA_FIELD_OF_VIEW = 60f;
    protected float cameraOriginalFov;

    public Vector3 AQUARIUM_CAMERA_DISTANCE_FROM_TARGET = new Vector3(0, 0, 0f);
    protected Vector3 cameraOriginalDistance;

    private Dictionary<GameData.FishType, GameObject> fishSchools;
    private Dictionary<GameData.FishType, GameObject> fishResearchRequirements;
    private int selectedResearchRequirementIndex = -1;

    // Use this for initialization
    void Start()
    {
        warningText.SetActive(false);

        fishSchools = new Dictionary<GameData.FishType, GameObject>();
        fishResearchRequirements = new Dictionary<GameData.FishType, GameObject>();

        for (int i = 0; i < researchRequirementTemplates.Length; i++)
        {
            GameObject newFishSchool = (GameObject)Instantiate(fishSchool);
            newFishSchool.transform.position = holdSlot.transform.position;
            FishSchoolController fishSchoolController = newFishSchool.GetComponent<FishSchoolController>();
            fishSchoolController.zoneX = 10;
            fishSchoolController.zoneY = 5;
            fishSchoolController.zoneZ = 10;
            fishSchools.Add(researchRequirementTemplates[i].fishType, newFishSchool);

            GameObject newFishResearchRequirements = (GameObject)Instantiate(researchRequirementTemplates[i].templateObject);
            newFishResearchRequirements.transform.SetParent(researchReqAnchor.transform, false);
            newFishResearchRequirements.transform.Translate(0 + (i * 5), 0, 0);
            newFishResearchRequirements.GetComponent<ResearchRequirementsController>().Init(researchRequirementTemplates[i].fishType);
            fishResearchRequirements.Add(researchRequirementTemplates[i].fishType, newFishResearchRequirements);
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
                        fishResearchRequirements[researchRequirementTemplates[selectedResearchRequirementIndex].fishType].GetComponent<ResearchRequirementsController>().Select();
                    }
                    else if ((int)selectedResearchRequirementIndex < (researchRequirementTemplates.Length - 1))
                    {
                        fishResearchRequirements[researchRequirementTemplates[selectedResearchRequirementIndex].fishType].GetComponent<ResearchRequirementsController>().Deselect();
                        selectedResearchRequirementIndex++;
                        fishResearchRequirements[researchRequirementTemplates[selectedResearchRequirementIndex].fishType].GetComponent<ResearchRequirementsController>().Select();
                    }
                }
                else if (playerInStation.controls.GetHorizontalAxis() < 0)
                {
                    selectInterval = 0;
                    if (selectedResearchRequirementIndex == -1)
                    {
                        selectedResearchRequirementIndex = 0;
                        fishResearchRequirements[researchRequirementTemplates[selectedResearchRequirementIndex].fishType].GetComponent<ResearchRequirementsController>().Select();
                    }
                    else if ((int)selectedResearchRequirementIndex > 0)
                    {
                        fishResearchRequirements[researchRequirementTemplates[selectedResearchRequirementIndex].fishType].GetComponent<ResearchRequirementsController>().Deselect();
                        selectedResearchRequirementIndex--;
                        fishResearchRequirements[researchRequirementTemplates[selectedResearchRequirementIndex].fishType].GetComponent<ResearchRequirementsController>().Select();
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
                    if (selectedResearchRequirementIndex >= 0 && selectedResearchRequirementIndex < (researchRequirementTemplates.Length - 1))
                    {
                        if (RemoveFish(playerControllerScript, researchRequirementTemplates[selectedResearchRequirementIndex].fishType))
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

    private bool IsFishTypeStoreable(GameData.FishType fishType)
    {
        for (int i = 0; i < researchRequirementTemplates.Length; i++)
        {
            if (fishType == researchRequirementTemplates[i].fishType)
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
            FishSchoolController schoolController = fishSchools[fishType].GetComponent<FishSchoolController>();
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
                        fishToStore.transform.SetParent(null);
                        fishToStore.transform.position = holdSlot.transform.position;
                    }
                }
            }
        }
    }

    public bool RemoveFish(PlayerInteractionController playerControllerScript, GameData.FishType fishType)
    {
        if (fishSchools.ContainsKey(fishType))
        {
            FishSchoolController schoolController = fishSchools[fishType].GetComponent<FishSchoolController>();
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
