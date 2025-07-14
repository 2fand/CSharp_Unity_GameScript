using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    public string[] commands;//目前支持tele命令, help命令, #命令, move命令, show命令, hide命令, play命令, turn命令, stop命令, wait命令, use命令 talk等命令之后实现
    public AudioClip[] sounds;
    public you u;
    public UnityEngine.UI.Image image;
    private bool isEnd = true;
    private readonly Hashtable stringModes = new Hashtable { { "show", change.enterMode.show }, { "fadein", change.enterMode.fadein }, { "hide", change.exitMode.hide }, { "fadeout", change.exitMode.fadeout }, { "W", you.wasd.w }, { "w", you.wasd.w }, { "A", you.wasd.a }, { "a", you.wasd.a }, { "S", you.wasd.s }, { "s", you.wasd.s }, { "D", you.wasd.d }, { "d", you.wasd.d }, { "true", true }, { "false", false }, { "t", true }, { "f", false } };
    private readonly Hashtable commandHelpStrings = new Hashtable { { "tele", "tele命令：让玩家传送至指定地点(命令格式：tele 退出转场 进入转场 [世界名 = \"nexus\"] [传送x坐标 = 0] [传送y坐标 = 0] [朝向 = 你的朝向] [传送时音效在sounds的索引 = 0] [传送后音效在sounds的索引 = 0])" }, { "help", "help命令：了解命令的主要作用(命令格式：help 命令名称)" }, { "#", "#命令：用来注释命令(命令格式：# ...)" }, { "move", "move命令：强制让玩家移动(命令格式：move [速度]朝向 [步数 = 1])" }, { "show", "show命令：显示玩家(命令格式：show)" }, { "hide", "hide命令：隐藏玩家(命令格式：hide)" }, {"play", "play命令：播放一段声音(命令格式：play [声音在sounds的索引 = 0] [是否等待声音结束 = false])"}, { "turn", "turn命令：改变玩家的朝向(命令格式：turn [朝向 = s])"}, { "stop", "stop命令：停止发出声音(命令格式：stop)"}, { "wait", "wait命令：等待一段时间(命令格式：wait [等待时间 = 1])"}, { "use", "use命令：使用物品栏里第一个道具名相同的道具(命令格式：use [道具名 = \"default\"])"}, { "debug", "debug命令：输出一些信息(命令格式：debug (信息))" } };
    private bool isDone = false;
    public bool tempSwitch = false;
    public static List<IEnumerator> funcs;
    private bool funcIsEnd = true;
#nullable enable
    private change.enterMode? enterMode;
    private change.exitMode? exitMode;
    private string? worldName;
    private int? teleX;
    private int? teleY;
    private float? teleHigh;
    private you.wasd? front;
    private int? closeSoundIndex;
    private int? teleSoundIndex;
    private int? soundIndex;
    private int? step;
    private float? tempSpeed = 4;
    private bool? isWaitSoundEnd;
    private float? waitTime;
    private string? itemName;
    private string? str;
