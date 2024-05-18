using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
[Serializable]
public class EnemyState
{
    protected EnemyAI ai;
    public EnemyState(EnemyAI ai) { this.ai = ai; }
    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}
