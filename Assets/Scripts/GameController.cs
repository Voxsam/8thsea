using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public static GameController Obj;

    KeyCode BUTTON_A_KEYBOARD = KeyCode.Space;
    KeyCode BUTTON_B_KEYBOARD = KeyCode.E;

    KeyCode BUTTON_A_JOYSTICK = KeyCode.JoystickButton0;
    KeyCode BUTTON_B_JOYSTICK = KeyCode.JoystickButton1;
    
    // Submarine Management
    public static SubmarineController SubmarineRef
    {
        get { return SubmarineController.Obj; }
    }

    // Player management
    protected List<PlayerController> players;
    [SerializeField] protected Transform PlayerHolder;
    [SerializeField] protected PlayerController Player1Ref;
    
    public Transform Player1Location
    {
        get { return Player1Ref.transform.parent; }
    }

    // Fish management
    protected List<FishController> fishes;
    [SerializeField] protected Transform FishHolder;

    // Camera management
    public CameraController gameCamera;

	public MultiplayerManager multiplayerManager;

    // GameObjects
    [SerializeField] protected Canvas mainCanvas;
    [SerializeField] protected Transform Sea;
    [SerializeField] protected Transform Lab;

    // Randomisation
    public static System.Random RNG;

    public Text timeLeftText;
	public Text moneyText;

	public int currentMoney;
	public float timeTillNextPayment;

	void Awake() {
        if (Obj == null)
        {
            Obj = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        // GameController object should not be destructable
        DontDestroyOnLoad(this.gameObject);

		multiplayerManager.Setup ();
        Setup();
	}

    #region Getter and setters

    #region Button handlers
    #region Button A handler
    public bool ButtonA_Down
    {
        get { return Input.GetKeyDown(BUTTON_A_KEYBOARD) || Input.GetKeyDown(BUTTON_A_JOYSTICK); }
    }

    public bool ButtonA_Hold
    {
        get { return Input.GetKey(BUTTON_A_KEYBOARD) || Input.GetKey(BUTTON_A_JOYSTICK); }
    }
    #endregion

    #region Button B handler
    public bool ButtonB_Down
    {
        get { return Input.GetKeyDown(BUTTON_B_KEYBOARD) || Input.GetKeyDown(BUTTON_B_JOYSTICK); }
    }

    public bool ButtonB_Hold
    {
        get { return Input.GetKey(BUTTON_B_KEYBOARD) || Input.GetKey(BUTTON_B_JOYSTICK); }
    }
    #endregion
    #endregion

	/*
    public PlayerController Player1
    {
        get
        {
            // Only allow it if there is at least one player stored
            if (players.Count >= 1)
            {
                return players[0];
            }
            else
            {
                return null;
            }
        }

        private set
        {
            if (players.Count >= 1)
            {
                players[0] = value;
            }
        }
    }
    */

    public void AddFish(FishController fish)
    {
        fishes.Add(fish);
        fish.transform.SetParent(FishHolder);
    }
    #endregion

    #region Helper functions
    #region Coroutine functions
    protected void ActivateCallbackAfterDelay(float delay, System.Action callback)
    {
        StartCoroutine(ActivateCallbackAfterDelayCoroutine(delay, callback));
    }

    public static IEnumerator ActivateCallbackAfterDelayCoroutine(float delay, System.Action callback)
    {
        float timeToWaitUntil = Time.time + delay;
        while(Time.time < timeToWaitUntil)
        {
            yield return new WaitForEndOfFrame();
        }

        if (callback != null)
        {
            callback();
        }

        yield return null;
    }

    /// <summary>
    /// Uses a Coroutine to move an Object from point A to point B over a given time. Can activate a callback after if necesary
    /// </summary>
    /// <param name="obj">Object to be mvoed</param>
    /// <param name="travelTime">Time required to reach destination</param>
    /// <param name="destination">The destination location. This must be in either local space or world space depending on the 'useLocalPosition' variable. By default, this expects a local space coordinate.</param>
    /// <param name="useLocalPosition">Whether the destination variable is using local or world coordinates</param>
    /// <param name="callback">The callback to activate when this movement is complete</param>
    protected void MoveGameObjectToLocationInGivenTimeAndActivateCallback(Transform obj, float travelTime, Vector3 destination, bool useLocalPosition = true, System.Action callback = null)
    {
        MoveGameObjectAndActivateCallbackCoroutine(obj, travelTime, destination, useLocalPosition, callback);
    }

    public static IEnumerator MoveGameObjectAndActivateCallbackCoroutine(Transform obj, float travelTime, Vector3 destination, bool useLocalPosition = true, System.Action callback = null)
    {
        float timeStarted = Time.time;
        float timeToWaitUntil = timeStarted + travelTime;
        float timeTaken = 0f;

        Vector3 location = (useLocalPosition) ? obj.localPosition : obj.position;

        while (Time.time < timeToWaitUntil)
        {
            timeTaken = Time.time - timeStarted;
            location = Vector3.Lerp(location, destination, timeTaken/travelTime);
            if (useLocalPosition)
            {
                obj.localPosition = location;
            }
            else
            {
                obj.position = location;
            }

            yield return new WaitForEndOfFrame();
        }

        // Final allocation in case it was incomplete
        if (useLocalPosition)
        {
            obj.localPosition = destination;
        }
        else
        {
            obj.position = destination;
        }

        // Use callback if exists
        if (callback != null)
        {
            callback();
        }

        yield return null;
    }
    #endregion

    public void AddMoney(int amount) {
		currentMoney += amount;
	}

	public void RemoveMoney(int amount) {
		currentMoney -= amount;
	}

    /// <summary>
    /// This functions checks if the collider belongs to a Player object, and if it does, it returns the PlayerController for that object
    /// </summary>
    /// <param name="collider">The collider whose owner to check for</param>
    /// <returns></returns>
    public PlayerController GetPlayerFromCollider(Collider collider)
    {
        foreach(PlayerController player in players)
        {
            if (collider.gameObject == player.gameObject)
            {
                return player;
            }
        }

        return null;
    }

    /// <summary>
    /// This functions checks if the collider belongs to a Fish object, and if it does, it returns the FishController for that object
    /// </summary>
    /// <param name="collider">The collider whose owner to check for</param>
    /// <returns></returns>
    public FishController GetFishFromCollider(Collider collider)
    {
        /* // For now this is not ready yet, so comment it out. This will take over the temp code later
        foreach (FishController fish in fishes)
        {
            if (collider.gameObject == fish.gameObject)
            {
                return fish;
            }
        }//*/

        // Temp code
        if (collider.gameObject.tag.Equals("FishObject"))
        {
            return collider.GetComponent<FishController>();
        }

        return null;
    }
    #endregion

    #region Main functions
    protected void Setup()
    {
        timeTillNextPayment = GameData.PAYMENT_INTERVAL;
        gameCamera = GetComponentInChildren<CameraController>();

		players = multiplayerManager.playerControllerList;
        fishes = new List<FishController>();
        
		/*
        players.Add(Player1Ref);
        Player1Ref.AssignCameraToPlayer(gameCamera);
        Player1Ref.transform.SetParent(PlayerHolder);
        */

        mainCanvas.transform.SetParent(this.transform);
        Sea.transform.SetParent(this.transform);
        Lab.transform.SetParent(this.transform);


        RNG = new System.Random();
    }
    #endregion

    #region Update handlers
    /// <summary>
    /// GameController's own update handler. Separate it in case some things want to go into FixedUpdate instead of Update or vice versa
    /// </summary>
    protected void GameUpdate()
    {
        if (timeTillNextPayment <= 0)
        {
            RemoveMoney(GameData.PAYMENT_AMOUNT);
            timeTillNextPayment = GameData.PAYMENT_INTERVAL;
        }

        timeTillNextPayment -= Time.deltaTime;

        moneyText.text = "$" + currentMoney.ToString();
        timeLeftText.text = "Time Left: " + (int)timeTillNextPayment + "s";
    }

    void Update()
    {

        // Handle the update loops for the others too
		foreach (PlayerController player in players) {
			player.GameUpdate ();
		}

		//Player1.GameUpdate();

		// Handle update loop for all players
		/*
		foreach (PlayerController player in multiplayerManager.playerList) {
			player.GameUpdate ();
		}
		*/


    }
    #endregion
}
