using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class item
{
    public string name = "default";
    public string recommendText = "";
    public bool canUse = true;
    public bool isHide = false;
    public abstract void use();
    public item(string name = "default", string recommendText = "", bool canUse = true, bool isHide = false)
    {
        this.name = name;
        this.recommendText = recommendText;
        this.canUse = canUse;
        this.isHide = isHide;
    }
    public static void addItem(item item)
    {
        you.items.Add(item);
        if (!item.isHide)
        {
            you.NotNullItemsNum++;
        }
        you.itemsIsChanged = true;
    }

    public static void delItem(item item)
    {
        bool isHide = item.isHide;
        if (you.items.Remove(item) && !isHide)
        {
            you.NotNullItemsNum--;
        }
        you.itemsIsChanged = true;
    }

    public static void addAction(actionItem item)
    {
        you.actions.Add(item);
        if (!item.isHide)
        {
            you.NotNullActionsNum++;
        }
        you.itemsIsChanged = true;
    }

    public static void delAction(actionItem item)
    {
        bool isHide = item.isHide;
        if (you.items.Remove(item) && !isHide)
        {
            you.NotNullActionsNum--;
        }
        you.actionIsChanged = true;
    }
}
