using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public enum dodgeMode
    {
        none,
        animation,
        @switch,
        fade
    }
    private bool isEnd = true;
    private bool moveIsEnd = true;
    private float x = 0;
    private static int indexI = 0;
    private static int indexJ = 0;
    public static int IndexI
    {
        get
        {
            return indexI;
        }
    }
    public static int IndexJ
    {
        get
        {
            return indexJ;
        }
    }
    public Vector2 positionAdjust = new Vector2(0, -2);
    public bool isWait = true;
    public static bool cursorCanMove = true;
    public you u;
    public static bool update = false;
    IEnumerator move()
    {
        moveIsEnd = false;
        if (Input.GetKey("w"))
        {
            GetComponent<AudioSource>().PlayOneShot(you.changeSelectSound);
            if (0 == indexI)
            {
                indexI = you.yourSelects.GetLength(0) - 1;
            }
            else
            {
                indexI--;
            }
            yield return new WaitForSeconds(isWait ? 0.4f : 0.1f);
            isWait = false;
        }
        else if (Input.GetKey("s"))
        {
            GetComponent<AudioSource>().PlayOneShot(you.changeSelectSound);
            if (you.yourSelects.GetLength(0) - 1 == indexI)
            {
                indexI = 0;
            }
            else
            {
                indexI++;
            }
            yield return new WaitForSeconds(isWait ? 0.4f : 0.1f);
            isWait = false;
        }
        else
        {
            isWait = true;
        }
        moveIsEnd = true;
    }
    IEnumerator dodge()
    {
        isEnd = false;
        switch (you.myMenu.cursorDodgeMode)
        {
            case dodgeMode.@switch:
                GetComponent<Image>().color += new Color(0, 0, 0, (int)(x + 1) % 2 - GetComponent<Image>().color.a);
                yield return new WaitForSeconds(0.5f);
                x++;
                break;
            case dodgeMode.animation:
                GetComponent<Image>().sprite = you.myMenu.cursorAnimation[(int)x];
                x++;
                x = (int)x % you.myMenu.cursorAnimation.Length;
                yield return new WaitForSeconds(0.1f);
                break;
            case dodgeMode.fade:
                GetComponent<Image>().color += new Color(0, 0, 0, Mathf.Abs(Mathf.Cos(x)) - GetComponent<Image>().color.a);
                x += 0.01f;
                break;
            default:
                break;
        }
        isEnd = true;
        yield return null;
    }

    void Start()
    {
        if (null == GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }        
    }

    void Update()
    {
        if (0 != you.Menus.Count)
        {
            if (update)
            {
                indexI = indexJ = 0;
                GetComponent<RectTransform>().localPosition = new Vector3(you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().localPosition.x - you.menuSelectsPositionAdjust.x + positionAdjust.x, you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().localPosition.y - you.menuSelectsPositionAdjust.y + positionAdjust.y, 0);
                GetComponent<RectTransform>().sizeDelta = you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().sizeDelta;
                update = false;
            }
            if (isEnd)
            {
                StartCoroutine(dodge());
            }
            if (cursorCanMove && moveIsEnd)
            {
                StartCoroutine(move());
            }
            if (indexI < you.yourSelects.GetLength(0) && indexJ < you.yourSelects.GetLength(1))
            {
                GetComponent<RectTransform>().localPosition = new Vector3(you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().localPosition.x - you.menuSelectsPositionAdjust.x + positionAdjust.x, you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().localPosition.y - you.menuSelectsPositionAdjust.y + positionAdjust.y, 0);
            }
        }
    }
}
