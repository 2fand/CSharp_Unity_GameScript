using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class fly : MonoBehaviour
{
    public GameObject o;
    public float r = 0.5f;
    public float summonTime = 0.1f;
    public float speed = 0.618f;
    public float dieTime = 0.16f;
    private bool isEnd = true;
    IEnumerator shot()
    {
        isEnd = false;
        yield return new WaitForSeconds(summonTime);
        Instantiate(o, this.transform.position, Quaternion.Euler(Vector3.zero));
        isEnd = true;
    }

    void Start()
    {
        if (null == o.GetComponent<addTransform>())
        {
            o.transform.AddComponent<addTransform>();
        }
        if (null == o.GetComponent<die>())
        {
            o.transform.AddComponent<die>();
        }
    }

    void Update()
    {
        if (isEnd)
        {
            if (r > 1){
                r = 1;
            }
            if (r < 0){
                r = 0;
            }
            float x = UnityEngine.Random.Range(-r, r);
            float y = UnityEngine.Random.Range(-r, r);
            if (x * x + y * y <= 1)
            {
                o.GetComponent<addTransform>().addPos = new Vector3(x, Mathf.Sqrt(1 - x * x - y * y), y) * speed;
                o.GetComponent<die>().dieTime = dieTime;
                StartCoroutine(shot());
            }
        }
    }
}
