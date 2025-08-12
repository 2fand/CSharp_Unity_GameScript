using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class trigger : MonoBehaviour
{
    public enum commandFronts
    {
        w, 
        a,
        s,
        d,
        u
    }
    public enum commandTurns
    {
        l,
        left,
        b,
        back,
        r,
        right
    }
    private enum stringCode
    {
        and,
        or,
        not
    }
    public int x = 0;
    public int y = 0;
    public readonly static Hashtable commandHelpStrings = new Hashtable { { "tele", "tele命令：让玩家传送至指定地点(命令格式：tele 退出转场 退出转场时间 进入转场 进入转场时间 [世界名 = \"nexus\"] [传送x坐标 = 0] [传送y坐标 = 0] [朝向 = 你的朝向] [传送时音效在sounds的索引 = 0(可为null)] [传送后音效在sounds的索引 = 0(可为null)] [是否自动清空当前打开的菜单 = true])" }, { "help", "help命令：了解命令的主要作用(命令格式：help 命令名称)" }, { "#", "#命令：用来注释命令(命令格式：# ...)" }, { "move", "move命令：强制让玩家移动(命令格式：move [速度]朝向 [步数 = 1] [是否自动清空当前打开的菜单 = true])" }, { "show", "show命令：显示玩家(命令格式：show  [是否自动清空当前打开的菜单 = true])" }, { "hide", "hide命令：隐藏玩家(命令格式：hide  [是否自动清空当前打开的菜单 = true])" }, { "play", "play命令：播放一段声音(命令格式：play [声音在sounds的索引 = 0] [是否等待声音结束 = false]  [是否自动清空当前打开的菜单 = false])" }, { "turn", "turn命令：改变玩家的朝向(命令格式：turn [朝向 = s]|[玩家转的方式 = (l(eft)|b(ack)|r(ight))]  [是否自动清空当前打开的菜单 = true]))" }, { "stop", "stop命令：停止发出声音(命令格式：stop)" }, { "wait", "wait命令：等待一段时间(命令格式：wait [等待时间 = 1])" }, { "use", "use命令：使用物品栏里第一个道具名相同的道具(命令格式：use [道具名 = \"default\"]  [是否自动清空当前打开的菜单 = true])" }, { "debug", "debug命令：输出一些信息(命令格式：debug (信息))" }, { "value", "value命令：查看关于某些特殊类型变量的详细介绍(命令格式：value)" }, { "goto", "goto命令：跳转到某一行(命令格式：goto (标签(标签格式：“(标签名):”)))" }, { "exit", "exit命令：退出游戏(命令格式：exit  [是否自动清空当前打开的菜单 = false])" }, { "close", "close命令：跳出当前菜单(命令格式：close)" }, { "clear", "clear命令：清空当前的菜单(命令格式：clear)" } };
    public readonly static Hashtable valueHelpStrings = new Hashtable { { "none", "(进入转场|离开转场)：无" }, { "show", "进入转场：逐渐显示" }, { "hide", "离开转场：逐渐隐藏" }, { "fadein", "进入转场：淡入" }, { "fadeout", "离开转场：淡出" }, { "w", "朝向：上" }, { "a", "朝向：左" }, { "s", "朝向：下" }, { "d", "朝向：右" }, { "u", "朝向：你的朝向" }, { "l", "旋转方式：向左旋转90度" }, { "b", "旋转方式：往后旋转" }, { "r", "旋转方式：向右旋转90度" }, { "left", "同“l”" }, { "back", "同“b”" }, { "right", "同“r”" } };
    public bool ChangeTransform = true;
    public GameObject triggerPrefab;
    public map m
    {
        get
        {
            return map.Map;
        }
    }
    public int triggerExtendLeft = 0;
    public int triggerExtendRight = 0;
    public int triggerExtendUp = 0;
    public int triggerExtendDown = 0;
    public string[] commands;//可用命令请见提示框
    public int commandsCount;
    public AudioClip[] sounds;
    public Sprite[] sprites;
    public int soundsCount;
    public int spritesCount;
    public you u
    {
        get
        {
            return you.You;
        }
    }
    private static bool isEnd = true;
    private static bool isDone = false;
    public static bool IsDone
    {
        get
        {
            return isDone;
        }
    }
    public bool tempSwitch = false;
    public static List<IEnumerator> funcs = new List<IEnumerator>();
    static IEnumerator debugStr(string str)
    {
        you.commandIsEnd = false;
        Debug.Log(str);
        you.commandIsEnd = true;
        yield return null;
    }
    static string easyNum(string stringNum) {
        string symbol = Regex.Match(stringNum, "^[+-]*").Value;
        string absNum = Regex.Match(stringNum, "[^+-]*$").Value;
        absNum = ("" == absNum ? "1" : absNum);
        bool isNegative = false;
        foreach (char ch in symbol)
        {
            if (ch == '-')
            {
                isNegative = !isNegative;
            }
        }
        return (isNegative ? "-" : "") + absNum;
    }
    void Start()
    {
        funcs = new List<IEnumerator>();
        if (ChangeTransform)
        {
            //根据地图xyz轴进行transform计算
            transform.position = new Vector3(m.minX + m.heightX / m.x * (0.5f + x), transform.position.y, m.maxY - m.widthY / m.y * (0.5f + y));
        }
        if (null == triggerPrefab)
        {
            GameObject emptyWall = new GameObject("emptyWall");
            triggerPrefab = emptyWall;
        }
        Vector3[] clones = { new Vector3(-m.heightX, 0, m.widthY), new Vector3(0, 0, m.widthY), new Vector3(m.heightX, 0, m.widthY), new Vector3(-m.heightX, 0, 0), new Vector3(m.heightX, 0, 0), new Vector3(-m.heightX, 0, -m.widthY), new Vector3(0, 0, -m.widthY), new Vector3(m.heightX, 0, -m.widthY) };
        foreach (Vector3 v in clones)
        {
            if (!m.horizontalIsCycle && v.x != 0 || !m.verticalIsCycle && v.z != 0)
            {
                continue;
            }
            triggerPrefab.transform.localPosition = v + transform.position;
            triggerPrefab.transform.rotation = transform.rotation;
            Instantiate(triggerPrefab, transform, true);
        }
    }

    public static IEnumerator runCommands(string[] commands, AudioClip[] sounds = null, you u = null)
    {
        commands ??= new string[0];
        isEnd = false;
        int[] runCounts = new int[commands.Length];
        List<int> delimiterIndexs = new List<int> { -1 };
        string value = "";
        string commandName = "";
        bool isCount = true;
        int ia = 0;
        float tempTime = 0;
        Hashtable labels = new Hashtable();
        Hashtable stringModes = new Hashtable { { "show", change.enterMode.show }, { "fadein", change.enterMode.fadein }, { "hide", change.exitMode.hide }, { "fadeout", change.exitMode.fadeout }, { "W", wasd.w }, { "w", wasd.w }, { "A", wasd.a }, { "a", wasd.a }, { "S", wasd.s }, { "s", wasd.s }, { "D", wasd.d }, { "d", wasd.d }, { "true", true }, { "false", false }, { "t", true }, { "f", false }, { "null", null } };
        Hashtable turnModes = new Hashtable { { "l", 3 }, { "left", 3 }, { "b", 2 }, { "back", 2 }, { "r", 1 }, { "right", 1 } };
        wasd[] turnArray = { wasd.w, wasd.d, wasd.s, wasd.a };
        int[] rturnArray = { 0, 3, 2, 1 };
        for (int commandI = 0; null != commands && commandI < commands.Length; commandI++)
        {
#nullable enable
            change.enterMode? enterMode = null;
            change.exitMode? exitMode = null;
            string? worldName = null;
            int? teleX = null;
            int? teleY = null;
            float? teleHigh = null;
            wasd? face = null;
            int? closeSoundIndex = null;
            int? teleSoundIndex = null;
            int? soundIndex = null;
            int? step = null;
            float? tempSpeed = null;
            bool? isWaitSoundEnd = null;
            float? waitTime = null;
            float? enterTime = null;
            float? exitTime = null;
            string? itemName = null;
            string? str = null;
            bool? autoClearMenu = null;
#nullable disable
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
            string regexStr = "";
            commandName = "";
            for (int i = 0; i < commands.Length; i++)
            {
                if (Regex.Match(commands[i], "^[^:]+:$").Success)
                {
                    regexStr = Regex.Match(commands[i], "^[^:]+:$").Value;
                    labels[regexStr.Substring(0, regexStr.Length - 1)] = i;
                }
            }
            runCounts[commandI]++;
            if (runCounts[commandI] >= 100000)
            {
                Debug.LogError("运行错误：第" + commandI + "条代码重复执行次数过多，已强制结束脚本执行");
                commandI = commands.Length;
                goto errorEnd;
            }
            for (int i = 0; i < delimiterIndexs.Count - 1; i++)
            {
                value = commands[commandI].Substring(delimiterIndexs[i] + 1, delimiterIndexs[i + 1] - delimiterIndexs[i] - 1);
                if ('\"' == value[0])
                {
                    value = value.Substring(1, value.Length - 2);
                }
                if (0 == i)
                {
                    commandName = value.ToLower();
                    if (!Regex.Match(commandName, "^[^:]+:$").Success && !commandHelpStrings.ContainsKey(commandName))
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
                                    exitTime = float.TryParse(value, out tempTime) ? tempTime : null;
                                    break;
                                case 3:
                                    enterMode = stringModes.ContainsKey(value.ToLower()) ? (change.enterMode)stringModes[value] : change.enterMode.none;
                                    break;
                                case 4:
                                    enterTime = float.TryParse(value, out tempTime) ? tempTime : null;
                                    break;
                                case 5:
                                    worldName = value;
                                    break;
                                case 6:
                                    teleX = int.Parse(value);
                                    break;
                                case 7:
                                    teleY = int.Parse(value);
                                    break;
                                case 8:
                                    teleHigh = float.Parse(value);
                                    break;
                                case 9:
                                    if ("u" == value.ToLower())
                                    {
                                        face = you.face;
                                        break;
                                    }
                                    face = stringModes.ContainsKey(value) ? (wasd)stringModes[value] : (wasd)int.Parse(value);
                                    break;
                                case 10:
                                    if ("null" != value.ToLower())
                                    {
                                        closeSoundIndex = int.Parse(value);
                                    }
                                    break;
                                case 11:
                                    if ("null" != value.ToLower())
                                    {
                                        teleSoundIndex = int.Parse(value);
                                    }
                                    break;
                                case 12:
                                    if (stringModes.ContainsKey(value))
                                    {
                                        autoClearMenu = (bool)stringModes[value];
                                    }
                                    break;
                                default:
                                    goto normalEnd;
                            }
                            break;
                        case "help":
                            if (1 == i && commandHelpStrings.ContainsKey(value))
                            {
                                str = (string)commandHelpStrings[value];
                            }
                            goto normalEnd;
                        case "show":
                        case "hide":
                            if (stringModes.ContainsKey(value))
                            {
                                autoClearMenu = (bool)stringModes[value];
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
                                    tempSpeed = float.Parse(easyNum(value.Substring(0, delimiterIndex)));
                                }
                                if (stringModes.ContainsKey(value.Substring(delimiterIndex))){
                                    face = ("u" == value.Substring(delimiterIndex) || "U" == value.Substring(delimiterIndex) ? you.face : (wasd)stringModes[value.Substring(delimiterIndex)]);
                                }
                            }
                            else if (2 == i)
                            {
                                step = int.Parse(value);
                            }
                            else
                            {
                                if (stringModes.ContainsKey(value))
                                {
                                    autoClearMenu = (bool)stringModes[value];
                                }
                                goto normalEnd;
                            }
                            break;
                        case "play":
                            switch (i) {
                                case 1:
                                    soundIndex = int.Parse(value);
                                    break;
                                case 2:
                                    if (stringModes.ContainsKey(value.ToLower()))
                                    {
                                        isWaitSoundEnd = (bool)stringModes[value];
                                    }
                                    else if (int.TryParse(value, out ia))
                                    {
                                        isWaitSoundEnd = 0 != ia;
                                    }
                                    break;
                                case 3:
                                    if (stringModes.ContainsKey(value))
                                    {
                                        autoClearMenu = (bool)stringModes[value];
                                    }
                                    break;
                                default:
                                    goto normalEnd;
                            }
                            break;
                        case "turn":
                            switch (i)
                            {
                                case 1:
                                    if ("u" == value || "U" == value)
                                    {
                                        face = you.face;
                                    }
                                    else if (stringModes.ContainsKey(value.ToLower()))
                                    {
                                        face = (wasd)stringModes[value];
                                    }
                                    else if (turnModes.ContainsKey(value.ToLower()))
                                    {
                                        face = turnArray[(rturnArray[(int)you.face] + (int)turnModes[value.ToLower()]) % 4];
                                    }
                                    break;
                                default:
                                    if (stringModes.ContainsKey(value))
                                    {
                                        autoClearMenu = (bool)stringModes[value];
                                    }
                                    break;
                            }
                            goto normalEnd;
                        case "wait":
                            waitTime = float.Parse(value);
                            goto normalEnd;
                        case "use":
                            if (1 == i)
                            {
                                itemName = value;
                            }
                            else
                            {
                                if (stringModes.ContainsKey(value))
                                {
                                    autoClearMenu = (bool)stringModes[value];
                                }
                                goto normalEnd;
                            }
                            break;
                        case "debug":
                            str = value;
                            goto normalEnd;
                        case "value":
                            if (valueHelpStrings.ContainsKey(value.ToLower()))
                            {
                                str = (string)valueHelpStrings[value.ToLower()];
                            }
                            goto normalEnd;
                        case "goto":
                            if (!labels.ContainsKey(value))
                            {
                                Debug.Log("标签错误：标签不存在");
                                goto errorEnd;
                            }
                            commandI = (int)labels[value];
                            goto normalEnd;
                        case "exit":
                            if (stringModes.ContainsKey(value))
                            {
                                autoClearMenu = (bool)stringModes[value];
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
                    if (autoClearMenu ?? true)
                    {
                        funcs.Add(you.You.clearMenu());
                    }
                    funcs.Add(you.tele(exitMode ?? change.exitMode.hide, exitTime ?? Game.exitTime, enterMode ?? change.enterMode.show, enterTime ?? Game.enterTime, worldName ?? "nexus", teleX ?? 0, teleY ?? 0, teleHigh ?? 0, face ?? wasd.s, null == sounds ? null : sounds[closeSoundIndex ?? 0] ?? sounds[0], null == sounds ? null : sounds[teleSoundIndex ?? 0] ?? sounds[0]));
                    break;
                case "move"://“u”对象可能为npc
                    if (autoClearMenu ?? true)
                    {
                        funcs.Add(you.You.clearMenu());
                    }
                    if (null == u)
                    {
                        break;
                    }
                    step ??= 1;
                    if (0 > tempSpeed)
                    {
                        tempSpeed *= -1;
                        step *= -1;
                    }
                    funcs.Add(u.move(face ?? you.face, step ?? 1, tempSpeed ?? 1));
                    break;
                case "show":
                    if (autoClearMenu ?? true)
                    {
                        funcs.Add(you.You.clearMenu());
                    }
                    if (null == u)
                    {
                        break;
                    }
                    funcs.Add(u.show());
                    break;
                case "hide":
                    if (autoClearMenu ?? true)
                    {
                        funcs.Add(you.You.clearMenu());
                    }
                    if (null == u)
                    {
                        break;
                    }
                    funcs.Add(u.hide());
                    break;
                case "play":
                    if (autoClearMenu ?? false)
                    {
                        funcs.Add(you.You.clearMenu());
                    }
                    if (null != sounds)
                    {
                        funcs.Add(you.play(sounds[soundIndex ?? 0] ?? sounds[0], isWaitSoundEnd ?? false));
                    }
                    break;
                case "turn":
                    if (autoClearMenu ?? true)
                    {
                        funcs.Add(you.You.clearMenu());
                    }
                    funcs.Add(you.turn(face ?? wasd.s));
                    break;
                case "stop":
                    funcs.Add(you.stop());
                    break;
                case "wait":
                    funcs.Add(you.wait(waitTime ?? 1));
                    break;
                case "use":
                    if (autoClearMenu ?? true)
                    {
                        funcs.Add(you.You.clearMenu());
                    }
                    for (int j = 0; j < you.items.Count; j++)
                    {
                        if ((itemName ?? "default") == you.items[j].name)
                        {
                            you.items[j].use();
                            break;
                        }
                    }
                    break;
                case "help":
                case "debug":
                case "value":
                    funcs.Add(debugStr(str ?? ""));
                    break;
                case "exit":
                    if (0 != you.Menus.Count && (autoClearMenu ?? false))
                    {
                        funcs.Add(you.You.clearMenu());
                    }
                    funcs.Add(you.exit());
                    break;
                case "close":
                    funcs.Add(you.You.closeMenu());
                    break;
                case "clear":
                    funcs.Add(you.You.clearMenu());
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

    
    void Update()
    {
        
        if (!isDone && isEnd && u.x >= x - triggerExtendLeft && u.x <= x + triggerExtendRight && u.y >= y - triggerExtendUp && u.y <= y + triggerExtendDown)
        {
            StartCoroutine(runCommands(commands, sounds, u));
        }
        if (isEnd && 0 == funcs.Count && !(u.x >= x - triggerExtendLeft && u.x <= x + triggerExtendRight && u.y >= y - triggerExtendUp && u.y <= y + triggerExtendDown && you.teleIsEnd && you.moveIsEnd && you.commandIsEnd))
        {
            isDone = false;
        }
    }
}