using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
[Serializable]
public class Dead : AIState
{
    public Dead(EnemyAI ai) : base(ai) { }
    public override void OnEnter()
    {
        ai.agent.isStopped = true;
        ai.inputDirection = Vector2.zero;
    }
    public override void OnUpdate() { }
    public override void OnExit() { }
}
