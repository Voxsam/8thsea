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
	public bool hasCaughtFish = false;
	public bool hasUsedDissectStation = false;
	public bool hasCompletedFish = false;
	public bool hasReleasedFish = false;
	public bool notifiedPlayerOfTutorialAccess = false;

	public GameObject teleporter;
	public GameObject aquariumConsole;
	public GameObject drivingStation;
	public GameObject suckStation;
	public GameObject tutorialFishZone;
	public GameObject dissectionStation;
	public GameObject photoStation;
	public GameObject researchStation;
	public GameObject submarineLever;

	public GameObject floatingArrowPrefab;
	private GameObject arrow;
	private GameObject pinkArrow;

	private Vector3 heightOffset = new Vector3 (0, 6f, 0);

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
			arrow = Instantiate (floatingArrowPrefab, teleporter.transform.position + heightOffset, Quaternion.identity);
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
					"Let's see what fish we need to complete this level. Press the \n\n <color=red>A button</color> " +
					"next to the aquarium console for a better view.");
				MainCanvasController.Obj.ShowCenterPanel ();
				RepositionArrow (aquariumConsole.transform.position + heightOffset);
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
					"We need to drive the submarine around the ocean to find the fish we need. Press the \n" +
					"<color=red>A button</color> at the steering wheel to drive the submarine. \n\n" +
					"(If you're in multiplayer, <b>leave one person behind in the lab!</b> You'll need their help later!)");
				MainCanvasController.Obj.ShowCenterPanel ();
				RepositionArrow (drivingStation.transform.position + heightOffset);
			} 
			break;

		case 4:
			print ("Waiting for player to drive");
			if (hasTriedDriving) {
				currentStep++;
				MainCanvasController.Obj.SetCenterPanelText ("Exploring the Sea",
					"Drive the submarine using the left stick. Pay attention to the <color=red>Energy</color> bar at the top. When it " +
					"reaches zero, you won't be able to activate anything in the submarine until you head back to the " +
					"lab to refuel. \n\n" +
					"Normally, you'll have to search for the fish you need on your own -- or get your friend to do it for you -- " +
					"but I have some insider info about where the fish we want is. Follow the <color=#ff6699ff>pink</color> arrow!");
				MainCanvasController.Obj.ShowCenterPanel ();
				MainCanvasController.Obj.ShowBoxOutline (5.0f);
				arrow.SetActive (false);
				pinkArrow = Instantiate (floatingArrowPrefab);
				pinkArrow.SetActive (false);
				pinkArrow.GetComponent<SpriteRenderer> ().color = new Color (1f, 0.26f, 0.39f, 1f);
				pinkArrow.transform.SetParent (drivingStation.transform);
				pinkArrow.transform.localPosition = new Vector3 (0, 0, 6f);
				pinkArrow.SetActive (true);
			} 
			break;

		case 5:
			print ("Waiting for player to reach fish");
			pinkArrow.transform.rotation = Quaternion.AngleAxis (Mathf.Atan2 (pinkArrow.transform.position.y - tutorialFishZone.transform.position.y,
				pinkArrow.transform.position.x - tutorialFishZone.transform.position.x) * Mathf.Rad2Deg - 90f,
				new Vector3 (0, 0, 1f));
			if (hasMovedNearFish) {
				currentStep++;
				pinkArrow.SetActive (false);
				arrow.transform.SetParent (suckStation.transform);
				arrow.transform.localPosition = new Vector3 (0, 3f, 0);
				arrow.SetActive (true);
				MainCanvasController.Obj.SetCenterPanelText ("Catching Fish",
					"Activate the vacuum tube station. Manipulate the vacuum tube with the left stick. When a fish is in the center " +
					"of the tube, press the <color=red>A button</color> to attempt to catch it.");
				MainCanvasController.Obj.ShowCenterPanel ();
			}
			break;

		case 6:
			print ("Waiting for player to use first research station");
			if (hasCaughtFish) {
				currentStep++;
				pinkArrow.SetActive (false);
				arrow.transform.SetParent (suckStation.transform);
				arrow.transform.localPosition = new Vector3 (0, 3f, 0);
				arrow.SetActive (true);
				MainCanvasController.Obj.SetCenterPanelText ("How to Research",
					"");
				MainCanvasController.Obj.ShowCenterPanel ();
			}
			break;

		case 7:
			print ("Waiting for player to transfer fish");
			if (hasCaughtFish) {
				currentStep++;
				pinkArrow.SetActive (false);
				RepositionArrow (submarineLever.transform.position + heightOffset);
				MainCanvasController.Obj.SetCenterPanelText ("How to Research",
					"The bar over a caught fish is its <color=red>Panic Timer</color> from being out of its natural habitat. " +
					"Once a fish's Panic Timer reaches zero, it dies of stress and must be thrown away in the lab's bin. \n\n" +
					"The icons below the fish's name are the steps needed to research the fish. We need to bring this fish to the " +
					"<color=blue>Dissection Station</color> first. \n\n" +
					"This fish seems pretty chill, but you won't usually have time to drive the submarine back to the lab before it kicks the bucket. " +
					"Luckily, the submarine and lab have technology that let you teleport fish between the two places. Neat, huh? \n\n" +
					"Put the fish in one of the tanks in the lower floor of the submarine and pull the lever with the <color=red>A button</color>.");
				MainCanvasController.Obj.ShowCenterPanel ();
			}
			break;

		case 8:
			print ("Waiting for player to use first research station");
			if (hasUsedDissectStation) {
				currentStep++;
				pinkArrow.SetActive (false);
				RepositionArrow (submarineLever.transform.position);
				MainCanvasController.Obj.SetCenterPanelText ("How to Research",
					"The bar over a caught fish is its <color=red>Panic Timer</color> from being out of its natural habitat. " +
					"Once a fish's Panic Timer reaches zero, it dies of stress and must be thrown away in the lab's bin. \n\n" +
					"The icons below the fish's name are the steps needed to research the fish. We need to bring this fish to the " +
					"<color=blue>Dissection Station</color> first. \n\n" +
					"This fish seems pretty chill, but you won't usually have time to drive the submarine back to the lab before it kicks the bucket. " +
					"Luckily, the submarine and lab have technology that let you teleport fish between the two places. Neat, huh? \n\n" +
					"Put the fish in one of the tanks in the lower floor of the submarine and pull the lever with the <color=red>A button</color>.");
				MainCanvasController.Obj.ShowCenterPanel ();
			}
			break;
	}


	}

	// Only for static objects
	private void RepositionArrow (Vector3 position)
	{
		if (arrow == null) {
			arrow = Instantiate (floatingArrowPrefab);
		}
		arrow.SetActive (false);
		arrow.transform.SetParent (null);
		arrow.transform.position = position;
		arrow.transform.rotation = Quaternion.identity;
		arrow.SetActive (true);
	}

	private void RepositionArrow (Transform location)
	{
		if (arrow == null) {
			arrow = Instantiate (floatingArrowPrefab);
		}
		arrow.SetActive (false);
		arrow.transform.SetParent (null);
		arrow.transform.SetPositionAndRotation (location.position, location.rotation);
		arrow.SetActive (true);
	}
		
}


