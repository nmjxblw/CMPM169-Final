using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using System;
using UnityEngine.Events;

public class NecromancerControl : MonoBehaviour
{
    [Header("Basic Component")]
    public NecromancerConfig necromancerConfig;
    public NecromancerConfig.Config currentConfig;
    public Animator animator;
    public EnemyCharacter enemyCharacter;
    [Header("Input Detector")]
    public bool canInput = true;
    public const float threshold = 0.01f;
    public Vector2 inputDirection;
    public bool isSprinting = false;
    [Header("Face Direction")]
    public float initialFaceDirection;
    [Header("Animation Setting")]
    public int basicLayerIndex;
    public int hurtLayerIndex;
    public static readonly int idleHash = Animator.StringToHash("idle");
    public static readonly int inputMagnitude = Animator.StringToHash("inputMagnitude");
    public static readonly int attackHash = Animator.StringToHash("attack");
    public static readonly int skill1Hash = Animator.StringToHash("skill1");
    public static readonly int skill2Hash = Animator.StringToHash("skill2");
    public static readonly int hurtHash = Animator.StringToHash("hurt");
    public static readonly int hitReactionHash = Animator.StringToHash("hit_reaction");
    public static readonly int deadHash = Animator.StringToHash("dead");
    [Header("Attack Target")]
    public Transform target;
    [Header("Damage Relative")]
    public bool isAttack = false;
    public GameObject attackArea;
    public Transform rodTop;
    public GameObject attackBullet;
    [Header("Skill Setting")]
    public bool isSkill = false;
    public bool skill1Activatable = true;
    public float skill1RemainingTime;

    public bool skill2Activatable = true;
    public float skill2RemainingTime;

    public void OnEnable()
    {

        UpdateConfig();
    }
    public void UpdateConfig()
    {
        GetComponent<Collider2D>().enabled = true;
        enemyCharacter = enemyCharacter ?? GetComponent<EnemyCharacter>();
        currentConfig = necromancerConfig.configs[GameManager.levelDifficulty];
        enemyCharacter.SetMaxHp(currentConfig.hp);
        enemyCharacter.SetInvincibleDuration(currentConfig.invincibleDuration);
        attackArea = attackArea == null ? transform.Find("AttackArea").gameObject : attackArea;
        attackArea.SetActive(true);
        attackArea.GetComponent<DamageDealer>().damage = currentConfig.attackDamage;
    }

    public void Start()
    {
        animator = animator == null ? GetComponent<Animator>() : animator;
        basicLayerIndex = animator.GetLayerIndex("Basic Layer");
        hurtLayerIndex = animator.GetLayerIndex("Hurt Layer");
        target = target == null ? GameObject.FindGameObjectWithTag("Player").transform : target;
        rodTop = rodTop == null ? transform.Find("RodTop") : rodTop;
        attackBullet = attackBullet == null ? Resources.Load<GameObject>("Prefabs/NecromancerBullet") : attackBullet;
        enemyCharacter.onTakenDamage.AddListener(HandleTakenDamage);
        enemyCharacter.onDead.AddListener(HandleDead);
        initialFaceDirection = transform.localScale.x;
    }

    public void OnAnimatorMove()
    {
        inputDirection = canInput ? inputDirection.normalized : Vector2.zero;
        if (enemyCharacter.hurt || enemyCharacter.dead || isSkill) return;
        HandleFaceDirection();
        HandleMovement();
    }

