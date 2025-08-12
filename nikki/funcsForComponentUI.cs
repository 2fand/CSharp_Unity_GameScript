using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class funcsForComponentUI
{
    public static void updateArray<T>(ref T[] array, int newLength)
    {
        if (null == array || array.Length != newLength)
        {
            T[] ts = new T[newLength];
            if (null != array)
            {
                Array.Copy(array, ts, Mathf.Min(array.Length, newLength));
            }
            array = ts;
        }
    }
}
