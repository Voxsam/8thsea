using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {

	public float amplitude = 0.5f;
	public float frequency = 1f;

	public void OnEnable ()
	{
		StartCoroutine (FloatObject ());
	}

	void OnDisable ()
	{
		StopAllCoroutines ();
	}

	IEnumerator FloatObject() {
		while (true) {
			Vector3 currPos = this.transform.localPosition;
			this.transform.localPosition = new Vector3(currPos.x, currPos.y + Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude, currPos.z);
			yield return new WaitForEndOfFrame ();
		}
	}
}
