using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
