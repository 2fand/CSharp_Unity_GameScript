using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;


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
    public static string classRegex => "int|float|string|bool|null|array|class";
    public static implicit operator jsonValue(string str) => new jsonValue(str);
    public static implicit operator string(jsonValue jsonValue) => jsonValue.jsonValueTojsonString();
    public jsonValue()
    {
        setValue(null);
    }
    public jsonValue(object v)
    {
        setValue(v);
    }
    public jsonValue(string json)
    {
        setValue(json);
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
        setValue(json);
    }
    public jsonValue(uint size, object defaultObject = null)
    {
        setArray(size, defaultObject);
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
    public object setValue(hashType hashType, object v)
    {
        hashType.valueClass = valueClass.value;
        if (!hashType.value.ContainsKey(0))
        {
            hashType.value.Add(0, null);
        }
        hashType.value[0] = v;
        return v;
    }
    public object getValue(hashType hashType)
    {
        return hashType.value[0];
    }

    public object indexToValue(int index)
    {
        return rootValue.value[index.ToString()];
    }

    public object indexToValue(hashType hashType, int index)
    {
        return hashType.value[index.ToString()];
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

    public void setArray(hashType hashType, uint size, object defaultObject = null)
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

    public void arrayAndValue(hashType hashType, string json)
    {
        hashType.value.Add(hashType.value.Count.ToString(), new hashType(valueClass.value));
        setValue(hashType.value[hashType.value.Count - 1].ConvertTo<hashType>(), json);
    }

    public void arrayAndValue(hashType hashType, jsonValue jsonValue)
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
    public void catArray(hashType hashType, uint size, object Object = null)
    {
        for (int i = 0; i < size; i++)
        {
            hashType.value.Add(hashType.value.Count.ToString(), Object);
        }
    }
    public void catArray(hashType hashType, jsonValue jsonValue)
    {
        for (int i = 0; valueClass.array == jsonValue.ThisClass && i < jsonValue.rootValue.value.Count; i++)
        {
            hashType.value.Add(hashType.value.Count.ToString(), jsonValue.rootValue.value[i]);
        }
    }
    public void catArray(hashType hashType, string json)
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

    public void arrayInsert(hashType hashType, int index, object o)
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

    public void arraySwap(hashType hashType, int index, int indexa)
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

    public void arrayErase(hashType hashType, int index, object o)
    {
        if ("array" == getRealType(hashType) && index > 0 && index < rootValue.value.Count)
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

    public void arrayRemove(hashType hashType)
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

    public void setObject(hashType hashType, string[] attributeNames, object[] objects)
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

    public void setObjectAttribute(hashType hashType, string attributeName, object @object)
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

    public object attributeNameToValue(string attributeName)
    {
        return rootValue.value[attributeName];
    }
    public object attributeNameToValue(hashType hashType, string attributeName)
    {
        return hashType.value[attributeName];
    }
    public object pathToValue(string path)
    {
        throw new NotImplementedException();
    }
    public object pathToValue(hashType hashType, string path)
    {
        throw new NotImplementedException();
    }
    public float ToScienceFloat(string floatStr)
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
            Hashtable tempTable = stack.Pop().value;
            hasValue.RemoveAt(hasValue.Count - 1);
            indexStack.RemoveAt(indexStack.Count - 1);
            if (0 == indexStack.Count)
            {
                rootValue.value = tempTable;
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

    public bool setValue(hashType hashType, string json, bool applyEspase = true)
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
                json += jsonValueTojsonString(indexToValue(i));
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
                json += label + " : " + jsonValueTojsonString(attributeNameToValue(label));
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
                json += jsonValueTojsonString(indexToValue(hashType.ConvertTo<hashType>(), i));
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
                json += label + " : " + jsonValueTojsonString(attributeNameToValue(hashType.ConvertTo<hashType>(), label));
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

    public static bool isInteger(string jsonvalue) {
        return Regex.Match(jsonvalue, "^\\s[+-]?[0-9]+\\s$").Success;
    }
    public static bool isTrue(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*true\\s*$").Success;
    }
    public static bool isFalse(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*false\\s*$").Success;
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
    public static string getInteger(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s[+-]?[0-9]+\\s*").Value;
    }
    public static string getTrue(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*true\\s*").Value;
    }
    public static string getFalse(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*false\\s*").Value;
    }
    public static string getNull(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*null\\s*").Value;
    }
    public static string getString(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*" + stringRegex + "\\s*").Value;
    }
    public static string getFloat(string jsonvalue)
    {
        return Regex.Match(jsonvalue, "^\\s*[+-]?([0-9]+(\\.([0-9]+)?)?|\\.[0-9]+)([eE][+-]?[0-9]+)?\\s*").Value;
    }
}
