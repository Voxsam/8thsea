using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

	public static TutorialManager Obj;

	public Transform[] transforms;

	public int currentStep = 0;

	public bool hasUsedTeleporter = false;
	public bool hasActivatedAquarium = false;
	public bool hasEnteredSubmarine = false;
	public bool hasTriedDriving = false;
	public bool hasMovedNearFish = false;
	public bool notifiedPlayerOfTutorialAccess = false;

	public GameObject teleporter;
	public GameObject aquariumConsole;
	public GameObject drivingStation;
	public GameObject suckStation;
	public GameObject tutorialFishZone;

	public GameObject floatingArrowPrefab;
	private GameObject arrow;

	public void Setup ()
	{
		Obj = this;
		gameObject.SetActive (true);
	}



	void Update ()
	{

		switch (currentStep) {

		default:
			break;

		case 0:
			print ("Instantiating arrow over teleporter");
			arrow = Instantiate (floatingArrowPrefab, teleporter.transform.position + new Vector3 (0, 6f, 0), Quaternion.identity);
			currentStep++;
			MainCanvasController.Obj.SetCenterPanelText ("Welcome to the tutorial!",
				"The goal of this game is to research all the fish required to pass the level.\n\n" +
				"First, let's look at the Aquarium. Take the <color=#ffe000ff>teleporter</color> " +
				"to the upper floor of the lab by pressing the <color=red>A button</color>.");
			
			break;

		case 1:
			print ("Waiting for any player to use teleporter");
			if (hasUsedTeleporter) {
				print ("Teleporter used");
				currentStep++;
				MainCanvasController.Obj.SetCenterPanelText ("The Aquarium",
					"Let's see what fish we need to complete this level. Press the <color=red>A button</color> " +
					"next to the aquarium console for a better view.");
				MainCanvasController.Obj.ShowCenterPanel ();
				RepositionArrow (aquariumConsole.transform.position + new Vector3 (0, 6f, 0));
			}
			break;

		case 2:
			if (hasActivatedAquarium) {
				print ("Aquarium activated");
				currentStep++;
				MainCanvasController.Obj.SetCenterPanelText ("The Aquarium",
					"Some fish species require you to perform research on more than one fish to be considered fully-researched. " +
					"In this case, we only need one <color=red>Clown Fish</color> to proceed. \n\n" +
					"Let's go catch some fish! Press the <color=blue>B button</color> to leave the aquarium, " +
					"then head to the <color=#ffe000ff>submarine</color>, which is connected to the bottom floor of the lab.");
				MainCanvasController.Obj.ShowCenterPanel ();
				RepositionArrow (transforms [0]);
			}
			break;
		
		case 3:
			print ("Waiting for player to enter submarine");
			if (hasEnteredSubmarine) {
				currentStep++;
				MainCanvasController.Obj.SetCenterPanelText ("The Submarine",
					"We need to drive the submarine around the ocean to find the fish we need. Press the " +
					"<color=red>A button</color> at the steering wheel to drive the submarine.");
				MainCanvasController.Obj.ShowCenterPanel ();
				RepositionArrow (drivingStation.transform.position + new Vector3 (0, 6f, 0));
			} 
			break;

		case 4:
			print ("Waiting for player to drive");
			if (hasTriedDriving) {
				currentStep++;
				MainCanvasController.Obj.SetCenterPanelText ("Explore!",
					"Drive the submarine using the left stick. \n\n" +
					"Normally, you'll have to search for the fish you need on your own -- or get your friend to do it for you -- " +
					"but I have some insider info about where the fish we want is. Follow the <color=#ff6699ff>pink</color> arrow! \n\n" +
					"When you see a fish, activate the vacuum tube station to suck it into the submarine!");
				MainCanvasController.Obj.ShowCenterPanel ();
				arrow.SetActive (false);
				arrow.transform.SetParent (suckStation.transform);
				arrow.transform.localPosition = new Vector3 (0, 6f, 0);
			} 
			break;

		case 5:
			print ("Waiting for player to drive");
			if (hasMovedNearFish) {
				currentStep++;
			}
			break;
		}

	}

	// Only for static objects
	private void RepositionArrow (Vector3 position)
	{
		if (arrow == null) { arrow = Instantiate (floatingArrowPrefab); }
		arrow.SetActive (false);
		arrow.transform.position = position;
		arrow.transform.rotation = Quaternion.identity;
		arrow.SetActive (true);
	}

	private void RepositionArrow (Transform location)
	{
		if (arrow == null) { arrow = Instantiate (floatingArrowPrefab); }
		arrow.SetActive (false);
		arrow.transform.SetPositionAndRotation (location.position, location.rotation);
		arrow.SetActive (true);
	}

		
}


