using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textAdjust
{
    public static void adjustText(ref GameObject text, TextAnchor alignment)
    {
        if (0 == (int)alignment % 3)
        {
            text.GetComponent<RectTransform>().localPosition += new Vector3(8, 0, 0);
        }
        else if (2 == (int)alignment % 3)
        {
            text.GetComponent<RectTransform>().localPosition += new Vector3(-8, 0, 0);
        }
    }

    public static int getOffset(TextAnchor alignment)
    {
        int[] values = { 8, 0, -8 };
        return values[(int)alignment % 3];
    }
}
