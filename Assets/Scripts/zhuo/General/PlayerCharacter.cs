using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCharacter : Character
{
    public PlayerConfig playerConfig;
    public GunController GunController;

    public override int maxHp
    {
        get
        {
            return _maxHp = playerConfig.maxHp;
        }
        set
        {
            playerConfig.maxHp = Math.Max(0, value);
            _maxHp = playerConfig.maxHp;
            UIUpdateEvent?.Invoke();
        }
    }
    public override float invincibleDuration
    {
        get
        {
            return _invincibleDuration = playerConfig.invincibleDuration;
        }
        protected set
        {
            playerConfig.invincibleDuration = Mathf.Max(0f, value);
            _invincibleDuration = playerConfig.invincibleDuration;
        }
    }

    public override void TakeDamage(DamageDealer damageDealer)
    {
        if (invincible || dead) return;
        else
        {
            int damage = Math.Max(0, damageDealer.damage - playerConfig.defense);
            if (damage > 0)
            {
                TriggerInvincible();
                hp -= damage;
                if (hp > 0)
                {
                    onTakenDamage?.Invoke(damageDealer);
                }
                else
                    currentState = CharacterHealthState.Dead;
                UIUpdateEvent?.Invoke();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Gun"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GunController.SelectGun(collision.GetComponent<Gun>().gunConfig.gunType);
                Destroy(collision.gameObject);
            }
        }
    }
}
