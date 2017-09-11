using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public static GameController controller;

	public Text timeLeftText;
	public Text moneyText;

	public int currentMoney;
	public float timeTillNextPayment;
	public float paymentInterval = 60f;
	public int paymentAmount = 100;

	void Start () {

		controller = this;
		timeTillNextPayment = paymentInterval;

	}

	void Update () {

		if (timeTillNextPayment == 0) {
			RemoveMoney(paymentAmount);
			timeTillNextPayment = paymentInterval;
		}

		timeTillNextPayment -= Time.deltaTime;

		moneyText.text = "$" + currentMoney.ToString ();	
		timeLeftText.text = "Time Left: " + (int) timeTillNextPayment + "s";
	}


	public void AddMoney(int amount) {
		currentMoney += amount;
	}

	public void RemoveMoney(int amount) {
		currentMoney -= amount;
	}
		
}
