using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;
[Serializable]
public class Chase : AIState
{
    float timer = 0f;
    public Chase(EnemyAI ai) : base(ai) { }
    public override void OnEnter()
    {
        ai.enemyControl.canInput = true;
    }
    public override void OnUpdate()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, 2f);
        if (!ai.agent.hasPath)
        {
            ai.SwitchState(Logic.idle);
            return;
        }
        if (ai.distanceToAgentDestination <= ai.agent.stoppingDistance)
        {
            ai.inputDirection = Vector2.zero;
            ai.SwitchState(Logic.attack);
            return;
        }
        if (ai.enemyControl.hasSkill && ai.enemyControl.skillActivable && ai.distanceToAgentDestination <= ai.skillRange && timer <= 0f)
        {
            if (Random.Range(0, 2) == 0)
            {
                ai.SwitchState(Logic.skill);
                return;
            }
            else
            {
                timer = 1f;
            }
        }
    }
    public override void OnExit() { }
}
