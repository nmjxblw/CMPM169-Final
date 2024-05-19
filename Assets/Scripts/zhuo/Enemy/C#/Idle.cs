using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
[Serializable]
public class Idle : AIState
{
    public Idle(EnemyAI ai) : base(ai) { }
    public override void OnEnter() { }
    public override void OnUpdate() { 
        ai.inputDirection = Vector2.zero;
    }
    public override void OnExit() { }
}
