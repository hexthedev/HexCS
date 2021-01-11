using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class ArrayConstructorTests
    {
        [Test]
        public void Works()
        {
            // Arrange
            int[] testArray;

            // Act
            using (ArrayBuilder<int> constructor = new ArrayBuilder<int>())
            {
                for (int i = 0; i<10; i++)
                {
                    constructor.AppendToArray(i);
                }

                constructor.AppendToArray( new int[] { 10, 11, 12 } );

                testArray = constructor.ToArray();
            }

            // Assert
            int index = 0;

            for (; index < testArray.Length; index++)
            {
                Assert.That(testArray[index] == index);
            }
        }
    }
}
