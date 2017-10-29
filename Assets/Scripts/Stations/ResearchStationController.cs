using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchStationController : IInteractable
{
    enum State
    {
        Empty,
        Holding,
        Working
    };

    State currentState;

    public GameData.StationType researchStationType;

    private GameObject heldObject;
    private float maxProgress = 20f;
    private float progressMade = 0f;
    private float progressPerInteraction = 1f;
    private GameObject mainCamera;
    private GameObject gameCanvas;

    //Static so only one instance is used.
    private GameObject uiObject;

    //Prefab to instantiate progress ui
    public GameObject progressUI;
    [SerializeField]
    private GameObject holdSlot;

    // Use this for initialization
    void Start()
    {
        currentState = State.Empty;
        //holdSlot = gameObject.transform.Find("HoldSlot").gameObject;
        heldObject = null;
        uiObject = null;

        mainCamera = GameController.Obj.gameCamera.GetCamera.gameObject;
        gameCanvas = GameController.Obj.gameCamera.GetCanvas.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (uiObject != null)
        {
            GameObject words = uiObject.transform.GetChild(0).gameObject;
            words.GetComponent<Text>().text = progressMade.ToString() + "/" + maxProgress.ToString();

            if (uiObject.name == gameObject.ToString())
            {
                Vector2 ViewportPosition = GameController.Obj.gameCamera.GetCamera.WorldToScreenPoint(gameObject.transform.position);
                uiObject.GetComponent<RectTransform>().anchoredPosition = ViewportPosition;

            }
        }
    }

    override public void Interact()
    {
    }

    override public void Interact(GameObject otherActor)
    {
        if (otherActor.tag == "Player")
        {
            if (currentState == State.Empty)
            {
                PlayerInteractionController playerControllerScript = otherActor.GetComponent<PlayerInteractionController>();
                if (playerControllerScript != null)
                {
                    //Get the object held by the player.
                    heldObject = playerControllerScript.GetHeldObject();
                    if (heldObject != null)
                    {
                        FishController heldObjectControllerScript = heldObject.GetComponent<FishController>();
                        if (heldObjectControllerScript != null)
                        {
                            playerControllerScript.DropObject();
                            heldObjectControllerScript.PutIn();
                        }
                        heldObject.transform.SetParent(holdSlot.transform);
                        heldObject.transform.localPosition = Vector3.zero;

                        currentState = State.Holding;
                    }
                }
            }
            else if (currentState == State.Holding)
            {
                PlayerInteractionController playerControllerScript = (PlayerInteractionController)otherActor.GetComponent(typeof(PlayerInteractionController));
                if (playerControllerScript != null)
                {
                    //Check that the player is not already holding on to something.
                    if (playerControllerScript.GetHeldObject() == null)
                    {
                        if (heldObject != null)
                        {
                            currentState = State.Working;
                        }
                    }
                }
            }
            else if (currentState == State.Working)
            {
                PlayerInteractionController playerControllerScript = (PlayerInteractionController)otherActor.GetComponent(typeof(PlayerInteractionController));
                if (playerControllerScript != null)
                {
                    if (progressMade < maxProgress)
                    {
                        progressMade += progressPerInteraction;
                    }
                    //Check that the player is not already holding on to something.
                    else if (playerControllerScript.GetHeldObject() == null)
                    {
                        if (heldObject != null)
                        {
                            FishController heldObjectControllerScript = (FishController)heldObject.GetComponent(typeof(FishController));
                            if (heldObjectControllerScript.GetCurrentResearchProtocol() == researchStationType)
                            {
                                heldObjectControllerScript.ResearchFish();
                            }
                            playerControllerScript.PickUpObject(heldObject);
                            heldObject = null;
                            currentState = State.Empty;
                        }
                    }
                }
            }
        }
    }

    override public void ToggleHighlight(PlayerController otherPlayerController, bool toggle = true)
    {
        if (toggle)
        {
            //meshRenderer.material.color = Color.blue;
            if (uiObject == null)
            {
                uiObject = (GameObject)Instantiate(progressUI);
                GameObject words = uiObject.transform.GetChild(0).gameObject;
                words.GetComponent<Text>().text = progressMade.ToString() + "/" + maxProgress.ToString();
                uiObject.transform.SetParent(gameCanvas.transform, false);
                uiObject.name = gameObject.ToString();
            }
        }
        else
        {
            //meshRenderer.material.color = originalColor;
            if (uiObject != null)
            {
                if (uiObject.name == gameObject.ToString())
                {
                    Destroy(uiObject);
                    uiObject = null;
                    progressMade = 0;
                }
            }
        }
    }
}
