using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class UTListTests
    {
        [Test]
        public void ShallowCopy()
        {
            // Arrange
            List<int> list = new List<int>(new int[] { 1, 2, 3 });

            // Act
            List<int> copy = list.ShallowCopy();

            // Assert;
            for(int i = 0; i<list.Count; i++)
            {
                Assert.That(list[i] == copy[i]);
            }
        }

        [Test]
        public void GetAtCircular()
        {
            // Arrange
            List<int> list = new List<int>(new int[] { 0, 1, 2 });

            // Act
            int test1 = list.GetAtIndexCircular(0); // 0
            int test2 = list.GetAtIndexCircular(3); // 0
            int test3 = list.GetAtIndexCircular(4); // 1
            int test4 = list.GetAtIndexCircular(8); // 2

            // Assert
            Assert.That(test1 == 0);
            Assert.That(test2 == 0);
            Assert.That(test3 == 1);
            Assert.That(test4 == 2);
        }

        [Test]
        public void Difference()
        {
            // Arrange
            List<int> list1 = new List<int>(new int[] { 0, 1, 2 });
            List<int> list2 = new List<int>(new int[] { 1, 2, 3 });

            // Act
            List<int> test1 = list1.Difference(list2);
            List<int> test2 = list2.Difference(list1);

            // Assert
            Assert.That(test1.Count == 1);
            Assert.That(test1[0] == 0);

            Assert.That(test2.Count == 1);
            Assert.That(test2[0] == 3);
        }
    }
}
