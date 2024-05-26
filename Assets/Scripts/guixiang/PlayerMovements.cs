using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float Speed = 10f;
    public float RollingSpeed = 20f;
    public float RollingCoolDownTimer = 0.5f;
    public Animator PlayerAnimator;

    [Header("Sprite Renderers")]
    public SpriteRenderer PlayerBodySprite;

    private string _walkingAnimName = "Walking";
    private string _rollAnimName = "Roll";

    private float _curSpeed;
    private bool _canRoll = true;

    private float vertical;
    private float horizontal;
    // Start is called before the first frame update
    void Start()
    {
        _curSpeed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.up * vertical * _curSpeed * Time.deltaTime);
        transform.Translate(Vector3.right * horizontal * _curSpeed * Time.deltaTime);

        PlayerAnimator.SetFloat(_walkingAnimName, Mathf.Abs(vertical) + Mathf.Abs(horizontal));

        // rotate player towards mouse pos
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
}
