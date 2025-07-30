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
    public Cursor.HorizontalMoveMode CursorHorizontalMoveMode = Cursor.HorizontalMoveMode.loop;
    public Cursor.VerticalMoveMode CursorVerticalMoveMode = Cursor.VerticalMoveMode.loop;
    public AudioClip clearMenuSound;
    public AudioClip wakeUpSound;
    private static bool isInit = false;
    public static bool IsInit
    {
        get
        {
            return isInit;
        }
    }
    //¶îÍâµÄ¡ý
    public Material[] screens;
    void Awake()
    {
        Game.gameFont = gameFont ?? Game.gameFont;
        you.defaultWalkSound = defaultWalkSound ?? you.defaultWalkSound;
        effectItem.effectEqiupSound = effectEqiupSound ?? effectItem.effectEqiupSound;
        effectItem.effectCancelEqiupSound = effectCancelEqiupSound ?? effectItem.effectCancelEqiupSound;
        effectItem.screens = screens ?? effectItem.screens;
        effectItem.effects = effects ?? effectItem.effects;
        effectItem.effectWalkSounds = effectWalkSounds ?? effectItem.effectWalkSounds;
        you.openMenuSound = openMenuSound ?? you.openMenuSound;
        you.closeMenuSound = closeMenuSound ?? you.closeMenuSound;
        you.changeSelectSound = changeSelectSound ?? you.changeSelectSound;
        you.wakeUpSound = wakeUpSound ?? you.wakeUpSound;
        you.clearSound = clearMenuSound ?? you.clearSound;
        Cursor.horizontalMoveMode = CursorHorizontalMoveMode;
        Cursor.verticalMoveMode = CursorVerticalMoveMode;
        isInit = true;
    }

    void Update()
    {

    }
}
