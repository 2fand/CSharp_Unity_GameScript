using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map : MonoBehaviour
{
    public static int x = 20;
    public static int y = 20;
    public int a = 100;
    public int b = 100;
    public GameObject o;
    public static int minX = -50;
    public static int minY = -50;
    public static int maxX = 50;
    public static int maxY = 50;
    public static int heightX { 
        get
        {
            return Mathf.Abs(maxX - minX);
        }
    }
    public static int widthY
    {
        get
        {
            return Mathf.Abs(maxY - minY);
        }
    }
    public static char[,] wmap;
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
        for (int i = 0; i < 9; i++)
        {
            if (4 == i)
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
