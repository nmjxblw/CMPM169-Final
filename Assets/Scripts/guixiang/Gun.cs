using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; set; }
    public GameObject BulletPrefab;
    public GameObject BulletSpawnPos;
    public GameObject BulletFiring;
    public float ShootingInterval = 1;
    public GunType GunType;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Shoot(bool rotated, Quaternion rotation)
    {
        Bullet b = PoolManager.Release(BulletPrefab, BulletSpawnPos.transform.position, rotation).GetComponent<Bullet>();
        b.rotated = rotated;
        ShowBulletFiring();
    }

    public void ShowBulletFiring(float hideSeconds = 0.1f)
    {
        if (BulletFiring == null) return;

        BulletFiring.SetActive(true);
        Invoke(nameof(HideBulletFiring), hideSeconds);
    }

    private void HideBulletFiring()
    {
        BulletFiring.SetActive(false);
    }
}
