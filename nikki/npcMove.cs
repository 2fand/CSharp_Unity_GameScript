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
    public float moveTime = 0.01f;
    public float waitTime = 0.2f;
    public int speed = 2;
    public int x = 5;
    public int y = 5;
    private float high = 5;
    public GameObject clone;
    wasd getwasd()
    {
        wasd w = (wasd)Random.Range(0, 4);
        if (wasd.w == w && (m.verticalIsCycle ? (' ' == m.wmap[x, y - 1 >= 0 ? y - 1 : m.y - 1]) : (0 != y && ' ' == m.wmap[x, y - 1])))
        {
            m.wmap[x, y--] = ' ';
            if (y < 0)
            {
                transform.position = new Vector3(transform.position.x, high, m.minY - m.widthY / m.y / 2.0f);
                y = m.y - 1;
            }
            m.wmap[x, y] = 'N';
            return wasd.w;
        }
        else if (wasd.a == w && (m.horizontalIsCycle ? (' ' == m.wmap[x - 1 >= 0 ? x - 1 : m.x - 1, y]) : (0 != x && ' ' == m.wmap[x - 1, y])))
        {
            m.wmap[x--, y] = ' ';
            if (x < 0)
            {
                transform.position = new Vector3(m.maxX + m.heightX / m.x / 2.0f, high, transform.position.z);
                x = m.x - 1;
            }
            m.wmap[x, y] = 'N';
            return wasd.a;
        }
        else if (wasd.s == w && (m.verticalIsCycle ? (' ' == m.wmap[x, (y + 1) % m.y]) : (y != m.y - 1 && ' ' == m.wmap[x, y + 1])))
        {
            m.wmap[x, y++] = ' ';
            if (y >= m.y)
            {
                transform.position = new Vector3(transform.position.x, high, m.maxY + m.widthY / m.y / 2.0f);
            }
            y %= m.y;
            m.wmap[x, y] = 'N';
            return wasd.s;
        }
        else if (wasd.d == w && (m.horizontalIsCycle ? (' ' == m.wmap[(x + 1) % m.x, y]) : (x != m.x - 1 && ' ' == m.wmap[x + 1, y])))
        {
            m.wmap[x++, y] = ' ';
            if (x >= m.x)
            {
                transform.position = new Vector3(m.minX - m.heightX / m.x / 2.0f, high, transform.position.z);
            }
            x %= m.x;
            m.wmap[x, y] = 'N';
            return wasd.d;
        }
        else
        {
            return wasd.n;
        }
    }

    IEnumerator pmove()
    {
        //��ʼ
        isEnd = false;
        wasd i = getwasd();
        for (int j = 0; j < 100 / speed; j++)
        {
            switch (i)//�ƶ�
            {
                case wasd.w:
                    transform.rotation = Quaternion.Euler(-90, 0, 180);
                    transform.position += new Vector3(0, 0, m.widthY / m.y / (100.0f / speed));
                    yield return new WaitForSeconds(moveTime);
                    break;
                case wasd.a:
                    transform.rotation = Quaternion.Euler(-90, 0, 90);
                    transform.position += new Vector3(-m.heightX / m.x / (100.0f / speed), 0, 0);
                    yield return new WaitForSeconds(moveTime);
                    break;
                case wasd.s:
                    transform.rotation = Quaternion.Euler(-90, 0, 0);
                    transform.position += new Vector3(0, 0, -m.widthY / m.y / (100.0f / speed));
                    yield return new WaitForSeconds(moveTime);
                    break;
                case wasd.d:
                    transform.rotation = Quaternion.Euler(-90, 0, -90);
                    transform.position += new Vector3(m.heightX / m.x / (100.0f / speed), 0, 0);
                    yield return new WaitForSeconds(moveTime);//�ƶ����ʱ��
                    break;
                default://wasd.nʱ��
                    goto nowait;
            }
        }
        yield return new WaitForSeconds(waitTime);//�ƶ��ȴ�ʱ��
    nowait:
        isEnd = true;
        yield return null;
    }

    void Start()
    {
        //���ݵ�ͼz����и߶ȼ���
        high = transform.position.y;
        //���ݵ�ͼxy�����λ�ü���
        transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), high, m.maxY - m.widthY / m.y * (0.5f + y));
        m.wmap[x, y] = 'N';
        Vector3[] clones = { new Vector3(-m.heightX, 0, m.widthY), new Vector3(0, 0, m.widthY), new Vector3(m.heightX, 0, m.widthY), new Vector3(-m.heightX, 0, 0), new Vector3(m.heightX, 0, 0), new Vector3(-m.heightX, 0, -m.widthY), new Vector3(0, 0, -m.widthY), new Vector3(m.heightX, 0, -m.widthY) };
        foreach (Vector3 v in clones){
            if (!m.horizontalIsCycle && v.x != 0 || !m.verticalIsCycle && v.y != 0){
                continue;
            }
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
