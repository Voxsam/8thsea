using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishController : MonoBehaviour, IInteractable {

    private struct ResearchProtocol
    {
        public string researchStation;
        public bool complete;

        public ResearchProtocol ( string _researchStation )
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

    public int fishType;

    State currentState;
    SecondaryState currentSecondaryState;

    private Rigidbody rigidBody;

    private GameObject mainCamera;
    private GameObject gameCanvas;
    private RectTransform canvasRect;

    private GameObject worldspaceCanvas;
    private GameObject panicBar;
    private RectTransform panicBarRect;
    private float panicTimer;
    private float panicBarWidth;
    private GameObject fishDetails;
    private ResearchProtocol[] researchProtocols;
    private int currentResearchProtocol;

    //Prefab to instantiate FishDetails
    public GameObject fishDetailsTemplate;

    // Use this for initialization
    void Start () {
        currentState = State.Idle;
        currentSecondaryState = SecondaryState.Idle;
        currentResearchProtocol = 0;

        //Get own rigidbody component.
        rigidBody = this.GetComponent<Rigidbody>();

        fishDetails = null;

        mainCamera = Camera.main.gameObject;
        gameCanvas = mainCamera.gameObject.transform.Find("SuperImposedUI").gameObject;
        canvasRect = gameCanvas.GetComponent<RectTransform>();

        worldspaceCanvas = gameObject.transform.Find("WorldspaceCanvas").gameObject;
        worldspaceCanvas.transform.Find("DeadText").gameObject.SetActive(false);
        panicBar = worldspaceCanvas.transform.Find("PanicBar").gameObject;
        panicBar.SetActive(false);
        panicBarRect = panicBar.GetComponent<RectTransform>();
        panicBarWidth = panicBarRect.rect.width;
        panicTimer = GameLogicController.AllFishParameters[fishType].panicTimerLength;

        fishDetails = (GameObject)Instantiate(fishDetailsTemplate);
        fishDetails.transform.SetParent(gameCanvas.transform, false);
        fishDetails.name = gameObject.ToString();
        fishDetails.GetComponent<FishDetailsController>().Init(fishType, gameObject);
        fishDetails.SetActive(false);

        researchProtocols = new ResearchProtocol[GameLogicController.AllFishParameters[fishType].researchProtocols.Length];
        for (int i = 0; i < GameLogicController.AllFishParameters[fishType].researchProtocols.Length; i++)
        {
            researchProtocols[i] = new ResearchProtocol (GameLogicController.AllFishParameters[fishType].researchProtocols[i].researchStation);
        }
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
                panicTimer -= Time.deltaTime;

                if (panicTimer <= 0)
                {
                    currentSecondaryState = SecondaryState.Dead;
                    panicTimer = 0f;
                    worldspaceCanvas.transform.Find("DeadText").gameObject.GetComponent<Text>().text = "d e d";
                    worldspaceCanvas.transform.Find("DeadText").gameObject.SetActive(true);
                }

                panicBarRect.sizeDelta = new Vector2(panicBarWidth * panicTimer / GameLogicController.AllFishParameters[fishType].panicTimerLength, panicBarRect.rect.height);
                break;

            default:
                break;
        }
	}

    void LateUpdate()
    {
        worldspaceCanvas.transform.rotation = Quaternion.identity;
        worldspaceCanvas.transform.position = transform.position;
    }

    //Activates/deactivates RigidBody of prefab.
    public void ToggleRigidBody ()
    {
        if (rigidBody)
        {
            rigidBody.isKinematic = !rigidBody.isKinematic;
        }
    }

    public void ToggleDetectCollisions()
    {
        if (rigidBody)
        {
            rigidBody.detectCollisions = !rigidBody.detectCollisions;
        }
    }

    public void pickUp ()
    {
        if (rigidBody)
        {
            rigidBody.isKinematic = true;
            rigidBody.detectCollisions = false;
        }
        currentState = State.Held;
        if (currentSecondaryState == SecondaryState.Idle)
        {
            currentSecondaryState = SecondaryState.Panic;
            panicBar.SetActive(true);
        }
    }

    public void putDown ()
    {
        if (rigidBody)
        {
            rigidBody.isKinematic = false;
            rigidBody.detectCollisions = true;
        }
        currentState = State.Idle;
    }

    public void putIn ()
    {
        if (rigidBody)
        {
            rigidBody.isKinematic = true;
            rigidBody.detectCollisions = false;
        }

        currentState = State.Placed;
    }

    public void interact ()
    {
        switch (currentState)
        {
            case State.Idle:
                pickUp();
                break;

            case State.Held:
                putDown();
                break;

            case State.Placed:
                pickUp();
                break;

            default:
                break;
        }
    }

    public void interact (GameObject otherActor)
    {
    }

    public void toggleHighlight(bool toggle = true)
    {
        if (toggle)
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.red;
            fishDetails.SetActive (true);
        }
        else
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.white;
            if (fishDetails.activeSelf)
            {
                fishDetails.SetActive(false);
            }
        }
    }

    public string getCurrentResearchProtocol ()
    {
        if (currentResearchProtocol < researchProtocols.Length)
            return researchProtocols[currentResearchProtocol].researchStation;
        else
            return "Done";
    }

    public void researchFish ()
    {
        currentResearchProtocol++;
        if ( currentResearchProtocol >= researchProtocols.Length )
        {
            currentResearchProtocol = researchProtocols.Length;
            currentSecondaryState = SecondaryState.Researched;
            worldspaceCanvas.transform.Find("DeadText").gameObject.GetComponent<Text>().text = "Researched";
            worldspaceCanvas.transform.Find("DeadText").gameObject.SetActive(true);
        }
    }
}
