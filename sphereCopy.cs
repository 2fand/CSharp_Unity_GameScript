using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class sphereCopy : MonoBehaviour
{
    public GameObject o;
    public int a = 11;
    public float splita = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        if (0 == a % 2)
        {
            a--;
        }
        for (int x = -a / 2; x <= a / 2; x++) {
            for (int y = -a / 2; y <= a / 2; y++)
            {
                for (int z = -a / 2; z <= a / 2; z++)
                {
                    if (x * x + y * y + z * z <= a * a / 4)
                    {
                        Instantiate(o, gameObject.transform.position + new Vector3(x * splita, y * splita, z * splita), Quaternion.Euler(Vector3.zero));
                    }
                }
            }
        }
        if (null != GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

}
