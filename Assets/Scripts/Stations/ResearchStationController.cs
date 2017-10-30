﻿using System;
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
    private const float MAX_PROGRESS = 20f;
    private const float PROGRESS_BAR_PER_INTERACTION = 1f;
    private const float PROGRESS_BAR_PER_FRAME = 0.3f;
    private float progressMade = 0f;
    private float progressBarWidth;
    private PlayerController playerControllerInStation;

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
        playerControllerInStation = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBarRect.gameObject.activeSelf)
        {
            progressBarRect.sizeDelta = new Vector2(progressBarWidth * progressMade / MAX_PROGRESS, progressBarRect.rect.height);
        }

        // If it is in the working state (the player is in the station)
        // And the A button is being held
        if (currentState == State.Working && playerControllerInStation != null)
        {
            if (progressMade < MAX_PROGRESS)
            {
                progressMade += PROGRESS_BAR_PER_FRAME;
            }
            else
            {
                Interact(playerControllerInStation.gameObject);
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
            PlayerInteractionController playerControllerScript = null;
            if (currentState == State.Empty)
            {
                playerControllerScript = otherActor.GetComponent<PlayerInteractionController>();
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
                playerControllerScript = otherActor.GetComponent<PlayerInteractionController>();
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
                playerControllerInStation = otherActor.GetComponent<PlayerController>();
                playerControllerScript = playerControllerInStation.interactionController;
                if (playerControllerScript != null)
                {
                    if (!progressBarRect.gameObject.activeSelf)
                    {
                        progressBarRect.gameObject.SetActive(true);
                    }

                    if (progressMade < MAX_PROGRESS)
                    {
                        progressMade += PROGRESS_BAR_PER_INTERACTION;
                    }
                    // If progress made is maxed and
                    // the the player is not already holding on to something.
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
                            playerControllerInStation = null;
                            heldObject = null;
                            progressMade = 0f;
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
