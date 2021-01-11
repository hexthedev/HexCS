using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class UTArrayTests
    {
        [Test]
        public void Combine()
        {
            //Arrange
            int[] first = { 1, 2 };
            int[] second = { 3, 4 };

            // Act
            int[] result = UTArray.Combine(first, second);

            // Assert
            int index = 1;

            foreach(int x in result)
            {
                Assert.That(x == index++);
            }
        }

        [Test]
        public void EqualsElementWise()
        {
            // Arrange
            int[] x = { 1, 2, 3 };
            int[] y = { 1, 2, 3 };
            int[] z = { 3, 2, 1 };

            // Assert
            Assert.That( x.EqualsElementWise(y) );
            Assert.That( !y.EqualsElementWise(z) );
        }

        [Test]
        public void SquareSideLength()
        {
            // Arrange
            int[] x = { 1, 2 };
            int[] y = { 1, 2, 3, 4, 5};
            int[] z = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] w = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Assert
            Assert.That(x.SquareSideLength() == 2);
            Assert.That(y.SquareSideLength() == 3);
            Assert.That(z.SquareSideLength() == 3);
            Assert.That(w.SquareSideLength() == 4);
        }

        [Test]
        public void ShallowCopy()
        {
            // Arrange
            int[] x = { 1, 2, 3 };

            // Act
            int[] test = x.ShallowCopy();

            // Assert
            int index = 1;

            foreach(int i in test)
            {
                Assert.That(i == index++);
            }
        }

        [Test]
        public void RandomElement()
        {
            // Arrange
            int[] x = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Act
            int attempt = 0;

            while( attempt++ < 10)
            {
                int test = x.RandomElement();

                //Assert
                Assert.That(test > 0 && test <= 10);
            }
        }

        [Test]
        public void ConstructArray()
        {
            // Act
            int[] test = UTArray.ConstructArray(10, () => 1);

            // Assert
            foreach(int el in test)
            {
                Assert.That(el == 1);
            }
        }

        [Test]
        public void ConstructArrayIndex()
        {
            // Act
            int[] test = UTArray.ConstructArray(10, (i) => i * 2);

            // Assert
            for (int i = 0; i < test.Length; i++)
            {
                Assert.That(test[i] == i * 2);
            }
        }

        [Test]
        public void ConstructArrayIndexAndArray()
        {
            // Act
            int[] test = UTArray.ConstructArray(10, 
                (int i, int[] a) =>
                {
                    return i == 0 ? 0 : a[i - 1] + 1;
                }
            );

            // Assert
            for(int i = 0; i< test.Length; i++)
            {
                Assert.That(test[i] == i);
            }
        }

        [Test]
        public void SubArray()
        {
            // Arrange
            int[] array = { 0,1,2,3,4,5,6,7,8,9,10 };

            // Act
            int[] subarray = array.SubArray(1, 10);

            // Assert
            int index = 1;

            while(index <= 10)
            {
                Assert.That(array[index] == index++);
            }
        }

        [Test]
        public void AsSimpleString()
        {
            // Arrange
            int[] array = { 0, 1, 2 };

            // Act
            string simple = array.AsSimpleString();

            // Assert
            Assert.That(String.Equals(simple, "0 1 2"));
        }
    }
}
