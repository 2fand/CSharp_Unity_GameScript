using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class change : MonoBehaviour
{
    public enum modeClass
    {
        enter, 
        exit,
        none
    }

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

    void Awake()
    {
        StartCoroutine(tele());
    }

    void Update()
    {

    }
    /*
    public static IEnumerator show(Image image)
    {
        for (int i = 0; i < 50; i++)
        {
            image.color -= new Color(0, 0, 0, 0.02f);
            yield return 5;
        }
    }
    */

    public IEnumerator tele()
    {
        yield return new WaitUntil(() => !you.isTele);
        you.isChangeEffect = true;
        you.You.CoroutineStart((IEnumerator)transition.transitions[you.enterMode]);
    }
}
