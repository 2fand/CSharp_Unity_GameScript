using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class npcMove : MonoBehaviour
{
    public enum wasd
    {
        w,
        a,
        s,
        d,
        n
    };
    public enum move
    {
        random,
        Catch,
        escape
    }
    private map m;
    private bool isEnd = true;
    public float waitTime = 0.2f;
    public int speed = 2;
    public wall npc;
    public float high = 5;
    public bool isTurn = true;
    public move moveMode;
    public bool canOver = false;
    public AudioClip catchSound;
    public AudioClip hurtSound;
    public change.enterMode enterMode = change.enterMode.show;
    public change.exitMode exitMode = change.exitMode.hide;
    public you u;
    public Image image;
    public static bool npcCanMove = true;
    public string teleWorldName = "nexus";
    public int teleWorldX = 15;
    public int teleWorldY = 15;
    public int teleWorldHigh = 0;
    public wasd teleYouFront = wasd.s;
    int maxNum(int a, int b)
    {
        return a > b ? a : b;
    }
    wasd getwasd()
    {
        wasd w = (wasd)UnityEngine.Random.Range(0, 4);
        int[] compare = new int[4];
        wasd[] ways = new wasd[4];
        int i = 0;
        for (i = 0; i < 4; i++)
        {
            ways[i] = (wasd)i;
        }
        if (move.Catch == moveMode || move.escape == moveMode)
        {
            compare[0] = m.verticalIsCycle && u.y > npc.y ? m.y + npc.y - u.y : maxNum(npc.y - u.y, 0);
            compare[1] = m.horizontalIsCycle && u.x > npc.x ? m.x + npc.x - u.x : maxNum(npc.x - u.x, 0);
            compare[2] = m.verticalIsCycle && u.y < npc.y ? m.y - npc.y + u.y : maxNum(u.y - npc.y, 0);
            compare[3] = m.horizontalIsCycle && u.x < npc.x ? m.x - npc.x + u.x : maxNum(u.x - npc.x, 0);
            Array.Sort(compare, ways);
        }
        switch (moveMode){
            case move.Catch:
                for (i = 0; i < 4; i++)
                {
                    if (0 == compare[i])
                    {
                        continue;
                    }
                    switch (ways[i])
                    {
                        case wasd.w:
                            if (m.verticalIsCycle ? (' ' == m.wmap[npc.x, npc.y - 1 >= 0 ? npc.y - 1 : m.y - 1] || 'I' == m.wmap[npc.x, npc.y - 1 >= 0 ? npc.y - 1 : m.y - 1]) : (0 != npc.y && ' ' == m.wmap[npc.x, npc.y - 1] || 'I' == m.wmap[npc.x, npc.y - 1]))
                            {
                                goto end;
                            }
                            break;
                        case wasd.a:
                            if (m.horizontalIsCycle ? (' ' == m.wmap[npc.x - 1 >= 0 ? npc.x - 1 : m.x - 1, npc.y] || 'I' == m.wmap[npc.x - 1 >= 0 ? npc.x - 1 : m.x - 1, npc.y]) : (0 != npc.x && ' ' == m.wmap[npc.x - 1, npc.y] || 'I' == m.wmap[npc.x - 1, npc.y]))
                            {
                                goto end;
                            }
                            break;
                        case wasd.s:
                            if (m.verticalIsCycle ? (' ' == m.wmap[npc.x, (npc.y + 1) % m.y] || 'I' == m.wmap[npc.x, (npc.y + 1) % m.y]) : (npc.y != m.y - 1 && ' ' == m.wmap[npc.x, npc.y + 1] || 'I' == m.wmap[npc.x, npc.y + 1])) {
                                goto end;
                            }
                            break;
                        default:
                            if (m.horizontalIsCycle ? (' ' == m.wmap[(npc.x + 1) % m.x, npc.y] || 'I' == m.wmap[(npc.x + 1) % m.x, npc.y]) : (npc.x != m.x - 1 && ' ' == m.wmap[npc.x + 1, npc.y] || 'I' == m.wmap[npc.x + 1, npc.y])) {
                                goto end;
                            }
                            break;
                    }
                }
            end:
                if (i < 4)
                {
                    w = ways[i];
                }
                else
                {
                    w = (wasd)UnityEngine.Random.Range(0, 4);
                }
                break;
            case move.escape:
                
                for (i = 0; i < 4; i++)
                {
                    if (0 == compare[i])
                    {
                        continue;
                    }
                    switch ((wasd)((int)(ways[i] + 2) % 4))
                    {
                        case wasd.w:
                            if (m.verticalIsCycle ? (' ' == m.wmap[npc.x, npc.y - 1 >= 0 ? npc.y - 1 : m.y - 1] || 'I' == m.wmap[npc.x, npc.y - 1 >= 0 ? npc.y - 1 : m.y - 1]) : (0 != npc.y && ' ' == m.wmap[npc.x, npc.y - 1] || 'I' == m.wmap[npc.x, npc.y - 1]))
                            {
                                goto enda;
                            }
                            break;
                        case wasd.a:
                            if (m.horizontalIsCycle ? (' ' == m.wmap[npc.x - 1 >= 0 ? npc.x - 1 : m.x - 1, npc.y] || 'I' == m.wmap[npc.x - 1 >= 0 ? npc.x - 1 : m.x - 1, npc.y]) : (0 != npc.x && ' ' == m.wmap[npc.x - 1, npc.y] || 'I' == m.wmap[npc.x - 1, npc.y]))
                            {
                                goto enda;
                            }
                            break;
                        case wasd.s:
                            if (m.verticalIsCycle ? (' ' == m.wmap[npc.x, (npc.y + 1) % m.y] || 'I' == m.wmap[npc.x, (npc.y + 1) % m.y]) : (npc.y != m.y - 1 && ' ' == m.wmap[npc.x, npc.y + 1] || 'I' == m.wmap[npc.x, npc.y + 1]))
                            {
                                goto enda;
                            }
                            break;
                        default:
                            if (m.horizontalIsCycle ? (' ' == m.wmap[(npc.x + 1) % m.x, npc.y] || 'I' == m.wmap[(npc.x + 1) % m.x, npc.y]) : (npc.x != m.x - 1 && ' ' == m.wmap[npc.x + 1, npc.y] || 'I' == m.wmap[npc.x + 1, npc.y]))
                            {
                                goto enda;
                            }
                            break;
                    }
                }
            enda:
                if (i < 4)
                {
                    w = (wasd)((int)(ways[i] + 2) % 4);
                }
                else
                {
                    w = (wasd)UnityEngine.Random.Range(0, 4);
                }
                break;
            default:
                break;
        }
        if (wasd.w == w && (m.verticalIsCycle ? (' ' == m.wmap[npc.x, npc.y - 1 >= 0 ? npc.y - 1 : m.y - 1]) : (0 != npc.y && ' ' == m.wmap[npc.x, npc.y - 1])))
        {
            m.wmap[npc.x, npc.y--] = ' ';
            if (npc.y < 0)
            {
                transform.position = new Vector3(transform.position.x, high, m.minY - m.widthY / m.y / 2.0f);
                npc.y = m.y - 1;
            }
            m.wmap[npc.x, npc.y] = 'N';
            return wasd.w;
        }
        else if (wasd.a == w && (m.horizontalIsCycle ? (' ' == m.wmap[npc.x - 1 >= 0 ? npc.x - 1 : m.x - 1, npc.y]) : (0 != npc.x && ' ' == m.wmap[npc.x - 1, npc.y])))
        {
            m.wmap[npc.x--, npc.y] = ' ';
            if (npc.x < 0)
            {
                transform.position = new Vector3(m.maxX + m.heightX / m.x / 2.0f, high, transform.position.z);
                npc.x = m.x - 1;
            }
            m.wmap[npc.x, npc.y] = 'N';
            return wasd.a;
        }
        else if (wasd.s == w && (m.verticalIsCycle ? (' ' == m.wmap[npc.x, (npc.y + 1) % m.y]) : (npc.y != m.y - 1 && ' ' == m.wmap[npc.x, npc.y + 1])))
        {
            m.wmap[npc.x, npc.y++] = ' ';
            if (npc.y >= m.y)
            {
                transform.position = new Vector3(transform.position.x, high, m.maxY + m.widthY / m.y / 2.0f);
            }
            npc.y %= m.y;
            m.wmap[npc.x, npc.y] = 'N';
            return wasd.s;
        }
        else if (wasd.d == w && (m.horizontalIsCycle ? (' ' == m.wmap[(npc.x + 1) % m.x, npc.y]) : (npc.x != m.x - 1 && ' ' == m.wmap[npc.x + 1, npc.y])))
        {
            m.wmap[npc.x++, npc.y] = ' ';
            if (npc.x >= m.x)
            {
                transform.position = new Vector3(m.minX - m.heightX / m.x / 2.0f, high, transform.position.z);
            }
            npc.x %= m.x;
            m.wmap[npc.x, npc.y] = 'N';
            return wasd.d;
        }
        else if (canOver && !you.notOver && ((wasd.w == w && (m.verticalIsCycle ? ('I' == m.wmap[npc.x, npc.y - 1 >= 0 ? npc.y - 1 : m.y - 1]) : (0 != npc.y && 'I' == m.wmap[npc.x, npc.y - 1]))) || (wasd.a == w && (m.horizontalIsCycle ? ('I' == m.wmap[npc.x - 1 >= 0 ? npc.x - 1 : m.x - 1, npc.y]) : (0 != npc.x && 'I' == m.wmap[npc.x - 1, npc.y]))) || (wasd.s == w && (m.verticalIsCycle ? ('I' == m.wmap[npc.x, (npc.y + 1) % m.y]) : (npc.y != m.y - 1 && 'I' == m.wmap[npc.x, npc.y + 1]))) || (wasd.d == w && (m.horizontalIsCycle ? ('I' == m.wmap[(npc.x + 1) % m.x, npc.y]) : (npc.x != m.x - 1 && 'I' == m.wmap[npc.x + 1, npc.y]))))) {
            StartCoroutine(hurt());
        }
        return wasd.n;
    }

    IEnumerator hurt()
    {
        if (null != hurtSound)
        {
            for (int j = 0; null != hurtSound && j < 5; j++)
            {
                GetComponent<AudioSource>().PlayOneShot(hurtSound);
                yield return 1;
            }
            hurtSound = null;
        }
        StartCoroutine(you.tele(exitMode, enterMode, image, teleWorldName, teleWorldX, teleWorldY, teleWorldHigh, you.wasd.s, null, catchSound));
    }

    IEnumerator pyou()
    {
        //初始
        isEnd = false;
        wasd i = getwasd();
        for (int j = 0; j < 20; j++)
        {
            switch (i)//移动
            {
                case wasd.w:
                    if (isTurn)
                    {
                        npc.front = you.wasd.w;
                    }
                    transform.position += new Vector3(0, 0, m.widthY / m.y / 20.0f);
                    yield return new WaitForSeconds(0.2f / speed / 20.0f);
                    break;
                case wasd.a:
                    if (isTurn)
                    {
                        npc.front = you.wasd.a;
                    }
                    transform.position += new Vector3(-m.heightX / m.x / 20.0f, 0, 0);
                    yield return new WaitForSeconds(0.2f / speed / 20.0f);
                    break;
                case wasd.s:
                    if (isTurn)
                    {
                        npc.front = you.wasd.s;
                    }
                    transform.position += new Vector3(0, 0, -m.widthY / m.y / 20.0f);
                    yield return new WaitForSeconds(0.2f / speed / 20.0f);
                    break;
                case wasd.d:
                    if (isTurn)
                    {
                        npc.front = you.wasd.d;
                    }
                    transform.position += new Vector3(m.heightX / m.x / 20.0f, 0, 0);
                    yield return new WaitForSeconds(0.2f / speed / 20.0f);//移动间隔时间
                    break;
                default://wasd.n时无
                    goto nowait;
            }
        }
        yield return new WaitForSeconds(waitTime);//移动等待时间
    nowait:
        isEnd = true;
        yield return null;
    }

    void Awake()
    {
        //根据地图z轴进行高度计算
        high = transform.position.y;
    }

    void Start()
    {
        m = npc.m;
        if (null == GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }
        m.wmap[npc.x, npc.y] = 'N';
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, high, transform.position.z);
        switch (npc.front)
        {
            case you.wasd.w:
                transform.rotation = Quaternion.Euler(-90, 0, 180);
                break;
            case you.wasd.a:
                transform.rotation = Quaternion.Euler(-90, 0, 90);
                break;
            case you.wasd.s:
                transform.rotation = Quaternion.Euler(-90, 0, 0);
                break;
            default:
                transform.rotation = Quaternion.Euler(-90, 0, -90);
                break;
        }
        if (isEnd && npcCanMove)
        {
            StartCoroutine(pyou());
        }
    }
}
