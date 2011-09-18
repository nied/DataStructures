using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructures
{
    public class LinkedList<T> : IEnumerable<T>
    {
        private Node _head { get; set; }
        private Node _tail { get; set; }

        internal class Node
        {
            public T Value;

            public Node Previous { get; set; }
            public Node Next { get; set; }

            public Node(T value, Node previous, Node next)
            {
                Value = value;

                Previous = previous;
                Next = next;

                if (Previous != null)
                    Previous.Next = this;

                if (Next != null)
                    Next.Previous = this;
            }
        }

        public LinkedList()
        {
        }

        private void Add(T value, Node node, bool addAfter)
        {
            if (node == null)
            {
                var newNode = new Node(value, null, null);

                _head = newNode;
                _tail = newNode;
            }
            else
            {
                var previous = addAfter ? node : node.Previous;
                var next = addAfter ? node.Next : node;

                var newNode = new Node(value, previous, next);

                if (previous == _tail)
                    _tail = newNode;

                if (next == _head)
                    _head = newNode;
            }
        }

        public void Add(T value, IComparer<T> cmp)
        {
            if (_head == null)
            {
                AddHead(value);
                return;
            }

            var current = _head;
            while (current != null && cmp.Compare(current.Value, value) > 0)
            {
                current = current.Next;
            }

            if (current == null)
                AddTail(value);
            else
                Add(value, current, false);
        }

        public void AddTail(T value)
        {
            Add(value, _tail, true);
        }

        public void AddHead(T value)
        {
            Add(value, _head, false);
        }

        public void Remove(Enumerator enumerator)
        {
            if (enumerator == null || enumerator.Node == null)
                return;

            var node = enumerator.Node;

            if (node == _head)
                _head = node.Next;

            if (node == _tail)
                _tail = node.Previous;

            if (node.Previous != null)
                node.Previous.Next = node.Next;

            if (node.Next != null)
                node.Next.Previous = node.Previous;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_head, true);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return (IEnumerator<T>)GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public class Enumerator : IEnumerator<T>
        {
            internal Node Node;
            public bool Forward;
            private bool isFirst = true;

            internal Enumerator(Node node, bool forward)
            {
                Node = node;
                Forward = forward;
            }

            public T Current
            {
                get { return Node.Value; }
            }

            public void Dispose() { }

            object IEnumerator.Current
            {
                get { return (object)Node.Value; }
            }

            public bool MoveNext()
            {
                if (Node == null)
                    return false;

                if (isFirst)
                {
                    isFirst = false;
                    return true;
                }

                if (Forward)
                    Node = Node.Next;
                else
                    Node = Node.Previous;

                return Node != null;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}