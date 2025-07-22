using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Float : MonoBehaviour
{
    private float offset = 0;
    private float x = 0;
    private float h = 0;
    public float floatHigh = 1;
    void Start()
    {
        h = transform.position.y;
    }

    void Update()
    {
        offset = Mathf.Sin(x) * floatHigh;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, h + offset, gameObject.transform.position.z);
        if (!you.IsOpenMenu)
        {
            x += 0.01f;
        }
    }

    private void OnDisable()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, h, gameObject.transform.position.z);
    }
}
