using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public GunController GunController;
    public PlayerMovements PlayerMovements;

    public UnityEvent<GunType> onGunChanged;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gun"))
        {
            var gunType = collision.GetComponent<Gun>().GunType;
            ChangeGun(gunType);
            Destroy(collision.gameObject);
        }
    }

    // decreseas the time between each bullet
    public void AdjustFiringRat(float amount)
    {
        GunController.AdjustFiringInterval += amount;
    }

    public void ChangeGun(GunType gunType)
    {
        if (GunController.SelectGun(gunType))
        {
            onGunChanged?.Invoke(gunType);
        }
    }

    public void IncreaseSpeed(float speeed)
    {
        PlayerMovements.Speed += speeed;
    }

    public void DecreaseSpeed(float speeed)
    {
        PlayerMovements.Speed -= speeed;
    }
}
