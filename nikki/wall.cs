using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    public bool ChangeTransform = true;
    public GameObject wallPrefab;
    private map m;
    public string mpath;
    void Start()
    {
        m = GameObject.Find(mpath).GetComponent<map>();
        if (ChangeTransform)
        {
            //���ݵ�ͼxyz�����transform����
            transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), transform.position.y, m.maxY - m.widthY / m.y * (0.5f + y));
        }
        m.GetComponent<map>().wmap[x, y] = 'X';
        if (null == wallPrefab){
            GameObject emptyWall = new GameObject("EmptyWall");
            wallPrefab = emptyWall;
        }
        Vector3[] clones = { new Vector3(-m.heightX, 0, m.widthY), new Vector3(0, 0, m.widthY), new Vector3(m.heightX, 0, m.widthY), new Vector3(-m.heightX, 0, 0), new Vector3(m.heightX, 0, 0), new Vector3(-m.heightX, 0, -m.widthY), new Vector3(0, 0, -m.widthY), new Vector3(m.heightX, 0, -m.widthY) };
        foreach (Vector3 v in clones)
        {
            if (!m.horizontalIsCycle && v.x != 0 || !m.verticalIsCycle && v.z != 0)
            {
                continue;
            }
            wallPrefab.transform.localPosition = v + transform.position;
            wallPrefab.transform.rotation = transform.rotation;
            Instantiate(wallPrefab, transform, true);
        }
    }

    void Update()
    {
        
    }
}
