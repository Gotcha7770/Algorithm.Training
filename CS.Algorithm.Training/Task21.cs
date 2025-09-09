using AwesomeAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task21
{
    [Theory]
    [InlineData(42, 6, 11, 42)]
    [InlineData(2, 6, 7, 7)]
    [InlineData(1, 16, 11, 16)]
    public void Acceptance(int a, int b, int c, int expected)
    {
        var result = Max(a, b, c);

        result.Should().Be(expected);
    }

    public static int Max(int a, int b, int c)
    {
        if (a > b)
        {
            if (a > c)
            {
                return a;
            }
            else
            {
                return c;
            }
        }
        else if (b > c)
        {
            return b;
        }
        else
        {
            return c;
        }
    }

    // public static int Max(int a, int b, int c)
    // {
    //     if (a > b)
    //     {
    //         return a > c ? a : c;
    //     }
    //     
    //     return b > c ? b : c;
    // }

    // public static int Max(int a, int b)
    // {
    //     return a > b ? a : b;
    // }
    
    // public static int Max(int a, int b)
    // {
    //     return Math.Max(a, b);
    // }

    // public static T Max<T>(T one, T other) where T : IComparableWithOperator<T>
    // {
    //     return one > other ? one : other;
    // }
    
    // public class Age : IComparable<Age>
    // {
    //     private readonly int _value;
    //
    //     public Age(int value)
    //     {
    //         _value = value;
    //     }
    //
    //     public int CompareTo(Age other)
    //     {
    //         return _value.CompareTo(other._value);
    //     }
    // }
    
    // public interface IComparableWithOperator<in T> : IComparable<T>
    // {
    //     public static bool operator <(IComparableWithOperator<T> left, T right) => left.CompareTo(right) < 0;
    //     public static bool operator >(IComparableWithOperator<T> left, T right) => left.CompareTo(right) > 0;
    // }
}