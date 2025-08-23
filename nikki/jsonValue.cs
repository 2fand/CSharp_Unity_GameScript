using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

//class {a,b,cd,e} (特点：似数组，有别名，子数据变量形式，转换为对象过程为
//  , -> " = null, "
//  { -> "
//  } -> " = null}
//)
//enum {a,b,cd,e} (特点：似类，可以设值(int)，转换为对象过程为
//  转为值为空对象
//  根据成员及设定值创建对象
//)
//func a &b &c &d{ (特点：字符串数组为命令，是变量)
//        b=2
//        c=3
//     }

public class hashType
{
    public Hashtable value = new Hashtable();
    public jsonValue.valueClass valueClass;
    public hashType(jsonValue.valueClass valueClass)
    {
        this.valueClass = valueClass;
    }
}

public class jsonValue
{
    public enum valueClass
    {
        value,
        array,
        @object
    }
    public valueClass ThisClass => rootValue.valueClass;
    public hashType rootValue = new hashType(valueClass.value);
    Stack<hashType> stack = new Stack<hashType>();
    Stack<valueClass> classStack = new Stack<valueClass>();
    List<string> indexStack = new List<string>();
    List<int> hasValue = new List<int>();
    public static string stringRegex => "\"(\\\\\"|[^\\\\\"])*?\"";
    public static string trueRegex => "true";
    public static string falseRegex => "false";
    public static string nullRegex => "null";
    public static string integerRegex => "[+-]?[0-9]+";
    public static string floatRegex => "[+-]?([0-9]+(\\.([0-9]+)?)?|\\.[0-9]+)([eE][+-]?[0-9]+)?";
    public static string varRegex => "[a-zA-Z_]+";
    public static string arrayRegex => "\\[((\\s*[^,]+\\s*,)*\\s*[^,]+\\s*)?\\]";
    public static string classRegex => "{((\\s*[a-zA-Z_]+\\s*,)*\\s*[a-zA-Z_]+\\s*)?}";
    public static string enumRegex => "{((\\s*[a-zA-Z_]+(\\s*=\\s*" + integerRegex + ")?\\s*,)*\\s*[a-zA-Z_]+(\\s*=\\s*" + integerRegex + ")?\\s*)?}";
    public static string objectRegex => "{((\\s*" + stringRegex + "\\s*:\\s*[^,:]+\\s*,)*\\s*" + stringRegex + "\\s*:\\s*[^,:]+\\s*)?}";
    private static string _typeRegex = "";
    private static List<string> types = new List<string>();
    public static string typeRegex 
    {
        get
        {
            if (types.ToArray() != commandClasses.classes)
            {
                types = new List<string>(commandClasses.classes);
                _typeRegex = "(";
                for (int i = 0; i < types.Count; i++) { 
                    _typeRegex += types[i];
                    if (types.Count - 1 != i)
                    {
                        _typeRegex += "|";
                    }
                }
                _typeRegex += ")";
            }
            return _typeRegex;
        }
    }
    public static implicit operator jsonValue(string str) => new jsonValue(str);
    public static implicit operator jsonValue(bool b) => new jsonValue(b);
    public static implicit operator jsonValue(int i) => new jsonValue(i);
    public static implicit operator jsonValue(float f) => new jsonValue(f);
    public static implicit operator string(jsonValue jsonValue) => jsonValue.jsonValueTojsonString();
    public jsonValue()
    {
        setValue();
    }
    public jsonValue(object o)
    {
        if (typeof(jsonValue) == o.GetType())
        {
            rootValue.value = new Hashtable(o.ConvertTo<jsonValue>().rootValue.value);
        }
        else if(typeof(hashType) == o.GetType())
        {
            rootValue.value = new Hashtable(o.ConvertTo<hashType>().value);
            rootValue.valueClass = o.ConvertTo<hashType>().valueClass;
        }
        else
        {
            setValue(o);
        }
    }
    public jsonValue(string json)
    {
        if (!setValue(json))
        {
            throw new Exception("“" + json + "”值不合法");
        }
    }
    public jsonValue(string json, _runCommands commandValues)
    {
        bool isInStr = false;
        for (int i = 0; i < json.Length; i++)
        {
            if ('\"' == json[i])
            {
                isInStr = !isInStr;
            }
            if (!isInStr && Regex.Match(json.Substring(i), "^\\s*" + varRegex).Success)
            {
                if (commandValues.vars.ContainsKey(Regex.Match(json.Substring(i), "^\\s*" + varRegex).Value))
                {
                    json = json.Substring(0, i) + Regex.Replace(json.Substring(i), "^\\s*" + varRegex, commandValues.vars[Regex.Match(json.Substring(i), "^\\s*" + varRegex).Value].ConvertTo<jsonValue>().jsonValueTojsonString());
                }
                i += Regex.Match(json.Substring(i), "^\\s*" + varRegex).Length - 1;
            }
        }
        if (!setValue(json))
        {
            throw new Exception("json数据“" + json + "”不合法");
        }
    }
    public static bool isCorrectJsonValue(string json)
    {
        return new jsonValue().setValue(json);
    }
    public static bool isCorrectJsonValue(string json, _runCommands commandValues)
    {
        jsonValue jsonValue = new jsonValue();
        bool isInStr = false;
        for (int i = 0; i < json.Length; i++)
        {
            if ('\"' == json[i])
            {
                isInStr = !isInStr;
            }
            if (!isInStr && Regex.Match(json.Substring(i), "^\\s*" + varRegex).Success)
            {
                if (commandValues.vars.ContainsKey(Regex.Match(json.Substring(i), "^\\s*" + varRegex).Value))
                {
                    json = json.Substring(0, i) + Regex.Replace(json.Substring(i), "^\\s*" + varRegex, commandValues.vars[Regex.Match(json.Substring(i), "^\\s*" + varRegex).Value].ConvertTo<jsonValue>().jsonValueTojsonString());
                }
                i += Regex.Match(json.Substring(i), "^\\s*" + varRegex).Length - 1;
            }
        }
        return jsonValue.setValue(json);
    }
    public bool isNull()
    {
        return "null" == getRealType();
    }
    public bool isInt()
    {
        return "int" == getRealType();
    }
    public bool isBool()
    {
        return "bool" == getRealType();
    }
    public bool isString()
    {
        return "string" == getRealType();
    }
    public bool isFloat()
    {
        return "float" == getRealType();
    }
    public bool isArray()
    {
        return "array" == getRealType();
    }
    public bool isObject()
    {
        return "object" == getRealType();
    }
    public bool isType()
    {
        return getRealType() == "string" && commandClasses.classIsHas.ContainsKey(getValue().ToString());
    }
    public static bool isNull(hashType hashType)
    {
        return "null" == getRealType(hashType);
    }
    public static bool isInt(hashType hashType)
    {
        return "int" == getRealType(hashType);
    }
    public static bool isBool(hashType hashType)
    {
        return "bool" == getRealType(hashType);
    }
    public static bool isString(hashType hashType)
    {
        return "string" == getRealType(hashType);
    }
    public static bool isFloat(hashType hashType)
    {
        return "float" == getRealType(hashType);
    }
    public static bool isArray(hashType hashType)
    {
        return "array" == getRealType(hashType);
    }
    public static bool isObject(hashType hashType)
    {
        return "object" == getRealType(hashType);
    }
    public static bool isType(hashType hashType)
    {
        return getRealType(hashType) == "string" && commandClasses.classIsHas.ContainsKey(getValue(hashType).ToString());
    }
    public jsonValue(uint size, object defaultObject)
    {
        setArray(size, defaultObject);
    }
    public jsonValue(uint size, string json)
    {
        setArray(size, json);
    }
    public jsonValue(string[] attributeNames, object[] objects)
    {
        setObject(attributeNames, objects);
    }
    public object setValue(object v)
    {
        rootValue.valueClass = valueClass.value;
        if (!rootValue.value.ContainsKey(0))
        {
            rootValue.value.Add(0, null);
        }
        rootValue.value[0] = v;
        return v;
    }
    public object getValue()
    {
        return rootValue.value[0];
    }
    public string getString()
    {
        if ("string" == getRealType())
        {
            return rootValue.value[0].ToString();
        }
        throw new Exception("类型错误：当前的值类型为“" + getRealType() + "”");
    }
    public int getInt()
    {
        if ("int" == getRealType())
        {
            return rootValue.value[0].ConvertTo<int>();
        }
        throw new Exception("类型错误：当前的值类型为“" + getRealType() + "”");
    }
    public float getFloat()
    {
        if ("string" == getRealType())
        {
            return rootValue.value[0].ConvertTo<float>();
        }
        throw new Exception("类型错误：当前的值类型为“" + getRealType() + "”");
    }
    public bool getBool()
    {
        if ("bool" == getRealType())
        {
            return rootValue.value[0].ConvertTo<bool>();
        }
        throw new Exception("类型错误：当前的值类型为“" + getRealType() + "”");
    }
    public jsonValue[] getArray()
    {
        if ("array" == getRealType())
        {
            jsonValue[] array = new jsonValue[getChildValueCount()];
            for (int i = 0; i < getChildValueCount(); i++)
            {
                array[i] = new jsonValue(getIndexValue(i));
            }
            return array;
        }
        throw new Exception("类型错误：当前的值类型为“" + getRealType() + "”");
    }
    public static jsonValue[] getArray(hashType hashType)
    {
        if ("array" == getRealType(hashType))
        {
            jsonValue[] array = new jsonValue[getChildValueCount(hashType)];
            for (int i = 0; i < getChildValueCount(hashType); i++)
            {
                array[i] = new jsonValue(getIndexValue(hashType, i));
            }
            return array;
        }
        throw new Exception("类型错误：当前的值类型为“" + getRealType(hashType) + "”");
    }
    public Hashtable getObject()
    {
        if ("object" == getRealType())
        {
            return rootValue.value;
        }
        throw new Exception("类型错误：当前的值类型为“" + getRealType() + "”");
    }
    public object setValue()
    {
        return setValue(null as object);
    }
    public static object setValue(hashType hashType)
    {
        return setValue(hashType, null as object);
    }
    public static object setValue(hashType hashType, object v)
    {
        hashType.valueClass = valueClass.value;
        if (!hashType.value.ContainsKey(0))
        {
            hashType.value.Add(0, null);
        }
        hashType.value[0] = v;
        return v;
    }
    public static object getValue(hashType hashType)
    {
        return hashType.value[0];
    }

