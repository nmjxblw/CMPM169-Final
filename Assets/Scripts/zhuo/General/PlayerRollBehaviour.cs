using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollBehaviour : StateMachineBehaviour
{
    public PlayerCharacter playerCharacter;
    public PlayerMovements playerMovements;
    public Vector3 rollDirection;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCharacter = playerCharacter ?? animator.transform.parent.GetComponent<PlayerCharacter>();
        playerMovements = playerMovements ?? animator.transform.parent.GetComponent<PlayerMovements>();
        playerCharacter.invincible = true;
        playerMovements.isRolling = true;
        playerMovements.PlayerBodySprite.flipX = playerMovements.lastInputDirection.x < 0;
        rollDirection = playerMovements.lastInputDirection;
        playerMovements.rollingTimeRemaining = playerMovements.maxRollingTime;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerMovements.rb.velocity = rollDirection * playerMovements.rollingSpeed;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerMovements.isRolling = false;
        playerCharacter.invincible = playerCharacter.invincibleTimeRemaining > 0;
        playerMovements.canRoll = false;
        playerMovements.rollingCoolDownRemainingTime = playerMovements.rollingCoolDown;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
