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

    public GameController.FishType fishType;

    State currentState;
    SecondaryState currentSecondaryState;

    private Rigidbody rb;

    
    private float panicTimer;
    private float panicBarWidth;
    private ResearchProtocol[] researchProtocols;
    private int currentResearchProtocol;
    private Color originalColor;
    
    // GameObjects used by this class

    [SerializeField] private MeshRenderer fishRenderer;
    [SerializeField] private GameObject WorldspaceCanvas;
    [SerializeField] private Text DeadText;
    [SerializeField] private RectTransform PanicBarRect;
    private GameObject fishDetails;

    //Prefab to instantiate FishDetails
    public GameObject fishDetailsTemplate;

    // Use this for initialization
    void Start () {
        currentState = State.Idle;
        currentSecondaryState = SecondaryState.Idle;
        currentResearchProtocol = 0;

        //Get own rigidbody component.
        rb = this.GetComponent<Rigidbody>();

        fishDetails = null;

        DeadText.gameObject.SetActive(false);
        PanicBarRect.gameObject.SetActive(false);
        panicBarWidth = PanicBarRect.rect.width;
        panicTimer = GameController.GetFishParameter(fishType).panicTimerLength;

        fishDetails = (GameObject)Instantiate(fishDetailsTemplate);
        fishDetails.transform.SetParent(GameController.Obj.gameCamera.GetCanvas.transform, false);
        fishDetails.name = gameObject.ToString();
        fishDetails.GetComponent<FishDetailsController>().Init(fishType, gameObject);
        fishDetails.SetActive(false);

        originalColor = fishRenderer.material.color;

        researchProtocols = new ResearchProtocol[GameController.GetFishParameter(fishType).researchProtocols.Length];
        for (int i = 0; i < GameController.GetFishParameter(fishType).researchProtocols.Length; i++)
        {
            researchProtocols[i] = new ResearchProtocol (GameController.GetFishParameter(fishType).researchProtocols[i].researchStation);
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
                    DeadText.text = "d e d";
                    DeadText.gameObject.SetActive(true);
                }

                PanicBarRect.sizeDelta = new Vector2(panicBarWidth * panicTimer / GameController.GetFishParameter(fishType).panicTimerLength, PanicBarRect.rect.height);
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

    public void Interact ()
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

    public void Interact (GameObject otherActor)
    {
    }

    public void ToggleHighlight(bool toggle = true)
    {
        if (toggle)
        {
            Renderer rend = GetComponentInChildren<Renderer>();
            rend.material.color = Color.red;
            fishDetails.SetActive (true);
        }
        else
        {
            Renderer rend = GetComponentInChildren<Renderer>();
            rend.material.color = originalColor;
            if (fishDetails.activeSelf)
            {
                fishDetails.SetActive(false);
            }
        }
    }

    public string GetCurrentResearchProtocol ()
    {
        if (currentResearchProtocol < researchProtocols.Length)
            return researchProtocols[currentResearchProtocol].researchStation;
        else
            return "Done";
    }

    public void ResearchFish ()
    {
        currentResearchProtocol++;
        if ( currentResearchProtocol >= researchProtocols.Length )
        {
            currentResearchProtocol = researchProtocols.Length;
            currentSecondaryState = SecondaryState.Researched;
            WorldspaceCanvas.transform.Find("DeadText").gameObject.GetComponent<Text>().text = "Researched";
            WorldspaceCanvas.transform.Find("DeadText").gameObject.SetActive(true);
        }
    }
}
