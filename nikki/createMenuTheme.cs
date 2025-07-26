using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static MenuTheme;

public class createMenuTheme : MonoBehaviour
{
    public makeMode mode = makeMode.concatenate;
    public string menuName = "";
    public int ID;
    public Sprite menu;
    public Sprite menuLeft;
    public Sprite menuRight;
    public Sprite menuUp;
    public Sprite menuDown;
    public Sprite menuLeftUp;
    public Sprite menuRightUp;
    public Sprite menuLeftDown;
    public Sprite menuRightDown;
    public Sprite menuCenter;
    public Sprite menu_1x1;
    public Sprite menu_1x_c;
    public Sprite menu_1x_u;
    public Sprite menu_1x_d;
    public Sprite menu_x1_c;
    public Sprite menu_x1_l;
    public Sprite menu_x1_r;
    public Sprite cursor;
    public Cursor.dodgeMode cursorDodgeMode;
    public Sprite[] cursorAnimation;
    public Color menuTextColor = Color.white;
    public Color menuHighlightColor = Color.white;
    public Color menuUnselectColor = Color.white;
    private MenuTheme menuTheme;
    private static List<int> IDs = new List<int>();
    private bool isCreated = false;
    void Start()
    {
        menuTheme = new MenuTheme(mode, menu, menuLeft, menuRight, menuUp, menuDown, menuLeftUp, menuRightUp, menuLeftDown, menuRightDown, menuCenter, menu_1x1, menu_1x_c, menu_1x_u, menu_1x_d, menu_x1_c, menu_x1_l, menu_x1_r, cursor, cursorDodgeMode, cursorAnimation, menuTextColor, menuHighlightColor, menuUnselectColor);
        if (null == menuTheme)
        {
            ID = -1;
            Debug.LogError("菜单错误：菜单无法创建");
            enabled = false;
        }
        IDs.Add(ID);
        IDs.Sort();
    }

    void Update()
    {
        if (!isCreated && 0 < IDs.Count && IDs[0] == ID)
        {
            isCreated = true;
            menuTheme.menuName = menuName;
            menuThemes.Add(menuTheme);
            menuTheme.myMenuID = menuThemes.Count - 1;
            IDs.Remove(ID);
            IDs.Sort();
        }
        if (menuThemes[menuTheme.myMenuID].mode != mode || menuName != menuThemes[menuTheme.myMenuID].menuName || menu != menuThemes[menuTheme.myMenuID].menu || menuLeft != menuThemes[menuTheme.myMenuID].menuLeft || menuRight != menuThemes[menuTheme.myMenuID].menuRight || menuUp != menuThemes[menuTheme.myMenuID].menuUp || menuDown != menuThemes[menuTheme.myMenuID].menuDown || menuLeftUp != menuThemes[menuTheme.myMenuID].menuLeftUp || menuRightUp != menuThemes[menuTheme.myMenuID].menuRightUp || menuLeftDown != menuThemes[menuTheme.myMenuID].menuLeftDown || menuRightDown != menuThemes[menuTheme.myMenuID].menuRightDown || menuCenter != menuThemes[menuTheme.myMenuID].menuCenter || menu_1x1 != menuThemes[menuTheme.myMenuID].menu_1x1 || menu_1x_c != menuThemes[menuTheme.myMenuID].menu_1x_c || menu_1x_u != menuThemes[menuTheme.myMenuID].menu_1x_u || menu_1x_d != menuThemes[menuTheme.myMenuID].menu_1x_d || menu_x1_c != menuThemes[menuTheme.myMenuID].menu_x1_c || menu_x1_l != menuThemes[menuTheme.myMenuID].menu_x1_l || menu_x1_r != menuThemes[menuTheme.myMenuID].menu_x1_r)
        {
            menuThemes[menuTheme.myMenuID].mode = mode;
            menuThemes[menuTheme.myMenuID].menuName = menuName;
            menuThemes[menuTheme.myMenuID].menu = menu;
            menuThemes[menuTheme.myMenuID].menuLeft = menuLeft;
            menuThemes[menuTheme.myMenuID].menuRight = menuRight;
            menuThemes[menuTheme.myMenuID].menuUp = menuUp;
            menuThemes[menuTheme.myMenuID].menuDown = menuDown;
            menuThemes[menuTheme.myMenuID].menuLeftUp = menuLeftUp;
            menuThemes[menuTheme.myMenuID].menuRightUp = menuRightUp;
            menuThemes[menuTheme.myMenuID].menuLeftDown = menuLeftDown;
            menuThemes[menuTheme.myMenuID].menuRightDown = menuRightDown;
            menuThemes[menuTheme.myMenuID].menuCenter = menuCenter;
            menuThemes[menuTheme.myMenuID].menu_1x1 = menu_1x1;
            menuThemes[menuTheme.myMenuID].menu_1x_c = menu_1x_c;
            menuThemes[menuTheme.myMenuID].menu_1x_u = menu_1x_u;
            menuThemes[menuTheme.myMenuID].menu_1x_d = menu_1x_d;
            menuThemes[menuTheme.myMenuID].menu_x1_c = menu_x1_c;
            menuThemes[menuTheme.myMenuID].menu_x1_l = menu_x1_l;
            menuThemes[menuTheme.myMenuID].menu_x1_r = menu_x1_r;
            menuThemes[menuTheme.myMenuID].isEdit = true;
        }
        ID = menuTheme.myMenuID;
    }
}
