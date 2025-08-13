using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static change;

public abstract class transition
{
    public abstract bool isExitTransition { get; }
    public abstract enterMode transitionEnterMode { get; }
    public abstract exitMode transitionExitMode { get; }
#nullable enable
    public static Hashtable transitions = new Hashtable { { enterMode.fadein, new fadein().changeScene() }, { enterMode.show, new show().changeScene() }, { exitMode.fadeout, new fadeout().changeScene() }, { exitMode.hide, new hide().changeScene() }, { enterMode.none, new enterNone().changeScene() }, { exitMode.none, new exitNone().changeScene() } };
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
        transitions = new Hashtable { { enterMode.fadein, new fadein().changeScene() }, { enterMode.show, new show().changeScene() }, { exitMode.fadeout, new fadeout().changeScene() }, { exitMode.hide, new hide().changeScene() }, { enterMode.none, new enterNone().changeScene() }, { exitMode.none, new exitNone().changeScene() } };
    }
    public static void resetTransitions()
    {
        init();
    }
}
