using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DamageDealer : MonoBehaviour
{
    public int damage;
    public float knockbackForce;
    public void OnTriggerStay2D(Collider2D other)
    {
        if (gameObject.CompareTag(other.gameObject.tag))
        {
            return;
        }
        else if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Character>()?.TakeDamage(this);
        }
    }
}
