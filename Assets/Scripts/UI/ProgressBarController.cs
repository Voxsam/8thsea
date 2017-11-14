using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarController : MonoBehaviour {
    public enum State
    {
        Idle,
        Explode
    }
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private AnimationCurve explodeCurve;

    private State currentState;
    private float explodeDuration;
    private float currentTime;


    // Use this for initialization
    void Start () {
        currentState = State.Idle;
        currentTime = 0f;
        explodeDuration = explodeCurve.keys[explodeCurve.length - 1].time;
    }
	
	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case State.Idle:
                rectTransform.localScale = new Vector3(1, Random.Range(0.95f, 1.05f), 1);
                break;
            case State.Explode:
                if (currentTime < explodeDuration)
                {
                    rectTransform.localScale = new Vector3(1, explodeCurve.Evaluate(currentTime), 1);

                    currentTime += Time.deltaTime;
                }
                else
                {
                    currentState = State.Idle;
                }
                break;
        }
    }

    public void Explode()
    {
        currentState = State.Explode;
        currentTime = 0f;
    }
}
