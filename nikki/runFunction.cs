using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static trigger;
public class runFunction : MonoBehaviour
{
    private bool funcIsEnd = true;
    IEnumerator runFunc()
    {
        funcIsEnd = false;
        you.canMove = false;
        if (you.moveIsEnd && IsDone && funcs.Count > 0)
        {
            while (funcs.Count > 0)
            {
                StartCoroutine(funcs[0]);
                funcs.RemoveAt(0);
                yield return new WaitUntil(() => you.teleIsEnd && you.moveIsEnd && you.commandIsEnd);
            }
        }
        you.canMove = true;
        funcIsEnd = true;
        yield return null;
    }

    void Start()
    {

    }

    void Update()
    {
        if (funcIsEnd && funcs.Count != 0)
        {
            StartCoroutine(runFunc());
        }
    }
}
