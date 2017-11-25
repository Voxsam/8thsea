using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	public Image logo;
	public GameObject pressAnyKeyText;

	public GameObject characterSelectInterface;

	private bool logoIsVisible = false;

	void Start ()
	{
		logo.color = new Color (1f, 1f, 1f, 0);
		Setup ();
	}


	private void Setup ()
	{

		if (MenuPersistence.Obj.hasSeenTitle) {
			characterSelectInterface.SetActive (true);
			logo.gameObject.SetActive (false);
		} else {
			StartCoroutine (FadeInLogo (3f));
		}
	}

	void Update ()
	{
			if (logoIsVisible) {
				if (Input.anyKeyDown) {
					StartCoroutine (PlaySoundAndShowCharacterSelectAfterDelay (0.8f));
				}
			}

	}

	IEnumerator PlaySoundAndShowCharacterSelectAfterDelay (float seconds)
	{
		MainMenuAudioManager.Obj.PlayConfirmNoise2 ();
		yield return new WaitForSecondsRealtime (seconds);
		logoIsVisible = false;
		logo.gameObject.SetActive (false);
		pressAnyKeyText.SetActive (false);
		MenuPersistence.Obj.hasSeenTitle = true;
		characterSelectInterface.SetActive (true);
	}

	IEnumerator FadeInLogo (float seconds)
	{
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
