using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace Algorithm.Training.Parsing.Parsers;

public static class ParseExt
{
    public static Parser<PositionAware<T>> WithPosition<T>(this Parser<T> parser)
    {
        return parser.Select(x => new PositionAware<T>(x)).Positioned();
    }

    public static Parser<T> Aggregate<T>(this Parser<IEnumerable<T>> parser, T seed, Func<T, T, T> func)
    {
        if (parser == null)
            throw new ArgumentNullException(nameof(parser));
        if (func == null)
            throw new ArgumentNullException(nameof(func));

        return parser.Then(x => Parse.Return(x.Aggregate(seed, func)));
    }

    public static Parser<IEnumerable<T>> Reverse<T>(this Parser<IEnumerable<T>> parser)
    {
        if (parser == null)
            throw new ArgumentNullException(nameof (parser));
        return parser.Select(x => x.Reverse());
    }
}