using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    public Character character;
    public Slider slider;
    public TextMeshProUGUI textMeshPro;

    private void Start()
    {
        slider.maxValue = character.maxHp;
        slider.value = character.maxHp;
        textMeshPro.text = $"{slider.value}/{slider.maxValue}";
    }

    private void OnEnable()
    {
        character.UIUpdateEvent.AddListener(UpdateUI);
    }

    private void OnDisable()
    {
        character.UIUpdateEvent.RemoveListener(UpdateUI);
    }

    private void UpdateUI()
    {
        slider.value = character.hp;
        textMeshPro.text = $"{slider.value}/{slider.maxValue}";
    }
}
