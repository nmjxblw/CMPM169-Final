using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyControl : MonoBehaviour
{
    public Animator animator;
    #region  Animator Hashes
    public static readonly int inputMagnitudeHash = Animator.StringToHash("inputMagnitude");
    public static readonly int idleHash = Animator.StringToHash("idle");
    public static readonly int attackHash = Animator.StringToHash("attack");
    public static readonly int hurtHash = Animator.StringToHash("hurt");
    public static readonly int deadHash = Animator.StringToHash("dead");
    public static readonly int skillHash = Animator.StringToHash("skill");
    public static readonly int jumpHash = Animator.StringToHash("jump");
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
    public bool isSkill = false;
    public bool skillActivable = true;
    public const float skillCoolDown = 10f;
    public float skillCoolDownRemaining;
    [Header("Take Damage")]
    public bool invincible = false;
    public bool isHurt = false;
    public UnityEvent onTakeDamage;
    public bool isDead = false;
    public UnityEvent onDead;
    void OnEnable()
    {
        animator = animator ?? GetComponent<Animator>();
        AnimatorInitialization();
    }
    void AnimatorInitialization()
    {
        animator.ResetTrigger(hurtHash);
    }
    void Start()
    {
        animator = animator ?? GetComponent<Animator>();
        initialFaceDirection = transform.localScale.x;
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
    public void HandleHurt()
    {
        if (invincible) return;
        animator.SetTrigger(hurtHash);
        onTakeDamage?.Invoke();
    }
    [ContextMenu("Test Dead")]
    public void HandleDead()
    {
        animator.SetBool(deadHash, true);
        onDead?.Invoke();
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
