using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextParticlesController : MonoBehaviour {
    public enum State
    {
        Idle,
        Explode
    }
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Text text;

    private AnimationCurve explodeCurve;
    private State currentState;
    private float explodeDuration;
    private float currentTime;


    // Use this for initialization
    void Start () {
        currentState = State.Idle;
        currentTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState)
        {
            case State.Idle:
                rectTransform.localScale = new Vector3(Random.Range(0.9f, 1.1f),
                                                        Random.Range(0.9f, 1.1f),
                                                        Random.Range(0.9f, 1.1f));
                break;
            case State.Explode:
                if (currentTime < explodeDuration)
                {
                    rectTransform.localScale = new Vector3(explodeCurve.Evaluate(currentTime),
                                                            explodeCurve.Evaluate(currentTime),
                                                            explodeCurve.Evaluate(currentTime));
                    Color c = text.color;
                    c.a = 1 - (currentTime / explodeDuration);
                    text.color = c;

                    currentTime += Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                }
                break;
        }
	}

    public void Init (AnimationCurve _explodeCurve, string _text = "0.00")
    {
        explodeCurve = _explodeCurve;
        currentTime = 0f;
        explodeDuration = explodeCurve[explodeCurve.length - 1].time;
        SetText(_text);
        rectTransform.localScale = Vector3.one;
        currentState = State.Idle;
    }

    public void Explode ()
    {
        currentState = State.Explode;
        currentTime = 0f;
    }

    public void SetText (string _text)
    {
        text.text = "+" + _text + "s";
    }
}
