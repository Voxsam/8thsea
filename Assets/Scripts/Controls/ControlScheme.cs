using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScheme {

	public bool isKeyboard;

	public string horzAxis;
	public string vertAxis;

	public ControlScheme(string horz, string vert) {

		isKeyboard = false;
		horzAxis = horz;
		vertAxis = vert;

	}

	public float GetHorizontalAxis() {
		return Input.GetAxis(horzAxis);
	}


	public float GetVerticalAxis() {
		return Input.GetAxis(vertAxis);
	}

}
