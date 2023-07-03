using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Algorithm.Training.Parsing.Float;

// https://learn.microsoft.com/ru-ru/dotnet/api/system.bitconverter.getbytes?view=net-7.0

internal class FloatToStringConverterV1 : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return sourceType == typeof(float);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return destinationType == typeof(string);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (value is float f && CanConvertTo(destinationType))
        {
            if (float.IsPositiveInfinity(f))
                return "+Infinity";
            if (float.IsNegativeInfinity(f))
                return "-Infinity";
            if (float.IsNaN(f))
                return "NaN";

            var bitString = string.Join(string.Empty,
                    BitConverter.GetBytes(f)
                        .Reverse()
                        .Select(x => Convert.ToString(x, 2)
                            .PadLeft(8, '0')));

            return $"0b{bitString[0]}.{bitString[1..9]}.{bitString[9..]}" as object;
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}