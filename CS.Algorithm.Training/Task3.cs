using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task3
{
    // Напишите функцию, которая развернёт список.
    // Последний элемент должен стать первым, а первый - последним.

    #region CustomList

    private record Node<T>(T Value)
    {
        public Node<T> Next { get; set; }

        public static implicit operator T(Node<T> node) => node.Value;

        public static implicit operator Node<T>(T value) => new(value);
    }

    private class CustomLinkedList<T> : IEnumerable<Node<T>>
    {
        public Node<T> Head { get; private set; }

        public CustomLinkedList(params T[] values)
        {
            if (values.Length > 0)
            {
                Head = values[0];
                var current = Head;

                for (int i = 1; i < values.Length; i++)
                {
                    current.Next = values[i];
                    current = current.Next;
                }
            }
        }

        public void Reverse()
        {
            // ()^, [1]^ -> [2] -> [3]
            // () <- [1]^,  [2]^ -> [3]
            // () <- [1] <- [2]^, [3]^

            Node<T> sourceHead = Head;
            Node<T> targetHead = null;

            while (sourceHead is not null)
            {
                // targetHead = sourceHead;
                // sourceHead = sourceHead.Next;
                var tmp = sourceHead;
                sourceHead = sourceHead.Next;
                tmp.Next = targetHead;
                targetHead = tmp;
            }

            Head = targetHead;
        }

        public IEnumerator<Node<T>> GetEnumerator()
        {
            var current = Head;

            while (current is not null)
            {
                yield return current;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Theory]
    [ClassData(typeof(ReverseLinkedListCases))]
    public void Acceptance(int[] input, int[] expected)
    {
        var list = new CustomLinkedList<int>(input);
        list.Reverse();

        list.Select(x => x.Value)
            .Should()
            .BeEquivalentTo(expected);
    }

    #endregion

    #region LinkedList

    void Reverse<T>(LinkedList<T> source)
    {
        var head = source.First;
        while (head?.Next is not null)
        {
            var next = head.Next;
            source.Remove(next);
            source.AddFirst(next);
        }
    }

    [Theory]
    [ClassData(typeof(ReverseLinkedListCases))]
    public void LinkedList_Acceptance(int[] input, int[] expected)
    {
        var list = new LinkedList<int>(input);
        Reverse(list);

        list.Should().BeEquivalentTo(expected);
    }

    #endregion

    #region NewLinkedList

    LinkedList<T> Reverse2<T>(LinkedList<T> source)
    {
        var result = new LinkedList<T>();

        for (var tail = source.Last; tail != null; tail = tail.Previous)
        {
            result.AddLast(tail.Value);
        }

        return result;
    }

    [Theory]
    [InlineData(new int[0], new int[0])]
    [InlineData(new[] { 1 }, new[] { 1 })]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 5, 4, 3, 2, 1 })]
    public void NewLinkedList_Acceptance(int[] input, int[] expected)
    {
        var list = new LinkedList<int>(input);
        var result = Reverse2(list);

        result.Should().BeEquivalentTo(expected);
    }

    #endregion
}

public class ReverseLinkedListCases : TheoryData<int[], int[]>
{
    public ReverseLinkedListCases()
    {
        Add([], []);
        Add([1], [1]);
        Add([1, 2, 3, 4, 5], [5, 4, 3, 2, 1]);
    }
}