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

public class runCommands
{
    bool isEnd = true;
    public IEnumerator runCommand(string[] commands, AudioClip[] sounds = null, you u = null)
    {
        if (isEnd)
        {
            isEnd = false;
            yield return null;
            isEnd = true;
        }
    }
}

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
    public int commandI = 0;
    public Hashtable labels = new Hashtable();
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
    public static IEnumerator debugStr(string str)
    {
        you.commandIsEnd = false;
        Debug.Log(str);
        you.commandIsEnd = true;
        yield return null;
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

    public IEnumerator runCommands(string[] commands, AudioClip[] sounds = null, you u = null)
    {
        commands ??= new string[0];
        isEnd = false;
        int[] runCounts = new int[commands.Length];
        List<int> delimiterIndexs = new List<int> { -1 };
        string value = "";
        string commandName = "";
        bool isCount = true;
        command _command = null;
        for (commandI = 0; null != commands && commandI < commands.Length; commandI++)
        {
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
                    _command = command.stringToCommands(commandName);
                }
                if (0 != i && !_command.setValue(value, i - 1, this))
                {
                    goto normalEnd;
                }
            }
        normalEnd:;
            if (!_command.execute())
            {
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
