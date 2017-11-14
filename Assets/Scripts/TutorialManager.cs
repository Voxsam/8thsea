using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	public static TutorialManager Obj;

	public int currentStep = 0;

	public bool hasUsedTeleporter = false;

	public void Setup() {

		Obj = this;

	}


	void Update () {

		switch (currentStep) {

		default:
			break;

		case 0:
			print ("Waiting on step 0");
			if (hasUsedTeleporter) {
				print ("Teleporter used");
				currentStep++;
			}
			break;

		case 1:
			print ("Waiting on step 1");
			break;
			


		}

	}

}
