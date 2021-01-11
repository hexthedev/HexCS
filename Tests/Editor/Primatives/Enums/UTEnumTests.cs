using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class UTEnumTests
    {
        private enum TestEnum{
            TEST1 = 100,
            TEST2 = 300,
            TEST3 = 200
        }

        [Test]
        public void GetEnumAsArray()
        {
            // Act
            TestEnum[] testArray = UTEnum.GetEnumAsArray<TestEnum>();

            // Assert
            TestEnum[] answer = new TestEnum[] { TestEnum.TEST1, TestEnum.TEST3, TestEnum.TEST2 };

            for(int i = 0; i<testArray.Length; i++)
            {
                Assert.That(testArray[i] == answer[i]);
            }
        }

        [Test]
        public void GetNext()
        {
            // Act
            TestEnum test1 = TestEnum.TEST1.GetNext();
            TestEnum test2 = TestEnum.TEST2.GetNext();
            TestEnum test3 = TestEnum.TEST3.GetNext();

            // Assert
            Assert.That(test1 == TestEnum.TEST3);
            Assert.That(test2 == TestEnum.TEST1);
            Assert.That(test3 == TestEnum.TEST2);
        }
    }
}
