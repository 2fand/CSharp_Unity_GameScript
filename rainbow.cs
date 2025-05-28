using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class rainbow : MonoBehaviour
{
    private float h, s, v;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(Random.Range(0.00f, 1.00f), 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Color.RGBToHSV(GetComponent<MeshRenderer>().material.color, out h, out s, out v);
        GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(h + 1 / 360.0f, s, v);
    }
}
