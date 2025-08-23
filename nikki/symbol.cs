using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public abstract class symbol
{
    public readonly static Hashtable symbolPriors;
    public readonly static Hashtable symbolArgCounts;
    public readonly static Hashtable symbolFuncs;
    public abstract string symbolName { get; }
    public abstract float symbolPrior { get; }
    public abstract int symbolArgCount { get; }
    public abstract System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc { get; }
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
                _runCommands.keyWordHas.Add(Activator.CreateInstance(types[i]).ConvertTo<symbol>().symbolName, true);
            }
        }
    }
    public static float symbolToLevel(string str)
    {
        str = str.ToLower().Trim();
        return (float)symbolPriors[str];
    }
    public static int getSymbolArgCount(string str)
    {
        str = str.ToLower().Trim();
        return (int)symbolArgCounts[str];
    }
#nullable enable
    public static System.Func<string, string, int, int, int, _runCommands, jsonValue> getSymbolFunc(string str)
    {
        str = str.ToLower().Trim();
        return (System.Func<string, string, int, int, int, _runCommands, jsonValue>)symbolFuncs[str];
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
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        if (!Regex.Match(s, "[a-zA-Z_]+").Success)
        {
            throw new Exception("在第" + commandI + "行第" + valueColumn + "列的变量“" + s + "”不合法");
        }
        if (!commandValues.vars.ContainsKey(s))
        {
            commandValues.vars.Add(s, new jsonValue(sa, commandValues));
        }
        else
        {
            commandValues.vars[s] = new jsonValue(sa, commandValues);
        }
        return commandValues.getVar(s).ConvertTo<jsonValue>();
    };
}

public class notSymbol : symbol
{
    public override string symbolName => "!";
    public override int symbolArgCount => 1;
    public override float symbolPrior => 12;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = new jsonValue(sa, commandValues);
        if ("bool" == json.getRealType())
        {
            return new jsonValue(!json.getValue().ConvertTo<bool>());
        }
        Debug.LogError("运算错误：无法将第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”进行逻辑取反");
        throw new Exception();
    };
}

public class addSymbol : symbol
{
    public override string symbolName => ".*+";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 3;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
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
            return json.isFloat() || jsona.isFloat() ? new jsonValue(json.getValue().ConvertTo<float>() + jsona.getValue().ConvertTo<float>()) : new jsonValue(json.getValue().ConvertTo<int>() + jsona.getValue().ConvertTo<int>());
        }
        if ("string" == json.getRealType() && "string" == jsona.getRealType())
        {
            return new jsonValue(json.getValue().ConvertTo<string>() + jsona.getValue().ConvertTo<string>());
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ \" + s + \" ”与第" + commandI + "行第" + valueaColumn + "列的“ " + sa + " ”相加");
        throw new Exception();
    };
}

public class subSymbol : symbol
{
    public override string symbolName => ".*-";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 3;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
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
            return json.isFloat() || jsona.isFloat() ? new jsonValue(json.getValue().ConvertTo<float>() - jsona.getValue().ConvertTo<float>()) : new jsonValue(json.getValue().ConvertTo<int>() - jsona.getValue().ConvertTo<int>());
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“ " + sa + " ”相减");
        throw new Exception();
    };
}

public class timSymbol : symbol
{
    public override string symbolName => "*";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 2;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
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
            return json.isFloat() || jsona.isFloat() ? new jsonValue(json.getValue().ConvertTo<float>() * jsona.getValue().ConvertTo<float>()) : new jsonValue(json.getValue().ConvertTo<int>() * jsona.getValue().ConvertTo<int>());
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“ " + sa + " ”相乘");
        throw new Exception();
    };
}

public class divSymbol : symbol
{
    public override string symbolName => "/";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 2;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
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
            return json.isFloat() || jsona.isFloat() ? new jsonValue(json.getValue().ConvertTo<float>() / jsona.getValue().ConvertTo<float>()) : new jsonValue(json.getValue().ConvertTo<int>() / jsona.getValue().ConvertTo<int>());
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“ " + sa + " ”相除");
        throw new Exception();
    };
}

public class divToIntSymbol : symbol
{
    public override string symbolName => "//";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 2;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
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
            return new jsonValue(json.getValue().ConvertTo<int>() / jsona.getValue().ConvertTo<int>());
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“ " + sa + " ”相整除");
        throw new Exception();
    };
}

