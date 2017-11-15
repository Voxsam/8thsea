using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishReturnStationController : IInteractable
{
    public SubNavigationStationController subNavStationController;
    public float minReturnDistance;

    public enum State
    {
        Empty,
        Inactive,
        Active
    };
    private State currentState;
    public State CurrentState
    {
        get
        {
            return currentState;
        }
    }

    public GameObject releaseSlot;
    public Renderer meshRenderer;
    public GameObject buttonObject;
    public AnimationCurve buttonDepressCurve;

    private Material lightMaterial;
    private FishController fishController;

    // Use this for initialization
    void Start () {
        currentState = State.Empty;
        lightMaterial = meshRenderer.materials[1];
        lightMaterial.SetColor("_EmissionColor", Color.red);
    }
	
	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case State.Empty:
                break;
            case State.Inactive:
                if (fishController != null )
                {
                    if (Vector3.Distance (transform.position, fishController.CaughtPosition) < minReturnDistance)
                    {
                        lightMaterial.SetColor("_EmissionColor", Color.green);
                        currentState = State.Active;
                    }
                }
                break;
            case State.Active:
                if (fishController != null)
                {
                    if (Vector3.Distance(transform.position, fishController.CaughtPosition) > minReturnDistance)
                    {
                        lightMaterial.SetColor("_EmissionColor", Color.red);
                        currentState = State.Inactive;
                    }
                }
                break;
            default:
                break;
        }
    }

    override public void Interact()
    {
    }

    override public void Interact(GameObject otherActor)
    {
        if (otherActor.tag == "Player")
        {
            if (currentState == State.Empty)
            {
                
            }
            else if (currentState == State.Inactive)
            {
                
            }
            else if (currentState == State.Active)
            {
                //Dead fishes cannot be put back to the sea.
                if (!fishController.IsDead())
                {

					if (GameController.Obj.isTutorial) {
						if (TutorialManager.Obj.currentStep == 11) {
							TutorialManager.Obj.hasReleasedFish = true;
						}
					}

                    //Housekeeping for changing a fish back into "swim" mode.
                    fishController.PutDown();
                    fishController.SetEnabled(false);
                    fishController.fishMovementController.SetEnabled(true);
                    fishController.fishMovementController.Initialise();

                    fishController.FishSchoolController.AddFishToSchool(fishController.gameObject);
                    GameObject fishObject = subNavStationController.removeHeldObject();
                    fishObject.transform.position = releaseSlot.transform.position;
                    fishObject.transform.SetParent(null);
                    fishController = null;

                    subNavStationController.labNavController.SetWireframe(GameData.FishType.None);

                    Deactivate();
                }
            }
        }
    }

    override public void ToggleHighlight(PlayerController otherPlayerController, bool toggle = true)
    {
    }

    public void Activate ( FishController otherFishController )
    {
        fishController = otherFishController;
        currentState = State.Inactive;
        lightMaterial.SetColor("_EmissionColor", Color.red);
    }

    public void Deactivate ()
    {
        fishController = null;
        currentState = State.Empty;
        lightMaterial.SetColor("_EmissionColor", Color.red);
    }
}
