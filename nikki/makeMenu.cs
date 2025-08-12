using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MenuTheme;
public class makeMenu : MonoBehaviour
{
    public Vector2 imageSize = new Vector2(1.5f, 1.5f);
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
    public Color menuColor = Color.white;
    private Color last_menuColor = Color.white;
    private static Vector2 cellSize = new Vector2();
    public static bool isDone = false;
    public static Vector2 CellSize
    {
        get
        {
            return cellSize;
        }
    }
    private uint realW
    {
        get
        {
            if (0 == (int)cellSize.x)
            {
                imageSize = cellSize = new Vector2(1, cellSize.y);
            }
            return (uint)((int)GetComponent<RectTransform>().sizeDelta.x / (int)cellSize.x);
        }
    }
    private uint realH
    {
        get
        {
            if (0 == (int)cellSize.y)
            {
                imageSize = cellSize = new Vector2(cellSize.x, 1);
            }
            return (uint)((int)GetComponent<RectTransform>().sizeDelta.y / (int)cellSize.y);
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
            setCellSize();
            GetComponent<RectTransform>().sizeDelta *= GetComponent<RectTransform>().localScale;
            GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.RoundToInt(GetComponent<RectTransform>().sizeDelta.x / cellSize.x) * (int)cellSize.x, Mathf.RoundToInt(GetComponent<RectTransform>().sizeDelta.y / cellSize.y) * (int)cellSize.y);
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
                    makeGrid();
                    break;
            }
            isDone = true;
        }
    }

    void setCellSize()
    {
        if (1 == realH && 1 == realW)
        {
            cellSize = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_1x1).rect.size;
        }
        else if (1 == realH)
        {
            cellSize = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_x1_l).rect.size;
        }
        else if (1 == realW)
        {
            cellSize = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menu_1x_u).rect.size;
        }
        else
        {
            cellSize = (menuTheme.mode == makeMode.easyConcatenate ? menuTheme.menu : menuTheme.menuLeftUp).rect.size;
        }
        cellSize *= imageSize;
    }
    IEnumerator makeARow(int y)
    {
        for (int x = 0; x < catW; x++)
        {
            image = new GameObject(("" != menuName ? menuName : name) + " - " + (y * catW + x));
            image.AddComponent<Image>();
            image.transform.SetParent(transform, true);
            image.transform.localScale = new Vector3(1, 1, 1);
            image.GetComponent<RectTransform>().sizeDelta = cellSize;
            image.GetComponent<RectTransform>().localPosition = new Vector3((0.5f + x) * cellSize.x + (0 - GetComponent<RectTransform>().pivot.x) * GetComponent<RectTransform>().sizeDelta.x, -(0.5f + y) * cellSize.y + (1 - GetComponent<RectTransform>().pivot.y) * GetComponent<RectTransform>().sizeDelta.y, 0);
            addChildren.Add(image);
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
            image.GetComponent<Image>().color = menuColor;
            image.GetComponent<RectTransform>().localScale = Vector3.one;
        }
        yield return null;
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
        //图片image的网格大小以第一个构成菜单的图片大小为准
        for (int y = 0; y < catH; y++)
        {
            StartCoroutine(makeARow(y));
        }
    }

    void Start()
    {
        isDone = false;
        init();
    }

    void Update()
    {
        GetComponent<RectTransform>().localScale = Vector3.one;
        init();
        if (initIsDone)
        {
            menuTheme = menuThemes[you.myMenuID];
            if (last_rect != GetComponent<RectTransform>().sizeDelta || last_imageSize != imageSize || last_ID != you.myMenuID || menuTheme.isEdit)
            {
                menuThemes[you.myMenuID].isEdit = false;
                menuTheme.isEdit = false;
                last_rect = GetComponent<RectTransform>().sizeDelta;
                last_imageSize = imageSize;
                last_menuName = menuName;
                last_ID = you.myMenuID;
                last_menuColor = menuColor;
                for (int i = 0; i < addChildren.Count; i++)
                {
                    Destroy(addChildren[i]);
                }
                addChildren.Clear();
                switch (menuTheme.mode)
                {
                    case makeMode.scale:
                        image = new GameObject("" != menuName ? menuName : name);
                        image.transform.parent = transform;
                        addChildren.Add(image);
                        image.AddComponent<Image>().color = menuColor;
                        image.GetComponent<Image>().rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
                        image.GetComponent<Image>().sprite = menuTheme.menu ?? image.GetComponent<Image>().sprite;
                        image.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
                        image.GetComponent<RectTransform>().localScale = Vector3.one;
                        break;
                    default:
                        makeGrid();
                        break;
                }
            }
            if (last_menuName != menuName)
            {
                last_menuName = menuName;
                if (makeMode.scale == menuTheme.mode)
                {
                    addChildren[0].name = "" != menuName ? menuName : name;
                }
                for (int i = 0; makeMode.scale != menuTheme.mode && i < addChildren.Count; i++) 
                {
                    addChildren[i].name = ("" != menuName ? menuName : name) + " - " + i;
                }
            }
            if (last_menuColor != menuColor) {
                last_menuColor = menuColor;
                for (int i = 0; i < addChildren.Count; i++)
                {
                    addChildren[i].GetComponent<Image>().color = menuColor;
                }
            }
        }
    }
}
