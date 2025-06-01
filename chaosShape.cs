using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaosShape : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        Vector3[] vector3s = GetComponent<MeshFilter>().mesh.vertices;
        for (int i = 0; i < 24; i++)
        {
            vector3s[i] = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        }
        GetComponent<MeshFilter>().mesh.SetVertices(vector3s);
        GetComponent<MeshFilter>().mesh.RecalculateNormals();
    }
}
