using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

[RequireComponent(typeof(Image))]
public class pictureAnimation : MonoBehaviour
{
    private Sprite[] last_pictures;
    public Sprite[] pictures;
    int p = 0;
    public float waitTime = 0.1f;
    bool isEnd = true;
    animationMode animationMode = animationMode.loop;
    IEnumerator showPicture()
    {
        isEnd = false;
        if (null != pictures && pictures.Length != 0)
        {
            GetComponent<Image>().sprite = pictures[p];
            animationRun.run(ref p, ref pictures, animationMode);
            yield return new WaitForSeconds(waitTime);
        }
        isEnd = true;
    }

    void Start()
    {
        StartCoroutine(showPicture());
        last_pictures = pictures;
    }

    void Update()
    {
        if (last_pictures != pictures)
        {
            last_pictures = pictures;
            p = 0;
        }
        if (isEnd && p >= 0 && p < pictures.Length)
        {
            StartCoroutine(showPicture());
        }
    }
}
