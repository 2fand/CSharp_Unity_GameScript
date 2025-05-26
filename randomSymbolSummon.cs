using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class randomSymbolSummon : MonoBehaviour
{
    public float space = 0.809f;
    public uint a = 2;
    public uint b = 3;
    public Material lineMaterial;
    public float waitTime = 0.1f;
    private bool isEnd = true;
    private void summon()
    {
        if (32 <= a + b) {
            a = 3;
            b = 4;
        }
        Vector3[] dots = new Vector3[a * b];
        for (int doti = 0; doti < dots.Length; ++doti)
        {
            dots[doti] = new Vector3(doti % a * space, doti / a * -space, 0);
        }
        List<Vector3> paths = new List<Vector3>();
        bool[] canMove = new bool[8];
        int[] modeMove = { 0, 0, 0, -1, 1, 0, 0, 0 };
        for (int modei = 0, step = (int)a + 1; modei < 3; ++modei)
        {
            modeMove[modei] = -step;
            modeMove[modeMove.Length - 1 - modei] = step--;
        }
        int dotuse = 0;
        int pos = Random.Range(0, (int)(a * b));
        int mode = 0;
        bool LastDraw = 1 == Random.Range(0, 2);
        paths.Add(dots[pos]);
        while (dotuse != (1 << (int)(a + b)) - 1 || LastDraw)
        {
            canMove[1] = (0 != pos / a);
            canMove[3] = (0 != pos % a);
            canMove[4] = ((a - 1) != pos % a);
            canMove[6] = ((b - 1) != pos / a);
            canMove[0] = (canMove[1] && canMove[3]);
            canMove[2] = (canMove[1] && canMove[4]);
            canMove[5] = (canMove[6] && canMove[3]);
            canMove[7] = (canMove[6] && canMove[4]);
            if (dotuse == (1 << (int)(a + b)) - 1)
            {
                canMove[7 - mode] = false;
                LastDraw = false;
            }
            mode = Random.Range(0, 8);
            while (!canMove[mode])
            {
                mode = Random.Range(0, 8);
            }
            pos += modeMove[mode];
            dotuse |= (1 << (pos / (int)a)) + (1 << (int)(b + pos % a));
            paths.Add(dots[pos]);
        }
        GetComponent<LineRenderer>().positionCount = paths.Count;
        GetComponent<LineRenderer>().SetPositions(paths.ToArray());
    }
    IEnumerator write()
    {
        isEnd = false;
        yield return new WaitForSeconds(waitTime);
        summon();
        isEnd = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (0 == a || 0 == b)
        {
            enabled = false;
        }
        a++;
        b++;
        space *= space < 0 ? -1 : 1;
        if (null == GetComponent<LineRenderer>())
        {
            transform.AddComponent<LineRenderer>();
            GetComponent<LineRenderer>().material = lineMaterial;
            GetComponent<LineRenderer>().useWorldSpace = false;
            GetComponent<LineRenderer>().startWidth = GetComponent<LineRenderer>().endWidth = 0.1f;
        }
        summon();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnd)
        {
            StartCoroutine(write());
        }
    }
}
