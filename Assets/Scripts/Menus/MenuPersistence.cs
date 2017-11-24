using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPersistence : MonoBehaviour {

	public bool hasSeenTitle = false;

	public Image logo;
	public GameObject pressAnyKeyText;

	public GameObject characterSelectInterface;

	private bool logoIsVisible = false;

	void Awake () {
		DontDestroyOnLoad(this);
		logo.color = new Color (1f, 1f, 1f, 0);
		Setup ();
	}

	private void Setup() {

		if (hasSeenTitle) {
			characterSelectInterface.SetActive (true);
		} else {
			StartCoroutine (FadeInLogo (3f));
		}
	}

	void Update () {

		if (logoIsVisible) {
			if (Input.anyKeyDown) {
				logoIsVisible = false;
				logo.gameObject.SetActive (false);
				pressAnyKeyText.SetActive (false);
				hasSeenTitle = true;
				MainMenuAudioManager.Obj.PlayConfirmNoise2 ();
				characterSelectInterface.SetActive (true);
			}
		}

	}

	IEnumerator FadeInLogo (float seconds) {
		float elapsedTime = 0;
		while (elapsedTime < seconds) {
			logo.color = Color.Lerp (new Color (1f, 1f, 1f, 0), Color.white, elapsedTime / seconds);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		logoIsVisible = true;
		pressAnyKeyText.SetActive (true);
	}

}
