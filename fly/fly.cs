using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class fly : MonoBehaviour
{
    public GameObject o;
    public float a = 0.5f;
    public float summonTime = 0.1f;
    public float speed = 0.0618f;
    public float dieTime = 2f;
    private bool isEnd = true;
    IEnumerator shot()
    {
        isEnd = false;
        yield return new WaitForSeconds(summonTime);
        Instantiate(o, this.transform.position, o.transform.rotation);
        isEnd = true;
    }

    void Start()
    {
        if (null == o.GetComponent<particleRun>())
        {
            o.transform.AddComponent<particleRun>();
        }
    }


    void Update()
    {
        if (isEnd)
        {
            if (a < 0)
            {
                a = 0;
            }
            if (a > 1)
            {
                a = 1;
            }
            float x = UnityEngine.Random.Range(-a, a);
            float y = UnityEngine.Random.Range(-(Mathf.Sqrt(1 - x * x) < a ? Mathf.Sqrt(1 - x * x) : a), Mathf.Sqrt(1 - x * x) < a ? Mathf.Sqrt(1 - x * x) : a);
            Vector3 spinPos = transform.rotation * new Vector3(x, y, Mathf.Sqrt(1 - x * x - y * y));
            o.GetComponent<particleRun>().direction = spinPos * speed;
            o.GetComponent<particleRun>().dieTime = dieTime;
            StartCoroutine(shot());
        }
    }
}
