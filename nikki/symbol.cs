using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class symbol
{
    public readonly static Hashtable symbolPriors = new Hashtable { { ".", 0 }, { "()", 0 }, { "[]", 0 }, { "?[]", 0 }, { "?.", 0 }, { ".*++", 0 }, { ".*--", 0 }, { "new", 0 }, { "typeof", 0 }, { "++", 1 }, { "--", 1 }, { "+", 1 }, { "-", 1 }, { "~", 1 }, { "*", 2 }, { "(class)", 2 }, { "/", 2 }, { "%", 2 }, { "//", 2 }, { "**", 2 }, { ".*+", 3 }, { ".*-", 3 }, { "<<", 4 }, { ">>", 4 }, { "<", 5 }, { ">", 5 }, { "<=", 5 }, { ">=", 5 }, { "is", 5 }, { "as", 5 }, { "==", 6 }, { "!=", 6 }, { "&", 7 }, { "^", 8 }, { "|", 9 }, { "&&", 10 }, { "||", 11 }, { "!", 12 }, { "??", 13 }, { "?", 14 }, { ":", 14 }, { "=", 15 }, { "+=", 15 }, { "-=", 15 }, { "*=", 15 }, { "/=", 15 }, { "**=", 15 }, { "%=", 15 }, { "&&=", 15 }, { "||=", 15 }, { "&=", 15 }, { "|=", 15 }, { "^=", 15 }, { "//=", 15 }, { ",", 16 } };
    public readonly static Hashtable symbolArgCounts = new Hashtable { { ".", 2 }, { "()", 0 }, { "[]", 0 }, { "?[]", 2 }, { "?.", 2 }, { ".*++", -1 }, { ".*--", -1 }, { "new", 1 }, { "typeof", 1 }, { "++", 1 }, { "--", 1 }, { "+", 1 }, { "-", 1 }, { "!", 1 }, { "~", 1 }, { "*", 2 }, { "(class)", 1 }, { "/", 2 }, { "%", 2 }, { "//", 2 }, { "**", 2 }, { ".*+", 2 }, { ".*-", 2 }, { "<<", 2 }, { ">>", 2 }, { "<", 2 }, { ">", 2 }, { "<=", 2 }, { ">=", 2 }, { "is", 2 }, { "as", 2 }, { "==", 2 }, { "!=", 2 }, { "&", 2 }, { "^", 2 }, { "|", 2 }, { "&&", 2 }, { "||", 2 }, { "??", 2 }, { "?", 2 }, { ":", 1 }, { "=", 2 }, { "+=", 2 }, { "-=", 2 }, { "*=", 2 }, { "/=", 2 }, { "**=", 2 }, { "%=", 2 }, { "&&=", 2 }, { "||=", 2 }, { "&=", 2 }, { "|=", 2 }, { "^=", 2 }, { "//=", 2 }, { ",", 2 }, { ".*!", -1 }, };
    public readonly static Hashtable symbolFuncs;
    public abstract string symbolName { get; }
    public abstract int symbolPrior { get; }
    public abstract int symbolArgCount { get; }
    public abstract Func<string, string, _runCommands, jsonValue> symbolFunc { get; }
    static symbol()
    {
        symbolFuncs = new Hashtable();
        symbolArgCounts = new Hashtable();
        symbolPriors = new Hashtable();
        Type[] types = typeof(symbol).Assembly.GetTypes();
        for (int i = 0; i < types.Length; i++) {
            if (null != types[i].BaseType && typeof(symbol) == types[i].BaseType)
            {
                symbolPriors.Add(Activator.CreateInstance(types[i]).ConvertTo<symbol>().symbolName, Activator.CreateInstance(types[i]).ConvertTo<symbol>().symbolPrior);
                symbolArgCounts.Add(Activator.CreateInstance(types[i]).ConvertTo<symbol>().symbolName, Activator.CreateInstance(types[i]).ConvertTo<symbol>().symbolArgCount);
                symbolFuncs.Add(Activator.CreateInstance(types[i]).ConvertTo<symbol>().symbolName, Activator.CreateInstance(types[i]).ConvertTo<symbol>().symbolFunc);
            }
        }
    }
    public static int symbolToLevel(string str)
    {
        str = str.ToLower().Trim();
        return (int)symbolPriors[str];
    }
    public static int getSymbolArgCount(string str)
    {
        str = str.ToLower().Trim();
        return (int)symbolArgCounts[str];
    }
#nullable enable
    public static Func<string, string, _runCommands, jsonValue> getSymbolFunc(string str)
    {
        str = str.ToLower().Trim();
        return (Func<string, string, _runCommands, jsonValue>)symbolFuncs[str];
    }
    public static bool CanSymbolToLevel(string str)
    {
        return symbolPriors.ContainsKey(str);
    }
    public static bool CanGetSymbolArgCount(string str)
    {
        return symbolArgCounts.ContainsKey(str);
    }
    public static bool CanGetSymbolFunc(string str)
    {
        return symbolFuncs.ContainsKey(str);
    }
}
public class setSymbol : symbol
{
    public override string symbolName => "=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        set setCommand = new set();
        setCommand.setValue(s, 0, commandValues);
        setCommand.setValue(sa, 1, commandValues);
        setCommand.execute();
        return commandValues.getVar(s).ConvertTo<jsonValue>();
    };
}

