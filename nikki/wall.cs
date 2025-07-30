using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class wall : MonoBehaviour
{
    public map m;
    public int x = 0;
    public int y = 0;
    public bool ChangeTransform = true;
    public GameObject wallPrefab;
    private GameObject wallReal;
    public int extendLeft = 0;
    public int extendRight = 0;
    public int extendUp = 0;
    public int extendDown = 0;
    public you.wasd front = you.wasd.s;
    void Start()
    {
        if (ChangeTransform)
        {
            //根据地图xyz轴进行transform计算
            transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), transform.position.y, m.maxY - m.widthY / m.y * (0.5f + y));
        }
        wallReal = wallPrefab ?? wallReal;
        if (null != wallReal && null != wallReal.GetComponent<wall>())
        {
            wallReal.GetComponent<wall>().enabled = false;
        }
        if (null == wallReal) {
            return;
        }
        Vector3[] clones = { new Vector3(-m.heightX, 0, m.widthY), new Vector3(0, 0, m.widthY), new Vector3(m.heightX, 0, m.widthY), new Vector3(-m.heightX, 0, 0), new Vector3(m.heightX, 0, 0), new Vector3(-m.heightX, 0, -m.widthY), new Vector3(0, 0, -m.widthY), new Vector3(m.heightX, 0, -m.widthY) };
        foreach (Vector3 v in clones)
        {
            if (!m.horizontalIsCycle && v.x != 0 || !m.verticalIsCycle && v.z != 0)
            {
                continue;
            }
            wallReal.transform.localPosition = v + transform.position;
            wallReal.transform.rotation = transform.rotation;
            Instantiate(wallReal, transform, true);
        }
    }

    void Update()
    {
        for (int i = x - extendLeft; i <= x + extendRight; i++)
        {
            for (int j = y - extendUp; j <= y + extendDown; j++)
            {
                m.wmap[i, j] = (' ' == m.wmap[i, j] ? 'X' : m.wmap[i ,j]);
            }
        }
    }

    void OnDestroy()
    {
        try
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            for (int i = x - extendLeft; i <= x + extendRight; i++)
            {
                for (int j = y - extendUp; j <= y + extendDown; j++)
                {
                    m.wmap[i, j] = ' ';
                }
            }
        }
        catch { }
    }
}