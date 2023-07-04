using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm.Training.Parsing.Parsers;

public static class IntParser
{
    public static int Parse(string input)
    {
        if (input is null)
            throw new ArgumentNullException(nameof(input));

        if (input.Length == 0)
            throw new FormatException("string length is 0");

        return input[0] == '-' 
            ? -1 * ParseWithoutSign(input, 1)
            : ParseWithoutSign(input);
    }

    public static int ParseWithoutSign(string input, int startIndex = 0)
    {
        return (int)FromDigits(DigitsSequence(input, startIndex));
    }

    public static uint FromDigits(IEnumerable<uint> digits) => digits.Aggregate((acc, current) => acc * 10 + current);

    public static IEnumerable<uint> DigitsSequence(string source, int startIndex)
    {
        for (int i = startIndex; i < source.Length; i++)
        {
            if (source[i].TryGetInt(out uint digit))
            {
                yield return digit;
            }
            else
            {
                throw new FormatException($"invalid character at {i} index");
            }
        }
    }
    
    public static IEnumerable<uint> DigitsSequenceReverse(string source, int endIndex)
    {
        for (int i = source.Length - 1; i >= endIndex; i--)
        {
            if (source[i].TryGetInt(out uint digit))
            {
                yield return digit;
            }
            else
            {
                throw new FormatException($"invalid character at {i} index");
            }
        }
    }
}