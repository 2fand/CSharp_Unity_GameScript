using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class select
{
    public enum menuClass
    {
        none,
        use,
        action,
        //status,
        quit,
        other
    }
    public string name = "";
    public menuClass MenuClass = menuClass.none;
    public GameObject text;
    private Canvas canvas;
    public static int textSize = 12;
    public select(Canvas canvas, string name = "", menuClass menuClass = menuClass.none)
    {
        this.name = name;
        MenuClass = menuClass;
        text = new GameObject(name);
        text.AddComponent<Text>().text = name;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = textSize;
        text.GetComponent<Text>().color = MenuTheme.menuThemes[you.myMenuID].menuTextColor;
        text.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
        text.transform.SetParent(canvas.transform, false);
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        //text.GetComponent<Text>().enabled = false;
    }
}
