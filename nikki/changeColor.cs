using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColor : MonoBehaviour
{
    private float c;
    void Start()
    {
        
    }

    void Update()
    {
        c = Mathf.Sin(Time.time / 10) * 0.5f + 0.5f;
        GetComponent<MeshRenderer>().material.color = new Color(c, c, c);
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(c, c, c));
    }
}