    public object getIndexValue(int index)
    {
        return rootValue.value[index.ToString()];
    }

    public static object getIndexValue(hashType hashType, int index)
    {
        return hashType.value[index.ToString()];
    }

    public bool tryGetIndexValue(int index)
    {
        return rootValue.value.ContainsKey(index.ToString());
    }

    public static bool tryGetIndexValue(hashType hashType, int index)
    {
        return hashType.value.ContainsKey(index.ToString());
    }

    public void setArray(uint size, object defaultObject = null)
    {
        rootValue.valueClass = valueClass.array;
        rootValue.value.Clear();
        for (int i = 0; i < size; i++)
        {
            rootValue.value.Add(i.ToString(), defaultObject);
        }
    }

    public static void setArray(hashType hashType, uint size, object defaultObject = null)
    {
        hashType.valueClass = valueClass.array;
        hashType.value.Clear();
        for (int i = 0; i < size; i++)
        {
            hashType.value.Add(i.ToString(), defaultObject);
        }
    }

    public void arrayAndValue(string json)
    {
        rootValue.value.Add(rootValue.value.Count.ToString(), new hashType(valueClass.value));
        setValue(rootValue.value[rootValue.value.Count - 1].ConvertTo<hashType>(), json);
    }

    public void arrayAndValue(jsonValue jsonValue)
    {
        rootValue.value.Add(rootValue.value.Count.ToString(), jsonValue.rootValue);
    }

