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

    public Gun Gun;

    private void Start()
    {
        Gun.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 0;

        RotateGun();

        if (Input.GetMouseButton(0))
        {
            Gun.gameObject.SetActive(true);
            Gun.Shoot(rotated, transform.rotation);
        }
    }

    public bool SelectGun(GunType gunType)
    {
        foreach (var gun in Guns)
        {
            if (gun.gunConfig.gunType == gunType)
            {
                Gun.gameObject.SetActive(false);
                gun.gameObject.SetActive(true);
                Gun = gun;
                return true;
            }
        }
        return false;
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
