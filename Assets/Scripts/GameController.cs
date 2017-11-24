using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController Obj;

	public TutorialManager tutManager;
	public bool isTutorial;

    // Audio Management
    public static AudioController Audio
    {
        get { return AudioController.Obj; }
    }

    public bool disablePlayersUntilLoadingIsDone = true;
    public bool allowBGM = true;
    public bool allowSFX = true;
    public AudioController.BGM bgmToStart = AudioController.BGM.None;
    public float bgmMaximumVolume = 0.8f;
    public float sfxMaximumVolume = 1.0f;

    // Submarine Management
    public static SubmarineController SubmarineRef
    {
        get { return SubmarineController.Obj; }
    }

    // Player management
    protected List<PlayerController> playerControllers;
    protected List<Player> players;
    [SerializeField] protected Transform PlayerHolder;
    [SerializeField] protected PlayerController Player1Ref;
    
    /// <summary>
    /// Gets player with the following number. If number is not valid, returns null
    /// </summary>
    public Player GetPlayerNumber(int num)
    {
        foreach(Player player in players)
        {
            if (player.playerNumber == num)
            {
                return player;
            }
        }

        return null;
    }

    // Fish management
    protected List<FishController> fishes;
    [SerializeField] protected Transform FishHolder;
    [SerializeField] public Transform AquariumHolder;

    // Camera management
    public CameraController gameCamera;

    //This is ONLY t set up the MultiplayerManager singleton. DO NOT REFERENCE AND USE MultiplayerManager.Obj INSTEAD.
	public MultiplayerManager multiplayerManager;

    // GameObjects
    [SerializeField] protected Canvas mainCanvas;
    [SerializeField] protected Transform Sea;
    [SerializeField] protected Transform Lab;

    // Randomisation
    public static System.Random RNG;

    public Text GameOverText;

    // Money
	public Text moneyText;
	public float currentMoney;


    #region Level management
    public int CurrentLevel = 1; // Start at level 1
    public float LevelClearPercentage = 0f;
    [SerializeField] private Text LevelProgressionPercentageText;
    [SerializeField] private Text LevelHasFinishedText;

    /// <summary>
    /// Loads the next level in the list. If there is no next level, it reloads the current level.
    /// If levelToLoad is a valid level from 1 to GameData.TOTAL_NUMBER_OF_LEVELS (inclusive), then it will load that level instead
    /// </summary>
    public void LoadNextLevel(int levelToLoad = 0)
    {
        // Only increment if it is not the last level
        if (levelToLoad >= 0 && levelToLoad < GameData.TOTAL_NUMBER_OF_LEVELS)
        {
            CurrentLevel = levelToLoad;
        }
        else if (CurrentLevel != GameData.TOTAL_NUMBER_OF_LEVELS)
        {
            CurrentLevel++;
        }
        
        // Reload this scene
        SceneManager.LoadScene("main_merged");
    }

    /// <summary>
    /// Cross checks the required number of fish to catch and counts the number of fish current in the aquarium, then returns the percentage
    /// </summary>
    /// <returns></returns>
    private float GetLevelClearedPercentage()
    {
        GameData.FishType[] req = GameData.GetResearchRequirementsForLevel(CurrentLevel);
        int sumCollected = 0;
        int totalToCollect = 0;
        foreach(GameData.FishType fish in req)
        {
            int cap = GameData.GetFishParameters(fish).totalToResearch;

            int fishCaught = GameData.GetFishParameters(fish).totalResearched;
            totalToCollect += cap;

            if (fishCaught > cap)
            {
                fishCaught = cap;
            }
            sumCollected += fishCaught;
        }

        // In case some things are not initialised yet
        if (totalToCollect == 0)
        {
            return 0f;
        }

        return sumCollected / (float)totalToCollect * 100f;
    }

    /// <summary>
    /// Update the gamescreen with the progress info
    /// </summary>
    public void UpdateLevelProgression()
    {
        if (LevelProgressionPercentageText != null)
        {
            LevelClearPercentage = GetLevelClearedPercentage();
            LevelProgressionPercentageText.text = Mathf.RoundToInt(LevelClearPercentage).ToString() + "%";
        }
    }
    #endregion

    void Awake() {
        if (Obj == null)
        {
            Obj = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

		if (isTutorial) {
			tutManager = GameObject.FindGameObjectWithTag ("Tutorial Manager").GetComponent<TutorialManager> ();
			GameObject.FindGameObjectWithTag ("Tutorial Canvas").GetComponent<MainCanvasController> ().Setup ();
			tutManager.Setup ();
        }

        if (!isTutorial)
        {
            CurrentLevel = Random.Range(0, GameData.TOTAL_NUMBER_OF_LEVELS) + 1;
        }
        
        // GameController object should not be destructable
        DontDestroyOnLoad(this.gameObject);
	}

    void Start()
    {
        multiplayerManager.Setup();
        Setup();
    }

    #region Getter and setters
    /// <summary>
    /// Sets whether the players can move or not. If true, players are permitted to move, else they are not.
    /// </summary>
    private void SetMovementForPlayers(bool active)
    {
        foreach (Player player in players)
        {
            player.controller.IsPlayerAllowedToMove = active;
        }
    }

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
        foreach(Player player in players)
        {
            if (collider.gameObject == player.controller.gameObject)
            {
                return player.controller;
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
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("tutorial")) { // change later
			tutManager = GameObject.FindGameObjectWithTag ("Tutorial Manager").GetComponent<TutorialManager> ();
			tutManager.Setup ();
		}
        gameCamera = GetComponentInChildren<CameraController>();
        
        //playerControllers = MultiplayerManager.Obj.playerControllerList;
        //players = multiplayerManager.playerControllerList;
        fishes = new List<FishController>();

        /*
        players.Add(Player1Ref);
        Player1Ref.AssignCameraToPlayer(gameCamera);
        Player1Ref.transform.SetParent(PlayerHolder);
        */

        mainCanvas.transform.SetParent(this.transform);
        Sea.transform.SetParent(this.transform);
        Lab.transform.SetParent(this.transform);

        currentMoney = GameData.STARTING_MONEY;
        GameOverText.enabled = false;
        LevelHasFinishedText.enabled = false;
        
        UpdateLevelProgression();
        RNG = new System.Random();
        moneyText.text = "";

        if (isTutorial)
        {
            LevelProgressionPercentageText.enabled = false;
        }

        if (disablePlayersUntilLoadingIsDone)
        {
            SetMovementForPlayers(false);
        }
        else
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (Audio.AreClipsLoaded)
        {
            Audio.PlayBGM(Audio.CurrentBGM);
        }
        SetMovementForPlayers(true);
    }
    #endregion

    #region Update handlers
    /// <summary>
    /// GameController's own update handler. Separate it in case some things want to go into FixedUpdate instead of Update or vice versa
    /// </summary>
    protected void GameUpdate()
    {
        // Disable the money function
        
        //currentMoney -= GameData.MONEY_DEPLETE_RATE * Time.deltaTime;
        //moneyText.text = "$" + ( (int)currentMoney).ToString();

        if (LevelClearPercentage >= 100f)
        {
            if (!LevelHasFinishedText.isActiveAndEnabled)
            {
                LevelHasFinishedText.enabled = true;
            }

            foreach(Player player in players)
            {
                if (player.controls.GetMenuKeyDown())
                {
                    FullGameReset();
                }
            }
        }
    }

    public void FullGameReset()
    {
        GameObject newGameObject = new GameObject();
        this.transform.SetParent(newGameObject.transform);
        foreach(Player player in players)
        {
            Destroy(player.controller.gameObject);
        }

        Destroy(PlayerList.Obj.gameObject);
        Destroy(multiplayerManager.gameObject);
        SceneManager.LoadScene("main_menu");
    }

    void Update()
    {
        // Handle the update loops for the others too
        foreach (Player player in players)
        {
            player.controller.GameUpdate();
        }

        GameUpdate();

        /*
        if (currentMoney > 0)
        {
            // Handle the update loops for the others too
            foreach (Player player in players)
            {
                player.controller.GameUpdate();
            }
        }
        else
        {
            if (!GameOverText.isActiveAndEnabled)
            {
                GameOverText.enabled = true;
            }
        }//*/
    }
    #endregion


	public void SetPlayers (List<Player> pList) {
		players = pList;
	}
}
