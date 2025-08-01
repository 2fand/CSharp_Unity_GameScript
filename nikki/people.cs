using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class people {
#nullable enable
    public RectTransform parentBlock;
    public closeUp? peopleCloseUp;
    public string name;
    public GameObject nameTextObject;
    public string recommend;
    public GameObject recommendTextObject;
    public bool useHealth = false;
    public int health = 1;
    public int maxHealth = 1;
    public GameObject hpTextObject;
    public bool useMp = false;
    public int mp = 0;
    public int maxMp = 0;
    public GameObject mpTextObject;
    public bool useTp = false;
    public int tp = 0;
    public int maxTp = 0;
    public GameObject tpTextObject;
    public bool useLevel = false;
    public int level = 0;
    public int maxLevel = 0;
    public GameObject levelTextObject;
    public string healthUnit;
    public string mpUnit;
    public string tpUnit;
    public string levelUnit;
    public people(string name, string recommend, RectTransform parentBlock, string levelUnit = "", string healthUnit = "", string mpUnit = "", string tpUnit = "", bool? useHealth = null, int health = 0, int maxHealth = 0, bool? useMp = null, int mp = 0, int maxMp = 0, bool? useTp = null, int tp = 0, int maxTp = 0, bool? useLevel = null, int level = 0, int maxLevel = 0)
    {
        peopleCloseUp = effectItem.effectCloseup?[0];
        this.name = name;
        this.recommend = recommend;
        this.useHealth = useHealth ?? Game.useHealth;
        this.health = health;
        this.maxHealth = maxHealth;
        this.useMp = useMp ?? Game.useMp;
        this.mp = mp;
        this.maxMp = maxMp;
        this.useTp = useTp ?? Game.useTp;
        this.tp = tp;
        this.maxTp = maxTp;
        this.useLevel = useLevel ?? Game.useLevel;
        this.level = level;
        this.maxLevel = maxLevel;
        this.healthUnit = healthUnit ?? Game.hpUnit;
        this.mpUnit = mpUnit ?? Game.mpUnit;
        this.tpUnit = tpUnit ?? Game.tpUnit;
        this.levelUnit = levelUnit ?? Game.levelUnit;
        this.parentBlock = parentBlock;
    }
}
