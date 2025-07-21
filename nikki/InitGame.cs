using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    public AudioClip defaultWalkSound;
    public AudioClip effectEqiupSound;
    public AudioClip effectCancelEqiupSound;
    public GameObject[] effects;
    public AudioClip[] effectWalkSounds;
    public Font gameFont;
    //额外的↓
    public Material[] screens;
    private static bool isInit = false;
    public static bool IsInit
    {
        get
        {
            return isInit;
        }
    }
    void Awake()
    {
        Game.gameFont = gameFont ?? Game.gameFont;
        you.defaultWalkSound = defaultWalkSound ?? you.defaultWalkSound;
        you.effectEqiupSound = effectEqiupSound ?? you.effectEqiupSound;
        you.effectCancelEqiupSound = effectCancelEqiupSound ?? you.effectCancelEqiupSound;
        you.screens = screens ?? you.screens;
        you.effects = effects ?? you.effects;
        you.effectWalkSounds = effectWalkSounds ?? you.effectWalkSounds;
        isInit = true;
    }

    void Update()
    {

    }
}
