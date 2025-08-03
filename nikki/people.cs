using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class people {
#nullable enable
    public RectTransform? parentBlock;
    public closeUp? peopleCloseUp;
    public GameObject peopleCloseUpInGame;
    public GameObject peopleCloseUpFrame;
    public GameObject peopleCloseUpBackGround;
    public string peopleName;
    public GameObject nameTextObject;
    public string recommend;
    public GameObject recommendTextObject;
    public bool useHp = false;
    public int hp = 1;
    public int maxHp = 1;
    public GameObject hpTextObject;
    public bool useMp = false;
    public int mp = 0;
    public int maxMp = 0;
    public GameObject mpTextObject;
    public bool useExp = false;
    private long exp = 0;
    public long Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
            if (exp >= levelUpExp)
            {
                levelUp(this);
            }
        }
    }
    public long levelUpExp = 0;
    public GameObject expTextObject;
    public bool useLevel = false;
    public int level = 0;
    public int maxLevel = 0;
    public GameObject levelTextObject;
    public string hpUnit;
    public string mpUnit;
    public string expUnit;
    public string levelUnit;
#nullable disable
    public static void levelUp(people people)
    {
        while (people.exp >= people.levelUpExp)
        {
            long? _levelUpExp = getlevelUpExp(people.level);
            if (people.useLevel && null != _levelUpExp)
            {
                //升级
                people.level++;
                people.levelUpExp = _levelUpExp.Value;
                //发出升级讯息
                levelUpHint();
            }
        }
    }
    public static void levelUpHint()
    {

    }
    public static long? getlevelUpExp(int level)
    {
        long exp = 0;
        while (0 < level)
        {
            exp += (long)(50 + Mathf.Pow(1.095f, level) * 16.18);
            level--;
        }
        return exp;
    }
    void setOther()
    {
        Texture2D blackTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        blackTexture.SetPixel(0, 0, Color.black);
        Sprite blackSprite = Sprite.Create(blackTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        Texture2D emptyTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        emptyTexture.SetPixel(0, 0, new Color(0, 0, 0, 0));
        Sprite emptySprite = Sprite.Create(emptyTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        you.UIinit(ref peopleCloseUpBackGround, "peopleCloseUpBackGround", parentBlock.GetComponent<RectTransform>().localPosition, new Vector2(parentBlock.GetComponent<RectTransform>().sizeDelta.y, parentBlock.GetComponent<RectTransform>().sizeDelta.y), parentBlock.transform, 0, 1);
        peopleCloseUpBackGround.AddComponent<Image>().sprite = (null != peopleCloseUp?.peopleCloseUpBackGround ? peopleCloseUp.peopleCloseUpBackGround : blackSprite);
        peopleCloseUpBackGround.GetComponent<Image>().color *= new Color(1, 1, 1, 0);
        you.UIinit(ref peopleCloseUpInGame, "peopleCloseUpInGame", parentBlock.GetComponent<RectTransform>().localPosition, new Vector2(parentBlock.GetComponent<RectTransform>().sizeDelta.y, parentBlock.GetComponent<RectTransform>().sizeDelta.y), parentBlock.transform, 0, 1);
        peopleCloseUpInGame.AddComponent<Image>().sprite = (null != peopleCloseUp?.peopleCloseUp ? peopleCloseUp.peopleCloseUp : emptySprite);
        peopleCloseUpInGame.GetComponent<Image>().color *= new Color(1, 1, 1, 0);
        you.UIinit(ref peopleCloseUpFrame, "peopleCloseUpFrame", parentBlock.GetComponent<RectTransform>().localPosition, new Vector2(parentBlock.GetComponent<RectTransform>().sizeDelta.y, parentBlock.GetComponent<RectTransform>().sizeDelta.y), parentBlock.transform, 0, 1);
        peopleCloseUpFrame.AddComponent<Image>().sprite = (null != peopleCloseUp?.peopleCloseUpFrame ? peopleCloseUp.peopleCloseUpFrame : blackSprite);
        peopleCloseUpFrame.GetComponent<Image>().color *= new Color(1, 1, 1, 0);
        Vector2 textsizeDelta = new Vector2((parentBlock.GetComponent<RectTransform>().sizeDelta.x - parentBlock.GetComponent<RectTransform>().sizeDelta.y) / 2, parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3);
        you.UIinit(ref nameTextObject, "nameText", parentBlock.GetComponent<RectTransform>().localPosition + new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y, 0), textsizeDelta, parentBlock.transform);
        you.textInit(ref nameTextObject, peopleName, TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass.normal);
        you.UIinit(ref recommendTextObject, "recommendText", parentBlock.GetComponent<RectTransform>().localPosition + new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y + (parentBlock.GetComponent<RectTransform>().sizeDelta.x - parentBlock.GetComponent<RectTransform>().sizeDelta.y) / 2, 0), textsizeDelta, parentBlock.gameObject.transform);
        you.textInit(ref recommendTextObject, recommend, TextAnchor.MiddleRight, MenuTheme.menuTextColorClass.normal);
        you.UIinit(ref levelTextObject, "levelText", parentBlock.GetComponent<RectTransform>().localPosition + new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y, -parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3), textsizeDelta, parentBlock.transform);
        if (useLevel)
        {
            you.textInit(ref levelTextObject, "<color=#" + you.myMenu.menuTextHighlightColor.ToHexString().Substring(0, 6) + "00>" + levelUnit + "</color>" + level, TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass.normal);
        }
        you.UIinit(ref hpTextObject, "hpText", parentBlock.GetComponent<RectTransform>().localPosition + new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y + (parentBlock.GetComponent<RectTransform>().sizeDelta.x - parentBlock.GetComponent<RectTransform>().sizeDelta.y) / 2, -parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3), textsizeDelta, parentBlock.transform);
        if (useHp)
        {
            you.textInit(ref hpTextObject, "<color=#" + you.myMenu.menuTextHighlightColor.ToHexString().Substring(0, 6) + "00>" + hpUnit + "</color>" + hp + "/" + maxHp, TextAnchor.MiddleRight, MenuTheme.menuTextColorClass.normal);
        }
        you.UIinit(ref mpTextObject, "mpText", parentBlock.GetComponent<RectTransform>().localPosition + new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y + (parentBlock.GetComponent<RectTransform>().sizeDelta.x - parentBlock.GetComponent<RectTransform>().sizeDelta.y) / 2, -parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3 * 2), textsizeDelta, parentBlock.transform);
        if (useMp)
        {
            you.textInit(ref mpTextObject, "<color=#" + you.myMenu.menuTextHighlightColor.ToHexString().Substring(0, 6) + "00>" + mpUnit + "</color>" + mp + "/" + maxMp, TextAnchor.MiddleRight, MenuTheme.menuTextColorClass.normal);
        }
        you.UIinit(ref expTextObject, "tpText", parentBlock.GetComponent<RectTransform>().localPosition + new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y, -parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3 * 2), textsizeDelta, parentBlock.transform);
        if (useExp)
        {
            you.textInit(ref expTextObject, "<color=#" + you.myMenu.menuTextHighlightColor.ToHexString().Substring(0, 6) + "00>" + expUnit + "</color>" + exp + "/" + levelUpExp, TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass.normal);
        }
    }
    

    public people(string name, string recommend, string levelUnit = "", string hpUnit = "", string mpUnit = "", string expUnit = "", bool? useHp = null, int hp = 0, int maxHp = 0, bool? useMp = null, int mp = 0, int maxMp = 0, bool? useExp = null, int exp = 0, bool? useLevel = null, int level = 0, int maxLevel = 0)
    {
        peopleCloseUp = effectItem.effectCloseup?[0];
        peopleName = name;
        this.recommend = recommend;
        this.useHp = useHp ?? Game.useHealth;
        this.hp = hp;
        this.maxHp = maxHp;
        this.useMp = useMp ?? Game.useMp;
        this.mp = mp;
        this.maxMp = maxMp;
        this.useExp = useExp ?? Game.useExp;
        this.exp = exp;
        this.levelUpExp = getlevelUpExp(0).Value;
        this.useLevel = useLevel ?? Game.useLevel;
        this.level = level;
        this.maxLevel = maxLevel;
        this.hpUnit = hpUnit ?? Game.hpUnit;
        this.mpUnit = mpUnit ?? Game.mpUnit;
        this.expUnit = expUnit ?? Game.expUnit;
        this.levelUnit = levelUnit ?? Game.levelUnit;
        int i = you.yourTeam.Count;
        parentBlock = new GameObject("parentBlock").AddComponent<RectTransform>();
        parentBlock.pivot = new Vector2(0, 1);
        parentBlock.transform.SetParent(you.You.YouMenuRectTransform, false);
        parentBlock.GetComponent<RectTransform>().localPosition = new Vector2(0, -90 * i);
        parentBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(you.You.YouMenuRectTransform.sizeDelta.x, 90);
        setOther();
        you.yourTeam.Add(this);
    }
    public people(string name, string recommend, RectTransform parentBlock, string levelUnit = "", string hpUnit = "", string mpUnit = "", string tpUnit = "", bool? useHp = null, int hp = 0, int maxHp = 0, bool? useMp = null, int mp = 0, int maxMp = 0, bool? useExp = null, int exp = 0, bool? useLevel = null, int level = 0, int maxLevel = 0)
    {
        peopleCloseUp = effectItem.effectCloseup?[0];
        this.peopleName = name;
        this.recommend = recommend;
        this.useHp = useHp ?? Game.useHealth;
        this.hp = hp;
        this.maxHp = maxHp;
        this.useMp = useMp ?? Game.useMp;
        this.mp = mp;
        this.maxMp = maxMp;
        this.useExp = useExp ?? Game.useExp;
        this.exp = exp;
        this.levelUpExp = getlevelUpExp(0).Value;
        this.useLevel = useLevel ?? Game.useLevel;
        this.level = level;
        this.maxLevel = maxLevel;
        this.hpUnit = hpUnit ?? Game.hpUnit;
        this.mpUnit = mpUnit ?? Game.mpUnit;
        this.expUnit = expUnit ?? Game.expUnit;
        this.levelUnit = levelUnit ?? Game.levelUnit;
        this.parentBlock = new GameObject("parentBlock").AddComponent<RectTransform>();
        this.parentBlock = parentBlock;
        setOther();
        you.yourTeam.Add(this);
    }
    
    public static void swapPeople(int indexI, int indexIa)
    {
        people temp = you.yourTeam[indexI];
        you.yourTeam[indexI] = you.yourTeam[indexIa];
        you.yourTeam[indexIa] = temp;
        RectTransform tempRect = you.yourTeam[indexIa].parentBlock;
        you.yourTeam[indexIa].parentBlock = you.yourTeam[indexI].parentBlock;
        you.yourTeam[indexI].parentBlock = tempRect;
    }
    
    public static void addPeoplesColor(Color c)
    {
        string colorStr = "";
        Hashtable HexToDec = new Hashtable { { '0', 0 }, { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 }, { '9', 9 }, { 'a', 10 }, { 'b', 11 }, { 'c', 12 }, { 'd', 13 }, { 'e', 14 }, { 'f', 15 } };
        string[] DecToHex = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
        Color color2 = new Color();
        for (int i = 0; i < you.yourTeam.Count; i++)
        {
            if (you.yourTeam[i].useHp)
            {
                colorStr = Regex.Match(you.yourTeam[i].hpTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}").Value.Substring(6, 2);
            }
            else if (you.yourTeam[i].useMp)
            {
                colorStr = Regex.Match(you.yourTeam[i].mpTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}").Value.Substring(6, 2);
            }
            else if (you.yourTeam[i].useLevel)
            {
                colorStr = Regex.Match(you.yourTeam[i].levelTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}").Value.Substring(6, 2);
            }
            else if (you.yourTeam[i].useExp)
            {
                colorStr = Regex.Match(you.yourTeam[i].expTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}").Value.Substring(6, 2);
            }
            if (you.yourTeam[i].useHp || you.yourTeam[i].useMp || you.yourTeam[i].useExp || you.yourTeam[i].useLevel)
            {
                color2 = you.myMenu.menuTextHighlightColor * new Color(1, 1, 1, 0) + new Color(0, 0, 0, ((int)HexToDec[colorStr[0]] * 16 + (int)HexToDec[colorStr[1]]) / 255.0f);
            }
            you.yourTeam[i].peopleCloseUpInGame.GetComponent<Image>().color += c;
            you.yourTeam[i].peopleCloseUpFrame.GetComponent<Image>().color += c;
            you.yourTeam[i].peopleCloseUpBackGround.GetComponent<Image>().color += c;
            color2 += c;
            if (null != you.yourTeam[i].hpTextObject.GetComponent<Text>())
            {
                you.yourTeam[i].hpTextObject.GetComponent<Text>().color += c;
                Regex.Replace(you.yourTeam[i].hpTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", "#" + color2.ToHexString());
            }
            if (null != you.yourTeam[i].mpTextObject.GetComponent<Text>())
            {
                you.yourTeam[i].mpTextObject.GetComponent<Text>().color += c;
                Regex.Replace(you.yourTeam[i].mpTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", "#" + color2.ToHexString());
            }
            if (null != you.yourTeam[i].expTextObject.GetComponent<Text>())
            {
                you.yourTeam[i].expTextObject.GetComponent<Text>().color += c;
                Regex.Replace(you.yourTeam[i].expTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", "#" + color2.ToHexString());
            }
            if (null != you.yourTeam[i].levelTextObject.GetComponent<Text>())
            {
                you.yourTeam[i].levelTextObject.GetComponent<Text>().color += c;
                Regex.Replace(you.yourTeam[i].levelTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", "#" + color2.ToHexString());
            }
            you.yourTeam[i].nameTextObject.GetComponent<Text>().color += c;
            you.yourTeam[i].recommendTextObject.GetComponent<Text>().color += c;
        }
    }

    public static void setPeoplesColor(Color c)
    {
        for (int i = 0; i < you.yourTeam.Count; i++)
        {
            you.yourTeam[i].peopleCloseUpInGame.GetComponent<Image>().color = you.yourTeam[i].peopleCloseUpFrame.GetComponent<Image>().color = you.yourTeam[i].peopleCloseUpBackGround.GetComponent<Image>().color = c;
            if (null != you.yourTeam[i].hpTextObject.GetComponent<Text>())
            {
                you.yourTeam[i].hpTextObject.GetComponent<Text>().color = c;
                Regex.Replace(you.yourTeam[i].hpTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", "#" + c.ToHexString());
            }
            if (null != you.yourTeam[i].mpTextObject.GetComponent<Text>())
            {
                you.yourTeam[i].mpTextObject.GetComponent<Text>().color = c;
                Regex.Replace(you.yourTeam[i].mpTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", "#" + c.ToHexString());
            }
            if (null != you.yourTeam[i].expTextObject.GetComponent<Text>())
            {
                you.yourTeam[i].expTextObject.GetComponent<Text>().color = c;
                Regex.Replace(you.yourTeam[i].expTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", "#" + c.ToHexString());
            }
            if (null != you.yourTeam[i].levelTextObject.GetComponent<Text>())
            {
                you.yourTeam[i].levelTextObject.GetComponent<Text>().color = c;
                Regex.Replace(you.yourTeam[i].levelTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", "#" + c.ToHexString());
            }
            you.yourTeam[i].nameTextObject.GetComponent<Text>().color = you.yourTeam[i].recommendTextObject.GetComponent<Text>().color = c;
        }
    }
}