public class powSymbol : symbol
{
    public override string symbolName => "**";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 1.9f;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
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
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”相幂");
        throw new Exception();
    };
}

public class andSymbol : symbol
{
    public override string symbolName => "&&";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 10;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
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
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”进行逻辑与运算");
        throw new Exception();
    };
}

public class orSymbol : symbol
{
    public override string symbolName => "||";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 11;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
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
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”进行逻辑或运算");
        throw new Exception();
    };
}

public class addsetSymbol : symbol
{
    public override string symbolName => "+=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc(".*+")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}

public class subsetSymbol : symbol
{
    public override string symbolName => "-=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc(".*-")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}

public class timsetSymbol : symbol
{
    public override string symbolName => "*=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("*")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}

public class divsetSymbol : symbol
{
    public override string symbolName => "/=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("/")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}

public class divToIntsetSymbol : symbol
{
    public override string symbolName => "//=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("//")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}

public class powsetSymbol : symbol
{
    public override string symbolName => "**=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("**")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}

public class orsetSymbol : symbol
{
    public override string symbolName => "||=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("||")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}

public class andsetSymbol : symbol
{
    public override string symbolName => "&&=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("&&")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}

public class equalSymbol : symbol
{
    public override string symbolName => "==";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 6;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return new jsonValue(new jsonValue(s).rootValue == new jsonValue(sa).rootValue);
    };
}

public class notEqualSymbol : symbol
{
    public override string symbolName => "!=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 6;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return new jsonValue(new jsonValue(s).rootValue != new jsonValue(sa).rootValue);
    };
}

public class greaterThanSymbol : symbol
{
    public override string symbolName => ">";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 5;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = s;
        jsonValue jsona = sa;
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
            return new jsonValue(json.getValue().ConvertTo<float>() > jsona.getValue().ConvertTo<float>());
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”进行大于比较");
        throw new Exception();
    };
}

public class lowerThanSymbol : symbol
{
    public override string symbolName => "<";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 5;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = s;
        jsonValue jsona = sa;
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
            return new jsonValue(json.getValue().ConvertTo<float>() < jsona.getValue().ConvertTo<float>());
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”进行小于比较");
        throw new Exception();
    };
}

public class greaterEqualSymbol : symbol
{
    public override string symbolName => ">=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 5;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = s;
        jsonValue jsona = sa;
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
            return new jsonValue(json.getValue().ConvertTo<float>() >= jsona.getValue().ConvertTo<float>());
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”进行大于等于比较");
        throw new Exception();
    };
}

public class lowerEqualSymbol : symbol
{
    public override string symbolName => "<=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = s;
        jsonValue jsona = sa;
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
            return new jsonValue(json.getValue().ConvertTo<float>() <= jsona.getValue().ConvertTo<float>());
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”进行小于等于比较");
        throw new Exception();
    };
}

public class isSymbol : symbol
{
    public override string symbolName => "is";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 5;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return new jsonValue(new jsonValue(s, commandValues).getRealType() == new jsonValue(sa, commandValues));
    };
}

public class asSymbol : symbol
{
    public override string symbolName => "as";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 5;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        if ("bool" == new jsonValue(sa, commandValues).getValue().ToString())
        {
            if ("null" == new jsonValue(s, commandValues).getRealType() || ("string" == new jsonValue(s, commandValues).getRealType() && new jsonValue(s, commandValues).getValue().ToString() == ""))
            {
                return false;
            }
            return getSymbolFunc("!=")("0", s, commandI, -1, valueColumn, commandValues);
        }
        if ("string" == new jsonValue(sa, commandValues).getValue().ToString())
        {
            return new jsonValue(s, commandValues).getValue().ToString();
        }
        if ("int" == new jsonValue(sa, commandValues).getValue().ToString())
        {
            if ("string" == new jsonValue(s, commandValues).getRealType())
            {
                return int.Parse(new jsonValue(s, commandValues).getValue().ToString());
            }
            if ("null" == new jsonValue(s, commandValues).getRealType())
            {
                return 0;
            }
            if ("bool" == new jsonValue(s, commandValues).getRealType())
            {
                return new jsonValue(s, commandValues).getValue().ConvertTo<bool>() ? 1 : 0;
            }
            return new jsonValue(s, commandValues).getValue().ConvertTo<int>();
        }
        if ("float" == new jsonValue(s, commandValues).getValue().ToString())
        {
            if ("string" == new jsonValue(s, commandValues).getRealType())
            {
                return int.Parse(new jsonValue(s, commandValues).getValue().ToString());
            }
            if ("null" == new jsonValue(s, commandValues).getRealType())
            {
                return 0;
            }
            if ("bool" == new jsonValue(s, commandValues).getRealType())
            {
                return new jsonValue(s, commandValues).getValue().ConvertTo<bool>() ? 1 : 0;
            }
            return new jsonValue(s, commandValues).getValue().ConvertTo<float>();
        }
        Debug.LogError("(第" + commandI + "行第" + valueColumn +" 列，第" + commandI + "行第" + valueaColumn +" 列)类型不匹配错误：无法将" + new jsonValue(s, commandValues).getRealType() + "转换为" + new jsonValue(sa, commandValues).getValue());
        throw new Exception();
    };
}

