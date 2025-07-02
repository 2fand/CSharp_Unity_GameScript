using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class door : MonoBehaviour
{
    public map m;
    public move you;
    public wall w;
    public move.wasd front;
    public bool haveFront = true;
    public Image image;
    public float waitTime = 0.5f;
    public string worldName = "nexus";
    public int teleX = 0;
    public int teleY = 0;
    private IEnumerator go()
    {
        if (GetComponent<Animation>() != null)
        {
            GetComponent<Animation>().Play();
        }
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < 50; i++)
        {
            image.color += new Color(0, 0, 0, 0.02f);
            yield return 5;
        }
        SceneManager.LoadScene(worldName);
        move.teleX = teleX;
        move.teleY = teleY;
        move.front = front;
        move.isTele = true;
    }

    void Start()
    {
        if (move.wasd.n == front)
        {
            haveFront = false;
            front = move.wasd.s;
        }
    }

    void Update()
    {
        if ((you.x + 1) % m.x == w.x && you.y == w.y && move.front == move.wasd.d && (!haveFront || move.wasd.a == front) && Input.GetKeyDown("z"))
        {
            //交互后动作
            StartCoroutine(go());
        }
        else if ((you.x - 1 < 0 ? m.x - 1 : you.x - 1) == w.x && you.y == w.y && move.front == move.wasd.a && (!haveFront || move.wasd.d == front) && Input.GetKeyDown("z"))
        {
            //交互后动作
            StartCoroutine(go());
        }
        if (you.x == w.x && (you.y + 1) % m.y == w.y && move.front == move.wasd.s && (!haveFront || move.wasd.w == front) && Input.GetKeyDown("z"))
        {
            //交互后动作
            StartCoroutine(go());
        }
        if (you.x == w.x && (you.y - 1 < 0 ? m.y - 1 : you.y - 1) == w.y && move.front == move.wasd.w && (!haveFront || move.wasd.s == front) && Input.GetKeyDown("z"))
        {
            //交互后动作
            StartCoroutine(go());
        }
    }
}
