using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public static GameController Obj;
    public const float PAYMENT_INTERVAL = 60f;
    public const int PAYMENT_AMOUNT = 100;

    public Text timeLeftText;
	public Text moneyText;

	public int currentMoney;
	public float timeTillNextPayment;

	void Start () {
        if (Obj == null)
        {
            Obj = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        // GameController object should not be destructable
        DontDestroyOnLoad(this.gameObject);

		timeTillNextPayment = PAYMENT_INTERVAL;
	}

	public void AddMoney(int amount) {
		currentMoney += amount;
	}

	public void RemoveMoney(int amount) {
		currentMoney -= amount;
	}

    void Update()
    {
        if (timeTillNextPayment == 0)
        {
            RemoveMoney(PAYMENT_AMOUNT);
            timeTillNextPayment = PAYMENT_INTERVAL;
        }

        timeTillNextPayment -= Time.deltaTime;

        moneyText.text = "$" + currentMoney.ToString();
        timeLeftText.text = "Time Left: " + (int)timeTillNextPayment + "s";
    }
}
