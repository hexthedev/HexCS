using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class EnumDictionaryTests
    {
        [Test]
        public void Works()
        {
            // Arrange
            EnumDicitonary<ETest, int> test = new EnumDicitonary<ETest, int>(1);

            // Act
            bool allInitalizedTo1 = true;

            foreach(ETest t in UTEnum.GetEnumAsArray<ETest>())
            {
                if (test[t] != 1) allInitalizedTo1 = false;
            }

            test[ETest.BANANA] = 3;
            test[ETest.CHERRY] = 4;

            bool setsWorks = test[ETest.STRAWBERRY] == 1 && test[ETest.BANANA] == 3 && test[ETest.CHERRY] == 4;

            // Assert
            Assert.That(allInitalizedTo1 && setsWorks);
        }

        private enum ETest
        {
            BANANA = 0,
            STRAWBERRY = 1,
            CHERRY = 3
        }
    }
}