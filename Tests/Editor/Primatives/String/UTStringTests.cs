using System.Text;
using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class UTStringTests
    {
        [Test]
        public void EnforceFistCharCaptialOnlyTest()
        {
            // Arrange
            string test1 = "ABCD";
            string test2 = "abcd";

            // Act
            string res1 = test1.EnforceFistCharCaptialOnly();
            string res2 = test2.EnforceFistCharCaptialOnly();

            // Assert
            Assert.That(res1 == "Abcd");
            Assert.That(res2 == "Abcd");
        }

        [Test]
        public void EnforceFistCharCaptial()
        {
            // Arrange
            string test1 = "ABCD";
            string test2 = "abcd";

            // Act
            string res1 = test1.EnforceFistCharCaptial();
            string res2 = test2.EnforceFistCharCaptial();

            // Assert
            Assert.That(res1 == "ABCD");
            Assert.That(res2 == "Abcd");
        }

        [Test]
        public void EnforceFistCharLowerCaseOnly()
        {
            // Arrange
            string test1 = "ABCD";
            string test2 = "abcd";

            // Act
            string res1 = test1.EnforceFistCharLowerCaseOnly();
            string res2 = test2.EnforceFistCharLowerCaseOnly();

            // Assert
            Assert.That(res1 == "aBCD");
            Assert.That(res2 == "aBCD");
        }

        [Test]
        public void EnforceFirstCharLowerCase()
        {
            // Arrange
            string test1 = "ABCD";
            string test2 = "abcd";

            // Act
            string res1 = test1.EnforceFirstCharLowerCase();
            string res2 = test2.EnforceFirstCharLowerCase();

            // Assert
            Assert.That(res1 == "aBCD");
            Assert.That(res2 == "abcd");
        }

        [Test]
        public void EnforceSnakeCase()
        {
            // Arrange
            string[] test = new string[] { "This", "IS", "a", "test" };

            // Act
            string res = UTString.EnforceSnakeCase(test);

            // Assert
            Assert.That(res == "this_is_a_test");
        }

        [Test]
        public void EnforceCamelCase()
        {
            // Arrange
            string[] test = new string[] { "This", "IS", "a", "test" };

            // Act
            string res1 = UTString.EnforceCamelCase(true, test);
            string res2 = UTString.EnforceCamelCase(false, test);

            // Assert
            Assert.That(res1 == "ThisIsATest");
            Assert.That(res2 == "thisIsATest");
        }

        [Test]
        public void RepeatedCharacter()
        {
            // Arrange
            string test = "a";

            // Act
            string res = UTString.RepeatedCharacter(test, 5);

            // Assert
            Assert.That(res == "aaaaa");
        }

        [Test]
        public void SeparatorLevelPrune()
        {
            // Arrange
            string test = "a.b.c.d";

            // Act
            string t1 = test.SeparatorLevelPrune('.', -1);
            string t2 = test.SeparatorLevelPrune('.', 5);
            string t3 = test.SeparatorLevelPrune('.', 2);
            
            // Assert
            Assert.That(t1 == test);
            Assert.That(t2 == test);
            Assert.That(t3 == "a.b");
        }
    }
}