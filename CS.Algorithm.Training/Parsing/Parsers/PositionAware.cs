using Sprache;

namespace Algorithm.Training.Parsing.Parsers;

public class PositionAware<T> : IPositionAware<PositionAware<T>>
{
    public PositionAware(T value) => Value = value;

    public T Value { get; }
    public Position Start { get; private set; }
    public int Length { get; private set; }

    public PositionAware<T> SetPos(Position start, int length)
    {
        Start = start;
        Length = length;

        return this;
    }
}