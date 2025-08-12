using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public static void turn(wasd face, Transform transform)
    {
        switch (face)
        {
            case wasd.w:
                transform.rotation = Quaternion.Euler(-90, 0, 180);
                break;
            case wasd.a:
                transform.rotation = Quaternion.Euler(-90, 0, 90);
                break;
            case wasd.s:
                transform.rotation = Quaternion.Euler(-90, 0, 0);
                break;
            case wasd.d:
                transform.rotation = Quaternion.Euler(-90, 0, -90);
                break;
            default:
                break;
        }
    }
}
