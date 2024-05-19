using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float Speed = 10f;
    public Animator PlayerAnimator;

    [Header("Sprite Renderers")]
    public SpriteRenderer PlayerBodySprite;

    private float vertical;
    private float horizontal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.up * vertical * Speed * Time.deltaTime);
        transform.Translate(Vector3.right * horizontal * Speed * Time.deltaTime);

        PlayerAnimator.SetFloat("Walking", Mathf.Abs(vertical) + Mathf.Abs(horizontal));

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
    }
}
