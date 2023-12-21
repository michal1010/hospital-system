using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class StartTimeBeforeEndTimeAttribute : ValidationAttribute
{
    private readonly string _endTimePropertyName;

    public StartTimeBeforeEndTimeAttribute(string endTimePropertyName)
    {
        _endTimePropertyName = endTimePropertyName;
    }

    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        var startTime = (DateTime)value;

        var endTimeProperty = validationContext.ObjectType.GetProperty(_endTimePropertyName);

        if (endTimeProperty == null)
        {
            throw new ArgumentException($"Property {_endTimePropertyName} not found on {validationContext.ObjectType}");
        }

        var endTime = (DateTime)endTimeProperty.GetValue(validationContext.ObjectInstance, null);

        if (startTime > endTime)
        {
            return new ValidationResult(ErrorMessage ?? "Start time must not be greater than end time.");
        }

        return ValidationResult.Success;
    }
}
