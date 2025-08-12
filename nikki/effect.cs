using System;
using System.Collections;
using System.Diagnostics;
using Unity.VisualScripting;

public enum effect
{
    none,//初始效果(第0个)
    angel,
    lockDoor
}//效果

public abstract class effectUse
{
    public static you You
    {
        get
        {
            return you.You;
        }
    }
    private static Hashtable effectUses;
    static effectUse() {
        effectUses = new Hashtable { { (effect)0, none.useAct }, { effect.angel, angel.useAct } };
    }
    public static Action getEffectUseAct(effect effect)
    {
        return (Action)effectUses[effect];
    }
    public abstract void use();
}
