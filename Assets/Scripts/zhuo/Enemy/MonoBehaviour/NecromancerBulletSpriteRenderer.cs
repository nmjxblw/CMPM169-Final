using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerBulletSpriteRenderer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float angle;
    public bool flipY;
    public void OnEnable()
    {
        angle = transform.parent.localEulerAngles.z;
        flipY = (angle >= 90f && angle <= 270f);
        spriteRenderer.flipY = flipY;
    }
}
