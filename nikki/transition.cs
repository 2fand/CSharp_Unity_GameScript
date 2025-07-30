using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static change;

public abstract class transition
{
#nullable enable
    public static Hashtable transitions = new Hashtable { { enterMode.fadein, fadein.changeScene() }, { enterMode.show, show.changeScene() }, { exitMode.fadeout, fadeout.changeScene() }, { exitMode.hide, hide.changeScene() }, { enterMode.none, enterNone.changeScene() }, { exitMode.none, exitNone.changeScene() } };
#nullable disable
    protected static bool transitionIsEnd = true;
    public static bool TransitionIsEnd
    {
        get
        {
            return transitionIsEnd;
        }
    }
    public static bool showhasTest
    {
        get {
            return transitions.ContainsKey(enterMode.show);
        }
    }
    public static void resetTransitions()
    {
        transitions = new Hashtable { { enterMode.fadein, fadein.changeScene() }, { enterMode.show, show.changeScene() }, { exitMode.fadeout, fadeout.changeScene() }, { exitMode.hide, hide.changeScene() }, { enterMode.none, enterNone.changeScene() }, { exitMode.none, exitNone.changeScene() } };
    }
}
