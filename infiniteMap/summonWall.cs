using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class summonWall : MonoBehaviour
{
    public Vector2[] wallXY;
    public Vector2 xyOffSet = Vector2.zero;//ƫ���������Դ�������1*1�����ǽ��Ҳ���Զ����ɳ�����ǽ���������ƶ�
    public bool ChangeTransform = true;
    public wall wallPreFab;
    void Start() {
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

    void Update()
    {
        
    }
}