public class typeofSymbol : symbol
{
    public override string symbolName => "typeof";
    public override int symbolArgCount => 1;
    public override float symbolPrior => 0;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return new jsonValue(s, commandValues).getRealType();
    };
}

public class trieAddOneSymbol : symbol
{
    public override string symbolName => "++";
    public override int symbolArgCount => 1;
    public override float symbolPrior => 1;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("+=")(s, "1", commandI, valueColumn, -1, commandValues);
    };
}

public class suffixAddOneSymbol : symbol
{
    public override string symbolName => ".*++";
    public override int symbolArgCount => -1;
    public override float symbolPrior => 0;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue before = s;
        getSymbolFunc("+=")(s, "1", commandI, valueColumn, -1, commandValues);
        return before;
    };
}

public class trieSubOneSymbol : symbol
{
    public override string symbolName => "--";
    public override int symbolArgCount => 1;
    public override float symbolPrior => 1;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("-=")(s, "1", commandI, valueColumn, -1, commandValues);
    };
}

public class suffixSubOneSymbol : symbol
{
    public override string symbolName => ".*--";
    public override int symbolArgCount => -1;
    public override float symbolPrior => 0;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue before = s;
        getSymbolFunc("-=")(s, "1", commandI, valueColumn, -1, commandValues);
        return before;
    };
}

public class newSymbol : symbol
{
    public override string symbolName => "new";
    public override int symbolArgCount => 1;
    public override float symbolPrior => 0;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        if (Regex.Match(s, "^\\s*" + jsonValue.typeRegex + "\\s*$").Success)
        {
            string strclass = Regex.Match(s, jsonValue.typeRegex).Value;
            if (strclass == "string") 
            {
                return getSymbolFunc("=")(s, "", commandI, valueColumn, -1, commandValues);
            }
            if (strclass == "bool")
            {
                return getSymbolFunc("=")(s, "false", commandI, valueColumn, -1, commandValues);
            }
            if (strclass == "int")
            {
                return getSymbolFunc("=")(s, "0", commandI, valueColumn, -1, commandValues);
            }
            if (strclass == "float")
            {
                return getSymbolFunc("=")(s, "0.0", commandI, valueColumn, -1, commandValues);
            }
            if (strclass == "null")
            {
                return getSymbolFunc("=")(s, "null", commandI, valueColumn, -1, commandValues);
            }
            if (strclass == "array")
            {
                return getSymbolFunc("=")(s, "[]", commandI, valueColumn, -1, commandValues);
            }
            if (strclass == "object")
            {
                return getSymbolFunc("=")(s, "{}", commandI, valueColumn, -1, commandValues);
            }
        }
        else if (Regex.Match(s, "^\\s*" + jsonValue.typeRegex + "\\[.+\\]\\s*$").Success)
        {
            string arrsize = Regex.Match(s, "\\[.+\\]").Value;
            arrsize = arrsize.Substring(1, arrsize.Length - 2);
            string strclass = Regex.Match(s, jsonValue.typeRegex).Value;
            if (strclass == "string")
            {
                return getSymbolFunc("=")(s, new jsonValue(new jsonValue(arrsize).getValue().ConvertTo<uint>(), ""), commandI, valueColumn, -1, commandValues);
            }
            if (strclass == "bool")
            {
                return getSymbolFunc("=")(s, new jsonValue(new jsonValue(arrsize).getValue().ConvertTo<uint>(), false), commandI, valueColumn, -1, commandValues);
            }
            if (strclass == "int")
            {
                return getSymbolFunc("=")(s, new jsonValue(new jsonValue(arrsize).getValue().ConvertTo<uint>(), 0), commandI, valueColumn, -1, commandValues);
            }
            if (strclass == "float")
            {
                return getSymbolFunc("=")(s, new jsonValue(new jsonValue(arrsize).getValue().ConvertTo<uint>(), 0.0), commandI, valueColumn, -1, commandValues);
            }
            if (strclass == "null")
            {
                return getSymbolFunc("=")(s, new jsonValue(new jsonValue(arrsize).getValue().ConvertTo<uint>(), null), commandI, valueColumn, -1, commandValues);
            }
            if (strclass == "array")
            {
                return getSymbolFunc("=")(s, new jsonValue(new jsonValue(arrsize).getValue().ConvertTo<uint>(), new jsonValue(0, null)), commandI, valueColumn, -1, commandValues);
            }
            if (strclass == "object")
            {
                return getSymbolFunc("=")(s, new jsonValue(new jsonValue(arrsize).getValue().ConvertTo<uint>(), new jsonValue("{}")), commandI, valueColumn, -1, commandValues);
            }
        }
        else if (Regex.Match(s, "^\\s*" + jsonValue.objectRegex + "\\s*$").Success)
        {
            return Regex.Match(s, "{.+}").Value;
        }
        else if (Regex.Match(s, "^\\s*" + jsonValue.arrayRegex + "\\s*$").Success)
        {
            return Regex.Match(s, "\\[.+\\]").Value;
        }
        Debug.LogError("格式错误：第" + commandI + "行第" + valueColumn + "列的“" + s + "”格式错误");
        throw new Exception();
    };
}

