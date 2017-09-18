using UnityEngine;
using System.Collections;

//Interface for 
public interface IInteractable
{
    void Interact();
    void Interact(GameObject otherActor);

    void ToggleHighlight(bool toggle = true);
}