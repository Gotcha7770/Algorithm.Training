using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm.Training.Parsing.Parsers;

public static class FloatParser
{
    /// <summary>
    /// Returns integer number that represented fraction in binary form
    /// </summary>
    /// <param name="fractional">fraction part of float</param>
    /// <param name="precision">count of bits to represent</param>
    /// <see cref="https://www.youtube.com/watch?v=RuKkePyo9zk&t=476s&ab_channel=ComputerScience"/>
    public static uint DecimalFractionToBinary(float fractional, int precision)
    {
        return DecimalFractionToBinarySequence(fractional)
            .Take(precision)
            .Aggregate((acc, cur) => (acc << 1) + cur);
    }

    public static IEnumerable<uint> DecimalFractionToBinarySequence(float fractional)
    {
        while (true)
        {
            if (fractional == 0.0)
                yield return 0;
            
            fractional *= 2;
            var integerPart = (byte)fractional; // берем целую часть
            fractional -= integerPart; // отбрасываем целую часть

            yield return integerPart;
        }
    }

    public static float ParseWithoutSign(string input, int startIndex = 0)
    {
        int pointIndex = input.IndexOf('.');
        if (pointIndex < 1)
            return IntParser.ParseWithoutSign(input);

        int integerPart = IntParser.ParseWithoutSign(input[..pointIndex], startIndex);
        float scale = MathF.Pow(0.1F, input.Length - (pointIndex + 1));
        float fractionalPart = IntParser.ParseWithoutSign(input[(pointIndex + 1)..]) * scale;
        if (fractionalPart == 0)
            return integerPart;

        uint exponent = GetExponent(integerPart, fractionalPart);
        uint mantissa = GetMantissa(integerPart, fractionalPart, pointIndex);

        return BitConverter.UInt32BitsToSingle(exponent + mantissa);
    }

    private static uint GetExponent(int integerPart, float fractionalPart)
    {
        uint result = (uint)(integerPart > 0 
            ? 127 + MathF.ILogB(integerPart) 
            : 127 + MathF.ILogB(fractionalPart));

        return result << 23;
    }

    private static uint GetMantissa(int integerPart, float fractionalPart, int pointIndex)
    {
        if (integerPart == 0)
        {
            return DecimalFractionToBinarySequence(fractionalPart)
                .SkipWhile(x => x == 0)
                .Take(1..24)
                .Aggregate((acc, cur) => (acc << 1) + cur);
        }
        
        uint result = (uint)integerPart << 23;
        result ^= 0b1000000000000000000000000000;
        result <<= 1;
        result += DecimalFractionToBinarySequence(fractionalPart)
            .Take(23 - pointIndex)
            .Aggregate((acc, cur) => (acc << 1) + cur);

        return result;
    }

    public static float CreateFloat(IEnumerable<uint> integer, IEnumerable<uint> fractional, int pointIndex)
    {
        throw new NotImplementedException();
        // 1bit - sign
        // 8bit - exponent
        // 23bit - mantissa

        // int integerPart = IntParser.FromDigits(integer);
        // int fractionalPart = IntParser.FromDigits(fractional);
        // uint exponent = GetExponent(integerPart, fractionalPart);
        // uint mantissa = GetMantissa(integerPart, fractionalPart, pointIndex);
        //
        // return Convert.ToSingle(exponent + mantissa);
    }
}

internal class NumberBuffer
{
    public NumberBuffer(string input, int startIndex, int pointIndex)
    {
        IntegerSequence = IntParser.DigitsSequence(input[..pointIndex], startIndex);
        FractionalPart = IntParser.ParseWithoutSign(input[(pointIndex + 1)..]);
        float factor = HasFractionalPart ? MathF.Pow(0.1F, input.Length - (pointIndex + 1)) : 0;
        FractionalPart *= factor;
    }

    public IEnumerable<uint> IntegerSequence { get; }
    public float FractionalPart { get; }
    public bool HasFractionalPart => FractionalPart != 0;

    public float ToSingle()
    {
        uint integerPart = IntParser.FromDigits(IntegerSequence);
        if (!HasFractionalPart)
            return integerPart;
        
        uint exponent = GetExponent(integerPart);
        uint mantissa = GetMantissa(integerPart);
        return BitConverter.UInt32BitsToSingle(exponent + mantissa);
    }
    
    private uint GetExponent(uint integerPart)
    {
        uint result = (uint)(integerPart > 0 
            ? 127 + MathF.ILogB(integerPart) 
            : 127 + MathF.ILogB(FractionalPart));

        return result << 23;
    }

    private uint GetMantissa(uint integerPart)
    {
        var fractionalSequence = FloatParser.DecimalFractionToBinarySequence(FractionalPart);
        if (integerPart == 0)
        {
            return fractionalSequence
                .SkipWhile(x => x == 0)
                .Take(1..24)
                .Aggregate((acc, cur) => (acc << 1) + cur);
        }

        return IntegerSequence
            .Concat(fractionalSequence)
            .Take(1..24)
            .Aggregate((acc, cur) => (acc << 1) + cur);
    }
}