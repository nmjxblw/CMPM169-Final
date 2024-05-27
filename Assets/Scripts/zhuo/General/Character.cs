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
    public bool invincible;
    public bool hurt;
    public UnityEvent<DamageDealer> onTakenDamage;
    public bool dead;
    public UnityEvent onDead;
    public Room generateRoom;

    public void OnEnable()
    {
        RefreshHp();
    }
    public void RefreshHp()
    {
        currentState = CharacterHealthState.Alive;
        hp = maxHp;
    }
    public void SetMaxHp(int value)
    {
        maxHp = value;
        RefreshHp();
    }
    void OnDisable()
    {
        onTakenDamage.RemoveAllListeners();
        onDead.RemoveAllListeners();
        generateRoom.killThisRoomEnemy(this.gameObject);
        
    }
    public void TakeDamage(DamageDealer damageDealer)
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
