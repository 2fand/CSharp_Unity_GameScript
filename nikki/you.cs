using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static change;
using static Unity.VisualScripting.Member;
using UnityEditor;
using System;
using System.Xml.Linq;
using System.ComponentModel;
using static select;
using static effectItem;
using JetBrains.Annotations;
using Unity.VisualScripting.Dependencies.Sqlite;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

public class you : MonoBehaviour
{
    private bool isEnd = true;
    public static float waitTime = 0.01f;
    public int x = 5;
    public int y = 5;
    public static wasd face = wasd.s;
    public map m;
    private float high = 5;
    public float High
    {
        get
        {
            return high;
        }
    }
    public static bool isTele = false;
    public static int teleX = 0;
    public static int teleY = 0;
    public static float speed = 2;
    public static bool canMove = true;
    public static uint coins = 0;
    public static AudioClip closeSound;
    public static AudioClip teleSound;
    public static bool[] effecthaves = new bool[18];
    public static int effectNum = 0;
    public static enterMode enterMode = enterMode.show;
    public static bool notOver = false;
    private GameObject gameCamera;
    public static int myMenuID = 0;
    public GameObject GameCamera
    {
        get
        {
            return gameCamera;
        }
        set
        {
            gameCamera = value;
        }
    }
    public static effect nowEffect = effect.angel;
    public readonly int[] changes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4 };
    public static float teleHigh = 0;
    public static bool isChangeEffect = false;
    public static bool commandIsEnd = true;
    public static bool teleIsEnd = true;
    public static bool moveIsEnd = true;
    public static List<item> items = new List<item>();
    public static List<actionItem> actions = new List<actionItem>();
    public static bool itemsIsChanged = true;
    public static bool canTurn = true;
    public Vector3 cameraPosition = new Vector3(0, 82, -67.5f);
    public Vector3 cameraRotation = new Vector3(50, 0, 0);
    public bool cameraIsOrthographic = true;
    public float cameraOrthographicSize = 20;
    public static int money = 0;
    public static string moneyUnit = "null";
    public static GameObject teleScreen;
    public static Canvas canvas
    {
        get
        {
            return FindObjectOfType<Canvas>();
        }
    }
    public static AudioClip wakeUpSound;
    private GameObject effectGetScreen;
    public GameObject EffectGetScreen
    {
        get
        {
            return effectGetScreen;
        }
        set
        {
            effectGetScreen = value;
        }
    }
    private GameObject effectText;
    public GameObject EffectText
    {
        get
        {
            return effectText;
        }
        set
        {
            effectText = value;
        }
    }
    private bool isDone = false;
    public static AudioClip openMenuSound;
    public static AudioClip closeMenuSound;
    public static bool canOpenMenu = true;
    private GameObject selectMenu;
    private GameObject moneyMenu;
    private GameObject youMenu;
    public RectTransform YouMenuRectTransform
    {
        get {
            return youMenu.GetComponent<RectTransform>();
        }
    }
    private GameObject blackScreen;
    private GameObject selectCursor;
    private static List<menuClass> menus = new List<menuClass>();
    private GameObject exitMenu;
    private GameObject exitChooseMenu;
    private GameObject exitHintText;
    public static AudioClip changeSelectSound;
    public static string[] selectNames = { "效果", "醒来", "退出" };
    private menuClass[] selectMenuClasses = { menuClass.item, menuClass.action, menuClass.quit };
    private GameObject tempCursor1;
    private GameObject itemMenu;
    private GameObject itemTextMenu;
    private GameObject statusMenu;
    private GameObject recommendMenu;
    private GameObject recommendText;
    private GameObject moneyText;
    public static int NotNullItemsNum = 0;
    public static int NotNullActionsNum = 0;
    private static bool itemIsInit = false;
    private static bool teamIsInit = false;
    private static int itemSelectsColumnCount = 2;
    private static int actionSelectsColumnCount = 2;
    private static bool itemSelectsColumnCountIsChanged = false;
    private static bool actionSelectsColumnCountIsChanged = false;
    private static bool actionIsInit = false;
    public static bool actionIsChanged = true;
    public static List<people> yourTeam = new List<people>();
    public bool tempSwitch;
    private Hashtable selectFrom = new Hashtable();
    private GameObject peopleMenu;
    private List<people> peoples = new List<people>();
    private GameObject backGround;
    private GameObject backGroundMask;
    private List<GameObject> backGrounds = new List<GameObject>();
    private List<GameObject> backGroundMasks = new List<GameObject>();
    public List<GameObject> BackGrounds
    {
        get
        {
            return backGrounds;
        }
    }
    private static char hitObject = ' ';
    public static char HitObject
    {
        get
        {
            return hitObject;
        }
    }
    public static int ItemSelectsColumnCount
    {
        get
        {
            return itemSelectsColumnCount;
        }
        set
        {
            itemSelectsColumnCount = value;
            itemSelectsColumnCountIsChanged = true;
        }
    }
    public static int ActionSelectsColumnCount
    {
        get
        {
            return actionSelectsColumnCount;
        }
        set
        {
            actionSelectsColumnCount = value;
            actionSelectsColumnCountIsChanged = true;
        }
    }
#nullable enable
    private static you? u;
    public static you? You{
        get
        {
            return u;
        }
    }
    private static bool isStop = false;
    private static AudioClip? playSound = null;
    public static AudioClip? clearSound;
