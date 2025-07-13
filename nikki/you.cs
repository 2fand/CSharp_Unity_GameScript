using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static change;

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
    public readonly string[] effectName = { "", "天使", "锁门" };
    public static enterMode enterMode = enterMode.show;
    public static bool notOver = false;
    public static AudioClip defaultWalkSound;
    public Camera gameCamera;
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
                for (int j = 0; j < 20; j++)
                {
                    switch (i)//移动
                    {
                        case wasd.w:
                            front = wasd.w;
                            transform.position += new Vector3(0, 0, m.widthY / m.y / 20.0f);
                            yield return new WaitForSeconds(0.2f / speed / 20.0f);
                            break;
                        case wasd.a:
                            front = wasd.a;
                            transform.position += new Vector3(-m.heightX / m.x / 20.0f, 0, 0);
                            yield return new WaitForSeconds(0.2f / speed / 20.0f);
                            break;
                        case wasd.s:
                            front = wasd.s;
                            transform.position += new Vector3(0, 0, -m.widthY / m.y / 20.0f);
                            yield return new WaitForSeconds(0.2f / speed / 20.0f);
                            break;
                        case wasd.d:
                            front = wasd.d;
                            transform.position += new Vector3(m.heightX / m.x / 20.0f, 0, 0);
                            yield return new WaitForSeconds(0.2f / speed / 20.0f);
                            break;
                    }
                }
            }
            speed = youSpeed;
            moveIsEnd = true;
            canMove = true;
        }
    }

    public static IEnumerator tele(exitMode exitMode, enterMode enterMode, Image image, string worldName, int teleX, int teleY, float teleHigh, wasd front = wasd.s, AudioClip closeSound = null, AudioClip teleSound = null)
    {
        if (teleIsEnd)
        {
            teleIsEnd = false;
            canMove = false;
            you.enterMode = enterMode;
            if (null != teleSound)
            {
                you.teleSound = teleSound;
            }
            yield return new WaitForSeconds(null != teleSound ? teleSound.length : 0);
            if (null != image)
            {
                switch (exitMode)
                {
                    case exitMode.hide:
                        for (int i = 0; i < 50; i++)
                        {
                            image.color += new Color(0, 0, 0, 0.02f);
                            yield return 5;
                        }
                        break;
                    default:
                        break;
                }
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
            if (null != closeSound)
            {
                you.closeSound = closeSound;
            }
            teleIsEnd = true;
            yield return new WaitForSeconds(0);
            
        }
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

    IEnumerator changeEffect(effect effect)
    {
        canMove = false;
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
    }

    void Start()
    {
        if (null == GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }
        if (null == GetComponent<Float>())
        {
            gameObject.AddComponent<Float>();
        }
        if (null == GetComponent<changeColor>())
        {
            gameObject.AddComponent<changeColor>();
        }
        //根据游戏y轴进行高度计算
        high = transform.position.y;
        //根据地图xy轴进行位置计算
        transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), transform.position.y, m.maxY - m.widthY / m.y * (0.5f + y));
        GetComponent<MeshRenderer>().sharedMaterials = effects[0].GetComponent<MeshRenderer>().sharedMaterials;
        GetComponent<MeshFilter>().sharedMesh = effects[0].GetComponent<MeshFilter>().sharedMesh;
    }

    void Update()
    {
        if (isTele)
        {
            x = teleX; 
            y = teleY;
            transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), teleHigh, m.maxY - m.widthY / m.y * (0.5f + y));
            isTele = false;
        }
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
        if (Input.GetKeyDown("9") && canMove)
        {
            StartCoroutine(changeEffect(effect.angel));
        }
        if (Input.GetKeyDown("0") && canMove)
        {
            StartCoroutine(changeEffect(effect.none));
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
                    speed = 1;
                    transform.position = new Vector3(transform.position.x, high, transform.position.z);
                    gameObject.GetComponent<Float>().enabled = false;
                    gameObject.GetComponent<changeColor>().enabled = false;
                    break;
            }
            isChangeEffect = false;
        }
        if (isEnd && canMove)
        {
            StartCoroutine(pmove());
        }
    }
}