    public static void arrayAndValue(hashType hashType, string json)
    {
        hashType.value.Add(hashType.value.Count.ToString(), new hashType(valueClass.value));
        setValue(hashType.value[hashType.value.Count - 1].ConvertTo<hashType>(), json);
    }

    public static void arrayAndValue(hashType hashType, jsonValue jsonValue)
    {
        hashType.value.Add(hashType.value.Count.ToString(), jsonValue.rootValue);
    }

    public void catArray(uint size, object Object = null)
    {
        for (int i = 0; i < size; i++)
        {
            rootValue.value.Add(rootValue.value.Count.ToString(), Object);
        }
    }
    public void catArray(jsonValue jsonValue)
    {
        for (int i = 0; valueClass.array == jsonValue.ThisClass && i < jsonValue.rootValue.value.Count; i++)
        {
            rootValue.value.Add(rootValue.value.Count.ToString(), jsonValue.rootValue.value[i]);
        }
    }
    public void catArray(string json)
    {
        jsonValue jsonValue = new jsonValue(json);
        for (int i = 0; valueClass.array == jsonValue.ThisClass && i < jsonValue.rootValue.value.Count; i++)
        {
            rootValue.value.Add(rootValue.value.Count.ToString(), jsonValue.rootValue.value[i]);
        }
    }
    public static void catArray(hashType hashType, uint size, object Object = null)
    {
        for (int i = 0; i < size; i++)
        {
            hashType.value.Add(hashType.value.Count.ToString(), Object);
        }
    }
    public static void catArray(hashType hashType, jsonValue jsonValue)
    {
        for (int i = 0; valueClass.array == jsonValue.ThisClass && i < jsonValue.rootValue.value.Count; i++)
        {
            hashType.value.Add(hashType.value.Count.ToString(), jsonValue.rootValue.value[i]);
        }
    }
    public static void catArray(hashType hashType, string json)
    {
        jsonValue jsonValue = new jsonValue(json);
        for (int i = 0; valueClass.array == jsonValue.ThisClass && i < jsonValue.rootValue.value.Count; i++)
        {
            hashType.value.Add(hashType.value.Count.ToString(), jsonValue.rootValue.value[i]);
        }
    }

    public void arrayInsert(int index, object o)
    {
        if ("array" == getRealType())
        {
            Hashtable hashtable = rootValue.value;
            index = Mathf.Min(index, hashtable.Count);
            rootValue.value.Clear();
            for (int i = 0, ia = 0; i < hashtable.Count + 1; i++)
            {
                if (index == i)
                {
                    rootValue.value.Add(ia++.ToString(), o);
                    if (i == hashtable.Count)
                    {
                        break;
                    }
                }
                rootValue.value.Add(ia++.ToString(), hashtable[i]);
            }
        }
    }

