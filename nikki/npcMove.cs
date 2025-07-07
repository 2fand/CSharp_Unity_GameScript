using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcMove : MonoBehaviour
{
    enum wasd
    {
        w,
        a,
        s,
        d,
        n
    };
    public map m;
    private bool isEnd = true;
    public float waitTime = 0.2f;
    public int speed = 2;
    public wall npc;
    public float high = 5;
    public bool isTurn = true;
    wasd getwasd()
    {
        wasd w = (wasd)Random.Range(0, 4);
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
        else
        {
            return wasd.n;
        }
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

    private void Start()
    {
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
        if (isEnd)
        {
            StartCoroutine(pyou());
        }
    }
}
