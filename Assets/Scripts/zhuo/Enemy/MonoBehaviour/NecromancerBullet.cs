using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerBullet : MonoBehaviour
{
    public float currentSpeed = 0f;
    public float initialSpeed = 20f;
    public float maxSpeed = 40f;
    public float accelerateDuration = 2f;
    public float acceleration => (maxSpeed - initialSpeed) / (accelerateDuration * Time.fixedDeltaTime);
    public float lifeTime = 4f;
    private void OnEnable()
    {
        currentSpeed = initialSpeed;
        IEnumerator DisableSelf()
        {
            yield return new WaitForSeconds(lifeTime);
            gameObject.SetActive(false);
        }
        StartCoroutine(DisableSelf());
    }
    private void FixedUpdate()
    {
        currentSpeed = Mathf.Clamp(currentSpeed + acceleration, initialSpeed, maxSpeed);
        transform.Translate(currentSpeed * Time.fixedDeltaTime * Vector3.right);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
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
}
