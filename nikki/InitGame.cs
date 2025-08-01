using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    public AudioClip effectEqiupSound;
    public AudioClip effectCancelEqiupSound;
    public GameObject[] effects;
    public AudioClip[] effectWalkSounds;
    public closeUp[] effectCloseUp;
    public Font gameFont;
    public AudioClip openMenuSound;
    public AudioClip closeMenuSound;
    public AudioClip changeSelectSound;
    public Cursor.HorizontalMoveMode CursorHorizontalMoveMode = Cursor.HorizontalMoveMode.loop;
    public Cursor.VerticalMoveMode CursorVerticalMoveMode = Cursor.VerticalMoveMode.loop;
    public AudioClip clearMenuSound;
    public AudioClip wakeUpSound;
    public bool useHealth = false;
    public string hpUnit = "";
    public bool useMp = false;
    public string mpUnit = "";
    public bool useTp = false;
    public string tpUnit = "";
    public bool useLevel = false;
    public string levelUnit = "";
    private static bool isInit = false;
    public Sprite[] closeUps;
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
        Game.gameFont = gameFont;
        Game.useHealth = useHealth;
        Game.hpUnit = hpUnit;
        Game.useMp = useMp;
        Game.mpUnit = mpUnit;
        Game.useTp = useTp;
        Game.tpUnit = tpUnit;
        Game.useLevel = useLevel;
        Game.levelUnit = levelUnit;
        effectItem.effectEqiupSound = effectEqiupSound ?? effectItem.effectEqiupSound;
        effectItem.effectCancelEqiupSound = effectCancelEqiupSound ?? effectItem.effectCancelEqiupSound;
        effectItem.screens = screens ?? effectItem.screens;
        effectItem.effects = effects ?? effectItem.effects;
        effectItem.effectWalkSounds = effectWalkSounds ?? effectItem.effectWalkSounds;
        effectItem.effectCloseup = effectCloseUp ?? effectItem.effectCloseup;
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
