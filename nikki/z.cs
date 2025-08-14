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
using UnityEditor;
using System;

[RequireComponent(typeof(wall))]
public class z : MonoBehaviour
{
    public enum mode{
        effect,
        tele,
        talk
    };
    public enum talkPos
    {
        top, 
        center,
        bottom
    }
    private map m;
    public you you;
    public bool haveFront = true;
    public mode mod;
    public float teleWaitTime = 0.5f;
    public string worldName = "nexus";
    public AudioClip openSound;
    public AudioClip closeSound;
    public int teleX = 0;
    public int teleY = 0;
    public float teleHigh = 0;
    public change.transitionMode enterMode = change.transitionMode.show;
    public float enterModeTime = Game.enterTime;
    public change.transitionMode exitMode = change.transitionMode.hide;
    public float exitModeTime = Game.exitTime;
    public effect getEffect;
    public Canvas canvas
    {
        get
        {
            return FindObjectOfType<Canvas>();
        }
    }
    public GameObject getHintPrefab;
    public AudioClip getSound;
    public AudioClip keySound;
    public bool extra = false;
    public GameObject screen;
    public AudioClip screenSound;
    private bool wait = true;
    public string[] talkTexts;
    public talkPos[] TalkPoses;
    private IEnumerator go()
    {
        if (wait)
        {
            wait = false;
            you.canMove = false;
            you.canOpenMenu = false;
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
            StartCoroutine(you.tele(exitMode, exitModeTime, enterMode, enterModeTime, worldName, teleX, teleY, teleHigh, GetComponent<wall>().face, closeSound, null));
            wait = true;
        }
    }

    private IEnumerator get(effect e)
    {
        if (wait)
        {
            wait = false;
            //示例
            npcMove.npcCanMove = false;
            you.canMove = false;
            you.canOpenMenu = false;
            //play screen(额外)
            /*
                screen.GetComponent<Image>().enabled = true;
                GetComponent<AudioSource>().PlayOneShot(screenSound);
                yield return new WaitForSeconds(1f);
                GetComponent<AudioSource>().Stop();
                screen.GetComponent<Image>().enabled = false;
                yield return new WaitForSeconds(1f);
            }
            */
            //audio
            if (null != keySound && null != getSound && null == GetComponent<AudioSource>())
            {
                gameObject.AddComponent<AudioSource>();
            }
            Instantiate(getHintPrefab, you.transform.position + new Vector3(0, 3, 0), getHintPrefab.transform.rotation);
            GetComponent<AudioSource>().PlayOneShot(keySound);
            yield return new WaitForSeconds(2f);
            GetComponent<AudioSource>().PlayOneShot(getSound);
            you.EffectText.GetComponent<Text>().text = effectItem.effectNames[(int)e];
            //show
            for (int i = 10; i > 0; i--)
            {
                you.EffectText.GetComponent<Text>().color = new Color(you.EffectText.GetComponent<Text>().color.r, you.EffectText.GetComponent<Text>().color.g, you.EffectText.GetComponent<Text>().color.b, 1 / (float)i);
                you.EffectGetScreen.GetComponent<makeMenu>().menuColor = new Color(you.EffectGetScreen.GetComponent<makeMenu>().menuColor.r, you.EffectGetScreen.GetComponent<makeMenu>().menuColor.g, you.EffectGetScreen.GetComponent<makeMenu>().menuColor.b, 1 / (float)i);
                yield return new WaitForSeconds(0.01f);
            }
            //hide
            yield return new WaitForSeconds(1);
            for (int i = 1; i <= 10; i++)
            {
                you.EffectText.GetComponent<Text>().color = new Color(you.EffectText.GetComponent<Text>().color.r, you.EffectText.GetComponent<Text>().color.g, you.EffectText.GetComponent<Text>().color.b, 1 / (float)i);
                you.EffectGetScreen.GetComponent<makeMenu>().menuColor = new Color(you.EffectGetScreen.GetComponent<makeMenu>().menuColor.r, you.EffectGetScreen.GetComponent<makeMenu>().menuColor.g, you.EffectGetScreen.GetComponent<makeMenu>().menuColor.b, 1 / (float)i);
                yield return new WaitForSeconds(0.01f);
            }
            you.EffectGetScreen.GetComponent<makeMenu>().menuColor = new Color(you.EffectGetScreen.GetComponent<makeMenu>().menuColor.r, you.EffectGetScreen.GetComponent<makeMenu>().menuColor.g, you.EffectGetScreen.GetComponent<makeMenu>().menuColor.b, 0);
            you.EffectText.GetComponent<Text>().enabled = false;
            //get effect
            you.canMove = true;
            npcMove.npcCanMove = true;
            you.canOpenMenu = true;
            item.addItem(new effectItem(e));
            wait = true;
        }
        yield return null;
    }

    void Start()
    {
        you = you.You;
        m = GetComponent<wall>().m;
        if ("" == worldName)
        {
            worldName = "nexus";
        }
        if (wasd.n == GetComponent<wall>().face)
        {
            haveFront = false;
            GetComponent<wall>().face = wasd.s;
        }
    }

    void Update()
    {
        if (((m.horizontalIsCycle ? (you.x + 1) % m.x : you.x + 1) == GetComponent<wall>().x && you.y == GetComponent<wall>().y && you.face == wasd.d && (!haveFront || wasd.a == GetComponent<wall>(). face) && Input.GetKeyDown("z")) || ((you.x - 1 < 0 && m.horizontalIsCycle ? m.x - 1 : you.x - 1) == GetComponent<wall>().x && you.y == GetComponent<wall>().y && you.face == wasd.a && (!haveFront || wasd.d == GetComponent<wall>().face) && Input.GetKeyDown("z")) || (you.x == GetComponent<wall>().x && (m.verticalIsCycle ? (you.y + 1) % m.y : you.y + 1) == GetComponent<wall>().y && you.face == wasd.s && (!haveFront || wasd.w == GetComponent<wall>().face) && Input.GetKeyDown("z")) || (you.x == GetComponent<wall>().x && (you.y - 1 < 0 && m.verticalIsCycle ? m.y - 1 : you.y - 1) == GetComponent<wall>().y && you.face == wasd.w && (!haveFront || wasd.s == GetComponent<wall>().face) && Input.GetKeyDown("z")))
        {
            if (mode.tele == mod)
            {
                StartCoroutine(go());
            }
            if (mode.effect == mod && !you.effecthaves[(int)getEffect])
            {
                StartCoroutine(get(getEffect));
            }
        }
    }
}
