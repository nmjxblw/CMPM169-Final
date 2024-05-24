using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
[Serializable]
public class Attack : AIState
{
    public Attack(EnemyAI ai) : base(ai) { }
    public override void OnEnter() { }
    public override void OnUpdate()
    {
        if (ai.distanceToAgentDestination > ai.agent.stoppingDistance)
        {
            ai.SwitchState(Logic.chase);
            return;
        }
    }
    public override void OnExit() { }
}