    public static void arrayInsert(hashType hashType, int index, object o)
    {
        if ("array" == getRealType(hashType))
        {
            Hashtable hashtable = hashType.value;
            index = Mathf.Min(index, hashtable.Count);
            hashType.value.Clear();
            for (int i = 0, ia = 0; i < hashtable.Count + 1; i++)
            {
                if (index == i)
                {
                    hashType.value.Add((ia++).ToString(), o);
                    if (i == hashtable.Count)
                    {
                        break;
                    }
                }
                hashType.value.Add(ia++.ToString(), hashtable[i]);
            }
        }
    }

    public void arraySwap(int index, int indexa)
    {
        if ("array" == getRealType())
        {
            object o = rootValue.value[index.ToString()];
            rootValue.value[index.ToString()] = rootValue.value[indexa.ToString()];
            rootValue.value[indexa.ToString()] = o;
        }
    }

    public static void arraySwap(hashType hashType, int index, int indexa)
    {
        if ("array" == getRealType(hashType))
        {
            object o = hashType.value[index.ToString()];
            hashType.value[index.ToString()] = hashType.value[indexa.ToString()];
            hashType.value[indexa.ToString()] = o;
        }
    }

    public void arrayErase(int index, object o)
    {
        if ("array" == getRealType() && index > 0 && index < rootValue.value.Count)
        {
            rootValue.value.Remove(index.ToString());
        }
        Hashtable hashtable = rootValue.value;
        rootValue.value.Clear();
        foreach (DictionaryEntry pair in hashtable)
        {
            rootValue.value.Add(pair.Key, pair.Value);
        }
    }

    public static void arrayErase(hashType hashType, int index, object o)
    {
        if ("array" == getRealType(hashType) && index > 0 && index < hashType.value.Count)
        {
            hashType.value.Remove(index.ToString());
        }
        Hashtable hashtable = hashType.value;
        hashType.value.Clear();
        foreach (DictionaryEntry pair in hashtable)
        {
            hashType.value.Add(pair.Key, pair.Value);
        }
    }

    public void arrayRemove()
    {
        if ("array" == getRealType() && 0 != rootValue.value.Count)
        {
            rootValue.value.Remove((rootValue.value.Count - 1).ToString());
        }
    }

    public static void arrayRemove(hashType hashType)
    {
        if ("array" == getRealType(hashType) && 0 != hashType.value.Count)
        {
            hashType.value.Remove((hashType.value.Count - 1).ToString());
        }
    }

    public void setObject(string[] attributeNames, object[] objects)
    {
        rootValue.valueClass = valueClass.@object;
        rootValue.value.Clear();
        for (int i = 0; i < Mathf.Min(attributeNames.Length, objects.Length); i++) 
        {
            rootValue.value.Add(attributeNames[i], objects[i]);
        }
    }

    public static void setObject(hashType hashType, string[] attributeNames, object[] objects)
    {
        hashType.valueClass = valueClass.@object;
        hashType.value.Clear();
        for (int i = 0; i < Mathf.Min(attributeNames.Length, objects.Length); i++)
        {
            hashType.value.Add(attributeNames[i], objects[i]);
        }
    }

    public void setObjectAttribute(string attributeName, object @object)
    {
        rootValue.valueClass = valueClass.@object;
        rootValue.value.Clear();
        rootValue.value.Add(attributeName, @object);
    }

    public static void setObjectAttribute(hashType hashType, string attributeName, object @object)
    {
        hashType.valueClass = valueClass.@object;
        hashType.value.Clear();
        hashType.value.Add(attributeName, @object);
    }

    public string getRealType()
    {
        if (valueClass.array == rootValue.valueClass)
        {
            return "array";
        }
        else if (valueClass.@object == rootValue.valueClass)
        {
            return "object";
        }
        else if (null == getValue())
        {
            return "null";
        }
        else if (typeof(int) == getValue().GetType() || typeof(long) == getValue().GetType() || typeof(short) == getValue().GetType() || typeof(byte) == getValue().GetType() || typeof(sbyte) == getValue().GetType() || typeof(uint) == getValue().GetType() || typeof(ulong) == getValue().GetType() || typeof(ushort) == getValue().GetType() || typeof(nint) == getValue().GetType() || typeof(nuint) == getValue().GetType())
        {
            return "int";
        }
        else if (typeof(float) == getValue().GetType() || typeof(double) == getValue().GetType() || typeof(decimal) == getValue().GetType())
        {
            return "float";
        }
        else if (typeof(string) == getValue().GetType())
        {
            return "string";
        }
        else if (typeof(bool) == getValue().GetType())
        {
            return "bool";
        }
        return getValue().GetType().ToString();
    }

