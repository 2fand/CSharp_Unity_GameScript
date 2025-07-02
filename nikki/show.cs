using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show : MonoBehaviour
{
    public Image image;
    IEnumerator d()
    {
        for (int i = 0; i < 50; i++)
        {
            image.color -= new Color(0, 0, 0, 0.02f);
            yield return 5;
        }
    }
    void Start()
    {
        StartCoroutine(d());
    }

    void Update()
    {

    }
}
