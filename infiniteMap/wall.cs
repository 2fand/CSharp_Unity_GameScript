using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    public bool ChangeTransform = true;
    public GameObject wallPrefab;
    void Start()
    {
        if (ChangeTransform)
        {
            //根据地图xyz轴进行transform计算
            transform.position = new Vector3(map.minX + map.heightX / map.x * (0.5f + x), transform.position.y, map.maxY - map.widthY / map.y * (0.5f + y));
        }
        map.wmap[x, y] = 'X';
        if (null != wallPrefab)
        {
            Vector3[] clones = { new Vector3(-map.heightX, 0, map.widthY), new Vector3(0, 0, map.widthY), new Vector3(map.heightX, 0, map.widthY), new Vector3(-map.heightX, 0, 0), new Vector3(map.heightX, 0, 0), new Vector3(-map.heightX, 0, -map.widthY), new Vector3(0, 0, -map.widthY), new Vector3(map.heightX, 0, -map.widthY) };
            foreach (Vector3 v in clones)
            {
                wallPrefab.transform.localPosition = v + transform.position;
                wallPrefab.transform.rotation = transform.rotation;
                Instantiate(wallPrefab, transform, true);
            }
        }
    }

    void Update()
    {
        
    }
}
