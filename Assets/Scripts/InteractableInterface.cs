using UnityEngine;
using System.Collections;

//Interface for 
public interface IInteractable
{
    void interact();
    void interact(GameObject otherActor);

    void toggleHighlight(bool toggle = true);
}