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
    private const float MAX_PROGRESS = 3f;
    private const float PROGRESS_BAR_PER_INTERACTION = 0.1f;
    //private const float PROGRESS_BAR_PER_FRAME = 0.1f;
    private float interactionProgressMade = 0f;
    private float progressMade = 0f;
    private float progressBarWidth;
    private PlayerController playerControllerInStation;


    public float minimumWaitTimeBetweenSFX = 0f; // seconds
    public float maximumWaitTimeBetweenSFX = 2f; // seconds
    private float timeToWaitUntil = 0f;
    private bool allowToPlaySFX = true;
    private AudioController.SoundEffect[] stationSounds = null;

    private float GetWaitTimeBetweenSFX()
    {
        float multiplier = (float)GameController.RNG.NextDouble();
        return minimumWaitTimeBetweenSFX + multiplier * (maximumWaitTimeBetweenSFX - minimumWaitTimeBetweenSFX);
    }

    //Prefab to instantiate progress ui
    [SerializeField] private GameObject holdSlot;
    [SerializeField] private RectTransform progressBarRect;
    [SerializeField] private GameObject worldspaceCanvas;
    [SerializeField] private ButtonPromptController buttonPromptController;
    [SerializeField] private ExpandingEmitterController buttonParticleController;
    [SerializeField] private ProgressBarController progressBarController;

    // Use this for initialization
    void Start()
    {
        currentState = State.Empty;
        heldObject = null;
        progressBarWidth = progressBarRect.rect.width;
        worldspaceCanvas.SetActive(false);
        playerControllerInStation = null;

        switch(researchStationType) {
            case GameData.StationType.Photograph:
                stationSounds = new AudioController.SoundEffect[] { AudioController.SoundEffect.Camera_Shutter_I, AudioController.SoundEffect.Camera_Shutter_II };
                break;
            default:
                break; // Nothing changes
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!allowToPlaySFX)
        {
            if (Time.time > timeToWaitUntil)
            {
                allowToPlaySFX = true;
            }
        }

        if (progressBarRect.gameObject.activeSelf)
        {
            progressBarRect.sizeDelta = new Vector2(progressBarWidth * progressMade / MAX_PROGRESS, progressBarRect.rect.height);
        }

        // If it is in the working state (the player is in the station)
        // And the A button is being held
        if (currentState == State.Working && playerControllerInStation != null)
        {
            // Play sound
            if (allowToPlaySFX && stationSounds != null)
            {
                allowToPlaySFX = false;
                GameController.Audio.PlaySFXOnce(stationSounds[GameController.RNG.Next(stationSounds.Length)]);
                timeToWaitUntil = Time.time + GetWaitTimeBetweenSFX();
            }

            if (progressMade < MAX_PROGRESS)
            {
                progressMade += Time.deltaTime;
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
                            if (!heldObjectControllerScript.IsDead())
                            {
                                if (heldObjectControllerScript.IsCurrentResearchProtocol(researchStationType))
                                {
                                    playerControllerScript.DropObject();
                                    heldObjectControllerScript.PutIn();

                                    heldObject.transform.SetParent(holdSlot.transform, true);
                                    heldObject.transform.localPosition = Vector3.zero;

                                    currentState = State.Holding;
                                    heldObjectControllerScript.StartFastPanic();
                                }
                            }
                        }
                    }
                }
            }

            if (currentState == State.Holding)
            {
                playerControllerInStation = otherActor.GetComponent<PlayerController>();
                playerControllerInStation.RequestControlChange(GameData.ControlType.STATION);
                playerControllerScript = otherActor.GetComponent<PlayerInteractionController>();

				if (researchStationType == GameData.StationType.Sample && GameController.Obj.isTutorial) {
					if (TutorialManager.Obj.currentStep == 8) {
						TutorialManager.Obj.hasUsedDissectStation = true;
					}
				}


                if (playerControllerScript != null)
                {
                    //Check that the player is not already holding on to something.
                    if (playerControllerScript.GetHeldObject() == null)
                    {
                        if (heldObject != null)
                        {
                            if (!worldspaceCanvas.activeSelf)
                            {
                                worldspaceCanvas.SetActive(true);
                            }
                            currentState = State.Working;
                        }
                    }
                }
            }
            else if (currentState == State.Working)
            {



                playerControllerScript = playerControllerInStation.interactionController;
                if (playerControllerScript != null)
                {
                    if (progressMade < MAX_PROGRESS)
                    {
                        progressMade += PROGRESS_BAR_PER_INTERACTION;
                        interactionProgressMade += PROGRESS_BAR_PER_INTERACTION;
                        buttonPromptController.Depress();
                        buttonParticleController.Fire();
                        progressBarController.Explode();
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
                            playerControllerInStation.ReturnControlToCharacter();
                            playerControllerScript.PickUpObject(heldObject);
                            playerControllerInStation = null;
                            heldObject = null;
                            progressMade = interactionProgressMade = 0f;
                            currentState = State.Empty;
                            worldspaceCanvas.SetActive(false);

							if (researchStationType == GameData.StationType.Sample && GameController.Obj.isTutorial) {
								if (TutorialManager.Obj.currentStep == 9) {
									TutorialManager.Obj.hasFinishedDissectStation = true;
								}
							}

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
