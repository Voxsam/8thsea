using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScheme {

	public bool isKeyboard;
	public int playerNumber;
	public int joystickNumber;

	public ControlScheme(int joystickNumber, bool isKeyboard) {

		this.joystickNumber = joystickNumber;
		this.isKeyboard = isKeyboard;
	}

	public float GetHorizontalAxis() {
		if (!isKeyboard) {
			return Input.GetAxis ("Horizontal_J" + joystickNumber);
		} else {
			return Input.GetAxis ("Horizontal_K" + joystickNumber);
		}
	}


	public float GetVerticalAxis() {
		if (!isKeyboard) {
			return Input.GetAxis ("Vertical_J" + joystickNumber);
		} else {
			return Input.GetAxis ("Vertical_K" + joystickNumber);
		}
	}

}
