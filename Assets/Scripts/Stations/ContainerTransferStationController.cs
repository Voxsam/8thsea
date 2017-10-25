using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerTransferStationController : IInteractable
{
    enum State
    {
        Idle
    };

    //Public editor variables
    [SerializeField]
    public GameObject[] containersSource;
    [SerializeField]
    public GameObject[] containersDestination;

    //Private variables
    private State currentState;

    // Use this for initialization
    void Start()
    {
        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
    }

    override public void Interact()
    {
    }

    override public void Interact(GameObject otherActor)
    {
        if (otherActor.tag == "Player")
        {
            if (currentState == State.Idle)
            {
                for (int sourceContainerIterator = 0; sourceContainerIterator < containersSource.Length; sourceContainerIterator++)
                {
                    ContainerStationController sourceContainerControllerScript = (ContainerStationController)containersSource[sourceContainerIterator].GetComponent(typeof(ContainerStationController));

                    GameObject heldObject = sourceContainerControllerScript.removeHeldObject();
                    bool successfulTransfer = true;
                    if (heldObject != null)
                    {
                        for (int destinationContainerIterator = 0; destinationContainerIterator < containersDestination.Length; destinationContainerIterator++)
                        {
                            heldObject.transform.SetParent(null);
                            ContainerStationController destinationContainerControllerScript = (ContainerStationController)containersDestination[destinationContainerIterator].GetComponent(typeof(ContainerStationController));
                            successfulTransfer = destinationContainerControllerScript.holdObject(heldObject);
                            if (successfulTransfer)
                            {
                                break;
                            }
                        }
                        if (!successfulTransfer)
                        {
                            sourceContainerControllerScript.holdObject(heldObject);
                        }
                    }
                }
            }
        }
    }

    override public void ToggleHighlight(bool toggle = true)
    {
    }
}
