using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class midNum
{
    public static int mid(int min, int i, int max)
    {
        return Mathf.Max(min, Mathf.Min(i, max));
    }
}