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
    public enum commandNames
    {
        tele, 
        help, 
        move, 
        show, 
        hide, 
        play, 
        turn, 
        stop, 
        wait, 
        use, 
        value, 
        debug, 
        Goto
    }
    public change.enterMode enterModesView;
    public change.exitMode exitModesView;
    public commandFronts frontsView;
    public commandTurns turnsView;
    public commandNames canUseCommandNamesView;
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
    public string[] commands;//Ŀǰ֧��tele����, help����, #����, move����, show����, hide����, play����, turn����, stop����, wait����, use����, value����, debug����, goto����, exit��� talk������֮��ʵ��
    public AudioClip[] sounds;
    public you u;
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
        m = GameObject.Find(mpath).GetComponent<map>();
        if (ChangeTransform)
        {
            //���ݵ�ͼxyz�����transform����
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
        Hashtable commandHelpStrings = new Hashtable { { "tele", "tele�������Ҵ�����ָ���ص�(�����ʽ��tele �˳�ת�� ����ת�� [������ = \"nexus\"] [����x���� = 0] [����y���� = 0] [���� = ��ĳ���] [����ʱ��Ч��sounds������ = 0(��Ϊnull)] [���ͺ���Ч��sounds������ = 0(��Ϊnull)] [�Ƿ��Զ���յ�ǰ�򿪵Ĳ˵� = true])" }, { "help", "help����˽��������Ҫ����(�����ʽ��help ��������)" }, { "#", "#�������ע������(�����ʽ��# ...)" }, { "move", "move���ǿ��������ƶ�(�����ʽ��move [�ٶ�]���� [���� = 1] [�Ƿ��Զ���յ�ǰ�򿪵Ĳ˵� = true])" }, { "show", "show�����ʾ���(�����ʽ��show  [�Ƿ��Զ���յ�ǰ�򿪵Ĳ˵� = true])" }, { "hide", "hide����������(�����ʽ��hide  [�Ƿ��Զ���յ�ǰ�򿪵Ĳ˵� = true])" }, { "play", "play�������һ������(�����ʽ��play [������sounds������ = 0] [�Ƿ�ȴ��������� = false]  [�Ƿ��Զ���յ�ǰ�򿪵Ĳ˵� = false])" }, { "turn", "turn����ı���ҵĳ���(�����ʽ��turn [���� = s]|[���ת�ķ�ʽ = (l(eft)|b(ack)|r(ight))]  [�Ƿ��Զ���յ�ǰ�򿪵Ĳ˵� = true]))" }, { "stop", "stop���ֹͣ��������(�����ʽ��stop)" }, { "wait", "wait����ȴ�һ��ʱ��(�����ʽ��wait [�ȴ�ʱ�� = 1])" }, { "use", "use���ʹ����Ʒ�����һ����������ͬ�ĵ���(�����ʽ��use [������ = \"default\"]  [�Ƿ��Զ���յ�ǰ�򿪵Ĳ˵� = true])" }, { "debug", "debug������һЩ��Ϣ(�����ʽ��debug (��Ϣ))" }, { "value", "value����鿴����ĳЩ�������ͱ�������ϸ����(�����ʽ��value)" }, { "goto", "goto�����ת��ĳһ��(�����ʽ��goto (��ǩ(��ǩ��ʽ����(��ǩ��):��)))" }, { "exit", "exit����˳���Ϸ(�����ʽ��exit  [�Ƿ��Զ���յ�ǰ�򿪵Ĳ˵� = false])" }, { "close", "close���������ǰ�˵�(�����ʽ��close)" }, { "clear", "clear�����յ�ǰ�Ĳ˵�(�����ʽ��clear)"} };
        Hashtable valueHelpStrings = new Hashtable { { "none", "(����ת��|�뿪ת��)����" }, { "show", "����ת��������ʾ" }, { "hide", "�뿪ת����������" }, { "fadein", "����ת��������" }, { "fadeout", "�뿪ת��������" }, { "w", "������" }, { "a", "������" }, { "s", "������" }, { "d", "������" }, { "u", "������ĳ���" }, { "l", "��ת��ʽ��������ת90��" }, { "b", "��ת��ʽ��������ת" }, { "r", "��ת��ʽ��������ת90��" }, { "left", "ͬ��l��" }, { "back", "ͬ��b��" }, { "right", "ͬ��r��" } };
        Hashtable labels = new Hashtable();
        Hashtable stringModes = new Hashtable { { "show", change.enterMode.show }, { "fadein", change.enterMode.fadein }, { "hide", change.exitMode.hide }, { "fadeout", change.exitMode.fadeout }, { "W", you.wasd.w }, { "w", you.wasd.w }, { "A", you.wasd.a }, { "a", you.wasd.a }, { "S", you.wasd.s }, { "s", you.wasd.s }, { "D", you.wasd.d }, { "d", you.wasd.d }, { "true", true }, { "false", false }, { "t", true }, { "f", false }, { "null", null } };
        Hashtable turnModes = new Hashtable { { "l", 3 }, { "left", 3 }, { "b", 2 }, { "back", 2 }, { "r", 1 }, { "right", 1 } };
        you.wasd[] turnArray = { you.wasd.w, you.wasd.d, you.wasd.s, you.wasd.a };
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
            you.wasd? front = null;
            int? closeSoundIndex = null;
            int? teleSoundIndex = null;
            int? soundIndex = null;
            int? step = null;
            float? tempSpeed = null;
            bool? isWaitSoundEnd = null;
            float? waitTime = null;
            string? itemName = null;
            string? str = null;
            bool? autoClearMenu = null;
#nullable disable
            delimiterIndexs.Clear();
            delimiterIndexs.Add(-1);
            //����
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
                Debug.LogError("˫���Ÿ�ʽ����");
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
                Debug.LogError("���д��󣺵�" + commandI + "�������ظ�ִ�д������࣬��ǿ�ƽ����ű�ִ��");
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
                        Debug.LogError("����" + commandName + "������");
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
                                    if ("u" == value.ToLower())
                                    {
                                        front = you.front;
                                        break;
                                    }
                                    front = stringModes.ContainsKey(value) ? (you.wasd)stringModes[value] : (you.wasd)int.Parse(value);
                                    break;
                                case 8:
                                    if ("null" != value.ToLower())
                                    {
                                        closeSoundIndex = int.Parse(value);
                                    }
                                    break;
                                case 9:
                                    if ("null" != value.ToLower())
                                    {
                                        teleSoundIndex = int.Parse(value);
                                    }
                                    break;
                                case 10:
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
                                    front = ("u" == value.Substring(delimiterIndex) || "U" == value.Substring(delimiterIndex) ? you.front : (you.wasd)stringModes[value.Substring(delimiterIndex)]);
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
                                        front = you.front;
                                    }
                                    else if (stringModes.ContainsKey(value.ToLower()))
                                    {
                                        front = (you.wasd)stringModes[value];
                                    }
                                    else if (turnModes.ContainsKey(value.ToLower()))
                                    {
                                        front = turnArray[(rturnArray[(int)you.front] + (int)turnModes[value.ToLower()]) % 4];
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
                                Debug.Log("��ǩ���󣺱�ǩ������");
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
                    funcs.Add(you.tele(exitMode ?? change.exitMode.hide, enterMode ?? change.enterMode.show, worldName ?? "nexus", teleX ?? 0, teleY ?? 0, teleHigh ?? 0, front ?? you.wasd.s, null == sounds ? null : sounds[closeSoundIndex ?? 0] ?? sounds[0], null == sounds ? null : sounds[teleSoundIndex ?? 0] ?? sounds[0]));
                    break;
                case "move"://��u���������Ϊnpc
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
                    funcs.Add(u.move(front ?? you.front, step ?? 1, tempSpeed ?? 1));
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
                    funcs.Add(you.turn(front ?? you.wasd.s));
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
                    if (autoClearMenu ?? false)
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
        
        if (!isDone && isEnd && u.x >= x - extendLeft && u.x <= x + extendRight && u.y >= y - extendUp && u.y <= y + extendDown)
        {
            StartCoroutine(runCommands(commands, sounds, u));
        }
        if (isEnd && 0 == funcs.Count && !(u.x >= x - extendLeft && u.x <= x + extendRight && u.y >= y - extendUp && u.y <= y + extendDown && you.teleIsEnd && you.moveIsEnd && you.commandIsEnd))
        {
            isDone = false;
        }
    }
}