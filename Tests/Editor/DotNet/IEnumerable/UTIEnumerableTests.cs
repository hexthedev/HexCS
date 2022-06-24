using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class UTIEnumerableTests
    {
        private readonly int[] prop1 = new int[] { 1, 2, 3 };
        private readonly int[] prop2 = new int[] { 2, 3, 4 };

        [Test]
        public void Merge()
        {
            // Act
            int[] test = UTIEnumerable.Combine(prop1, prop2).ToArray();

            // Assert
            Assert.That(UTArray.EqualsElementWise(test, new int[] { 1, 2, 3, 2, 3, 4 }));
        }

        [Test]
        public void UniqueMerge()
        {
            // Act
            int[] test = UTIEnumerable.UniqueMerge(prop1, prop2).ToArray();

            // Assert
            Assert.That(UTArray.EqualsElementWise(test, new int[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void Difference()
        {
            // Act
            int[] test = UTIEnumerable.Difference(prop1, prop2).ToArray();

            // Assert
            Assert.That(UTArray.EqualsElementWise(test, new int[] { 1 }));
        }

        [Test]
        public void QueryContains()
        {
            // Act
            bool test = prop1.QueryContains(i => i == 2 || i == 3);
            bool test2 = prop1.QueryContains(i => i == 4);

            // Assert
            Assert.That(test == true);
            Assert.That(test2 == false);
        }

        [Test]
        public void QueryCount()
        {
            // Act
            int test = prop1.QueryCount(i => i == 2 || i == 3);
            int test2 = prop1.QueryCount(i => i == 4);

            // Assert
            Assert.That(test == 2);
            Assert.That(test2 == 0);
        }

        [Test]
        public void QueryIndexOf()
        {
            // Act
            int test = prop1.QueryIndexOf(i => i == 2 || i == 3);
            int test2 = prop1.QueryIndexOf(i => i == 4);

            // Assert
            Assert.That(test == 1);
            Assert.That(test2 == -1);
        }

        [Test]
        public void CastedEnumerable()
        {
            // Arrange
            IList a1 = prop1 as IList;
            IList a2 = prop1 as IList;

            IList[] a3 = new IList[] { a1, a2 };

            // Act
            IEnumerable<int[]> test = a3.Select( a => a as int[] );

            // Assert
            foreach(int[] x in test)
            {
                Assert.That(x == prop1);
            }
        }
    }
}