    public void HandleFaceDirection()
    {
        float localScaleX = transform.localScale.x * inputDirection.x * initialFaceDirection >= 0 ? transform.localScale.x : -transform.localScale.x;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    public void HandleMovement()
    {
        if (inputDirection.sqrMagnitude > threshold)
        {
            float speed = isSprinting ? currentConfig.sprintSpeed : currentConfig.moveSpeed;
            transform.Translate((Vector3)inputDirection * speed * Time.deltaTime);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }
    public void Update()
    {
        UpdateAnimatorValue();
    }
    public void UpdateAnimatorValue()
    {
        animator.SetFloat(inputMagnitude, inputDirection.magnitude);
    }
    public void FaceToTarget()
    {
        float localScaleX = transform.localScale.x * initialFaceDirection * (target.position.x - transform.position.x) >= 0 ? transform.localScale.x : -transform.localScale.x;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    public void HandleAttack()
    {
        FaceToTarget();
        animator.Play(attackHash, basicLayerIndex);
    }

    public void FireBullet()
    {
        Vector2 bulletDirection = new Vector2(target.position.x - rodTop.position.x, target.position.y - rodTop.position.y).normalized;
        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
        PoolManager.Release(attackBullet, rodTop.position, Quaternion.Euler(new Vector3(0, 0, angle))).GetComponent<DamageDealer>().damage = currentConfig.attackDamage;
    }

    public void HandleSkill1()
    {
        if (!skill1Activatable) return;
        FaceToTarget();
        animator.Play(skill1Hash, basicLayerIndex);
        IEnumerator StartSkill1Cooldown()
        {
            skill1Activatable = false;
            skill1RemainingTime = currentConfig.skill1Cooldown;
            while (skill1RemainingTime > 0f)
            {
                skill1RemainingTime -= Time.deltaTime;
                yield return null;
            }
            skill1Activatable = true;
        }
        StartCoroutine(StartSkill1Cooldown());
    }

    public void DisplaySkill1()
    {
        for (int i = 0; i < 16; i++)
        {
            float angle = 22.5f * i;
            Vector3 pos = rodTop.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f) * 10f;
            PoolManager.Release(attackBullet, pos, Quaternion.Euler(new Vector3(0, 0, angle))).GetComponent<DamageDealer>().damage = currentConfig.skill1Damage;
        }
    }

    public void HandleSkill2()
    {
        if (!skill2Activatable) return;
        FaceToTarget();
        animator.Play(skill2Hash, basicLayerIndex);
        IEnumerator StartSkill2Cooldown()
        {
            skill2Activatable = false;
            skill2RemainingTime = currentConfig.skill2Cooldown;
            while (skill2RemainingTime > 0f)
            {
                skill2RemainingTime -= Time.deltaTime;
                yield return null;
            }
            skill2Activatable = true;
        }
        StartCoroutine(StartSkill2Cooldown());
    }

    public void DisplaySkill2()
    {
        for (int i = 0; i < 8; i++)
        {
            float angle = 45f * i;
            Vector3 pos = target.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f) * 100f;
            PoolManager.Release(attackBullet, pos, Quaternion.Euler(new Vector3(0, 0, angle + 180f))).GetComponent<DamageDealer>().damage = currentConfig.skill2Damage;
        }
    }

    public void HandleTakenDamage(DamageDealer damageDealer)
    {
        if (isAttack || isSkill)
        {
            animator.Play(hitReactionHash, hurtLayerIndex);
            return;
        }
        animator.Play(hurtHash, hurtLayerIndex);
        float localScaleX = transform.localScale.x * initialFaceDirection * (transform.position.x - damageDealer.transform.position.x) <= 0 ? transform.localScale.x : -transform.localScale.x;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        IEnumerator Knockback()
        {
            float timer = 0.5f;
            while (timer > 0f)
            {
                transform.Translate(damageDealer.transform.rotation.eulerAngles.normalized * damageDealer.knockbackForce * Time.deltaTime);
                timer -= Time.deltaTime;
                yield return null;
            }
        }
        StartCoroutine(Knockback());
    }

    public void HandleDead()
    {
        animator.Play(deadHash, hurtLayerIndex);
        attackArea.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        StopAllCoroutines();
    }

    public void HandleDeadAnimationDone()
    {
        IEnumerator DisableCoroutine()
        {
            float timer = 0.5f;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }
        StartCoroutine(DisableCoroutine());
    }
}
