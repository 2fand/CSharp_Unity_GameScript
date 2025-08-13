using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static change;

public class show : transition
{
    public override bool isExitTransition { get { return false; } }
    public override enterMode transitionEnterMode { get { return enterMode.show; } }
    public override exitMode transitionExitMode { get { return exitMode.none; } }
    public override IEnumerator changeScene(float time = 0.1f)
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
    public override bool isExitTransition { get { return true; } }
    public override enterMode transitionEnterMode { get { return enterMode.show; } }
    public override exitMode transitionExitMode { get { return exitMode.hide; } }
    public override IEnumerator changeScene(float time = 0.1f)
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
    public override bool isExitTransition { get { return false; } }
    public override enterMode transitionEnterMode { get { return enterMode.none; } }
    public override exitMode transitionExitMode { get { return exitMode.none; } }
    public override IEnumerator changeScene(float time = 0)
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
    public override bool isExitTransition { get { return true; } }
    public override enterMode transitionEnterMode { get { return enterMode.show; } }
    public override exitMode transitionExitMode { get { return exitMode.none; } }
    public override IEnumerator changeScene(float time = 0)
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
    public override bool isExitTransition { get { return false; } }
    public override enterMode transitionEnterMode { get { return enterMode.fadein; } }
    public override exitMode transitionExitMode { get { return exitMode.none; } }
    public override IEnumerator changeScene(float time = 0.1f)
    {
        transitionIsEnd = false;
        
        transitionIsEnd = true;
        yield return null;
    }
}

public class fadeout : transition
{
    public override bool isExitTransition { get { return true; } }
    public override enterMode transitionEnterMode { get { return enterMode.show; } }
    public override exitMode transitionExitMode { get { return exitMode.fadeout; } }
    public override IEnumerator changeScene(float time = 0.1f)
    {
        transitionIsEnd = false;
        
        transitionIsEnd = true;
        yield return null;
    }
}
