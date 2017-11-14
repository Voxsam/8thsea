using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    private Animator anim;
    private PlayerInteractionController interactionController;
    private PlayerController controller;

	// Use this for initialization
	void Start () {
        anim = GetComponentInChildren<Animator>();
        controller = gameObject.GetComponentInParent<PlayerController>();
        interactionController = controller.interactionController;
	}
	
	// Update is called once per frame
	public void GameUpdate () {
        if (controller != null)
        {
            bool isMoving = controller.IsPlayerMoving;
            bool isHolding = (interactionController.GetCurrentState() == PlayerInteractionController.State.Hold || interactionController.GetCurrentState() == PlayerInteractionController.State.PickingUp);

            anim.SetBool("isHoldingWalk", isMoving && isHolding);
            anim.SetBool("isHoldingIdle", !isMoving && isHolding);
            anim.SetBool("isWalking", isMoving && !isHolding);
            anim.SetBool("isIdle", !isMoving && !isHolding);
        }
    }
}