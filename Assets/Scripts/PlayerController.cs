using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public string playerName;

	public float movementSpeed = 10;
	public float turnSpeed = 7;

    private GameData.ControlType controlMode;
    private PlayerInteractionController interactionController;
    public CameraController cameraController;
	public Camera cam;

	public GameObject model;


	// Controls
	private KeyCode fire1Button;
	private string horizontalAxis;
	private string verticalAxis;


	[SerializeField] public Rigidbody rb;

	void Start () {
        //rb = this.GetComponent<Rigidbody> (); // Assigned in editor
        ControlMode = GameData.ControlType.CHARACTER;
        interactionController = GetComponentInChildren<PlayerInteractionController>();

		SetControls ();
    }

    public GameData.ControlType ControlMode
    {
        get { return controlMode; }
        private set { controlMode = value; }
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

    public void GameUpdate () {

		
		if (ControlMode == GameData.ControlType.CHARACTER) {
			Vector3 direction = new Vector3 (Input.GetAxis (horizontalAxis), 0, Input.GetAxis (verticalAxis));
            
            if (direction != Vector3.zero) {

				transform.rotation = Quaternion.Slerp (
					transform.rotation,
					Quaternion.LookRotation (direction),
					Time.deltaTime * turnSpeed
				);

				transform.Translate (new Vector3 (0, 0, movementSpeed / 100f));
			}
            
		}
			
	}
		

	#region Button handlers
	public bool Fire1Down() {
		return Input.GetKeyDown (fire1Button);
	}
	public bool Fire1Up() {
		return Input.GetKeyUp (fire1Button);
	}
	public bool Fire1Hold() {
		return Input.GetKey (fire1Button);
	}
	#endregion



	// Sets the appropriate controls for P1/P2 in a very stupid way, WILL BE CHANGED
	void SetControls() {
		if (playerName == "P1") {
			horizontalAxis = "Horizontal_P1";
			verticalAxis = "Vertical_P1";
			fire1Button = KeyCode.Space;
		} else {
			horizontalAxis = "Horizontal_P2";
			verticalAxis = "Vertical_P2";
			fire1Button = KeyCode.E;
		}
	}

}
