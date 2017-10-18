using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public ControlScheme controls;
	public PlayerController controller;
	public int playerNumber;

	public Player(ControlScheme cs, int number)
	{
		this.controls = cs;
		this.playerNumber = number;
	}

}