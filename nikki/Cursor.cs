using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static select;

public class Cursor : MonoBehaviour
{
    public enum dodgeMode
    {
        none,
        animation,
        @switch,
        fade
    }
    public enum VerticalMoveMode
    {
        normal, 
        loop,
        toNextColumn,
        toNextColumnAndLoop
    }
    public enum HorizontalMoveMode
    {
        normal,
        loop,
        toNextRow,
        toNextRowAndLoop
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
    public static HorizontalMoveMode horizontalMoveMode;
    public static VerticalMoveMode verticalMoveMode;
    private select[,] last_yourSelects = you.yourSelects; 
    private int[,] horizontalLoopField = new int[0, 2];
    private int[,] verticalLoopField = new int[0, 2];
    private int[] rowLoopField = new int[2];
    private int[] columnLoopField = new int[2];
    IEnumerator move()
    {
        moveIsEnd = false;
        bool isCanDo = false;
        if (Input.GetKey("w"))
        {
            if (1 < you.yourSelects.GetLength(0) && 0 != indexI && null != you.yourSelects[indexI - 1, indexJ])
            {
                isCanDo = true;
                indexI--;
            }
            else {
                switch (verticalMoveMode)
                {
                    case VerticalMoveMode.loop:
                        if (1 < you.yourSelects.GetLength(0) && verticalLoopField[indexJ, 0] == indexI)
                        {
                            isCanDo = true;
                            indexI = verticalLoopField[indexJ, 1];
                        }
                        break;
                    case VerticalMoveMode.toNextColumn:
                        if (indexJ > columnLoopField[0] && verticalLoopField[indexJ, 0] == indexI && -1 != verticalLoopField[indexJ - 1, 0] && !(rowLoopField[0] == rowLoopField[1] && columnLoopField[0] == columnLoopField[1]))
                        {
                            isCanDo = true;
                            indexI = verticalLoopField[--indexJ, 1];
                        }
                        break;
                    case VerticalMoveMode.toNextColumnAndLoop:
                        if (indexJ > columnLoopField[0] && verticalLoopField[indexJ, 0] == indexI && -1 != verticalLoopField[indexJ - 1, 0] && !(rowLoopField[0] == rowLoopField[1] && columnLoopField[0] == columnLoopField[1]))
                        {
                            isCanDo = true;
                            indexI = verticalLoopField[--indexJ, 1];
                        }
                        else if(columnLoopField[0] == indexJ && verticalLoopField[columnLoopField[0], 0] == indexI && !(rowLoopField[0] == rowLoopField[1] && columnLoopField[0] == columnLoopField[1]))
                        {
                            isCanDo = true;
                            indexI = verticalLoopField[indexJ = columnLoopField[1], 1];
                        }
                        break;
                    default:
                        break;
                }
            }
            if (isCanDo)
            {
                GetComponent<AudioSource>().PlayOneShot(you.changeSelectSound);
            }
            yield return new WaitForSeconds(isWait ? 0.3f : 0.1f);
            isWait = false;
        }
        else if (Input.GetKey("s"))
        {
            if (1 < you.yourSelects.GetLength(0) && you.yourSelects.GetLength(0) - 1 != indexI && null != you.yourSelects[indexI + 1, indexJ])
            {
                isCanDo = true;
                indexI++;
            }
            else
            {
                switch (verticalMoveMode)
                {
                    case VerticalMoveMode.loop:
                        if (1 < you.yourSelects.GetLength(0) && verticalLoopField[indexJ, 1] == indexI)
                        {
                            isCanDo = true;
                            indexI = verticalLoopField[indexJ, 0];
                        }
                        break;
                    case VerticalMoveMode.toNextColumn:
                        if (indexJ < columnLoopField[1] && verticalLoopField[indexJ, 1] == indexI && -1 != verticalLoopField[indexJ + 1, 0] && !(rowLoopField[0] == rowLoopField[1] && columnLoopField[0] == columnLoopField[1]))
                        {
                            isCanDo = true;
                            indexI = verticalLoopField[++indexJ, 0];
                        }
                        break;
                    case VerticalMoveMode.toNextColumnAndLoop:
                        if (indexJ < columnLoopField[1] && verticalLoopField[indexJ, 1] == indexI && -1 != verticalLoopField[indexJ + 1, 0] && !(rowLoopField[0] == rowLoopField[1] && columnLoopField[0] == columnLoopField[1]))
                        {
                            isCanDo = true;
                            indexI = verticalLoopField[++indexJ, 0];
                        }
                        else if (you.yourSelects.GetLength(1) - 1 == indexJ && verticalLoopField[columnLoopField[1], 1] == indexI && !(rowLoopField[0] == rowLoopField[1] && columnLoopField[0] == columnLoopField[1]))
                        {
                            isCanDo = true;
                            indexI = verticalLoopField[indexJ = columnLoopField[0], 0];
                        }
                        break;
                    default:
                        break;
                }
            }
            if (isCanDo)
            {
                GetComponent<AudioSource>().PlayOneShot(you.changeSelectSound);
            }
            yield return new WaitForSeconds(isWait ? 0.3f : 0.1f);
            isWait = false;
        }
        else if (Input.GetKey("a"))
        {
            if (1 < you.yourSelects.GetLength(1) && 0 != indexJ && null != you.yourSelects[indexI, indexJ - 1])
            {
                isCanDo = true;
                indexJ--;
            }
            else
            {
                switch (horizontalMoveMode)
                {
                    case HorizontalMoveMode.loop:
                        if (1 < you.yourSelects.GetLength(1) && horizontalLoopField[indexI, 0] == indexJ)
                        {
                            isCanDo = true;
                            indexJ = horizontalLoopField[indexI, 1];
                        }
                        break;
                    case HorizontalMoveMode.toNextRow:
                        if (indexI > rowLoopField[0] && horizontalLoopField[indexI, 0] == indexJ && -1 != horizontalLoopField[indexI - 1, 0] && !(rowLoopField[0] == rowLoopField[1] && columnLoopField[0] == columnLoopField[1]))
                        {
                            isCanDo = true;
                            indexJ = horizontalLoopField[--indexI, 1];
                        }
                        break;
                    case HorizontalMoveMode.toNextRowAndLoop:
                        if (indexI > rowLoopField[0] && horizontalLoopField[indexI, 0] == indexJ && -1 != horizontalLoopField[indexI - 1, 0] && !(rowLoopField[0] == rowLoopField[1] && columnLoopField[0] == columnLoopField[1]))
                        {
                            isCanDo = true;
                            indexJ = horizontalLoopField[--indexI, 1];
                        }
                        else if (rowLoopField[0] == indexI && horizontalLoopField[rowLoopField[0], 0] == indexJ && !(rowLoopField[0] == rowLoopField[1] && columnLoopField[0] == columnLoopField[1]))
                        {
                            isCanDo = true;
                            indexJ = horizontalLoopField[indexI = rowLoopField[1], 1];
                        }
                        break;
                    default:
                        break;
                }
            }
            if (isCanDo)
            {
                GetComponent<AudioSource>().PlayOneShot(you.changeSelectSound);
            }
            yield return new WaitForSeconds(isWait ? 0.3f : 0.1f);
            isWait = false;
        }
        else if (Input.GetKey("d"))
        {
            if (1 < you.yourSelects.GetLength(1) && you.yourSelects.GetLength(1) - 1 != indexJ && null != you.yourSelects[indexI, indexJ + 1])
            {
                isCanDo = true;
                indexJ++;
            }
            else
            {
                switch (horizontalMoveMode)
                {
                    case HorizontalMoveMode.loop:
                        if (1 < you.yourSelects.GetLength(1) && horizontalLoopField[indexI, 1] == indexJ)
                        {
                            isCanDo = true;
                            indexJ = horizontalLoopField[indexI, 0];
                        }
                        break;
                    case HorizontalMoveMode.toNextRow:
                        if (indexI < rowLoopField[1] && horizontalLoopField[indexI, 1] == indexJ && -1 != horizontalLoopField[indexI + 1, 0] && !(rowLoopField[0] == rowLoopField[1] && columnLoopField[0] == columnLoopField[1]))
                        {
                            isCanDo = true;
                            indexJ = horizontalLoopField[++indexI, 0];
                        }
                        break;
                    case HorizontalMoveMode.toNextRowAndLoop:
                        if (indexI < rowLoopField[1] && horizontalLoopField[indexI, 1] == indexJ && -1 != horizontalLoopField[indexI + 1, 0] && !(rowLoopField[0] == rowLoopField[1] && columnLoopField[0] == columnLoopField[1]))
                        {
                            isCanDo = true;
                            indexJ = horizontalLoopField[++indexI, 0];
                        }
                        else if (rowLoopField[1] == indexI && horizontalLoopField[rowLoopField[1], 1] == indexJ && !(rowLoopField[0] == rowLoopField[1] && columnLoopField[0] == columnLoopField[1]))
                        {
                            isCanDo = true;
                            indexJ = horizontalLoopField[indexI = columnLoopField[0], 0];
                        }
                        break;
                    default:
                        break;
                }
            }
            if (isCanDo)
            {
                GetComponent<AudioSource>().PlayOneShot(you.changeSelectSound);
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
                if (null == GetComponent<pictureAnimation>())
                {
                    gameObject.AddComponent<pictureAnimation>().pictures = you.myMenu.cursorAnimation;
                }
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

    private void init()
    {
        last_yourSelects = you.yourSelects;
        if (null == you.yourSelects)
        {
            return;
        }
        horizontalLoopField = new int[you.yourSelects.GetLength(0), 2];
        verticalLoopField = new int[you.yourSelects.GetLength(1), 2];
        bool horizontalIsNull = true;
        bool verticalIsNull = true;
        bool rowLoopIsNull = true;
        bool columnLoopIsNull = true;
        for (int i = 0; i < horizontalLoopField.GetLength(0); i++)
        {
            for (int j = 0; j < you.yourSelects.GetLength(1); j++)
            {
                if (null != you.yourSelects[i, j])
                {
                    horizontalLoopField[i, 0] = j;
                    horizontalIsNull = false;
                    break;
                }
            }
            for (int j = you.yourSelects.GetLength(1) - 1; j >= 0; j--)
            {
                if (null != you.yourSelects[i, j])
                {
                    horizontalLoopField[i, 1] = j;
                    break;
                }
            }
            if (horizontalIsNull)
            {
                horizontalLoopField[i, 0] = horizontalLoopField[i, 1] = -1;
            }
            horizontalIsNull = true;
        }
        for (int i = 0; i < verticalLoopField.GetLength(0); i++)
        {
            for (int j = 0; j < you.yourSelects.GetLength(0); j++)
            {
                if (null != you.yourSelects[j, i])
                {
                    verticalLoopField[i, 0] = j;
                    verticalIsNull = false;
                    break;
                }
            }
            for (int j = you.yourSelects.GetLength(0) - 1; j >= 0; j--)
            {
                if (null != you.yourSelects[j, i])
                {
                    verticalLoopField[i, 1] = j;
                    break;
                }
            }
            if (verticalIsNull)
            {
                verticalLoopField[i, 0] = verticalLoopField[i, 1] = -1;
            }
            verticalIsNull = true;
        }
        for (int i = 0; i < horizontalLoopField.GetLength(0); i++) {
            if (-1 != horizontalLoopField[i, 0])
            {
                rowLoopField[0] = i;
                rowLoopIsNull = false;
                break;
            }
        }
        for (int i = horizontalLoopField.GetLength(0) - 1; i >= 0; i--)
        {
            if (-1 != horizontalLoopField[i, 0])
            {
                rowLoopField[1] = i;
                break;
            }
        }
        if (rowLoopIsNull)
        {
            rowLoopField[0] = rowLoopField[1] = -1;
        }
        for (int i = 0; i < verticalLoopField.GetLength(0); i++)
        {
            if (-1 != verticalLoopField[i, 0])
            {
                columnLoopField[0] = i;
                columnLoopIsNull = false;
                break;
            }
        }
        for (int i = verticalLoopField.GetLength(0) - 1; i >= 0; i--)
        {
            if (-1 != verticalLoopField[i, 0])
            {
                columnLoopField[1] = i;
                break;
            }
        }
        if (columnLoopIsNull)
        {
            columnLoopField[0] = columnLoopField[1] = -1;
        }
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
        init();
    }

    void Update()
    {
        if (last_yourSelects != you.yourSelects)
        {
            init();   
        }
        GetComponent<Image>().color = 0 != you.Menus.Count && null != you.yourSelects && 0 != you.yourSelects.GetLength(0) && 0 != you.yourSelects.GetLength(1) && !(select.menuClass.item == you.Menus[you.Menus.Count - 1] && 0 == you.NotNullItemsNum) ? cursorColor : new Color(1,1,1,0);
        if (0 != you.Menus.Count)
        {
            if (update)
            {
                indexI = indexJ = 0;
                if (null != you.yourSelects && 0 != you.yourSelects.GetLength(0) && 0 != you.yourSelects.GetLength(1) && null != you.yourSelect && null != you.yourSelects[indexI, indexJ].text)
                {
                    GetComponent<RectTransform>().localPosition = new Vector3(you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().localPosition.x, you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().localPosition.y, 0);
                    transform.SetParent(you.yourSelect.text.transform.parent, false);
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
            if (null != you.yourSelects && cursorCanMove && moveIsEnd && -1 != rowLoopField[0])
            {
                StartCoroutine(move());
            }
            if (null != you.yourSelects && indexI < you.yourSelects.GetLength(0) && indexJ < you.yourSelects.GetLength(1) && null != you.yourSelect && null != you.yourSelects[indexI, indexJ].text)
            {
                GetComponent<RectTransform>().localPosition = new Vector3(you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().localPosition.x - textAdjust.getOffset(you.yourSelects[indexI, indexJ].text.GetComponent<Text>().alignment), you.yourSelects[indexI, indexJ].text.GetComponent<RectTransform>().localPosition.y, 0);
                transform.SetParent(you.yourSelects[indexI, indexJ].text.transform.parent, false);
            }
        }
    }
}
