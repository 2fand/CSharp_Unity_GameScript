using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class makeMenu : MonoBehaviour
{
    public enum makeMode
    {
        scale,
        concatenate
    }
    public makeMode mode;
    public string menuName = "default";
    public Sprite menu;
    private GameObject image;
    private uint catW = 0;
    private uint catH = 0;
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
    private GameObject[] images;

    void makeGrid()
    {
        if (null == GetComponent<GridLayoutGroup>())
        {
            gameObject.AddComponent<GridLayoutGroup>();
        }
        //GridLayoutGroup的网格大小以第一个构成菜单的图片大小为准
        if (1 == catH && 1 == catW)
        {
            GetComponent<GridLayoutGroup>().cellSize = menu_1x1.rect.size;
        }
        else if (1 == catH)
        {
            GetComponent<GridLayoutGroup>().cellSize = menu_x1_l.rect.size;
        }
        else if (1 == catW)
        {
            GetComponent<GridLayoutGroup>().cellSize = menu_1x_u.rect.size;
        }
        else
        {
            GetComponent<GridLayoutGroup>().cellSize = menuLeftUp.rect.size;
        }
        catW = (uint)((int)GetComponent<RectTransform>().sizeDelta.x / (int)GetComponent<GridLayoutGroup>().cellSize.x);
        catH = (uint)((int)GetComponent<RectTransform>().sizeDelta.y / (int)GetComponent<GridLayoutGroup>().cellSize.y);
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        if ((0 == catH || 0 == catW) || (1 == catH && 1 != catW && (null == menu_x1_l || null == menu_x1_r || 2 < catW && null == menu_x1_c)) || (1 != catH && 1 == catW && (null == menu_1x_u || null == menu_1x_d || 2 < catH && null == menu_1x_c)) || (1 == catH && 1 == catW && null == menu_1x1) || (1 != catH && 1 != catW && (null == menuLeftDown || null == menuRightDown || null == menuLeftUp || null == menuRightUp || 2 < catH && (null == menuLeft || null == menuRight) || 2 < catW && (null == menuUp || null == menuDown) || 2 < catH && 2 < catW && null == menuCenter)))
        {
            return;
        }
        for (int y = 0; y < catH; y++)
        {
            for (int x = 0; x < catW; x++)
            {
                image = new GameObject(menuName + " - " + (y * catW + x));
                image.transform.parent = transform;
                image.AddComponent<Image>();
                if (1 == catH && 1 == catW)
                {
                    image.GetComponent<Image>().sprite = menu_1x1;
                }
                else if (1 == catW && 0 == y)
                {
                    image.GetComponent<Image>().sprite = menu_1x_u;
                }
                else if (1 == catW && catH - 1 == y)
                {
                    image.GetComponent<Image>().sprite = menu_1x_d;
                }
                else if (1 == catW)
                {
                    image.GetComponent<Image>().sprite = menu_1x_c;
                }
                else if (1 == catH && 0 == x)
                {
                    image.GetComponent<Image>().sprite = menu_x1_l;
                }
                else if (1 == catH && catW - 1 == x)
                {
                    image.GetComponent<Image>().sprite = menu_x1_r;
                }
                else if (1 == catH)
                {
                    image.GetComponent<Image>().sprite = menu_x1_c;
                }
                else if (0 == x && 0 == y)
                {
                    image.GetComponent<Image>().sprite = menuLeftUp;
                }
                else if (catW - 1 == x && 0 == y)
                {
                    image.GetComponent<Image>().sprite = menuRightUp;
                }
                else if (0 == y)
                {
                    image.GetComponent<Image>().sprite = menuUp;
                }
                else if (0 == x && catH - 1 == y)
                {
                    image.GetComponent<Image>().sprite = menuLeftDown;
                }
                else if (catW - 1 == x && catH - 1 == y)
                {
                    image.GetComponent<Image>().sprite = menuRightDown;
                }
                else if (catH - 1 == y)
                {
                    image.GetComponent<Image>().sprite = menuDown;
                }
                else if (0 == x)
                {
                    image.GetComponent<Image>().sprite = menuLeft;
                }
                else if (catW - 1 == x)
                {
                    image.GetComponent<Image>().sprite = menuRight;
                }
                else
                {
                    image.GetComponent<Image>().sprite = menuCenter;
                }
                image.GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
    }

    void Start()
    {

        switch (mode)
        {
            case makeMode.scale:
                image = new GameObject(menuName);
                image.transform.parent = GameObject.Find("Canvas").transform;
                image.AddComponent<Image>();
                break;
            default:
                makeGrid();
                break;
        }
    }

    void Update()
    {
        switch (mode)
        {
            case makeMode.scale:
                image.GetComponent<Image>().rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
                image.GetComponent<Image>().sprite = menu ?? image.GetComponent<Image>().sprite;
                image.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
                image.GetComponent<RectTransform>().localScale = Vector3.one;
                break;
            default:
                if ((uint)((int)GetComponent<RectTransform>().sizeDelta.y / (int)GetComponent<GridLayoutGroup>().cellSize.y) != catH || (uint)((int)GetComponent<RectTransform>().sizeDelta.x / (int)GetComponent<GridLayoutGroup>().cellSize.x) != catW)
                {
                    makeGrid();
                }
                break;
        }
    }
}
