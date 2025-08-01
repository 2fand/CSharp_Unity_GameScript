using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show : transition
{
    public static IEnumerator changeScene()
    {
        transitionIsEnd = false;
        for (int i = 0; i < 50; i++)
        {
            you.teleScreen.GetComponent<Image>().color -= new Color(0, 0, 0, 0.02f);
            yield return 5;
        }
        transitionIsEnd = true;
    }
}
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
public class enterNone : transition
{
    public static IEnumerator changeScene()
    {
        transitionIsEnd = false;
        you.teleScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        transitionIsEnd = true;
        yield return null;
    }
}
public class exitNone : transition
{
    public static IEnumerator changeScene()
    {
        transitionIsEnd = false;
        you.teleScreen.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        transitionIsEnd = true;
        yield return null;
    }
}
public class fadein : transition
{
    public static IEnumerator changeScene()
    {
        transitionIsEnd = false;
        
        transitionIsEnd = true;
        yield return null;
    }
}

public class fadeout : transition
{
    public static IEnumerator changeScene()
    {
        transitionIsEnd = false;
        
        transitionIsEnd = true;
        yield return null;
    }
}