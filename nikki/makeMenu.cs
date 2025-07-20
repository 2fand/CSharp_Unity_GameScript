using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MenuTheme;
public class makeMenu : MonoBehaviour
{
    public Vector2 imageSize = Vector2.one;
    private Vector2 last_imageSize;
    private GameObject image;
    private Vector2 last_rect;
    private uint catW;
    private uint catH;
    private GameObject[] images;
    private List<Vector2> spriteSizes = new List<Vector2>();
    private List<GameObject> addChildren = new List<GameObject>();
    private MenuTheme menuTheme;
    public string menuName = "";
    private string last_menuName;
    private bool initIsDone = false;
    private int last_ID;
    private uint realW
    {
        get
        {
            if (0 == (int)GetComponent<GridLayoutGroup>().cellSize.x)
            {
                GetComponent<GridLayoutGroup>().cellSize = new Vector2(1, GetComponent<GridLayoutGroup>().cellSize.y);
                imageSize = new Vector2(1, imageSize.y);
            }
            return (uint)((int)GetComponent<RectTransform>().sizeDelta.x / (int)GetComponent<GridLayoutGroup>().cellSize.x);
        }
    }
    private uint realH
    {
        get
        {
            if (0 == (int)GetComponent<GridLayoutGroup>().cellSize.y)
            {
                GetComponent<GridLayoutGroup>().cellSize = new Vector2(GetComponent<GridLayoutGroup>().cellSize.x, 1);
                imageSize = new Vector2(imageSize.x, 1);
            }
            return (uint)((int)GetComponent<RectTransform>().sizeDelta.y / (int)GetComponent<GridLayoutGroup>().cellSize.y);
        }
    }
    void init()
    {
        if (!initIsDone && menuThemes.Count > you.myMenuID)
        {
            initIsDone = true;
            menuTheme = menuThemes[you.myMenuID];
            last_menuName = menuName;
            last_ID = you.myMenuID;
            if (null == GetComponent<GridLayoutGroup>())
            {
                gameObject.AddComponent<GridLayoutGroup>().enabled = false;
            }
            setCellSize();
            GetComponent<RectTransform>().sizeDelta *= GetComponent<RectTransform>().localScale;
            GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.RoundToInt(GetComponent<RectTransform>().sizeDelta.x / GetComponent<GridLayoutGroup>().cellSize.x) * (int)GetComponent<GridLayoutGroup>().cellSize.x, Mathf.RoundToInt(GetComponent<RectTransform>().sizeDelta.y / GetComponent<GridLayoutGroup>().cellSize.y) * (int)GetComponent<GridLayoutGroup>().cellSize.y);
            GetComponent<RectTransform>().localScale = Vector3.one;
            last_imageSize = imageSize;
            last_rect = GetComponent<RectTransform>().sizeDelta;
            switch (menuTheme.mode)
            {
                case makeMode.scale:
                    image = new GameObject("" != menuName ? menuName : name);
                    image.transform.parent = transform;
                    addChildren.Add(image);
                    image.AddComponent<Image>();
                    image.GetComponent<Image>().rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
                    image.GetComponent<Image>().sprite = menuTheme.menu ?? image.GetComponent<Image>().sprite;
                    image.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
                    image.GetComponent<RectTransform>().localScale = Vector3.one;
                    break;
                default:
                    gameObject.GetComponent<GridLayoutGroup>().enabled = true;
                    makeGrid();
                    break;
            }
        }
    }

    void setCellSize()
    {
        if (1 == realH && 1 == realW)
        {
            GetComponent<GridLayoutGroup>().cellSize = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_1x1).rect.size;
        }
        else if (1 == realH)
        {
            GetComponent<GridLayoutGroup>().cellSize = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_x1_l).rect.size;
        }
        else if (1 == realW)
        {
            GetComponent<GridLayoutGroup>().cellSize = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_1x_u).rect.size;
        }
        else
        {
            GetComponent<GridLayoutGroup>().cellSize = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menuLeftUp).rect.size;
        }
        GetComponent<GridLayoutGroup>().cellSize *= imageSize;
    }
    void makeGrid()
    {
        setCellSize();
        catW = realW;
        catH = realH;
        if ((0 == catH || 0 == catW) || menuTheme.mode == makeMode.easyConcatenate ? null == menuTheme.menu : ((1 == catH && 1 != catW && (null == menuTheme.menu_x1_l || null == menuTheme.menu_x1_r || 2 < catW && null == menuTheme.menu_x1_c)) || (1 != catH && 1 == catW && (null == menuTheme.menu_1x_u || null == menuTheme.menu_1x_d || 2 < catH && null == menuTheme.menu_1x_c)) || (1 == catH && 1 == catW && null == menuTheme.menu_1x1) || (1 != catH && 1 != catW && (null == menuTheme.menuLeftDown || null == menuTheme.menuRightDown || null == menuTheme.menuLeftUp || null == menuTheme.menuRightUp || 2 < catH && (null == menuTheme.menuLeft || null == menuTheme.menuRight) || 2 < catW && (null == menuTheme.menuUp || null == menuTheme.menuDown) || 2 < catH && 2 < catW && null == menuTheme.menuCenter))))
        {
            return;
        }
        //GridLayoutGroup的网格大小以第一个构成菜单的图片大小为准
        for (int y = 0; y < catH; y++)
        {
            for (int x = 0; x < catW; x++)
            {
                image = new GameObject(("" != menuName ? menuName : name) + " - " + (y * catW + x));
                image.transform.parent = transform;
                image.transform.localScale = imageSize;
                addChildren.Add(image);
                image.AddComponent<Image>();
                if (1 == catH && 1 == catW)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_1x1);
                }
                else if (1 == catW && 0 == y)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_1x_u);
                }
                else if (1 == catW && catH - 1 == y)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_1x_d);
                }
                else if (1 == catW)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_1x_c);
                }
                else if (1 == catH && 0 == x)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_x1_l);
                }
                else if (1 == catH && catW - 1 == x)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_x1_r);
                }
                else if (1 == catH)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_x1_c);
                }
                else if (0 == x && 0 == y)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menuLeftUp);
                }
                else if (catW - 1 == x && 0 == y)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menuRightUp);
                }
                else if (0 == y)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menuUp);
                }
                else if (0 == x && catH - 1 == y)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menuLeftDown);
                }
                else if (catW - 1 == x && catH - 1 == y)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menuRightDown);
                }
                else if (catH - 1 == y)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menuDown);
                }
                else if (0 == x)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menuLeft);
                }
                else if (catW - 1 == x)
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menuRight);
                }
                else
                {
                    image.GetComponent<Image>().sprite = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menuCenter);
                }
                image.GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
    }

    void Start()
    {
        init();
    }

    void Update()
    {
        GetComponent<RectTransform>().localScale = Vector3.one;
        init();
        if (initIsDone)
        {
            menuTheme = menuThemes[you.myMenuID];
            if (last_rect != GetComponent<RectTransform>().sizeDelta || last_imageSize != imageSize || last_menuName != menuName || last_ID != you.myMenuID || menuTheme.isEdit)
            {
                menuThemes[you.myMenuID].isEdit = false;
                menuTheme.isEdit = false;
                last_rect = GetComponent<RectTransform>().sizeDelta;
                last_imageSize = imageSize;
                last_menuName = menuName;
                last_ID = you.myMenuID;
                for (int i = 0; i < addChildren.Count; i++)
                {
                    Destroy(addChildren[i]);
                }
                addChildren.Clear();
                switch (menuTheme.mode)
                {
                    case makeMode.scale:
                        GetComponent<GridLayoutGroup>().enabled = false;
                        image = new GameObject("" != menuName ? menuName : name);
                        image.transform.parent = transform;
                        addChildren.Add(image);
                        image.AddComponent<Image>();
                        image.GetComponent<Image>().rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
                        image.GetComponent<Image>().sprite = menuTheme.menu ?? image.GetComponent<Image>().sprite;
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
}
