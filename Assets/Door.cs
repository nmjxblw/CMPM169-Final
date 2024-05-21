using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked;
    public bool isUp, isDown, isLeft, isRight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //检测player入门，如果进入大门且大门已经解锁，则player进入对应方向的下一个房间的对应门
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isLocked)
            {
                if(isUp)
                {
                    other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 120f, other.transform.position.z);
                }
                else if(isDown)
                {
                    other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y - 120f, other.transform.position.z);
                }
                else if(isLeft)
                {
                    other.transform.position = new Vector3(other.transform.position.x - 120f, other.transform.position.y, other.transform.position.z);
                }
                else if(isRight)
                {
                    other.transform.position = new Vector3(other.transform.position.x + 120f, other.transform.position.y, other.transform.position.z);
                }
            }
        }

    }
}
