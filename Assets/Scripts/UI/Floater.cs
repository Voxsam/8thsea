using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {

	public bool horizontal = false;


	public float amplitude = 0.5f;
	public float frequency = 1f;
	public float horzAmplitude = 0.5f;
	public float horzFrequency = 1f;


	private Vector3 initPos;

	void Start () {
		initPos = transform.localPosition;
	}

	void OnEnable ()
	{
		StartCoroutine (FloatObject ());
	}

	void OnDisable ()
	{
		StopAllCoroutines ();
		transform.localPosition = initPos;
	}

	IEnumerator FloatObject() {
		while (true) {
			Vector3 currPos = this.transform.localPosition;
			if (!horizontal) {
				this.transform.localPosition = new Vector3 (currPos.x, currPos.y + Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude, currPos.z);
			} else {
				this.transform.localPosition = new Vector3 (currPos.x + Mathf.Sin (Time.fixedTime * Mathf.PI * horzFrequency) * horzAmplitude, currPos.y + Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude, currPos.z);
			}
			yield return new WaitForEndOfFrame ();
		}
	}
}
