using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show : transition
{
    public static IEnumerator changeScene(float time = 0.1f)
    {
        transitionIsEnd = false;
        for (int i = 0; i < 10; i++)
        {
            you.teleScreen.GetComponent<Image>().color -= new Color(0, 0, 0, 0.1f);
            yield return new WaitForSeconds(time / 10);
        }
        transitionIsEnd = true;
    }
}
public class hide : transition
{
    public static IEnumerator changeScene(float time = 0.1f)
    {
        transitionIsEnd = false;
        for (int i = 0; i < 10; i++)
        {
            you.teleScreen.GetComponent<Image>().color += new Color(0, 0, 0, 0.1f);
            yield return new WaitForSeconds(time / 10);
        }
        transitionIsEnd = true;
    }
}
public class enterNone : transition
{
    public static IEnumerator changeScene(float time = 0)
    {
        transitionIsEnd = false;
        you.teleScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(time);
        transitionIsEnd = true;
        yield return null;
    }
}
public class exitNone : transition
{
    public static IEnumerator changeScene(float time = 0)
    {
        transitionIsEnd = false;
        you.teleScreen.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(time);
        transitionIsEnd = true;
        yield return null;
    }
}
public class fadein : transition
{
    public static IEnumerator changeScene(float time = 0.1f)
    {
        transitionIsEnd = false;
        
        transitionIsEnd = true;
        yield return null;
    }
}

public class fadeout : transition
{
    public static IEnumerator changeScene(float time = 0.1f)
    {
        transitionIsEnd = false;
        
        transitionIsEnd = true;
        yield return null;
    }
}