using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
[Serializable]
public class Chase : EnemyState
{
    public Chase(EnemyAI ai) : base(ai) { }
    public override void OnEnter() { }
    public override void OnUpdate() { }
    public override void OnExit() { }
}
