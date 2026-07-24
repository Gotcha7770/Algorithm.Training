using AwesomeAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task17
{
    // Suppose an array of length n sorted in ascending order is rotated between 1 and n times. For example, the array nums = [0,1,2,4,5,6,7] might become:
    //
    // [4,5,6,7,0,1,2] if it was rotated 4 times.
    // [0,1,2,4,5,6,7] if it was rotated 7 times.
    //
    //     Notice that rotating an array [a[0], a[1], a[2], ..., a[n-1]] 1 time results in the array [a[n-1], a[0], a[1], a[2], ..., a[n-2]].
    //
    // Given the sorted rotated array nums of unique elements, return the minimum element of this array.
    //
    //     You must write an algorithm that runs in O(log n) time.

    public int FindMin(int[] numbers)
    {
        int left = 0, right = numbers.Length - 1;

        while (left < right)
        {
            int mid = left + (right - left) / 2;

            if (numbers[mid] > numbers[right])
            {
                left = mid + 1;
            }
            else
            {
                right = mid;
            }
        }

        return numbers[left];
    }

    [Theory]
    [InlineData(new[] { 0, 1, 2, 4, 5, 6, 7 }, 0)]
    [InlineData(new[] { 1, 2, 4, 5, 6, 7, 0 }, 0)]
    [InlineData(new[] { 4, 5, 6, 7, 0, 1, 2 }, 0)]
    [InlineData(new[] { 6, 7, 0, 1, 2, 4, 5 }, 0)]
    public void Acceptance(int[] numbers, int expected)
    {
        FindMin(numbers).Should().Be(expected);
    }
}