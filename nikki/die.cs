using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class die : MonoBehaviour
{
    public float waitTime = 1;
    IEnumerator di()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < GetComponent<MeshRenderer>().materials.Length; j++)
            {
                GetComponent<MeshRenderer>().materials[j].color -= new Color(0, 0, 0, 0.1f);
            }
            yield return new WaitForSeconds(waitTime / 10);
        }
        Destroy(gameObject);
    }
    void Start()
    {
        StartCoroutine(di());
    }

    void Update()
    {
        
    }
}
