using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialColliderController : MonoBehaviour {

	public enum Location {
		Submarine_Entry
	}

	public Location colliderLocation;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			switch (colliderLocation) {
			case Location.Submarine_Entry:
				if (TutorialManager.Obj.currentStep == 3) {
					TutorialManager.Obj.hasEnteredSubmarine = true;
					gameObject.SetActive (false);
				}
				break;
			default:
				break;
			}


		}

	}
}
