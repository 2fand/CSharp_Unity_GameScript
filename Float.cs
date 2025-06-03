using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    private float f;
    private bool b;

    void Start()
    {
        f = UnityEngine.Random.Range(-0.5f, 0.5f);
        this.transform.localPosition += new Vector3(0, f, 0);
        b = f > 0;
    }

    void Update()
    {
        if (f < -0.5f)
        {
            b = false;
            f = -0.45f;
        }
        else if(f > 0.5f)
        {
            b = true;
            f = 0.45f;
        }
        if (b)
        {
            f -= 0.05f;
        }
        else
        {
            f += 0.05f;
        }
        this.transform.localPosition += new Vector3(0, f, 0);
    }
}
