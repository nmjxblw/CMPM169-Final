using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject GunWrapper;
    public Gun Gun;

    private Vector3 mousePos;
    private Vector3 objectPos;
    private float rotateAngle;
    private bool rotated;

    private bool CanShoot = true;
    

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 0;

        RotateGun();

        if (Input.GetMouseButton(0) && CanShoot)
        {
            // shoot a bullet
            CanShoot = false;
            Quaternion quaternion = Quaternion.Euler(GunWrapper.transform.rotation.x, rotated ? GunWrapper.transform.rotation.y - 180: GunWrapper.transform.rotation.y, GunWrapper.transform.rotation.z);
            Instantiate(Gun.BulletPrefab, Gun.BulletSpawnPos.transform.position, GunWrapper.transform.rotation);
            Invoke(nameof(UpdateCanShoot), Gun.ShootingInterval);
        }
    }

    private void UpdateCanShoot()
    {
        CanShoot = true;
    }

    private void RotateGun()
    {
        objectPos = Camera.main.WorldToScreenPoint(GunWrapper.transform.position);
        var rotation = Gun.gameObject.transform.localRotation;
        rotated = false;
        rotation.y = 0;
        if (mousePos.x < objectPos.x)
        {
            rotation.y = 180;
            rotated = true;
        }
        Gun.gameObject.transform.localRotation = rotation;

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        rotateAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if (rotated) rotateAngle -= 180;
        GunWrapper.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateAngle));
    }
}
