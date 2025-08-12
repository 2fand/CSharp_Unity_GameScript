using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum animationMode
{
    loop,
    once,
    ping_pong
}
public class animationRun
{
    static bool isReverse = false;
    public static void run<T>(ref int p, ref T[] arr, animationMode animationMode = animationMode.loop)
    {
        if (animationMode.ping_pong != animationMode)
        {
            isReverse = false;
        }
        switch (animationMode)
        {
            case animationMode.loop:
                p++;
                p %= arr.Length;
                break;
            case animationMode.ping_pong:
                if (arr.Length - 1 == p || 0 == p)
                {
                    isReverse = !isReverse;
                }
                if (!isReverse)
                {
                    p++;
                }
                else
                {
                    p--;
                }
                break;
            default:
                p++;
                break;
        }
    }
    public static void start(ref int p)
    {
        p = 0;
    }
    public static void end<T>(ref int p, ref T[] arr)
    {
        p = arr.Length - 1;
    }
    public static void jumpTo<T>(ref int p, int newP, ref T[] arr)
    {
        p = Mathf.Max(0, Mathf.Min(newP, arr.Length - 1));
    }
}