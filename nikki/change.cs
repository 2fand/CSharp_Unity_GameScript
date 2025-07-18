using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class change : MonoBehaviour
{
    public enum enterMode
    {
        none,
        show,
        fadein
    }

    public enum exitMode
    {
        none,
        hide,
        fadeout
    }
    //public IEnumerator hide(Image image)
    //{
    //    for (int i = 0; i < 50; i++)
    //    {
    //        image.color += new Color(0, 0, 0, 0.02f);
    //        yield return 5;
    //    }
    //}

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
        you.isChangeEffect = true;
        switch (you.enterMode)
        {
            case enterMode.show:
                StartCoroutine(show(you.teleScreen.GetComponent<Image>()));
                break;
            default:
                you.teleScreen.GetComponent<Image>().color -= new Color(0, 0, 0, 1);
                break;
        }
    }

    void Update()
    {
        
    }
}
