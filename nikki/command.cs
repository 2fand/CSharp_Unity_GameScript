using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using static change;
using static symbol;

public abstract class commandClasses
{
    public static readonly string[] classes = { "int", "float", "string", "bool", "null", "array", "object" };
    public static readonly Hashtable classIsHas;
    static commandClasses(){
        classIsHas = new Hashtable();
        for (int i = 0; i < classes.Length; i++) {
            classIsHas.Add(classes[i], true);
        }
    }
}

public abstract class command
{
    public static List<IEnumerator> funcs => trigger.funcs;
    public abstract string commandName { get; }
    public abstract string commandRecommend { get; }
#nullable enable
    public jsonValue? result = null;
#nullable disable
    public abstract bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues);
    public abstract bool execute();
    public static Hashtable stringCommands = new Hashtable();
    public static Hashtable CommandRecommends = new Hashtable();
    public readonly static Hashtable valueHelps = new Hashtable { { "none", "(进入转场|离开转场)：无" }, { "show", "进入转场：逐渐显示" }, { "hide", "离开转场：逐渐隐藏" }, { "fadein", "进入转场：淡入" }, { "fadeout", "离开转场：淡出" }, { "w", "朝向：上" }, { "a", "朝向：左" }, { "s", "朝向：下" }, { "d", "朝向：右" }, { "u", "朝向：你的朝向" }, { "l", "旋转方式：向左旋转90度" }, { "b", "旋转方式：往后旋转" }, { "r", "旋转方式：向右旋转90度" }, { "left", "同“l”" }, { "back", "同“b”" }, { "right", "同“r”" } };
    public static Hashtable stringOfTransitionModes = new Hashtable { { "show", change.transitionMode.show }, { "fadein", change.transitionMode.fadein }, { "hide", change.transitionMode.hide }, { "fadeout", change.transitionMode.fadeout }, { "enterNone", change.transitionMode.enterNone }, { "exitNone", change.transitionMode.exitNone } };
    public static Hashtable stringOfWASDs = new Hashtable { { "W", wasd.w }, { "w", wasd.w }, { "A", wasd.a }, { "a", wasd.a }, { "S", wasd.s }, { "s", wasd.s }, { "D", wasd.d }, { "d", wasd.d } };
    public static Hashtable stringOfBools = new Hashtable { { "true", true }, { "false", false } };
    public static Hashtable stringOfNull = new Hashtable { { "null", null } };
    public AudioClip[] sounds;
    static command()
    {
        CommandRecommends = new Hashtable();
        _runCommands.keyWordHas = new Hashtable();
        stringCommands = new Hashtable();
        Type[] types = typeof(command).Assembly.GetTypes();
        for (int i = 0; i < types.Length; i++)
        {
            if (null != types[i].BaseType && typeof(command) == types[i].BaseType)
            {
                stringCommands.Add(Activator.CreateInstance(types[i]).ConvertTo<command>().commandName, Activator.CreateInstance(types[i]).ConvertTo<command>());
                _runCommands.keyWordHas.Add(Activator.CreateInstance(types[i]).ConvertTo<command>().commandName, true);
                CommandRecommends.Add(Activator.CreateInstance(types[i]).ConvertTo<command>().commandName, Activator.CreateInstance(types[i]).ConvertTo<command>().commandRecommend);
            }
        }
    }
    public static string getRealStr(string str)
    {
        if (2 <= str.Length && '\"' == str[0] && '\"' == str[str.Length - 1])
        {
            str = str.Substring(1, str.Length - 2);
        }
        return str;
    }
    public static transitionMode stringToTransitionModes(string str){
        str = str.ToLower().Trim();
        return (transitionMode)stringOfTransitionModes[str];
    }
    public static wasd stringToWASDs(string str){
        str = str.ToLower().Trim();
        return (wasd)stringOfWASDs[str];
    }
    public static bool stringToBools(string str){
        str = str.ToLower().Trim();
        return (bool)stringOfBools[str];
    }
    public static object stringToNull(string str) {
        str = str.ToLower().Trim();
        return stringOfNull[str];
    }
