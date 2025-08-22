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
using static symbol;

public class _runCommands
{
    public int commandI = 0;
    public Hashtable labels = new Hashtable();
    public static Hashtable keyWordHas = new Hashtable();
    public Hashtable vars = new Hashtable();
    public Hashtable classes = new Hashtable();
#nullable enable
    public jsonValue? getVar(string strvar)
    {
        if (vars.ContainsKey(strvar))
        {
            return (jsonValue)vars[strvar];
        }
        return null;
    }
    public bool canGetVar(string strvar)
    {
        return null != getVar(strvar);
    }
    bool getValue(string command, string partten, ref int i)
    {
        if (Regex.Match(command.Substring(i), "^\\s*" + partten + "\\s*").Success)
        {
            i += Regex.Match(command.Substring(i), "^\\s*" + partten + "\\s*").Length;
            return true;
        }
        return false;
    }
#nullable enable
    public List<string>? getValues(string command, ref List<int> valueIndexs) {
#nullable disable
        List<string> values = new List<string>();
        int left = 0; //^\\s*{\\s*((.+\\s*,\\s*)*.+)?\\s*}\\s*$ class
        int right = 0;
        for (int i = 0; i < command.Length;)
        {
            left = i;
            //Debug.Log("(" + jsonValue.classRegex + "|" + jsonValue.classRegex + "\\[\\s*[^[\\]]+\\s*\\]|{\\s*((" + jsonValue.stringRegex + ":\\s*[^,]+\\s*,\\s*)*" + jsonValue.stringRegex + "\\s*:\\s*[^,]+\\s*)?}|{\\s*(\\s*(\\s*[^,]+\\s*,)*\\s*[^,]+\\s*)?})");
            if (getValue(command, "(" + jsonValue.classRegex + "\\(\\)|" + jsonValue.classRegex + "\\[\\s*[^[\\]]+\\s*\\]|{\\s*((" + jsonValue.stringRegex + ":\\s*[^,]+\\s*,\\s*)*" + jsonValue.stringRegex + "\\s*:\\s*[^,]+\\s*)?}|{\\s*(\\s*(\\s*[^,]+\\s*,)*\\s*[^,]+\\s*)?})", ref i)) { }
            else if (getValue(command, "\\(" + jsonValue.classRegex + "\\)", ref i)) { }
            else if (getValue(command, jsonValue.stringRegex, ref i)) { }
            else if (getValue(command, "\\+[+=]?", ref i)) { }//读取运算符
            else if (getValue(command, "-[-=]?", ref i)) { }
            else if (getValue(command, "[+-]?[0-9]+", ref i)) { }
            else if (getValue(command, "[+-]?([0-9]+(\\.([0-9]+)?)?|\\.[0-9]+)([eE][+-]?[0-9]+)?", ref i)) { }
            else if (getValue(command, "\\*\\*?=?", ref i)) { }
            else if (getValue(command, "//?=?", ref i)) { }
            else if (getValue(command, "\\?[?.=]?", ref i)) { }
            else if (getValue(command, "[!=]=?", ref i)) { }
            else if (getValue(command, "&&?=?", ref i)) { }
            else if (getValue(command, "\\|\\|?=?", ref i)) { }
            else if (getValue(command, "\\^=?", ref i)) { }
            else if (getValue(command, ">>?=?", ref i)) { }
            else if (getValue(command, "<<?=?", ref i)) { }
            else if (getValue(command, "[[\\](),.~:]", ref i)) { }
            else if (getValue(command, "%=?", ref i)) { }
            else if (getValue(command, "[ai]s|new|(type|size)of|true|false|null|[a-zA-Z_]+", ref i)) { }//读取非运算符
            else
            {
                getValue(command, "[^\\s]*", ref i);
            }
            right = i - 1;
            values.Add(command.Substring(left, right - left + 1).Trim());
            valueIndexs.Add(left);
        }
        return values;
    }
    public void runCommands(string[] commands, AudioClip[] sounds = null, you u = null)
    {
        new wait();
        new setSymbol();
        commands ??= new string[0];
        int[] runCounts = new int[commands.Length];
        List<int> delimiterIndexs = new List<int> { -1 };
        string value = "";
        //string commandName = "";
        bool isCount = true;
        //command _command = null;
        for (commandI = 0; null != commands && commandI < commands.Length; commandI++)
        {
            for (int i = 0; i < commands[commandI].Length; i++)
            {
                if ('\"' == commands[commandI][i] && !(0 != i && '\\' == commands[commandI][i - 1]))
                {
                    isCount = !isCount;
                }
            }
            if (!isCount)
            {
                Debug.LogError("双引号格式错误");
                goto errorEnd;
            }
            //获取值域，分配括号，添加括号，移动运算符，过滤非括号，计算(还剩函数要完成)
            //获取值域
#nullable enable
            List<string>? values = null;
            List<int> valueIndexs = new List<int>();
#nullable disable
            values = getValues(commands[commandI], ref valueIndexs);
            if (null == values)
            {
                goto errorEnd;
            }
            //分配括号
            Stack<int>groupStack = new Stack<int>();
            Hashtable groupLeftIndexToGroupRightIndex = new Hashtable();
            Hashtable groupRightIndexToGroupLeftIndex = new Hashtable();
            Hashtable groupLeftStringToGroupRightString = new Hashtable { { "(", ")" }, { "[", "]" }, { "?[", "]" }, { "?", ":" } };
            for (int i = 0; i < values.Count; i++)
            {
                if ("(" == values[i] || "[" == values[i] || "?[" == values[i] || "?" == values[i])
                {
                    groupStack.Push(i);
                }
                if (")" == values[i] || "]" == values[i] || ":" == values[i])
                {
                    if ((string)groupLeftStringToGroupRightString[values[groupStack.Peek()]] != values[i])
                    {
                        Debug.LogError("括号错误：前文中第" + commandI + "行第" + valueIndexs[groupStack.Peek()] + "列的“" + values[groupStack.Peek()] + "”不可匹配于第" + commandI + "行第" + valueIndexs[i] + "列的“" + values[i] + "”");
                        goto errorEnd;
                    }
                    groupLeftIndexToGroupRightIndex.Add(groupStack.Peek(), i);
                    groupRightIndexToGroupLeftIndex.Add(i, groupStack.Pop());
                }
            }
            List<string> tempValues = new List<string>();
            for (int i = 0; i < values.Count; i++)
            {
                if (i > 0 && i < values.Count - 1 && "(" == values[i - 1].Trim() && ")" == values[i + 1].Trim() && commandClasses.classIsHas.ContainsKey(values[i].Trim()))
                {
                    tempValues[tempValues.Count - 1] = values[i].Trim();
                    tempValues.Add("(class)");
                }
                else
                {
                    tempValues.Add(values[i]);
                }
            }
            values = new List<string>(tempValues);
            List<float> beforeLevels = new List<float>{-1};
            List<int> beforeLefts = new List<int>{-1};
            List<int> beforeRights = new List<int>{-1};
            Stack<string> groupStrings = new Stack<string>();
            Stack<int> groupIndexs = new Stack<int>();
            List<int> addBrackets = new List<int>();
            int offset = 0;
            for (int i = 0; i < values.Count; i++)
            {
                addBrackets.Add(0);
            }
            bool frontHasValue = false;
            List<int> rbracketPoses = new List<int>();
            int delIndex = 0;
            //优先级<=左变
            for (int i = 0; i < values.Count; i++) {
                value = values[i];
                //\002
                if (frontHasValue && "++" == value)
                {
                    values[i] = value = ".*++";
                }
                if (frontHasValue && "--" == value)
                {
                    values[i] = value = ".*--";
                }
                if (frontHasValue && "+" == value)
                {
                    values[i] = value = ".*+";
                }
                if (frontHasValue && "-" == value)
                {
                    values[i] = value = ".*-";
                }
                if ("(" == value || "[" == value || "?[" == value) 
                {
                    beforeLefts.Add(-1);
                    beforeRights.Add(-1);
                    beforeLevels.Add(-1);
                    groupStrings.Push(value);
                    groupIndexs.Push(i);
                    addBrackets[i]++;
                    addBrackets[(int)groupLeftIndexToGroupRightIndex[i + offset] - offset]--;
                    if (-1 != beforeLevels[beforeLevels.Count - 1])
                    {
                        if (beforeLevels[beforeLevels.Count - 1] <= 0)
                        {
                            addBrackets[i]--;
                            addBrackets[beforeLefts[beforeLefts.Count - 1]]++;
                        }
                        else
                        {
                            addBrackets[beforeRights[beforeRights.Count - 1]]++;
                            addBrackets[(int)groupLeftIndexToGroupRightIndex[i + offset] - offset]--;
                        }
                        continue;
                    }
                    beforeLefts[beforeLefts.Count - 1] = i;
                    beforeRights[beforeRights.Count - 1] = (int)groupLeftIndexToGroupRightIndex[delIndex = i + offset] - offset;
                }
                if ("]" == value || ")" == value)
                {
                    if (0 == beforeLefts.Count)
                    {
                        Debug.LogError("括号错误：应去掉第" + commandI + "行第" + valueIndexs[i] + "列的右括号");
                        goto errorEnd;
                    }
                    beforeLefts.RemoveAt(beforeLefts.Count - 1);
                    beforeRights.RemoveAt(beforeRights.Count - 1);
                    beforeLevels.RemoveAt(beforeLevels.Count - 1);
                    if ((string)groupLeftStringToGroupRightString[groupStrings.Peek()] != values[i].Trim())
                    {
                        Debug.LogError("括号错误：第" + commandI + "行第" + valueIndexs[i] + "列匹配的括号不对");
                        goto errorEnd;
                    }
                    values[i] = groupStrings.Peek() + values[i--];
                    values.RemoveAt(groupIndexs.Peek());
                    addBrackets[groupIndexs.Peek()] += addBrackets[groupIndexs.Peek() + 1];
                    addBrackets.RemoveAt(groupIndexs.Peek() + 1);
                    groupStrings.Pop();
                    groupIndexs.Pop();
                    offset++;
                    continue;
                }
                if (CanSymbolToLevel(value))
                {
                    switch (getSymbolArgCount(value))
                    {
                        case 1:
                            addBrackets[i]++;
                            addBrackets[groupLeftIndexToGroupRightIndex.ContainsKey(i + 1) ? (int)groupLeftIndexToGroupRightIndex[i + 1] : i + 1]--;
                            if (-1 != beforeLevels[beforeLevels.Count - 1])
                            {
                                if (beforeLevels[beforeLevels.Count - 1] <= symbolToLevel(value))
                                {
                                    addBrackets[i]--;
                                    addBrackets[beforeLefts[beforeLefts.Count - 1]]++;
                                }
                                else
                                {
                                    addBrackets[beforeRights[beforeRights.Count - 1]]++;
                                    addBrackets[groupLeftIndexToGroupRightIndex.ContainsKey(i + 1) ? (int)groupLeftIndexToGroupRightIndex[i + 1] : i + 1]--;
                                }
                            }
                            beforeLefts[beforeLefts.Count - 1] = i;
                            beforeRights[beforeRights.Count - 1] = groupLeftIndexToGroupRightIndex.ContainsKey(i + 1 + offset) ? (int)groupLeftIndexToGroupRightIndex[i + 1 + offset] - offset : i + 1;
                            break;
                        case 2:
                            addBrackets[groupRightIndexToGroupLeftIndex.ContainsKey(i - 1 + offset) ? (int)groupRightIndexToGroupLeftIndex[i - 1 + offset] : i - 1]++;
                            addBrackets[groupLeftIndexToGroupRightIndex.ContainsKey(i + 1) ? (int)groupLeftIndexToGroupRightIndex[i + 1] - offset : i + 1]--;
                            if (-1 != beforeLevels[beforeLevels.Count - 1])
                            {
                                if (beforeLevels[beforeLevels.Count - 1] <= symbolToLevel(value))
                                {
                                    //“(”左移
                                    addBrackets[groupRightIndexToGroupLeftIndex.ContainsKey(i - 1 + offset) ? (int)groupRightIndexToGroupLeftIndex[i - 1 + offset] : i - 1]--;
                                    addBrackets[beforeLefts[beforeLefts.Count-1]]++;
                                }
                                else
                                {
                                    //“)”右移
                                    addBrackets[beforeRights[beforeRights.Count - 1]]++;
                                    addBrackets[groupLeftIndexToGroupRightIndex.ContainsKey(i + 1) ? (int)groupLeftIndexToGroupRightIndex[i + 1] : i + 1]--;
                                }
                             }
                            beforeLefts[beforeLefts.Count - 1] = groupRightIndexToGroupLeftIndex.ContainsKey(i - 1 + offset) ? (int)groupRightIndexToGroupLeftIndex[i - 1 + offset] : i - 1;
                            beforeRights[beforeRights.Count - 1] = groupLeftIndexToGroupRightIndex.ContainsKey(i + 1) ? (int)groupLeftIndexToGroupRightIndex[i + 1] : i + 1;
                            break;
                        case -1:
                            addBrackets[groupRightIndexToGroupLeftIndex.ContainsKey(i - 1 + offset) ? (int)groupRightIndexToGroupLeftIndex[i - 1 + offset] : i - 1]++;
                            addBrackets[i]--;
                            if (-1 != beforeLevels[beforeLevels.Count-1])
                            {
                                if (beforeLevels[beforeLevels.Count - 1] <= symbolToLevel(value))
                                {
                                    addBrackets[groupRightIndexToGroupLeftIndex.ContainsKey(i - 1 + offset) ? (int)groupRightIndexToGroupLeftIndex[i - 1 + offset] : i - 1]--;
                                    addBrackets[beforeLefts[beforeLefts.Count-1]]++;
                                }
                                else
                                {
                                    addBrackets[beforeRights[beforeRights.Count-1]]++;
                                    addBrackets[i]--;
                                }
                            }
                            beforeLefts[beforeLefts.Count - 1] = groupRightIndexToGroupLeftIndex.ContainsKey(i - 1 + offset) ? (int)groupRightIndexToGroupLeftIndex[i - 1 + offset] : i - 1;
                            beforeRights[beforeRights.Count - 1] = i;
                            break;
                        default:
                            break;
                    }
                    frontHasValue = false;
                    beforeLevels[beforeLevels.Count - 1] = symbolToLevel(value);
                }
                else if (command.CanCommandNameToRecommends(value))
                {
                    values.Insert(i++, "\x02");
                    addBrackets.Insert(i, 1);
                    rbracketPoses.Add(i);
                }
                else
                {
                    frontHasValue = true;
                }
                if (0 != rbracketPoses.Count)
                {
                    rbracketPoses[rbracketPoses.Count - 1]++;
                }
            }
            if (0 != groupStrings.Count)
            {
                Debug.LogError("括号错误：应去除第" + commandI + "行第" + groupIndexs.ToArray()[groupIndexs.ToArray().Length - 1] + "列的括号");
                goto errorEnd;
            }
            if (0 != rbracketPoses.Count)
            {
                addBrackets[values.Count - 1] -= rbracketPoses.Count;
            }
            //添加括号
            List<string> suffixCode = new List<string>();
            for (int i = 0; i < values.Count; i++)
            {
                if (0 < addBrackets[i])
                {
                    for (int j = 0; j < addBrackets[i]; j++)
                    {
                        suffixCode.Add("(");
                    }
                }
                suffixCode.Add(values[i]);
                if (0 > addBrackets[i])
                {
                    for (int j = 0; j > addBrackets[i]; j--)
                    {
                        suffixCode.Add(")");
                    }
                }
            }
            List<string> tempCode = new List<string>();
            //移动运算符
            bool[] canMove = new bool[suffixCode.Count];
            for (int i = 0; i < suffixCode.Count; i++)
            {
                canMove[i] = true;
            }
            for (int i = 0; i < suffixCode.Count; i++)
            {
                int groupCount = 0;
                int j = 0;
                while (canMove[i] && (CanSymbolToLevel(suffixCode[i]) || command.CanCommandNameToRecommends(suffixCode[i])))
                {
                    for (j = i; j < suffixCode.Count - 1; j++)
                    {
                        if ("(" == suffixCode[j + 1].Trim())
                        {
                            groupCount++;
                        }
                        if (")" == suffixCode[j + 1].Trim())
                        {
                            groupCount--;
                        }
                        if (0 > groupCount || 0 == groupCount && CanSymbolToLevel(suffixCode[j]) && CanSymbolToLevel(suffixCode[j + 1]) && symbolToLevel(suffixCode[j]) < symbolToLevel(suffixCode[j + 1]))
                        {
                            canMove[j] = false;
                            break;
                        }
                        string swapString = suffixCode[j];
                        suffixCode[j] = suffixCode[j + 1];
                        suffixCode[j + 1] = swapString;
                        bool swapCanMove = canMove[j];
                        canMove[j] = canMove[j + 1];
                        canMove[j + 1] = swapCanMove;
                    }
                }
            }
            //过滤非括号
            tempCode.Clear();
            for (int i = 0; i < suffixCode.Count; i++)
            {
                if (!Regex.Match(suffixCode[i], "^\\s*[()]\\s*$").Success) 
                {
                    tempCode.Add(suffixCode[i]);
                }
            }
            suffixCode = new List<string>(tempCode);
            //计算
            Stack<string> calcStack = new Stack<string>();
            Stack<int> symbolIndexStack = new Stack<int>();
            Stack<List<string>> argsStack = new Stack<List<string>>();
            bool canExecute = true;
            for (int i = 0; i < suffixCode.Count; i++)
            {
                if (CanSymbolToLevel(suffixCode[i]))
                {
                    if (2 == getSymbolArgCount(suffixCode[i]))
                    {
                        string sa = calcStack.Pop();
                        string s = calcStack.Pop();
                        if (argsStack.Count > 0)
                        {
                            argsStack.Peek().RemoveAt(argsStack.Peek().Count - 1);
                            argsStack.Peek().RemoveAt(argsStack.Peek().Count - 1);
                        }
                        calcStack.Push(getSymbolFunc(suffixCode[i])(s, sa, commandI, valueIndexs[symbolIndexStack.Peek()], valueIndexs[symbolIndexStack.Pop()], this).jsonValueTojsonString());
                        if (argsStack.Count > 0)
                        {
                            argsStack.Peek().Add(getSymbolFunc(suffixCode[i])(s, sa, commandI, valueIndexs[symbolIndexStack.Peek()], valueIndexs[symbolIndexStack.Pop()], this).jsonValueTojsonString());
                        }
                    }
                    else if(1 == getSymbolArgCount(suffixCode[i]))
                    {
                        string s = calcStack.Pop();
                        if (argsStack.Count > 0)
                        {
                            argsStack.Peek().RemoveAt(argsStack.Peek().Count - 1);
                        }
                        calcStack.Push(getSymbolFunc(suffixCode[i])(s, "", commandI, valueIndexs[symbolIndexStack.Peek()], -1, this).jsonValueTojsonString());
                        symbolIndexStack.Push(i);
                        if (argsStack.Count > 0)
                        {
                            argsStack.Peek().Add(getSymbolFunc(suffixCode[i])(s, "", commandI, valueIndexs[symbolIndexStack.Peek()], -1, this).jsonValueTojsonString());
                        }
                    }
                    else if(-1 == getSymbolArgCount(suffixCode[i]))
                    {
                        string s = calcStack.Pop();
                        if (argsStack.Count > 0)
                        {
                            argsStack.Peek().RemoveAt(argsStack.Peek().Count - 1);
                        }
                        calcStack.Push(getSymbolFunc(suffixCode[i])(s, "", commandI, valueIndexs[symbolIndexStack.Peek()], -1, this).jsonValueTojsonString());
                        symbolIndexStack.Push(i);
                        if (argsStack.Count > 0)
                        {
                            argsStack.Peek().Add(getSymbolFunc(suffixCode[i])(s, "", commandI, valueIndexs[symbolIndexStack.Peek()], -1, this).jsonValueTojsonString());
                        }
                    }
                }
                else if (command.CanCommandNameToRecommends(suffixCode[i]))
                {
                    command runCommand = command.stringToCommands(suffixCode[i]);
                    for (int j = 0; argsStack.Peek().Count > j; j++)
                    {
                        if (!runCommand.setValue(new jsonValue(argsStack.Peek()[j].Trim(), this), j, this))
                        {
                            break;
                        }
                    }
                    if (!runCommand.execute())
                    {
                        canExecute = false;
                        break;
                    }
                    for (; "\x02" != calcStack.Pop();) { }
                    if (null != runCommand.result)
                    {
                        calcStack.Push(runCommand.result.jsonValueTojsonString());
                    }
                }
                else if ("\x02" == suffixCode[i])
                {
                    argsStack.Push(new List<string>());
                    calcStack.Push(suffixCode[i]);
                }
                else
                {
                    if (argsStack.Count > 0) {
                        argsStack.Peek().Add(suffixCode[i]);
                    }
                    calcStack.Push(suffixCode[i]);
                    symbolIndexStack.Push(i);
                }
            }
            if (!canExecute)
            {
                break;
            }
            if (calcStack.Count > 1) {
                Debug.LogError("计算错误：第" + commandI + "行第" + valueIndexs[symbolIndexStack.Peek()] + "列的运算符应去除");
                goto errorEnd;
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
    public string commandsStr = "";
    private _runCommands _runCommands;
    public you u
    {
        get
        {
            return you.You;
        }
    }
    public static bool isDone = false;
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
//value = Regex.Match(commands[commandI].Substring(i), "").Value;
//value = commands[commandI].Substring(delimiterIndexs[i] + 1, delimiterIndexs[i + 1] - delimiterIndexs[i] - 1);//获取由空格分隔的内容，双引号内不算
/*
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
*/

