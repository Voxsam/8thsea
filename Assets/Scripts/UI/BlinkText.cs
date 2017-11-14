using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour {

	public Color initColor;

	public float onTime = 1.2f;
	public float offTime = 0.6f;

	private Color invisible = new Color (0,0,0,0);
	private Text _text;

	void Awake () {
		_text = GetComponent<Text> ();
		initColor = _text.color;
	}

	void OnEnable () {
		_text.color = initColor;
		StartCoroutine (Blink ());
	}

	void OnDisable () {
		StopAllCoroutines ();
	}

	IEnumerator Blink() {
		while (true) {
			if (_text.color == initColor) {
				_text.color = invisible;
				yield return new WaitForSecondsRealtime (offTime);
			} else {
				_text.color = initColor;
				yield return new WaitForSecondsRealtime (onTime);
			}
		}
	}
}
