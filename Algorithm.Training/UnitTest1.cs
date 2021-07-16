using NUnit.Framework;

namespace Algorithm.Training
{
    public class RemoveDuplicatesFromSortedArray
    {
        public int RemoveDuplicates(int[] nums)
        {
            int index = 1;

            for (int i = 1; i < nums.Length; i++)
            {
                if (nums[i - 1] != nums[i])
                {
                    nums[index++] = nums[i];
                }
            }

            return index;
        }

        [Test]
        public void EmptyArray_ReturnsZero()
        {
            Assert.Fail();
        }

        [Test]
        public void ArrayWithSingleElement_ReturnsThatElement()
        {
            Assert.Fail();
        }

        // [Test]
        // public void ArrayWithSingleValue_ReturnsSingleElement()
        // {
        //     Assert.Fail();
        // }
        //
        // [Test]
        // public void ArrayWithSingleValue_ReturnsSingleElement()
        // {
        //     Assert.Fail();
        // }
    }
}