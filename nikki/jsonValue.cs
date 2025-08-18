using System;
using System.Collections;
using System.Collections.Generic;
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
    private valueClass thisClass = valueClass.value;
    public valueClass ThisClass => thisClass;
    public Hashtable rootValue = new Hashtable();
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
        thisClass = valueClass.value;
        if (!rootValue.ContainsKey(0))
        {
            rootValue.Add(0, null);
        }
        rootValue[0] = v;
        return v;
    }
    public hashType getValue()
    {
        return rootValue[0].ConvertTo<hashType>();
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
    public hashType getValue(hashType hashType)
    {
        return hashType.value[0].ConvertTo<hashType>();
    }

    public hashType indexToValue(int index)
    {
        return rootValue[index.ToString()].ConvertTo<hashType>();
    }

    public hashType indexToValue(hashType hashType, int index)
    {
        return hashType.value[index.ToString()].ConvertTo<hashType>();
    }

    public void setArray(uint size, object defaultObject = null)
    {
        thisClass = valueClass.array;
        rootValue.Clear();
        for (int i = 0; i < size; i++)
        {
            rootValue.Add(i.ToString(), defaultObject);
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

    public void setObject(string[] attributeNames, object[] objects)
    {
        thisClass = valueClass.@object;
        rootValue.Clear();
        for (int i = 0; i < Mathf.Min(attributeNames.Length, objects.Length); i++) 
        {
            rootValue.Add(attributeNames[i], objects[i]);
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
        thisClass = valueClass.@object;
        rootValue.Clear();
        rootValue.Add(attributeName, @object);
    }

    public void setObjectAttribute(hashType hashType, string attributeName, object @object)
    {
        hashType.valueClass = valueClass.@object;
        hashType.value.Clear();
        hashType.value.Add(attributeName, @object);
    }

    public string getRealType()
    {
        if (valueClass.array == thisClass)
        {
            return "array";
        }
        else if (valueClass.@object == thisClass)
        {
            return "object";
        }
        else if (null == getValue().value)
        {
            return "null";
        }
        else if (typeof(int) == getValue().value.GetType() || typeof(long) == getValue().value.GetType() || typeof(short) == getValue().value.GetType() || typeof(byte) == getValue().value.GetType() || typeof(sbyte) == getValue().value.GetType() || typeof(uint) == getValue().value.GetType() || typeof(ulong) == getValue().value.GetType() || typeof(ushort) == getValue().value.GetType() || typeof(nint) == getValue().value.GetType() || typeof(nuint) == getValue().value.GetType())
        {
            return "integer";
        }
        else if (typeof(float) == getValue().value.GetType() || typeof(double) == getValue().value.GetType() || typeof(decimal) == getValue().value.GetType())
        {
            return "float";
        }
        else if (typeof(string) == getValue().value.GetType())
        {
            return "string";
        }
        else if (typeof(bool) == getValue().value.GetType())
        {
            return "bool";
        }
        return getValue().value.GetType().ToString();
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
            return "integer";
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

    public hashType attributeNameToValue(string attributeName)
    {
        return rootValue[attributeName].ConvertTo<hashType>();
    }
    public hashType attributeNameToValue(hashType hashType, string attributeName)
    {
        return hashType.value[attributeName].ConvertTo<hashType>();
    }
    public hashType pathToValue(string path)
    {
        throw new NotImplementedException();
    }
    public hashType pathToValue(hashType hashType, string path)
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
                rootValue = tempTable;
                thisClass = classStack.Pop();
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

    public string jsonValueTojsonString()
    {
        string json = "";
        if (valueClass.array == thisClass)
        {
            json += "[";
            for (int i = 0; i < rootValue.Count; i++)
            {
                json += jsonValueTojsonString(indexToValue(i));
                if (i != rootValue.Count - 1)
                {
                    json += ", ";
                }
            }
            json += "]";
            return json;
        }
        else if (valueClass.@object == thisClass)
        {
            json += "{";
            int i = 0;
            foreach (string label in rootValue.Keys)
            {
                json += label + " : " + jsonValueTojsonString(attributeNameToValue(label));
                if (i++ != rootValue.Count - 1)
                {
                    json += ", ";
                }
            }
            json += "}";
            return json;
        }
        return getValue().value.ToString();
    }

    private string jsonValueTojsonString(hashType hashType)
    {
        string json = "";
        if (valueClass.array == hashType.valueClass)
        {
            json += "[";
            for (int i = 0; i < hashType.value.Count; i++)
            {
                json += jsonValueTojsonString(indexToValue(hashType, i));
                if (i != hashType.value.Count - 1)
                {
                    json += ", ";
                }
            }
            json += "]";
            return json;
        }
        else if (valueClass.@object == hashType.valueClass)
        {
            json += "{";
            int i = 0;
            foreach (string label in hashType.value.Keys)
            {
                json += label + " : " + jsonValueTojsonString(attributeNameToValue(hashType, label));
                if (i++ != hashType.value.Count - 1)
                {
                    json += ", ";
                }
            }
            json += "}";
            return json;
        }
        return hashType.value.ToString();
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
