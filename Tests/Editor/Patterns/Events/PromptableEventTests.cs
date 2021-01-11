using NUnit.Framework;
using HexCS.Core;

namespace HexCSTests.Core
{
    [TestFixture]
    public class PromptableEventTests
    {
        [Test]
        public void Works()
        {
            // Arrange
            PromptableEvent ev = new PromptableEvent();
            PromptableEvent<int> evi = new PromptableEvent<int>();

            // Act
            int evRes = 0;
            ev.Subscribe( () => evRes++ );

            ev.Invoke();
            bool t1 = evRes == 0;

            ev.Prompt();
            bool t2 = evRes == 1;

            ev.Invoke();
            bool t3 = evRes == 1;

            int eviRes = 0;
            evi.Subscribe((i) => eviRes += i);

            evi.Invoke(7);
            bool ti1 = eviRes == 0;

            evi.Invoke(2);
            bool ti2 = eviRes == 0;

            evi.Prompt();
            bool ti3 = eviRes == 2;

            // Assert
            Assert.That(t1 && t2 && t3);
            Assert.That(ti1 && ti2 && ti3);
        }
    }
}