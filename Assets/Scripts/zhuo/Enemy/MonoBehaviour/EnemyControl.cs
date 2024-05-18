using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyControl : MonoBehaviour
{
    public Animator animator;
    #region  Animator Hashes
    public static readonly int walkHash = Animator.StringToHash("walk");
    public static readonly int idleHash = Animator.StringToHash("idle");
    public static readonly int attackHash = Animator.StringToHash("attack");
    public static readonly int hurtHash = Animator.StringToHash("hurt");
    public static readonly int deadHash = Animator.StringToHash("dead");
    public static readonly int skillHash = Animator.StringToHash("skill");
    public static readonly int jumpHash = Animator.StringToHash("jump");
    #endregion
    public float moveSpeed = 5f;
    public bool canInput = true;
    public Vector2 inputDirection;
    [Header("Face Direction")]
    public float initialFaceDirection;
    [Header("Take Damage")]
    public bool invincible = false;
    public UnityEvent onTakeDamage;
    public UnityEvent onDead;
    void OnEnable()
    {
        animator = animator ?? GetComponent<Animator>();
        AnimatorInitialization();
    }
    void AnimatorInitialization()
    {
        animator.SetBool(walkHash, false);
        animator.ResetTrigger(hurtHash);

    }
    void Start()
    {
        animator = animator ?? GetComponent<Animator>();
        initialFaceDirection = transform.localScale.x;
    }

    public void OnAnimatorMove()
    {
        inputDirection = canInput ? inputDirection.normalized : Vector2.zero;
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
        if (inputDirection.magnitude > 0.1f)
        {
            animator.SetBool(walkHash, true);
            transform.Translate((Vector3)inputDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool(walkHash, false);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }
    public void HandleAttack() { }

    public void HandleHurt()
    {
        if (invincible) return;
        animator.SetBool(hurtHash, true);
        onTakeDamage?.Invoke();
    }
    public void HandleDead()
    {
        animator.SetBool(deadHash, true);
        onDead?.Invoke();
        IEnumerator DispalyDead()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
        StartCoroutine(DispalyDead());
    }
}
