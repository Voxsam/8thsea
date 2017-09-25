using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StationControllerInterface : MonoBehaviour {
    public abstract GameData.ControlType ControlMode {
        get;
    }

    public bool isActivated = false; // False by default
    
    /// <summary>
    /// The switch condition that checks whether the player can return to the Character controller
    /// </summary>
    public abstract bool SwitchCondition();
    public PlayerController playerInStation = null;
    protected CameraController stationCamera = null;

    void Start()
    {
    }

    /// <summary>
    /// Swap the player's control to the station
    /// </summary>
    /// <param name="player"></param>
    public virtual void SetPlayerToStation(PlayerController player)
    {
        playerInStation = player;
        playerInStation.RequestControlChange(ControlMode);
        isActivated = true;
        SetStationCamera(playerInStation.cameraController);
    }

    /// <summary>
    /// Releasea the player from the station
    /// </summary>
    public virtual void ReleasePlayerFromStation()
    {
        if (playerInStation != null)
        {
            playerInStation.ReturnControlToCharacter();
            playerInStation.ReattachCameraToPlayer();
            playerInStation = null;
            stationCamera = null;
            isActivated = false;
        }
    }

    public void SetStationCamera(CameraController cc)
    {
        stationCamera = cc;
    }
}
