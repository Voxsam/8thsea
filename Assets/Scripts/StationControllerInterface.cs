using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StationControllerInterface : MonoBehaviour {
    public abstract GameController.ControlType ControlMode {
        get;
    }

    public bool isActivated = false; // False by default
    
    /// <summary>
    /// The switch condition that checks whether the player can return to the Character controller
    /// </summary>
    public abstract bool SwitchCondition();
    public PlayerController playerInStation = null;
    public CameraController stationCamera = null;

    void Start()
    {
        stationCamera.AssignCameraToObject(gameObject);
    }

    /// <summary>
    /// Swap the player's control to the station
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayerToStation(PlayerController player)
    {
        playerInStation = player;
        playerInStation.RequestControlChange(ControlMode);
        isActivated = true;
    }

    /// <summary>
    /// Releasea the player from the station
    /// </summary>
    public void ReleasePlayerFromStation()
    {
        if (playerInStation != null)
        {
            playerInStation.ReturnControlToCharacter();
            playerInStation = null;
            isActivated = false;
        }
    }
}
