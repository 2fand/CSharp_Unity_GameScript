using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using TMPro;
using System.Net;
using System.Net.Sockets;

public class randomSymbolSummon1 : MonoBehaviour
{
    public float waitTime = 0.1f;
    private bool isEnd = true;
    private char[] chars;
    IEnumerator write()
    {
        isEnd = false;
        GetComponent<TextMeshPro>().text = chars[Random.Range(0, chars.Length)].ToString();
        yield return new WaitForSeconds(waitTime);
        isEnd = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (null == GetComponent<TextMeshPro>())
        {
            Debug.LogError("错误：没有TextMeshPro组件");
        }
        chars = new char[127 - 33];
        char ch = (char)33;
        int offset = 0;
        for (; ch < 127; ch++)
        {
            if ('~' == ch || '{' == ch)
            {
                offset--;
                continue;
            }
            chars[ch - 33] = ch;
        }
        write();
    }
    void Update()
    {
        if (isEnd)
        {
            StartCoroutine(write());
        }
    }
}
