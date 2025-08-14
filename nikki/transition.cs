using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static change;

public abstract class transition
{
    public abstract transitionMode _transitionMode { get; }
#nullable enable
    public static Hashtable transitions;
#nullable disable
    protected static bool transitionIsEnd = true;
    public static bool TransitionIsEnd
    {
        get
        {
            return transitionIsEnd;
        }
    }
    public abstract IEnumerator changeScene(float time = 0.1f);
    static transition(){
        init();
    }
    private static void init()
    {
        transitions = new Hashtable { { transitionMode.fadein, new fadein().changeScene() }, { transitionMode.show, new show().changeScene() }, { transitionMode.fadeout, new fadeout().changeScene() }, { transitionMode.hide, new hide().changeScene() }, { transitionMode.enterNone, new enterNone().changeScene() }, { transitionMode.exitNone, new exitNone().changeScene() } };
    }
    public static void resetTransitions()
    {
        init();
    }
}
