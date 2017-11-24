using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour {

	public static MainMenuAudioManager Obj;

	public AudioSource source;
	public AudioClip[] clips;

	void Awake () {
		Obj = this;
	}

	public void PlayConfirmNoise () {
		source.clip = clips [0];
		source.Play ();
	}

	public void PlayMoveNoise () {
		source.clip = clips [1];
		source.Play ();
	}

	public void PlayConfirmNoise2 () {
		source.clip = clips [2];
		source.Play ();
	}



}
