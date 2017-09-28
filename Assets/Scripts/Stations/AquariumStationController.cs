using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AquariumStationController : MonoBehaviour, IInteractable
{
    public GameController.ControlType ControlMode
    {
        get { return GameController.ControlType.STATION; }
    }
    private bool isPlayerControlled;
    private GameObject playerGameObject;
    private Vector3 playerStationPosition;

    [SerializeField]
    public Shader outlineShader;
    [SerializeField]
    public GameObject fishSchool;
    [SerializeField]
    public GameObject fishResearchRequirementsTemplate;

    private GameObject holdSlot;
    private Renderer meshRenderer;
    private GameObject worldspaceCanvas;
    private Shader originalShader;
    private Transform playerAnchor;
    private GameObject warningText;
    private float warningTextLifespan = 0;

    private List<GameObject> fishSchools;
    private List<GameObject> fishResearchRequirements;
    private int selectedResearchRequirementIndex = 0;

    // Use this for initialization
    void Start()
    {
        holdSlot = gameObject.transform.Find("HoldSlot").gameObject;
        meshRenderer = gameObject.transform.Find("Mesh").GetComponent<Renderer>();
        originalShader = meshRenderer.material.shader;
        worldspaceCanvas = GetComponentInChildren<Canvas>().gameObject;
        warningText = worldspaceCanvas.transform.Find("Feedback").gameObject;
        warningText.SetActive(false);
        playerAnchor = gameObject.transform.Find("PlayerAnchor").transform;

        fishSchools = new List<GameObject>();
        fishResearchRequirements = new List<GameObject>();

        int fishIndex = 0;
        foreach (GameLogicController.FishParameters fish in GameLogicController.AllFishParameters)
        {
            GameObject newFishSchool = (GameObject)Instantiate(fishSchool);
            newFishSchool.transform.position = holdSlot.transform.position;
            FishSchoolController fishSchoolController = newFishSchool.GetComponent<FishSchoolController>();
            fishSchoolController.zoneWidth = 10;
            fishSchoolController.zoneLength = 5;
            fishSchoolController.zoneHeight = 10;
            fishSchools.Add(newFishSchool);

            GameObject newFishResearchRequirements = (GameObject)Instantiate(fishResearchRequirementsTemplate);
            newFishResearchRequirements.transform.SetParent(worldspaceCanvas.transform, false);
            newFishResearchRequirements.GetComponent<ResearchRequirementsController>().Init(fishIndex);
            newFishResearchRequirements.GetComponent<RectTransform>().anchoredPosition = new Vector2((fishIndex * 500), 500);
            fishResearchRequirements.Add(newFishResearchRequirements);

            fishIndex++;
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

        if (isPlayerControlled)
        {
            if (Input.GetAxis("Horizontal") > 0.1)
            {
                if (selectedResearchRequirementIndex < (fishResearchRequirements.Count - 1))
                {
                    fishResearchRequirements[selectedResearchRequirementIndex].GetComponent<ResearchRequirementsController>().ToggleFishDetails(false);
                    selectedResearchRequirementIndex++;
                    fishResearchRequirements[selectedResearchRequirementIndex].GetComponent<ResearchRequirementsController>().ToggleFishDetails(true);
                }
            }
            else if (Input.GetAxis("Horizontal") < -0.1)
            {
                if (selectedResearchRequirementIndex > 0)
                {
                    fishResearchRequirements[selectedResearchRequirementIndex].GetComponent<ResearchRequirementsController>().ToggleFishDetails(false);
                    selectedResearchRequirementIndex--;
                    fishResearchRequirements[selectedResearchRequirementIndex].GetComponent<ResearchRequirementsController>().ToggleFishDetails(true);
                }
            }

            if (GameController.Obj.ButtonB_Down)
            {
                PlayerController playerControllerScript = playerGameObject.GetComponent<PlayerController>();
                if (playerControllerScript != null)
                {
                    DisengagePlayer();
                }
            }
            else if (GameController.Obj.ButtonA_Down)
            {
                PlayerInteractionController playerControllerScript = playerGameObject.GetComponent<PlayerInteractionController>();
                if (playerControllerScript != null)
                {
                    if (removeFish(playerControllerScript, selectedResearchRequirementIndex))
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
        
        if (playerGameObject != null)
        {
            isPlayerControlled = true;
        }
    }

    public void Interact()
    {
    }

    public void Interact(GameObject otherActor)
    {
        if (otherActor.tag == "Player")
        {
            //Make sure the station is not currently controlled by another player.
            if (!isPlayerControlled)
            {
                PlayerInteractionController playerInteractionControllerScript = otherActor.GetComponent<PlayerInteractionController>();
                if (playerInteractionControllerScript != null)
                {
                    //Get the object held by the player.
                    GameObject objectToHold = playerInteractionControllerScript.getHeldObject();
                    //Attempt to deposit the fish into the aquarium if the player is holding one.
                    if (objectToHold != null)
                    {
                        FishController heldObjectControllerScript = objectToHold.GetComponent<FishController>();
                        if (heldObjectControllerScript != null)
                        {
                            //Only allow the player to put a fish into the aquarium if they have researched the required amount for the species.
                            if (GameLogicController.AllFishParameters[heldObjectControllerScript.fishType].totalResearched == GameLogicController.AllFishParameters[heldObjectControllerScript.fishType].totalToResearch)
                            {
                                playerInteractionControllerScript.DropObject();
                                storeFish(objectToHold, heldObjectControllerScript.fishType);
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

    public void storeFish ( GameObject fishToStore, int fishType )
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

    public bool removeFish ( PlayerInteractionController playerControllerScript, int fishType )
    {
        if (fishType >= 0 && fishType < fishSchools.Count)
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

    public void ToggleHighlight(bool toggle = true)
    {
        if (toggle)
        {
            if (outlineShader != null)
            {
                meshRenderer.material.shader = outlineShader;
            }
        }
        else
        {
            meshRenderer.material.shader = originalShader;
        }
    }

    private void ShowWarningText (string text)
    {
        warningText.GetComponent<Text>().text = text;
        warningText.SetActive(true);
        warningTextLifespan = 0;
    }

    private bool EngagePlayer (GameObject player)
    {
        playerGameObject = player;
        PlayerController playerControllerScript = player.GetComponent<PlayerController>();
        if (playerControllerScript != null)
        {
            playerControllerScript.RequestControlChange(ControlMode);

            // Store player's initial position
            playerStationPosition = playerGameObject.transform.position;

            // Set player to be in a specific position.
            playerGameObject.transform.SetParent(playerAnchor);
            playerGameObject.transform.localPosition = Vector3.zero;

            // Prevent the player from activating its rigidbody
            playerGameObject.SetActive(false);

            selectedResearchRequirementIndex = 0;
            fishResearchRequirements[selectedResearchRequirementIndex].GetComponent<ResearchRequirementsController>().ToggleFishDetails(true);

            return true;
        }
        return false;
    }

    private bool DisengagePlayer()
    {
        PlayerController playerControllerScript = playerGameObject.GetComponent<PlayerController>();
        if (playerControllerScript != null)
        {
            isPlayerControlled = false;

            playerControllerScript.ReturnControlToCharacter();

            // Reactivate the player
            playerGameObject.SetActive(true);

            // Reset the player Position
            playerGameObject.transform.position = playerStationPosition;
            playerGameObject.transform.SetParent(null);
            playerGameObject = null;

            foreach (GameObject researchRequirement in fishResearchRequirements)
            {
                researchRequirement.GetComponent<ResearchRequirementsController>().ToggleFishDetails(false);
            }
            return true;
        }
        return false;
    }
}