#nullable disable
    public static List<menuClass> Menus
    {
        get
        {
            return menus;
        }
        set
        {
            menus = value;
        }
    }
    public static menuClass? nowOpenMenu
    {
        get
        {
            if (menus.Count == 0)
            {
                return null;
            }
            return menus[menus.Count - 1];
        }
    }
    public static select[,] yourSelects
    {
        get
        {
            if (null == menuSelects || 0 == menus.Count || 0 == ((select[,])menuSelects[menus[menus.Count - 1]]).GetLength(0) || 0 == ((select[,])menuSelects[menus[menus.Count - 1]]).GetLength(1))
            {
                return null;
            }
            return (select[,])menuSelects[menus[menus.Count - 1]];
        }
    }
    public static select yourSelect
    {
        get
        {
            if (null == yourSelects || 0 == yourSelects.GetLength(0) || 0 == yourSelects.GetLength(1) || Cursor.IndexI >= yourSelects.GetLength(0) || Cursor.IndexJ >= yourSelects.GetLength(1))
            {
                return null;
            }
            return yourSelects[Cursor.IndexI, Cursor.IndexJ];
        }
    }
    public static MenuTheme myMenu
    {
        get
        {
            return MenuTheme.menuThemes[myMenuID];
        }
    }

    static void toRightPos(ref GameObject UIObject, float beforePivotX = 0.5f, float beforePivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
    {
        UIObject.GetComponent<RectTransform>().localPosition += new Vector3(UIObject.GetComponent<RectTransform>().sizeDelta.x * (pivotXMoveTo - beforePivotX), UIObject.GetComponent<RectTransform>().sizeDelta.y * (pivotYMoveTo - beforePivotY), 0);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, Vector2 postition, Vector2 sizeDelta, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
#nullable disable
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = postition;
        UIObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, Vector2 postition, Vector2 sizeDelta, Vector2 anchorMin, Vector2 anchorMax, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
    {
#nullable disable
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = postition;
        UIObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = anchorMin;
        UIObject.GetComponent<RectTransform>().anchorMax = anchorMax;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
    {
#nullable disable
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, float anchorX, float anchorY, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
    {
#nullable disable
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = UIObject.GetComponent<RectTransform>().anchorMax = new Vector2(anchorX, anchorY);
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public void UIinit(ref GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, float anchorMinX, float anchorMinY, float anchorMaxX, float anchorMaxY, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
    {
#nullable disable
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = new Vector2(anchorMinX, anchorMinY);
        UIObject.GetComponent<RectTransform>().anchorMax = new Vector2(anchorMaxX, anchorMaxY);
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, Vector2 postition, Vector2 sizeDelta, Color menuColor, Vector2 imageSize, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
    {
#nullable disable
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = postition;
        UIObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.AddComponent<makeMenu>().menuColor = menuColor;
        UIObject.GetComponent<makeMenu>().imageSize = imageSize;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, Vector2 postition, Vector2 sizeDelta, Vector2 anchorMin, Vector2 anchorMax, Color menuColor, Vector2 imageSize, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
    {
#nullable disable
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.GetComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.AddComponent<RectTransform>().localPosition = postition;
        UIObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = anchorMin;
        UIObject.GetComponent<RectTransform>().anchorMax = anchorMax;
        UIObject.AddComponent<makeMenu>().menuColor = menuColor;
        UIObject.GetComponent<makeMenu>().imageSize = imageSize;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, Color menuColor, Vector2 imageSize, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
    {
#nullable disable
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.AddComponent<makeMenu>().menuColor = menuColor;
        UIObject.GetComponent<makeMenu>().imageSize = imageSize;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, float anchorX, float anchorY, Color menuColor, Vector2 imageSize, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
    {
#nullable disable
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = UIObject.GetComponent<RectTransform>().anchorMax = new Vector2(anchorX, anchorY);
        UIObject.AddComponent<makeMenu>().menuColor = menuColor;
        UIObject.GetComponent<makeMenu>().imageSize = imageSize;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public void UIinit(ref GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, float anchorMinX, float anchorMinY, float anchorMaxX, float anchorMaxY, Color menuColor, Vector2 imageSize, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
    {
#nullable disable
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = new Vector2(anchorMinX, anchorMinY);
        UIObject.GetComponent<RectTransform>().anchorMax = new Vector2(anchorMaxX, anchorMaxY);
        UIObject.AddComponent<makeMenu>().menuColor = menuColor;
        UIObject.GetComponent<makeMenu>().imageSize = imageSize;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, Vector2 postition, Vector2 sizeDelta, Vector2 imageSize, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
    {

#nullable disable
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = postition;
        UIObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.AddComponent<makeMenu>().menuColor = new Color(1, 1, 1, 0);
        UIObject.GetComponent<makeMenu>().imageSize = imageSize;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public void UIinit(ref GameObject UIObject, string UIName, Vector2 postition, Vector2 sizeDelta, Vector2 anchorMin, Vector2 anchorMax, Vector2 imageSize, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
    {
#nullable disable
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.GetComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.AddComponent<RectTransform>().localPosition = postition;
        UIObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = anchorMin;
        UIObject.GetComponent<RectTransform>().anchorMax = anchorMax;
        UIObject.AddComponent<makeMenu>().menuColor = new Color(1, 1, 1, 0);
        UIObject.GetComponent<makeMenu>().imageSize = imageSize;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, Vector2 imageSize, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
#nullable disable
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.AddComponent<makeMenu>().menuColor = new Color(1, 1, 1, 0);
        UIObject.GetComponent<makeMenu>().imageSize = imageSize;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, float anchorX, float anchorY, Vector2 imageSize, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
#nullable disable
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = UIObject.GetComponent<RectTransform>().anchorMax = new Vector2(anchorX, anchorY);
        UIObject.AddComponent<makeMenu>().menuColor = new Color(1, 1, 1, 0);
        UIObject.GetComponent<makeMenu>().imageSize = imageSize;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, float anchorMinX, float anchorMinY, float anchorMaxX, float anchorMaxY, Vector2 imageSize, Transform? parent = null, float pivotX = 0.5f, float pivotY = 0.5f, float pivotXMoveTo = 0, float pivotYMoveTo = 1)
#nullable disable
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = new Vector2(pivotXMoveTo, pivotYMoveTo);
        UIObject.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = new Vector2(anchorMinX, anchorMinY);
        UIObject.GetComponent<RectTransform>().anchorMax = new Vector2(anchorMaxX, anchorMaxY);
        UIObject.AddComponent<makeMenu>().menuColor = new Color(1, 1, 1, 0);
        UIObject.GetComponent<makeMenu>().imageSize = imageSize;
        toRightPos(ref UIObject, pivotX, pivotY, pivotXMoveTo, pivotYMoveTo);
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, RectTransform copyTransform, Transform? parent = null)
#nullable disable
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = copyTransform.pivot;
        UIObject.GetComponent<RectTransform>().localPosition = copyTransform.localPosition;
        UIObject.GetComponent<RectTransform>().sizeDelta = copyTransform.sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = copyTransform.anchorMin;
        UIObject.GetComponent<RectTransform>().anchorMax = copyTransform.anchorMax;
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, RectTransform copyTransform, Color menuColor, Vector2 imageSize, Transform? parent = null)
#nullable disable
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = copyTransform.pivot;
        UIObject.GetComponent<RectTransform>().localPosition = copyTransform.localPosition;
        UIObject.GetComponent<RectTransform>().sizeDelta = copyTransform.sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = copyTransform.anchorMin;
        UIObject.GetComponent<RectTransform>().anchorMax = copyTransform.anchorMax;
        UIObject.AddComponent<makeMenu>().menuColor = menuColor;
        UIObject.GetComponent<makeMenu>().imageSize = imageSize;
    }
#nullable enable
    public static void UIinit(ref GameObject UIObject, string UIName, RectTransform copyTransform, Vector2 imageSize, Transform? parent = null)
#nullable disable
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = parent ?? canvas.transform;
        UIObject.AddComponent<RectTransform>().pivot = copyTransform.pivot;
        UIObject.GetComponent<RectTransform>().localPosition = copyTransform.localPosition;
        UIObject.GetComponent<RectTransform>().sizeDelta = copyTransform.sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = copyTransform.anchorMin;
        UIObject.GetComponent<RectTransform>().anchorMax = copyTransform.anchorMax;
        UIObject.AddComponent<makeMenu>().menuColor = new Color(1, 1, 1, 0);
        UIObject.GetComponent<makeMenu>().imageSize = imageSize;
    }
    void UIPosLink(ref GameObject child, GameObject parent)
    {
        ConstraintSource source = new ConstraintSource { weight = 1, sourceTransform = parent.transform };
        child.AddComponent<PositionConstraint>().weight = 1;
        child.GetComponent<PositionConstraint>().translationOffset = Vector3.zero;
        child.GetComponent<PositionConstraint>().translationAtRest = effectGetScreen.GetComponent<RectTransform>().position;
        child.GetComponent<PositionConstraint>().SetSources(new List<ConstraintSource> { source });
        child.GetComponent<PositionConstraint>().constraintActive = true;
    }

    void UIPosLink(ref GameObject child, GameObject parent, Vector3 offset)
    {
        ConstraintSource source = new ConstraintSource { weight = 1, sourceTransform = parent.transform };
        child.AddComponent<PositionConstraint>().weight = 1;
        child.GetComponent<PositionConstraint>().translationOffset = offset;
        child.GetComponent<PositionConstraint>().translationAtRest = effectGetScreen.GetComponent<RectTransform>().position;
        child.GetComponent<PositionConstraint>().SetSources(new List<ConstraintSource> { source });
        child.GetComponent<PositionConstraint>().constraintActive = true;
    }
    int get2DArrayLength(object[,] _2DArray)
    {
        return _2DArray.GetLength(0) * _2DArray.GetLength(1);
    }

    select[,] GetSelects(menuClass menuClass)
    {
        if (null == menuSelects[menuClass])
        {
            return null;
        }
        return (select[,])menuSelects[menuClass];
    }

    public static void textInit(ref GameObject text, int size, string str = "", TextAnchor alignment = TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass colorClass = MenuTheme.menuTextColorClass.normal, HorizontalWrapMode horizontalWrapMode = HorizontalWrapMode.Overflow, VerticalWrapMode verticalWrapMode = VerticalWrapMode.Overflow)
    {
        switch (colorClass)
        {
            case MenuTheme.menuTextColorClass.normal:
                text.AddComponent<Text>().color = myMenu.menuTextColor * new Color(1, 1, 1, 0);
                break;
            case MenuTheme.menuTextColorClass.highlight:
                text.AddComponent<Text>().color = myMenu.menuTextHighlightColor * new Color(1, 1, 1, 0);
                break;
            default:
                text.AddComponent<Text>().color = myMenu.menuTextUnselectColor * new Color(1, 1, 1, 0);
                break;
        }
        text.GetComponent<Text>().text = str;
        textAdjust.adjustText(ref text, alignment);
        text.GetComponent<Text>().alignment = alignment;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size;
        text.GetComponent<Text>().horizontalOverflow = horizontalWrapMode;
        text.GetComponent<Text>().verticalOverflow = verticalWrapMode;
    }

    public static void textInit(ref GameObject text, string str = "", TextAnchor alignment = TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass colorClass = MenuTheme.menuTextColorClass.normal, HorizontalWrapMode horizontalWrapMode = HorizontalWrapMode.Overflow, VerticalWrapMode verticalWrapMode = VerticalWrapMode.Overflow)
    {
        switch (colorClass)
        {
            case MenuTheme.menuTextColorClass.normal:
                text.AddComponent<Text>().color = myMenu.menuTextColor * new Color(1, 1, 1, 0);
                break;
            case MenuTheme.menuTextColorClass.highlight:
                text.AddComponent<Text>().color = myMenu.menuTextHighlightColor * new Color(1, 1, 1, 0);
                break;
            default:
                text.AddComponent<Text>().color = myMenu.menuTextUnselectColor * new Color(1, 1, 1, 0);
                break;
        }
        text.GetComponent<Text>().text = str;
        textAdjust.adjustText(ref text, alignment);
        text.GetComponent<Text>().alignment = alignment;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = textSize;
        text.GetComponent<Text>().horizontalOverflow = horizontalWrapMode;
        text.GetComponent<Text>().verticalOverflow = verticalWrapMode;
    }

    public static void textInit(ref GameObject text, int size, Color menuTextColor, string str = "", TextAnchor alignment = TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass colorClass = MenuTheme.menuTextColorClass.normal, HorizontalWrapMode horizontalWrapMode = HorizontalWrapMode.Overflow, VerticalWrapMode verticalWrapMode = VerticalWrapMode.Overflow)
    {
        switch (colorClass)
        {
            case MenuTheme.menuTextColorClass.normal:
                text.AddComponent<Text>().color = myMenu.menuTextColor * menuTextColor;
                break;
            case MenuTheme.menuTextColorClass.highlight:
                text.AddComponent<Text>().color = myMenu.menuTextHighlightColor * menuTextColor;
                break;
            default:
                text.AddComponent<Text>().color = myMenu.menuTextUnselectColor * menuTextColor;
                break;
        }
        text.GetComponent<Text>().text = str;
        textAdjust.adjustText(ref text, alignment);
        text.GetComponent<Text>().alignment = alignment;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = size;
        text.GetComponent<Text>().horizontalOverflow = horizontalWrapMode;
        text.GetComponent<Text>().verticalOverflow = verticalWrapMode;
    }

    public static void textInit(ref GameObject text, Color menuTextColor, string str = "", TextAnchor alignment = TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass colorClass = MenuTheme.menuTextColorClass.normal, HorizontalWrapMode horizontalWrapMode = HorizontalWrapMode.Overflow, VerticalWrapMode verticalWrapMode = VerticalWrapMode.Overflow)
    {
        switch (colorClass)
        {
            case MenuTheme.menuTextColorClass.normal:
                text.AddComponent<Text>().color = myMenu.menuTextColor * menuTextColor;
                break;
            case MenuTheme.menuTextColorClass.highlight:
                text.AddComponent<Text>().color = myMenu.menuTextHighlightColor * menuTextColor;
                break;
            default:
                text.AddComponent<Text>().color = myMenu.menuTextUnselectColor * menuTextColor;
                break;
        }
        text.GetComponent<Text>().text = str;
        textAdjust.adjustText(ref text, alignment);
        text.GetComponent<Text>().alignment = alignment;
        text.GetComponent<Text>().font = Game.gameFont;
        text.GetComponent<Text>().fontSize = textSize;
        text.GetComponent<Text>().horizontalOverflow = horizontalWrapMode;
        text.GetComponent<Text>().verticalOverflow = verticalWrapMode;
    }

    void menuSelectsAdd(menuClass menuClass, select[,] selects)
    {
        if (menuSelects.ContainsKey(menuClass))
        {
            menuSelects.Remove(menuClass);
        }
        menuSelects.Add(menuClass, selects);
    }

    void setPivot(ref GameObject UIObject, Vector2 pivotPos)
    {
        UIObject.GetComponent<RectTransform>().localPosition += new Vector3(((pivotPos - UIObject.GetComponent<RectTransform>().pivot) * UIObject.GetComponent<RectTransform>().sizeDelta).x, ((pivotPos - UIObject.GetComponent<RectTransform>().pivot) * UIObject.GetComponent<RectTransform>().sizeDelta).y);
        UIObject.GetComponent<RectTransform>().pivot = pivotPos;
    }

    void setPivot(ref GameObject UIObject, float pivotX, float pivotY)
    {
        UIObject.GetComponent<RectTransform>().localPosition += new Vector3(((new Vector2(pivotX, pivotY) - UIObject.GetComponent<RectTransform>().pivot) * UIObject.GetComponent<RectTransform>().sizeDelta).x, ((new Vector2(pivotX, pivotY) - UIObject.GetComponent<RectTransform>().pivot) * UIObject.GetComponent<RectTransform>().sizeDelta).y);
        UIObject.GetComponent<RectTransform>().pivot = new Vector2(pivotX, pivotY);
    }

    void initItem()
    {
        if (!itemIsInit)
        {
            item.addItem(new effectItem(0, true, true));
            //debug用↓
            item.addItem(new effectItem(effect.angel));
            item.addItem(new effectItem(effect.angel));
            item.addItem(new effectItem(effect.angel));
            item.addItem(new effectItem(effect.angel));
            item.addItem(new effectItem(effect.angel));
            item.addItem(new effectItem(effect.angel));
            item.addItem(new effectItem(effect.angel));
            item.addItem(new effectItem(effect.angel));
            item.addItem(new effectItem(effect.angel));
            //debug用↑
            itemIsInit = true;
        }
    }

    void initAction()
    {
        if (!actionIsInit)
        {
            item.addAction(new actionItem(new string[1] { "tele show hide nexus 15 15 0 s null 0" }, new AudioClip[1] { wakeUpSound }, "醒来", "尔可醒来，然何能离?"));
            actionIsInit = true;
        }
    }

    void initTeam()
    {
        for (int i = 0; i < peoples.Count; i++)
        {
            Destroy(peoples[i].nameTextObject);
            Destroy(peoples[i].hpTextObject);
            Destroy(peoples[i].mpTextObject);
            Destroy(peoples[i].expTextObject);
            Destroy(peoples[i].levelTextObject);
            Destroy(peoples[i].recommendTextObject);
            Destroy(peoples[i].peopleCloseUpBackGround);
            Destroy(peoples[i].peopleCloseUpInGame);
            Destroy(peoples[i].peopleCloseUpFrame);
        }
        if (!teamIsInit)
        {
            peoples.Add(new people("as", "asa", effectCloseup[(int)nowEffect], "keys", "_", "_", "_", true, 1, 1, true, 0, 0, true, 0, true));
            peoples.Add(new people("..aaa", "...", effectCloseup[(int)nowEffect], "keys", "_", "_", "_"));
            peoples.Add(new people("..", "...", effectCloseup[(int)nowEffect], "keys", "_", "_", "_"));
            teamIsInit = true;
        }
        else
        {
            for (int i = 0; i < yourTeam.Count; i++)
            {
                yourTeam[i].resetOther(i);
            }
        }
    }

    void initEffect()
    {
        //nowEffect = 0;
        funcsForComponentUI.updateArray(ref effecthaves, effectCount + 1);
    }

    Vector2 pivotTo0_1Offset(RectTransform UIObject)
    {
        return new Vector2((0 - UIObject.pivot.x) * UIObject.sizeDelta.x, (1 - UIObject.pivot.y) * UIObject.sizeDelta.y);
    }

    Vector2 pivotToX_YOffset(int x, int y, RectTransform UIObject)
    {
        return new Vector2((x - UIObject.pivot.x) * UIObject.sizeDelta.x, (y - UIObject.pivot.y) * UIObject.sizeDelta.y);
    }

    void initBackGround()
    {
        if (null != m.backGround && 0 != m.backGround.rect.size.x && 0 != m.backGround.rect.size.y)
        {
            Texture2D whiteTexture = new Texture2D(Mathf.CeilToInt(m.heightX), Mathf.CeilToInt(m.widthY));
            for (int x = 0; x < Mathf.CeilToInt(m.heightX); x++) { 
                for (int y = 0; y < Mathf.CeilToInt(m.widthY); y++)
                {
                    whiteTexture.SetPixel(x, y, Color.white);
                }
            }
            whiteTexture.Apply();
            for (int ib = 0; ib < 5 * 5; ib++)
            {
                backGroundMask = new GameObject("backGroundMask");
                backGroundMask.transform.localPosition = new Vector3(m.minX + (ib / 5 - 5 / 2) * m.heightX, m.transform.localPosition.y - 5, m.minY + (ib % 5 - 5 / 2) * m.widthY);
                backGroundMask.transform.rotation = Quaternion.Euler(90, 0, 0);
                backGroundMask.transform.localScale = new Vector3(100, 100, 1);
                backGroundMask.AddComponent<SpriteMask>().sprite = Sprite.Create(whiteTexture, new Rect(0, 0, Mathf.CeilToInt(m.heightX), Mathf.CeilToInt(m.widthY)), Vector2.zero);
                backGroundMask.AddComponent<backgroundMove>().face = m.backGroundMoveVector;
                backGroundMasks.Add(backGroundMask);
                for (int ia = 0; ia < Mathf.CeilToInt(backGroundMask.transform.localScale.y * 2 / m.backGround.rect.size.y); ia++)
                {
                    for (int i = 0; i < Mathf.CeilToInt(backGroundMask.transform.localScale.x * 2 / m.backGround.rect.size.x); i++)
                    {
                        backGround = new GameObject("background");
                        backGround.transform.localScale = new Vector3(100 / backGroundMask.transform.localScale.x, 100 / backGroundMask.transform.localScale.y, 1);
                        backGround.transform.localPosition = new Vector3(i * m.backGround.rect.size.x / backGroundMask.transform.localScale.x, ia * m.backGround.rect.size.y / backGroundMask.transform.localScale.y);
                        if (m.backGround.rect.size.x <= backGround.transform.localScale.x && m.backGround.rect.size.y <= backGround.transform.localScale.z)
                        {
                            backGround.AddComponent<SpriteRenderer>().sprite = Sprite.Create(m.backGround.texture, m.backGround.rect, Vector2.zero);
                        }
                        else
                        {
                            backGround.AddComponent<SpriteRenderer>().sprite = Sprite.Create(m.backGround.texture, new Rect(0, 0, m.heightX, m.widthY), Vector2.zero);
                        }
                        //backGround.GetComponent<SpriteRenderer>().sprite.;
                        backGround.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                        backGround.transform.SetParent(backGroundMask.transform, false);
                        backGrounds.Add(backGround);
                    }
                }
            }
        }
    }

    IEnumerator init()
    {
        UIinit(ref teleScreen, "teleScreen", 0, 0, Screen.width, Screen.height, 0, 0, 1, 1);
        teleScreen.AddComponent<Image>().color = Color.black;
        teleScreen.GetComponent<Image>().rectTransform.localScale = Vector3.one;
        //根据游戏y轴进行高度计算
        high = transform.position.y;
        //根据地图xy轴进行位置计算
        transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), transform.position.y, m.maxY - m.widthY / m.y * (0.5f + y));
        ConstraintSource source = new ConstraintSource { sourceTransform = transform, weight = 1 };
        gameCamera = new GameObject("gameCamera");
        gameCamera.transform.position = cameraPosition;
        gameCamera.tag = "MainCamera";
        gameCamera.transform.rotation = Quaternion.Euler(cameraRotation);
        gameCamera.AddComponent<AudioListener>();
        gameCamera.AddComponent<Camera>().orthographic = cameraIsOrthographic;
        gameCamera.GetComponent<Camera>().orthographicSize = cameraOrthographicSize;
        gameCamera.AddComponent<PositionConstraint>().SetSources(new List<ConstraintSource> { source });
        gameCamera.GetComponent<PositionConstraint>().translationAxis = Axis.X | Axis.Z;
        gameCamera.GetComponent<PositionConstraint>().translationOffset = cameraPosition;
        gameCamera.GetComponent<PositionConstraint>().translationAtRest = cameraPosition;
        gameCamera.GetComponent<PositionConstraint>().constraintActive = true;
        yield return new WaitUntil(() => null != You && null != map.Map);
        if (null == gameObject.GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }
        if (null == gameObject.GetComponent<runFunction>())
        {
            gameObject.AddComponent<runFunction>();
        }
        if (null == gameObject.GetComponent<Float>())
        {
            gameObject.AddComponent<Float>().enabled = false;
        }
        if (null == gameObject.GetComponent<changeColor>())
        {
            gameObject.AddComponent<changeColor>().enabled = false;
        }
        fastChangeEffect();
        initEffect();
        initItem();
        initAction();
        initBackGround();
        if (null == canvas.GetComponent<change>())
        {
            canvas.AddComponent<change>();
        }
        UIinit(ref effectGetScreen, "effectGetScreen", 0, 40, 360, 48, 0.5f, 0, new Vector2(1.5f, 1.5f));
        yield return new WaitUntil(() => myMenuID < MenuTheme.menuThemes.Count && InitGame.IsInit);
        UIinit(ref effectText, "effectText", 0, 40, 360, 50, 0.5f, 0);
        textInit(ref effectText, "", TextAnchor.MiddleCenter);
        UIPosLink(ref effectText, effectGetScreen);
        UIinit(ref blackScreen, "blackScreen", 0, 0, Screen.width, Screen.height, 0, 0, 1, 1);
        blackScreen.AddComponent<Image>().color = new Color(0, 0, 0, 0);
        UIinit(ref selectMenu, "selectMenu", -167.9f, 180f, 144, 1, new Vector2(1.5f, 1.5f));
        yield return new WaitUntil(() => Vector2.zero != makeMenu.CellSize);
        UIinit(ref moneyMenu, "moneyMenu", -167.9f, -155f , 144, 48, new Vector2(1.5f, 1.5f));
        UIinit(ref youMenu, "youMenu", 95f, 0, 384, 360, new Vector2(1.5f, 1.5f));
        UIinit(ref peopleMenu, "peopleMenu", youMenu.GetComponent<RectTransform>());
        peopleMenu.AddComponent<Mask>().showMaskGraphic = false;
        peopleMenu.AddComponent<Image>().color = new Color(0, 0, 0, 1);
        UIinit(ref selectCursor, "selectCursor", selectMenu.GetComponent<RectTransform>().localPosition.x, selectMenu.GetComponent<RectTransform>().localPosition.y - makeMenu.CellSize.y / 2, selectMenu.GetComponent<RectTransform>().sizeDelta.x, makeMenu.CellSize.y);
        selectCursor.AddComponent<Image>().sprite = myMenu.cursor;
        selectCursor.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        selectCursor.AddComponent<Cursor>();
        UIinit(ref exitMenu, "exitMenu", 0, 60, 169, 46, new Vector2(1.5f, 1.5f));
        setPivot(ref exitMenu, new Vector2(0.5f, 0.5f));
        UIinit(ref exitChooseMenu, "exitChooseMenu", 0, -25, 38, 69, new Vector2(1.5f, 1.5f));
        UIinit(ref exitHintText, "exitHintText", exitMenu.GetComponent<RectTransform>(), new Vector2(1.5f, 1.5f));
        textInit(ref exitHintText, 19, "要结束梦境了吗?", TextAnchor.MiddleCenter);
        exitMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(makeMenu.CellSize.x * exitHintText.GetComponent<Text>().text.Length, exitMenu.GetComponent<RectTransform>().sizeDelta.y);
        UIinit(ref moneyText, "moneyText", moneyMenu.GetComponent<RectTransform>());
        textInit(ref moneyText, "<color=#" + myMenu.menuTextColor.ToHexString().Substring(0, 6) + "00>" + money.ToString() + "</color> " + moneyUnit, TextAnchor.MiddleRight, MenuTheme.menuTextColorClass.highlight);
        UIinit(ref recommendMenu, "recommendMenu", 0, 145, 480, 44, new Vector2(1.5f, 1.5f));
        UIinit(ref recommendText, "recommendText", recommendMenu.GetComponent<RectTransform>());
        textInit(ref recommendText);
        //recommendText.GetComponent<Text>().text = "あ";
        UIinit(ref itemMenu, "itemMenu", 0, -23f, 480, 288, new Vector2(1.5f, 1.5f));
        selectMenu.GetComponent<makeMenu>().imageSize = new Vector2(1.5f, 1.5f);
        yield return new WaitUntil(() => makeMenu.isDone);
        initTeam();
        UIinit(ref itemTextMenu, "itemTextMenu", itemMenu.GetComponent<RectTransform>());
        itemTextMenu.AddComponent<Mask>().showMaskGraphic = false;
        itemTextMenu.AddComponent<Image>().color = new Color(0, 0, 0, 0.01f);
        select[,] mainSelects = new select[selectNames.Length, 1];
        select[,] quitSelects = new select[2, 1];
        selectMenuClasses = new menuClass[] { menuClass.item, menuClass.action, menuClass.quit };
        for (int i = 0; i < mainSelects.GetLength(0); i++)
        {
            mainSelects[i, 0] = new select(canvas, new Vector2(selectMenu.GetComponent<RectTransform>().localPosition.x, selectMenu.GetComponent<RectTransform>().localPosition.y - makeMenu.CellSize.y * (i + 0.5f)), new Vector2(selectMenu.GetComponent<RectTransform>().sizeDelta.x, makeMenu.CellSize.y), selectNames[i], selectMenuClasses[i], TextAnchor.MiddleLeft, 19);
        }
        for (int i = 0; i < 2; i++)
        {
            quitSelects[i, 0] = new select(canvas, new Vector2(exitChooseMenu.GetComponent<RectTransform>().localPosition.x, exitChooseMenu.GetComponent<RectTransform>().localPosition.y - (0.5f + i) * makeMenu.CellSize.y), new Vector2(exitChooseMenu.GetComponent<RectTransform>().sizeDelta.x, makeMenu.CellSize.y), new string[1]{0 == i ? "exit" : ""}, 0 == i ? "是" : "否", 0 == i ? menuClass.none : menuClass.back, TextAnchor.MiddleCenter, 19);
        }
        menuSelectsAdd(menuClass.main, mainSelects);
        menuSelectsAdd(menuClass.quit, quitSelects);
        selectMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(selectMenu.GetComponent<RectTransform>().sizeDelta.x, makeMenu.CellSize.y * (get2DArrayLength(GetSelects(menuClass.main)) + 1));
        //----------------------
        isDone = true;
    }

    wasd getwasd()
    {
        if (Input.GetKey("w"))
        {
            face = wasd.w;
        }
        else if (Input.GetKey("a"))
        {
            face = wasd.a;
        }
        else if (Input.GetKey("s"))
        {
            face = wasd.s;
        }
        else if(Input.GetKey("d"))
        {
            face = wasd.d;
        }
        if (Input.GetKey("w") && !(!m.verticalIsCycle && 0 == y))
        {
            hitObject = m.verticalIsCycle ? (m.wmap[x, y - 1 >= 0 ? y - 1 : m.y - 1]) : (0 <= y - 1 ? m.wmap[x, y - 1] : 'X');
            if (' ' != hitObject)
            {
                return wasd.n;
            }
            m.wmap[x, y--] = ' ';
            if (y < 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, m.minY - m.widthY / m.y / 2.0f);
                y = m.y - 1;
            }
            m.wmap[x, y] = 'I';
            return wasd.w;
        }
        else if (Input.GetKey("a") && !(!m.horizontalIsCycle && 0 == x))
        {
            hitObject = m.horizontalIsCycle ? m.wmap[x - 1 >= 0 ? x - 1 : m.x - 1, y] : (0 <= x - 1 ? m.wmap[x - 1, y] : 'X');
            if (' ' != hitObject)
            {
                return wasd.n;
            }
            m.wmap[x--, y] = ' ';
            if (x < 0)
            {
                transform.position = new Vector3(m.maxX + m.heightX / m.x / 2.0f, transform.position.y, transform.position.z);
                x = m.x - 1;
            }
            m.wmap[x, y] = 'I';
            return wasd.a;
        }
        else if (Input.GetKey("s") && !(!m.verticalIsCycle && y == m.y - 1))
        {
            hitObject = m.verticalIsCycle ? m.wmap[x, (y + 1) % m.y] : y != m.y - 1 ? m.wmap[x, y + 1] : 'X';
            if (' ' != hitObject)
            {
                return wasd.n;
            }
            m.wmap[x, y++] = ' ';
            if (y >= m.y)
            {
                transform.position = new Vector3(transform.position.x, high, m.maxY + m.widthY / m.y / 2.0f);
            }
            y %= m.y;
            m.wmap[x, y] = 'I';
            return wasd.s;
        }
        else if (Input.GetKey("d") && !(!m.horizontalIsCycle && m.x == x + 1))
        {
            hitObject = m.horizontalIsCycle ? m.wmap[(x + 1) % m.x, y] : (x != m.x - 1 ? m.wmap[x + 1, y] : 'X');
            if (' ' != hitObject)
            {
                return wasd.n;
            }
            m.wmap[x++, y] = ' ';
            if (x >= m.x)
            {
                transform.position = new Vector3(m.minX - m.heightX / m.x / 2.0f, transform.position.y, transform.position.z);
            }
            x %= m.x;
            m.wmap[x, y] = 'I';
            return wasd.d;
        }
        else 
        {
            return wasd.n;
        }
    }

    public IEnumerator move(wasd i, int step = 1, float tempSpeed = 1)
    {
        float youSpeed = speed;
        speed = tempSpeed;
        face = i;
        if (0 > step)
        {
            canTurn = false;
            i = (wasd)((int)(i + 2) % 4);
            step = Mathf.Abs(step);
        }
        if (moveIsEnd)
        {
            moveIsEnd = false;
            for (; step > 0; step--)
            {
                switch (i)
                {
                    case wasd.w:
                        if (m.verticalIsCycle || !m.verticalIsCycle && 0 != y)
                        {
                            m.wmap[x, y--] = ' ';
                            if (y < 0)
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, m.minY - m.widthY / m.y / 2.0f);
                                y = m.y - 1;
                            }
                            m.wmap[x, y] = 'I';
                        }
                        else
                        {
                            yield break;
                        }
                        break;
                    case wasd.a:
                        if (m.horizontalIsCycle || !m.horizontalIsCycle && 0 != x)
                        {
                            m.wmap[x--, y] = ' ';
                            if (x < 0)
                            {
                                transform.position = new Vector3(m.maxX + m.heightX / m.x / 2.0f, transform.position.y, transform.position.z);
                                x = m.x - 1;
                            }
                            m.wmap[x, y] = 'I';
                        }
                        else
                        {
                            yield break;
                        }
                        break;
                    case wasd.s:

                        if (m.verticalIsCycle || !m.verticalIsCycle && m.y - 1 != y)
                        {
                            m.wmap[x, y++] = ' ';
                            if (y >= m.y)
                            {
                                transform.position = new Vector3(transform.position.x, high, m.maxY + m.widthY / m.y / 2.0f);
                            }
                            y %= m.y;
                            m.wmap[x, y] = 'I';
                        }
                        else
                        {
                            yield break;
                        }
                        break;
                    default:
                        if (m.horizontalIsCycle ||!m.horizontalIsCycle && m.x - 1 != x)
                        {
                            m.wmap[x++, y] = ' ';
                            if (x >= m.x)
                            {
                                transform.position = new Vector3(m.minX - m.heightX / m.x / 2.0f, transform.position.y, transform.position.z);
                            }
                            x %= m.x;
                            m.wmap[x, y] = 'I';
                        }
                        else
                        {
                            yield break;
                        }
                        break;
                }
                if (null != (effectWalkSounds[(int)nowEffect] ?? effectWalkSounds[0]))
                {
                    GetComponent<AudioSource>().PlayOneShot(effectWalkSounds[(int)nowEffect] ?? effectWalkSounds[0]);
                }
                for (int j = 0; j < 10; j++)
                {
                    switch (i)//移动
                    {
                        case wasd.w:
                            transform.position += new Vector3(0, 0, m.widthY / m.y / 10.0f);
                            yield return new WaitForSeconds(0.1f / speed / 10.0f);
                            break;
                        case wasd.a:
                            transform.position += new Vector3(-m.heightX / m.x / 10.0f, 0, 0);
                            yield return new WaitForSeconds(0.1f / speed / 10.0f);
                            break;
                        case wasd.s:
                            transform.position += new Vector3(0, 0, -m.widthY / m.y / 10.0f);
                            yield return new WaitForSeconds(0.1f / speed / 10.0f);
                            break;
                        case wasd.d:
                            transform.position += new Vector3(m.heightX / m.x / 10.0f, 0, 0);
                            yield return new WaitForSeconds(0.1f / speed / 10.0f);
                            break;
                    }
                }
            }
            speed = youSpeed;
            canTurn = true;
            moveIsEnd = true;
        }
    }
    public IEnumerator hide()
    {
        commandIsEnd = false;
        GetComponent<MeshRenderer>().enabled = false;
        commandIsEnd = true;
        yield return null;
    }

    public IEnumerator show()
    {
        commandIsEnd = false;
        GetComponent<MeshRenderer>().enabled = true;
        commandIsEnd = true;
        yield return null;
    }

    public static IEnumerator play(AudioClip sound, bool isWait)
    {
        if (null == sound)
        {
            yield break;
        }
        commandIsEnd = !isWait || false;
        playSound = sound;
        yield return new WaitForSeconds(sound.length);
        commandIsEnd = true;
    }

    public static IEnumerator wait(float waitTime)
    {
        commandIsEnd = false;
        yield return new WaitForSeconds(waitTime);
        commandIsEnd = true;
    }

#nullable enable
    public static IEnumerator tele(exitMode exitMode, float exitModeTime, enterMode enterMode, float enterModeTime, string worldName, int teleX, int teleY, float teleHigh, wasd face = wasd.s, AudioClip? closeSound = null, AudioClip? teleSound = null, bool waitTeleSound = false)
#nullable disable
    {
        if (teleIsEnd)
        {
            teleIsEnd = false;
            canMove = false;
            canOpenMenu = false;
            npcMove.npcCanMove = false;
            npcMove.stopAnimation = true;
            //Debug.Log(enterMode + "," + exitMode);
            you.enterMode = enterMode;
            you.teleSound = teleSound ?? you.teleSound;
            yield return new WaitForSeconds(waitTeleSound && null != teleSound ? teleSound.length : 0);
            You.CoroutineStart((IEnumerator)transition.transitions[exitMode]);
            yield return new WaitUntil(() => transition.TransitionIsEnd);
            you.teleX = teleX;
            you.teleY = teleY;
            you.teleHigh = teleHigh;
            you.face = face;
            isTele = itemsIsChanged = actionIsChanged = true;
            if (worldName != SceneManager.GetActiveScene().name)
            {
                SceneManager.LoadScene(worldName);
            }
            else
            {
                yield return new WaitUntil(() => !isTele);
                yield return new WaitForSeconds(0.15f);
                isChangeEffect = true;
                You.CoroutineStart((IEnumerator)transition.transitions[enterMode]);
                yield return new WaitUntil(() => transition.TransitionIsEnd);
            }
            you.closeSound = closeSound ?? you.closeSound;
            npcMove.npcCanMove = true;
            canMove = true;
            canOpenMenu = true;
            teleIsEnd = true;
            npcMove.stopAnimation = false;
        }
        yield return null;
    }

    public static IEnumerator turn(wasd face)
    {
        commandIsEnd = false;
        you.face = face;
        commandIsEnd = true;
        yield return null;
    }

    public static IEnumerator stop()
    {
        commandIsEnd = false;
        isStop = true;
        commandIsEnd = true;
        yield return null;
    }

    public static IEnumerator exit()
    {
        Application.Quit();
        EditorApplication.isPlaying = false;
        yield return null;
    }

    void hideOrShowMenu(menuClass menuClass, bool isHide = true)
    {
        string moneyTextColor = Regex.Match(moneyText.GetComponent<Text>().text, "#[0-9a-fA-F]{8}").Value;
        Hashtable HexToDec = new Hashtable{ { '0', 0 }, { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 }, { '9', 9 }, { 'a', 10 }, { 'b', 11 }, { 'c', 12 }, { 'd', 13 }, { 'e', 14 }, { 'f', 15 }};
        string[] DecToHex = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
        int a = (int)HexToDec[moneyTextColor.ToLower()[7]] * 16 + (int)HexToDec[moneyTextColor.ToLower()[8]];
        switch (menuClass)
        {
            case menuClass.main:
                youMenu.GetComponent<makeMenu>().menuColor += new Color(0, 0, 0, isHide ? -0.2f : 0.2f);
                moneyMenu.GetComponent<makeMenu>().menuColor += new Color(0, 0, 0, isHide ? -0.2f : 0.2f);
                selectMenu.GetComponent<makeMenu>().menuColor += new Color(0, 0, 0, isHide ? -0.2f : 0.2f);
                moneyText.GetComponent<Text>().color += new Color(0, 0, 0, isHide ? -0.2f : 0.2f);
                a += (int)(myMenu.menuTextHighlightColor.a * 255 * (isHide ? -0.2f : 0.2f));
                moneyTextColor = Regex.Replace(moneyTextColor, "..$", DecToHex[a / 16] + DecToHex[a % 16]);
                moneyText.GetComponent<Text>().text = Regex.Replace(moneyText.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", moneyTextColor);
                people.addPeoplesColor(new Color(0, 0, 0, isHide ? -0.2f : 0.2f));
                break;
            case menuClass.quit:
                exitMenu.GetComponent<makeMenu>().menuColor += new Color(0, 0, 0, isHide ? -0.2f : 0.2f);
                exitChooseMenu.GetComponent<makeMenu>().menuColor += new Color(0, 0, 0, isHide ? -0.2f : 0.2f);
                exitHintText.GetComponent<Text>().color += new Color(0, 0, 0, isHide ? -0.2f : 0.2f);
                break;
            case menuClass.item:
            case menuClass.action:
                itemMenu.GetComponent<makeMenu>().menuColor += new Color(0, 0, 0, isHide ? -0.2f : 0.2f);
                recommendMenu.GetComponent<makeMenu>().menuColor += new Color(0, 0, 0, isHide ? -0.2f : 0.2f);
                recommendText.GetComponent<Text>().color += new Color(0, 0, 0, isHide ? -0.2f : 0.2f);
                break;
            default:
                break;
        }
        if (isHide && null != tempCursor1)
        {
            tempCursor1.GetComponent<Image>().color -= new Color(0, 0, 0, 0.2f);
        }
        else
        {
            Cursor.cursorColor += new Color(0, 0, 0, 0.2f);
        }
        addSelectsColor(new Color(0, 0, 0, isHide ? -0.2f : 0.2f), menuClass);
    }

    public IEnumerator openMenu(menuClass menuClass)
    {
        commandIsEnd = false;
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(openMenuSound);
        canMove = false;
        npcMove.npcCanMove = false;
        npcMove.stopAnimation = true;
        canOpenMenu = false;
        Cursor.cursorCanMove = false;
        Cursor.cursorColor = new Color(1, 1, 1, 0);
        string[] commands = new string[0];
        if (0 != menus.Count)
        {
            commands = yourSelect.commands;
        }
        GetComponent<AudioSource>().PlayOneShot(openMenuSound);
        menus.Add(menuClass);
        if (1 != menus.Count)
        {
            tempCursor1 = Instantiate(selectCursor, canvas.transform, true);
            tempCursor1.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            tempCursor1.GetComponent<Cursor>().enabled = false;
            tempCursor1.GetComponent<Image>().color = tempCursor1.GetComponent<Image>().color * new Color(1, 1, 1, 0) + new Color(0, 0, 0, 1);
        }
        else
        {
            tempCursor1 = null;
        }
        Cursor.update = true;
        for (int i = 0; 1 <= menus.Count && i < 5; i++)
        {
            if (1 == menus.Count)
            {
                blackScreen.GetComponent<Image>().color += new Color(0, 0, 0, 0.2f);
            }
            else
            {
                hideOrShowMenu(menus[menus.Count - 2]);
            }
            hideOrShowMenu(menus[menus.Count - 1], false);
            yield return new WaitForSeconds(0.05f);
        }
        Cursor.cursorCanMove = true;
        Destroy(tempCursor1);
        if (0 != menus.Count)
        {
            StartCoroutine(trigger.runCommands(commands, null, this));
        }
        canOpenMenu = true;
        commandIsEnd = true;
    }

    public IEnumerator closeMenu()
    {
        commandIsEnd = false;
        if (0 < menus.Count)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().PlayOneShot(closeMenuSound);
            canMove = true;
            if (1 == menus.Count)
            {
                npcMove.npcCanMove = true;
                npcMove.stopAnimation = false;
            }
            canOpenMenu = false;
            Cursor.cursorCanMove = false;
            Cursor.cursorColor = new Color(1, 1, 1, 0);
            menuClass last_menu = menus[menus.Count - 1];
            if (null != yourSelects && 0 != yourSelects.GetLength(0) && 0 != yourSelects.GetLength(1))
            {
                tempCursor1 = Instantiate(selectCursor, canvas.transform, true);
                tempCursor1.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                tempCursor1.GetComponent<Cursor>().enabled = false;
                tempCursor1.GetComponent<Image>().color = tempCursor1.GetComponent<Image>().color * new Color(1, 1, 1, 0) + new Color(0, 0, 0, 1);
            }
            menus.RemoveAt(menus.Count - 1);
            Cursor.update = true;
            yield return new WaitUntil(() => 0 == Menus.Count || !Cursor.update);
            for (int i = 0; i < 5; i++)
            {
                if (0 == menus.Count)
                {
                    blackScreen.GetComponent<Image>().color -= new Color(0, 0, 0, 0.2f);
                }
                else
                {
                    hideOrShowMenu(menus[menus.Count - 1], false);
                }
                hideOrShowMenu(last_menu);
                yield return new WaitForSeconds(0.05f);
            }
            Cursor.cursorCanMove = true;
            yield return 10;
            Destroy(tempCursor1);
            canOpenMenu = true;
        }
        commandIsEnd = true;
    }
#nullable enable
    public IEnumerator clearMenu()
#nullable disable
    {
        commandIsEnd = false;
        if (menus.Count > 0)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().PlayOneShot(clearSound.IsUnityNull() ? openMenuSound : clearSound);
            canMove = false;
            npcMove.npcCanMove = false;
            npcMove.stopAnimation = true;
            canOpenMenu = false;
            Cursor.cursorCanMove = false;
            menuClass last_menu = menus[menus.Count - 1];
            string[] commands = yourSelect.commands;
            menus.Clear();
            tempCursor1 = selectCursor;
            for (int i = 0; i < 5; i++)
            {
                blackScreen.GetComponent<Image>().color -= new Color(0, 0, 0, 0.2f);
                hideOrShowMenu(last_menu);
                yield return new WaitForSeconds(0.05f);
            }
            StartCoroutine(trigger.runCommands(commands, null, this));
            canOpenMenu = true;
            Cursor.cursorCanMove = true;
            if (0 == trigger.funcs.Count)
            {
                canMove = true;
            }
            npcMove.npcCanMove = true;
            npcMove.stopAnimation = false;
        }
        commandIsEnd = true;
    }

    IEnumerator pmove()
    {
        //初始
        moveIsEnd = false;
        isEnd = false;
        wasd i = getwasd();
        if (wasd.n == i)
        {
            goto nowait;
        }
        if (null != (effectWalkSounds[(int)nowEffect] ?? effectWalkSounds[0]))
        {
            GetComponent<AudioSource>().PlayOneShot(effectWalkSounds[(int)nowEffect] ?? effectWalkSounds[0]);
        }
        for (int j = 0; j < 20; j++)
        {
            switch (i)//移动
            {
                case wasd.w:
                    face = wasd.w;
                    transform.rotation = Quaternion.Euler(-90, 0, 180);
                    transform.position += new Vector3(0, 0, m.widthY / m.y / 20.0f);
                    yield return new WaitForSeconds(0.2f / speed / 20.0f);
                    break;
                case wasd.a:
                    face = wasd.a;
                    transform.rotation = Quaternion.Euler(-90, 0, 90);
                    transform.position += new Vector3(-m.heightX / m.x / 20.0f, 0, 0);
                    yield return new WaitForSeconds(0.2f / speed / 20.0f);
                    break;
                case wasd.s:
                    face = wasd.s;
                    transform.rotation = Quaternion.Euler(-90, 0, 0);
                    transform.position += new Vector3(0, 0, -m.widthY / m.y / 20.0f);
                    yield return new WaitForSeconds(0.2f / speed / 20.0f);
                    break;
                case wasd.d:
                    face = wasd.d;
                    transform.rotation = Quaternion.Euler(-90, 0, -90);
                    transform.position += new Vector3(m.heightX / m.x / 20.0f, 0, 0);
                    yield return new WaitForSeconds(0.2f / speed / 20.0f);//移动间隔时间
                    break;
                default:
                    goto nowait;
            }
        }
        yield return new WaitForSeconds(waitTime);//移动等待时间
    nowait:
        isEnd = true;
        moveIsEnd = true;
        yield return null;
    }

    public IEnumerator changeEffect(effect _effect)
    {
        commandIsEnd = false;
        canMove = false;
        canOpenMenu = false;
        npcMove.npcCanMove = false;
        if (0 == nowEffect ? null != effectEqiupSound : null != effectCancelEqiupSound)
        {
            GetComponent<AudioSource>().PlayOneShot(0 == nowEffect ? effectEqiupSound : effectCancelEqiupSound);
        }
        nowEffect = _effect;
        GetComponent<MeshRenderer>().material = screens[changes[UnityEngine.Random.Range(0, changes.Length)]];
        yield return new WaitForSeconds(0.2f);
        isChangeEffect = true;
        canMove = true;
        npcMove.npcCanMove = true;
        canOpenMenu = true;
        commandIsEnd = true;
    }

    void Awake()
    {
        StartCoroutine(init());
    }

    void Start()
    {
        if (null == effects)
        {
            Debug.LogError("初始化错误：当前世界并没有inityou组件");
            enabled = false;
            return;
        }
        GetComponent<MeshRenderer>().sharedMaterials = effects[0].GetComponent<MeshRenderer>().sharedMaterials;
        GetComponent<MeshFilter>().sharedMesh = effects[0].GetComponent<MeshFilter>().sharedMesh;
    }

    void changeItemSelects()
    {
        for (int i = 0; i < itemTextMenu.transform.childCount; i++)
        {
            if ("ItemMenu" == (string)selectFrom[itemTextMenu.transform.GetChild(i).gameObject])
            {
                Destroy(itemTextMenu.transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < get2DArrayLength(GetSelects(menuClass.item)); i++)
        {
            GetSelects(menuClass.item)[i / itemSelectsColumnCount, i % itemSelectsColumnCount] = null;
        }
        for (int i = 0, ia = 0; i < get2DArrayLength(GetSelects(menuClass.item)) && ia < items.Count; i++, ia++)
        {
            if (items[ia].isHide)
            {
                i--;
                continue;
            }
            GetSelects(menuClass.item)[i / itemSelectsColumnCount, i % itemSelectsColumnCount] = new select(itemTextMenu, new Vector2(i % itemSelectsColumnCount * itemTextMenu.GetComponent<RectTransform>().sizeDelta.x / itemSelectsColumnCount, -i / itemSelectsColumnCount * makeMenu.CellSize.y), new Vector2(itemTextMenu.GetComponent<RectTransform>().sizeDelta.x / itemSelectsColumnCount, makeMenu.CellSize.y), items[ia].name, menuClass.useItem);
            selectFrom.Add(GetSelects(menuClass.item)[i / itemSelectsColumnCount, i % itemSelectsColumnCount].text, "ItemMenu");
            GetSelects(menuClass.item)[i / itemSelectsColumnCount, i % itemSelectsColumnCount].UsedItem = items[ia];
        }
    }

    void changeActionSelects()
    {
        for (int i = 0; i < itemTextMenu.transform.childCount; i++)
        {
            if ("ActionMenu" == (string)selectFrom[itemTextMenu.transform.GetChild(i).gameObject])
            {
                Destroy(itemTextMenu.transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < get2DArrayLength(GetSelects(menuClass.action)); i++)
        {
            GetSelects(menuClass.action)[i / actionSelectsColumnCount, i % actionSelectsColumnCount] = null;
        }
        for (int i = 0, ia = 0; i < get2DArrayLength(GetSelects(menuClass.action)) && ia < actions.Count; i++, ia++)
        {
            if (actions[ia].isHide)
            {
                i--;
                continue;
            }
            GetSelects(menuClass.action)[i / actionSelectsColumnCount, i % actionSelectsColumnCount] = new select(itemTextMenu, new Vector2(i % itemSelectsColumnCount * itemTextMenu.GetComponent<RectTransform>().sizeDelta.x / actionSelectsColumnCount, -i / itemSelectsColumnCount * makeMenu.CellSize.y), new Vector2(itemTextMenu.GetComponent<RectTransform>().sizeDelta.x / itemSelectsColumnCount, makeMenu.CellSize.y), actions[ia].name, menuClass.doAction);
            selectFrom.Add(GetSelects(menuClass.action)[i / actionSelectsColumnCount, i % actionSelectsColumnCount].text, "ActionMenu");
            GetSelects(menuClass.action)[i / actionSelectsColumnCount, i % actionSelectsColumnCount].UsedItem = actions[ia];
        }
    }

    public void CoroutineStart(IEnumerator coroutine)
    {
        transition.resetTransitions();
        StartCoroutine(coroutine);
    }

    void sets()
    {
        if ((menuClass.item == nowOpenMenu || menuClass.action == nowOpenMenu) && null != yourSelect)
        {
            recommendText.GetComponent<Text>().text = yourSelect.UsedItem.recommendText;
        }
    }

    void OnEnable()
    {
        if (null == u || u.IsUnityNull())
        {
            u = this;
        }
    }

    void fastChangeEffect()
    {
        GetComponent<MeshFilter>().sharedMesh = effects[(int)nowEffect].GetComponent<MeshFilter>().sharedMesh ?? effects[0].GetComponent<MeshFilter>().sharedMesh;
        GetComponent<MeshRenderer>().sharedMaterials = effects[(int)nowEffect].GetComponent<MeshRenderer>().sharedMaterials ?? effects[0].GetComponent<MeshRenderer>().sharedMaterials;
        effectUse.getEffectUseAct(nowEffect)();
    }

    void Update()
    {
        if (isDone)
        {
            sets();
            while (get2DArrayLength(GetSelects(menuClass.item)) <= items.Count)
            {
                menuSelects[menuClass.item] = new select[GetSelects(menuClass.item).GetLength(0) * 2 + 1, itemSelectsColumnCount];
            }
            while (get2DArrayLength(GetSelects(menuClass.action)) <= actions.Count)
            {
                menuSelects[menuClass.action] = new select[GetSelects(menuClass.action).GetLength(0) * 2 + 1, actionSelectsColumnCount];
            }
            if (itemsIsChanged || itemSelectsColumnCountIsChanged)
            {
                itemSelectsColumnCountIsChanged = itemsIsChanged = false;
                changeItemSelects();
            }
            if (actionIsChanged || actionSelectsColumnCountIsChanged)
            {
                actionSelectsColumnCountIsChanged = actionIsChanged = false;
                changeActionSelects();
            }
            if (null != playSound)
            {
                GetComponent<AudioSource>().PlayOneShot(playSound);
            }
            else if (isStop)
            {
                GetComponent<AudioSource>().Stop();
            }
            if (isTele)
            {
                x = teleX;
                y = teleY;
                transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), teleHigh, m.maxY - m.widthY / m.y * (0.5f + y));
                isTele = false;
            }
            if (canTurn)
            {
                switch (face)
                {
                    case wasd.w:
                        transform.rotation = Quaternion.Euler(-90, 0, 180);
                        break;
                    case wasd.a:
                        transform.rotation = Quaternion.Euler(-90, 0, 90);
                        break;
                    case wasd.s:
                        transform.rotation = Quaternion.Euler(-90, 0, 0);
                        break;
                    default:
                        transform.rotation = Quaternion.Euler(-90, 0, -90);
                        break;
                }
            }
            if (null != teleSound)
            {
                GetComponent<AudioSource>().PlayOneShot(teleSound);
                teleSound = null;
            }
            if (null != closeSound)
            {
                GetComponent<AudioSource>().PlayOneShot(closeSound);
                closeSound = null;
            }
            m.wmap[x, y] = 'I';
            if (Input.GetKeyDown("x") && canOpenMenu)
            {
                if (0 == menus.Count)
                {
                    StartCoroutine(openMenu(menuClass.main));
                }
                else
                {
                    StartCoroutine(closeMenu());
                }
            }
            if (Input.GetKeyDown("z") && canOpenMenu && 0 != menus.Count && null != yourSelects && null != yourSelect && 0 != yourSelects.GetLength(0) && 0 != yourSelects.GetLength(0) && yourSelect.canSelected && menuClass.cantIn != yourSelect.MenuClass)
            {
                if (menuClass.back == yourSelect.MenuClass)
                {
                    StartCoroutine(closeMenu());
                }
                else if (menuClass.useItem == yourSelect.MenuClass)
                {
                    StartCoroutine(clearMenu());
                }
                else if (menuClass.doAction == yourSelect.MenuClass)
                { 
                    StartCoroutine(trigger.runCommands(((actionItem)yourSelect.UsedItem).commands, ((actionItem)yourSelect.UsedItem).sounds, this));
                }
                else
                {
                    StartCoroutine(openMenu(yourSelect.MenuClass));
                }
            }
            if (isChangeEffect)
            {
                fastChangeEffect();
                isChangeEffect = false;
            }
            if (isEnd && canMove && 0 == menus.Count)
            {
                StartCoroutine(pmove());
            }
        }
    }
}
