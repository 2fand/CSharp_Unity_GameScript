using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class effectItem : item
{
    public effect effect;
    public static string[] effectRecommendTexts = { "原本之物", "同根之害", "空象虚门" };
    public static AudioClip[] effectWalkSounds;
    public static AudioClip effectEqiupSound;
    public static AudioClip effectCancelEqiupSound;
    public static Material[] screens;
    public static GameObject[] effects;
    public override void use()
    {
        if (effect == you.nowEffect)
        {
            trigger.funcs.Add(you.You.changeEffect(effect.none));
        }
        else
        {
            trigger.funcs.Add(you.You.changeEffect(effect));
        }
    }
    public effectItem(effect effect, bool canUse = true, bool isHide = false)
    {
        this.effect = effect;
        name = you.effectName[(int)effect];
        recommendText = effectRecommendTexts[(int)effect];
        this.canUse = canUse;
        this.isHide = isHide;
    }
}
