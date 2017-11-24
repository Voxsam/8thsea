using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectElement : MonoBehaviour {

	public Animator animator;
	public RotateAround rotator;

	public void Activate () {
		//rotator.isRotating = true;
		animator.SetBool ("isWalking", true);
	}

}
