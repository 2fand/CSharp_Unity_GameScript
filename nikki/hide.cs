using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hide : transition
{
    public static IEnumerator changeScene()
    {
        transitionIsEnd = false;
        for (int i = 0; i < 50; i++)
        {
            you.teleScreen.GetComponent<Image>().color += new Color(0, 0, 0, 0.02f);
            yield return 5;
        }
        transitionIsEnd = true;
    }
}
