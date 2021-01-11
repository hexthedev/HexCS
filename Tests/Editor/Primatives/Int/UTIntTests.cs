using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    class UTIntTests
    {
        [Test]
        public void Works()
        {
            // Arrange
            int int1 = 3;
            int int2 = -3;

            // Act
            int test1 = int1.CircularMod(2);
            int test2 = int1.CircularMod(4);
            int test3 = int2.CircularMod(2);

            // Assert
            Assert.That(test1 == 1);
            Assert.That(test2 == 3);
            Assert.That(test3 == 1);
        }
    }
}
