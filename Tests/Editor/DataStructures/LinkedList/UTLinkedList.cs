using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class UTLinkedList
    {
        [Test]
        public void StepTowardsHead()
        {
            // Arrange
            LinkedList<int> list = TestList();

            int[] expectedOutcome = { 0, 1, 2, 3, 4, 5, 6, 9, 7, 8 };

            // Act
            int steps1 = list.StepTowardsHead(9, 2); // move 9 2 forward
            int steps2 = list.StepTowardsHead(0, 5); // move 0 0 forward
            int steps3 = list.StepTowardsHead(100, 5); // return -1 cause 100 dosen't exist

            // Assert
            Assert.That(steps1 == 2);
            Assert.That(steps2 == 0);
            Assert.That(steps3 == -1);

            LinkedListNode<int> node = list.First;

            for(int i = 0; i<expectedOutcome.Length; i++)
            {
                if (i != 0) node = node.Next;

                Assert.That(node.Value == expectedOutcome[i]);
            }
        }

        [Test]
        public void StepTowardsTail()
        {
            // Arrange
            LinkedList<int> list = TestList();

            int[] expectedOutcome = { 1, 2, 0, 3, 4, 5, 6, 7, 8, 9 };

            // Act
            int steps1 = list.StepTowardsTail(0, 2); // move 0 2 back
            int steps2 = list.StepTowardsTail(9, 5); // move 9 0 back
            int steps3 = list.StepTowardsTail(100, 5); // return -1 cause 100 dosen't exist

            // Assert
            Assert.That(steps1 == 2);
            Assert.That(steps2 == 0);
            Assert.That(steps3 == -1);

            LinkedListNode<int> node = list.First;

            for (int i = 0; i < expectedOutcome.Length; i++)
            {
                if (i != 0) node = node.Next;

                Assert.That(node.Value == expectedOutcome[i]);
            }
        }

        [Test]
        public void JumpToHead()
        {
            // Arrange
            LinkedList<int> list = TestList();

            // Act
            bool test1 = list.JumpToHead(5); // 5 is now head
            bool test2 = list.JumpToHead(100); // 100 dosen't exist

            // Assert
            Assert.That(test1);
            Assert.That(list.First.Value == 5);
            Assert.That(!test2);
        }

        [Test]
        public void JumpToTail()
        {
            // Arrange
            LinkedList<int> list = TestList();

            // Act
            bool test1 = list.JumpToTail(5); // 5 is now tail
            bool test2 = list.JumpToTail(100); // 100 dosen't exist

            // Assert
            Assert.That(test1);
            Assert.That(list.Last.Value == 5);
            Assert.That(!test2);
        }

        /// <summary>
        /// Returs a linked list containing int values 0-9 in order
        /// </summary>
        /// <returns></returns>
        private LinkedList<int> TestList()
        {
            LinkedList<int> list = new LinkedList<int>();

            for(int i = 0; i<10; i++)
            {
                list.AddLast(i);
            }

            return list;
        }
    }
}
