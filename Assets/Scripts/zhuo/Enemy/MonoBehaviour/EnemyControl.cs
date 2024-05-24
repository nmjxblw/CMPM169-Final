using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyControl : MonoBehaviour
{
    [Header("Enemy Config")]
    public EnemyConfig enemyConfig;
    public EnemyConfig.Config currentConfig;
    [Header("Enemy Character")]
    public Character enemyCharacter;
    [Header("Animator Part")]
    public Animator animator;
    #region  Animator Hashes
    public static readonly int inputMagnitudeHash = Animator.StringToHash("inputMagnitude");
    public static readonly int idleHash = Animator.StringToHash("idle");
    public static readonly int attackHash = Animator.StringToHash("attack");
    public static readonly int hurtHash = Animator.StringToHash("hurt");
    public static readonly int deadHash = Animator.StringToHash("dead");
    public static readonly int skillHash = Animator.StringToHash("skill");
    public static readonly int jumpHash = Animator.StringToHash("jump");
    public static int baseLayerIndex;
    public static int hurtLayerIndex;
    #endregion
    [Header("Moving Setting")]
    public float moveSpeed = 5f;
    public bool isSprinting = false;
    public float sprintSpeed = 8f;
    public bool canInput = true;
    public const float threshold = 0.01f;
    public Vector2 inputDirection;
    [Header("Face Direction")]
    public float initialFaceDirection;
    [Header("Skill Info")]
    public bool _isSkill = false;
    public bool isSkill
    {
        get { return _isSkill; }
        set
        {
            if (_isSkill != value)
            {
                _isSkill = value;
                isSkillChanged?.Invoke(_isSkill);
            }
        }
    }
    public UnityEvent<bool> isSkillChanged;
    public bool _skillActivable = true;
    public bool skillActivable
    {
        get { return _skillActivable; }
        set
        {
            if (_skillActivable != value)
            {
                _skillActivable = value;
                skillActivableChanged?.Invoke(_skillActivable);
            }
        }
    }
    public UnityEvent<bool> skillActivableChanged;
    public const float skillCoolDown = 10f;
    public float skillCoolDownRemaining;
    [Header("Take Damage")]
    public bool invincible = false;
    public bool isHurt = false;
    public bool isDead = false;
    void OnEnable()
    {
        currentConfig = enemyConfig.configs[GameManager.levelDifficulty];
        canInput = true;
        isDead = false;
        isHurt = false;
        GetComponent<Collider2D>().enabled = true;
        animator = animator ?? GetComponent<Animator>();
        enemyCharacter = enemyCharacter ?? GetComponent<Character>();
        enemyCharacter.onTakenDamage.AddListener(HandleTakenDamage);
        enemyCharacter.onDead.AddListener(HandleDead);
        enemyCharacter.SetMaxHp(currentConfig.hp);
        transform.Find("DamageAreas/AttackArea").GetComponent<DamageDealer>().damage = currentConfig.attackDamage;
        transform.Find("DamageAreas/SkillAttackArea").GetComponent<DamageDealer>().damage = currentConfig.skillDamage;
        transform.Find("DamageAreas/SkillAttackArea").gameObject.SetActive(false);
        AnimatorInitialization();
    }
    void OnDisable()
    {
        enemyCharacter.onTakenDamage.RemoveListener(HandleTakenDamage);
        enemyCharacter.onDead.RemoveListener(HandleDead);
    }
    void AnimatorInitialization()
    {
        animator.SetBool(deadHash, false);
    }
    void Start()
    {
        animator = animator ?? GetComponent<Animator>();
        initialFaceDirection = transform.localScale.x;
        baseLayerIndex = animator.GetLayerIndex("Base Layer");
        hurtLayerIndex = animator.GetLayerIndex("Hurt Layer");
    }

    public void OnAnimatorMove()
    {
        if (isSkill || isDead || isHurt) return;
        inputDirection = canInput ? inputDirection.normalized : Vector2.zero;
        HandleFaceDirection();
        HandleMovement();
    }
    public void Update()
    {
        UpdateAnimatorValue();
    }
    void UpdateAnimatorValue()
    {
        animator.SetFloat(inputMagnitudeHash, inputDirection.sqrMagnitude);
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
            float speed = isSprinting ? sprintSpeed : moveSpeed;
            transform.Translate((Vector3)inputDirection * speed * Time.deltaTime);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }
    public void HandleAttack() { }
    public void HandleSkill()
    {
        if (!skillActivable) return;
        animator.SetBool(skillHash, true);
        StartCoroutine(SkillCoolDownCoroutine());
    }
    public IEnumerator SkillCoolDownCoroutine()
    {
        skillCoolDownRemaining = skillCoolDown;
        while (skillCoolDownRemaining > 0f)
        {
            skillCoolDownRemaining = Mathf.Clamp(skillCoolDownRemaining - Time.deltaTime, 0f, skillCoolDown);
            yield return null;
        }
        skillActivable = true;
    }

    [ContextMenu("Test Hurt")]
    public void HandleTakenDamage(DamageDealer damageDealer)
    {
        if (invincible) return;
        animator.Play(hurtHash, hurtLayerIndex);
        enemyCharacter.hp -= damageDealer.damage;
        transform.Translate(damageDealer.transform.rotation.eulerAngles.normalized * damageDealer.knockbackForce);
    }
    [ContextMenu("Test Dead")]
    public void HandleDead()
    {
        if (isDead) return;
        isDead = true;
        canInput = false;
        animator.Play(deadHash, hurtLayerIndex);
        GetComponent<Collider2D>().enabled = false;
    }
    public void HandleDeadAnimationDone()
    {
        IEnumerator DisableSelf()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
        StartCoroutine(DisableSelf());
    }
    [ContextMenu("Reset Dead")]
    public void ResetDead()
    {
        animator.SetBool(deadHash, false);
        StopAllCoroutines();
    }
}
