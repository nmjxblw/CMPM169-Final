using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRollSkillBehaviour : StateMachineBehaviour
{
    public const float duration = 0.5f;
    public float remainingTime;
    public float rollSpeed = 10f;
    public Vector3 rollDirection;
    public Transform transform;
    public GameObject skillAttackArea;
    public Animator animator;
    public static readonly int skillHash = Animator.StringToHash("skill");
    public EnemyControl enemyControl;
    public EnemyAI ai;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        transform = animator.transform;
        skillAttackArea = transform.Find("DamageAreas/SkillAttackArea").gameObject;
        skillAttackArea.SetActive(true);
        enemyControl = transform.GetComponent<EnemyControl>();
        ai = transform.GetComponent<EnemyAI>();
        enemyControl.isSkill = true;
        enemyControl.skillActivable = false;
        enemyControl.StartCoroutine(RollingCoroutine());
        rollDirection = (Vector3)enemyControl.inputDirection;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {

    // }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skillAttackArea.SetActive(false);
        enemyControl.isSkill = false;
        enemyControl.StopCoroutine(RollingCoroutine());
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
        transform.Translate(rollDirection * rollSpeed * Time.deltaTime);
        if (ai.distanceToAgentDestination <= ai.agent.stoppingDistance) animator.SetBool(skillHash, false);
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    public IEnumerator RollingCoroutine()
    {
        remainingTime = duration;
        while (remainingTime > 0f)
        {
            remainingTime = Mathf.Clamp(remainingTime - Time.deltaTime, 0f, duration);
            yield return null;
        }
        animator.SetBool(skillHash, false);
    }
}
