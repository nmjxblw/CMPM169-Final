using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletAnim : MonoBehaviour
{
    public float duration = 0.25f;
    public float maxMargin = 4f;
    void OnEnable()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 dir = Vector3.up * (i - 1);
                StartCoroutine(MovingAnim(transform.GetChild(i), dir));
            }
        }
    }
    void OnDisable()
    {
        StopAllCoroutines();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    IEnumerator MovingAnim(Transform transform, Vector3 dir)
    {
        float timer = duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.localPosition = dir * Mathf.Lerp(0f, maxMargin, 1 - timer / duration);
            yield return null;
        }
        transform.localPosition = dir * maxMargin;
    }
}
