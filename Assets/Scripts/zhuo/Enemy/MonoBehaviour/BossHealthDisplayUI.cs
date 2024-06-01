using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BossHealthDisplayUI : HealthDisplayUI
{
    public TextMeshProUGUI bossName;
    public TextMeshProUGUI bossStage;
    public NercomancerAI ai;
    public override void Awake()
    {
        base.Awake();
        bossName.text = "Boss:Shadow";
        ai = character.GetComponent<NercomancerAI>();
        bossStage.text = "Stage:Normal";
        ai.StageChangeEvent.AddListener(UpdateBossStage);
    }

    public void UpdateBossStage()
    {
        bossStage.text = "Stage:Fury";
        bossStage.color = Color.yellow;
    }
}
