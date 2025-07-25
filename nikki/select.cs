using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class select
{
    public enum menuClass
    {
        none,
        cantIn,
        back,
        main,
        item,
        action,
        //status,
        quit,
        other,
    }
    public string name = "";
    public menuClass MenuClass = menuClass.none;
    public GameObject text;
    private Canvas canvas;
    public static int textSize = 12;
    public static Hashtable menuSelects = new Hashtable();
    public string[] commands;
    public select(Canvas canvas, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int textSize = 12)
    {
        this.name = name;
        MenuClass = menuClass;
        text = new GameObject(name);
        if (0 == (int)textAnchor % 3)
        {
            text.AddComponent<Text>().text = "  " + name;
        }
        else if (1 == (int)textAnchor % 3)
        {
            text.AddComponent<Text>().text = name;
        }
        else
        {
            text.AddComponent<Text>().text = name + "  ";
        }
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize =  textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(canvas.transform, false);
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(Canvas canvas, string[] commands, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int textSize = 12)
    {
        this.name = name;
        this.commands = commands;
        MenuClass = menuClass;
        text = new GameObject(name);
        if (0 == (int)textAnchor % 3)
        {
            text.AddComponent<Text>().text = "  " + name;
        }
        else if (1 == (int)textAnchor % 3)
        {
            text.AddComponent<Text>().text = name;
        }
        else
        {
            text.AddComponent<Text>().text = name + "  ";
        }
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(canvas.transform, false);
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(Canvas canvas, Vector2 pos, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int textSize = 12)
    {
        this.name = name;
        MenuClass = menuClass;
        text = new GameObject(name);
        if (0 == (int)textAnchor % 3)
        {
            text.AddComponent<Text>().text = "  " + name;
        }
        else if (1 == (int)textAnchor % 3)
        {
            text.AddComponent<Text>().text = name;
        }
        else
        {
            text.AddComponent<Text>().text = name + "  ";
        }
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(canvas.transform, false);
        text.GetComponent<RectTransform>().localPosition = pos;
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(Canvas canvas, Vector2 pos, string[] commands, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int textSize = 12)
    {
        this.name = name;
        this.commands = commands;
        MenuClass = menuClass;
        text = new GameObject(name);
        if (0 == (int)textAnchor % 3)
        {
            text.AddComponent<Text>().text = "  " + name;
        }
        else if (1 == (int)textAnchor % 3)
        {
            text.AddComponent<Text>().text = name;
        }
        else
        {
            text.AddComponent<Text>().text = name + "  ";
        }
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(canvas.transform, false);
        text.GetComponent<RectTransform>().localPosition = pos;
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(Canvas canvas, Vector2 pos, Vector2 sizeDelta, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int textSize = 12)
    {
        this.name = name;
        MenuClass = menuClass;
        text = new GameObject(name);
        if (0 == (int)textAnchor % 3)
        {
            text.AddComponent<Text>().text = "  " + name;
        }
        else if (1 == (int)textAnchor % 3)
        {
            text.AddComponent<Text>().text = name;
        }
        else
        {
            text.AddComponent<Text>().text = name + "  ";
        }
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(canvas.transform, false);
        text.GetComponent<RectTransform>().localPosition = pos;
        text.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public select(Canvas canvas, Vector2 pos, Vector2 sizeDelta, string[] commands, string name = "", menuClass menuClass = menuClass.none, TextAnchor textAnchor = TextAnchor.MiddleLeft, int textSize = 12)
    {
        this.name = name;
        this.commands = commands;
        MenuClass = menuClass;
        text = new GameObject(name);
        if (0 == (int)textAnchor % 3)
        {
            text.AddComponent<Text>().text = "  " + name;
        }
        else if (1 == (int)textAnchor % 3)
        {
            text.AddComponent<Text>().text = name;
        }
        else
        {
            text.AddComponent<Text>().text = name + "  ";
        }
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor * new Color(1, 1, 1, 0);
        text.GetComponent<Text>().alignment = textAnchor;
        text.transform.SetParent(canvas.transform, false);
        text.GetComponent<RectTransform>().localPosition = pos;
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
                ((select[,])menuSelects[menuClass])[i, j].text.GetComponent<Text>().color += c;
            }
        }
    }
}
