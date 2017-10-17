using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public ControlScheme controls;
	public PlayerController controller;

	public Player(ControlScheme cs)
	{
		this.controls = cs;
	}

}
