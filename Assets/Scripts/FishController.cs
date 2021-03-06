﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishController : IInteractable {
    public struct ResearchProtocol
    {
        public GameData.StationType researchStation;
        public bool complete;
        public ResearchProtocol(GameData.StationType _researchStation)
        {
            researchStation = _researchStation;
            complete = false;
        }
    };

    //Enums for fish states
    public enum State
    {
        Idle,
        Held,
        Placed
    };

    //Enums for fish secondary states
    public enum SecondaryState
    {
        Idle,
        Panic,
        Dead,
        Researched
    };

	// Const
	public const string ANIMATOR_SWIM = "Swim";

    State currentState;
    SecondaryState currentSecondaryState;
    
    public GameData.FishType fishType;
    public FishMovementController fishMovementController;

    //ResearchProtocols to display to user and to show research progress for fish.
    private ResearchProtocol[] researchProtocols;

    private float floorPanicRate = 5;
    private float slowPanicRate = 0.2f;

    private float currentPanicRate;
    private float previousPanicRate;
    private float panicTimer;
    private float panicBarWidth;
    private int currentResearchProtocol;
    private Color originalColor;
    private Vector3 caughtPosition;
    private FishSchoolController fishSchoolController;
	private Animator fishAnimator = null;

    // GameObjects used by this class
    public Rigidbody rb;

    //Sprites for status of fish
    public Sprite deadSprite;
    public Sprite researchedSprite;

	[SerializeField] private SkinnedMeshRenderer fishRenderer;
    [SerializeField] private GameObject WorldspaceCanvas;
    [SerializeField] private Image StatusIcon;
    [SerializeField] private RectTransform PanicBarRect;

    //Prefab to instantiate researchProtocols.
    public GameObject researchProtocolTemplate;

    // Use this for initialization
    void Start()
    {
        currentState = State.Idle;
        currentSecondaryState = SecondaryState.Idle;
        currentResearchProtocol = 0;

        //Get own rigidbody component.
        rb = this.GetComponent<Rigidbody>();

        // Get own movement controller
        fishMovementController = GetComponent<FishMovementController>();

        StatusIcon.enabled = false;
        PanicBarRect.gameObject.SetActive(false);
        panicBarWidth = PanicBarRect.rect.width;
        panicTimer = GameData.GetFishParameters(fishType).panicTimerLength;
        currentPanicRate = slowPanicRate;

        if (fishType != GameData.FishType.None)
        {
			fishRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            Setup();
        }

        SetEnabled(false);
    }

	public void Setup(GameData.FishType type, SkinnedMeshRenderer mesh, Animator animator) {
		fishType = type;
		fishRenderer = mesh;
		fishAnimator = animator;

		Setup();
	}

	private void Setup() {
		originalColor = fishRenderer.material.color;

		researchProtocols = new ResearchProtocol[GameData.GetFishParameters(fishType).ResearchProtocols.Length];
		for (int i = 0; i < GameData.GetFishParameters(fishType).ResearchProtocols.Length; i++)
		{
			GameData.ResearchStationParameters currentResearchStationParameters = GameData.GetResearchStationParameters(GameData.GetFishParameters(fishType).ResearchProtocols[i]);
			researchProtocols[i] = new ResearchProtocol(currentResearchStationParameters.researchStation);
		}

		// Enable the fish to swim
		if (fishAnimator != null)
		{
			fishAnimator.SetTrigger(ANIMATOR_SWIM);
		}

		SetEnabled(false);
		// Use the FishParameters for this
		//researchProtocols = new GameData.StationType[GameData.GetFishParameter(fishType).researchProtocols.Length];
		//for (int i = 0; i < GameData.GetFishParameter(fishType).researchProtocols.Length; i++)
		//{
		//    researchProtocols[i] = GameData.GetFishParameter(fishType).researchProtocols[i];
		//}
	}
	
	// Update is called once per frame
	void Update () {
        switch (currentSecondaryState)
        {
            case SecondaryState.Idle:
                
                break;

            case SecondaryState.Dead:
                break;

            case SecondaryState.Researched:

            case SecondaryState.Panic:
                panicTimer -= Time.deltaTime * currentPanicRate;

                if (panicTimer <= 0)
                {
                    currentSecondaryState = SecondaryState.Dead;
                    panicTimer = 0f;
                    StatusIcon.sprite = deadSprite;
                    StatusIcon.enabled = true;
                }

                PanicBarRect.sizeDelta = new Vector2(panicBarWidth * panicTimer / GameData.GetFishParameters(fishType).panicTimerLength, PanicBarRect.rect.height);
                break;

            default:
                break;
        }
	}

    void LateUpdate()
    {
        WorldspaceCanvas.transform.rotation = Quaternion.identity;
        WorldspaceCanvas.transform.position = transform.position;
    }

    //Activates/deactivates RigidBody of prefab.
    public void ToggleRigidBody ()
    {
        if (rb)
        {
            rb.isKinematic = !rb.isKinematic;
        }
    }

    public void ToggleDetectCollisions()
    {
        if (rb)
        {
            rb.detectCollisions = !rb.detectCollisions;
        }
    }

    public void PickUp ()
    {
        if (rb)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
        currentState = State.Held;
        if (currentPanicRate == floorPanicRate)
        {
            currentPanicRate = previousPanicRate;
        }
        if (currentSecondaryState == SecondaryState.Idle)
        {
            currentSecondaryState = SecondaryState.Panic;
            PanicBarRect.gameObject.SetActive(true);
        }
    }

    public void PutDown ()
    {
        if (rb)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }
        currentState = State.Idle;
        previousPanicRate = currentPanicRate;
        currentPanicRate = floorPanicRate;
    }

    public void PutIn ()
    {
        if (rb)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        currentState = State.Placed;
    }

    public void StartFastPanic ()
    {
        currentPanicRate = GameData.GetFishParameters(fishType).researchPanicRate;
    }
    public void StartSlowPanic ()
    {
        if (previousPanicRate != GameData.GetFishParameters(fishType).researchPanicRate)
        {
            currentPanicRate = slowPanicRate;
        }
    }

    public void SetEnabled(bool enabled)
	{
        SetRigidbody(enabled);
        WorldspaceCanvas.SetActive(enabled);
        this.enabled = enabled;
    } 

    public void SetRigidbody(bool enabled)
    {
        if (rb)
        {
            rb.isKinematic = !enabled;
            rb.useGravity = enabled;
        }
    }

	override public void Interact ()
    {
        switch (currentState)
        {
            case State.Idle:
            case State.Placed:
                PickUp();
                break;
            case State.Held:
                PutDown();
                break;
            default:
                break;
        }
    }

    override public void Interact (GameObject otherActor)
    {
		switch (currentState) {
		case State.Idle:
		case State.Placed:
			PickUp ();
			otherActor.GetComponent<PlayerController> ().canvas.ShowFishDetailsPanel (GameData.GetFishName(fishType), researchProtocols, currentSecondaryState);
			break;
		case State.Held:
			otherActor.GetComponent<PlayerController> ().canvas.HideBottomLeftPanel ();
			PutDown ();
			break;
		default:
			break;
		}
    }

    override public void ToggleHighlight(PlayerController player, bool toggle = true)
    {
        if (toggle)
        {
            Renderer rend = GetComponentInChildren<Renderer>();
            rend.material.color = Color.red;
        }
        else
        {
            Renderer rend = GetComponentInChildren<Renderer>();
            rend.material.color = originalColor;
        }
    }

    public bool IsCurrentResearchProtocol(GameData.StationType stationType)
    {
        if (currentResearchProtocol >= researchProtocols.Length)
            return false;
        return (!researchProtocols[currentResearchProtocol].complete && (researchProtocols[currentResearchProtocol].researchStation == stationType));
    }

    /// <summary>
    /// Returns the current ResearchProtocol that needs to be performed. If there are none left (which means the fish is done), None is returned
    /// </summary>
    /// <returns></returns>
    public GameData.StationType GetCurrentResearchProtocol ()
    {
        if (currentResearchProtocol < researchProtocols.Length)
            return researchProtocols[currentResearchProtocol].researchStation;
        else
            return GameData.StationType.None; // Means Done
    }

    public void ResearchFish ()
    {
        //Eventually move this color change thing to a ResearchProtocolController.
		researchProtocols [currentResearchProtocol].complete = true;
        currentResearchProtocol++;
        if ( currentResearchProtocol >= researchProtocols.Length )
        {
            panicTimer = GameData.GetFishParameters(fishType).panicTimerLength;
            currentPanicRate = slowPanicRate;
            currentResearchProtocol = researchProtocols.Length;
            currentSecondaryState = SecondaryState.Researched;
            StatusIcon.sprite = researchedSprite;
            StatusIcon.enabled = true;
            GameData.AddResearchedFish(fishType);

        }
    }

    // If the player isn't holding it, attach it to its appropriate location
    public void DropFish()
    {

    }

    public bool IsDead()
    {
        return currentSecondaryState == SecondaryState.Dead;
    }

    #region Getters/Settrs
    public Vector3 CaughtPosition
    {
        get
        {
            return caughtPosition;
        }
        set
        {
            caughtPosition = value;
        }
    }

    public FishSchoolController FishSchoolController
    {
        get
        {
            return fishSchoolController;
        }
        set
        {
            fishSchoolController = value;
        }
    }

    public SecondaryState CurrentSecondaryState
    {
        get
        {
            return currentSecondaryState;
        }
    }

    public State GetCurrentState
    {
        get { return currentState; }
    }
    #endregion

}
