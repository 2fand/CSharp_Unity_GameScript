using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeUp
{
#nullable enable
    public Sprite? peopleCloseUp = null;
    public Sprite? peopleCloseUpFrame = null;
    public Sprite? peopleCloseUpBackGround = null;
    public closeUp()
    {
        
    }
    public closeUp(Sprite? peopleCloseUp = null, Sprite? peopleCloseUpFrame = null, Sprite? peopleCloseUpBackGround = null)
    {
        this.peopleCloseUp = peopleCloseUp;
        this.peopleCloseUpFrame = peopleCloseUpFrame;
        this.peopleCloseUpBackGround = peopleCloseUpBackGround;
    }
}
