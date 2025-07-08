using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineWall : MonoBehaviour
{
    public Vector2[] xystartends;
    public Vector2 xyOffSet = Vector2.zero;//偏移量，可以创建出比1*1还大的墙，也可以对生成出来的墙进行整体移动
    public bool ChangeTransform = true;
    public wall wallPreFab;
    void Start()
    {
        Vector2 xystart = Vector2.zero;
        Vector2 xyend = Vector2.zero;
        for (int j = 0; j < xystartends.Length; j += 2)
        {
            xystart = xystartends[j];
            xyend = xystartends[j + 1];
            Vector2[] wallXY = new Vector2[(int)(Mathf.Abs(xyend.x - xystart.x) > Mathf.Abs(xyend.y - xystart.y) ? (int)Mathf.Abs(xyend.x - xystart.x) : (int)Mathf.Abs(xyend.y - xystart.y)) + 1];
            Vector2 point = xystart;
            float x = (int)xystart.x;
            float y = (int)xystart.y;
            wallXY[0] = xystart;
            for (int i = 0; i < wallXY.Length - 1; i++)
            {
                if (Mathf.Abs(xyend.x - xystart.x) > Mathf.Abs(xyend.y - xystart.y))
                {
                    x += (int)(xyend.x - xystart.x) / (int)Mathf.Abs(xyend.x - xystart.x);
                    y += (xyend.y - xystart.y) / (wallXY.Length - 1);
                }
                else
                {
                    y += (int)(xyend.y - xystart.y) / (int)Mathf.Abs(xyend.y - xystart.y);
                    x += (xyend.x - xystart.x) / (wallXY.Length - 1);
                }
                point = new Vector2((int)x, (int)y);
                wallXY[i + 1] = point;
            }
            foreach (Vector2 xy in wallXY)
            {
                if (null == wallPreFab)
                {
                    GameObject emptyWall = new GameObject("EmptyWall");
                    wallPreFab = emptyWall.AddComponent<wall>();
                }
                wallPreFab.x = (int)xy.x + (int)xyOffSet.x;
                wallPreFab.y = (int)xy.y + (int)xyOffSet.y;
                wallPreFab.ChangeTransform = ChangeTransform;
                Instantiate(wallPreFab);
            }
        }
    }

    void Update()
    {

    }
}
