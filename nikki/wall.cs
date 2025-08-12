using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class wall : MonoBehaviour
{
    public map m
    {
        get
        {
            return map.Map;
        }
    }
    public int x = 0;
    public int y = 0;
    public bool ChangeTransform = true;
    public GameObject wallPrefab;
    private GameObject wallReal;
    public int extendLeft = 0;
    public int extendRight = 0;
    public int extendUp = 0;
    public int extendDown = 0;
    public wasd face = wasd.s;
    private List<GameObject> addVirtualChildren = new List<GameObject>();
    public List<GameObject> AddVirtualChildren
    {
        get
        {
            return addVirtualChildren;
        }
    }
    void Start()
    {
        if (ChangeTransform)
        {
            //根据地图xyz轴进行transform计算
            transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), transform.position.y, m.maxY - m.widthY / m.y * (0.5f + y));
        }
        wallReal = wallPrefab ?? wallReal;
        if (null != wallReal && !wallReal.GetComponent<wall>().IsUnityNull() && wallReal.GetComponent<wall>().enabled)
        {
            wallReal.GetComponent<wall>().enabled = false;
        }
        if (null == wallReal) {
            return;
        }
        Vector3[] clones = { new Vector3(-m.heightX, 0, m.widthY), new Vector3(0, 0, m.widthY), new Vector3(m.heightX, 0, m.widthY), new Vector3(-m.heightX, 0, 0), new Vector3(m.heightX, 0, 0), new Vector3(-m.heightX, 0, -m.widthY), new Vector3(0, 0, -m.widthY), new Vector3(m.heightX, 0, -m.widthY) };
        GameObject cloneWall;
        foreach (Vector3 v in clones)
        {
            if (!m.horizontalIsCycle && v.x != 0 || !m.verticalIsCycle && v.z != 0)
            {
                continue;
            }
            if (null != wallReal && !wallReal.GetComponent<npcMove>().IsUnityNull() && wallReal.GetComponent<npcMove>().enabled)
            {
                wallReal.GetComponent<npcMove>().enabled = false;
            }
            if (null != wallReal && !wallReal.GetComponent<wall>().IsUnityNull() && wallReal.GetComponent<wall>().enabled)
            {
                wallReal.GetComponent<wall>().enabled = false;
            }
            wallReal.transform.localPosition = v + transform.position;
            wallReal.transform.rotation = transform.rotation;
            cloneWall = Instantiate(wallReal);
            ConstraintSource constraintSource = new ConstraintSource { weight = 1, sourceTransform = transform };
            cloneWall.AddComponent<PositionConstraint>().AddSource(constraintSource);
            cloneWall.GetComponent<PositionConstraint>().weight = 1;
            cloneWall.GetComponent<PositionConstraint>().translationAtRest = Vector3.zero;
            cloneWall.GetComponent<PositionConstraint>().translationOffset = v;
            cloneWall.GetComponent<PositionConstraint>().constraintActive = true;
            addVirtualChildren.Add(cloneWall);
            //pos const
            if (null != wallReal && !wallReal.GetComponent<npcMove>().IsUnityNull() && !wallReal.GetComponent<npcMove>().enabled)
            {
                wallReal.GetComponent<npcMove>().enabled = true;
            }
            if (null != wallReal && !wallReal.GetComponent<wall>().IsUnityNull() && !wallReal.GetComponent<wall>().enabled)
            {
                wallReal.GetComponent<wall>().enabled = true;
            }
        }
    }

    void Update()
    {
        for (int i = x - extendLeft; i <= x + extendRight; i++)
        {
            for (int j = y - extendUp; j <= y + extendDown; j++)
            {
                if (!m.verticalIsCycle && (i < 0 || i >= m.y) || !m.horizontalIsCycle && (j < 0 || j >= m.x))
                {
                    continue;
                }
                if (m.verticalIsCycle && i < 0)
                {
                    i = m.y - (i % m.y);
                }
                if (m.horizontalIsCycle && j < 0)
                {
                    j = m.x - (j % m.x);
                }
                m.wmap[i % m.x, j % m.y] = (' ' == m.wmap[i % m.x, j % m.y] ? 'X' : m.wmap[i % m.x, j % m.y]);
            }
        }
    }

    void OnDestroy()
    {
        try
        {
            for (int i = 0; i < addVirtualChildren.Count; i++)
            {
                Destroy(addVirtualChildren[i]);
            }
            addVirtualChildren.Clear();
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