using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class error
{
    public static string defaultFormatedExceptionString(int commandI, int? valueColumn, int? valueaColumn, string errorString)
    {
        if (null == valueColumn && null == valueaColumn)
        {
            return errorString;
        }
        else if (null == valueaColumn)
        {
            return "(第" + commandI + "行第" + valueColumn + "列)" + errorString;
        }
        else if (null == valueColumn)
        {
            return "(第" + commandI + "行第" + valueaColumn + "列)" + errorString;
        }
        return "(第" + commandI + "行第" + valueColumn + "列，第" + commandI + "行第" + valueaColumn + "列)" + errorString;
    }
}
