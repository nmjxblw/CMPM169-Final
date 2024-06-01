using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifeTime = 4;
    public bool rotated = false;
    public Coroutine disableCoroutine;
    public bool isShotgunBullet;
    public void OnEnable()
    {
        if (disableCoroutine != null)
            StopCoroutine(disableCoroutine);
        disableCoroutine = StartCoroutine(DisableBullet());

        if (isShotgunBullet)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

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
        transform.Translate((rotated ? -Vector3.right : Vector3.right) * speed * Time.deltaTime);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }

    public void SetBulletData(float speed, int damage, float knockbackForce)
    {
        this.speed = speed;
        if (isShotgunBullet)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DamageDealer damageDealer = transform.GetChild(i).GetComponent<DamageDealer>();
                damageDealer.damage = damage;
                damageDealer.knockbackForce = knockbackForce;
            }
        }
        else
        {
            DamageDealer damageDealer = GetComponent<DamageDealer>();
            damageDealer.damage = damage;
            damageDealer.knockbackForce = knockbackForce;
        }
    }
}
