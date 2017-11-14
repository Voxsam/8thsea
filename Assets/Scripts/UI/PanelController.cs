using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour {

	public Text[] textboxes;

	private Image bgImage;

	private Color invisible = new Color (0,0,0,0);

	Color initBgColor;
	Color[] initTextColors;

	void Awake () {
		bgImage = GetComponent<Image> ();
		initBgColor = bgImage.color;
		initTextColors = new Color[textboxes.Length];
		for (int i = 0; i < textboxes.Length; i++) {
			initTextColors[i] = textboxes [i].color;
		}
	}

	void OnEnable () {
		Reset ();
	}

	void OnDisable () {
		StopAllCoroutines ();
	}
		
	private void Reset () {
		bgImage.color = initBgColor;
		for (int i = 0; i < textboxes.Length; i++) {
			textboxes [i].color = initTextColors [i]; 
		}
	}

	IEnumerator FadeInOverTime(float seconds, float secondsToHold)
	{
		bgImage.color = invisible;
		for (int i = 0; i < textboxes.Length; i++) {
			textboxes [i].color = invisible;
		}
		float elapsedTime = 0f;
		while (elapsedTime < seconds) {
			bgImage.color = Color.Lerp (invisible, initBgColor, elapsedTime / seconds);
			for (int i = 0; i < textboxes.Length; i++) {
				textboxes [i].color = Color.Lerp (invisible, initTextColors [i], elapsedTime / seconds);
			}
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSecondsRealtime (secondsToHold);

	}

	IEnumerator FadeOutOverTime(float seconds, bool deactivateAfterFading)
	{
		float elapsedTime = 0f;
		while (elapsedTime < seconds) {
			bgImage.color = Color.Lerp (initBgColor, invisible, elapsedTime / seconds);
			for (int i = 0; i < textboxes.Length; i++) {
				textboxes [i].color = Color.Lerp (initTextColors [i], invisible, elapsedTime / seconds);
			}
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		if (deactivateAfterFading) {
			gameObject.SetActive (false);
		}
	}

	IEnumerator FadeInHoldAndOut (float inTime, float holdTime, float outTime, bool deactivateAfter) {
		yield return StartCoroutine (FadeInOverTime (inTime, holdTime));
		yield return FadeOutOverTime (outTime, deactivateAfter);
	}

	public void FadeInAndHold (float seconds, float secondsToHold) {
		StartCoroutine (FadeInOverTime (seconds, secondsToHold));
	}

	public void FadeOut (float seconds, bool deactivateAfterFading) {
		if (!deactivateAfterFading) {
			StartCoroutine (FadeOutOverTime (seconds, false));
		} else {
			StartCoroutine (FadeOutOverTime (seconds, true));
		}
	}

	public void FadeInAndOut (float inTime, float holdTime, float outTime, bool deactivateAfter) 
	{
		StartCoroutine (FadeInHoldAndOut (inTime, holdTime, outTime, deactivateAfter));
	}

}
