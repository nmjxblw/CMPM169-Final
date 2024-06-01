using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public TextAsset textAsset;
    public int maxHp;
    public int defense;
    public float moveSpeed;
    public float sprintSpeed;
    public float rollingSpeed;
    public float rollingCoolDown;
    public float maxRollingTime;
    public float invincibleDuration;

    public void OnEnable()
    {
        ReadDataFromCSV();
    }
    [ContextMenu("ReadDataFromCSV")]
    public void ReadDataFromCSV()
    {
        if (textAsset == null) return;
        string[] lines = textAsset.text.Split('\n');
        string[] values = lines[1].Split(',');
        maxHp = int.Parse(values[0].Trim());
        defense = int.Parse(values[1].Trim());
        moveSpeed = float.Parse(values[2].Trim());
        sprintSpeed = float.Parse(values[3].Trim());
        rollingSpeed = float.Parse(values[4].Trim());
        rollingCoolDown = float.Parse(values[5].Trim());
        maxRollingTime = float.Parse(values[6].Trim());
        invincibleDuration = float.Parse(values[7].Trim());
    }
}
