using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 10;
	public float turnSpeed = 7;

    private GameData.ControlType controlMode;
    public PlayerInteractionController interactionController;
    private PlayerAnimationController animationController;
	public PlayerCameraController pCameraController;
    public AudioSource audioOutput;

	public Player playerMasterRef;
	public ControlScheme controls;
	public PlayerCanvasController canvas;

    private bool isMoving = false;
    private bool isPlayerAllowedToMove = true;

    public bool IsPlayerMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }
    public bool IsPlayerAllowedToMove
    {
        get { return isPlayerAllowedToMove; }
        set { isPlayerAllowedToMove = value; }
    }
    
    [SerializeField] public Transform playerHolder;
    [SerializeField] public Rigidbody rb;

	void Awake () {
        //rb = this.GetComponent<Rigidbody> (); // Assigned in editor
        ControlMode = GameData.ControlType.CHARACTER;
        interactionController = GetComponentInChildren<PlayerInteractionController>();
        animationController = GetComponentInChildren<PlayerAnimationController>();

		isMoving = false;

    }

    public GameData.ControlType ControlMode
    {
        get { return controlMode; }
        private set { controlMode = value; }
    }

    /// <summary>
    /// Returns the location where the player is in
    /// </summary>
    public Transform LocationRef
    {
        // Since the player will always be a child of its location, return the parent of the player as its location
        get { return transform.parent; }
        set { transform.SetParent(value); }
    }

    #region Control handlers
    /// <summary>
    /// For other objects to request the player change its control to a certain object type.
    /// </summary>
    /// <param name="changeTo">The control type that the caller wishes the Player to change to</param>
    public void RequestControlChange(GameData.ControlType changeTo)
    {
        // if there are no problems (aka insert checks here), change to that control type
        ControlMode = changeTo;
    }

    public void ReturnControlToCharacter()
    {
        ControlMode = GameData.ControlType.CHARACTER;
    }
		
    public void AssignCameraToPlayer(PlayerCameraController cc)
    {
        pCameraController = cc;
        ReattachCameraToPlayer();
    }

    public void ReattachCameraToPlayer()
    {
        pCameraController.SetCameraToObject(this.gameObject);
    }
    #endregion

    public void GameUpdate ()
    {

        interactionController.GameUpdate();
        if (ControlMode == GameData.ControlType.CHARACTER && IsPlayerAllowedToMove)
        {
			Vector3 direction = new Vector3 (playerMasterRef.controls.GetHorizontalAxis(), 0.0f, playerMasterRef.controls.GetVerticalAxis());
            
			if (direction != Vector3.zero) {
				isMoving = true;
			    playerHolder.rotation = Quaternion.Slerp (
					playerHolder.rotation,
					Quaternion.LookRotation (direction),
					Time.deltaTime * turnSpeed
				);

				transform.Translate(new Vector3 (0, 0, movementSpeed / 100f), playerHolder);

			} else {

				isMoving = false;

			}
		}

        animationController.GameUpdate();
	}

	public ControlScheme GetPlayerControls
	{
		get { return playerMasterRef.controls; }
	}


	// For use with MultiplayerManager
	public void Setup (Player p, PlayerCameraController camCtrl, PlayerCanvasController pc) {

		playerMasterRef = p;
        p.controller = this;
		pCameraController = camCtrl;
		pCameraController.player = this;
		controls = p.controls;
		canvas = pc;
		canvas.Setup (camCtrl.cam);
	}

}
