using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class select
{
    public enum menuClass
    {
        none,
        cantIn,
        back,
        main,
        item,
        useItem,
        action,
        doAction,
        //status,
        quit,
        other,
    }
    public string name = "";
    public menuClass MenuClass = menuClass.none;
    public GameObject text;
    private Canvas canvas;
    public static int textSize = 19;
    public static Hashtable menuSelects = new Hashtable { { menuClass.none, new select[0, 0] }, { menuClass.main, new select[0, 0] }, { menuClass.item, new select[0, 0] }, { menuClass.action, new select[0, 0] }, { menuClass.quit, new select[0, 0] }, { menuClass.other, new select[0, 0] }};
    public string[] commands;
    public bool canSelected = true;
#nullable enable
    private item? usedItem = null;
    public item? UsedItem
    {
        get
        {
            return usedItem;
        }
        set
        {
            if (null != value && value.isHide) {
                return;
            }
            usedItem = value;
            if (null == usedItem || usedItem.canUse)
            {
                text.GetComponent<Text>().color = you.myMenu.menuTextColor * new Color(1, 1, 1, 0);
                canSelected = true;
                if (null != usedItem)
                {
                    commands = new string[1] { "use " + usedItem.name };
                }
            }
            else if(!usedItem.canUse)
            {
                text.GetComponent<Text>().color = you.myMenu.menuTextUnselectColor * new Color(1, 1, 1, 0);
                canSelected = false;
            }
        }
    }
#nullable disable
    public bool CanSelected
    {
        get
        {
            return canSelected;
        }
        set
        {
            canSelected = value;
            if (canSelected) {
                text.GetComponent<Text>().color = you.myMenu.menuTextColor * new Color(1, 1, 1, 0);
            }
            else
            {
                text.GetComponent<Text>().color = you.myMenu.menuTextUnselectColor * new Color(1, 1, 1, 0);
            }
        }
    }
    public select(Canvas canvas, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int? size = null)
    {
        this.name = name;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size ?? textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(canvas.transform, false);
        textAdjust.adjustText(ref text, textAnchor);
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(Canvas canvas, string[] commands, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int? size = null)
    {
        this.name = name;
        this.commands = commands;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size ?? textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(canvas.transform, false);
        textAdjust.adjustText(ref text, textAnchor);
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(Canvas canvas, Vector2 pos, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int? size = null)
    {
        this.name = name;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size ?? textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(canvas.transform, false);
        text.GetComponent<RectTransform>().localPosition = pos;
        textAdjust.adjustText(ref text, textAnchor);
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(Canvas canvas, Vector2 pos, string[] commands, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int? size = null)
    {
        this.name = name;
        this.commands = commands;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size ?? textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(canvas.transform, false);
        text.GetComponent<RectTransform>().localPosition = pos;
        textAdjust.adjustText(ref text, textAnchor);
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(Canvas canvas, Vector2 pos, Vector2 sizeDelta, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int? size = null)
    {
        this.name = name;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size ?? textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(canvas.transform, false);
        text.GetComponent<RectTransform>().localPosition = pos;
        textAdjust.adjustText(ref text, textAnchor);
        text.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(Canvas canvas, Vector2 pos, Vector2 sizeDelta, string[] commands, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int? size = null)
    {
        this.name = name;
        this.commands = commands;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size ?? textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(canvas.transform, false);
        text.GetComponent<RectTransform>().localPosition = pos;
        textAdjust.adjustText(ref text, textAnchor);
        text.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(GameObject parent, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int? size = null)
    {
        this.name = name;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size ?? textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(parent.transform, false);
        textAdjust.adjustText(ref text, textAnchor);
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(GameObject parent, string[] commands, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int? size = null)
    {
        this.name = name;
        this.commands = commands;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size ?? textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(parent.transform, false);
        textAdjust.adjustText(ref text, textAnchor);
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(GameObject parent, Vector2 pos, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int? size = null)
    {
        this.name = name;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size ?? textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(parent.transform, false);
        text.GetComponent<RectTransform>().localPosition = pos;
        textAdjust.adjustText(ref text, textAnchor);
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(GameObject parent, Vector2 pos, string[] commands, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int? size = null)
    {
        this.name = name;
        this.commands = commands;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size ?? textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(parent.transform, false);
        textAdjust.adjustText(ref text, textAnchor);
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(GameObject parent, Vector2 pos, Vector2 sizeDelta, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int? size = null)
    {
        this.name = name;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size ?? textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(parent.transform, false);
        text.GetComponent<RectTransform>().localPosition = pos;
        textAdjust.adjustText(ref text, textAnchor);
        text.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(GameObject parent, Vector2 pos, Vector2 sizeDelta, string[] commands, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int? size = null)
    {
        this.name = name;
        this.commands = commands;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size ?? textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(parent.transform, false);
        text.GetComponent<RectTransform>().localPosition = pos;
        textAdjust.adjustText(ref text, textAnchor);
        text.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public static void setYourSelectsColor(Color c)
    {
        if (null == you.yourSelects)
        {
            return;
        }
        for (int i = 0; i < you.yourSelects.GetLength(0); i++) 
        {
            for (int j = 0; j < you.yourSelects.GetLength(1); j++)
            {
                you.yourSelects[i, j].text.GetComponent<Text>().color = c;
            }
        }
    }

    public static void addYourSelectsColor(Color c)
    {
        if (null == you.yourSelects)
        {
            return;
        }
        for (int i = 0; i < you.yourSelects.GetLength(0); i++)
        {
            for (int j = 0; j < you.yourSelects.GetLength(1); j++)
            {
                you.yourSelects[i, j].text.GetComponent<Text>().color += c;
            }
        }
    }

    public static void setSelectsColor(Color c, menuClass menuClass)
    {
        if (null == menuSelects[menuClass])
        {
            return;
        }
        for (int i = 0; i < ((select[,])menuSelects[menuClass]).GetLength(0); i++)
        {
            for (int j = 0; j < ((select[,])menuSelects[menuClass]).GetLength(1); j++)
            {
                if (null == ((select[,])menuSelects[menuClass])[i, j] || null == ((select[,])menuSelects[menuClass])[i, j].text)
                {
                    continue;
                }
                ((select[,])menuSelects[menuClass])[i, j].text.GetComponent<Text>().color = c;
            }
        }
    }

    public static void addSelectsColor(Color c, menuClass menuClass)
    {
        if (null == menuSelects[menuClass])
        {
            return;
        }
        for (int i = 0; i < ((select[,])menuSelects[menuClass]).GetLength(0); i++)
        {
            for (int j = 0; j < ((select[,])menuSelects[menuClass]).GetLength(1); j++)
            {
                if (null == ((select[,])menuSelects[menuClass])[i, j] || null == ((select[,])menuSelects[menuClass])[i, j].text)
                {
                    continue;
                }
                ((select[,])menuSelects[menuClass])[i, j].text.GetComponent<Text>().color += c;
            }
        }
    }
}
