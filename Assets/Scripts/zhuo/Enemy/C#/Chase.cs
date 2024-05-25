using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
[Serializable]
public class Chase : AIState
{
    public Chase(EnemyAI ai) : base(ai) { }
    public override void OnEnter()
    {
        ai.enemyControl.canInput = true;
    }
    public override void OnUpdate()
    {
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
        if (ai.enemyControl.hasSkill && ai.enemyControl.skillActivable && ai.distanceToAgentDestination <= ai.skillRange)
        {
            ai.SwitchState(Logic.skill);
            return;
        }
    }
    public override void OnExit() { }
}
