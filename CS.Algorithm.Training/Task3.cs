using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task3
{
    // Напишите функцию, которая развернёт список.
    // Последний элемент должен стать первым, а первый - последним.

    #region CustomList

    private class Node<T>
    {
        public T Value { get; }
            
        public Node<T> Next { get; set; }

        private Node(T value) => Value = value;

        public static implicit operator T(Node<T> node) => node.Value;
            
        public static implicit operator Node<T>(T value) => new Node<T>(value);
    }

    private class List<T> : IEnumerable<Node<T>>
    {
        public Node<T> Head { get; private set; }

        public List(params T[] values)
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

            Node<T> prev = null;
            var current = Head;
            
            while (current is not null)
            {
                var next = current.Next;
                current.Next = prev;
                prev = current;
                current = next;
            }

            Head = prev;
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<T> Values => ValuesIterator();
            
        private IEnumerable<T> ValuesIterator()
        {
            using (var enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }
    }

    [Fact]
    public void Acceptance_Empty()
    {
        var list = new List<int>();
        list.Reverse();
            
        list.Should().BeEmpty();
    }

    [Fact]
    public void Acceptance_Single()
    {
        var list = new List<int>(1);
        list.Reverse();
            
        list.Values.Should().BeEquivalentTo(new []{1});
    }

    [Fact]
    public void Acceptance_Many()
    {
        var list = new List<int>(1, 2, 3, 4, 5);
        list.Reverse();
            
        list.Values.Should().BeEquivalentTo(new []{5, 4, 3, 2, 1});
    }
        

    #endregion

    #region LinkedList

    void Reverse<T>(LinkedList<T> source)
    {
        var head  = source.First;
        while (head?.Next is not null)
        {
            var next = head.Next;
            source.Remove(next);
            source.AddFirst(next);
        }
    }

    [Fact]
    public void LinkedList_Acceptance_Empty()
    {
        var list = new LinkedList<int>();
        Reverse(list);
            
        list.Should().BeEmpty();
    }

    [Fact]
    public void LinkedList_Acceptance_Single()
    {
        var list = new LinkedList<int>(new[] { 1 });
        Reverse(list);
            
        list.Should().BeEquivalentTo(new []{1});
    }

    [Fact]
    public void LinkedList_Acceptance_Many()
    {
        var list = new LinkedList<int>(new[] { 1, 2, 3, 4, 5 });
        Reverse(list);
            
        list.Should().BeEquivalentTo(new []{5, 4, 3, 2, 1});
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

    [Fact]
    public void NewLinkedList_Acceptance_Empty()
    {
        var list = new LinkedList<int>();
        var result = Reverse2(list);
            
        result.Should().BeEmpty();
    }

    [Fact]
    public void NewLinkedList_Acceptance_Single()
    {
        var list = new LinkedList<int>(new[] { 1 });
        var result = Reverse2(list);
            
        result.Should().BeEquivalentTo(new[] { 1 });
    }

    [Fact]
    public void NewLinkedList_Acceptance_Many()
    {
        var list = new LinkedList<int>(new[] { 1, 2, 3, 4, 5 });
        var result = Reverse2(list);
            
        result.Should().BeEquivalentTo(new []{5, 4, 3, 2, 1});
    }

    #endregion
}