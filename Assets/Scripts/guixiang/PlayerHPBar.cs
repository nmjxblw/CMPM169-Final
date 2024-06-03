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
        GameManager.Instance.onPlayerSpawnEvent.AddListener(HandlePlayerSpawn);
    }

    public void HandlePlayerSpawn(GameObject player)
    {
        Debug.Log("Player spawned");
        gameObject.SetActive(true);
        character = player.GetComponent<Character>();
        character.UIUpdateEvent.AddListener(UpdateUI);
        slider.maxValue = character.maxHp;
        slider.value = character.maxHp;
        textMeshPro.text = $"{slider.value}/{slider.maxValue}";
    }

    private void OnDisable()
    {
        //character.UIUpdateEvent.RemoveListener(UpdateUI);
    }

    private void UpdateUI()
    {
        slider.value = character.hp;
        slider.maxValue = character.maxHp;
        textMeshPro.text = $"{slider.value}/{slider.maxValue}";
    }
}
