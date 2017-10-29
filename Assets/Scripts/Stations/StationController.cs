using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : IInteractable
{
    [SerializeField]
    public IInteractable controller;
    [SerializeField]
    public Shader outlineShader;

    //Used for highlighting the object the player may interact with.
    private Shader originalShader;
    private Renderer meshRenderer;

    //Control whether station may be interacted with.
    private bool isActivated;
    public bool IsActivated
    {
        get
        {
            return isActivated;
        }
        set
        {
            isActivated = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        gameObject.tag = "StationObject";
        meshRenderer = gameObject.transform.GetComponent<Renderer>();
        originalShader = meshRenderer.material.shader;
        isActivated = true;
    }

    override public void Interact ()
    {
    }

    override public void Interact ( GameObject other )
    {
        if (isActivated)
            controller.Interact ( other );
    }

    override public void ToggleHighlight (PlayerController otherPlayerController, bool toggle)
    {
        if (toggle)
        {
            if (outlineShader != null)
            {
                meshRenderer.material.shader = outlineShader;
            }
        }
        else
        {
            meshRenderer.material.shader = originalShader;
        }
        controller.ToggleHighlight(otherPlayerController, toggle);
    }
}
