using System.Collections.Generic;
using Sprache;

namespace Algorithm.Training.Parsing.Parsers;

public static class NumericParse
{
    public static readonly Parser<IOption<char>> SignChar = Parse.Char('-').Optional();

    public static readonly Parser<int> Sign = SignChar.Select(x => x.IsDefined ? -1 : 1);

    public static readonly Parser<IEnumerable<int>> DigitsOrEmpty = Parse.Digit.Select(CharExtensions.ToInt)
        .Many();

    public static readonly Parser<IEnumerable<int>> Digits = Parse.Digit.Select(CharExtensions.ToInt)
        .AtLeastOnce();
}