public class bracketSymbol : symbol
{
    public override string symbolName => "()";
    public override int symbolArgCount => 1;
    public override float symbolPrior => 0;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return new jsonValue(s);
    };
}

public class SquareBracketSymbol : symbol
{
    public override string symbolName => "[]";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 0;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        if (!json.isArray())
        {
            throw new Exception("(第" + commandI + "行第" + valueColumn + "列)类型错误：“" + s + "”类型不能为“" + json.getRealType() + "”");
        }
        if (json.tryGetIndexValue(new jsonValue(sa, commandValues).getInt()))
        {
            return new jsonValue(json.getIndexValue(new jsonValue(sa, commandValues).getInt()));
        }
        throw new ArgumentOutOfRangeException("(第" + commandI + "行第" + valueaColumn + "列)索引溢出错误：当前“s”数组大小为" + json.getChildValueCount() + "，索引“" + new jsonValue(sa, commandValues).jsonValueTojsonString() + "”无法访问");
    };
}

public class dotSymbol : symbol
{
    public override string symbolName => ".";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 0;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        if (!json.isObject())
        {
            throw new Exception("(第" + commandI + "行第" + valueaColumn + "列)类型错误：“" + json.jsonValueTojsonString() + "”不是对象");
        }
        if (json.tryGetAttribute(sa))
        {
            return new jsonValue(json.getAttribute(sa));
        }
        throw new Exception();
    };
}

