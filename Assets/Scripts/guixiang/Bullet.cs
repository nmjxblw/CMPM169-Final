using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 1;
    public float lifeTime = 4;
    public bool rotated = false;
    public Coroutine disableCoroutine;
    public void OnEnable()
    {
        if (disableCoroutine != null)
            StopCoroutine(disableCoroutine);
        disableCoroutine = StartCoroutine(DisableBullet());
    }

    private IEnumerator DisableBullet()
    {
        float timer = lifeTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((rotated ? -Vector3.right : Vector3.right) * Speed * Time.deltaTime);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "Wall")
        {
            this.gameObject.SetActive(false);
        }
    }
}
