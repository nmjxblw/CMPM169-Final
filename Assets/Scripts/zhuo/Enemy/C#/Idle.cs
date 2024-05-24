using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
[Serializable]
public class Idle : AIState
{
    public Idle(EnemyAI ai) : base(ai) { }
    public override void OnEnter()
    {
    }
    public override void OnUpdate()
    {
        ai.inputDirection = Vector2.zero;
        if (ai.agent.hasPath)
        {
            ai.SwitchState(Logic.chase);
            return;
        }
    }
    public override void OnExit()
    {
    }
}
