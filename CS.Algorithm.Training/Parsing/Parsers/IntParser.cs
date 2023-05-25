using System;

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
            ? -1 * ParseWithoutSign(input.AsSpan(1))
            : ParseWithoutSign(input);
    }

    public static int ParseWithoutSign(string input) => ParseWithoutSign(input.AsSpan());

    public static int ParseWithoutSign(ReadOnlySpan<char> input)
    {
        int result = 0;
        for (int i = 0; i < input.Length; i++)
        {
            char character = input[i];
            if (!char.IsDigit(character))
                throw new FormatException($"invalid character at {i} index");
            result = result * 10 + character.ToInt();
        }

        return result;
    }
}