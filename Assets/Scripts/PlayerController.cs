using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 10;
	public float turnSpeed = 7;

    private GameData.ControlType controlMode;
    private PlayerInteractionController interactionController;
    private PlayerAnimationController animationController;
    public CameraController cameraController;
	public PlayerCameraController pCameraController;

	public Player player;

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

    [SerializeField] public Rigidbody rb;



	void Start () {
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
        get { return this.transform.parent; }
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
		
    public void AssignCameraToPlayer(CameraController cc)
    {
        cameraController = cc;
        ReattachCameraToPlayer();
    }

    public void ReattachCameraToPlayer()
    {
        cameraController.SetCameraToObject(this.gameObject);
    }
    #endregion

    public void GameUpdate ()
    {

        interactionController.GameUpdate();

        if (ControlMode == GameData.ControlType.CHARACTER && IsPlayerAllowedToMove)
        {
			
			Vector3 direction = new Vector3 (player.controls.GetHorizontalAxis(), 0.0f, player.controls.GetVerticalAxis());
            
			if (direction != Vector3.zero) {
				isMoving = true;
				transform.rotation = Quaternion.Slerp (
					transform.rotation,
					Quaternion.LookRotation (direction),
					Time.deltaTime * turnSpeed
				);

				transform.Translate (new Vector3 (0, 0, movementSpeed / 100f));

			} else {

				isMoving = false;

			}
		}

        animationController.GameUpdate();
	}

}