    public static string getRealType(hashType hashType)
    {
        if (valueClass.array == hashType.valueClass)
        {
            return "array";
        }
        else if (valueClass.@object == hashType.valueClass)
        {
            return "object";
        }
        else if (null == hashType.value)
        {
            return "null";
        }
        else if (typeof(int) == hashType.value[0].GetType() || typeof(long) == hashType.value[0].GetType() || typeof(short) == hashType.value[0].GetType() || typeof(byte) == hashType.value[0].GetType() || typeof(sbyte) == hashType.value[0].GetType() || typeof(uint) == hashType.value[0].GetType() || typeof(ulong) == hashType.value[0].GetType() || typeof(ushort) == hashType.value[0].GetType() || typeof(nint) == hashType.value[0].GetType() || typeof(nuint) == hashType.value[0].GetType())
        {
            return "int";
        }
        else if (typeof(float) == hashType.value[0].GetType() || typeof(double) == hashType.value[0].GetType() || typeof(decimal) == hashType.value[0].GetType())
        {
            return "float";
        }
        else if (typeof(string) == hashType.value[0].GetType())
        {
            return "string";
        }
        else if (typeof(bool) == hashType.value[0].GetType())
        {
            return "bool";
        }
        return hashType.value[0].GetType().ToString();
    }
    public int getChildValueCount()
    {
        return rootValue.value.Count;
    }
    public static int getChildValueCount(hashType hashType)
    {
        return hashType.value.Count;
    }
    public ICollection getRootValueKeys()
    {
        return rootValue.value.Keys;
    }
    public ICollection getRootValueValues()
    {
        return rootValue.value.Values;
    }
    public static ICollection getValueKeys(hashType hashType)
    {
        return hashType.value.Keys;
    }
    public static ICollection getValueValues(hashType hashType)
    {
        return hashType.value.Values;
    }
    public object getAttribute(string attributeName)
    {
        return rootValue.value[attributeName];
    }
    public static object getAttribute(hashType hashType, string attributeName)
    {
        return hashType.value[attributeName];
    }
    public bool tryGetAttribute(string attributeName)
    {
        return rootValue.value.ContainsKey(attributeName);
    }
    public static bool tryGetAttribute(hashType hashType, string attributeName)
    {
        return hashType.value.ContainsKey(attributeName);
    }
    public static float ToScienceFloat(string floatStr)
    {
        return float.Parse(Regex.Match(floatStr, "^\\s*" + floatRegex).Value) * (Regex.Match(floatStr, "[eE][+-]?[0-9]+").Success ? Mathf.Pow(10, int.Parse(Regex.Match(floatStr, "[eE][+-]?[0-9]+").Value.Substring(1))) : 1);
    }
    public bool setValue(string json, bool applyEspase = true)
    {
        stack = new Stack<hashType>();
        classStack = new Stack<valueClass>();
        indexStack = new List<string>();
        hasValue = new List<int>();
        Action<object> addValue = (object o) =>
        {
            if (0 == stack.Count)
            {
                setValue(o);
            }
            else
            {
                if (valueClass.array == classStack.Peek())
                {
                    indexStack[indexStack.Count - 1] = "" == indexStack[indexStack.Count - 1] ? "0" : (int.Parse(indexStack[indexStack.Count - 1]) + 1).ToString();
                }
                stack.Peek().value.Add(indexStack[indexStack.Count - 1], o);
                hasValue[hasValue.Count - 1] = 1;
            }
        };
        Action EndOther = () =>
        {
            hashType tempTable = stack.Pop();
            hasValue.RemoveAt(hasValue.Count - 1);
            indexStack.RemoveAt(indexStack.Count - 1);
            if (0 == indexStack.Count)
            {
                rootValue.value = tempTable.value;
                rootValue.valueClass = classStack.Pop();
            }
            else
            {
                classStack.Pop();
                if (valueClass.array == classStack.Peek())
                {
                    indexStack[indexStack.Count - 1] = "" == indexStack[indexStack.Count - 1] ? "0" : (int.Parse(indexStack[indexStack.Count - 1]) + 1).ToString();
                }
                stack.Peek().value.Add(indexStack[indexStack.Count - 1], tempTable);
            }
        };
        Action<valueClass> addOther = (valueClass valueClass) =>
        {
            if (0 != stack.Count) {
                hasValue[hasValue.Count - 1] = 1;
            }
            stack.Push(new hashType(valueClass));
            hasValue.Add(-1);
            indexStack.Add("");
            classStack.Push(valueClass);
        };
        Func<string, string> noTwoSides = (string str) =>
        {
            return str.Substring(1, str.Length - 2);
        };
        for (int i = 0; i < json.Length; )
        {
            while (i < json.Length && (Regex.Match(json.Substring(i), "^\\s*\\[\\s*").Success || Regex.Match(json.Substring(i), "^\\s*{\\s*").Success))
            {
                while (i < json.Length && Regex.Match(json.Substring(i), "^\\s*\\[\\s*").Success)
                {
                    addOther(valueClass.array);
                    i += Regex.Match(json.Substring(i), "^\\s*\\[\\s*").Length;
                }
                while (i < json.Length && Regex.Match(json.Substring(i), "^\\s*{\\s*").Success)
                {
                    addOther(valueClass.@object);
                    i += Regex.Match(json.Substring(i), "^\\s*{\\s*").Length;
                }
            }
            while (i < json.Length && Regex.Match(json.Substring(i), "^\\s*\\]\\s*").Success || Regex.Match(json.Substring(i), "^\\s*}\\s*").Success)
            {
                while (i < json.Length && Regex.Match(json.Substring(i), "^\\s*\\]\\s*").Success)
                {
                    if (0 == stack.Count || 0 == hasValue[hasValue.Count - 1] || valueClass.@object == classStack.Peek())
                    {
                        return false;
                    }
                    EndOther();
                    i += Regex.Match(json.Substring(i), "^\\s*\\]\\s*").Length;
                }
                while (i < json.Length && Regex.Match(json.Substring(i), "^\\s*}\\s*").Success)
                {
                    if (0 == stack.Count || 0 == hasValue[hasValue.Count - 1] || valueClass.array == classStack.Peek())
                    {
                        return false;
                    }
                    EndOther();
                    i += Regex.Match(json.Substring(i), "^\\s*}\\s*").Length;
                }
            }
            if (stack.Count != 0 && -1 != hasValue[hasValue.Count - 1])
            {
                if (Regex.Match(json.Substring(i), "^\\s*,\\s*").Success && 1 == hasValue[hasValue.Count - 1])
                {
                    i += Regex.Match(json.Substring(i), "^\\s*,\\s*").Length;
                    hasValue[hasValue.Count - 1] = 0;
                }
                else
                {
                    return false;
                }
            }
            if (0 != stack.Count && valueClass.@object == classStack.Peek())
            {
                Match match = Regex.Match(json.Substring(i), "^\\s*"+stringRegex+"\\s*:\\s*");
                if (!match.Success)
                {
                    return false;
                }
                i += match.Length;
                indexStack[indexStack.Count - 1] = Regex.Match(match.Value, stringRegex).Value.Substring(1, Regex.Match(match.Value, stringRegex).Length - 2);
            }
            if (Regex.Match(json.Substring(i), "^\\s*[+-]?[0-9]+\\s*").Success)
            {
                addValue(int.Parse(Regex.Match(json.Substring(i), "^\\s*[+-]?[0-9]+\\s*").Value));
                i += Regex.Match(json.Substring(i), "^\\s*[+-]?[0-9]+\\s*").Length;
                if (0 == stack.Count && i < json.Length)
                {
                    return false;
                }
            }
            else if (Regex.Match(json.Substring(i), "^\\s*[+-]?([0-9]+(\\.([0-9]+)?)?|\\.[0-9]+)([eE][+-]?[0-9]+)?\\s*").Success)
            {
                string floatNum = Regex.Match(json.Substring(i), "^\\s*[+-]?([0-9]+(\\.([0-9]+)?)?|\\.[0-9]+)([eE][+-]?[0-9]+)?\\s*").Value;
                addValue(ToScienceFloat(json.Substring(i)));
                i += floatNum.Length;
                if (0 == stack.Count && i < json.Length)
                {
                    return false;
                }
            }
            else if(Regex.Match(json.Substring(i), "^\\s*false\\s*").Success){
                addValue(false);
                i += Regex.Match(json.Substring(i), "^\\s*false\\s*").Length;
                if (0 == stack.Count && i < json.Length)
                {
                    return false;
                }
            }
            else if (Regex.Match(json.Substring(i), "^\\s*true\\s*").Success)
            {
                addValue(true);
                i += Regex.Match(json.Substring(i), "^\\s*true\\s*").Length;
                if (0 == stack.Count && i < json.Length)
                {
                    return false;
                }
            }
            else if (Regex.Match(json.Substring(i), "^\\s*null\\s*").Success)
            {
                addValue(null);
                i += Regex.Match(json.Substring(i), "^\\s*null\\s*").Length;
                if (0 == stack.Count && i < json.Length)
                {
                    return false;
                }
            }
            else if (Regex.Match(json.Substring(i), "^\\s*"+stringRegex+"\\s*").Success)
            {
                addValue(Regex.Replace(noTwoSides(Regex.Match(Regex.Match(json.Substring(i), "^\\s*" + stringRegex + "\\s*").Value, stringRegex).Value), applyEspase ? "\\\\\"" : "", applyEspase ? "\"" : ""));
                i += Regex.Match(json.Substring(i), "^\\s*"+stringRegex+"\\s*").Length;
                if (0 == stack.Count && i < json.Length)
                {
                    return false;
                }
            }
            if (hasValue.Count != 0 && -1 == hasValue[hasValue.Count - 1])
            {
                hasValue[hasValue.Count - 1] = 0;
            }
            if (stack.Count == 0 && i < json.Length)
            {
                return false;
            }
        }
        return stack.Count == 0;
    }

