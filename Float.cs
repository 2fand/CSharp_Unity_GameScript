using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    private float f = -6.28f;
    public float limit = 10;
    public float y = 0;
    void Start()
    {

    }

    void Update()
    {
        this.transform.position = new Vector3(transform.position.x, Mathf.Cos(f / 2.0f) * limit + y, transform.position.z);
        f += 0.01f;
        if (f > 6.28f)
        {
            f = -6.28f;
        }
    }
}
