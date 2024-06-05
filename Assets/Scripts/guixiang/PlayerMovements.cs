using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [Header("Player Config")]
    public PlayerConfig playerConfig;
    [Header("Player Character")]
    public PlayerCharacter playerCharacter;
    public Rigidbody2D rb;
    public float rollingCoolDownRemainingTime;
    public float rollingTimeRemaining;
    public Animator PlayerAnimator;

    [Header("Sprite Renderers")]
    public SpriteRenderer PlayerBodySprite;

    private readonly int walkingHash = Animator.StringToHash("Walking");
    private readonly int rollHash = Animator.StringToHash("Roll");

    [Header("Hurt and Dead")]
    public static readonly int hurtHash = Animator.StringToHash("hurt");
    public static readonly int deadHash = Animator.StringToHash("dead");
    public static readonly int invincibleHash = Animator.StringToHash("invincible");
    public int hurtLayerIndex;
    public int invincibleLayerIndex;
    private float curSpeed;
    public bool isRolling = false;
    public bool canRoll = true;
    public Vector2 inputDirection;
    public Vector2 lastInputDirection = new Vector2(1, 0);
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCharacter = playerCharacter ?? GetComponent<PlayerCharacter>();
        playerCharacter.onTakenDamage.AddListener(HandleTakenDamage);
        playerCharacter.onDead.AddListener(HandleDead);
        playerCharacter.onInvincibleStart.AddListener(HandleInvincibleStart);
        playerCharacter.onInvincibleEnd.AddListener(HandleInvincibleEnd);
        hurtLayerIndex = PlayerAnimator.GetLayerIndex("Hurt Layer");
        invincibleLayerIndex = PlayerAnimator.GetLayerIndex("Invincible Layer");
    }

    // Update is called once per frame
    public void Update()
    {
        inputDirection.x = Input.GetAxis("Horizontal");
        inputDirection.y = Input.GetAxis("Vertical");
        inputDirection = inputDirection.normalized;
        lastInputDirection = inputDirection.sqrMagnitude > 0 ? inputDirection : lastInputDirection;
        UpdateAnimatorValue();
    }
    public void HandleSpriteFlip()
    {
        var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Input.mousePosition;
        PlayerBodySprite.flipX = mousePos.x < playerScreenPoint.x;
    }


    public void UpdateAnimatorValue()
    {
        PlayerAnimator.SetFloat(walkingHash, inputDirection.sqrMagnitude);
        PlayerAnimator.SetBool(rollHash, Input.GetKey(KeyCode.Space) && canRoll);
    }

    public void FixedUpdate()
    {
        if (playerCharacter.hurt || playerCharacter.dead || isRolling)
        {
            inputDirection = Vector2.zero;
        }
        else
        {
            HandleSpriteFlip();
            HandlePlayerMoving();
        }
        RollingCoolDownFixedUpdate();
        FixedUpdateMaxRollingTime();
    }
    public void HandlePlayerMoving()
    {
        rb.velocity = inputDirection * playerConfig.moveSpeed;
    }

    void RollingCoolDownFixedUpdate()
    {
        if (!canRoll)
        {
            rollingCoolDownRemainingTime -= Time.fixedDeltaTime;
            if (rollingCoolDownRemainingTime <= 0)
            {
                canRoll = true;
            }
        }
    }

    void FixedUpdateMaxRollingTime()
    {
        if (isRolling)
        {
            rollingTimeRemaining -= Time.fixedDeltaTime;
            if (rollingTimeRemaining <= 0)
            {
                canRoll = false;
            }
        }
    }

    public void HandleTakenDamage(DamageDealer damageDealer)
    {
        PlayerAnimator.Play(hurtHash, hurtLayerIndex);
        rb.velocity = Vector2.zero;
        PlayerBodySprite.flipX = damageDealer.transform.position.x < transform.position.x;
        Vector2 dir = new Vector2(transform.position.x - transform.position.x, transform.position.y - transform.position.y).normalized;
        rb.AddForce(dir * damageDealer.knockbackForce, ForceMode2D.Impulse);
    }

    public void HandleDead()
    {
        GameManager.Instance.GameOver = true;
        rb.velocity = Vector2.zero;
        PlayerAnimator.Play(deadHash, hurtLayerIndex);
    }

    public void HandleInvincibleStart()
    {
        PlayerAnimator.Play(invincibleHash, invincibleLayerIndex);
    }
    public void HandleInvincibleEnd()
    {
        PlayerAnimator.Play("empty", invincibleLayerIndex);
    }
}
