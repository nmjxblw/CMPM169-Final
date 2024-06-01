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
    public int maxHp { get { return _maxHp; } set { _maxHp = value; } }
    public bool invincible;
    [SerializeField]
    protected float _invincibleDuration;
    public float invincibleDuration { get { return _invincibleDuration; } protected set { _invincibleDuration = value; } }
    public float invincibleTimeRemaining;
    public UnityEvent onInvincibleStart;
    public UnityEvent onInvincibleEnd;
    public bool hurt;
    public UnityEvent<DamageDealer> onTakenDamage;
    public bool dead;
    public UnityEvent onDead;
    public UnityEvent UIUpdateEvent;

    public virtual void Awake()
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
    public virtual void SetInvincibleDuration(float duration)
    {
        invincibleDuration = duration;
        //Refresh invincible remaining time while get buff.
        if (invincible)
            invincibleTimeRemaining = duration;
    }
    protected virtual void OnDisable()
    {
        onInvincibleStart.RemoveAllListeners();
        onTakenDamage.RemoveAllListeners();
        onDead.RemoveAllListeners();
    }
    public virtual void TakeDamage(DamageDealer damageDealer)
    {
        if (invincible || dead) return;
        else
        {
            TriggerInvincible();
            hp -= damageDealer.damage;
            if (hp > 0)
            {
                onTakenDamage?.Invoke(damageDealer);
            }
            else
                currentState = CharacterHealthState.Dead;
            //TODO:UI Display
            UIUpdateEvent?.Invoke();
        }
    }
    public virtual void TriggerInvincible()
    {
        Debug.Log($"Trigger Invincible, Current invincible is {invincible}");
        invincible = true;
        invincibleTimeRemaining = invincibleDuration;
        onInvincibleStart?.Invoke();
    }

    public virtual void FixedUpdate()
    {
        if (invincible)
        {
            invincibleTimeRemaining -= Time.fixedDeltaTime;
            if (invincibleTimeRemaining <= 0f)
            {
                invincible = false;
                onInvincibleEnd?.Invoke();
            }
        }
    }
}
