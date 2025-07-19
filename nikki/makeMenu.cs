using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class makeMenu : MonoBehaviour
{
    public enum makeMode
    {
        scale,
        concatenate,
        easyConcatenate
    }
    public makeMode mode = makeMode.concatenate;
    private makeMode last_mode;
    public string menuName = "default";
    private string last_menuName;
    public Sprite menu;
    private Sprite last_menu;
    private GameObject image;
    private Vector2 lastRect = Vector2.zero;
    private uint catW;
    private uint catH;
    public Sprite easyMenu;
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
    private Sprite last_easyMenu;
    private Sprite last_menuLeft;
    private Sprite last_menuRight;
    private Sprite last_menuUp;
    private Sprite last_menuDown;
    private Sprite last_menuLeftUp;
    private Sprite last_menuRightUp;
    private Sprite last_menuLeftDown;
    private Sprite last_menuRightDown;
    private Sprite last_menuCenter;
    private Sprite last_menu_1x1;
    private Sprite last_menu_1x_c;
    private Sprite last_menu_1x_u;
    private Sprite last_menu_1x_d;
    private Sprite last_menu_x1_c;
    private Sprite last_menu_x1_l;
    private Sprite last_menu_x1_r;
    private GameObject[] images;
    private List<Vector2> spriteSizes = new List<Vector2>();
    private List<GameObject> addChildren = new List<GameObject>();
    private uint realW
    {
        get
        {
            return (uint)((int)GetComponent<RectTransform>().sizeDelta.x / (int)GetComponent<GridLayoutGroup>().cellSize.x);
        }
    }
    private uint realH
    {
        get
        {
            return (uint)((int)GetComponent<RectTransform>().sizeDelta.y / (int)GetComponent<GridLayoutGroup>().cellSize.y);
        }
    }
    void setCellSize()
    {
        if (1 == realH && 1 == realW)
        {
            GetComponent<GridLayoutGroup>().cellSize = (mode == makeMode.easyConcatenate ? easyMenu : menu_1x1).rect.size;
        }
        else if (1 == realH)
        {
            GetComponent<GridLayoutGroup>().cellSize = (mode == makeMode.easyConcatenate ? easyMenu : menu_x1_l).rect.size;
        }
        else if (1 == realW)
        {
            GetComponent<GridLayoutGroup>().cellSize = (mode == makeMode.easyConcatenate ? easyMenu : menu_1x_u).rect.size;
        }
        else
        {
            GetComponent<GridLayoutGroup>().cellSize = (mode == makeMode.easyConcatenate ? easyMenu : menuLeftUp).rect.size;
        }
    }
    void makeGrid()
    {
        setCellSize();
        catW = realW;
        catH = realH;
        if ((0 == catH || 0 == catW) || mode == makeMode.easyConcatenate ? null == easyMenu : ((1 == catH && 1 != catW && (null == menu_x1_l || null == menu_x1_r || 2 < catW && null == menu_x1_c)) || (1 != catH && 1 == catW && (null == menu_1x_u || null == menu_1x_d || 2 < catH && null == menu_1x_c)) || (1 == catH && 1 == catW && null == menu_1x1) || (1 != catH && 1 != catW && (null == menuLeftDown || null == menuRightDown || null == menuLeftUp || null == menuRightUp || 2 < catH && (null == menuLeft || null == menuRight) || 2 < catW && (null == menuUp || null == menuDown) || 2 < catH && 2 < catW && null == menuCenter))))
        {
            return;
        }
        //GridLayoutGroup的网格大小以第一个构成菜单的图片大小为准
        for (int y = 0; y < catH; y++)
        {
            for (int x = 0; x < catW; x++)
            {
                image = new GameObject(menuName + " - " + (y * catW + x));
                image.transform.parent = transform;
                addChildren.Add(image);
                image.AddComponent<Image>();
                if (1 == catH && 1 == catW)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menu_1x1);
                }
                else if (1 == catW && 0 == y)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menu_1x_u);
                }
                else if (1 == catW && catH - 1 == y)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menu_1x_d);
                }
                else if (1 == catW)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menu_1x_c);
                }
                else if (1 == catH && 0 == x)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menu_x1_l);
                }
                else if (1 == catH && catW - 1 == x)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menu_x1_r);
                }
                else if (1 == catH)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menu_x1_c);
                }
                else if (0 == x && 0 == y)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menuLeftUp);
                }
                else if (catW - 1 == x && 0 == y)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menuRightUp);
                }
                else if (0 == y)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menuUp);
                }
                else if (0 == x && catH - 1 == y)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menuLeftDown);
                }
                else if (catW - 1 == x && catH - 1 == y)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menuRightDown);
                }
                else if (catH - 1 == y)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menuDown);
                }
                else if (0 == x)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menuLeft);
                }
                else if (catW - 1 == x)
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menuRight);
                }
                else
                {
                    image.GetComponent<Image>().sprite = (mode == makeMode.easyConcatenate ? easyMenu : menuCenter);
                }
                image.GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
    }

    void Start()
    {
        last_mode = mode;
        lastRect = GetComponent<RectTransform>().sizeDelta;
        last_menu = menu;
        last_menuName = menuName;
        last_easyMenu = easyMenu;
        last_menuLeft = menuLeft;
        last_menuRight = menuRight;
        last_menuUp = menuUp;
        last_menuDown = menuDown;
        last_menuLeftUp = menuLeftUp;
        last_menuRightUp = menuRightUp;
        last_menuLeftDown = menuLeftDown;
        last_menuRightDown = menuRightDown;
        last_menuCenter = menuCenter;
        last_menu_1x1 = menu_1x1;
        last_menu_1x_c = menu_1x_c;
        last_menu_1x_u = menu_1x_u;
        last_menu_1x_d = menu_1x_d;
        last_menu_x1_c = menu_x1_c;
        last_menu_x1_l = menu_x1_l;
        last_menu_x1_r = menu_x1_r;
        if (null == GetComponent<GridLayoutGroup>())
        {
            gameObject.AddComponent<GridLayoutGroup>().enabled = false;
        }
        switch (mode)
        {
            case makeMode.scale:
                image = new GameObject(menuName);
                image.transform.parent = transform;
                addChildren.Add(image);
                image.AddComponent<Image>();
                image.GetComponent<Image>().rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
                image.GetComponent<Image>().sprite = menu ?? image.GetComponent<Image>().sprite;
                image.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
                image.GetComponent<RectTransform>().localScale = Vector3.one;
                break;
            default:
                gameObject.GetComponent<GridLayoutGroup>().enabled = true;
                makeGrid();
                break;
        }
    }

    void Update()
    {
        if (lastRect != GetComponent<RectTransform>().sizeDelta || last_menu != menu || last_easyMenu != easyMenu || last_menuLeft != menuLeft || last_menuRight != menuRight || last_menuUp != menuUp || last_menuDown != menuDown || last_menuLeftUp != menuLeftUp || last_menuRightUp != menuRightUp || last_menuLeftDown != menuLeftDown || last_menuRightDown != menuRightDown || last_menuCenter != menuCenter || last_menu_1x1 != menu_1x1 || last_menu_1x_c != menu_1x_c || last_menu_1x_u != menu_1x_u || last_menu_1x_d != menu_1x_d || last_menu_x1_c != menu_x1_c || last_menu_x1_l != menu_x1_l || last_menu_x1_r != menu_x1_r || last_menuName != menuName || last_mode != mode)
        {
            lastRect = GetComponent<RectTransform>().sizeDelta;
            last_mode = mode;
            last_menu = menu;
            last_menuName = menuName;
            last_easyMenu = easyMenu;
            last_menuLeft = menuLeft;
            last_menuRight = menuRight;
            last_menuUp = menuUp;
            last_menuDown = menuDown;
            last_menuLeftUp = menuLeftUp;
            last_menuRightUp = menuRightUp;
            last_menuLeftDown = menuLeftDown;
            last_menuRightDown = menuRightDown;
            last_menuCenter = menuCenter;
            last_menu_1x1 = menu_1x1;
            last_menu_1x_c = menu_1x_c;
            last_menu_1x_u = menu_1x_u;
            last_menu_1x_d = menu_1x_d;
            last_menu_x1_c = menu_x1_c;
            last_menu_x1_l = menu_x1_l;
            last_menu_x1_r = menu_x1_r;
            for (int i = 0; i < addChildren.Count; i++) { 
                Destroy(addChildren[i]);
            }
            addChildren.Clear();
            switch (mode)
            {
                case makeMode.scale:
                    GetComponent<GridLayoutGroup>().enabled = false;
                    image = new GameObject(menuName);
                    image.transform.parent = transform;
                    addChildren.Add(image);
                    image.AddComponent<Image>();
                    image.GetComponent<Image>().rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
                    image.GetComponent<Image>().sprite = menu ?? image.GetComponent<Image>().sprite;
                    image.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
                    image.GetComponent<RectTransform>().localScale = Vector3.one;
                    break;
                default:
                    GetComponent<GridLayoutGroup>().enabled = true;
                    makeGrid();
                    break;
            }
        }
    }
}
