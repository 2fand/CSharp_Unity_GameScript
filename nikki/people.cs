using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class people {
#nullable enable
    private RectTransform? parentBlock;
    public RectTransform? ParentBlock
    {
        get
        {
            return parentBlock;
        }
        set
        {
            parentBlock = value;
            //last_block = parentBlock?.rect ?? new Rect();
        }
    }
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
    //public Rect last_block;
#nullable disable
    public static void levelUp(people people)
    {
        while (people.exp >= people.levelUpExp)
        {
            long? _levelUpExp = getlevelUpExp(people.level);
            if (people.useLevel && null != _levelUpExp)
            {
                if (people.level == people.maxLevel)
                {
                    return;
                }
                //升级
                people.level++;
                people.levelUpExp = _levelUpExp.Value;
                //发出升级讯息
                levelUpHint();
            }
        }
    }
    public void addHp(int addHp)
    {
        hp = Mathf.Max(addHp + hp, maxHp);
    }
    public void addMp(int addMp)
    {
        hp = Mathf.Max(addMp + mp, maxMp);
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
#nullable enable
    void setOther(RectTransform? block = null)
#nullable disable
    {
        if (null != parentBlock)
        {
            parentBlock = new GameObject("parentBlock").AddComponent<RectTransform>();
            parentBlock = block;
        }
        else
        {
            parentBlock = new GameObject("parentBlock" + peopleName).AddComponent<RectTransform>();
            parentBlock.pivot = new Vector2(0, 1);
            parentBlock.transform.SetParent(you.You.YouMenuRectTransform, false);
            parentBlock.GetComponent<RectTransform>().localPosition = new Vector2(0, -90 * you.yourTeam.Count);
            parentBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(you.You.YouMenuRectTransform.sizeDelta.x, 90);
        }
        if (null == parentBlock.gameObject.transform.parent.GetComponent<Mask>())
        {
            this.parentBlock.gameObject.AddComponent<Mask>().showMaskGraphic = false;
        }
        if (null == parentBlock.gameObject.transform.parent.GetComponent<Image>())
        {
            this.parentBlock.gameObject.AddComponent<Image>().color = Color.white;
        }
        Texture2D blackTexture = new Texture2D(1, 1);
        blackTexture.SetPixel(0, 0, Color.black);
        blackTexture.Apply();
        Sprite blackSprite = Sprite.Create(blackTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        Sprite emptySprite = Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        you.UIinit(ref peopleCloseUpBackGround, "peopleCloseUpBackGround", Vector2.zero, new Vector2(parentBlock.GetComponent<RectTransform>().sizeDelta.y, parentBlock.GetComponent<RectTransform>().sizeDelta.y), parentBlock.transform, 0, 1);
        peopleCloseUpBackGround.AddComponent<Image>().sprite = (null != peopleCloseUp?.peopleCloseUpBackGround ? peopleCloseUp.peopleCloseUpBackGround : blackSprite);
        peopleCloseUpBackGround.GetComponent<Image>().color *= new Color(1, 1, 1, 0);
        peopleCloseUpBackGround.GetComponent<Image>().type = Image.Type.Sliced;
        you.UIinit(ref peopleCloseUpInGame, "peopleCloseUpInGame", Vector2.zero, new Vector2(parentBlock.GetComponent<RectTransform>().sizeDelta.y, parentBlock.GetComponent<RectTransform>().sizeDelta.y), parentBlock.transform, 0, 1);
        peopleCloseUpInGame.AddComponent<Image>().sprite = (null != peopleCloseUp?.peopleCloseUp ? peopleCloseUp.peopleCloseUp : emptySprite);
        peopleCloseUpInGame.GetComponent<Image>().color *= new Color(1, 1, 1, 0);
        peopleCloseUpInGame.GetComponent<Image>().type = Image.Type.Sliced;
        you.UIinit(ref peopleCloseUpFrame, "peopleCloseUpFrame", Vector2.zero, new Vector2(parentBlock.GetComponent<RectTransform>().sizeDelta.y, parentBlock.GetComponent<RectTransform>().sizeDelta.y), parentBlock.transform, 0, 1);
        peopleCloseUpFrame.AddComponent<Image>().sprite = (null != peopleCloseUp?.peopleCloseUpFrame ? peopleCloseUp.peopleCloseUpFrame : emptySprite);
        peopleCloseUpFrame.GetComponent<Image>().color *= new Color(1, 1, 1, 0);
        peopleCloseUpFrame.GetComponent<Image>().type = Image.Type.Sliced;
        Vector2 textsizeDelta = new Vector2((parentBlock.GetComponent<RectTransform>().sizeDelta.x - parentBlock.GetComponent<RectTransform>().sizeDelta.y) / 2, parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3);
        you.UIinit(ref nameTextObject, "nameText", new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y, 0), textsizeDelta, parentBlock.transform, 0, 1);
        you.textInit(ref nameTextObject, peopleName, TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass.normal);
        you.UIinit(ref recommendTextObject, "recommendText", new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y + (parentBlock.GetComponent<RectTransform>().sizeDelta.x - parentBlock.GetComponent<RectTransform>().sizeDelta.y) / 2, 0), textsizeDelta, parentBlock.gameObject.transform, 0, 1);
        you.textInit(ref recommendTextObject, recommend, TextAnchor.MiddleRight, MenuTheme.menuTextColorClass.normal);
        you.UIinit(ref levelTextObject, "levelText", new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y, -parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3), textsizeDelta, parentBlock.transform, 0, 1);
        you.textInit(ref levelTextObject, useLevel ? "<color=#" + you.myMenu.menuTextHighlightColor.ToHexString().Substring(0, 6) + "00>" + levelUnit + "</color> " + level : "", TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass.normal);
        you.UIinit(ref hpTextObject, "hpText", new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y + (parentBlock.GetComponent<RectTransform>().sizeDelta.x - parentBlock.GetComponent<RectTransform>().sizeDelta.y) / 2, -parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3), textsizeDelta, parentBlock.transform, 0, 1);
        you.textInit(ref hpTextObject, useHp ? "<color=#" + you.myMenu.menuTextHighlightColor.ToHexString().Substring(0, 6) + "00>" + hpUnit + "</color> " + hp + "/" + maxHp : "", TextAnchor.MiddleRight, MenuTheme.menuTextColorClass.normal);
        you.UIinit(ref mpTextObject, "mpText", new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y + (parentBlock.GetComponent<RectTransform>().sizeDelta.x - parentBlock.GetComponent<RectTransform>().sizeDelta.y) / 2, -parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3 * 2), textsizeDelta, parentBlock.transform, 0, 1);
        you.textInit(ref mpTextObject, useMp ? "<color=#" + you.myMenu.menuTextHighlightColor.ToHexString().Substring(0, 6) + "00>" + mpUnit + "</color> " + mp + "/" + maxMp : "", TextAnchor.MiddleRight, MenuTheme.menuTextColorClass.normal);
        you.UIinit(ref expTextObject, "tpText", new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y, -parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3 * 2), textsizeDelta, parentBlock.transform, 0, 1);
        you.textInit(ref expTextObject, useExp ? "<color=#" + you.myMenu.menuTextHighlightColor.ToHexString().Substring(0, 6) + "00>" + expUnit + "</color> " + exp + "/" + levelUpExp : "", TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass.normal);
        parentBlock.transform.SetAsLastSibling();
    }

