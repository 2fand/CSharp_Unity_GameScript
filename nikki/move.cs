using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
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
    public float moveTime = 0.01f;
    public float waitTime = 0.2f;
    public int speed = 2;
    public int x = 5;
    public int y = 5;
    public static wasd front;
    public map m;
    private float high = 5;
    public static bool isTele = false;
    //public static int lastteleX = -1;
    //public static int lastteleY = -1;
    public static int teleX = 0;
    public static int teleY = 0;
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
                transform.position = new Vector3(transform.position.x, high, m.minY - m.widthY / m.y / 2.0f);
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
                transform.position = new Vector3(m.maxX + m.heightX / m.x / 2.0f, high, transform.position.z);
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
                transform.position = new Vector3(m.minX - m.heightX / m.x / 2.0f, high, transform.position.z);
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

    IEnumerator pmove()
    {
        //初始
        isEnd = false;
        wasd i = getwasd();
        for (int j = 0; j < 100 / speed; j++)
        {
            switch (i)//移动
            {
                case wasd.w:
                    front = wasd.w;
                    transform.position += new Vector3(0, 0, m.widthY / m.y / (100.0f / speed));
                    yield return new WaitForSeconds(moveTime);
                    break;
                case wasd.a:
                    front = wasd.a;
                    transform.position += new Vector3(-m.heightX / m.x / (100.0f / speed), 0, 0);
                    yield return new WaitForSeconds(moveTime);
                    break;
                case wasd.s:
                    front = wasd.s;
                    transform.position += new Vector3(0, 0, -m.widthY / m.y / (100.0f / speed));
                    yield return new WaitForSeconds(moveTime);
                    break;
                case wasd.d:
                    front = wasd.d;
                    transform.position += new Vector3(m.heightX / m.x / (100.0f / speed), 0, 0);
                    yield return new WaitForSeconds(moveTime);//移动间隔时间
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

    void Start()
    {
        //根据地图z轴进行高度计算
        high = transform.position.y;
        //根据地图xy轴进行位置计算
        transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), high, m.maxY - m.widthY / m.y * (0.5f + y));
    }

    void Update()
    {
        if (isTele)
        {
            x = teleX; 
            y = teleY;
            transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), high, m.maxY - m.widthY / m.y * (0.5f + y));
            isTele = false;
        }
        m.wmap[x, y] = 'I';
        if (isEnd)
        {
            StartCoroutine(pmove());
        }
    }
}
