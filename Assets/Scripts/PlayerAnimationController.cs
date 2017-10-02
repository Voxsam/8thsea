﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    private Animator anim;
    private PlayerInteractionController interactionController;
    private PlayerController controller;

	// Use this for initialization
	void Start () {
        anim = GetComponentInChildren<Animator>();
        interactionController = gameObject.GetComponent<PlayerInteractionController>();
        controller = gameObject.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	public void GameUpdate () {
        bool isMoving = controller.IsPlayerMoving;
        bool isHolding = (interactionController.GetCurrentState() == PlayerInteractionController.State.Hold || interactionController.GetCurrentState() == PlayerInteractionController.State.PickingUp);
        
        anim.SetBool("isHoldingWalk", isMoving && isHolding);
        anim.SetBool("isHoldingIdle", !isMoving && isHolding);
        anim.SetBool("isWalking", isMoving && !isHolding);
        anim.SetBool("isIdle", !isMoving && !isHolding);
        /*
        if (isMoving && isHolding)
        {
            anim.SetBool("isHoldingWalk", true);
            anim.SetBool("isHoldingIdle", false);
        }
        else if (isMoving && !isHolding)
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isIdle", false);
        }
        else if (!isMoving && isHolding)
        {
            anim.SetBool("isHoldingIdle", true);
            anim.SetBool("isHoldingWalk", false);
        }
        else
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalking", false);
        }//*/
    }
}/*
f (interactionController.getCurrentState() == PlayerInteractionController.State.Hold)
                {
                    anim.Play("Holding Walk");
                } else
                {
                    anim.Play("Walking");
                }
                
*/
