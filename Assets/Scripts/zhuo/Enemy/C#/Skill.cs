using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
[Serializable]
public class Skill : AIState
{
    Vector2 skillDirection;
    public Skill(EnemyAI ai) : base(ai) { }
    public override void OnEnter()
    {
        skillDirection = ai.inputDirection;
        ai.enemyControl.HandleSkill();
    }
    public override void OnUpdate()
    {
        ai.inputDirection = skillDirection;
    }
    public override void OnExit() { }
}
