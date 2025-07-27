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
    public bool isWait = true;
    public static bool cursorCanMove = true;
    public you u;
    public static bool update = false;
    private Color cursorBeforeColor;
    public static Color cursorColor = new Color(1, 1, 1, 0);
    IEnumerator move()
    {
        moveIsEnd = false;
        if (Input.GetKey("w"))
        {
            GetComponent<AudioSource>().PlayOneShot(you.changeSelectSound);
            int last_indexI = indexI;
            if (0 == indexI)
            {
                indexI = you.yourSelects.GetLength(0) - 1;
            }
            else
            {
                indexI--;
            }
            if (null == you.yourSelect)
            {
                indexI = last_indexI;
            }
            yield return new WaitForSeconds(isWait ? 0.3f : 0.1f);
            isWait = false;
        }
        else if (Input.GetKey("s"))
        {
            GetComponent<AudioSource>().PlayOneShot(you.changeSelectSound);
            int last_indexI = indexI;
            if (you.yourSelects.GetLength(0) - 1 == indexI)
            {
                indexI = 0;
            }
            else
            {
                indexI++;
            }
            if (null == you.yourSelect)
            {
                indexI = last_indexI;
            }
            yield return new WaitForSeconds(isWait ? 0.3f : 0.1f);
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
        GetComponent<RectTransform>().pivot = new Vector2(0, 1);
        if (null == GetComponent<Image>())
        {
            gameObject.AddComponent<Image>();
        }
        GetComponent<Image>().type = Image.Type.Sliced;
        cursorBeforeColor = GetComponent<Image>().color;
        if (null == GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }        
    }

    void Update()
    {
        GetComponent<Image>().color = 0 != you.Menus.Count && null != you.yourSelects && 0 != you.yourSelects.GetLength(0) && 0 != you.yourSelects.GetLength(1) && !(select.menuClass.item == you.Menus[you.Menus.Count - 1] && 0 == you.NotNullItemsNum) ? cursorColor : new Color(1,1,1,0);
        if (0 != you.Menus.Count)
        {
            if (update)
            {
                indexI = indexJ = 0;
                if (null != you.yourSelects && 0 != you.yourSelects.GetLength(0) && 0 != you.yourSelects.GetLength(1) && null != you.yourSelect)
                {
                    GetComponent<RectTransform>().localPosition = new Vector3(you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().localPosition.x, you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().localPosition.y, 0);
                    GetComponent<RectTransform>().sizeDelta = you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().sizeDelta;
                }
                transform.SetAsLastSibling();
                x = 0;
                update = false;
            }
            if (isEnd)
            {
                StartCoroutine(dodge());
            }
            if (null != you.yourSelects && cursorCanMove && moveIsEnd)
            {
                StartCoroutine(move());
            }
            if (null != you.yourSelects && indexI < you.yourSelects.GetLength(0) && indexJ < you.yourSelects.GetLength(1) && null != you.yourSelect)
            {
                GetComponent<RectTransform>().localPosition = new Vector3(you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().localPosition.x - textAdjust.getOffset(you.yourSelects[indexI, indexJ].text.GetComponent<Text>().alignment), you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().localPosition.y, 0);
            }
        }
    }
}
