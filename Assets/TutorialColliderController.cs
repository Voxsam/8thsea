using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialColliderController : MonoBehaviour {

	public enum Location {
		Submarine_Entry,
		Fish_Zone
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

			case Location.Fish_Zone:
				if (TutorialManager.Obj.currentStep == 5) {
					TutorialManager.Obj.hasMovedNearFish = true;
					gameObject.SetActive (false);
				}
				break;
			
			default:
				break;
			}


		}

	}
}
