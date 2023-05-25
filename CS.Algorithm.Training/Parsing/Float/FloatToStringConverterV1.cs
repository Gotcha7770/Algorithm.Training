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

            var span = string.Join(string.Empty,
                    BitConverter.GetBytes(f)
                        .Reverse()
                        .Select(x => Convert.ToString(x, 2)
                            .PadLeft(8, '0')))
                .AsSpan();

            return $"0b{span[0]}.{span[1..9]}.{span[9..]}" as object;
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}