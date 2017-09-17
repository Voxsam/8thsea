using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class OxygenCountdown : MonoBehaviour {
    public float timeLeft;
    public float rate;
    public bool isActivated;
    public Slider OxygenBar;
    public Text DebugText;
    public Image Fill;


	// Use this for initialization
	void Start () {
        // Expensive so it is allocated in the editor
        //DebugText = GameObject.Find("Debug").GetComponent<Text>();
        //OxygenBar = GameObject.Find("OxygenBar").GetComponent<Slider>();
        //Fill = OxygenBar.GetComponentsInChildren<UnityEngine.UI.Image>().FirstOrDefault(t => t.name == "Fill");
        timeLeft = OxygenBar.maxValue;
        //isActivated = true; // Handled by the SubmarineController
        rate = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        if (isActivated)
        {
            timeLeft -= rate * Time.deltaTime;
            OxygenBar.value = timeLeft;
            if (timeLeft < 5)
            {
                if (Fill != null)
                {
                    Fill.color = Color.red;
                }
            }

            if (timeLeft < 0)
            {
                if (Fill != null)
                {
                    Fill.color = Color.black;
                }
                DebugText.text = "Game Over";
            }
        }
        else {
            timeLeft = OxygenBar.maxValue;
            OxygenBar.value = timeLeft;
        }
	}

}