#nullable disable
    private void init()
    {
        soundIndex = step = teleX = teleY = closeSoundIndex = teleSoundIndex = null;
        str = itemName = worldName = null;
        waitTime = tempSpeed = teleHigh = null;
        front = null;
        enterMode = null;
        exitMode = null;
        isWaitSoundEnd = null;
    }

    void Start()
    {
        funcs = new List<IEnumerator>();
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
        int ia = 0;
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
                Debug.LogError("双引号格式错误");
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
                    if (!commandHelpStrings.ContainsKey(commandName))
                    {
                        Debug.LogError("命令" + commandName + "不存在");
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
                                    if ("u" == value)
                                    {
                                        front = you.front;
                                        break;
                                    }
                                    front = stringModes.ContainsKey(value) ? (you.wasd)stringModes[value] : (you.wasd)int.Parse(value);
                                    break;
                                case 8:
                                    closeSoundIndex = int.Parse(value);
                                    break;
                                case 9:
                                    teleSoundIndex = int.Parse(value);
                                    break;
                                default:
                                    goto normalEnd;
                            }
                            break;
                        case "help":
                            if (1 == i && commandHelpStrings.ContainsKey(value))
                            {
                                Debug.Log(commandHelpStrings[value]);
                            }
                            else
                            {
                                goto normalEnd;
                            }
                            break;
                        case "move":
                            if (1 == i)
                            {
                                int delimiterIndex = -1;
                                for (int j = 0; j < value.Length; j++)
                                {
                                    if (char.IsLetter(value[j]))
                                    {
                                        delimiterIndex = j;
                                        break;
                                    }
                                }
                                if (-1 == delimiterIndex)
                                {
                                    delimiterIndex = value.Length;
                                }
                                if (0 != delimiterIndex)
                                {
                                    tempSpeed = float.Parse(value.Substring(0, delimiterIndex));
                                }
                                if (stringModes.ContainsKey(value.Substring(delimiterIndex))){
                                    front = ("u" == value.Substring(delimiterIndex) || "U" == value.Substring(delimiterIndex) ? you.front : (you.wasd)stringModes[value.Substring(delimiterIndex)]);
                                }
                            }
                            else if (2 == i)
                            {
                                step = int.Parse(value);
                            }
                            else
                            {
                                goto normalEnd;
                            }
                            break;
                        case "play":
                            switch (i) {
                                case 1:
                                    soundIndex = int.Parse(value);
                                    break;
                                case 2:
                                    if (stringModes.ContainsKey(value))
                                    {
                                        isWaitSoundEnd = (bool)stringModes[value];
                                    }
                                    else if (int.TryParse(value, out ia))
                                    {
                                        isWaitSoundEnd = 0 != ia;
                                    }
                                    break;
                                default:
                                    goto normalEnd;
                            }
                            break;
                        case "turn":
                            if ("u" == value || "U" == value)
                            {
                                front = you.front;
                            }
                            else if (stringModes.ContainsKey(value))
                            {
                                front = (you.wasd)stringModes[value];
                            }
                            goto normalEnd;
                        case "wait":
                            waitTime = float.Parse(value);
                            goto normalEnd;
                        case "use":
                            itemName = value;
                            goto normalEnd;
                        case "debug":
                            Debug.Log(value);
                            goto normalEnd;
                        default:
                            goto normalEnd;
                    }
                }
            }
        normalEnd:;
            switch (commandName)
            {
                case "tele":
                    funcs.Add(you.tele(exitMode ?? change.exitMode.hide, enterMode ?? change.enterMode.show, image, worldName ?? "nexus", teleX ?? 0, teleY ?? 0, teleHigh ?? 0, front ?? you.wasd.s, sounds[closeSoundIndex ?? 0] ?? sounds[0], sounds[teleSoundIndex ?? 0] ?? sounds[0]));
                    break;
                case "move":
                    funcs.Add(u.move(front ?? you.front, step ?? 1, tempSpeed ?? 1));
                    break;
                case "show":
                    funcs.Add(u.show());
                    break;
                case "hide":
                    funcs.Add(u.hide());
                    break;
                case "play":
                    funcs.Add(u.play(sounds[soundIndex ?? 0] ?? sounds[0], isWaitSoundEnd ?? false));
                    break;
                case "turn":
                    funcs.Add(you.turn(front ?? you.wasd.s));
                    break;
                case "stop":
                    funcs.Add(u.stop());
                    break;
                case "wait":
                    funcs.Add(you.wait(waitTime ?? 1));
                    break;
                case "use":
                    for (int j = 0; j < you.items.Count; j++)
                    {
                        if ((itemName ?? "default") == you.items[j].name)
                        {
                            you.items[j].use();
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
            errorEnd:;
        }
        isEnd = true;
        isDone = true;
        yield return null;
    }

    IEnumerator runFunc() {
        funcIsEnd = false;
        you.canMove = false;
        if (you.moveIsEnd && isDone && funcs.Count > 0)
        {
            while (funcs.Count > 0)
            {
                StartCoroutine(funcs[0]);
                funcs.RemoveAt(0);
                yield return new WaitUntil(() => you.teleIsEnd && you.moveIsEnd && you.commandIsEnd);
            }
        }
        you.canMove = true;
        funcIsEnd = true;
        yield return null;
    }
    void Update()
    {
        if (funcIsEnd && funcs.Count != 0)
        {
            StartCoroutine(runFunc());
        }
        if (!isDone && isEnd && u.x >= x - extendLeft && u.x <= x + extendRight && u.y >= y - extendUp && u.y <= y + extendDown)
        {
            StartCoroutine(runCommand());
        }
        if (isEnd && 0 == funcs.Count && !(u.x >= x - extendLeft && u.x <= x + extendRight && u.y >= y - extendUp && u.y <= y + extendDown && you.commandIsEnd))
        {
            isDone = false;
        }
    }
}
