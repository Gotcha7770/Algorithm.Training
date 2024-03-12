using FluentAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task7
{
    // есть строка, состоит из предложений разделенных разными знаками типа точки, вопроса и т. д.
    // Все в нижнем регистре, надо первую букву каждого предложения перевести в верхний

    private readonly char[] _delimiters = ['.', '!', '?'];
    private readonly string _example = "тестовый текст. тестовый, тестовый текст?  тестовый! тест тест тест.";

    [Fact]
    public void Test()
    {
        string[] result = Divide(_example);

        result.Should()
            .BeEquivalentTo([
                "Тестовый текст.",
                "Тестовый, тестовый текст?",
                "Тестовый!",
                "Тест тест тест."
            ]);
    }

    private string[] Divide(string input)
    {
        throw new System.NotImplementedException();
    }
}