#nullable enable
    public static string valueNameToHelp(string str)
#nullable disable
    {
        str = str.ToLower().Trim();
        return (string)valueHelps[str];
    }
    public static bool CanStrToTransitionModes(string str)
    {
        return stringOfTransitionModes.ContainsKey(str);
    }
    public static bool CanStrToWASDs(string str)
    {
        return stringOfWASDs.ContainsKey(str);
    }
    public static bool CanStrToBools(string str)
    {
        return stringOfBools.ContainsKey(str);
    }
    public static bool CanStrToNull(string str)
    {
        return stringOfNull.ContainsKey(str);
    }
    public static bool CanVauleNameToHelp(string str)
    {
        return valueHelps.ContainsKey(str);
    }
    public static bool CanCommandNameToRecommends(string str)
    {
        return CommandRecommends.ContainsKey(str);
    }
#nullable enable
    public static command stringToCommands(string str)
    {
        str = str.ToLower().Trim();
        return (command)stringCommands[str];
    }
    public static string commandNameToRecommends(string str)
    {
        str = str.ToLower().Trim();
        return (string)CommandRecommends[str];
    }
}
public class tele : command
{
    transitionMode? exitMode = null;
    transitionMode? enterMode = null;
    float? exitTime = null;
    float? enterTime = null;
    float tempTime = 0;
    string? worldName = null;
    int? teleX = null;
    int? teleY = null;
    int? closeSoundIndex = null;
    int? teleSoundIndex = null;
    float? teleHigh = null;
    wasd? face = null;
    bool? autoClearMenu = null;
#nullable disable
    public override string commandName => "tele";
    public override string commandRecommend => "tele命令：让玩家传送至指定地点(命令格式：tele 退出转场 退出转场时间 进入转场 进入转场时间 [世界名 = \"nexus\"] [传送x坐标 = 0] [传送y坐标 = 0] [朝向 = 你的朝向] [传送时音效在sounds的索引 = 0(可为null)] [传送后音效在sounds的索引 = 0(可为null)] [是否自动清空当前打开的菜单 = true])";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        switch (valueNumber)
        {
            case 0:
                exitMode = CanStrToTransitionModes(value) ? stringToTransitionModes(value) : change.transitionMode.exitNone;
                break;
            case 1:
                exitTime = float.TryParse(value, out tempTime) ? tempTime : null;
                break;
            case 2:
                enterMode = CanStrToTransitionModes(value) ? stringToTransitionModes(value) : change.transitionMode.exitNone;
                break;
            case 3:
                enterTime = float.TryParse(value, out tempTime) ? tempTime : null;
                break;
            case 4:
                worldName = getRealStr(value);
                break;
            case 5:
                teleX = int.Parse(value);
                break;
            case 6:
                teleY = int.Parse(value);
                break;
            case 7:
                teleHigh = float.Parse(value);
                break;
            case 8:
                if ("u" == value.getString().ToLower())
                {
                    face = you.face;
                    break;
                }
                face = CanStrToWASDs(value) ? stringToWASDs(value) : (wasd)int.Parse(value);
                break;
            case 9:
                if ("null" != value.getString().ToLower())
                {
                    closeSoundIndex = int.Parse(value);
                }
                break;
            case 10:
                if ("null" != value.getString().ToLower())
                {
                    teleSoundIndex = int.Parse(value);
                }
                break;
            case 11:
                if (CanStrToBools(value))
                {
                    autoClearMenu = stringToBools(value);
                }
                break;
            default:
                return false;
        }
        return true;
    }
    public override bool execute()
    {
        if (autoClearMenu ?? true)
        {
            funcs.Add(you.You.clearMenu());
        }
        funcs.Add(you.tele(exitMode ?? change.transitionMode.hide, exitTime ?? Game.exitTime, enterMode ?? change.transitionMode.show, enterTime ?? Game.enterTime, worldName ?? "nexus", teleX ?? 0, teleY ?? 0, teleHigh ?? 0, face ?? wasd.s, null == sounds ? null : sounds[closeSoundIndex ?? 0] ?? sounds[0], null == sounds ? null : sounds[teleSoundIndex ?? 0] ?? sounds[0]));
        return true;
    }
}
public class note : command
{
    public override string commandName => "#";
    public override string commandRecommend => "#命令：用来注释命令(命令格式：# ...)";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        return false;
    }
    public override bool execute()
    {
        return true;
    }
}
public class help : command
{
#nullable enable
    string? str = null;
#nullable disable
    public override string commandName => "help";
    public override string commandRecommend => "help命令：了解命令的主要作用(命令格式：help 命令名称)";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        if (0 == valueNumber && null != commandNameToRecommends(getRealStr(value)))
        {
            str = (string)commandNameToRecommends(getRealStr(value));
            return true;
        }
        return false;
    }
    public override bool execute()
    {
        throw new System.NotImplementedException();
    }
}
public class _move : command
{
    float? tempSpeed = null;
    wasd? face = null;
    int step;
    bool? autoClearMenu = null;
    public override string commandName => "move";
    public override string commandRecommend => "move命令：强制让玩家移动(命令格式：move [速度]朝向 [步数 = 1] [是否自动清空当前打开的菜单 = true])";
    static string easyNum(string stringNum)
    {
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
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        if (0 == valueNumber)
        {
            int delimiterIndex = -1;
            for (int j = 0; j < value.getString().Length; j++)
            {
                if (char.IsLetter(value.getString()[j]))
                {
                    delimiterIndex = j;
                    break;
                }
            }
            if (-1 == delimiterIndex)
            {
                delimiterIndex = value.getString().Length;
            }
            if (0 != delimiterIndex)
            {
                tempSpeed = float.Parse(easyNum(value.getString().Substring(0, delimiterIndex)));
            }
            if (CanStrToWASDs(value.getString().Substring(delimiterIndex)))
            {
                face = ("u" == value.getString().Substring(delimiterIndex) || "U" == value.getString().Substring(delimiterIndex) ? you.face : (wasd)stringToWASDs(value.getString().Substring(delimiterIndex)));
            }
        }
        else if (1 == valueNumber)
        {
            step = int.Parse(value);
        }
        else
        {
            if (CanStrToBools(value)) {
                autoClearMenu = stringToBools(value);
            }
            return false;
        }
        return true;
    }
    public override bool execute()
    {
        if (autoClearMenu ?? true)
        {
            funcs.Add(you.You.clearMenu());
        }
        if (0 > tempSpeed)
        {
            tempSpeed *= -1;
            step *= -1;
        }
        funcs.Add(you.You.move(face ?? you.face, step, tempSpeed ?? 1));
        return true;
    }
}
public class _show : command
{
    bool? autoClearMenu = null;
    public override string commandName => "show";
    public override string commandRecommend => "show命令：显示玩家(命令格式：show  [是否自动清空当前打开的菜单 = true])";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        if (CanStrToBools(value))
        {
            autoClearMenu = (bool)stringToBools(value);
            return true;
        }
        return false;
    }
    public override bool execute()
    {
        if (autoClearMenu ?? true)
        {
            funcs.Add(you.You.clearMenu());
        }
        funcs.Add(you.You.show());
        return true;
    }
}
public class _hide : command
{
    bool? autoClearMenu = null;
    public override string commandName => "hide";
    public override string commandRecommend => "hide命令：隐藏玩家(命令格式：hide  [是否自动清空当前打开的菜单 = true])";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        if (CanStrToBools(value))
        {
            autoClearMenu = (bool)stringToBools(value);
            return true;
        }
        return false;
    }
    public override bool execute()
    {
        if (autoClearMenu ?? true)
        {
            funcs.Add(you.You.clearMenu());
        }
        funcs.Add(you.You.hide());
        return true;
    }
}

