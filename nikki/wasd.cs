using System.Collections;
using UnityEngine;

public enum wasd
{
    w,
    a,
    s,
    d,
    n,
    u
}
public class face
{
    public Vector2 faceValue;
    private static Hashtable wasdToVector2 = new Hashtable { { wasd.w, new Vector2(0, 1) }, { wasd.a, new Vector2(-1, 0) }, { wasd.s, new Vector2(0, -1) }, { wasd.d, new Vector2(1, 0) }, { wasd.n, new Vector2(0, 0) } };
    private static Hashtable vector2ToWasd = new Hashtable { { new Vector2(0, 1), wasd.w }, { new Vector2(-1, 0), wasd.a }, { new Vector2(0, -1), wasd.s }, { new Vector2(1, 0), wasd.d }, { new Vector2(0, 0), wasd.n } };
    public static Vector2 WasdToVector2(wasd wasd)
    {
        if (wasd.u == wasd)
        {
            wasd = you.face;
        }
        return (Vector2)wasdToVector2[wasd];
    }
    public static wasd Vector2ToWasd(Vector2 face)
    {
        if (vector2ToWasd.ContainsKey(face))
        {
            return (wasd)vector2ToWasd[face];
        }
        return (wasd)(-1);
    }
    public static wasd ReverseFace(wasd wasd)
    {
        if (wasd.n == wasd)
        {
            return wasd;
        }
        if (wasd.u == wasd)
        {
            wasd = you.face;
        }
        wasd = (wasd)((int)(wasd + 2) % 4);
        return wasd;
    }
}