#nullable enable
    public void resetOther(int i, RectTransform? block = null)
#nullable disable
    {
        if(!(null == nameTextObject && null == recommendTextObject && null == levelTextObject && null == hpTextObject && null == mpTextObject && null == expTextObject && null == peopleCloseUpBackGround && null == peopleCloseUpFrame && null == peopleCloseUpInGame && null == parentBlock))
        {
            return; 
        }
        if (null != parentBlock)
        {
            parentBlock = new GameObject("parentBlock").AddComponent<RectTransform>();
            parentBlock = block;
        }
        else
        {
            parentBlock = new GameObject("parentBlock" + peopleName).AddComponent<RectTransform>();
            parentBlock.pivot = new Vector2(0, 1);
            parentBlock.transform.SetParent(you.You.YouMenuRectTransform, false);
            parentBlock.GetComponent<RectTransform>().localPosition = new Vector2(0, -90 * i);
            parentBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(you.You.YouMenuRectTransform.sizeDelta.x, 90);
        }
        if (null == parentBlock.gameObject.transform.parent.GetComponent<Mask>())
        {
            this.parentBlock.gameObject.AddComponent<Mask>().showMaskGraphic = false;
        }
        if (null == parentBlock.gameObject.transform.parent.GetComponent<Image>())
        {
            this.parentBlock.gameObject.AddComponent<Image>().color = Color.white;
        }
        Texture2D blackTexture = new Texture2D(1, 1);
        blackTexture.SetPixel(0, 0, Color.black);
        blackTexture.Apply();
        Sprite blackSprite = Sprite.Create(blackTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        Sprite emptySprite = Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        you.UIinit(ref peopleCloseUpBackGround, "peopleCloseUpBackGround", Vector2.zero, new Vector2(parentBlock.GetComponent<RectTransform>().sizeDelta.y, parentBlock.GetComponent<RectTransform>().sizeDelta.y), parentBlock.transform, 0, 1);
        peopleCloseUpBackGround.AddComponent<Image>().sprite = (null != peopleCloseUp?.peopleCloseUpBackGround ? peopleCloseUp.peopleCloseUpBackGround : blackSprite);
        peopleCloseUpBackGround.GetComponent<Image>().color *= new Color(1, 1, 1, 0);
        peopleCloseUpBackGround.GetComponent<Image>().type = Image.Type.Sliced;
        you.UIinit(ref peopleCloseUpInGame, "peopleCloseUpInGame", Vector2.zero, new Vector2(parentBlock.GetComponent<RectTransform>().sizeDelta.y, parentBlock.GetComponent<RectTransform>().sizeDelta.y), parentBlock.transform, 0, 1);
        peopleCloseUpInGame.AddComponent<Image>().sprite = (null != peopleCloseUp?.peopleCloseUp ? peopleCloseUp.peopleCloseUp : emptySprite);
        peopleCloseUpInGame.GetComponent<Image>().color *= new Color(1, 1, 1, 0);
        peopleCloseUpInGame.GetComponent<Image>().type = Image.Type.Sliced;
        you.UIinit(ref peopleCloseUpFrame, "peopleCloseUpFrame", Vector2.zero, new Vector2(parentBlock.GetComponent<RectTransform>().sizeDelta.y, parentBlock.GetComponent<RectTransform>().sizeDelta.y), parentBlock.transform, 0, 1);
        peopleCloseUpFrame.AddComponent<Image>().sprite = (null != peopleCloseUp?.peopleCloseUpFrame ? peopleCloseUp.peopleCloseUpFrame : emptySprite);
        peopleCloseUpFrame.GetComponent<Image>().color *= new Color(1, 1, 1, 0);
        peopleCloseUpFrame.GetComponent<Image>().type = Image.Type.Sliced;
        Vector2 textsizeDelta = new Vector2((parentBlock.GetComponent<RectTransform>().sizeDelta.x - parentBlock.GetComponent<RectTransform>().sizeDelta.y) / 2, parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3);
        you.UIinit(ref nameTextObject, "nameText", new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y, 0), textsizeDelta, parentBlock.transform, 0, 1);
        you.textInit(ref nameTextObject, peopleName, TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass.normal);
        you.UIinit(ref recommendTextObject, "recommendText", new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y + (parentBlock.GetComponent<RectTransform>().sizeDelta.x - parentBlock.GetComponent<RectTransform>().sizeDelta.y) / 2, 0), textsizeDelta, parentBlock.gameObject.transform, 0, 1);
        you.textInit(ref recommendTextObject, recommend, TextAnchor.MiddleRight, MenuTheme.menuTextColorClass.normal);
        you.UIinit(ref levelTextObject, "levelText", new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y, -parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3), textsizeDelta, parentBlock.transform, 0, 1);
        you.textInit(ref levelTextObject, useLevel ? "<color=#" + you.myMenu.menuTextHighlightColor.ToHexString().Substring(0, 6) + "00>" + levelUnit + "</color> " + level : "", TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass.normal);
        you.UIinit(ref hpTextObject, "hpText", new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y + (parentBlock.GetComponent<RectTransform>().sizeDelta.x - parentBlock.GetComponent<RectTransform>().sizeDelta.y) / 2, -parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3), textsizeDelta, parentBlock.transform, 0, 1);
        you.textInit(ref hpTextObject, useHp ? "<color=#" + you.myMenu.menuTextHighlightColor.ToHexString().Substring(0, 6) + "00>" + hpUnit + "</color> " + hp + "/" + maxHp : "", TextAnchor.MiddleRight, MenuTheme.menuTextColorClass.normal);
        you.UIinit(ref mpTextObject, "mpText", new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y + (parentBlock.GetComponent<RectTransform>().sizeDelta.x - parentBlock.GetComponent<RectTransform>().sizeDelta.y) / 2, -parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3 * 2), textsizeDelta, parentBlock.transform, 0, 1);
        you.textInit(ref mpTextObject, useMp ? "<color=#" + you.myMenu.menuTextHighlightColor.ToHexString().Substring(0, 6) + "00>" + mpUnit + "</color> " + mp + "/" + maxMp : "", TextAnchor.MiddleRight, MenuTheme.menuTextColorClass.normal);
        you.UIinit(ref expTextObject, "tpText", new Vector3(parentBlock.GetComponent<RectTransform>().sizeDelta.y, -parentBlock.GetComponent<RectTransform>().sizeDelta.y / 3 * 2), textsizeDelta, parentBlock.transform, 0, 1);
        you.textInit(ref expTextObject, useExp ? "<color=#" + you.myMenu.menuTextHighlightColor.ToHexString().Substring(0, 6) + "00>" + expUnit + "</color> " + exp + "/" + levelUpExp : "", TextAnchor.MiddleLeft, MenuTheme.menuTextColorClass.normal);
        parentBlock.transform.SetAsLastSibling();
    }

    public people(string name, string recommend, closeUp peopleCloseUp,
#nullable enable
        string? levelUnit = null, string? hpUnit = null, string? mpUnit = null, string? expUnit = null, bool? useHp = null, int? hp = null, int? maxHp = null, bool? useMp = null, int? mp = null, int? maxMp = null, bool? useExp = null, int? exp = null, bool? useLevel = null, int? level = null, int? maxLevel = null)
