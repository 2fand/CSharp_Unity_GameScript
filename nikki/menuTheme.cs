using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTheme
{
    public enum makeMode
    {
        scale,
        concatenate,
        easyConcatenate
    }
    public makeMode mode = makeMode.concatenate;
    public string menuName;
    public int myMenuID;
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
    public bool isEdit = false;
    public static List<MenuTheme> menuThemes = new List<MenuTheme>();
    public MenuTheme(makeMode mode, Sprite menu, Sprite menuLeft, Sprite menuRight, Sprite menuUp, Sprite menuDown, Sprite menuLeftUp, Sprite menuRightUp, Sprite menuLeftDown, Sprite menuRightDown, Sprite menuCenter, Sprite menu_1x1, Sprite menu_1x_c, Sprite menu_1x_u, Sprite menu_1x_d, Sprite menu_x1_c, Sprite menu_x1_l, Sprite menu_x1_r, string menuName = "")
    {
        this.mode = mode;
        this.menu = menu;
        this.menuLeft = menuLeft;
        this.menuRight = menuRight;
        this.menuUp = menuUp;
        this.menuDown = menuDown;
        this.menuLeftUp = menuLeftUp;
        this.menuRightUp = menuRightUp;
        this.menuLeftDown = menuLeftDown;
        this.menuRightDown = menuRightDown;
        this.menuCenter = menuCenter;
        this.menu_1x1 = menu_1x1;
        this.menu_1x_c = menu_1x_c;
        this.menu_1x_u = menu_1x_u;
        this.menu_1x_d = menu_1x_d;
        this.menu_x1_c = menu_x1_c;
        this.menu_x1_l = menu_x1_l;
        this.menu_x1_r = menu_x1_r;
        this.menuName = menuName;
    }
}
