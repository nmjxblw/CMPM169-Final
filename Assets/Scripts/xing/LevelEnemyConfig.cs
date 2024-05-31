using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelEnemyConfig", menuName = "Configurations/LevelEnemyConfig")]
public class LevelEnemyConfig : ScriptableObject
{
    public LevelData[] levels;
}

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public EnemySpawnData[] enemies;
    public bool isBuffGift;
    public bool isWeaponGift;
}

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public int count; 
}
