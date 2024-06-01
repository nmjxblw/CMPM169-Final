using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BossHealthDisplayUI : HealthDisplayUI
{
    public TextMeshProUGUI bossName;
    public TextMeshProUGUI bossStage;
    public NecromancerAI ai;
    public Coroutine displayHealthChange;
    public override void Awake()
    {
        base.Awake();
        character.UIUpdateEvent.AddListener(HandleUIUpdate);
        bossName.text = "Necromancer";
        ai = character.GetComponent<NecromancerAI>();
        bossStage.text = "Stage:Normal";
        ai.StageChangeEvent.AddListener(UpdateBossStage);
    }

    public void OnEnable()
    {
        currentHealthImage.fillAmount = (float)character.hp / (float)character.maxHp;
        displayHealthImage.fillAmount = currentHealthImage.fillAmount;
    }

    public void UpdateBossStage()
    {
        bossStage.text = "Stage:Fury";
        bossStage.color = Color.yellow;
    }

    public void HandleUIUpdate()
    {
        currentHealthImage.fillAmount = (float)character.hp / (float)character.maxHp;
        IEnumerator DisplayHealthChange()
        {
            float timer = 0.5f;
            float start = displayHealthImage.fillAmount;
            float end = currentHealthImage.fillAmount;
            while (timer > 0)
            {
                displayHealthImage.fillAmount = Mathf.Lerp(end, start, timer / 0.5f);
                timer -= Time.deltaTime;
                yield return null;
            }
            displayHealthImage.fillAmount = end;
        }
        if (displayHealthChange != null)
        {
            StopCoroutine(displayHealthChange);
        }
        displayHealthChange = StartCoroutine(DisplayHealthChange());
    }
}
