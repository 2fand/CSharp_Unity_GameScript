using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class pictureAnimation : MonoBehaviour
{
    public Sprite[] pictures;
    int p = 0;
    float waitTime = 0.1f;
    bool isEnd = true;
    IEnumerator showPicture()
    {
        isEnd = false;
        GetComponent<Image>().sprite = you.myMenu.cursorAnimation[p++];
        p %= you.myMenu.cursorAnimation.Length;
        yield return new WaitForSeconds(waitTime);
        isEnd = true;
    }

    void Start()
    {
        StartCoroutine(showPicture());
    }

    void Update()
    {
        if (isEnd)
        {
            StartCoroutine(showPicture());
        }
    }
}
