using System;
using System.ComponentModel;
using System.Globalization;

namespace Algorithm.Training.Parsing.Float;

public class FloatToStringConverterV2 : TypeConverter
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

            int bits = BitConverter.SingleToInt32Bits(f);

            char[] result = new char[36];
            result[0] = '0';
            result[1] = 'b';
            result[2] = bits < 0 ? '1' : '0';
            result[3] = '.';

            for (int i = 0; i < 23; i++)
            {
                result[35-i] = (bits & 1) == 0 ? '0' : '1';
                bits >>= 1;
            }

            result[12] = '.';

            for (int i = 0; i < 8; i++)
            {
                result[11-i] = (bits & 1) == 0 ? '0' : '1';
                bits >>= 1;
            }

            return new string(result);
        }
        
        return base.ConvertTo(context, culture, value, destinationType);
    }
}