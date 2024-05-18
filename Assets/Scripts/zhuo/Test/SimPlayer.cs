using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimPlayer : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }
}
