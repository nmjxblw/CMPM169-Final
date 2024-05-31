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
    [SerializeField]
    protected float _invincibleDuration = 0.5f;
    public float invincibleDuration { get { return _invincibleDuration; } protected set { _invincibleDuration = value; } }
    public float invincibleTimeRemaining;
    public UnityEvent onInvincibleStart;
    public UnityEvent onInvincibleEnd;
    public Coroutine invincibleCoroutine;
    public bool hurt;
    public UnityEvent<DamageDealer> onTakenDamage;
    public bool dead;
    public UnityEvent onDead;
    public UnityEvent UIUpdateEvent;

    public virtual void OnEnable()
    {
        onInvincibleStart.AddListener(TriggerInvincible);
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
        if (invincibleCoroutine != null)
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
        hp -= damageDealer.damage;
        if (hp > 0)
        {
            onInvincibleStart?.Invoke();
            onTakenDamage?.Invoke(damageDealer);
        }
        else
            currentState = CharacterHealthState.Dead;
        //TODO:UI Display
        UIUpdateEvent?.Invoke();
    }
    public virtual void TriggerInvincible()
    {
        if (invincibleCoroutine != null)
            StopCoroutine(invincibleCoroutine);
        invincibleCoroutine = StartCoroutine(InvincibleCoroutine());
    }

    public virtual IEnumerator InvincibleCoroutine()
    {
        invincible = true;
        invincibleTimeRemaining = invincibleDuration;
        while (invincibleTimeRemaining > 0)
        {
            invincibleTimeRemaining = Mathf.Clamp(invincibleTimeRemaining - Time.deltaTime, 0f, invincibleDuration);
            yield return null;
        }
        invincible = false;
        onInvincibleEnd?.Invoke();
    }
}
