using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    class UTFloatTest
    {
        [Test]
        public void Works()
        {
            // Arrange
            float f = 7f;

            // Act
            float test1 = f.Clamp(1, 10);
            float test2 = f.Clamp(10, 20);
            float test3 = f.Clamp(-10, 0);

            // Assert
            Assert.That(test1 == 7);
            Assert.That(test2 == 10);
            Assert.That(test3 == 0);
        }
    }
}
