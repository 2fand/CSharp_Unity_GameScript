using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionItem : item
{
    public string[] commands;
    public AudioClip[] sounds;
    public override void use()
    {
        trigger.runCommands(commands, sounds, you.You);
    }
    public actionItem(string[] commands, AudioClip[] sounds = null, string name = "default", string recommendText = "", bool canUse = true, bool isHide = false)
    {
        this.name = name;
        this.recommendText = recommendText;
        this.commands = commands;
        this.sounds = sounds;
        this.canUse = canUse;
        this.isHide = isHide;
    }
}
