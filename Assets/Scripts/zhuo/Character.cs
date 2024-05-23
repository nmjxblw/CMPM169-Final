using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Character : MonoBehaviour
{
    public int _hp;
    public int hp
    {
        get
        { return _hp; }
        set
        { _hp = Math.Clamp(value, 0, maxHp); }
    }
    public int _maxHp;
    public int maxHp { get { return _maxHp; } private set { _maxHp = value; } }
    public UnityEvent<int> onTakenDamage;
    public UnityEvent onDead;

    public void TakeDamage(int damage)
    {
        onTakenDamage?.Invoke(damage);
        if (hp <= 0)
        {
            onDead?.Invoke();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            Destroy(this.gameObject);
        }
    }
}
