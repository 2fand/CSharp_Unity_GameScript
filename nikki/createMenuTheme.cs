using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    private MenuTheme menuTheme;
    private static List<int> IDs = new List<int>();
    private bool isCreated = false;
    private void Start()
    {
        List<Vector2?> sizes = new List<Vector2?>{ menu?.rect.size, menuLeft?.rect.size, menuRight?.rect.size, menuUp?.rect.size, menuDown?.rect.size, menuLeftUp?.rect.size, menuRightUp?.rect.size, menuLeftDown?.rect.size, menuRightDown?.rect.size, menuCenter?.rect.size, menu_1x1?.rect.size, menu_1x_c?.rect.size, menu_1x_u?.rect.size, menu_1x_d?.rect.size, menu_x1_c?.rect.size, menu_x1_l?.rect.size, menu_x1_r?.rect.size };
        List<string> spriteNames = new List<string> { "menu", "menuLeft", "menuRight", "menuUp", "menuDown", "menuLeftUp", "menuRightUp", "menuLeftDown", "menuRightDown", "menuCenter", "menu_1x1", "menu_1x_c", "menu_1x_u", "menu_1x_d", "menu_x1_c", "menu_x1_l", "menu_x1_r" };
        Vector2 maxCountSize = Vector2.zero;
        int maxCount = 0;
        bool isOk = true;
        Hashtable sizeCounts = new Hashtable();
        for (int i = 0; i < sizes.Count; i++) {
            if (null == sizes[i])
            {
                Debug.LogWarning("菜单样式警告：" + spriteNames[i] + "为null");
                sizes.RemoveAt(i);
                spriteNames.RemoveAt(i--);
            }
            else if (0 == sizes[i].Value.x || 0 == sizes[i].Value.y)
            {
                Debug.LogWarning("菜单样式警告：" + spriteNames[i] + "长或宽为0");
            }
        }
        for (int i = 0; i < sizes.Count; i++) {
            if (i < sizes.Count - 1 && sizes[i] != sizes[i + 1]) 
            { 
                isOk = false;
            }
            if (!sizeCounts.ContainsKey(sizes[i]))
            {
                sizeCounts.Add(sizes[i], 1);
            }
            else
            {
                sizeCounts[sizes[i]] = (int)sizeCounts[sizes[i]] + 1;
            }
            if (maxCount < (int)sizeCounts[sizes[i]])
            {
                maxCount = (int)sizeCounts[sizes[i]];
                maxCountSize = sizes[i].Value;
            }
        }
        if (!isOk)
        {
            string warning = "菜单样式警告：精灵大小不同，建议修改菜单";
            for (int i = 0; i < sizes.Count; i++)
            {
                if (maxCount != (int)sizeCounts[sizes[i]])
                {
                    warning += spriteNames[i];
                    warning += ", ";
                }
            }
            warning = warning.Substring(0, warning.Length - 2);
            warning = warning.Substring(0, Regex.Match(warning, ", ", RegexOptions.RightToLeft).Index) + Regex.Replace(warning.Substring(Regex.Match(warning, ", ", RegexOptions.RightToLeft).Index), ", ", "和");
            Debug.LogWarning(warning);
        }
        menuTheme = new MenuTheme(mode, menu, menuLeft, menuRight, menuUp, menuDown, menuLeftUp, menuRightUp, menuLeftDown, menuRightDown, menuCenter, menu_1x1, menu_1x_c, menu_1x_u, menu_1x_d, menu_x1_c, menu_x1_l, menu_x1_r);
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