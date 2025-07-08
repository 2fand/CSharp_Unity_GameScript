using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class change : MonoBehaviour
{
    public enum changeMode
    {
        none,
        showAndHide,
        fade
    }
    public Image image;
    public IEnumerator hide(Image image)
    {
        for (int i = 0; i < 50; i++)
        {
            image.color += new Color(0, 0, 0, 0.02f);
            yield return 5;
        }
    }

    public IEnumerator show(Image image)
    {
        for (int i = 0; i < 50; i++)
        {
            image.color -= new Color(0, 0, 0, 0.02f);
            yield return 5;
        }
    }

    void Start()
    {
        switch (you.mode)
        {
            case changeMode.showAndHide:
                StartCoroutine(show(image));
                break;
            default:
                image.color -= new Color(0, 0, 0, 1);
                break;
        }
    }

    void Update()
    {
        
    }
}
