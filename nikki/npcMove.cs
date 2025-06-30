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
    private bool isEnd = true;
    public float moveTime = 0.01f;
    public float waitTime = 0.2f;
    public int x = 5;
    public int y = 5;
    private float high = 5;
    public GameObject clone;
    wasd getwasd()
    {
        wasd w = (wasd)Random.Range(0, 4);
        if (wasd.w == w && ' ' == map.wmap[x, y - 1 >= 0 ? y - 1 : map.y - 1])
        {
            map.wmap[x, y--] = ' ';
            if (y < 0)
            {
                transform.position = new Vector3(transform.position.x, high, map.minY - map.widthY / map.y / 2.0f);
                y = map.y - 1;
            }
            map.wmap[x, y] = 'N';
            return wasd.w;
        }
        else if (wasd.a == w && ' ' == map.wmap[x - 1 >= 0 ? x - 1 : map.x - 1, y])
        {
            map.wmap[x--, y] = ' ';
            if (x < 0)
            {
                transform.position = new Vector3(map.maxX + map.heightX / map.x / 2.0f, high, transform.position.z);
                x = map.x - 1;
            }
            map.wmap[x, y] = 'N';
            return wasd.a;
        }
        else if (wasd.s == w && ' ' == map.wmap[x, (y + 1) % map.y])
        {
            map.wmap[x, y++] = ' ';
            if (y >= map.y)
            {
                transform.position = new Vector3(transform.position.x, high, map.maxY + map.widthY / map.y / 2.0f);
            }
            y %= map.y;
            map.wmap[x, y] = 'N';
            return wasd.s;
        }
        else if (wasd.d == w && ' ' == map.wmap[(x + 1) % map.x, y])
        {
            map.wmap[x++, y] = ' ';
            if (x >= map.x)
            {
                transform.position = new Vector3(map.minX - map.heightX / map.x / 2.0f, high, transform.position.z);
            }
            x %= map.x;
            map.wmap[x, y] = 'N';
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
        for (int j = 0; j < 100; j++)
        {
            switch (i)//移动
            {
                case wasd.w:
                    transform.rotation = Quaternion.Euler(-90, 0, 180);
                    transform.position += new Vector3(0, 0, map.widthY / map.y / 100.0f);
                    yield return new WaitForSeconds(moveTime);
                    break;
                case wasd.a:
                    transform.rotation = Quaternion.Euler(-90, 0, 90);
                    transform.position += new Vector3(-map.heightX / map.x / 100.0f, 0, 0);
                    yield return new WaitForSeconds(moveTime);
                    break;
                case wasd.s:
                    transform.rotation = Quaternion.Euler(-90, 0, 0);
                    transform.position += new Vector3(0, 0, -map.widthY / map.y / 100.0f);
                    yield return new WaitForSeconds(moveTime);
                    break;
                case wasd.d:
                    transform.rotation = Quaternion.Euler(-90, 0, -90);
                    transform.position += new Vector3(map.heightX / map.x / 100.0f, 0, 0);
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
        transform.position = new Vector3(map.minX + map.heightX / map.x * (0.5f + x), high, map.maxY - map.widthY / map.y * (0.5f + y));
        map.wmap[x, y] = 'N';
        Vector3[] clones = { new Vector3(-map.heightX, 0, map.widthY), new Vector3(0, 0, map.widthY), new Vector3(map.heightX, 0, map.widthY), new Vector3(-map.heightX, 0, 0), new Vector3(map.heightX, 0, 0), new Vector3(-map.heightX, 0, -map.widthY), new Vector3(0, 0, -map.widthY), new Vector3(map.heightX, 0, -map.widthY) };
        foreach (Vector3 v in clones){
            clone.transform.localPosition = v + transform.position;
            clone.transform.rotation = transform.rotation;
            Instantiate(clone, transform, true);
        }
    }

    void Update()
    {
        if (isEnd)
        {
            StartCoroutine(pmove());
        }
    }
}
