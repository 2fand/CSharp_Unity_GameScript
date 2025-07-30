using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
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
    public Sprite cursor;
    public Sprite[] cursorAnimation;
    public Cursor.dodgeMode cursorDodgeMode;
    public Color menuTextColor = Color.white;
    public Color menuTextHighlightColor = Color.white;
    public Color menuTextUnselectColor = Color.white;
    public bool isEdit = false;
    public bool isDebug = true;
    public static List<MenuTheme> menuThemes = new List<MenuTheme>();
    public enum menuTextColorClass
    {
        normal,
        highlight,
        unselect
    }
    public MenuTheme(makeMode mode, Sprite menu, Sprite menuLeft, Sprite menuRight, Sprite menuUp, Sprite menuDown, Sprite menuLeftUp, Sprite menuRightUp, Sprite menuLeftDown, Sprite menuRightDown, Sprite menuCenter, Sprite menu_1x1, Sprite menu_1x_c, Sprite menu_1x_u, Sprite menu_1x_d, Sprite menu_x1_c, Sprite menu_x1_l, Sprite menu_x1_r, Sprite cursor, Cursor.dodgeMode dodgeMode, Sprite[] animation, Color menuTextColor, Color menuHighlightColor, Color menuUnselectColor, string menuName = "")
    {
        bool[] isNull = { menu.IsUnityNull(), menuLeft.IsUnityNull(), menuRight.IsUnityNull(), menuUp.IsUnityNull(), menuDown.IsUnityNull(), menuLeftUp.IsUnityNull(), menuRightUp.IsUnityNull(), menuLeftDown.IsUnityNull(), menuRightDown.IsUnityNull(), menuCenter.IsUnityNull(), menu_1x1.IsUnityNull(), menu_1x_c.IsUnityNull(), menu_1x_u.IsUnityNull(), menu_1x_d.IsUnityNull(), menu_x1_c.IsUnityNull(), menu_x1_l.IsUnityNull(), menu_x1_r.IsUnityNull() };
        Sprite emptySprite = Sprite.Create(new Texture2D(0, 0), new Rect(0, 0, 0, 0), new Vector2(0f, 0f));
        this.mode = mode;
        if (menu.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menuͼ�񲻴���");
        }
        this.menu = menu.IsUnityNull() ? emptySprite : menu;
        if (menuLeft.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menuLeftͼ�񲻴���");
        }
        this.menuLeft = menuLeft.IsUnityNull() ? emptySprite : menuLeft;
        if (menuRight.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menuRightͼ�񲻴���");
        }
        this.menuRight = menuRight.IsUnityNull() ? emptySprite : menuRight;
        if (menuUp.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menuUpͼ�񲻴���");
        }
        this.menuUp = menuUp.IsUnityNull() ? emptySprite : menuUp;
        if (menuDown.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menuDownͼ�񲻴���");
        }
        this.menuDown = menuDown.IsUnityNull() ? emptySprite : menuDown;
        if (menuLeftUp.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menuLeftUpͼ�񲻴���");
        }
        this.menuLeftUp = menuLeftUp.IsUnityNull() ? emptySprite : menuLeftUp;
        if (menuRightUp.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menuRightUpͼ�񲻴���");
        }
        this.menuRightUp = menuRightUp.IsUnityNull() ? emptySprite : menuRightUp;
        if (menuLeftDown.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menuLeftDownͼ�񲻴���");
        }
        this.menuLeftDown = menuLeftDown.IsUnityNull() ? emptySprite : menuLeftDown;
        if (menuRightDown.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menuRightDownͼ�񲻴���");
        }
        this.menuRightDown = menuRightDown.IsUnityNull() ? emptySprite : menuRightDown;
        if (menuCenter.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menuCenterͼ�񲻴���");
        }
        this.menuCenter = menuCenter.IsUnityNull() ? emptySprite : menuCenter;
        if (menu_1x1.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menu_1x1ͼ�񲻴���");
        }
        this.menu_1x1 = menu_1x1.IsUnityNull() ? emptySprite : menu_1x1;
        if (menu_1x_c.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menu_1x_cͼ�񲻴���");
        }
        this.menu_1x_c = menu_1x_c.IsUnityNull() ? emptySprite : menu_1x_c;
        if (menu_1x_u.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menu_1x_uͼ�񲻴���");
        }
        this.menu_1x_u = menu_1x_u.IsUnityNull() ? emptySprite : menu_1x_u;
        if (menu_1x_d.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menu_1x_dͼ�񲻴���");
        }
        this.menu_1x_d = menu_1x_d.IsUnityNull() ? emptySprite : menu_1x_d;
        if (menu_x1_c.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menu_x1_cͼ�񲻴���");
        }
        this.menu_x1_c = menu_x1_c.IsUnityNull() ? emptySprite : menu_x1_c;
        if (menu_x1_l.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menu_x1_lͼ�񲻴���");
        }
        this.menu_x1_l = menu_x1_l.IsUnityNull() ? emptySprite : menu_x1_l;
        if (menu_x1_r.IsUnityNull() && isDebug)
        {
            Debug.LogWarning("�˵���ʽ���棺menu_x1_rͼ�񲻴���");
        }
        this.menu_x1_r = menu_x1_r.IsUnityNull() ? emptySprite : menu_x1_r;
        this.menuName = menuName;
        this.menuTextColor = menuTextColor;
        this.menuTextHighlightColor = menuHighlightColor;
        this.menuTextUnselectColor = menuUnselectColor;
        this.cursor = cursor;
        cursorAnimation = animation;
        cursorDodgeMode = dodgeMode;
        if (isDebug)
        {
            Vector2[] sizes = { this.menu.rect.size, this.menuLeft.rect.size, this.menuRight.rect.size, this.menuUp.rect.size, this.menuDown.rect.size, this.menuLeftUp.rect.size, this.menuRightUp.rect.size, this.menuLeftDown.rect.size, this.menuRightDown.rect.size, this.menuCenter.rect.size, this.menu_1x1.rect.size, this.menu_1x_c.rect.size, this.menu_1x_u.rect.size, this.menu_1x_d.rect.size, this.menu_x1_c.rect.size, this.menu_x1_l.rect.size, this.menu_x1_r.rect.size };
            string[] names = { "menu", "menuLeft", "menuRight", "menuUp", "menuDown", "menuLeftUp", "menuRightUp", "menuLeftDown", "menuRightDown", "menuCenter", "menu_1x1", "menu_1x_c", "menu_1x_u", "menu_1x_d", "menu_x1_c", "menu_x1_l", "menu_x1_r" };
            bool isDifferent = false;
            int isNotEmptyIndex = 0;
            for (int i = 0; i < sizes.Length; i++)
            {
                if (0 != sizes[i].x && 0 != sizes[i].y)
                {
                    isNotEmptyIndex = i;
                    break;
                }
            }
            for (int i = 0; i < names.Length; i++)
            {
                if (isNull[i] || 0 == sizes[i].x || 0 == sizes[i].y)
                {
                    if (!isNull[i])
                    {
                        Debug.LogWarning("�˵���ʽ���棺" + names[i] + "Ϊ��ͼ��");
                    }
                    sizes[i] = Vector2.zero;
                }
                else if (isNotEmptyIndex < sizes.Length && sizes[isNotEmptyIndex] != sizes[i])
                {
                    isDifferent = true;
                }
            }
            if (isDifferent)
            {
                Hashtable sizeCounts = new Hashtable();
                int maxCount = 0;
                for (int i = 0; i < sizes.Length; i++)
                {
                    if (sizeCounts.ContainsKey(sizes[i]))
                    {
                        sizeCounts[sizes[i]] = (int)sizeCounts[sizes[i]] + 1;
                    }
                    else
                    {
                        sizeCounts.Add(sizes[i], 1);
                    }
                    if (maxCount < (int)sizeCounts[sizes[i]])
                    {
                        maxCount = (int)sizeCounts[sizes[i]];
                    }
                }
                string warning = "�˵���ʽ���棺�����С��ͬ�������޸Ĳ˵�";
                for (int i = 0; i < sizes.Length; i++)
                {
                    if (maxCount != (int)sizeCounts[sizes[i]])
                    {
                        warning += names[i];
                        warning += ", ";
                    }
                }
                warning = warning.Substring(0, warning.Length - 2);
                warning = warning.Substring(0, Regex.Match(warning, ", ", RegexOptions.RightToLeft).Index) + Regex.Replace(warning.Substring(Regex.Match(warning, ", ", RegexOptions.RightToLeft).Index), ", ", "��");
                Debug.LogWarning(warning);
            }
        }
    }
}
