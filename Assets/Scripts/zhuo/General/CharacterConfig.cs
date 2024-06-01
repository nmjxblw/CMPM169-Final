using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "CharacterConfig")]
public class CharacterConfig : ScriptableObject
{
    public int maxHp;
    public int attack;
    public int defense;
    public float moveSpeed;
    public float sprintSpeed;
}
