using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class effectItem : item
{
    public effect effect;
    public static int effectCount = 18;
    public static string[] effectNames = { "", "天使", "锁门" };
    public static string[] effectRecommendTexts = { "原本之物", "同根之害", "空象虚门" };
    public static AudioClip[] effectWalkSounds;
    public static AudioClip effectEqiupSound;
    public static AudioClip effectCancelEqiupSound;
    public static closeUp[] effectCloseup;
    public static Material[] screens;
    public static GameObject[] effects;
    public override void use()
    {
        if (effect == you.nowEffect)
        {
            trigger.funcs.Add(you.You.changeEffect(0));
        }
        else
        {
            trigger.funcs.Add(you.You.changeEffect(effect));
        }
    }
    public effectItem()
    {
        this.effect = 0;
        name = effectNames[0];
        recommendText = effectRecommendTexts[0];
        this.canUse = false;
        this.isHide = false;
    }
    public effectItem(effect effect, bool canUse = true, bool isHide = false)
    {
        this.effect = effect;
        name = effectNames[(int)effect];
        recommendText = effectRecommendTexts[(int)effect];
        this.canUse = canUse;
        this.isHide = isHide;
    }
}

public class none : effectUse
{
    public static Action useAct = () => { new none().use(); };
    public override void use()
    {
        you.speed = 1;
        You.transform.position = new Vector3(You.transform.position.x, You.High, You.transform.position.z);
        You.GetComponent<Float>().enabled = false;
        You.GetComponent<changeColor>().enabled = false;
    }
}

public class angel : effectUse
{
    public static Action useAct = () => { new angel().use(); };
    public override void use()
    {
        you.speed = 2;
        You.transform.position = new Vector3(You.transform.position.x, You.High + 2f, You.transform.position.z);
        You.GetComponent<Float>().enabled = true;
        You.GetComponent<changeColor>().enabled = true;
    }
}