using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Serializable]
    public class Config : IComparable<Config>
    {
        public int difficulty;
        public int hp;
        public int attackDamage;
        public int skillDamage;
        public int CompareTo(Config other)
        {
            int result = this.difficulty.CompareTo(other.difficulty);
            if (result == 0)
            {
                result = this.attackDamage.CompareTo(other.attackDamage);
                if (result == 0)
                {
                    result = this.skillDamage.CompareTo(other.skillDamage);
                    if (result == 0)
                    {
                        result = this.hp.CompareTo(other.hp);
                    }
                }
            }
            return result;
        }
    }

    public List<Config> configs;
    private void OnEable()
    {
        SortList();
    }
    [ContextMenu("Sort List")]
    public void SortList()
    {
        if (configs.Count > 1)
        {
            configs.Sort();
        }
        for (int i = 0; i < configs.Count; i++)
        {
            configs[i].difficulty = i;
        }
    }
}