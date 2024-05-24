using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DamageDealer : MonoBehaviour
{
    public enum DamageTarget
    {
        Player,
        Enemy,
        Both
    }
    public DamageTarget damageTarget;
    public int damage;
    public float knockbackForce;
    public void OnTriggerStay2D(Collider2D other)
    {
        if (gameObject.CompareTag(other.gameObject.tag))
        {
            return;
        }
        else if (other.gameObject.CompareTag(damageTarget.ToString()) || damageTarget == DamageTarget.Both)
        {
            other.gameObject.GetComponent<Character>()?.TakeDamage(this);
        }
    }
}
