using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public enum GunType
{
    Pistol,
    AutomaticRifle,
    Shotgun
}


[CreateAssetMenu(fileName = "GunConfig", menuName = "GunConfig")]
public class GunConfig : ScriptableObject, IComparable<GunConfig>
{
    public TextAsset textAsset;
    public GunType gunType;
    public int damage;
    public float knockbackForce;
    public float fireInterval;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    
    public int CompareTo(GunConfig other)
    {
        return this.gunType.CompareTo(other.gunType);
    }

    // public void OnEnable()
    // {
    //     ReadDataFromCSV();
    //     Debug.Log("Run");
    // }

    [ContextMenu("ReadDataFromCSV")]
    public void ReadDataFromCSV()
    {
        textAsset = textAsset == null ? Resources.Load<TextAsset>($"GunConfigs/{gunType.ToString()}Config") : textAsset;
        if (textAsset == null) return;
        string[] lines = textAsset.text.Split('\n');
        string[] values = lines[1].Split(',');
        Enum.TryParse(values[0].Trim(), true, out gunType);
        damage = int.Parse(values[1].Trim());
        knockbackForce = float.Parse(values[2].Trim());
        fireInterval = float.Parse(values[3].Trim());
        bulletPrefab = Resources.Load<GameObject>($"Prefabs/{values[4].Trim()}");
        bulletSpeed = float.Parse(values[5].Trim());
    }
}
