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

public class _runCommands
{
    public int commandI = 0;
    public Hashtable labels = new Hashtable();
    public static Hashtable keyWordHas = new Hashtable();
    public Hashtable vars = new Hashtable();
    public void runCommands(string[] commands, AudioClip[] sounds = null, you u = null)
    {
        commands ??= new string[0];
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
                else if ('\"' == commands[commandI][i])
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
                    if (!Regex.Match(commandName, "^[^:]+:$").Success && !command.CanCommandNameToRecommend(commandName))
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
        trigger.isDone = true;
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
    private _runCommands _runCommands;
    public you u
    {
        get
        {
            return you.You;
        }
    }
    public static bool isDone = false;
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
    
    public static void runCommands(string[] commands, AudioClip[] sounds = null, you u = null)
    {
        new _runCommands().runCommands(commands, sounds, u);
    }

    void Update()
    {
        _runCommands = new _runCommands();
        if (!isDone && u.x >= x - triggerExtendLeft && u.x <= x + triggerExtendRight && u.y >= y - triggerExtendUp && u.y <= y + triggerExtendDown)
        {
            runCommands(commands, sounds, u);
        }
        if (0 == funcs.Count && !(u.x >= x - triggerExtendLeft && u.x <= x + triggerExtendRight && u.y >= y - triggerExtendUp && u.y <= y + triggerExtendDown && you.teleIsEnd && you.moveIsEnd && you.commandIsEnd))
        {
            isDone = false;
        }
    }
}