public class _play : command
{
    int? soundIndex = null;
    bool? isWaitSoundEnd = null;
    bool? autoClearMenu = null;
    int ia;
    public override string commandName => "play";
    public override string commandRecommend => "play命令：播放一段声音(命令格式：play [声音在sounds的索引 = 0] [是否等待声音结束 = false]  [是否自动清空当前打开的菜单 = false])";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        switch (valueNumber)
        {
            case 0:
                soundIndex = int.Parse(value);
                break;
            case 1:
                if (CanStrToBools(value))
                {
                    isWaitSoundEnd = (bool)stringToBools(value);
                }
                else if (int.TryParse(value, out ia))
                {
                    isWaitSoundEnd = 0 != ia;
                }
                break;
            case 2:
                if (CanStrToBools(value))
                {
                    autoClearMenu = stringToBools(value);
                }
                break;
            default:
                return false;
        }
        return true;
    }
    public override bool execute()
    {
        if (autoClearMenu ?? false)
        {
            funcs.Add(you.You.clearMenu());
        }
        if (null != sounds)
        {
            funcs.Add(you.play(sounds[soundIndex ?? 0] ?? sounds[0], isWaitSoundEnd ?? false));
        }
        return true;
    }
}

public class turn : command
{
    wasd? face;
    Hashtable turnModes = new Hashtable { { "l", 3 }, { "left", 3 }, { "b", 2 }, { "back", 2 }, { "r", 1 }, { "right", 1 } };
    wasd[] turnArray = { wasd.w, wasd.d, wasd.s, wasd.a };
    int[] rturnArray = { 0, 3, 2, 1 };
    bool? autoClearMenu = null;
    public override string commandName => "turn";
    public override string commandRecommend => "turn命令：改变玩家的朝向(命令格式：turn [朝向 = s]|[玩家转的方式 = (l(eft)|b(ack)|r(ight))]  [是否自动清空当前打开的菜单 = true]))";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        switch (valueNumber)
        {
            case 0:
                if ("u" == value || "U" == value)
                {
                    face = you.face;
                }
                else if (CanStrToWASDs(value))
                {
                    face = (wasd)stringToWASDs(value);
                }
                else if (turnModes.ContainsKey(value.getString().ToLower()))
                {
                    face = turnArray[(rturnArray[(int)you.face] + (int)turnModes[value.getString().ToLower()]) % 4];
                }
                break;
            default:
                if (CanStrToBools(value))
                {
                    autoClearMenu = (bool)stringToBools(value);
                }
                return false;
        }
        return true;
    }
    public override bool execute()
    {
        if (autoClearMenu ?? true)
        {
            funcs.Add(you.You.clearMenu());
        }
        funcs.Add(you.turn(face ?? wasd.s));
        return true;
    }
}

