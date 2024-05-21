using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Gun[] Guns;

    private Vector3 mousePos;
    private Vector3 objectPos;
    private float rotateAngle;
    private bool rotated;

    private bool CanShoot = true;

    private Gun Gun;

    private void Start()
    {
        foreach (var gun in Guns)
        {
            gun.gameObject.SetActive(false);
        }
        Gun = Guns[0];
        Gun.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        SelectGun();

        mousePos = Input.mousePosition;
        mousePos.z = 0;

        RotateGun();

        if (Input.GetMouseButton(0) && CanShoot)
        {
            Gun.Shoot(rotated, transform.rotation);
            CanShoot = false;
            Invoke(nameof(UpdateCanShoot), Gun.ShootingInterval);
        }
    }

    private void SelectGun()
    {
        bool selected = false;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Gun = Guns[0];
            selected = true;
        } else if (Input.GetKeyDown(KeyCode.Alpha2) && Guns.Length > 1)
        {
            Gun = Guns[1];
            selected = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && Guns.Length > 2)
        {
            Gun = Guns[2];
            selected = true;
        }

        if (selected)
        {
            foreach (var gun in Guns)
            {
                gun.gameObject.SetActive(false);
            }
            Gun.gameObject.SetActive(true);
        }
    }

    private void UpdateCanShoot()
    {
        CanShoot = true;
    }

    private void RotateGun()
    {
        objectPos = Camera.main.WorldToScreenPoint(transform.position);
        var rotation = Gun.gameObject.transform.localRotation;
        rotated = false;
        rotation.y = 0;
        if (mousePos.x < objectPos.x)
        {
            rotation.y = 180;
            rotated = true;
        }

        int playerOrder = 5;
        if (mousePos.y < objectPos.y)
        {
            Gun.SpriteRenderer.sortingOrder = playerOrder + 1;
        } 
        else
        {
            Gun.SpriteRenderer.sortingOrder = playerOrder - 1;
        }

        Gun.gameObject.transform.localRotation = rotation;

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        rotateAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if (rotated) rotateAngle -= 180;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateAngle));
    }
}
