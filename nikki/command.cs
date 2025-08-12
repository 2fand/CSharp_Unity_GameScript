using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class command
{
    public uint valueCount = 0;
    public const string commandName = "";
    public const string commandRecommend = "";
    public abstract void setVaule(string value);
    public abstract void execute();
    public static Hashtable stringCommands = new Hashtable();
}
public class tele : command
{
    public override void setVaule(string value)
    {
        throw new System.NotImplementedException();
    }
    public override void execute()
    {
        throw new System.NotImplementedException();
    }
}
public class note : command
{
    public override void setVaule(string value)
    {
        throw new System.NotImplementedException();
    }
    public override void execute()
    {
        throw new System.NotImplementedException();
    }
}
public class help : command
{
    public override void setVaule(string value)
    {
        throw new System.NotImplementedException();
    }
    public override void execute()
    {
        throw new System.NotImplementedException();
    }
}
public class _move : command
{
    public override void setVaule(string value)
    {
        throw new System.NotImplementedException();
    }
    public override void execute()
    {
        throw new System.NotImplementedException();
    }
}
public class _show : command
{
    public override void setVaule(string value)
    {
        throw new System.NotImplementedException();
    }
    public override void execute()
    {
        throw new System.NotImplementedException();
    }
}
public class _hide : command
{
    public override void setVaule(string value)
    {
        throw new System.NotImplementedException();
    }
    public override void execute()
    {
        throw new System.NotImplementedException();
    }
}

public class _play : command
{
    public override void setVaule(string value)
    {
        throw new System.NotImplementedException();
    }
    public override void execute()
    {
        throw new System.NotImplementedException();
    }
}