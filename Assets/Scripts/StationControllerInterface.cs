using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StationControllerInterface : IInteractable {
    public abstract GameData.ControlType ControlMode {
        get;
    }

    private bool isActivated = false; // False by default
    public virtual bool IsActivated
    {
        get { return isActivated; }
        set { isActivated = value; }
    }
    
    public PlayerController playerInStation = null;
    protected CameraController stationCamera = null;
    
    /// <summary>
    /// The switch condition that checks whether the player can return to the Character controller
    /// </summary>
    public abstract bool SwitchCondition();

    /// <summary>
    /// A function that the StationController will call after activating the station
    /// </summary>
    public abstract void WhenActivated();

    /// <summary>
    /// A function that the StationController will call after deactivating the station
    /// </summary>
    public abstract void WhenDeactivated();

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
            isActivated = false;
            playerInStation.ReturnControlToCharacter();
            playerInStation.ReattachCameraToPlayer();
            playerInStation = null;
            stationCamera = null;
        }
    }

    public void SetStationCamera(CameraController cc)
    {
        stationCamera = cc;
    }
}
