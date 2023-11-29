using System.Linq;
using FluentAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task8
{
    //задача input превратить в output
    // (независимая сортировка для разных типов)
        
    readonly string[] _input = new[]
    {
        "Вишня",
        "1",
        "Боб",
        "3",
        "Яблоко",
        "22",
        "0",
        "Арбуз"
    };

    readonly string[] _output = new[]
    {
        "Арбуз",
        "22",
        "Боб",
        "3",
        "Вишня",
        "1",
        "0",
        "Яблоко"
    };
        
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
            var item = _input[i];
            result[i] = item.IsNumber() ? numbers[n++] : words[w++];
        }
        
        result.Should().BeEquivalentTo(_output);
    }
    
    [Fact]
    public void Declarative()
    {
        // сортировать можно только имея все элементы на руках
        //(input, strings, integers) => input is int ? integers.Next : strings.Next

        var result = EnumerableEx.Create<string>(async yielder =>
        {
            using var control =  _input.Select(x => x.IsNumber()).GetEnumerator();
            using var words = _input.Where(x => !x.IsNumber()).OrderBy(x => x).GetEnumerator();
            using var numbers = _input.Where(x => x.IsNumber()).OrderByDescending(int.Parse).GetEnumerator();

            while (control.MoveNext())
            {
                var awaitable = control.Current switch
                {
                    true when numbers.MoveNext() => yielder.Return(numbers.Current),
                    false when words.MoveNext() => yielder.Return(words.Current),
                    _ => yielder.Break()
                };

                await awaitable;
            }
        });

        result.Should().BeEquivalentTo(_output);
    }
}

public static class Strings
{
    public static bool IsNumber(this string value) => value.All(char.IsDigit);
}