using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    class ByteArrayExtractorTests
    {
        [Test]
        public void Works()
        {
            // Arrange
            byte[] testArray;

            using (ArrayBuilder<byte> con = new ArrayBuilder<byte>())
            {
                con.AppendToArray(BitConverter.GetBytes((uint)1) );
                con.AppendToArray(BitConverter.GetBytes(1));
                con.AppendToArray(BitConverter.GetBytes(true));
                con.AppendToArray(BitConverter.GetBytes('a'));
                con.AppendToArray(BitConverter.GetBytes(1.1f));
                con.AppendToArray(BitConverter.GetBytes(1.1d));
                con.AppendToArray(Encoding.UTF8.GetBytes("aaa"));
                con.AppendToArray(Encoding.UTF32.GetBytes("aaa"));
                con.AppendToArray(new byte[] { 1, 1, 1 });

                testArray = con.ToArray();
            }

            // Act
            using (ByteArrayExtractor ex = new ByteArrayExtractor(testArray))
            {
                uint test1 = ex.ExtractUInt();
                int test2 = ex.ExtractInt();
                bool test3 = ex.ExtractBool();
                char test4 = ex.ExtractChar();
                float test5 = ex.ExtractFloat();
                double test6 = ex.ExtractDouble();
                string test7s1 = ex.ExtractString(3, Encoding.UTF8);
                string test7s2 = ex.ExtractString(3, Encoding.UTF32);
                byte[] test8 = ex.ExtractRemaining();

                // Assert
                Assert.That(test1 == 1);
                Assert.That(test2 == 1);
                Assert.That(test3 == true);
                Assert.That(test4 == 'a');
                Assert.That(test5 == 1.1f);
                Assert.That(test6 == 1.1);
                Assert.That(test7s1 == "aaa");
                Assert.That(test7s2 == "aaa");

                Assert.That(test8.Length == 3);
                
                foreach(byte b in test8)
                {
                    Assert.That(b == 1);
                }
            }
        }
    }
}
