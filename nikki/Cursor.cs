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
    private bool canInit = true;
    private static int index = 0;
    public Vector2 positionAdjust = new Vector2(0, -2);
    IEnumerator move()
    {
        moveIsEnd = false;
        yield return new WaitForSeconds(0.08f);
        if (Input.GetKey("w"))
        {
            if (0 == index)
            {
                index = you.selects.Length - 1;
            }
            else
            {
                index--;
            }
            GetComponent<RectTransform>().localPosition = new Vector3(you.selects[index].text.GetComponent<RectTransform>().localPosition.x - you.menuSelectsPositionAdjust.x + positionAdjust.x, you.selects[index].text.GetComponent<RectTransform>().localPosition.y - you.menuSelectsPositionAdjust.y + positionAdjust.y, 0);
        }
        else if (Input.GetKey("s"))
        {
            if (you.selects.Length - 1 == index)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            GetComponent<RectTransform>().localPosition = new Vector3(you.selects[index].text.GetComponent<RectTransform>().localPosition.x - you.menuSelectsPositionAdjust.x + positionAdjust.x, you.selects[index].text.GetComponent<RectTransform>().localPosition.y - you.menuSelectsPositionAdjust.y + positionAdjust.y, 0);
        }
        moveIsEnd = true;
    }
    IEnumerator dodge()
    {
        isEnd = false;
        switch (MenuTheme.myMenu.cursorDodgeMode)
        {
            case dodgeMode.@switch:
                GetComponent<Image>().color += new Color(0, 0, 0, (int)(x + 1) % 2 - GetComponent<Image>().color.a);
                yield return new WaitForSeconds(0.5f);
                x++;
                break;
            case dodgeMode.animation:
                GetComponent<Image>().sprite = MenuTheme.myMenu.cursorAnimation[(int)x];
                x++;
                x = (int)x % MenuTheme.myMenu.cursorAnimation.Length;
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

    void Update()
    {
        if (you.IsOpenMenu)
        {
            if (canInit)
            {
                GetComponent<Image>().color += new Color(0, 0, 0, 1 - GetComponent<Image>().color.a);
                canInit = false;
            }
            if (isEnd)
            {
                StartCoroutine(dodge());
            }
            
        }
        else
        {
            GetComponent<Image>().color += new Color(0, 0, 0, 0 - GetComponent<Image>().color.a);
            canInit = true;
            index = 0;
            GetComponent<RectTransform>().localPosition = new Vector3(you.selects[index].text.GetComponent<RectTransform>().localPosition.x - you.menuSelectsPositionAdjust.x + positionAdjust.x, you.selects[index].text.GetComponent<RectTransform>().localPosition.y - you.menuSelectsPositionAdjust.y + positionAdjust.y, 0);
        }
        if (moveIsEnd)
        {
            StartCoroutine(move());
        }
    }
}
