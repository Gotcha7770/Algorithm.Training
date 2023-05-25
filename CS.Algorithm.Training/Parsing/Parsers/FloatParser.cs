using System;
using System.Linq;
using Algorithm.Training.Parsing.Float;

namespace Algorithm.Training.Parsing.Parsers;

public static class FloatParser
{
    /// <summary>
    /// Returns integer number that represented fraction in binary form
    /// </summary>
    /// <param name="fraction">fraction part of float</param>
    /// <param name="precision">count of bits to represent</param>
    /// <see cref="https://www.youtube.com/watch?v=RuKkePyo9zk&t=476s&ab_channel=ComputerScience"/>
    public static uint DecimalFractionToBinary(float fraction, byte precision)
    {
        uint result = 0;
        for (int i = 0; i < precision; i++)
        {
            result <<= 1; // считаем следующий разряд
            
            if (fraction == 0.0)
                continue;
            
            fraction *= 2;
            var integerPart = (byte)fraction; // берем целую часть
            result += integerPart;
            fraction -= integerPart; // отбрасываем целую часть
        }

        return result;
    }
}