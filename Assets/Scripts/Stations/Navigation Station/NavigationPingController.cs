﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationPingController : MonoBehaviour {
    public float finalSize;
    public float lifespan;

    public RectTransform canvasRectTransform;

    public enum State
    {
        Inactive,
        Active
    };
    private State currentState;

    private Vector3 targetLocation;
    private Vector3 targetScreenPosition;
    private Vector2 targetOnScreenPosition;
    private float maxOffset;

    private Image navigationPingImage;
    private RectTransform imageRectTransform;
    private float currentLifespan;
    private float currentSize;

    public void Init (Canvas canvasToAttach, Vector3 _targetLocation, float _lifespan = 3, float _finalSize = 300)
    {
        lifespan = _lifespan;
        finalSize = _finalSize;
        targetLocation = _targetLocation;
        canvasRectTransform = canvasToAttach.GetComponent<RectTransform>();
    }

    // Use this for initialization
    void Start () {
        currentLifespan = currentSize = 0;
        navigationPingImage = gameObject.GetComponent<Image>();
        imageRectTransform = navigationPingImage.GetComponent<RectTransform>();
        currentState = State.Active;
	}
	
	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case State.Inactive:
                    Destroy(gameObject);
                break;
            case State.Active:
                //Update the size of the element.
                if (currentLifespan <= lifespan)
                {
                    //currentSize = (initialSpeed * currentLifespan) + (acceleration * currentLifespan * currentLifespan);
                    currentSize = QuadEaseOut(currentLifespan, 0, finalSize, lifespan);
                    imageRectTransform.sizeDelta = new Vector2(currentSize, currentSize);

                    Color c = navigationPingImage.color;
                    c.a = 1 - (currentLifespan / lifespan);
                    navigationPingImage.color = c;
                }
                else
                {
                    currentState = State.Inactive;
                }
                currentLifespan += Time.deltaTime;

                //Position the element.
                targetScreenPosition = GameController.Obj.gameCamera.GetCamera.WorldToViewportPoint(targetLocation); //get viewport positions

                if (targetScreenPosition.x >= 0 && targetScreenPosition.x <= 1 
                    && targetScreenPosition.y >= 0 && targetScreenPosition.y <= 1)
                {
                    PositionElement(targetScreenPosition);
                }
                else
                {
                    targetOnScreenPosition = new Vector2(targetScreenPosition.x - 0.5f, targetScreenPosition.y - 0.5f) * 2; //2D version, new mapping
                    maxOffset = Mathf.Max(Mathf.Abs(targetOnScreenPosition.x), Mathf.Abs(targetOnScreenPosition.y)); //get largest offset
                    targetOnScreenPosition = (targetOnScreenPosition / (maxOffset * 2)) + new Vector2(0.5f, 0.5f); //undo mapping
                    PositionElement(targetOnScreenPosition);
                }
                break;
            default:
                break;
        }
    }

    private void PositionElement ( Vector2 viewportPosition )
    {
        Vector2 worldObjectScreenPosition = new Vector2(
        ((viewportPosition.x * canvasRectTransform.sizeDelta.x) - (canvasRectTransform.sizeDelta.x * 0.5f)),
        ((viewportPosition.y * canvasRectTransform.sizeDelta.y) - (canvasRectTransform.sizeDelta.y * 0.5f)));
        imageRectTransform.anchoredPosition = worldObjectScreenPosition;
    }

    private float QuadEaseInOut (float currentTime, float initialValue, float changeValue, float duration)
    {
        if ((currentTime /= duration / 2) < 1)
        {
            return changeValue / 2 * currentTime * currentTime + initialValue;
        }
	    return -changeValue / 2 * ((--currentTime) *(currentTime - 2) - 1) + initialValue;
    }

    private float QuadEaseOut (float currentTime, float initialValue, float changeValue, float duration)
    {
        currentTime /= duration;
        return -changeValue * currentTime * (currentTime - 2) + initialValue;
    }

#region Getters/Setters
public Vector3 TargetLocation
    {
        get
        {
            return targetLocation;
        }
        set
        {
            targetLocation = value;
        }
    }
    #endregion
}
