using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MoneySystem : MonoBehaviour {

	private DateTime gameDate;
	private float money;
	private String moneyString;

	void Start () {
		gameDate = new DateTime( 2017, 1, 1 );
		money = 100.0f;
		moneyString = money.ToString ();

		InvokeRepeating ("AddDay", 1.0f, 0.1f);
		InvokeRepeating ("ToString", 1, 1);
	}
		
	void Update () {
	}
		
	public void AddDay() {
		gameDate = gameDate.AddDays( 1 );

		if (gameDate.Day == 1) {
			money += 10.0f;
			Debug.Log (money);
		}

		if (money > 0.0f) {
			money -= 1.0f;
		}
		moneyString = money.ToString ();
	}

	public override string ToString() {
		Debug.Log(gameDate.ToString("dd.MM.yyyy"));
		return gameDate.ToString("dd.MM.yyyy");
	}

	void OnGUI() {
		GUI.TextArea(new Rect(10, 10, 150, 25), "Game Date: " + gameDate.ToString("dd.MM.yyyy"), 150);
		GUI.TextArea(new Rect(10, 35, 150, 25), "Money : " + moneyString, 150);
	}
}