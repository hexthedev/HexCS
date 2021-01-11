using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class UTArray2DTests
    {
        [Test]
        public void Construct()
        {
            // Act
            int[,] result = UTArray2D.ConstructArray( new DiscreteVector2(2, 2), () => 1 );

            // Assert
            Assert.That(result.GetLength(0) == 2);
            Assert.That(result.GetLength(1) == 2);

            foreach (int x in result)
            {
                Assert.That(x == 1);
            }
        }
    }
}
