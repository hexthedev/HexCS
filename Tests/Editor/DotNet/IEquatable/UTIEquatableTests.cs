using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class UTIEquatableTests
    {
        private readonly int[] prop1 = new int[] { 1, 2, 3 };
        private readonly int[] prop2 = new int[] { 2, 3, 1 };
        private readonly int[] prop3 = new int[] { 3, 2, 4 };

        [Test]
        public void AreEqualUnorderedCollections()
        {
            // Act
            bool test1 = UTIEquatable.AreEqualUnorderedCollections<int>(prop1, prop2);
            bool test2 = UTIEquatable.AreEqualUnorderedCollections<int>(prop1, prop3);

            // Assert
            Assert.That(test1);
            Assert.That(!test2);
        }

        [Test]
        public void SharedValuesCount()
        {
            // Act
            int test1 = UTIEquatable.SharedValuesCount(prop1, prop2);
            int test2 = UTIEquatable.SharedValuesCount(prop1, prop3);

            // Assert
            Assert.That(test1 == 3);
            Assert.That(test2 == 2);
        }
    }
}
