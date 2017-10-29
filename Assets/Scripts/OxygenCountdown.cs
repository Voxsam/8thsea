using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class OxygenCountdown : MonoBehaviour {
    public enum State
    {
        Running,
        Refilling,
        Ready,
        Emergency
    }
    private State currentState;
    public State CurrentState
    {
        get
        {
            return currentState;
        }
    }

    private float timeLeft;
    public float drainRate;
    public float refillRate;

    public bool isActivated;

    public Slider OxygenBar;
    public Image Fill;


	// Use this for initialization
	void Start () {
        // Expensive so it is allocated in the editor
        //DebugText = GameObject.Find("Debug").GetComponent<Text>();
        //OxygenBar = GameObject.Find("OxygenBar").GetComponent<Slider>();
        //Fill = OxygenBar.GetComponentsInChildren<UnityEngine.UI.Image>().FirstOrDefault(t => t.name == "Fill");
        timeLeft = OxygenBar.maxValue;
        currentState = State.Ready;

        //isActivated = true; // Handled by the SubmarineController
	}
	
	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case State.Running:
                if (isActivated)
                {
                    timeLeft -= drainRate * Time.deltaTime;
                    OxygenBar.value = timeLeft;
                    if (timeLeft < OxygenBar.maxValue * 0.5)
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
                        timeLeft = 0f;
                        currentState = State.Emergency;
                    }
                }
                break;
            case State.Refilling:
                if (isActivated)
                {
                    timeLeft += refillRate * Time.deltaTime;
                    if (timeLeft < OxygenBar.maxValue * 0.5)
                    {
                        if (Fill != null)
                        {
                            Fill.color = Color.red;
                        }
                    }
                    else if (timeLeft < 0)
                    {
                        if (Fill != null)
                        {
                            Fill.color = Color.black;
                        }
                    }
                    else
                    {
                        if (Fill != null)
                        {
                            Fill.color = Color.blue;
                        }
                    }

                    if (timeLeft > OxygenBar.maxValue)
                    {
                        timeLeft = OxygenBar.maxValue;
                        currentState = State.Ready;
                    }

                    OxygenBar.value = timeLeft;
                }
                break;
            case State.Ready:
                break;
            case State.Emergency:
                //TODO: Make this spend cash to "power" the submarine.
                break;
        }
        
	}

    public void StartRunning ()
    {
        currentState = State.Running;
    }

    public void StartRefilling ()
    {
        currentState = State.Refilling;
    }

    public bool IsReady()
    {
        return currentState == State.Ready;
    }
}