#nullable disable
    {
        this.peopleCloseUp = peopleCloseUp;
        peopleName = name;
        this.recommend = recommend;
        this.useHp = useHp ?? Game.useHealth;
        this.hp = hp ?? Game.defaultHp;
        this.maxHp = maxHp ?? Game.defaultMaxHp;
        this.useMp = useMp ?? Game.useMp;
        this.mp = mp ?? Game.defaultMp;
        this.maxMp = maxMp ?? Game.defaultMaxMp;
        this.useExp = useExp ?? Game.useExp;
        this.exp = exp ?? Game.defaultExp;
        this.levelUpExp = getlevelUpExp(0).Value;
        this.useLevel = useLevel ?? Game.useLevel;
        this.level = level ?? Game.defaultLevel;
        this.maxLevel = maxLevel ?? Game.defaultLevel;
        this.hpUnit = hpUnit ?? Game.hpUnit;
        this.mpUnit = mpUnit ?? Game.mpUnit;
        this.expUnit = expUnit ?? Game.expUnit;
        this.levelUnit = levelUnit ?? Game.levelUnit;
        setOther();
        you.yourTeam.Add(this);
    }
    public people(string name, string recommend, closeUp peopleCloseUp, 
        #nullable enable 
        RectTransform parentBlock, string? levelUnit = null, string? hpUnit = null, string? mpUnit = null, string? expUnit = null, bool? useHp = null, int? hp = null, int? maxHp = null, bool? useMp = null, int? mp = null, int? maxMp = null, bool? useExp = null, int? exp = null, bool? useLevel = null, int? level = null, int? maxLevel = null)
