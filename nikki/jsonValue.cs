using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class typeValue
{
    public Type type;
    public object value;
    public typeValue(object value)
    {
        this.value = value; 
        this.type = value.GetType();
    }
}

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
    public typeValue getValue()
    {
        return new typeValue(rootValue[0]);
    }
    
    public typeValue indexToValue(int index)
    {
        return new typeValue(rootValue[index.ToString()]);
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
    
    public void setObject(string[] attributeNames, object[] objects)
    {
        thisClass = valueClass.@object;
        rootValue.Clear();
        for (int i = 0; i < Mathf.Min(attributeNames.Length, objects.Length); i++) 
        {
            rootValue.Add(attributeNames[i], objects[i]);
        }
    }

    public void setObjectAttribute(string attributeName, object @object)
    {
        thisClass = valueClass.@object;
        rootValue.Clear();
        rootValue.Add(attributeName, @object);
    }

    public string getType()
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
        else if (typeof(int) == getValue().type || typeof(long) == getValue().type || typeof(short) == getValue().type || typeof(byte) == getValue().type || typeof(sbyte) == getValue().type || typeof(uint) == getValue().type || typeof(ulong) == getValue().type || typeof(ushort) == getValue().type || typeof(nint) == getValue().type || typeof(nuint) == getValue().type)
        {
            return "integer";
        }
        else if (typeof(float) == getValue().type || typeof(double) == getValue().type || typeof(decimal) == getValue().type)
        {
            return "float";
        }
        else if (typeof(string) == getValue().type)
        {
            return "string";
        }
        else if (typeof(bool) == getValue().type)
        {
            return "bool";
        }
        return getValue().type.ToString();
    }

    public typeValue attributeNameToValue(string attributeName)
    {
        return new typeValue(rootValue[attributeName]);
    }
    public typeValue pathToValue(string path)
    {
        throw new NotImplementedException();
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
                Match match = Regex.Match(json.Substring(i), "^\\s*\"(\\\\\"|[^\\\\\"])*?\"\\s*:\\s*");
                if (!match.Success)
                {
                    return false;
                }
                i += match.Length;
                indexStack[indexStack.Count - 1] = Regex.Match(match.Value, "\"([^\\\\\"]|\\\\\")*?\"").Value.Substring(1, Regex.Match(match.Value, "\"([^\\\\\"]|\\\\\")*?\"").Length - 2);
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
            else if (Regex.Match(json.Substring(i), "^\\s*[+-]?([0-9]+(\\.([+-]?[0-9]+)?)?|\\.[0-9]+)([eE][+-]?[0-9]+)?\\s*").Success)
            {
                string floatNum = Regex.Match(json.Substring(i), "^\\s*[+-]?([0-9]+(\\.([+-]?[0-9]+)?)?|\\.[0-9]+)([eE][+-]?[0-9]+)?\\s*").Value;
                Match eMatch = Regex.Match(floatNum, "[eE][+-]?[0-9]+");
                addValue(float.Parse(Regex.Match(json.Substring(i), "^\\s*[+-]?([0-9]+(\\.([+-]?[0-9]+)?)?|\\.[0-9]+)").Value) * (eMatch.Success ? Mathf.Pow(10, int.Parse(eMatch.Value.Substring(1))) : 1));
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
            else if (Regex.Match(json.Substring(i), "^\\s*\"(\\\\\"|[^\\\\\"])*?\"\\s*").Success)
            {
                addValue(Regex.Replace(noTwoSides(Regex.Match(Regex.Match(json.Substring(i), "^\\s*\"(\\\\\"|[^\\\\\"])*?\"\\s*").Value, "\"(\\\\\"|[^\\\\\"])*?\"").Value), applyEspase ? "\\\\\"" : "", applyEspase ? "\"" : ""));
                i += Regex.Match(json.Substring(i), "^\\s*\"(\\\\\"|[^\\\\\"])*?\"\\s*").Length;
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
}
