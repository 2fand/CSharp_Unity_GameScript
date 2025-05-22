using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class color : MonoBehaviour
{
    // Start is called before the first frame update
    public enum colors
    {
        red,
        yellow,
        blue,
        orange,
        green,
        purple,
        cyan,
        pink,
        brown,
        white,
        grey,
        black
    }//color组件总共有的颜色
    public colors c;
    public bool IsUpdate;
    private Color[] cs
    {
        get
        {
            Dictionary<char, int> hexs = new Dictionary<char, int> { { '0', 0 }, { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 }, { '9', 9 }, { 'a', 10 }, { 'b', 11 }, { 'c', 12 }, { 'd', 13 }, { 'e', 14 }, { 'f', 15 }};
            string[] stringColors = { "ff2d2d", "ffdf0d", "169aff", "ff632c", "3ed92d", "d160fb", "8df0f9", "ff669c", "935806", "ffffff", "777777", "161616" };//枚举colors所代表的16进制颜色表
            Color[] colors = new Color[stringColors.Length];
            for (int i = 0; i < stringColors.Length; i++)
            {
                colors[i] = new Color((hexs[stringColors[i][0]] * 16 + hexs[stringColors[i][1]]) / 255.0f, (hexs[stringColors[i][2]] * 16 + hexs[stringColors[i][3]]) / 255.0f, (hexs[stringColors[i][4]] * 16 + hexs[stringColors[i][5]]) / 255.0f);
            }
            return colors;
        }
    }
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = cs[(int)c];
    }

    // Update is called once per frame
    void Update()
    {
        if (IsUpdate)
        {
            GetComponent<MeshRenderer>().material.color = cs[(int)c];
        }
    }
}
