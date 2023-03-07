using FluentAssertions;
using Xunit;

namespace Algorithm.Training
{
    public class Task5
    {
        public int CountPages(int total, int pageSize)
        {
            //return total / total + (total % pageSize > 0 ? 1 : 0);
            //return (total + pageSize - 1) / pageSize;
            return (total - 1) / pageSize + 1;
        }
        
        [Theory]
        [InlineData(2, 3, 1)]
        [InlineData(3, 3, 1)]
        [InlineData(4, 3, 2)]
        [InlineData(5, 3, 2)]
        [InlineData(6, 3, 2)]
        [InlineData(7, 3, 3)]
        public void Acceptance(int total, int pageSize, int expected)
        {
            CountPages(total, pageSize).Should().Be(expected);
        }
    }
}