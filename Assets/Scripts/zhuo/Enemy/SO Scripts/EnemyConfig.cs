using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
[CreateAssetMenu(fileName = "EnemyConfig", menuName = "EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField]
    private string dataName = "EnemyConfigs/";
    [SerializeField]
    private TextAsset dataFile;
    [SerializeField]
    private string firstLine = "Difficulty,Hp,Moveable,Speed,Sprintable,Sprint Speed,Has Attack,Attack Damage,Has Skill,Skill Damage,Skill Cooldown,";
    [Serializable]
    public class Config : IComparable<Config>
    {
        public int difficulty;
        public int _hp;
        public int hp
        {
            get
            {
                return _hp;
            }
            set
            {
                _hp = Math.Abs(value);
            }
        }
        public bool moveAble = true;
        [SerializeField]
        private float _moveSpeed;
        public float moveSpeed
        {
            get
            {
                return _moveSpeed;
            }
            set
            {
                _moveSpeed = Math.Abs(value);
            }
        }
        public bool sprintAble = true;
        [SerializeField]
        private float _sprintSpeed;
        public float sprintSpeed
        {
            get
            {
                return _sprintSpeed;
            }
            set
            {
                _sprintSpeed = Math.Abs(value);
            }
        }
        public bool hasAttack = true;
        [SerializeField]
        private int _attackDamage;
        public int attackDamage
        {
            get
            {
                return _attackDamage;
            }
            set
            {
                _attackDamage = Math.Abs(value);
            }
        }
        public bool hasSkill = false;
        [SerializeField]
        private int _skillDamage;
        public int skillDamage
        {
            get
            {
                return _skillDamage;
            }
            set
            {
                _skillDamage = hasSkill ? Math.Abs(value) : 0;
            }
        }
        [SerializeField]
        private float _skillCoolDown;
        public float skillCoolDown
        {
            get
            {
                return _skillCoolDown;
            }
            set
            {
                _skillCoolDown = hasSkill ? Math.Abs(value) : 0;
            }
        }
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
        ReadDataFormCSV();
    }
    [ContextMenu("ReadDataFormCSV")]
    public void ReadDataFormCSV()
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
            config.moveAble = bool.Parse(line[2].Trim().ToLower());
            config.moveSpeed = float.Parse(line[3].Trim());
            config.sprintAble = bool.Parse(line[4].Trim().ToLower());
            config.sprintSpeed = float.Parse(line[5].Trim());
            config.hasAttack = bool.Parse(line[6].Trim().ToLower());
            config.attackDamage = int.Parse(line[7].Trim());
            config.hasSkill = bool.Parse(line[8].Trim().ToLower());
            config.skillDamage = int.Parse(line[9].Trim());
            config.skillCoolDown = float.Parse(line[10].Trim());
            configs.Add(config);
        }
        SortList();
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
        for (int i = 0; i < configs.Count; i++)
        {
            data += configs[i].difficulty + "," + configs[i].hp + "," + configs[i].moveAble + "," + configs[i].moveSpeed + "," + configs[i].sprintAble + "," + configs[i].sprintSpeed + "," + configs[i].hasAttack + "," + configs[i].attackDamage + "," + configs[i].hasSkill + "," + configs[i].skillDamage + "," + configs[i].skillCoolDown + ",\n";
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