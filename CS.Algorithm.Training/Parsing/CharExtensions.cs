namespace Algorithm.Training.Parsing;

public static class CharExtensions
{
    public static uint ToInt(this char x) => (uint)(x - '0');

    public static bool TryGetInt(this char x, out uint output)
    {
        if (char.IsDigit(x))
        {
            output = ToInt(x);
            return true;
        }

        output = 0;
        return false;
    }
}