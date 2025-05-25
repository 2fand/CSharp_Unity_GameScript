using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class die : MonoBehaviour
{
    public float dieTime = 0.16f;
    private IEnumerator toDie()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(dieTime / 10);
            if (null != GetComponent<LineRenderer>())
            {
                GetComponent<LineRenderer>().material.color -= new Color(0, 0, 0, 0.1f);
            }
            else
            {
                GetComponent<TextMeshPro>().color -= new Color(0, 0, 0, 0.1f);
            }
        }
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(toDie());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
