using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; set; }
    public GameObject BulletSpawnPos;
    public GameObject BulletFiring;
    public bool canShot;
    public float intervalRemaining;
    public GunConfig gunConfig;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Shoot(bool rotated, Quaternion rotation)
    {
        if (!canShot) return;
        canShot = false;
        Bullet b = PoolManager.Release(gunConfig.bulletPrefab, BulletSpawnPos.transform.position, rotation).GetComponent<Bullet>();
        b.rotated = rotated;
        b.SetBulletData(gunConfig.bulletSpeed, gunConfig.damage, gunConfig.knockbackForce);
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
    public void FixedUpdate()
    {
        if (!canShot)
        {
            intervalRemaining -= Time.fixedDeltaTime;
            if (intervalRemaining <= 0)
            {
                canShot = true;
                intervalRemaining = gunConfig.fireInterval;
            }
        }
    }
}
