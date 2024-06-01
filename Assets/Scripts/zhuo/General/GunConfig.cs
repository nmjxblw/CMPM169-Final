using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GunType
{
    R1, R2, R3
}


[CreateAssetMenu(fileName = "GunConfig", menuName = "GunConfig")]
public class GunConfig : ScriptableObject, IComparable<GunConfig>
{
    public GunType gunType;
    public int basicDamage;
    public float basicKnockbackForce;
    public float fireInterval;
    public GameObject bulletPrefab;
    public int CompareTo(GunConfig other)
    {
        return this.gunType.CompareTo(other.gunType);
    }
}
