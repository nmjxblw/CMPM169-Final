using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
[Serializable]
public class Chase : AIState
{
    public Chase(EnemyAI ai) : base(ai) { }
    public override void OnEnter() { }
    public override void OnUpdate()
    {
        if (ai.enemyControl.skillActivable && ai.distanceToAgentDestination <= ai.skillRange)
        {
            ai.SwitchState(Logic.skill);
            return;
        }
    }
    public override void OnExit() { }
}
