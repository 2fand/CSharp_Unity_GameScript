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
    public Hashtable value = new Hashtable();
    Stack<Hashtable> stack = new Stack<Hashtable>();
    Stack<valueClass> classStack = new Stack<valueClass>();
    List<string> indexStack = new List<string>();
    List<bool> hasValue = new List<bool>();
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
        if (!value.ContainsKey(null))
        {
            value.Add(null, null);
        }
        value[null] = v;
        return v;
    }
    public typeValue getValue()
    {
        return new typeValue(value[null]);
    }
    
    public typeValue indexToValue(int index)
    {
        return new typeValue(value[index.ToString()]);
    }
    
    public void setArray(uint size, object defaultObject = null)
    {
        thisClass = valueClass.array;
        value.Clear();
        for (int i = 0; i < size; i++)
        {
            value.Add(i.ToString(), defaultObject);
        }
    }
    
    public void setObject(string[] attributeNames, object[] objects)
    {
        thisClass = valueClass.@object;
        value.Clear();
        for (int i = 0; i < Mathf.Min(attributeNames.Length, objects.Length); i++) 
        {
            value.Add(attributeNames[i], objects[i]);
        }
    }

    public void setObjectAttribute(string attributeName, object @object)
    {
        thisClass = valueClass.@object;
        value.Clear();
        value.Add(attributeName, @object);
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
        return getValue().type.ToString();
    }

    public typeValue attributeNameToValue(string attributeName)
    {
        return new typeValue(value[attributeName]);
    }
    public typeValue pathToValue(string path)
    {
        throw new NotImplementedException();
    }
    
    void EndGroup()
    {
        classStack.Pop();
        indexStack.RemoveAt(indexStack.Count - 1);
        Hashtable tempTable = stack.Pop();
        hasValue.RemoveAt(hasValue.Count - 1);
        if (null != indexStack[indexStack.Count - 1])
        {
            stack.Peek().Add(indexStack[indexStack.Count - 1], tempTable);
        }
    }
    public bool setValue(string json)
    {
        stack = new Stack<Hashtable>();
        classStack = new Stack<valueClass>();
        indexStack = new List<string>();
        hasValue = new List<bool>();
        Action addOther = () => {
            stack.Push(new Hashtable());
            indexStack.Add(null);
            hasValue.Add(false);
        };
        Func<string, string, Match> matchHaveSpace = (string input, string pattern) =>
        {
            return Regex.Match(input, "^\\s*" + pattern + "\\s*$");
        };
        for (int i = 0; i < json.Length; )                              
        {                                                               
            if (Regex.Match(json.Substring(i), "^\\s*\\[\\s*").Success)
            {
                hasValue[hasValue.Count - 1] = true;
                if (valueClass.array == classStack.Peek())
                {
                    indexStack[indexStack.Count - 1] = null == indexStack[indexStack.Count - 1] ? "0" : (int.Parse(indexStack[indexStack.Count - 1]) + 1).ToString();
                }
                if (value.ContainsKey(null))
                {
                    return false;
                }
                classStack.Push(valueClass.array);
                addOther();
                i += Regex.Match(json.Substring(i), "^\\s*\\[\\s*").Length;
            }
            else if (Regex.Match(json.Substring(i), "^\\s*\\{\\s*").Success)
            {
                hasValue[hasValue.Count - 1] = true;
                if (valueClass.array == classStack.Peek())
                {
                    indexStack[indexStack.Count - 1] = null == indexStack[indexStack.Count - 1] ? "0" : (int.Parse(indexStack[indexStack.Count - 1]) + 1).ToString();
                }
                if (value.ContainsKey(null))
                {
                    return false;
                }
                classStack.Push(valueClass.@object);
                addOther();
                i += Regex.Match(json.Substring(i), "^\\s*{\\s*").Length;
            }
            else if (matchHaveSpace(json.Substring(i), "\\]").Success)
            {
                if (0 == stack.Count || valueClass.array != classStack.Peek())
                {
                    return false;
                }
                EndGroup();
                i += Regex.Match(json.Substring(i), "^\\s*\\]\\s*").Length;
            }
            else if (matchHaveSpace(json.Substring(i), "}").Success)
            {
                if (0 == stack.Count || valueClass.@object != classStack.Peek())
                {
                    return false;
                }
                EndGroup();
                i += Regex.Match(json.Substring(i), "^\\s*}\\s*").Length;
            }
            else if(0 != classStack.Count && matchHaveSpace(json.Substring(i), ",").Success)
            {
                if (!hasValue[hasValue.Count - 1])
                {
                    return false;
                }
                i += matchHaveSpace(json.Substring(i), ",").Length;
                hasValue[hasValue.Count - 1] = false;
            }
            else
            {
                if (valueClass.array == classStack.Peek())
                {
                    indexStack[indexStack.Count - 1] = null == indexStack[indexStack.Count - 1] ? "0" : (int.Parse(indexStack[indexStack.Count - 1]) + 1).ToString();
                }
                string matchString = Regex.Match(json.Substring(i), "^\\s*[^\\s]*").Value;
                if (valueClass.@object == classStack.Peek())
                {
                    indexStack[indexStack.Count - 1] = matchHaveSpace(matchString, "\"\".+?\"\"").Value;
                    i += indexStack[indexStack.Count - 1].Length;
                    matchString = Regex.Match(json.Substring(i), "^\\s*[^\\s]*").Value;
                    i += matchHaveSpace(matchString, ":").Length;
                    matchString = Regex.Match(json.Substring(i), "^\\s*[^\\s]*").Value;
                }
                Match match = Regex.Match(matchString, "^\\s*[+-]?([0-9]+(\\.([+-]?[0-9]+)?)?|\\.[0-9]+)([eE][+-]?[0-9]+)?\\s*$");
                if (match.Success)
                {
                    if (0 == stack.Count)
                    {
                        if (value.ContainsKey(null))
                        {
                            return false;
                        }
                        setValue(float.Parse(Regex.Match(match.Value, "^[^eE]+").Value) * Mathf.Pow(10, int.Parse(Regex.Match(match.Value, "[eE].+$").Value.Substring(1))));
                    }
                    else
                    {
                        stack.Peek().Add(indexStack[indexStack.Count - 1], float.Parse(Regex.Match(match.Value, "^[^eE]+").Value) * Mathf.Pow(10, int.Parse(Regex.Match(match.Value, "[eE].+$").Value.Substring(1))));
                    }
                    i += match.Length;
                    hasValue[hasValue.Count - 1] = true;
                }
                match = matchHaveSpace(matchString, "[+-]?[0-9]+");
                if (match.Success)
                {
                    if (0 == stack.Count)
                    {
                        if (value.ContainsKey(null))
                        {
                            return false;
                        }
                        setValue(int.Parse(Regex.Match(match.Value, "^[^eE]+").Value) * (int)Mathf.Pow(10, int.Parse(Regex.Match(match.Value, "[eE].+$").Value.Substring(1))));
                    }
                    else
                    {
                        stack.Peek().Add(indexStack[indexStack.Count - 1], int.Parse(Regex.Match(match.Value, "^[^eE]+").Value) * (int)Mathf.Pow(10, int.Parse(Regex.Match(match.Value, "[eE].+$").Value.Substring(1))));
                    }
                    i += match.Length;
                    hasValue[hasValue.Count - 1] = true;
                    continue;
                }
                if (matchHaveSpace(matchString, "true").Success)
                {
                    if (0 == stack.Count)
                    {
                        if (value.ContainsKey(null))
                        {
                            return false;
                        }
                        setValue(true);
                    }
                    else
                    {
                        stack.Peek().Add(indexStack[indexStack.Count - 1], true);
                    }
                    i += match.Length;
                    hasValue[hasValue.Count - 1] = true;
                    continue;
                }
                if (matchHaveSpace(matchString, "false").Success)
                {
                    if (0 == stack.Count)
                    {
                        if (value.ContainsKey(null))
                        {
                            return false;
                        }
                        setValue(false);
                    }
                    else
                    {   
                        stack.Peek().Add(indexStack[indexStack.Count - 1], false);
                    }
                    i += match.Length;
                    hasValue[hasValue.Count - 1] = true;
                    continue;
                }
                if (Regex.Match(matchString, "^\\s*null\\s*$").Success)
                {
                    if (0 == stack.Count)
                    {
                        if (value.ContainsKey(null))
                        {
                            return false;
                        }
                        setValue(null);
                    }
                    else
                    {
                        stack.Peek().Add(indexStack[indexStack.Count - 1], null);
                    }
                    i += match.Length;
                    hasValue[hasValue.Count - 1] = true;
                    continue;
                }
                match = Regex.Match(matchString, "^\\s*\"\".+?\"\"\\s*$");
                if (match.Success) {

                    if (0 == stack.Count)
                    {
                        if (value.ContainsKey(null))
                        {
                            return false;
                        }
                        setValue(match.Value.Substring(0, match.Value.Length - 1).Substring(1));
                    }
                    else
                    {
                        stack.Peek().Add(indexStack[indexStack.Count - 1], match.Value.Substring(0, match.Value.Length - 1).Substring(1));
                    }
                    i += match.Length;
                    hasValue[hasValue.Count - 1] = true;
                    continue;
                }
                return false;
            }
        }
        return stack.Count == 0;
    }
}
