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
using static getChangeObject;
using UnityEditor;

public class you : MonoBehaviour
{
    public enum wasd
    {
        w,
        a,
        s,
        d,
        n
    };
    private bool isEnd = true;
    public static float waitTime = 0.01f;
    public int x = 5;
    public int y = 5;
    public static wasd front = wasd.s;
    public map m;
    private float high = 5;
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
    public static readonly string[] effectName = { "", "天使", "锁门" };
    public static enterMode enterMode = enterMode.show;
    public static bool notOver = false;
    public static AudioClip defaultWalkSound;
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
    public static AudioClip effectEqiupSound;
    public static AudioClip effectCancelEqiupSound;
    public static effect nowEffect = effect.none;
    public static Material[] screens;
    public readonly int[] change = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4 };
    public static GameObject[] effects;
    public static float teleHigh = 0;
    public static bool isChangeEffect = false;
    public static AudioClip[] effectWalkSounds;
    public static bool commandIsEnd = true;
    public static bool teleIsEnd = true;
    public static bool moveIsEnd = true;
    public static List<item> items = new List<item>();
    public static bool canTurn = true;
    public Vector3 cameraPosition = new Vector3(0, 82, -67.5f);
    public Vector3 cameraRotation = new Vector3(50, 0, 0);
    public bool cameraIsOrthographic = true;
    public float cameraOrthographicSize = 20;
    public static int money = 0;
    public static string moneyUnit = "";
    public static GameObject teleScreen;
    public Canvas canvas;
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
    private static bool isOpenMenu = false;
    public static bool IsOpenMenu
    {
        get
        {
            return isOpenMenu;
        }
    }
    public static bool canOpenMenu = true;
    private GameObject selectMenu;
    private GameObject moneyMenu;
    private GameObject youMenu;
    private GameObject blackScreen;
    public static select[] selects;
    public static Vector2 menuSelectsPositionAdjust = new Vector2(7, -2);
    public static float menuSelectsSpaceAdjust = -2;
    private static Vector2 last_menuSelectsPositionAdjust;
    private static float last_menuSelectsSpaceAdjust;
    private GameObject selectCursor;
    private void effectInit()
    {
        speed = 1;
        transform.position = new Vector3(transform.position.x, high, transform.position.z);
        gameObject.GetComponent<Float>().enabled = false;
        gameObject.GetComponent<changeColor>().enabled = false;
    }

    GameObject UIinit(GameObject UIObject, string UIName, Vector2 postition, Vector2 sizeDelta)
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = canvas.transform;
        UIObject.AddComponent<RectTransform>().localPosition = postition;
        UIObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        return UIObject;
    }

    GameObject UIinit(GameObject UIObject, string UIName, Vector2 postition, Vector2 sizeDelta, Vector2 anchorMin, Vector2 anchorMax)
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = canvas.transform;
        UIObject.AddComponent<RectTransform>().localPosition = postition;
        UIObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = anchorMin;
        UIObject.GetComponent<RectTransform>().anchorMax = anchorMax;
        return UIObject;
    }

    GameObject UIinit(GameObject UIObject, string UIName, Vector2 postition, Vector2 sizeDelta, Vector2 pivotPosition)
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = canvas.transform;
        UIObject.AddComponent<RectTransform>().localPosition = postition;
        UIObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().pivot = pivotPosition;
        return UIObject;
    }

    GameObject UIinit(GameObject UIObject, string UIName, Vector2 postition, Vector2 sizeDelta, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivotPosition)
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = canvas.transform;
        UIObject.AddComponent<RectTransform>().localPosition = postition;
        UIObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = anchorMin;
        UIObject.GetComponent<RectTransform>().anchorMax = anchorMax;
        UIObject.GetComponent<RectTransform>().pivot = pivotPosition;
        return UIObject;
    }

    GameObject UIinit(GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY)
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = canvas.transform;
        UIObject.AddComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        return UIObject;
    }

    GameObject UIinit(GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, Vector2 pivotPosition)
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = canvas.transform;
        UIObject.AddComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().pivot = pivotPosition;
        return UIObject;
    }

    GameObject UIinit(GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, float anchorX, float anchorY)
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = canvas.transform;
        UIObject.AddComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = UIObject.GetComponent<RectTransform>().anchorMax = new Vector2(anchorX, anchorY);
        return UIObject;
    }

    GameObject UIinit(GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, float anchorMinX, float anchorMinY, float anchorMaxX, float anchorMaxY)
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = canvas.transform;
        UIObject.AddComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = new Vector2(anchorMinX, anchorMinY);
        UIObject.GetComponent<RectTransform>().anchorMax = new Vector2(anchorMaxX, anchorMaxY);
        return UIObject;
    }

    GameObject UIinit(GameObject UIObject, string UIName, float x, float y, float sizeX, float sizeY, float anchorMinX, float anchorMinY, float anchorMaxX, float anchorMaxY, float pivotX, float pivotY)
    {
        UIObject = new GameObject(UIName);
        UIObject.transform.parent = canvas.transform;
        UIObject.AddComponent<RectTransform>().localPosition = new Vector2(x, y);
        UIObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
        UIObject.GetComponent<RectTransform>().localScale = Vector3.one;
        UIObject.GetComponent<RectTransform>().anchorMin = new Vector2(anchorMinX, anchorMinY);
        UIObject.GetComponent<RectTransform>().anchorMax = new Vector2(anchorMaxX, anchorMaxY);
        UIObject.GetComponent<RectTransform>().pivot = new Vector2(pivotX, pivotY);
        return UIObject;
    }

    GameObject UIPosLink(GameObject child, GameObject parent)
    {
        ConstraintSource source = new ConstraintSource { weight = 1, sourceTransform = parent.transform };
        child.AddComponent<PositionConstraint>().weight = 1;
        child.GetComponent<PositionConstraint>().translationOffset = Vector3.zero;
        child.GetComponent<PositionConstraint>().translationAtRest = effectGetScreen.GetComponent<RectTransform>().position;
        child.GetComponent<PositionConstraint>().SetSources(new List<ConstraintSource> { source });
        child.GetComponent<PositionConstraint>().constraintActive = true;
        return child;
    }

    GameObject UIPosLink(GameObject child, GameObject parent, Vector3 offset)
    {
        ConstraintSource source = new ConstraintSource { weight = 1, sourceTransform = parent.transform };
        child.AddComponent<PositionConstraint>().weight = 1;
        child.GetComponent<PositionConstraint>().translationOffset = offset;
        child.GetComponent<PositionConstraint>().translationAtRest = effectGetScreen.GetComponent<RectTransform>().position;
        child.GetComponent<PositionConstraint>().SetSources(new List<ConstraintSource> { source });
        child.GetComponent<PositionConstraint>().constraintActive = true;
        return child;
    }

    IEnumerator init()
    {
        items.Add(new effectItem(effect.none, this));
        if (null == gameObject.GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }
        if (null == gameObject.GetComponent<Float>())
        {
            gameObject.AddComponent<Float>().enabled = false;
        }
        if (null == gameObject.GetComponent<changeColor>())
        {
            gameObject.AddComponent<changeColor>().enabled = false;
        }
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
        teleScreen = UIinit(teleScreen, "teleScreen", 0, 0, Screen.width, Screen.height, 0, 0, 1, 1);
        teleScreen.AddComponent<Image>().color = Color.black;
        teleScreen.GetComponent<Image>().rectTransform.localScale = Vector3.one;
        if (null == canvas.GetComponent<change>())
        {
            canvas.AddComponent<change>();
        }
        effectGetScreen = UIinit(effectGetScreen, "effectGetScreen", 0, 40, 360, 48, 0.5f, 0);
        effectGetScreen.AddComponent<makeMenu>().imageSize = new Vector2(1.5f, 1.5f);
        effectGetScreen.GetComponent<makeMenu>().menuColor = new Color(1, 1, 1, 0);
        yield return new WaitUntil(() => myMenuID < MenuTheme.menuThemes.Count && InitGame.IsInit);
        effectText = UIinit(effectText, "effectText", 0, 40, 360, 50, 0.5f, 0);
        effectText.AddComponent<Text>().color = MenuTheme.menuThemes[myMenuID].menuTextColor;
        effectText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        effectText.GetComponent<Text>().font = Game.gameFont;
        effectText.GetComponent<Text>().fontSize = 30;
        effectText = UIPosLink(effectText, effectGetScreen);
        blackScreen = UIinit(blackScreen, "blackScreen", 0, 0, Screen.width, Screen.height, 0, 0, 1, 1);
        blackScreen.AddComponent<Image>().color = new Color(0, 0, 0, 0);
        selects = new select[] { new select(canvas, "效果", select.menuClass.use), new select(canvas, "醒来", select.menuClass.action), new select(canvas, "退出", select.menuClass.quit) };
        selectMenu = UIinit(selectMenu, "selectMenu", -125.6f, 120, 96, 96, new Vector2(0.5f, 1));
        selectMenu.AddComponent<makeMenu>().imageSize = new Vector2(1.5f, 1.5f);
        selectMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(96, makeMenu.CellSize.y * selects.Length);
        selectMenu.GetComponent<makeMenu>().menuColor = new Color(1, 1, 1, 0);
        last_menuSelectsPositionAdjust = menuSelectsPositionAdjust;
        last_menuSelectsSpaceAdjust = menuSelectsSpaceAdjust;
        for (int i = 0; i < selects.Length; i++)
        {
            selects[i].text.GetComponent<RectTransform>().localPosition = new Vector3(selectMenu.GetComponent<RectTransform>().localPosition.x + menuSelectsPositionAdjust.x, (selectMenu.GetComponent<RectTransform>().localPosition.y + menuSelectsPositionAdjust.y) - (makeMenu.CellSize.y + menuSelectsSpaceAdjust) * (0.5f + i));
            selects[i].text.GetComponent<RectTransform>().sizeDelta = new Vector2(selectMenu.GetComponent<RectTransform>().sizeDelta.x, makeMenu.CellSize.y);
        }
        moneyMenu = UIinit(moneyMenu, "moneyMenu", -125.6f, -107.82f, 96, 24);
        moneyMenu.AddComponent<makeMenu>().imageSize = new Vector2(1.5f, 1.5f);
        moneyMenu.GetComponent<makeMenu>().menuColor = new Color(1, 1, 1, 0);
        youMenu = UIinit(youMenu, "youMenu", 51.7f, 0, 260, 240);
        youMenu.AddComponent<makeMenu>().imageSize = new Vector2(1.5f, 1.5f);
        youMenu.GetComponent<makeMenu>().menuColor = new Color(1, 1, 1, 0);
        selectCursor = UIinit(selectCursor, "selectCursor", selectMenu.GetComponent<RectTransform>().localPosition.x, selectMenu.GetComponent<RectTransform>().localPosition.y - makeMenu.CellSize.y / 2, selectMenu.GetComponent<RectTransform>().sizeDelta.x, makeMenu.CellSize.y);
        selectCursor.AddComponent<Image>().sprite = MenuTheme.myMenu.cursor;
        selectCursor.AddComponent<Cursor>();
        isDone = true;
    }

    wasd getwasd()
    {
        if (Input.GetKey("w"))
        {
            front = wasd.w;
        }
        else if (Input.GetKey("a"))
        {
            front = wasd.a;
        }
        else if (Input.GetKey("s"))
        {
            front = wasd.s;
        }
        else if(Input.GetKey("d"))
        {
            front = wasd.d;
        }
        if (Input.GetKey("w") && (m.verticalIsCycle ? (' ' == m.wmap[x, y - 1 >= 0 ? y - 1 : m.y - 1]) : (0 != y && ' ' == m.wmap[x, y - 1])))
        {
            m.wmap[x, y--] = ' ';
            if (y < 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, m.minY - m.widthY / m.y / 2.0f);
                y = m.y - 1;
            }
            m.wmap[x, y] = 'I';
            return wasd.w;
        }
        else if (Input.GetKey("a") && (m.horizontalIsCycle ? (' ' == m.wmap[x - 1 >= 0 ? x - 1 : m.x - 1, y]) : (0 != x && ' ' == m.wmap[x - 1, y])))
        {
            m.wmap[x--, y] = ' ';
            if (x < 0)
            {
                transform.position = new Vector3(m.maxX + m.heightX / m.x / 2.0f, transform.position.y, transform.position.z);
                x = m.x - 1;
            }
            m.wmap[x, y] = 'I';
            return wasd.a;
        }
        else if (Input.GetKey("s") && (m.verticalIsCycle ? (' ' == m.wmap[x, (y + 1) % m.y]):(y != m.y - 1 && ' ' == m.wmap[x, y + 1])))
        {
            m.wmap[x, y++] = ' ';
            if (y >= m.y)
            {
                transform.position = new Vector3(transform.position.x, high, m.maxY + m.widthY / m.y / 2.0f);
            }
            y %= m.y;
            m.wmap[x, y] = 'I';
            return wasd.s;
        }
        else if (Input.GetKey("d") && (m.horizontalIsCycle ? (' ' == m.wmap[(x + 1) % m.x, y]) : (x != m.x - 1 && ' ' == m.wmap[x + 1, y])))
        {
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
        front = i;
        if (0 > step)
        {
            canTurn = false;
            i = (wasd)((int)(i + 2) % 4);
            step = Mathf.Abs(step);
        }
        if (moveIsEnd)
        {
            moveIsEnd = false;
            canMove = false;
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
                if (null != (effectWalkSounds[(int)nowEffect] ?? defaultWalkSound))
                {
                    GetComponent<AudioSource>().PlayOneShot(effectWalkSounds[(int)nowEffect] ?? defaultWalkSound);
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

    public IEnumerator play(AudioClip sound, bool isWait)
    {
        if (null == sound)
        {
            yield break;
        }
        commandIsEnd = !isWait || false;
        GetComponent<AudioSource>().PlayOneShot(sound);
        yield return new WaitForSeconds(sound.length);
        commandIsEnd = true;
    }

    public static IEnumerator wait(float waitTime)
    {
        commandIsEnd = false;
        yield return new WaitForSeconds(waitTime);
        commandIsEnd = true;
    }

    public static IEnumerator tele(exitMode exitMode, enterMode enterMode, string worldName, int teleX, int teleY, float teleHigh, wasd front = wasd.s, AudioClip closeSound = null, AudioClip teleSound = null)
    {
        if (teleIsEnd)
        {
            teleIsEnd = false;
            canMove = false;
            canOpenMenu = false;
            you.enterMode = enterMode;
            if (null != teleSound)
            {
                you.teleSound = teleSound;
            }
            yield return new WaitForSeconds(null != teleSound ? teleSound.length : 0);
            switch (exitMode)
            {
                case exitMode.hide:
                    for (int i = 0; i < 50; i++)
                    {
                        teleScreen.GetComponent<Image>().color += new Color(0, 0, 0, 0.02f);
                        yield return 5;
                    }
                    break;
                default:
                    break;
            }
            if (worldName != SceneManager.GetActiveScene().name)
            {
                SceneManager.LoadScene(worldName);
            }
            you.teleX = teleX;
            you.teleY = teleY;
            you.teleHigh = teleHigh;
            you.front = front;
            isTele = true;
            canMove = true;
            canOpenMenu = true;
            you.closeSound = closeSound ?? you.closeSound;
            teleIsEnd = true;
            yield return new WaitForSeconds(0);
            
        }
        yield return null;
    }

    public static IEnumerator turn(wasd front)
    {
        commandIsEnd = false;
        you.front = front;
        commandIsEnd = true;
        yield return null;
    }

    public IEnumerator stop()
    {
        commandIsEnd = false;
        GetComponent<AudioSource>().Stop();
        commandIsEnd = true;
        yield return null;
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
        if (null != (effectWalkSounds[(int)nowEffect] ?? defaultWalkSound))
        {
            GetComponent<AudioSource>().PlayOneShot(effectWalkSounds[(int)nowEffect] ?? defaultWalkSound);
        }
        for (int j = 0; j < 20; j++)
        {
            switch (i)//移动
            {
                case wasd.w:
                    front = wasd.w;
                    transform.rotation = Quaternion.Euler(-90, 0, 180);
                    transform.position += new Vector3(0, 0, m.widthY / m.y / 20.0f);
                    yield return new WaitForSeconds(0.2f / speed / 20.0f);
                    break;
                case wasd.a:
                    front = wasd.a;
                    transform.rotation = Quaternion.Euler(-90, 0, 90);
                    transform.position += new Vector3(-m.heightX / m.x / 20.0f, 0, 0);
                    yield return new WaitForSeconds(0.2f / speed / 20.0f);
                    break;
                case wasd.s:
                    front = wasd.s;
                    transform.rotation = Quaternion.Euler(-90, 0, 0);
                    transform.position += new Vector3(0, 0, -m.widthY / m.y / 20.0f);
                    yield return new WaitForSeconds(0.2f / speed / 20.0f);
                    break;
                case wasd.d:
                    front = wasd.d;
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

    public IEnumerator changeEffect(effect effect)
    {
        commandIsEnd = false;
        canMove = false;
        canOpenMenu = false;
        npcMove.npcCanMove = false;
        if (effect.none != effect ? null != effectEqiupSound : null != effectCancelEqiupSound)
        {
            GetComponent<AudioSource>().PlayOneShot(effect.none != effect ? effectEqiupSound : effectCancelEqiupSound);
        }
        nowEffect = effect;
        GetComponent<MeshRenderer>().material = screens[change[Random.Range(0, change.Length)]];
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

    void Update()
    {
        if (isDone)
        {
            if (last_menuSelectsPositionAdjust != menuSelectsPositionAdjust || last_menuSelectsSpaceAdjust != menuSelectsSpaceAdjust)
            {
                last_menuSelectsPositionAdjust = menuSelectsPositionAdjust;
                last_menuSelectsSpaceAdjust = menuSelectsSpaceAdjust;
                for (int i = 0; i < selects.Length; i++)
                {
                    selects[i].text.GetComponent<RectTransform>().localPosition = new Vector3((selectMenu.GetComponent<RectTransform>().localPosition.x + menuSelectsPositionAdjust.x), (selectMenu.GetComponent<RectTransform>().localPosition.y + menuSelectsPositionAdjust.y) - (makeMenu.CellSize.y + menuSelectsSpaceAdjust) * (0.5f + i));
                    selects[i].text.GetComponent<RectTransform>().sizeDelta = new Vector2(selectMenu.GetComponent<RectTransform>().sizeDelta.x, makeMenu.CellSize.y);
                }
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
                switch (front)
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
            //测试效果
            if (Input.GetKeyDown("9") && canOpenMenu)
            {
                StartCoroutine(changeEffect(effect.angel));
            }
            if (Input.GetKeyDown("0") && canOpenMenu)
            {
                StartCoroutine(changeEffect(effect.none));
            }
            if (Input.GetKeyDown("x") && canOpenMenu)
            {
                StartCoroutine(openMenu());
            }
            if (isChangeEffect)
            {
                GetComponent<MeshFilter>().sharedMesh = effects[(int)nowEffect].GetComponent<MeshFilter>().sharedMesh ?? effects[0].GetComponent<MeshFilter>().sharedMesh;
                GetComponent<MeshRenderer>().sharedMaterials = effects[(int)nowEffect].GetComponent<MeshRenderer>().sharedMaterials ?? effects[0].GetComponent<MeshRenderer>().sharedMaterials;
                switch (nowEffect)
                {
                    case effect.angel:
                        speed = 2;
                        transform.position = new Vector3(transform.position.x, high + 2f, transform.position.z);
                        gameObject.GetComponent<Float>().enabled = true;
                        gameObject.GetComponent<changeColor>().enabled = true;
                        break;
                    default:
                        effectInit();
                        break;
                }
                isChangeEffect = false;
            }
            if (isEnd && canMove && !isOpenMenu)
            {
                StartCoroutine(pmove());
            }
        }

        IEnumerator openMenu()
        {
            if (!isOpenMenu)
            {
                GetComponent<AudioSource>().PlayOneShot(openMenuSound);
                canMove = false;
                npcMove.npcCanMove = false;
                canOpenMenu = false;
                for (int i = 0; i < 10; i++)
                {
                    blackScreen.GetComponent<Image>().color += new Color(0, 0, 0, 0.1f);
                    youMenu.GetComponent<makeMenu>().menuColor += new Color(0, 0, 0, 0.1f);
                    moneyMenu.GetComponent<makeMenu>().menuColor += new Color(0, 0, 0, 0.1f);
                    selectMenu.GetComponent<makeMenu>().menuColor += new Color(0, 0, 0, 0.1f);
                    yield return 1;
                }
                isOpenMenu = true;
                canOpenMenu = true;
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(closeMenuSound);
                canMove = true;
                npcMove.npcCanMove = true;
                canOpenMenu = false;
                for (int i = 0; i < 10; i++)
                {
                    blackScreen.GetComponent<Image>().color -= new Color(0, 0, 0, 0.1f);
                    youMenu.GetComponent<makeMenu>().menuColor -= new Color(0, 0, 0, 0.1f);
                    moneyMenu.GetComponent<makeMenu>().menuColor -= new Color(0, 0, 0, 0.1f);
                    selectMenu.GetComponent<makeMenu>().menuColor -= new Color(0, 0, 0, 0.1f);
                    yield return 1;
                }
                isOpenMenu = false;
                canOpenMenu = true;
            }
        }
    }
}
