using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(wall))]
public class npcMove : MonoBehaviour
{
    public enum move
    {
        random,
        Catch,
        escape
    }
    private map m
    {
        get
        {
            return map.Map;
        }
    }
    private bool isEnd = true;
    public float waitTime = 0.2f;
    public float speed = 2;
    public wall npc;
    public move moveMode = move.random;
    public bool canOver = false;
    public AudioClip catchSound;
    public AudioClip hurtSound;
    public change.transitionMode enterMode = change.transitionMode.show;
    public float enterTime = Game.enterTime;
    public change.transitionMode exitMode = change.transitionMode.hide;
    public float exitTime = Game.exitTime;
    public you u;
    public static bool npcCanMove = true;
    public string teleWorldName = "nexus";
    public int teleWorldX = 15;
    public int teleWorldY = 15;
    public float teleWorldHigh = 0;
    public wasd teleYouFront = wasd.s;
    public bool canTurn = true;
    public static bool stopAnimation = false;
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
                transform.position = new Vector3(transform.position.x, transform.position.y, m.minY - m.widthY / m.y / 2.0f);
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
                transform.position = new Vector3(m.maxX + m.heightX / m.x / 2.0f, transform.position.y, transform.position.z);
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
                transform.position = new Vector3(transform.position.x, transform.position.y, m.maxY + m.widthY / m.y / 2.0f);
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
                transform.position = new Vector3(m.minX - m.heightX / m.x / 2.0f, transform.position.y, transform.position.z);
            }
            npc.x %= m.x;
            m.wmap[npc.x, npc.y] = 'N';
            return wasd.d;
        }
        else if (canOver && !you.notOver && ((wasd.w == w && (m.verticalIsCycle ? ('I' == m.wmap[npc.x, npc.y - 1 >= 0 ? npc.y - 1 : m.y - 1]) : (0 != npc.y && 'I' == m.wmap[npc.x, npc.y - 1]))) || (wasd.a == w && (m.horizontalIsCycle ? ('I' == m.wmap[npc.x - 1 >= 0 ? npc.x - 1 : m.x - 1, npc.y]) : (0 != npc.x && 'I' == m.wmap[npc.x - 1, npc.y]))) || (wasd.s == w && (m.verticalIsCycle ? ('I' == m.wmap[npc.x, (npc.y + 1) % m.y]) : (npc.y != m.y - 1 && 'I' == m.wmap[npc.x, npc.y + 1]))) || (wasd.d == w && (m.horizontalIsCycle ? ('I' == m.wmap[(npc.x + 1) % m.x, npc.y]) : (npc.x != m.x - 1 && 'I' == m.wmap[npc.x + 1, npc.y]))))) {
            canTurn = false;
            turn(w);
            StartCoroutine(you.tele(exitMode, exitTime, enterMode, enterTime, teleWorldName, teleWorldX, teleWorldY, teleWorldHigh, wasd.s, catchSound, hurtSound));
        }
        return wasd.u;
    }

    IEnumerator pyou()
    {
        //初始
        isEnd = false;
        wasd i = getwasd();
        for (int j = 0; j < 10; j++)
        {
            switch (i)//移动
            {
                case wasd.w:
                    if (canTurn)
                    {
                        npc.face = wasd.w;
                    }
                    transform.position += new Vector3(0, 0, m.widthY / m.y / 10.0f);
                    yield return new WaitForSeconds(0.1f / speed / 10.0f);
                    yield return new WaitUntil(() => 0 == you.Menus.Count);
                    break;
                case wasd.a:
                    if (canTurn)
                    {
                        npc.face = wasd.a;
                    }
                    transform.position += new Vector3(-m.heightX / m.x / 10.0f, 0, 0);
                    yield return new WaitForSeconds(0.1f / speed / 10.0f);
                    yield return new WaitUntil(() => 0 == you.Menus.Count);
                    break;
                case wasd.s:
                    if (canTurn)
                    {
                        npc.face = wasd.s;
                    }
                    transform.position += new Vector3(0, 0, -m.widthY / m.y / 10.0f);
                    yield return new WaitForSeconds(0.1f / speed / 10.0f);
                    yield return new WaitUntil(() => 0 == you.Menus.Count);
                    break;
                case wasd.d:
                    if (canTurn)
                    {
                        npc.face = wasd.d;
                    }
                    transform.position += new Vector3(m.heightX / m.x / 10.0f, 0, 0);
                    yield return new WaitForSeconds(0.1f / speed / 10.0f);//移动间隔时间
                    yield return new WaitUntil(() => 0 == you.Menus.Count);
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
        npc = GetComponent<wall>();
    }

    void Start()
    {
        u = you.You;
        if (null == GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }
        m.wmap[npc.x, npc.y] = 'N';
    }

    void turn(wasd face)
    {
        Quaternion rotation = Quaternion.identity;
        switch (face)
        {
            case wasd.w:
                rotation = transform.rotation = Quaternion.Euler(-90, 0, 180);
                break;
            case wasd.a:
                rotation = transform.rotation = Quaternion.Euler(-90, 0, 90);
                break;
            case wasd.s:
                rotation = transform.rotation = Quaternion.Euler(-90, 0, 0);
                break;
            case wasd.d:
                rotation = transform.rotation = Quaternion.Euler(-90, 0, -90);
                break;
            default:
                break;
        }
        for (int i = 0; i < npc.AddVirtualChildren.Count; i++)
        {
            npc.AddVirtualChildren[i].transform.rotation = rotation;
        }
    }

    void Update()
    {
        if (canTurn)
        {
            turn(npc.face);
        }
        if (!GetComponent<wall>().enabled)
        {
            GetComponent<wall>().enabled = true;
        }
        if (canTurn)
        {
            if ((m.horizontalIsCycle ? (you.You.x + 1) % m.x : you.You.x + 1) == GetComponent<wall>().x && you.You.y == GetComponent<wall>().y && you.face == wasd.d && Input.GetKeyDown("z"))
            {
                npc.face = wasd.a;
            }
            else if ((you.You.x - 1 < 0 && m.horizontalIsCycle ? m.x - 1 : you.You.x - 1) == GetComponent<wall>().x && you.You.y == GetComponent<wall>().y && you.face == wasd.a && Input.GetKeyDown("z"))
            {
                npc.face = wasd.d;
            }
            else if (you.You.x == GetComponent<wall>().x && (m.verticalIsCycle ? (you.You.y + 1) % m.y : you.You.y + 1) == GetComponent<wall>().y && you.face == wasd.s && Input.GetKeyDown("z"))
            {
                npc.face = wasd.w;
            }
            else if (you.You.x == GetComponent<wall>().x && (you.You.y - 1 < 0 && m.verticalIsCycle ? m.y - 1 : you.You.y - 1) == GetComponent<wall>().y && you.face == wasd.w && Input.GetKeyDown("z"))
            {
                npc.face = wasd.s;
            }
        }
        if (isEnd && npcCanMove)
        {
            StartCoroutine(pyou());
        }
        if (canOver && !you.notOver && ' ' != you.HitObject && ((wasd.s == you.face && (m.verticalIsCycle ? (you.You.x == npc.x && you.You.y == (npc.y - 1 >= 0 ? npc.y - 1 : m.y - 1)) : (0 != npc.y && you.You.x == npc.x && you.You.y == npc.y - 1))) || (wasd.d == you.face && (m.horizontalIsCycle ? (you.You.x == (npc.x - 1 >= 0 ? npc.x - 1 : m.x - 1) && npc.y == you.You.y) : (0 != npc.x && you.You.x == npc.x - 1 && you.You.y == npc.y))) || (wasd.w == you.face && (m.verticalIsCycle ? (you.You.x == npc.x && you.You.y == (npc.y + 1) % m.y) : (npc.y != m.y - 1 && you.You.x == npc.x && you.You.y == npc.y + 1))) || (wasd.a == you.face && (m.horizontalIsCycle ? (you.You.x == (npc.x + 1) % m.x && you.You.y == npc.y) : (npc.x != m.x - 1 && you.You.x == npc.x + 1 && you.You.y == npc.y)))))
        {
            canTurn = false;
            wasd face = wasd.w;
            if (m.verticalIsCycle ? (you.You.x == npc.x && you.You.y == (npc.y - 1 >= 0 ? npc.y - 1 : m.y - 1)) : (0 != npc.y && you.You.x == npc.x && you.You.y == npc.y - 1)){
                face = wasd.s;
            }
            else if (m.horizontalIsCycle ? (you.You.x == (npc.x - 1 >= 0 ? npc.x - 1 : m.x - 1) && npc.y == you.You.y) : (0 != npc.x && you.You.x == npc.x - 1 && you.You.y == npc.y))
            {
                face = wasd.d;
            }
            else if (m.verticalIsCycle ? (you.You.x == npc.x && you.You.y == (npc.y + 1) % m.y) : (npc.y != m.y - 1 && you.You.x == npc.x && you.You.y == npc.y + 1))
            {
                face = wasd.w;
            }
            else
            {
                face = wasd.a;
            }
            turn(face);
            StartCoroutine(you.tele(exitMode, exitTime, enterMode, enterTime, teleWorldName, teleWorldX, teleWorldY, teleWorldHigh, wasd.s, catchSound, hurtSound));
        }
        if (null != GetComponent<Animation>())
        {
            GetComponent<Animation>().enabled = !stopAnimation;
        }
        if (null != GetComponent<Animator>())
        {
            GetComponent<Animator>().enabled = !stopAnimation;
        }
    }
}