#nullable disable
    {
        this.peopleCloseUp = peopleCloseUp;
        this.peopleName = name;
        this.recommend = recommend;
        this.useHp = useHp ?? Game.useHealth;
        this.hp = hp ?? Game.defaultHp;
        this.maxHp = maxHp ?? Game.defaultMaxHp;
        this.useMp = useMp ?? Game.useMp;
        this.mp = mp ?? Game.defaultMp;
        this.maxMp = maxMp ?? Game.defaultMaxMp;
        this.useExp = useExp ?? Game.useExp;
        this.exp = exp ?? Game.defaultExp;
        this.levelUpExp = getlevelUpExp(0).Value;
        this.useLevel = useLevel ?? Game.useLevel;
        this.level = level ?? Game.defaultLevel;
        this.maxLevel = maxLevel ?? Game.defaultMaxLevel;
        this.hpUnit = hpUnit ?? Game.hpUnit;
        this.mpUnit = mpUnit ?? Game.mpUnit;
        this.expUnit = expUnit ?? Game.expUnit;
        this.levelUnit = levelUnit ?? Game.levelUnit;
        setOther(parentBlock);
        you.yourTeam.Add(this);
    }
    
    public static void swapPeople(int indexI, int indexIa)
    {
        if (indexI >= 0 && indexIa >= 0 && indexI < you.yourTeam.Count && indexIa < you.yourTeam.Count)
        {
            people temp = you.yourTeam[indexI];
            you.yourTeam[indexI] = you.yourTeam[indexIa];
            you.yourTeam[indexIa] = temp;
            Vector2 pos = you.yourTeam[indexI].parentBlock.localPosition;
            you.yourTeam[indexI].parentBlock.localPosition = you.yourTeam[indexIa].parentBlock.localPosition;
            you.yourTeam[indexIa].parentBlock.localPosition = pos;
        }
    }

    public static void delPeople()
    {
        if (0 < you.yourTeam.Count)
        {
            you.yourTeam.RemoveAt(you.yourTeam.Count - 1);
        }
    }

    public static void clearPeople()
    {

        you.yourTeam.Clear();
        
    }

    public static void delPeople(int index)
    {
        Debug.Log(index + "/" + you.yourTeam.Count);
        if (index < you.yourTeam.Count)
        {
            you.yourTeam.RemoveAt(index);
            while (index < you.yourTeam.Count)
            {
                you.yourTeam[index].parentBlock.localPosition += new Vector3(0, 90);
                index++;
            }
        }
    }
    public static void addPeoplesColor(Color c)
    {
        string colorStr = "";
        Hashtable HexToDec = new Hashtable { { '0', 0 }, { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 }, { '9', 9 }, { 'a', 10 }, { 'b', 11 }, { 'c', 12 }, { 'd', 13 }, { 'e', 14 }, { 'f', 15 } };
        string[] DecToHex = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
        Color color2 = new Color();
        for (int i = 0; i < you.yourTeam.Count; i++)
        {
            Debug.Log(you.yourTeam[i].peopleName);
            if (you.yourTeam[i].useHp)
            {
                colorStr = Regex.Match(you.yourTeam[i].hpTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}").Value;
            }
            else if (you.yourTeam[i].useMp)
            {
                colorStr = Regex.Match(you.yourTeam[i].mpTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}").Value;
            }
            else if (you.yourTeam[i].useLevel)
            {
                colorStr = Regex.Match(you.yourTeam[i].levelTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}").Value;
            }
            else if (you.yourTeam[i].useExp)
            {
                colorStr = Regex.Match(you.yourTeam[i].expTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}").Value;
            }
            if (you.yourTeam[i].useHp || you.yourTeam[i].useMp || you.yourTeam[i].useExp || you.yourTeam[i].useLevel)
            {
                color2 = new Color(((int)HexToDec[colorStr.ToLower()[1]] * 16 + (int)HexToDec[colorStr.ToLower()[2]]) / 255.0f, ((int)HexToDec[colorStr.ToLower()[3]] * 16 + (int)HexToDec[colorStr.ToLower()[4]]) / 255.0f, ((int)HexToDec[colorStr.ToLower()[5]] * 16 + (int)HexToDec[colorStr.ToLower()[6]]) / 255.0f, ((int)HexToDec[colorStr.ToLower()[7]] * 16 + (int)HexToDec[colorStr.ToLower()[8]]) / 255.0f);
            }
            you.yourTeam[i].peopleCloseUpInGame.GetComponent<Image>().color += c;
            you.yourTeam[i].peopleCloseUpFrame.GetComponent<Image>().color += c;
            you.yourTeam[i].peopleCloseUpBackGround.GetComponent<Image>().color += c;
            color2 += c;
            if (null != you.yourTeam[i].hpTextObject.GetComponent<Text>())
            {
                you.yourTeam[i].hpTextObject.GetComponent<Text>().color += c;
                you.yourTeam[i].hpTextObject.GetComponent<Text>().text = Regex.Replace(you.yourTeam[i].hpTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", "#" + color2.ToHexString());
            }
            if (null != you.yourTeam[i].mpTextObject.GetComponent<Text>())
            {
                you.yourTeam[i].mpTextObject.GetComponent<Text>().color += c;
                you.yourTeam[i].mpTextObject.GetComponent<Text>().text = Regex.Replace(you.yourTeam[i].mpTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", "#" + color2.ToHexString());
            }
            if (null != you.yourTeam[i].expTextObject.GetComponent<Text>())
            {
                you.yourTeam[i].expTextObject.GetComponent<Text>().color += c;
                you.yourTeam[i].expTextObject.GetComponent<Text>().text = Regex.Replace(you.yourTeam[i].expTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", "#" + color2.ToHexString());
            }
            if (null != you.yourTeam[i].levelTextObject.GetComponent<Text>())
            {
                you.yourTeam[i].levelTextObject.GetComponent<Text>().color += c;
                you.yourTeam[i].levelTextObject.GetComponent<Text>().text = Regex.Replace(you.yourTeam[i].levelTextObject.GetComponent<Text>().text, "#[0-9a-fA-F]{8}", "#" + color2.ToHexString());
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
