using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class summonWall : MonoBehaviour
{
    public string wallXYstr;
    public Vector2Int[] wallXY;
    public Vector2Int xyOffSet = Vector2Int.zero;//偏移量，可以创建出比1*1还大的墙，也可以对生成出来的墙进行整体移动
    public bool ChangeTransform = true;
    public GameObject wallPrefab;
    private GameObject wall;
    public int extendLeft = 0;
    public int extendRight = 0;
    public int extendUp = 0;
    public int extendDown = 0;
    void Start() {
        wall = Instantiate(wallPrefab);
        if (null == wall.GetComponent<wall>())
        {
            wall.AddComponent<wall>();
        }
        wall.GetComponent<wall>().enabled = true;
        if (0 == wallXY.Length)
        {
            int begin = 0;
            int end = 0;
            int x = -1;
            int y = -1;
            char status = 'x';
            List<Vector2Int> tempList = new List<Vector2Int>();
            if (0 == wallXYstr.Length)
            {
                return;
            }
            if (map.itemDelimiter != wallXYstr[wallXYstr.Length - 1])
            {
                wallXYstr += map.itemDelimiter;
            }
            for (int i = 0; i < wallXYstr.Length; i++)
            {
                if (map.itemDelimiter == wallXYstr[i] || map.xyDelimiter == wallXYstr[i])
                {
                    end = i;
                    if ('x' == status)
                    {
                        if (!int.TryParse(wallXYstr.Substring(begin, end - begin), out x))
                        {
                            break;
                        }
                        begin = end + 1;
                        status = 'y';
                    }
                    else
                    {
                        if (!int.TryParse(wallXYstr.Substring(begin, end - begin), out y))
                        {
                            break;
                        }
                        begin = end + 1;
                        tempList.Add(new Vector2Int(x, y));
                        status = 'x';
                    }
                }
            }
            wallXY = new Vector2Int[tempList.Count];
            tempList.CopyTo(wallXY);
        }
        if (wallXY.Length == 0)
        {
            return;
        }
        foreach (Vector2 xy in wallXY)
        {
            if (null == wall)
            {
                GameObject emptyWall = new GameObject("EmptyWall");
                emptyWall.AddComponent<wall>();
                wall = emptyWall;
            }
            wall.GetComponent<wall>().x = (int)xy.x + (int)xyOffSet.x;
            wall.GetComponent<wall>().y = (int)xy.y + (int)xyOffSet.y;
            wall.GetComponent<wall>().ChangeTransform = ChangeTransform;
            wall.GetComponent<wall>().extendUp = extendUp;
            wall.GetComponent<wall>().extendLeft = extendLeft;
            wall.GetComponent<wall>().extendDown = extendDown;
            wall.GetComponent<wall>().extendRight = extendRight;
            wall.GetComponent<wall>().wallPrefab = wallPrefab;
            Instantiate(wall);
        }
    }

    void Update()
    {
        
    }
}
