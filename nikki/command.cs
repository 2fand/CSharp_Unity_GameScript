using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using static change;

public abstract class command
{
    public static List<IEnumerator> funcs => trigger.funcs;
    public abstract string commandName { get; }
    public abstract string commandRecommend { get; }
    public abstract bool setValue(string value, int valueNumber, trigger _trigger);
    public abstract bool execute();
    public static Hashtable stringCommands = new Hashtable();
    public static Hashtable CommandRecommends = new Hashtable();
    public readonly static Hashtable valueHelps = new Hashtable { { "none", "(进入转场|离开转场)：无" }, { "show", "进入转场：逐渐显示" }, { "hide", "离开转场：逐渐隐藏" }, { "fadein", "进入转场：淡入" }, { "fadeout", "离开转场：淡出" }, { "w", "朝向：上" }, { "a", "朝向：左" }, { "s", "朝向：下" }, { "d", "朝向：右" }, { "u", "朝向：你的朝向" }, { "l", "旋转方式：向左旋转90度" }, { "b", "旋转方式：往后旋转" }, { "r", "旋转方式：向右旋转90度" }, { "left", "同“l”" }, { "back", "同“b”" }, { "right", "同“r”" } };
    public static Hashtable stringOfEnterModes = new Hashtable { { "show", change.enterMode.show }, { "fadein", change.enterMode.fadein }};
    public static Hashtable stringOfExitModes = new Hashtable { { "hide", change.exitMode.hide }, { "fadeout", change.exitMode.fadeout } };
    public static Hashtable stringOfWASDs = new Hashtable { { "W", wasd.w }, { "w", wasd.w }, { "A", wasd.a }, { "a", wasd.a }, { "S", wasd.s }, { "s", wasd.s }, { "D", wasd.d }, { "d", wasd.d } };
    public static Hashtable stringOfBools = new Hashtable { { "true", true }, { "false", false } };
    public static Hashtable stringOfNull = new Hashtable { { "null", null } };
    public AudioClip[] sounds;
    static command()
    {
        Type[] types = typeof(command).Assembly.GetTypes();
        for (int i = 0; i < types.Length; i++) { 
            if (null != types[i].BaseType && typeof(command) == types[i].BaseType)
            {
                stringCommands.Add(Activator.CreateInstance(types[i]).ConvertTo<command>().commandName, Activator.CreateInstance(types[i]).ConvertTo<command>());
                CommandRecommends.Add(Activator.CreateInstance(types[i]).ConvertTo<command>().commandName, Activator.CreateInstance(types[i]).ConvertTo<command>().commandRecommend);
            }
        }
    }
    public static enterMode? stringToEnterModes(string str){
        str = str.ToLower();
        if (stringOfEnterModes.ContainsKey(str))
        {
            return (enterMode)stringOfEnterModes[str];
        }
        return null;
    }
    public static exitMode? stringToExitModes(string str){
        str = str.ToLower();
        if (stringOfExitModes.ContainsKey(str))
        {
            return (exitMode)stringOfExitModes[str];
        }
        return null;
    }
    public static wasd? stringToWASDs(string str){
        str = str.ToLower();
        if (stringOfWASDs.ContainsKey(str))
        {
            return (wasd)stringOfWASDs[str];
        }
        return null;
    }
    public static bool? stringToBools(string str){
        str = str.ToLower();
        if (stringOfBools.ContainsKey(str))
        {
            return (bool)stringOfBools[str];
        }
        return null;
    }
    public static bool? stringToNull(string str) {
        str = str.ToLower();
        if (stringOfNull.ContainsKey(str))
        {
            return null;
        }
        return false;
    }
#nullable enable
    public static string? valueNameToHelp(string str)
#nullable disable
    {
        str = str.ToLower();
        if (valueHelps.ContainsKey(str))
        {
            return (string)valueHelps[str];
        }
        return null;
    }
    public static bool CanStrToEnterModes(string str)
    {
        return null != stringToEnterModes(str);
    }
    public static bool CanStrToExitModes(string str)
    {
        return null != stringToExitModes(str);
    }
    public static bool CanStrToWASDs(string str)
    {
        return null != stringToWASDs(str);
    }
    public static bool CanStrToBools(string str)
    {
        return null != stringToBools(str);
    }
    public static bool CanStrToNull(string str)
    {
        return null == stringToNull(str);
    }
    public static bool CanVauleNameToHelp(string str)
    {
        return null != valueNameToHelp(str);
    }
#nullable enable
    public static command? stringToCommands(string str)
    {
        str = str.ToLower();
        if (stringCommands.ContainsKey(str))
        {
            return (command)stringCommands[str];
        }
        return null;
    }
    public static string? commandNameToRecommends(string str)
    {
        str = str.ToLower();
        if (CommandRecommends.ContainsKey(str))
        {
            return (string)CommandRecommends[str];
        }
        return null;
    }
}
public class tele : command
{
    exitMode? exitMode = null;
    enterMode? enterMode = null;
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
        switch (valueNumber)
        {
            case 0:
                exitMode = null != stringToExitModes(value) ? stringToExitModes(value) : change.exitMode.none;
                break;
            case 1:
                exitTime = float.TryParse(value, out tempTime) ? tempTime : null;
                break;
            case 2:
                enterMode = null != stringToEnterModes(value) ? stringToEnterModes(value) : change.enterMode.none;
                break;
            case 3:
                enterTime = float.TryParse(value, out tempTime) ? tempTime : null;
                break;
            case 4:
                worldName = value;
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
                if ("u" == value.ToLower())
                {
                    face = you.face;
                    break;
                }
                face = null != stringToWASDs(value) ? stringToWASDs(value) : (wasd)int.Parse(value);
                break;
            case 9:
                if ("null" != value.ToLower())
                {
                    closeSoundIndex = int.Parse(value);
                }
                break;
            case 10:
                if ("null" != value.ToLower())
                {
                    teleSoundIndex = int.Parse(value);
                }
                break;
            case 11:
                if (null != stringToBools(value))
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
        funcs.Add(you.tele(exitMode ?? change.exitMode.hide, exitTime ?? Game.exitTime, enterMode ?? change.enterMode.show, enterTime ?? Game.enterTime, worldName ?? "nexus", teleX ?? 0, teleY ?? 0, teleHigh ?? 0, face ?? wasd.s, null == sounds ? null : sounds[closeSoundIndex ?? 0] ?? sounds[0], null == sounds ? null : sounds[teleSoundIndex ?? 0] ?? sounds[0]));
        return true;
    }
}
public class note : command
{
    public override string commandName => "#";
    public override string commandRecommend => "#命令：用来注释命令(命令格式：# ...)";
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
        return false;
    }
    public override bool execute()
    {
        return false;
    }
}
public class help : command
{
#nullable enable
    string? str = null;
#nullable disable
    public override string commandName => "help";
    public override string commandRecommend => "help命令：了解命令的主要作用(命令格式：help 命令名称)";
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
        if (0 == valueNumber && null != commandNameToRecommends(value))
        {
            str = (string)commandNameToRecommends(value);
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
        if (0 == valueNumber)
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
            if (CanStrToWASDs(value.Substring(delimiterIndex)))
            {
                face = ("u" == value.Substring(delimiterIndex) || "U" == value.Substring(delimiterIndex) ? you.face : (wasd)stringToWASDs(value.Substring(delimiterIndex)));
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
        if (null != stringToBools(value))
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
        if (null != stringToBools(value))
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
        switch (valueNumber)
        {
            case 0:
                soundIndex = int.Parse(value);
                break;
            case 1:
                if (null != stringToBools(value))
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
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
                else if (turnModes.ContainsKey(value.ToLower()))
                {
                    face = turnArray[(rturnArray[(int)you.face] + (int)turnModes[value.ToLower()]) % 4];
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
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

public class debug : command
{
#nullable enable
    string? str = null;
#nullable disable
    public override string commandName => "debug";
    public override string commandRecommend => "debug命令：输出一些信息(命令格式：debug (信息))";
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
        str = value;
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
        if (CanVauleNameToHelp(value))
        {
            str = valueNameToHelp(value);
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
        if (!_trigger.labels.ContainsKey(value))
        {
            Debug.Log("标签错误：标签不存在");
            return false;
        }
        _trigger.commandI = (int)_trigger.labels[value];
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
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
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
        return false;
    }
    public override bool execute()
    {
        funcs.Add(you.You.clearMenu());
        return true;
    }
}

public class set : command
{
    public override string commandName => "set";
    public override string commandRecommend => "set命令(同=运算符)：设置变量，定义变量(命令格式：set 变量 值)";//数 字符串 枚举 对象 数组 布尔
    public override bool setValue(string value, int valueNumber, trigger _trigger)
    {
        return false;
    }
    public override bool execute()
    {
        funcs.Add(you.You.clearMenu());
        return true;
    }
}
