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
    public AudioClip openMenuSound;
    public AudioClip closeMenuSound;
    public AudioClip changeSelectSound;
    private static bool isInit = false;
    public static bool IsInit
    {
        get
        {
            return isInit;
        }
    }
    //额外的↓
    public Material[] screens;
    void Awake()
    {
        Game.gameFont = gameFont ?? Game.gameFont;
        you.defaultWalkSound = defaultWalkSound ?? you.defaultWalkSound;
        you.effectEqiupSound = effectEqiupSound ?? you.effectEqiupSound;
        you.effectCancelEqiupSound = effectCancelEqiupSound ?? you.effectCancelEqiupSound;
        you.screens = screens ?? you.screens;
        you.effects = effects ?? you.effects;
        you.effectWalkSounds = effectWalkSounds ?? you.effectWalkSounds;
        you.openMenuSound = openMenuSound ?? you.openMenuSound;
        you.closeMenuSound = closeMenuSound ?? you.closeMenuSound;
        you.changeSelectSound = changeSelectSound ?? you.changeSelectSound;
        isInit = true;
    }

    void Update()
    {

    }
}
