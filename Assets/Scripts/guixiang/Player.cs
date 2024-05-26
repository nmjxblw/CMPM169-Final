using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GunController GunController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gun"))
        {
            GunController.SelectGun(collision.GetComponent<Gun>().GunType);
            Destroy(collision.gameObject);
        }
    }
}
