using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class rainbow : MonoBehaviour
{
    private float h, s, v;
    public Color color;
    void Start()
    {
        if (0 == color.a || 0 == color.r && 0 == color.g && 0 == color.b || 1 == color.r && 1 == color.g && 1 == color.b)
        {
            color = Color.red;
        }
        GetComponent<MeshRenderer>().material.color = color;
    }

    void Update()
    {
        Color.RGBToHSV(GetComponent<MeshRenderer>().material.color, out h, out s, out v);
        Color addc = Color.HSVToRGB(h + 1 / 360.0f, s, v);
        addc.a = GetComponent<MeshRenderer>().material.color.a;
        GetComponent<MeshRenderer>().material.color = addc;
    }
}

