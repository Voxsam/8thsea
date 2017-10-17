using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScheme {

	public bool isKeyboard;

	public int joystickNumber;

	public ControlScheme(int joystickNumber) {

		isKeyboard = false;

	}

	public float GetHorizontalAxis() {
		return Input.GetAxis("Horizontal_J" + joystickNumber);
	}


	public float GetVerticalAxis() {
		return Input.GetAxis("Vertical_J" + joystickNumber);
	}

}
