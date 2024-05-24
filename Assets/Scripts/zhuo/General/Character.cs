using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public enum CharacterHealthState
{
    Alive,
    Dead
}
public class Character : MonoBehaviour
{
    public CharacterHealthState currentState = CharacterHealthState.Alive;

    [SerializeField]
    private int _hp;
    public int hp
    {
        get
        { return _hp; }
        set
        { _hp = Math.Clamp(value, 0, maxHp); }
    }
    [SerializeField]
    private int _maxHp;
    public int maxHp { get { return _maxHp; } private set { _maxHp = value; } }
    public UnityEvent<DamageDealer> onTakenDamage;
    public UnityEvent onDead;

    public void OnEnable()
    {
        currentState = CharacterHealthState.Alive;
        hp = maxHp;
    }
    public void TakeDamage(DamageDealer damageDealer)
    {
        onTakenDamage?.Invoke(damageDealer);
        if (hp <= 0 && currentState == CharacterHealthState.Alive)
        {
            currentState = CharacterHealthState.Dead;
            onDead?.Invoke();
        }
    }
}
