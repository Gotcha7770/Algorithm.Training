using Xunit;

namespace Algorithm.Training;

public class Task22
{
    // two players take turns removing (or "nimming") matches from pile.
    // On each turn, a player must remove 1, 2 or 3 matches
    // The one who takes the last matches wins
    // What if tha goal was to avoid taking the last matches?
    // What if player could take 4 matches? k matches?
    // What if the game involves several separate piles?
    
    // https://ru.algorithmica.org/cs/games/nim/
    
    [Theory]
    [InlineData(1)]
    public void Acceptance(int numberOfMatches)
    {
        var game = new NimGame();
    }
}

public class NimGame
{
    private readonly int[] _numbers;

    public NimGame(params int[] numbers)
    {
        _numbers = numbers;
    }
}