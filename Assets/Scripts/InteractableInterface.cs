using UnityEngine;
using System.Collections;

//Interface for 
public abstract class IInteractable : MonoBehaviour
{
    public abstract void Interact();
    public abstract void Interact(GameObject otherActor);
    public abstract void ToggleHighlight(PlayerController otherPlayerController, bool toggle = true);
}