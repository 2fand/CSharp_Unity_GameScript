using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMove : MonoBehaviour
{
    public Vector2 face;
    private Vector3 beforePos;
    float moveX = 0;
    float moveY = 0;
    bool isEnd = true;
    void Start()
    {
        beforePos = transform.localPosition;
    }

    IEnumerator moveBackGround()
    {
        isEnd = false;
        if (0 != face.x)
        {
            transform.localPosition += new Vector3(face.x / 10, 0, 0);
            moveX += Mathf.Abs(face.x) / 10;
        }
        if (0 != face.y)
        {
            transform.localPosition += new Vector3(0, 0, face.y / 10);
            moveY += Mathf.Abs(face.y) / 10;
        }
        if (moveX > map.Map.heightX) 
        {
            moveX = 0;
            Vector3 v = transform.localPosition;
            v.x = beforePos.x;
            transform.localPosition = v;
        }
        if (moveY > map.Map.widthY)
        {
            moveY = 0;
            Vector3 v = transform.localPosition;
            v.z = beforePos.z;
            transform.localPosition = v;
        }
        isEnd = true;
        yield return null;
    }

    void Update()
    {
        if (isEnd)
        {
            StartCoroutine(moveBackGround());
        }
    }
}
