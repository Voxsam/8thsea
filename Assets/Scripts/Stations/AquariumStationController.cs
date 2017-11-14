using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AquariumStationController : StationControllerInterface
{
    public AquariumStationController Obj;

    public override GameData.ControlType ControlMode
    {
        get { return GameData.ControlType.STATION; }
    }

    [SerializeField]
    public GameObject fishSchool;

    [SerializeField] public GameData.FishType[] researchRequirementsForLevel;

    [SerializeField] private GameObject holdSlot;
    [SerializeField] private GameObject worldspaceCanvas;
    [SerializeField] private GameObject warningText;
    [SerializeField] private GameObject researchReqAnchor;
    private float warningTextLifespan = 0;
    private float selectInterval = 0;
    
    private Dictionary<GameData.FishType, GameObject> fishResearchRequirements;
    private int selectedResearchRequirementIndex = -1;

    void Awake()
    {
         Obj = this;
    }

    // Use this for initialization
    void Start()
    {
        warningText.SetActive(false);

        fishResearchRequirements = new Dictionary<GameData.FishType, GameObject>();
        researchRequirementsForLevel = GameData.GetResearchRequirementsForLevel(GameController.Obj.CurrentLevel);

        for (int i = 0; i < researchRequirementsForLevel.Length; i++)
        {
            // If does not contain a school of this type, instantiate it
            if (!GameController.Obj.MasterAquarium.ContainsKey(researchRequirementsForLevel[i]))
            {
                GameObject newFishSchool = (GameObject)Instantiate(fishSchool);
                newFishSchool.transform.position = holdSlot.transform.position;
                newFishSchool.transform.SetParent(GameController.Obj.AquariumHolder);
                FishSchoolController fishSchoolController = newFishSchool.GetComponent<FishSchoolController>();
                fishSchoolController.zoneX = 10;
                fishSchoolController.zoneY = 5;
                fishSchoolController.zoneZ = 10;
                GameController.Obj.MasterAquarium.Add(researchRequirementsForLevel[i], fishSchoolController);
            }

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
                    else if ((int)selectedResearchRequirementIndex > 0)
                    {
                        GameData.FishType fishType = researchRequirementsForLevel[selectedResearchRequirementIndex];
                        fishResearchRequirements[fishType].GetComponent<ResearchRequirementsController>().Deselect();
                        selectedResearchRequirementIndex--;
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
        stationCamera.SetCameraToObject(holdSlot);
    }

    public override void WhenActivated()
    {
        selectedResearchRequirementIndex = -1;

        GameData.FishType fishType = researchRequirementsForLevel[selectedResearchRequirementIndex];
        fishResearchRequirements[fishType].GetComponent<ResearchRequirementsController>().Select();
    }

    public override void WhenDeactivated()
    {
        foreach (KeyValuePair<GameData.FishType, GameObject> researchRequirement in fishResearchRequirements)
        {
            researchRequirement.Value.GetComponent<ResearchRequirementsController>().Deselect();
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
            FishSchoolController schoolController = GameController.Obj.MasterAquarium[fishType];
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

                    GameController.Obj.UpdateLevelProgression();

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
        if (GameController.Obj.MasterAquarium.ContainsKey(fishType))
        {
            FishSchoolController schoolController = GameController.Obj.MasterAquarium[fishType];
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

                            GameController.Obj.UpdateLevelProgression();
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
