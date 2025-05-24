using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class randomSymbolSummon2 : MonoBehaviour
{
    public uint space = 10;
    public Material lineMaterial;
    public bool isChange = true;
    public float waitTime = 0.1f;
    private bool isEnd = true;

    private void summon()
    {
        Vector3[] dots = { new Vector3(0, 0, 0), new Vector3(space, 0, 0), new Vector3(2 * space, 0, 0), new Vector3(0, -space, 0), new Vector3(space, -space, 0), new Vector3(2 * space, -space, 0), new Vector3(0, -2 * space, 0), new Vector3(space, -2 * space, 0), new Vector3(2 * space, -2 * space, 0), new Vector3(0, -3 * space, 0), new Vector3(space, -3 * space, 0), new Vector3(2 * space, -3 * space, 0) };
        for (int doti = 0; doti < 12; doti++)
        {
            dots[doti] += transform.position;
        }
        List<Vector3> paths = new List<Vector3>();
        bool[] canMove = new bool[8];
        int[] modeMove = { -4, -3, -2, -1, 1, 2, 3, 4 };
        short dotuse = 0b100000000000;
        int pos = 0;
        int mode = 0;
        while (0b111111111111 != dotuse)
        {
            paths.Add(dots[pos]);
            canMove[1] = (0 != pos / 3);
            canMove[3] = (0 != pos % 3);
            canMove[4] = (2 != pos % 3);
            canMove[6] = (3 != pos / 3);
            canMove[0] = (canMove[1] && canMove[3]);
            canMove[2] = (canMove[1] && canMove[4]);
            canMove[5] = (canMove[6] && canMove[3]);
            canMove[7] = (canMove[6] && canMove[4]);
            mode = Random.Range(0, 8);
            while (!canMove[mode])
            {
                mode = Random.Range(0, 8);
            }
            pos += modeMove[mode];
            dotuse |= (short)(1 << pos);
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
        if (null == GetComponent<LineRenderer>())
        {
            transform.AddComponent<LineRenderer>();
            GetComponent<LineRenderer>().material = lineMaterial;
        }
        summon();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnd && isChange)
        {
            StartCoroutine(write());
        }
    }
}
