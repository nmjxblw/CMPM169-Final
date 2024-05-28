using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [Header("Player Character")]
    public PlayerCharacter playerCharacter;
    public float Speed = 10f;
    public float RollingSpeed = 20f;
    public float RollingCoolDownTimer = 0.5f;
    public Animator PlayerAnimator;

    [Header("Sprite Renderers")]
    public SpriteRenderer PlayerBodySprite;

    private string _walkingAnimName = "Walking";
    private string _rollAnimName = "Roll";

    [Header("Hurt and Dead")]
    public static readonly int hurtHash = Animator.StringToHash("hurt");
    public static readonly int deadHash = Animator.StringToHash("dead");
    public static readonly int invincibleHash = Animator.StringToHash("invincible");
    public int hurtLayerIndex;
    public int invincibleLayerIndex;
    private float _curSpeed;
    private bool _canRoll = true;

    private float vertical;
    private float horizontal;
    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = playerCharacter ?? GetComponent<PlayerCharacter>();
        playerCharacter.onTakenDamage.AddListener(HandleTakenDamage);
        playerCharacter.onDead.AddListener(HandleDead);
        playerCharacter.onInvincibleStart.AddListener(HandleInvincibleStart);
        playerCharacter.onInvincibleEnd.AddListener(HandleInvincibleEnd);
        _curSpeed = Speed;
        hurtLayerIndex = PlayerAnimator.GetLayerIndex("Hurt Layer");
        invincibleLayerIndex = PlayerAnimator.GetLayerIndex("Invincible Layer");
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        if (playerCharacter.hurt || playerCharacter.dead)
        {
            PlayerAnimator.SetFloat(_walkingAnimName, 0);
        }
        else
        {
            transform.Translate(Vector3.up * vertical * _curSpeed * Time.deltaTime);
            transform.Translate(Vector3.right * horizontal * _curSpeed * Time.deltaTime);

            PlayerAnimator.SetFloat(_walkingAnimName, Mathf.Abs(vertical) + Mathf.Abs(horizontal));

            var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x < playerScreenPoint.x)
            {
                PlayerBodySprite.flipX = true;
            }
            else
            {
                PlayerBodySprite.flipX = false;
            }
        }
        // rotate player towards mouse pos


        if (!PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("roll_anim"))
        {
            PlayerAnimator.SetBool(_rollAnimName, false);
            _curSpeed = Speed;
        }
        if (_canRoll && Input.GetKeyDown(KeyCode.Space))
        {
            _canRoll = false;
            PlayerAnimator.SetBool(_rollAnimName, true);
            _curSpeed = RollingSpeed;
            Invoke(nameof(UpdateCanRoll), RollingCoolDownTimer);
        }
    }

    void UpdateCanRoll()
    {
        _canRoll = true;
    }

    public void HandleTakenDamage(DamageDealer damageDealer)
    {
        PlayerAnimator.Play(hurtHash, hurtLayerIndex);
        //TODO: Apply Hurt Knockback Force
        PlayerBodySprite.flipX = damageDealer.transform.position.x > transform.position.x;
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