    public static bool setValue(hashType hashType, string json, bool applyEspase = true)
    {
        Stack<hashType> stack = new Stack<hashType>();
        Stack<valueClass> classStack = new Stack<valueClass>();
        List<string> indexStack = new List<string>();
        List<int> hasValue = new List<int>();
        Action<object> addValue = (object o) =>
        {
            if (0 == stack.Count)
            {
                setValue(hashType, o);
            }
            else
            {
                if (valueClass.array == classStack.Peek())
                {
                    indexStack[indexStack.Count - 1] = "" == indexStack[indexStack.Count - 1] ? "0" : (int.Parse(indexStack[indexStack.Count - 1]) + 1).ToString();
                }
                stack.Peek().value.Add(indexStack[indexStack.Count - 1], o);
                hasValue[hasValue.Count - 1] = 1;
            }
        };
        Action EndOther = () =>
        {
            Hashtable tempTable = stack.Pop().value;
            hasValue.RemoveAt(hasValue.Count - 1);
            indexStack.RemoveAt(indexStack.Count - 1);
            if (0 == indexStack.Count)
            {
                hashType.value = tempTable;
                hashType.valueClass = classStack.Pop();
            }
            else
            {
                classStack.Pop();
                if (valueClass.array == classStack.Peek())
                {
                    indexStack[indexStack.Count - 1] = "" == indexStack[indexStack.Count - 1] ? "0" : (int.Parse(indexStack[indexStack.Count - 1]) + 1).ToString();
                }
                stack.Peek().value.Add(indexStack[indexStack.Count - 1], tempTable);
            }
        };
        Action<valueClass> addOther = (valueClass valueClass) =>
        {
            if (0 != stack.Count)
            {
                hasValue[hasValue.Count - 1] = 1;
            }
            stack.Push(new hashType(valueClass));
            hasValue.Add(-1);
            indexStack.Add("");
            classStack.Push(valueClass);
        };
        Func<string, string> noTwoSides = (string str) =>
        {
            return str.Substring(1, str.Length - 2);
        };
        for (int i = 0; i < json.Length;)
        {
            while (i < json.Length && Regex.Match(json.Substring(i), "^\\s*\\[\\s*").Success)
            {
                addOther(valueClass.array);
                i += Regex.Match(json.Substring(i), "^\\s*\\[\\s*").Length;
            }
            while (i < json.Length && Regex.Match(json.Substring(i), "^\\s*{\\s*").Success)
            {
                addOther(valueClass.@object);
                i += Regex.Match(json.Substring(i), "^\\s*{\\s*").Length;
            }
            while (i < json.Length && Regex.Match(json.Substring(i), "^\\s*\\]\\s*").Success)
            {
                if (0 == hasValue[hasValue.Count - 1] || valueClass.@object == classStack.Peek())
                {
                    return false;
                }
                EndOther();
                i += Regex.Match(json.Substring(i), "^\\s*\\]\\s*").Length;
            }
            while (i < json.Length && Regex.Match(json.Substring(i), "^\\s*}\\s*").Success)
            {
                if (0 == hasValue[hasValue.Count - 1] || valueClass.array == classStack.Peek())
                {
                    return false;
                }
                EndOther();
                i += Regex.Match(json.Substring(i), "^\\s*}\\s*").Length;
            }
            if (stack.Count != 0 && -1 != hasValue[hasValue.Count - 1])
            {
                if (Regex.Match(json.Substring(i), "^\\s*,\\s*").Success && 1 == hasValue[hasValue.Count - 1])
                {
                    i += Regex.Match(json.Substring(i), "^\\s*,\\s*").Length;
                    hasValue[hasValue.Count - 1] = 0;
                }
                else
                {
                    return false;
                }
            }
            if (0 != stack.Count && valueClass.@object == classStack.Peek())
            {
                Match match = Regex.Match(json.Substring(i), "^\\s*" + stringRegex + "\\s*:\\s*");
                if (!match.Success)
                {
                    return false;
                }
                i += match.Length;
                indexStack[indexStack.Count - 1] = Regex.Match(match.Value, stringRegex).Value.Substring(1, Regex.Match(match.Value, stringRegex).Length - 2);
            }
            if (Regex.Match(json.Substring(i), "^\\s*[+-]?[0-9]+\\s*").Success)
            {
                addValue(int.Parse(Regex.Match(json.Substring(i), "^\\s*[+-]?[0-9]+\\s*").Value));
                i += Regex.Match(json.Substring(i), "^\\s*[+-]?[0-9]+\\s*").Length;
                if (0 == stack.Count && i < json.Length)
                {
                    return false;
                }
            }
            else if (Regex.Match(json.Substring(i), "^\\s*[+-]?([0-9]+(\\.([0-9]+)?)?|\\.[0-9]+)([eE][+-]?[0-9]+)?\\s*").Success)
            {
                string floatNum = Regex.Match(json.Substring(i), "^\\s*[+-]?([0-9]+(\\.([0-9]+)?)?|\\.[0-9]+)([eE][+-]?[0-9]+)?\\s*").Value;
                addValue(ToScienceFloat(json.Substring(i)));
                i += floatNum.Length;
                if (0 == stack.Count && i < json.Length)
                {
                    return false;
                }
            }
            else if (Regex.Match(json.Substring(i), "^\\s*false\\s*").Success)
            {
                addValue(false);
                i += Regex.Match(json.Substring(i), "^\\s*false\\s*").Length;
                if (0 == stack.Count && i < json.Length)
                {
                    return false;
                }
            }
            else if (Regex.Match(json.Substring(i), "^\\s*true\\s*").Success)
            {
                addValue(true);
                i += Regex.Match(json.Substring(i), "^\\s*true\\s*").Length;
                if (0 == stack.Count && i < json.Length)
                {
                    return false;
                }
            }
            else if (Regex.Match(json.Substring(i), "^\\s*null\\s*").Success)
            {
                addValue(null);
                i += Regex.Match(json.Substring(i), "^\\s*null\\s*").Length;
                if (0 == stack.Count && i < json.Length)
                {
                    return false;
                }
            }
            else if (Regex.Match(json.Substring(i), "^\\s*" + stringRegex + "\\s*").Success)
            {
                addValue(Regex.Replace(noTwoSides(Regex.Match(Regex.Match(json.Substring(i), "^\\s*" + stringRegex + "\\s*").Value, stringRegex).Value), applyEspase ? "\\\\\"" : "", applyEspase ? "\"" : ""));
                i += Regex.Match(json.Substring(i), "^\\s*" + stringRegex + "\\s*").Length;
                if (0 == stack.Count && i < json.Length)
                {
                    return false;
                }
            }
            if (hasValue.Count != 0 && -1 == hasValue[hasValue.Count - 1])
            {
                hasValue[hasValue.Count - 1] = 0;
            }
            if (stack.Count == 0 && i < json.Length)
            {
                return false;
            }
        }
        return stack.Count == 0;
    }

