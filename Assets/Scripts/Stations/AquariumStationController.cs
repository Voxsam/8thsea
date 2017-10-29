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
    [SerializeField]
    public GameObject fishResearchRequirementsTemplate;

    private GameObject holdSlot;
    private GameObject worldspaceCanvas;
    private Transform playerAnchor;
    private GameObject warningText;
    private float warningTextLifespan = 0;

    private Dictionary<GameData.FishType, GameObject> fishSchools;
    private Dictionary<GameData.FishType, GameObject> fishResearchRequirements;
    private GameData.FishType selectedResearchRequirementIndex = GameData.FishType.ClownFish;

    // Use this for initialization
    void Start()
    {
        holdSlot = gameObject.transform.Find("HoldSlot").gameObject;
        worldspaceCanvas = GetComponentInChildren<Canvas>().gameObject;
        warningText = worldspaceCanvas.transform.Find("Feedback").gameObject;
        warningText.SetActive(false);
        playerAnchor = gameObject.transform.Find("PlayerAnchor").transform;

        fishSchools = new Dictionary<GameData.FishType, GameObject>();
        fishResearchRequirements = new Dictionary<GameData.FishType, GameObject>();

        for (GameData.FishType fishType = GameData.FishType.ClownFish; (int)fishType < GameData.TOTAL_NUMBER_OF_FISHTYPES; fishType++)
        {
            GameObject newFishSchool = (GameObject)Instantiate(fishSchool);
            newFishSchool.transform.position = holdSlot.transform.position;
            FishSchoolController fishSchoolController = newFishSchool.GetComponent<FishSchoolController>();
            fishSchoolController.zoneWidth = 10;
            fishSchoolController.zoneLength = 5;
            fishSchoolController.zoneHeight = 10;
            fishSchools.Add(fishType, newFishSchool);

            GameObject newFishResearchRequirements = (GameObject)Instantiate(fishResearchRequirementsTemplate);
            newFishResearchRequirements.transform.SetParent(worldspaceCanvas.transform, false);
            newFishResearchRequirements.GetComponent<ResearchRequirementsController>().Init(fishType);
            newFishResearchRequirements.GetComponent<RectTransform>().anchoredPosition = new Vector2(((int)fishType * 500), 500);
            fishResearchRequirements.Add(fishType, newFishResearchRequirements);
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

        if (this.IsActivated)
        {
            if (Input.GetAxis("Horizontal") > 0.2)
            {
                if ((int)selectedResearchRequirementIndex < (GameData.TOTAL_NUMBER_OF_FISHTYPES - 1))
                {
                    fishResearchRequirements[selectedResearchRequirementIndex].GetComponent<ResearchRequirementsController>().ToggleFishDetails(false);
                    selectedResearchRequirementIndex++;
                    fishResearchRequirements[selectedResearchRequirementIndex].GetComponent<ResearchRequirementsController>().ToggleFishDetails(true);
                }
            }
            else if (Input.GetAxis("Horizontal") < -0.2)
            {
                if ((int)selectedResearchRequirementIndex > 0)
                {
                    fishResearchRequirements[selectedResearchRequirementIndex].GetComponent<ResearchRequirementsController>().ToggleFishDetails(false);
                    selectedResearchRequirementIndex--;
                    fishResearchRequirements[selectedResearchRequirementIndex].GetComponent<ResearchRequirementsController>().ToggleFishDetails(true);
                }
            }

            if (GameController.Obj.ButtonB_Down)
            {
                PlayerController playerControllerScript = this.playerInStation.GetComponent<PlayerController>();
                if (playerControllerScript != null)
                {
                    DisengagePlayer();
                }
            }
            else if (GameController.Obj.ButtonA_Down)
            {
                PlayerInteractionController playerControllerScript = this.playerInStation.GetComponent<PlayerInteractionController>();
                if (playerControllerScript != null)
                {
                    if (RemoveFish(playerControllerScript, selectedResearchRequirementIndex))
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
        stationCamera.SetCameraToObject(holdSlot);
    }

    public override void WhenActivated()
    {
        selectedResearchRequirementIndex = GameData.FishType.ClownFish;
        fishResearchRequirements[selectedResearchRequirementIndex].GetComponent<ResearchRequirementsController>().ToggleFishDetails(true);
    }

    public override void WhenDeactivated()
    {
        foreach (KeyValuePair<GameData.FishType, GameObject> researchRequirement in fishResearchRequirements)
        {
            researchRequirement.Value.GetComponent<ResearchRequirementsController>().ToggleFishDetails(false);
        }
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
