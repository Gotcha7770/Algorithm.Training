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
    
    public static float Parse(string input)
    {
        if (input is null)
            throw new ArgumentNullException(nameof(input));

        if (input.Length == 0)
            throw new FormatException("string length is 0");

        return input[0] == '-' 
            ? -1 * FloatParser.ParseWithoutSign(input, 1) 
            : FloatParser.ParseWithoutSign(input);
    }

    public static float ParseWithoutSign(string input, int startIndex = 0)
    {
        int pointIndex = input.IndexOf('.');
        if (pointIndex < 1)
            return IntParser.ParseWithoutSign(input);

        float integerPart = IntParser.ParseWithoutSign(input[..pointIndex], startIndex);
        int scale = input.Length - (pointIndex + 1);
        if(scale <= 0)
            return integerPart;

        float fractionalPart = IntParser.DigitsSequenceReverse(input, pointIndex + 1)
            .Aggregate(0F, (acc, current) => (acc + current) / 10);

        return integerPart + fractionalPart;
    }

    public static float FromFractional(IEnumerable<uint> digits) => digits.Reverse().Aggregate(0F, (acc, current) => (acc + current) / 10);
}
