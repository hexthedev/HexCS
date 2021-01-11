using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class DiscreteVector2Tests
    {
        [Test]
        public void Works()
        {
            // Arrange
            DiscreteVector2 vec1 = new DiscreteVector2(1, 1);
            DiscreteVector2 vec2 = new DiscreteVector2(-1, 2);

            // Act
            DiscreteVector2 add = vec1 + vec2;
            DiscreteVector2 sub = vec2 - vec1;

            // Assert
            Assert.That(add == new DiscreteVector2(0, 3));
            Assert.That(sub == new DiscreteVector2(-2, 1));
        }
    }
}
