using System.Collections.Generic;
using NUnit.Framework;

namespace Algorithm.Training
{
    [TestFixture]
    public class Task2
    {
        // Даны две поисковые выдачи размера N, каждая из которых
        // состоит из уникальных неотрицательных идентификаторов документов.
        // Требуется посчитать количество общих документов в top-K выдачах
        // для K от 1 до N.
        // top-K выдачей называется выдача первых K результатов как из
        // первой, так и из второй поисковой выдачи.
        
        // Например:
        // ([0, 1, 2, 3, 4], [0, 3, 4, 2, 1]) => [1, 1, 1, 3, 5]
        // Первый элемент 1, потому что в [0] и [0] один общий документ.
        // Второй элемент 1, потому что в [0, 1] и [0, 3] один общий документ.
        // Третий элемент 1, потому что в [0, 1, 2] и [0, 3, 4] один общий документ.
        // Четвертый элемент 3, потому что в [0, 1, 2, 3] и [0, 3, 4, 2] три общих документа (0, 2 и 3).
        // Пятый элемент 5, потому что в [0, 1, 2, 3, 4] и [0, 3, 4, 2, 1] встречается каждый идентификатор документа.

        public int[] CountDuplicates(int[] one, int[] other)
        {
            int counter = 0;
            int[] result = new int[one.Length]; 
            var set1 = new HashSet<int>();
            var set2 = new HashSet<int>();
            
            for (int i = 0; i < one.Length; i++)
            {
                if (one[i] == other[i])
                {
                    ++counter;
                }
                else
                {
                    set1.Add(one[i]);
                    set2.Add(other[i]);
                    
                    counter += set2.Contains(one[i]) ? 1 : 0;
                    counter += set1.Contains(other[i]) ? 1 : 0;
                }

                result[i] = counter;
            }

            return result;
        }
        
        [Test]
        public void Acceptance()
        {
            var first = new [] {0, 1, 2, 3, 4};
            var second = new [] {0, 3, 4, 2, 1};

            var result = CountDuplicates(first, second);
            
            CollectionAssert.AreEqual(new []{1, 1, 1, 3, 5}, result);
        }
    }
}