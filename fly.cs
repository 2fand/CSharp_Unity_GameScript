using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class fly : MonoBehaviour
{
    public GameObject o;
    public float space = 0.809f;
    public float waitTime = 0.1f;
    public uint a = 2;
    public uint b = 3;
    public float r = 0.5f;
    public Material m;
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
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if (isEnd)
        {
            float x = UnityEngine.Random.Range(-r, r);
            float y = UnityEngine.Random.Range(-r, r);
            if (null != o.GetComponent<randomSymbolSummon>())
            {
                o.GetComponent<randomSymbolSummon>().space = space;
                o.GetComponent<randomSymbolSummon>().waitTime = waitTime;
                o.GetComponent<randomSymbolSummon>().a = a;
                o.GetComponent<randomSymbolSummon>().b = b;
                o.GetComponent<randomSymbolSummon>().lineMaterial = m;
            }
            o.GetComponent<addTransform>().addPos = new Vector3(x, Mathf.Sqrt(1 - x * x - y * y), y) * speed;
            o.GetComponent<die>().dieTime = dieTime;
            StartCoroutine(shot());
        }
    }
}
