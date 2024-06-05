using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


[CreateAssetMenu(fileName = "NecromancerConfig", menuName = "NecromancerConfig")]
public class NecromancerConfig : ScriptableObject
{
    [Serializable]
    public class Config : IComparable<Config>
    {
        public int difficulty;
        [SerializeField]
        private int _hp;
        public int hp
        {
            get { return _hp; }
            set { _hp = Math.Abs(value); }
        }
        [SerializeField]
        private float _invincibleDuration;
        public float invincibleDuration
        {
            get { return _invincibleDuration; }
            set { _invincibleDuration = Math.Abs(value); }
        }
        [SerializeField]
        private float _moveSpeed;
        public float moveSpeed
        {
            get { return _moveSpeed; }
            set { _moveSpeed = Math.Abs(value); }
        }
        [SerializeField]
        private float _sprintSpeed;
        public float sprintSpeed
        {
            get { return _sprintSpeed; }
            set { _sprintSpeed = Math.Abs(value); }
        }
        [SerializeField]
        private int _attackDamage;
        public int attackDamage
        {
            get { return _attackDamage; }
            set { _attackDamage = Math.Abs(value); }
        }
        [SerializeField]
        private int _skill1Damage;
        public int skill1Damage
        {
            get { return _skill1Damage; }
            set { _skill1Damage = Math.Abs(value); }
        }
        [SerializeField]
        private float _skill1Cooldown;
        public float skill1Cooldown
        {
            get { return _skill1Cooldown; }
            set { _skill1Cooldown = Math.Abs(value); }
        }
        [SerializeField]
        private int _skill2Damage;
        public int skill2Damage
        {
            get { return _skill2Damage; }
            set { _skill2Damage = Math.Abs(value); }
        }
        [SerializeField]
        private float _skill2Cooldown;
        public float skill2Cooldown
        {
            get { return _skill2Cooldown; }
            set { _skill2Cooldown = Math.Abs(value); }
        }
        public int CompareTo(Config other)
        {
            int result = difficulty.CompareTo(other.difficulty);
            if (result == 0)
            {
                result = hp.CompareTo(other.hp);
            }
            return result;
        }
    }

    [SerializeField]
    private string dataName = "EnemyConfigs/NecromancerConfig";
    [SerializeField]
    private TextAsset dataFile;
    [SerializeField]
    private string firstLine = "Difficulty,Hp,Invincible Duration,Move Speed,Sprint Speed,Attack Damage,Skill1 Damage,Skill1 Cooldown,Skill2 Damage,Skill2 Cooldown,";

    public List<Config> configs;
    [ContextMenu("ReadDataFormCSV")]
    public void ReadDataFromCSV()
    {
        dataFile = Resources.Load<TextAsset>(dataName);
        if (dataFile == null)
        {
            Debug.LogError("dataFile is null");
            return;
        }
        UpdateData();
    }
    [ContextMenu("UpdateData")]
    public void UpdateData()
    {
        configs = new List<Config>();
        string[] lines = dataFile.text.Split('\n');
        firstLine = lines[0];
        for (int i = 1; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(',');
            Config config = new Config();
            config.difficulty = int.Parse(line[0].Trim());
            config.hp = int.Parse(line[1].Trim());
            config.invincibleDuration = float.Parse(line[2].Trim());
            config.moveSpeed = float.Parse(line[3].Trim());
            config.sprintSpeed = float.Parse(line[4].Trim());
            config.attackDamage = int.Parse(line[5].Trim());
            config.skill1Damage = int.Parse(line[6].Trim());
            config.skill1Cooldown = float.Parse(line[7].Trim());
            config.skill2Damage = int.Parse(line[8].Trim());
            config.skill2Cooldown = float.Parse(line[9].Trim());
            configs.Add(config);
        }
        SortList();
    }
    [ContextMenu("SortList")]
    public void SortList()
    {
        configs.Sort();
        for (int i = 0; i < configs.Count; i++)
        {
            configs[i].difficulty = i;
        }
    }
    [ContextMenu("WriteDataToCSV")]
    public void WriteDataToCSV()
    {
        if (configs == null || configs.Count == 0)
        {
            Debug.LogError("configs is empty");
            return;
        }
        string data = firstLine + "\n";
        foreach (Config config in configs)
        {
            data += config.difficulty + ", " + config.hp + ", " + config.invincibleDuration + ", " + config.moveSpeed + ", " + config.sprintSpeed + ", " + config.attackDamage + ", " + config.skill1Damage + ", " + config.skill1Cooldown + ", " + config.skill2Damage + ", " + config.skill2Cooldown + ",\n";
        }
        data = data.TrimEnd('\n');
        if (!File.Exists(GetFilePath()))
        {
            try
            {
                File.WriteAllText(GetFilePath(), string.Empty);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to create new CSV file. Error: " + e.Message);
            }
        }
        try
        {
            using (StreamWriter writer = new StreamWriter(GetFilePath()))
            {
                writer.Write(data);
                Debug.Log("CSV file written successfully at path: " + GetFilePath());
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to write CSV file. Error: " + e.Message);
        }
    }
    private string GetFilePath()
    {
        return Path.Combine(Application.dataPath, "Resources", dataName + ".csv");
    }
}