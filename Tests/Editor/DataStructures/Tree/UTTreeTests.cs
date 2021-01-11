using System;
using System.Collections.Generic;
using System.Text;
using HexCS.Core;

namespace HexCSTests.Core.DataStructures
{
    public static class UTTreeTests
    {
        /// <summary>
        /// Create a basic complete binary tree with an ITreeMode type
        /// </summary>
        /// <param name="elementsAfterFirst"></param>
        /// <returns></returns>
        public static ITreeNode<int> CreateBasicBinaryTree<T>(int elementsAfterFirst)
            where T : ITreeNode<int>, new()
        {
            // Create binary tree 0 to 10
            ITreeNode<int> t = new T();
            t.SetValue(0);

            Queue<ITreeNode<int>> nodes = new Queue<ITreeNode<int>>();

            Queue<int> num_stack = new Queue<int>();

            for (int i = 1; i <= elementsAfterFirst; i++)
            {
                num_stack.Enqueue(i);
            }

            nodes.Enqueue(t.Root);

            while (nodes.Count > 0)
            {
                ITreeNode<int> node = nodes.Dequeue();

                if (num_stack.Count == 0)
                {
                    break;
                }

                nodes.Enqueue(node.AddChild(num_stack.Dequeue()));

                if (num_stack.Count == 0)
                {
                    break;
                }

                nodes.Enqueue(node.AddChild(num_stack.Dequeue()));
            }

            return t.Root;
        }
    }
}
