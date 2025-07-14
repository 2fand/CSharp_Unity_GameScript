using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class item
{
    public string name = "default";
    public bool canUse = true;
    public bool isHide = false;
    public abstract void use();
    public item(string name = "default", bool canUse = true, bool isHide = false)
    {
        this.name = name;
        this.canUse = canUse;
        this.isHide = isHide;
    }
}