public class stop : command
{
    public override string commandName => "stop";
    public override string commandRecommend => "stop命令：停止发出声音(命令格式：stop)";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        return true;
    }
    public override bool execute()
    {
        funcs.Add(you.stop());
        return true;
    }
}

public class wait : command
{
    float waitTime;
    public override string commandName => "wait";
    public override string commandRecommend => "wait命令：等待一段时间(命令格式：wait [等待时间 = 1])";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        value = new jsonValue(value, commandsValues);
        waitTime = float.Parse(value);
        return false;
    }
    public override bool execute()
    {
        funcs.Add(you.wait(waitTime));
        return true;
    }
}

public class use : command
{
#nullable enable
    string? itemName = null;
#nullable disable
    bool? autoClearMenu = null;
    public override string commandName => "use";
    public override string commandRecommend => "use命令：使用物品栏里第一个道具名相同的道具(命令格式：use [道具名 = \"default\"]  [是否自动清空当前打开的菜单 = true])";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        value = new jsonValue(value, commandsValues);
        if (0 == valueNumber)
        {
            itemName = value;
        }
        else
        {
            if (CanStrToBools(value))
            {
                autoClearMenu = stringToBools(value);
            }
            return false;
        }
        return true;
    }
    public override bool execute()
    {
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
        return true;
    }
}

