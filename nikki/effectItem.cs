using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class effectItem : item
{
    effect effect;
    you you;
    public override void use()
    {
        trigger.funcs.Add(you.changeEffect(effect));
    }
    public effectItem(effect effect, you you, bool isHide = false)
    {
        this.effect = effect;
        this.name = you.effectName[(int)effect];
        this.canUse = true;
        this.you = you;
        this.isHide = isHide;
    }
}
