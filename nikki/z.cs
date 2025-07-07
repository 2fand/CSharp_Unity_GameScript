using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Device;

public class z : MonoBehaviour
{
    public enum mode{
        normal,
        tele,
        effect,
        talk
    };
    public map m;
    public you you;
    public wall w;
    public bool haveFront = true;
    public mode mod = mode.tele;
    public Image image;
    public float teleWaitTime = 0.5f;
    public string worldName = "nexus";
    public AudioClip openSound;
    public AudioClip closeSound;
    public int teleX = 0;
    public int teleY = 0;
    public effect getEffect;
    public GameObject getImage;
    public AudioClip getSound;
    public bool extra = false;
    public GameObject screen;
    public AudioClip screenSound;
    private IEnumerator go()
    {
        you.canMove = false;
        if (null != openSound && null == GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }
        if (null != openSound)
        {
            GetComponent<AudioSource>().PlayOneShot(openSound);
        }
        if (GetComponent<Animation>() != null)
        {
            GetComponent<Animation>().Play();
        }
        yield return new WaitForSeconds(teleWaitTime);
        StartCoroutine(you.tele(image, worldName, teleX, teleY, w.front, closeSound));
    }

    private IEnumerator get(effect e)
    {
        //Ê¾Àý
        you.effects[(int)e] = true;
        you.effectNum++;
        you.canMove = false;
        //audio
        if (null == GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }
        //play screen
        if (extra)
        {
            screen.GetComponent<Image>().enabled = true;
            GetComponent<AudioSource>().PlayOneShot(screenSound);
            yield return new WaitForSeconds(1f);
            GetComponent<AudioSource>().Stop();
            screen.GetComponent<Image>().enabled = false;
            yield return new WaitForSeconds(1f);
        }
        GetComponent<AudioSource>().PlayOneShot(getSound);
        getImage.GetComponentInChildren<Text>().text = you.effectName[(int)e];
        getImage.GetComponent<Image>().enabled = true;
        getImage.GetComponentInChildren<Text>().enabled = true;
        //show
        for (int i = 10; i > 0; i--)
        {
            getImage.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 1f / (float)i);
            getImage.GetComponentInChildren<Text>().color = new Color(0, 0, 0.2f, 1 / (float)i);
            yield return new WaitForSeconds(0.01f);
        }
        //hide
        yield return new WaitForSeconds(1);
        for (int i = 1; i <= 10; i++)
        {
            getImage.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 1f / (float)i);
            getImage.GetComponentInChildren<Text>().color = new Color(0, 0, 0.2f, 1 / (float)i);
            yield return new WaitForSeconds(0.01f);
        }
        getImage.GetComponent<Image>().enabled = false;
        getImage.GetComponentInChildren<Text>().enabled = false;
        //get effect
        you.canMove = true;
        mod = mode.normal;
        yield return null;
    }

    void Start()
    {
        if ("" == worldName)
        {
            worldName = "nexus";
        }
        if (you.wasd.n == w.front)
        {
            haveFront = false;
            w.front = you.wasd.s;
        }
    }

    void Update()
    {
        if ((you.x + 1) % m.x == w.x && you.y == w.y && you.front == you.wasd.d && (!haveFront || you.wasd.a == w.front) && Input.GetKeyDown("z"))
        {
            if (mode.tele == mod)
            {
                StartCoroutine(go());
            }
            if (mode.effect == mod && !you.effects[(int)getEffect])
            {
                StartCoroutine(get(getEffect));
            }
        }
        else if ((you.x - 1 < 0 ? m.x - 1 : you.x - 1) == w.x && you.y == w.y && you.front == you.wasd.a && (!haveFront || you.wasd.d == w.front) && Input.GetKeyDown("z"))
        {
            if (mode.tele == mod)
            {
                StartCoroutine(go());
            }
            if (mode.effect == mod && !you.effects[(int)getEffect])
            {
                StartCoroutine(get(getEffect));
            }
        }
        if (you.x == w.x && (you.y + 1) % m.y == w.y && you.front == you.wasd.s && (!haveFront || you.wasd.w == w.front) && Input.GetKeyDown("z"))
        {
            if (mode.tele == mod)
            {
                StartCoroutine(go());
            }
            if (mode.effect == mod && !you.effects[(int)getEffect])
            {
                StartCoroutine(get(getEffect));
            }
        }
        if (you.x == w.x && (you.y - 1 < 0 ? m.y - 1 : you.y - 1) == w.y && you.front == you.wasd.w && (!haveFront || you.wasd.s == w.front) && Input.GetKeyDown("z"))
        {
            if (mode.tele == mod)
            {
                StartCoroutine(go());
            }
            if (mode.effect == mod && !you.effects[(int)getEffect])
            {
                StartCoroutine(get(getEffect));
            }
        }
    }
}
