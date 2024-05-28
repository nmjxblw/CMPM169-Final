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
    public CharacterHealthState _currentState = CharacterHealthState.Alive;
    public CharacterHealthState currentState
    {
        get { return _currentState; }
        set
        {
            _currentState = value;
            if (_currentState == CharacterHealthState.Dead)
            {
                onDead?.Invoke();
            }
        }
    }

    [SerializeField]
    protected int _hp;
    public int hp
    {
        get
        { return _hp; }
        set
        { _hp = Math.Clamp(value, 0, maxHp); }
    }
    [SerializeField]
    protected int _maxHp;
    public int maxHp { get { return _maxHp; } protected set { _maxHp = value; } }
    public bool invincible;
    public bool hurt;
    public UnityEvent<DamageDealer> onTakenDamage;
    public bool dead;
    public UnityEvent onDead;

    public virtual void OnEnable()
    {
        RefreshHp();
    }
    public virtual void RefreshHp()
    {
        currentState = CharacterHealthState.Alive;
        hp = maxHp;
    }
    public virtual void SetMaxHp(int value)
    {
        maxHp = value;
        RefreshHp();
    }
    protected virtual void OnDisable()
    {
        onTakenDamage.RemoveAllListeners();
        onDead.RemoveAllListeners();
    }
    public virtual void TakeDamage(DamageDealer damageDealer)
    {
        if (invincible || dead) return;
        hp -= damageDealer.damage;
        if (hp > 0)
            onTakenDamage?.Invoke(damageDealer);
        else
            currentState = CharacterHealthState.Dead;
        //TODO:UI Display
    }
}
