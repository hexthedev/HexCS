using System.Text;
using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class UTStringBuilderTests
    {
        [Test]
        public void Works()
        {
            // Arrange
            StringBuilder sb = new StringBuilder();
            int[] x = { 1, 2, 3 };

            // Act
            sb.AppendCharacterSeparatedCollection(
                x, (s, el) => s.Append(el), ", " 
            );

            // Assert
            Assert.That(sb.ToString() == "1, 2, 3");
        }
    }
}