using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public static GameController Obj;

    public enum ControlType
    {
        CHARACTER,
        SUBMARINE,
        STATION,
    };

    public const float PAYMENT_INTERVAL = 60f;
    public const int PAYMENT_AMOUNT = 100;
    
    // Player management
    protected List<PlayerController> players;
    [SerializeField] protected PlayerController Player1Ref;

    // Fish management
    protected List<FishController> fishes;

    // Camera management
    [SerializeField] public CameraController gameCamera;

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
        Setup();
	}

    #region Getter and setters

    #region Button handlers
    #region Button A handler
    public bool ButtonA_Down
    {
        get { return Input.GetKeyDown(KeyCode.Space); }
    }

    public bool ButtonA_Up
    {
        get { return Input.GetKeyUp(KeyCode.Space); }
    }

    public bool ButtonA_Hold
    {
        get { return Input.GetKey(KeyCode.Space); }
    }
    #endregion

    #region Button B handler
    public bool ButtonB_Down
    {
        get { return Input.GetKeyDown(KeyCode.E); }
    }

    public bool ButtonB_Up
    {
        get { return Input.GetKeyUp(KeyCode.E); }
    }

    public bool ButtonB_Hold
    {
        get { return Input.GetKey(KeyCode.E); }
    }
    #endregion
    #endregion

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
    #endregion

    #region Helper functions
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
        timeTillNextPayment = PAYMENT_INTERVAL;
        gameCamera = GetComponentInChildren<CameraController>();
        players = new List<PlayerController>();
        players.Add(Player1Ref);
        Player1Ref.AssignCameraToPlayer(gameCamera);
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
            RemoveMoney(PAYMENT_AMOUNT);
            timeTillNextPayment = PAYMENT_INTERVAL;
        }

        timeTillNextPayment -= Time.deltaTime;

        moneyText.text = "$" + currentMoney.ToString();
        timeLeftText.text = "Time Left: " + (int)timeTillNextPayment + "s";
    }

    void Update()
    {
        // Handle the update loops for the others too

		Player1.GameUpdate();
    }
    #endregion
}
