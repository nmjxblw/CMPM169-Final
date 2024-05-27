using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked;
    public bool isUp, isDown, isLeft, isRight;
    public float cooldownTime = 1f;
    private bool isCooldown = false;

    void Start()
    {

    }

    void Update()
    {
        isLocked = transform.parent.GetComponent<Room>().isLocked;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCooldown)
        {
            if (!isLocked)
            {
                if (isUp)
                {
                    other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 140f, other.transform.position.z);
                }
                else if (isDown)
                {
                    other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y - 140f, other.transform.position.z);
                }
                else if (isLeft)
                {
                    other.transform.position = new Vector3(other.transform.position.x - 140f, other.transform.position.y, other.transform.position.z);
                }
                else if (isRight)
                {
                    other.transform.position = new Vector3(other.transform.position.x + 140f, other.transform.position.y, other.transform.position.z);
                }

                StartCoroutine(StartCooldown());
            }
        }
    }

    private IEnumerator StartCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
}
