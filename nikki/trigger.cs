using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class trigger : MonoBehaviour
{
    public change.enterMode enterModesView;
    public change.exitMode exitModesView;
    public int x = 0;
    public int y = 0;
    public bool ChangeTransform = true;
    public GameObject wallPrefab;
    private map m;
    public string mpath;
    public int extendLeft = 0;
    public int extendRight = 0;
    public int extendUp = 0;
    public int extendDown = 0;
    public string[] commands;//目前只用tele命令 talk等命令之后
    public AudioClip[] sounds;
    public you u;
    private change.enterMode? enterMode;
    private change.exitMode? exitMode;
    public Image image;
    private string? worldName;
    private int? teleX;
    private int? teleY;
    private float? teleHigh;
    private you.wasd? front;
    private int? closeSoundIndex;
    private int? teleSoundIndex;
    private bool isEnd = true;
    private Hashtable stringModes = new Hashtable { { "show", change.enterMode.show }, { "fadein", change.enterMode.fadein }, { "hide", change.exitMode.hide }, { "fadeout", change.exitMode.fadeout } };
    private void init()
    {
        teleX = teleY = closeSoundIndex = teleSoundIndex = null;
        worldName = null;
        teleHigh = null;
        front = null;
        enterMode = null;
        exitMode = null;
    }

    void Start()
    {
        m = GameObject.Find(mpath).GetComponent<map>();
        if (ChangeTransform)
        {
            //根据地图xyz轴进行transform计算
            transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), transform.position.y, m.maxY - m.widthY / m.y * (0.5f + y));
        }
        if (null == wallPrefab)
        {
            GameObject emptyWall = new GameObject("emptyWall");
            wallPrefab = emptyWall;
        }
        Vector3[] clones = { new Vector3(-m.heightX, 0, m.widthY), new Vector3(0, 0, m.widthY), new Vector3(m.heightX, 0, m.widthY), new Vector3(-m.heightX, 0, 0), new Vector3(m.heightX, 0, 0), new Vector3(-m.heightX, 0, -m.widthY), new Vector3(0, 0, -m.widthY), new Vector3(m.heightX, 0, -m.widthY) };
        foreach (Vector3 v in clones)
        {
            if (!m.horizontalIsCycle && v.x != 0 || !m.verticalIsCycle && v.z != 0)
            {
                continue;
            }
            wallPrefab.transform.localPosition = v + transform.position;
            wallPrefab.transform.rotation = transform.rotation;
            Instantiate(wallPrefab, transform, true);
        }
    }

    IEnumerator runCommand()
    {
        isEnd = false;
        List<int> delimiterIndexs = new List<int> { -1 };
        string value = "";
        string commandName = "";
        bool isCount = true;
        for (int commandI = 0; commandI < commands.Length; commandI++)
        {
            init();
            delimiterIndexs.Clear();
            delimiterIndexs.Add(-1);
            //参数
            for (int i = 0; i < commands[commandI].Length; i++)
            {
                if (isCount && ' ' == commands[commandI][i])
                {
                    delimiterIndexs.Add(i);
                }
                else if('\"' == commands[commandI][i])
                {
                    isCount = !isCount;
                }
            }
            if (!isCount)
            {
                goto errorEnd;
            }
            delimiterIndexs.Add(commands[commandI].Length);
            for (int i = 0; i < delimiterIndexs.Count - 1; i++)
            {
                value = commands[commandI].Substring(delimiterIndexs[i] + 1, delimiterIndexs[i + 1] - delimiterIndexs[i] - 1);
                if ('\"' == value[0])
                {
                    value = value.Substring(1, value.Length - 2);
                }
                if (0 == i)
                {
                    commandName = value;
                    if ("tele" != commandName)
                    {
                        goto errorEnd;
                    }
                }
                else if (0 != i)
                {
                    switch (commandName)
                    {
                        case "tele":
                            switch (i)
                            {
                                case 1:
                                    exitMode = stringModes.ContainsKey(value.ToLower()) ? (change.exitMode)stringModes[value] : change.exitMode.none;
                                    break;
                                case 2:
                                    enterMode = stringModes.ContainsKey(value.ToLower()) ? (change.enterMode)stringModes[value] : change.enterMode.none;
                                    break;
                                case 3:
                                    worldName = value;
                                    break;
                                case 4:
                                    teleX = int.Parse(value);
                                    break;
                                case 5:
                                    teleY = int.Parse(value);
                                    break;
                                case 6:
                                    teleHigh = float.Parse(value);
                                    break;
                                case 7:
                                    switch (value)
                                    {
                                        case "W":
                                        case "w":
                                            front = you.wasd.w;
                                            break;
                                        case "A":
                                        case "a":
                                            front = you.wasd.a;
                                            break;
                                        case "S":
                                        case "s":
                                            front = you.wasd.s;
                                            break;
                                        case "D":
                                        case "d":
                                            front = you.wasd.d;
                                            break;
                                        case "U":
                                        case "u":
                                            front = you.front;
                                            break;
                                        default:
                                            front = (you.wasd)int.Parse(value);
                                            break;
                                    }
                                    break;
                                case 8:
                                    closeSoundIndex = int.Parse(value);
                                    break;
                                case 9:
                                    teleSoundIndex = int.Parse(value);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            goto normalEnd;
                    }
                }
            }
        normalEnd:;
            switch (commandName)
            {
                case "tele":
                    if (you.teleIsEnd)
                    {
                        StartCoroutine(you.tele(exitMode ?? change.exitMode.hide, enterMode ?? change.enterMode.show, image, worldName ?? "nexus", teleX ?? 0, teleY ?? 0, teleHigh ?? 0, front ?? you.wasd.s, sounds[closeSoundIndex ?? 0] ?? sounds[0], sounds[teleSoundIndex ?? 0] ?? sounds[0]));
                    }
                    break;
                default:
                    break;
            }
        errorEnd:;
        }
        isEnd = true;
        yield return null;
    }
    void Update()
    {
        if (isEnd && u.x >= x - extendLeft && u.x <= x + extendRight && u.y >= y - extendUp && u.y <= y + extendDown && you.isEndMove)
        {
            StartCoroutine(runCommand());
        }
    }
}
