using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class DivisibleBy15Attribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime time)
        {
            int minutes = time.Minute;
            return minutes % 15 == 0;
        }

        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be divisible by 15.";
    }
}