public class digitRerveseSymbol : symbol
{
    public override string symbolName => "~";
    public override int symbolArgCount => 1;
    public override float symbolPrior => 1;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        if ("int" == json.getRealType() || "bool" == json.getRealType())
        {
            if (json.getValue() is bool)
            {
                json.setValue(json.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            return ~json.getValue().ConvertTo<int>();
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”进行位取反运算");
        throw new Exception();
    };
}

public class digitAndSymbol : symbol
{
    public override string symbolName => "&";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 7;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (("int" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "bool" == jsona.getRealType()))
        {
            if (json.getValue() is bool)
            {
                json.setValue(json.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            if (jsona.getValue() is bool)
            {
                jsona.setValue(jsona.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            return json.getValue().ConvertTo<int>() & jsona.getValue().ConvertTo<int>();
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”进行位与运算");
        throw new Exception();
    };
}

public class digitOrSymbol : symbol
{
    public override string symbolName => "|";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 9;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (("int" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "bool" == jsona.getRealType()))
        {
            if (json.getValue() is bool)
            {
                json.setValue(json.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            if (jsona.getValue() is bool)
            {
                jsona.setValue(jsona.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            return json.getValue().ConvertTo<int>() | jsona.getValue().ConvertTo<int>();
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”进行位或运算");
        throw new Exception();
    };
}

public class digitAndsetSymbol : symbol
{
    public override string symbolName => "&=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("&")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}

public class digitOrsetSymbol : symbol
{
    public override string symbolName => "|=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("|")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}
public class digitXORSymbol : symbol
{
    public override string symbolName => "^";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 8;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (("int" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "bool" == jsona.getRealType()))
        {
            if (json.getValue() is bool)
            {
                json.setValue(json.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            if (jsona.getValue() is bool)
            {
                jsona.setValue(jsona.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            return json.getValue().ConvertTo<int>() ^ jsona.getValue().ConvertTo<int>();
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”进行位异或运算");
        throw new Exception();
    };
}

public class digitXORsetSymbol : symbol
{
    public override string symbolName => "^=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("^")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}
public class digitLeftMoveSymbol : symbol
{
    public override string symbolName => "<<";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 4;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (("int" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "bool" == jsona.getRealType()))
        {
            if (json.getValue() is bool)
            {
                json.setValue(json.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            if (jsona.getValue() is bool)
            {
                jsona.setValue(jsona.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            return json.getValue().ConvertTo<int>() << jsona.getValue().ConvertTo<int>();
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”进行左移位运算");
        throw new Exception();
    };
}

public class digitRightMoveSymbol : symbol
{
    public override string symbolName => ">>";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 4;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        jsonValue json = new jsonValue(s, commandValues);
        jsonValue jsona = new jsonValue(sa, commandValues);
        if (("int" == json.getRealType() || "bool" == json.getRealType()) && ("int" == jsona.getRealType() || "bool" == jsona.getRealType()))
        {
            if (json.getValue() is bool)
            {
                json.setValue(json.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            if (jsona.getValue() is bool)
            {
                jsona.setValue(jsona.getValue().ConvertTo<bool>() ? 1 : 0);
            }
            return json.getValue().ConvertTo<int>() >> jsona.getValue().ConvertTo<int>();
        }
        Debug.LogError("运算错误：无法让第" + commandI + "行第" + valueColumn + "列的“ " + s + " ”与第" + commandI + "行第" + valueaColumn + "列的“" + sa + "”进行右移位运算");
        throw new Exception();
    };
}

public class digitLeftMovesetSymbol : symbol
{
    public override string symbolName => "<<=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc("<<")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}

public class digitRightMovesetSymbol : symbol
{
    public override string symbolName => ">>=";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("=")(s, getSymbolFunc(">>")(s, sa, commandI, valueColumn, valueaColumn, commandValues), commandI, valueColumn, valueaColumn, commandValues);
    };
}

public class nullMergeSymbol : symbol
{
    public override string symbolName => "??";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 13;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return new jsonValue(new jsonValue(s, commandValues).getValue() ?? new jsonValue(sa, commandValues).getValue());
    };
}

public class nullCheckSymbol : symbol
{
    public override string symbolName => "?.";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 0;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return new jsonValue(new jsonValue(s, commandValues).tryGetAttribute(sa) ? new jsonValue(s, commandValues).getAttribute(sa) : null);
    };
}

public class nullIndexCheckSymbol : symbol
{
    public override string symbolName => "?[]";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 0;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return new jsonValue(new jsonValue(s, commandValues).tryGetIndexValue(new jsonValue(sa, commandValues).getValue().ConvertTo<int>()) ? new jsonValue(s, commandValues).getIndexValue(new jsonValue(sa, commandValues).getValue().ConvertTo<int>()) : null);
    };
}

public class toClassSymbol : symbol
{
    public override string symbolName => "(class)";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 0;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("as")(sa, s, commandI, valueaColumn, valueColumn, commandValues);
    };
}

public class CommaSymbol : symbol
{
    public override string symbolName => ",";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 16;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return new jsonValue(sa);
    };
}
public class ifSymbol : symbol
{
    public override string symbolName => "?";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return new jsonValue(s, commandValues).getBool() ? sa : null;
    };
}
public class elseSymbol : symbol
{
    public override string symbolName => ":";
    public override int symbolArgCount => 2;
    public override float symbolPrior => 14;
    public override System.Func<string, string, int, int, int, _runCommands, jsonValue> symbolFunc => (string s, string sa, int commandI, int valueColumn, int valueaColumn, _runCommands commandValues) => {
        return getSymbolFunc("??")(s, sa, commandI, valueColumn, valueaColumn, commandValues);
    };
}