public class print : command
{
#nullable enable
    string? str = null;
#nullable disable
    public override string commandName => "print";
    public override string commandRecommend => "print命令：输出一些信息(命令格式：print (信息))";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        str = getRealStr(new jsonValue(value, commandsValues));
        return false;
    }
    public override bool execute()
    {
        funcs.Add(trigger.debugStr(str ?? ""));
        return true;
    }
}

public class value : command
{
#nullable enable
    string? str = null;
#nullable disable
    public override string commandName => "value";
    public override string commandRecommend => "value命令：查看关于某些特殊类型变量的详细介绍(命令格式：value)";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        if (CanVauleNameToHelp(getRealStr(value)))
        {
            str = valueNameToHelp(getRealStr(value));
        }
        return false;
    }
    public override bool execute()
    {
        funcs.Add(trigger.debugStr(str ?? ""));
        return true;
    }
}

public class _goto : command
{
    public override string commandName => "goto";
    public override string commandRecommend => "goto命令：跳转到某一行(命令格式：goto (标签(标签格式：“(标签名):”)))";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        if (!commandsValues.labels.ContainsKey(getRealStr(value)))
        {
            Debug.Log("标签错误：标签不存在");
            return false;
        }
        commandsValues.commandI = (int)commandsValues.labels[getRealStr(value)];
        return false;
    }
    public override bool execute()
    {
        return true;
    }
}

public class exit : command
{
    bool? autoClearMenu = null;
    public override string commandName => "exit";
    public override string commandRecommend => "exit命令：退出游戏(命令格式：exit  [是否自动清空当前打开的菜单 = false])";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        if (CanStrToBools(value))
        {
            autoClearMenu = (bool)stringToBools(value);
        }
        return false;
    }
    public override bool execute()
    {
        if (0 != you.Menus.Count && (autoClearMenu ?? false))
        {
            funcs.Add(you.You.clearMenu());
        }
        funcs.Add(you.exit());
        return true;
    }
}

public class close : command
{
    public override string commandName => "close";
    public override string commandRecommend => "close命令：跳出当前菜单(命令格式：close)";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        return false;
    }
    public override bool execute()
    {
        funcs.Add(you.You.closeMenu());
        return true;
    }
}

public class clear : command
{
    public override string commandName => "clear";
    public override string commandRecommend => "clear命令：清空当前的菜单(命令格式：clear)";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        return false;
    }
    public override bool execute()
    {
        funcs.Add(you.You.clearMenu());
        return true;
    }
}
public class customsizeCommand : command
{
    public List<string> commands;
    public List<string> args;
    public Hashtable realArgs;
    public override string commandName => "";
    public override string commandRecommend => "";
    public override bool setValue(jsonValue value, int valueNumber, _runCommands commandsValues)
    {
        if (0 == valueNumber)
        {
            commands = new List<string>();
            args = new List<string>();
            realArgs = new Hashtable();
            jsonValue[] commandsArray =  jsonValue.getArray(value.getIndexValue(0).ConvertTo<hashType>());
            jsonValue[] argsArray = jsonValue.getArray(value.getIndexValue(0).ConvertTo<hashType>());
            for (int i = 0; i < commandsArray.Length; i++) 
            {
                commands.Add(commandsArray[i].getString()); 
            }
            for (int i = 0; i < argsArray.Length; i++)
            {
                commands.Add(argsArray[i].getString());
            }
            return true;
        }
        if (args.Count >= valueNumber)
        {
            realArgs.Add(args[valueNumber - 1], value);
        }
        return false;
    }
    public override bool execute()
    {
        trigger.runCommands(commands.ToArray());
        return true;
    }
}
