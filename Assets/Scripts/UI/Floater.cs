using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {

	public float amplitude = 0.5f;
	public float frequency = 1f;
	private Vector3 initPos;

	public void OnEnable ()
	{
		initPos = this.transform.position;
		StartCoroutine (FloatObject ());
	}

	void OnDisable ()
	{
		StopAllCoroutines ();
	}

	IEnumerator FloatObject() {
		while (true) {
			initPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
			this.transform.position = initPos;
			yield return new WaitForEndOfFrame ();
		}
	}
}
