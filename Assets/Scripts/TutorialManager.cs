using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	public static TutorialManager Obj;

	public Transform[] transforms;

	public int currentStep = 0;

	public bool hasUsedTeleporter = false;
	public bool notifiedPlayerOfTutorialAccess = false;

	public GameObject teleporter;
	public GameObject aquariumConsole;

	public GameObject floatingArrowPrefab;
	private GameObject arrow;

	public void Setup() {

		Obj = this;

	}


	void Update () {

		switch (currentStep) {

		default:
			break;

		case 0:
			print ("Instantiating arrow over teleporter");
			arrow = Instantiate (floatingArrowPrefab, teleporter.transform.position + new Vector3 (0, 6f, 0), Quaternion.identity);
			MainCanvasController.Obj.SetCenterPanelText("Welcome to the tutorial!",
				"The goal of this game is to research all the fish required to pass the level.\n\n" +
				"Let's see what fish we need to complete this level! Take the <color=yellow>teleporter</color> " +
				"to the upper floor of the lab by pressing the <color=red>A button</color>.");
			currentStep++;
			break;

		case 1:
			print ("Waiting for any player to use teleporter");
			if (hasUsedTeleporter) {
				print ("Teleporter used");
				currentStep++;
				MainCanvasController.Obj.SetCenterPanelText ("The Aquarium",
				"The aquarium on the second floor of the lab shows the fish species you have to research to complete the level. " +
				"Some fish species require you to perform research on more than one fish to be considered fully-researched. \n\n" +
				"In this case, we only need one <color=red>Clown Fish</color> to proceed. Let's go catch some fish! Head to the " +
				"<color=yellow>submarine</color>, which is connected to the bottom floor of the lab.");
				MainCanvasController.Obj.ShowCenterPanel ();
				RepositionArrow (transforms [0]);
			}
			break;
		
		case 3:
			print ("Waiting for player to enter submarine");
			

			break;
		}

	}


	private void RepositionArrow(Transform location) {
		arrow.SetActive (false);
		arrow.transform.SetPositionAndRotation(location.position, location.rotation);
		arrow.SetActive(true);
	}

}


