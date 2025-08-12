using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class map : MonoBehaviour
{
    public int x = 20;
    public int y = 20;
    private float a;
    private float b;
    public GameObject o;
    public float minX = -50;
    public float minY = -50;
    public float maxX = 50;
    public float maxY = 50;
    public bool horizontalIsCycle = true;
    public bool verticalIsCycle = true;
    public const char xyDelimiter = ',';
    public const char itemDelimiter = ' ';
    public AudioClip worldMusic;
    public Sprite backGround;
    public Vector2 backGroundMoveVector;
    public float heightX { 
        get
        {
            return Mathf.Abs(maxX - minX);
        }
    }
    public float widthY
    {
        get
        {
            return Mathf.Abs(maxY - minY);
        }
    }
    public char[,] wmap;
    private static map _map;
    public static map Map
    {
        get { return _map; }
    }

    private void OnEnable()
    {
        if (null == _map || _map.IsUnityNull())
        {
            _map = this;
        }
    }
    void Awake()
    {
        a = heightX;
        b = widthY;
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
        if (null != worldMusic)
        {
            if (null == GetComponent<AudioSource>())
            {
                gameObject.AddComponent<AudioSource>();
            }
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().clip = worldMusic;
            GetComponent<AudioSource>().Play();
        }
        //xy «∑Ò—≠ª∑
        for (int i = 0; null != o && i < 9 && (horizontalIsCycle || verticalIsCycle); i++)
        {
            if (4 == i || (!horizontalIsCycle && i % 3 != 1) || (!verticalIsCycle && i / 3 != 1))
            {
                continue;
            }
            Instantiate(o, new Vector3(transform.position.x + (i % 3 - 1) * a, transform.position.y, transform.position.z + (i / 3 - 1) * b), transform.rotation);
        }
    }
    void Update()
    {
        
    }
}