public class notSymbol : symbol
{
    public override string symbolName => "!";
    public override int symbolArgCount => 1;
    public override int symbolPrior => 12;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class addSymbol : symbol
{
    public override string symbolName => ".*+";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 3;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (jsonValue.valueClass.array == json.ThisClass && jsonValue.valueClass.array == jsona.ThisClass)
        {
            json.catArray(jsona);
        }
        if (("int" == json.getRealType() || "float" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "float" == jsona.getRealType() || "bool" == jsona.getRealType())){
            if (json.getValue() is bool)
            {
                json.setValue(json.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            if (jsona.getValue() is bool)
            {
                jsona.setValue(jsona.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            return new jsonValue(json.getValue().ConvertTo<float>() + jsona.getValue().ConvertTo<float>());
        }
        if ("string" == json.getRealType() && "string" == jsona.getRealType())
        {
            return new jsonValue(json.getValue().ConvertTo<string>() + jsona.getValue().ConvertTo<string>());
        }
        throw new Exception("类型不匹配错误");
    };
}

public class subSymbol : symbol
{
    public override string symbolName => ".*-";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 3;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (("int" == json.getRealType() || "float" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "float" == jsona.getRealType() || "bool" == jsona.getRealType()))
        {
            if (json.getValue() is bool)
            {
                json.setValue(json.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            if (jsona.getValue() is bool)
            {
                jsona.setValue(jsona.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            return new jsonValue(json.getValue().ConvertTo<float>() - jsona.getValue().ConvertTo<float>());
        }
        throw new Exception("类型不匹配错误");
    };
}

public class timSymbol : symbol
{
    public override string symbolName => "*";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 2;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (("int" == json.getRealType() || "float" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "float" == jsona.getRealType() || "bool" == jsona.getRealType()))
        {
            if (json.getValue() is bool)
            {
                json.setValue(json.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            if (jsona.getValue() is bool)
            {
                jsona.setValue(jsona.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            return new jsonValue(json.getValue().ConvertTo<float>() * jsona.getValue().ConvertTo<float>());
        }
        throw new Exception("类型不匹配错误");
    };
}

public class divSymbol : symbol
{
    public override string symbolName => "/";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 2;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (("int" == json.getRealType() || "float" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "float" == jsona.getRealType() || "bool" == jsona.getRealType()))
        {
            if (json.getValue() is bool)
            {
                json.setValue(json.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            if (jsona.getValue() is bool)
            {
                jsona.setValue(jsona.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            return new jsonValue(json.getValue().ConvertTo<float>() / jsona.getValue().ConvertTo<float>());
        }
        throw new Exception("类型不匹配错误");
    };
}

public class divToIntSymbol : symbol
{
    public override string symbolName => "//";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 2;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (("int" == json.getRealType() || "float" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "float" == jsona.getRealType() || "bool" == jsona.getRealType()))
        {
            if (json.getValue() is bool)
            {
                json.setValue(json.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            if (jsona.getValue() is bool)
            {
                jsona.setValue(jsona.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            return new jsonValue(json.getValue().ConvertTo<int>() / jsona.getValue().ConvertTo<float>());
        }
        throw new Exception("类型不匹配错误");
    };
}

public class powSymbol : symbol
{
    public override string symbolName => "**";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 2;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (("int" == json.getRealType() || "float" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "float" == jsona.getRealType() || "bool" == jsona.getRealType()))
        {
            if (json.getValue() is bool)
            {
                json.setValue(json.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            if (jsona.getValue() is bool)
            {
                jsona.setValue(jsona.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            return new jsonValue(Mathf.Pow(json.getValue().ConvertTo<float>(), jsona.getValue().ConvertTo<float>()));
        }
        throw new Exception("类型不匹配错误");
    };
}

public class andSymbol : symbol
{
    public override string symbolName => "&&";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 10;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (("int" == json.getRealType() || "float" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "float" == jsona.getRealType() || "bool" == jsona.getRealType()))
        {
            if (!(json.getValue() is bool))
            {
                json.setValue(json.getValue().ConvertTo<float>() != 0);
            }
            if (!(jsona.getValue() is bool))
            {
                jsona.setValue(jsona.getValue().ConvertTo<float>() != 0);
            }
            return new jsonValue(json.getValue().ConvertTo<bool>() && jsona.getValue().ConvertTo<bool>());
        }
        throw new Exception("类型不匹配错误");
    };
}

public class orSymbol : symbol
{
    public override string symbolName => "||";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 11;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (("int" == json.getRealType() || "float" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "float" == jsona.getRealType() || "bool" == jsona.getRealType()))
        {
            if (!(json.getValue() is bool))
            {
                json.setValue(json.getValue().ConvertTo<float>() != 0);
            }
            if (!(jsona.getValue() is bool))
            {
                jsona.setValue(jsona.getValue().ConvertTo<float>() != 0);
            }
            return new jsonValue(json.getValue().ConvertTo<bool>() || jsona.getValue().ConvertTo<bool>());
        }
        throw new Exception("类型不匹配错误");
    };
}

public class addsetSymbol : symbol
{
    public override string symbolName => "+=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc(".*+")(s, sa, commandValues), commandValues);
    };
}

public class subsetSymbol : symbol
{
    public override string symbolName => "-=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc(".*-")(s, sa, commandValues), commandValues);
    };
}

public class timsetSymbol : symbol
{
    public override string symbolName => "*=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("*")(s, sa, commandValues), commandValues);
    };
}

public class divsetSymbol : symbol
{
    public override string symbolName => "/=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("/")(s, sa, commandValues), commandValues);
    };
}

public class divToIntsetSymbol : symbol
{
    public override string symbolName => "//=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("//")(s, sa, commandValues), commandValues);
    };
}

public class powsetSymbol : symbol
{
    public override string symbolName => "**=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("**")(s, sa, commandValues), commandValues);
    };
}

public class orsetSymbol : symbol
{
    public override string symbolName => "||=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("||")(s, sa, commandValues), commandValues);
    };
}

public class andsetSymbol : symbol
{
    public override string symbolName => "&&=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("&&")(s, sa, commandValues), commandValues);
    };
}

public class equalSymbol : symbol
{
    public override string symbolName => "==";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 6;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue(new jsonValue(s).rootValue == new jsonValue(sa).rootValue);
    };
}

public class notEqualSymbol : symbol
{
    public override string symbolName => "!=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 6;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue(new jsonValue(s).rootValue != new jsonValue(sa).rootValue);
    };
}

public class greaterThanSymbol : symbol
{
    public override string symbolName => ">";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 5;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class lowerThanSymbol : symbol
{
    public override string symbolName => "<";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 5;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class greaterEqualSymbol : symbol
{
    public override string symbolName => ">=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 5;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class lowerEqualSymbol : symbol
{
    public override string symbolName => "<=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class isSymbol : symbol
{
    public override string symbolName => "is";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 5;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class asSymbol : symbol
{
    public override string symbolName => "as";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 5;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class typeofSymbol : symbol
{
    public override string symbolName => "typeof";
    public override int symbolArgCount => 1;
    public override int symbolPrior => 0;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class trieAddOneSymbol : symbol
{
    public override string symbolName => "++";
    public override int symbolArgCount => 1;
    public override int symbolPrior => 1;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class suffixAddOneSymbol : symbol
{
    public override string symbolName => ".*++";
    public override int symbolArgCount => -1;
    public override int symbolPrior => 0;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class trieSubOneSymbol : symbol
{
    public override string symbolName => "--";
    public override int symbolArgCount => 1;
    public override int symbolPrior => 1;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class suffixSubOneSymbol : symbol
{
    public override string symbolName => ".*--";
    public override int symbolArgCount => -1;
    public override int symbolPrior => 0;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class newSymbol : symbol
{
    public override string symbolName => "new";
    public override int symbolArgCount => 1;
    public override int symbolPrior => 0;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class bracketSymbol : symbol
{
    public override string symbolName => "()";
    public override int symbolArgCount => 0;
    public override int symbolPrior => 0;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class SquareBracketSymbol : symbol
{
    public override string symbolName => "[]";
    public override int symbolArgCount => 0;
    public override int symbolPrior => 0;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class dotSymbol : symbol
{
    public override string symbolName => ".";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 0;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class digitRerveseSymbol : symbol
{
    public override string symbolName => "~";
    public override int symbolArgCount => 1;
    public override int symbolPrior => 1;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class digitAndSymbol : symbol
{
    public override string symbolName => "&";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 7;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class digitOrSymbol : symbol
{
    public override string symbolName => "|";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 9;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class digitAndsetSymbol : symbol
{
    public override string symbolName => "&=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class digitOrsetSymbol : symbol
{
    public override string symbolName => "|=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}
public class digitXORSymbol : symbol
{
    public override string symbolName => "^";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 8;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class digitXORsetSymbol : symbol
{
    public override string symbolName => "^=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}
public class digitLeftMoveSymbol : symbol
{
    public override string symbolName => "<<";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 4;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class digitRightMoveSymbol : symbol
{
    public override string symbolName => ">>";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 4;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class digitLeftMovesetSymbol : symbol
{
    public override string symbolName => "<<=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class digitRightMovesetSymbol : symbol
{
    public override string symbolName => ">>=";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 14;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class nullMergeSymbol : symbol
{
    public override string symbolName => "??";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 13;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class nullCheckSymbol : symbol
{
    public override string symbolName => "?.";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 0;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class nullIndexCheckSymbol : symbol
{
    public override string symbolName => "?[]";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 0;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class toClassSymbol : symbol
{
    public override string symbolName => "(class)";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 0;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue();
    };
}

public class CommaSymbol : symbol
{
    public override string symbolName => ",";
    public override int symbolArgCount => 2;
    public override int symbolPrior => 16;
    public override Func<string, string, _runCommands, jsonValue> symbolFunc => (string s, string sa, _runCommands commandValues) => {
        return new jsonValue(sa);
    };
}
