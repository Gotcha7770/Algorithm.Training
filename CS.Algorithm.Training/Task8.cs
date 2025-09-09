using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task8
{
    //задача input превратить в output
    // (независимая сортировка для разных типов)
        
    readonly string[] _input =
    [
        "Вишня",
        "1",
        "Боб",
        "3",
        "Яблоко",
        "22",
        "0",
        "Арбуз"
    ];

    readonly string[] _output =
    [
        "Арбуз",
        "22",
        "Боб",
        "3",
        "Вишня",
        "1",
        "0",
        "Яблоко"
    ];
        
    [Fact]
    public void Imperative()
    {
        string[] result = new string[_input.Length];
        string[] numbers = _input.Where(x => x.IsNumber())
            .OrderByDescending(int.Parse)
            .ToArray();
        string[] words = _input.Where(x => !x.IsNumber())
            .OrderBy(x => x)
            .ToArray();

        int n = 0;
        int w = 0;
        for (int i = 0; i < _input.Length; i++)
        {
            string item = _input[i];
            result[i] = item.IsNumber() ? numbers[n++] : words[w++];
        }
        
        result.Should().BeEquivalentTo(_output);
    }
    
    [Fact]
    public void Declarative()
    {
        var result = MixedSort(_input);

        result.Should().BeEquivalentTo(_output);
    }

    private IEnumerable<string> MixedSort(string[] values)
    {
        // ["1", "b", "2", "a"] -> []
        // сортировать можно только имея все элементы на руках
        //(input, strings, integers) => input is int ? integers.Next : strings.Next
        using var words = values.Where(x => !x.IsNumber())
            .OrderBy(x => x)
            .GetEnumerator();
        using var numbers = values.Where(x => x.IsNumber())
            .OrderByDescending(int.Parse)
            .GetEnumerator();

        foreach (string value in values)
        {
            if (value.IsNumber() && numbers.MoveNext())
            {
                yield return numbers.Current;
            }
            else if (words.MoveNext())
            {
                yield return words.Current;
            }
            else
            {
                yield break;
            }
        }
    }
}

public static class Strings
{
    public static bool IsNumber(this string value) => value.All(char.IsDigit);
}