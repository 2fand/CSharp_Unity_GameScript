using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map : MonoBehaviour
{
    public int x = 20;
    public int y = 20;
    public int a = 100;
    public int b = 100;
    public GameObject o;
    public int minX = -50;
    public int minY = -50;
    public int maxX = 50;
    public int maxY = 50;
    public bool horizontalIsCycle = true;
    public bool verticalIsCycle = true;
    public int heightX { 
        get
        {
            return Mathf.Abs(maxX - minX);
        }
    }
    public int widthY
    {
        get
        {
            return Mathf.Abs(maxY - minY);
        }
    }
    public char[,] wmap;
    void Awake()
    {
        wmap = new char[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                wmap[i, j] = ' ';
            }
        }
    }

    private void Start()
    {
        //xyÊÇ·ñÑ­»·
        for (int i = 0; i < 9 && (horizontalIsCycle || verticalIsCycle); i++)
        {
            if (4 == i || (!horizontalIsCycle && i % 3 != 1) || (!verticalIsCycle && i / 3 != 1))
            {
                continue;
            }
            Instantiate(o, new Vector3(transform.position.x + (i % 3 - 1) * a, 0, transform.position.z + (i / 3 - 1) * b), transform.rotation);
        }

    }
    void Update()
    {
        
    }
}
