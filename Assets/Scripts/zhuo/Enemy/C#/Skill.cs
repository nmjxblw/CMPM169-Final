using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
[Serializable]
public class Skill : AIState
{
    public Skill(EnemyAI ai) : base(ai) { }
    public override void OnEnter()
    {
        ai.enemyControl.HandleSkill();
    }
    public override void OnUpdate()
    {
    }
    public override void OnExit() { }
}
