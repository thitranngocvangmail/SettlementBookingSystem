using System;
using System.Collections.Generic;

public static class Check
{
    public static string NotNullOrWhiteSpace(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(parameterName + " can not be null, empty or white space!", parameterName);
        }
       
        return value;
    }

    public static DateTimeOffset CheckBetween(DateTimeOffset value, DateTimeOffset start, DateTimeOffset end, string parameterName)
    {
        if (!(value >= start && value <= end))
        {
            throw new ArgumentException(parameterName + " should be between " + start.ToString() + " and " + end.ToString(), parameterName);
        }
        return value;
    }
}