using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setPos : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    public GameObject ObjectPrefab;
    private map m;
    public string mpath;
    void Start()
    {
        m = GameObject.Find(mpath).GetComponent<map>();
        //根据地图xyz轴进行transform计算
        transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), transform.position.y, m.maxY - m.widthY / m.y * (0.5f + y));
        if (null == ObjectPrefab)
        {
            GameObject emptyObject = new GameObject("EmptyObject");
            ObjectPrefab = emptyObject;
        }
        Vector3[] clones = { new Vector3(-m.heightX, 0, m.widthY), new Vector3(0, 0, m.widthY), new Vector3(m.heightX, 0, m.widthY), new Vector3(-m.heightX, 0, 0), new Vector3(m.heightX, 0, 0), new Vector3(-m.heightX, 0, -m.widthY), new Vector3(0, 0, -m.widthY), new Vector3(m.heightX, 0, -m.widthY) };
        foreach (Vector3 v in clones)
        {
            if (!m.horizontalIsCycle && v.x != 0 || !m.verticalIsCycle && v.z != 0)
            {
                continue;
            }
            ObjectPrefab.transform.localPosition = v + transform.position;
            ObjectPrefab.transform.rotation = transform.rotation;
            Instantiate(ObjectPrefab, transform, true);
        }
    }

    void Update()
    {

    }
}
