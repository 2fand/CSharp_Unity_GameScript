using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Float : MonoBehaviour
{
    public float speed = 1;
    public float high = 1;
    private float x;
    public float ox;
    public float oy;
    public float oz;
    void Start()
    {
        x = -3.14f / speed;
        ox = transform.position.x;
        oy = transform.position.y;
        oz = transform.position.z;
    }

    void Update()
    {
        transform.position = new Vector3(ox, oy + Mathf.Sin(x * speed) * high, oz);
        x += 0.01f;
        if (x > 3.14f / speed)
        {
            x = -3.14f / speed;
        }
    }
}
