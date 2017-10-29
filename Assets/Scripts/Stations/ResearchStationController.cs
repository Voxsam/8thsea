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
    private float progressBarWidth;

    //Prefab to instantiate progress ui
    [SerializeField] private GameObject holdSlot;
    [SerializeField] private RectTransform progressBarRect;

    // Use this for initialization
    void Start()
    {
        currentState = State.Empty;
        heldObject = null;
        progressBarWidth = progressBarRect.rect.width;
        progressBarRect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBarRect.gameObject.activeSelf)
            progressBarRect.sizeDelta = new Vector2(progressBarWidth * progressMade / maxProgress, progressBarRect.rect.height);
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
                    if (progressBarRect.gameObject.activeSelf)
                    {
                        progressBarRect.gameObject.SetActive(true);
                    }
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
                            progressBarRect.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    override public void ToggleHighlight(PlayerController otherPlayerController, bool toggle = true)
    {
    }
}
