using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 1;
    public bool rotated = false;
    public int Damage;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate((rotated ? -Vector3.right : Vector3.right) * Speed * Time.deltaTime);
    }
}