    public string jsonValueTojsonString()
    {
        string json = "";
        if (valueClass.array == rootValue.valueClass)
        {
            json += "[";
            for (int i = 0; i < rootValue.value.Count; i++)
            {
                json += jsonValueTojsonString(getIndexValue(i));
                if (i != rootValue.value.Count - 1)
                {
                    json += ", ";
                }
            }
            json += "]";
            return json;
        }
        else if (valueClass.@object == rootValue.valueClass)
        {
            json += "{";
            int i = 0;
            foreach (string label in rootValue.value.Keys)
            {
                json += "\"" + label + "\" : " + jsonValueTojsonString(getAttribute(label));
                if (i++ != rootValue.value.Count - 1)
                {
                    json += ", ";
                }
            }
            json += "}";
            return json;
        }
        return getValue().ToString();
    }

    private string jsonValueTojsonString(object hashType)
    {
        string json = "";
        if (typeof(hashType) == hashType.GetType() && valueClass.array == hashType.ConvertTo<hashType>().valueClass)
        {
            json += "[";
            for (int i = 0; i < hashType.ConvertTo<hashType>().value.Count; i++)
            {
                json += jsonValueTojsonString(getIndexValue(hashType.ConvertTo<hashType>(), i));
                if (i != hashType.ConvertTo<hashType>().value.Count - 1)
                {
                    json += ", ";
                }
            }
            json += "]";
            return json;
        }
        else if (typeof(hashType) == hashType.GetType() && valueClass.@object == hashType.ConvertTo<hashType>().valueClass)
        {
            json += "{";
            int i = 0;
            foreach (string label in hashType.ConvertTo<hashType>().value.Keys)
            {
                json += "\"" + label + "\" : " + jsonValueTojsonString(getAttribute(hashType.ConvertTo<hashType>(), label));
                if (i++ != hashType.ConvertTo<hashType>().value.Count - 1)
                {
                    json += ", ";
                }
            }
            json += "}";
            return json;
        }
        return hashType.ToString();
    }

    public static bool isInt(string jsonvalue) {
        return Regex.Match(jsonvalue, "^\\s[+-]?[0-9]+\\s$").Success;
    }
    public static bool isBool(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*(true|false)\\s*$").Success;
    }
    public static bool isNull(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*" + nullRegex + "\\s$*").Success;
    }
    public static bool isString(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*" + stringRegex + "\\s*$").Success;
    }
    public static bool isFloat(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*[+-]?([0-9]+(\\.([0-9]+)?)?|\\.[0-9]+)([eE][+-]?[0-9]+)?\\s*$").Success;
    }
    public static Match getIntegerMatchInJsonString(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s[+-]?[0-9]+\\s*");
    }
    public static Match getTrueMatchInJsonString(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*true\\s*");
    }
    public static Match getFalseMatchInJsonString(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*false\\s*");
    }
    public static Match getNullMatchInJsonString(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*null\\s*");
    }
    public static Match getStringMatchInJsonString(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*" + stringRegex + "\\s*");
    }
    public static Match getFloatMatchInJsonString(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*[+-]?([0-9]+(\\.([0-9]+)?)?|\\.[0-9]+)([eE][+-]?[0-9]+)?\\s*");
    }
}
