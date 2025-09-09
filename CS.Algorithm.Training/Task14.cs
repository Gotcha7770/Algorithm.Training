using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using Xunit;

namespace Algorithm.Training;

// несколько вариантов решения FizzBuzz

public class Task14
{
    [Fact]
    public void FizzBuzz1()
    {
        string Selector(int x)
        {
            string output = null;
            if (x % 3 == 0) output += "Fizz";
            if (x % 5 == 0) output += "Buzz";

            return output ?? x.ToString();
        }

        var result = Enumerable.Range(1, 25).Select(Selector);

        result.Should().BeEquivalentTo(_standard);
    }

    [Fact]
    public void FizzBuzz2()
    {
        string Selector(int x) =>
            (x % 3, x % 5) switch
            {
                (0, 0) => "FizzBuzz",
                (0, _) => "Fizz",
                (_, 0) => "Buzz",
                _ => x.ToString()
            };

        var result = Enumerable.Range(1, 25).Select(Selector);

        result.Should().BeEquivalentTo(_standard);
    }

    [Fact]
    public void FizzBuzz3()
    {
        string Selector(int x)
        {
            return x % 15 == 0
                ? "FizzBuzz"
                : x % 3 == 0
                    ? "Fizz"
                    : x % 5 == 0
                        ? "Buzz"
                        : x.ToString();
        }

        var result = Enumerable.Range(1, 25).Select(Selector);

        result.Should().BeEquivalentTo(_standard);
    }

    [Fact]
    public void FizzBuzz4()
    {
        string Selector(int x)
        {
            return $"{(x % 3 * x % 5 == 0 ? 0 : x):#;}{x % 3:;;Fizz}{x % 5:;;Buzz}";
        }

        var result = Enumerable.Range(1, 25)
            .Select(Selector);

        result.Should().BeEquivalentTo(_standard);
    }

    [Fact]
    public void FizzBuzz6()
    {
        IEnumerable<string> Iterator(int x)
        {
            if(x % 3 == 0) yield return "Fizz";
            if(x % 5 == 0) yield return "Buzz";
        }

        var result = Enumerable.Range(1, 25)
            .Select(x => string.Join("", Iterator(x).DefaultIfEmpty(x.ToString())));

        result.Should().BeEquivalentTo(_standard);
    }

    [Fact]
    public void FizzBuzz7()
    {
        string[] fizzBuzzCycle =
        [
            "FizzBuzz",
            "{0}",
            "{0}",
            "Fizz",
            "{0}",
            "Buzz",
            "Fizz",
            "{0}",
            "{0}",
            "Fizz",
            "Buzz",
            "{0}",
            "Fizz",
            "{0}",
            "{0}"
        ];

        string Selector(int x)
        {
            var template = fizzBuzzCycle[x % fizzBuzzCycle.Length];
            return string.Format(template, x);
        }

        var result = Enumerable.Range(1, 25).Select(Selector);
        result.Should().BeEquivalentTo(_standard);
    }

    [Fact]
    public void FizzBuzz8()
    {
        for (int i = 1; i <= 25; i++)
        {
            if (i % 3 == 0)
                Console.Write("Fizz");
            if (i % 5 == 0)
                Console.Write("Buzz");
            if (i % 3 != 0 && i % 5 != 0)
                Console.Write(i);

            Console.Write(Environment.NewLine);
        }
    }

    private readonly string[] _standard =
    [
        "1",
        "2",
        "Fizz",
        "4",
        "Buzz",
        "Fizz",
        "7",
        "8",
        "Fizz",
        "Buzz",
        "11",
        "Fizz",
        "13",
        "14",
        "FizzBuzz",
        "16",
        "17",
        "Fizz",
        "19",
        "Buzz",
        "Fizz",
        "22",
        "23",
        "Fizz",
        "Buzz"
    ];
}