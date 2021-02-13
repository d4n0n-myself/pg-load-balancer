using NUnit.Framework;

namespace LoadBalancer.Tests.Definer
{
    public class OlapTests
    {
        // [SetUp]
        // public void Setup()
        // {
        // }

        [Test]
        public void SelectWith10MillionRowsIsOltp()
        {
            Assert.Pass();
        }

        [Test]
        public void SelectWithCteIsOlap()
        {
            Assert.Pass();
        }

        [Test]
        public void SelectWithSubqueryIsOlap()
        {
            Assert.Pass();
        }

        [Test]
        public void SelectWithComplexTypeIsOlap()
        {
            Assert.Pass();
        }

        [Test]
        public void SelectWithHighCostIsOlap()
        {
            // definition of high cost !?
            Assert.Pass();
        }

        [Test]
        public void SelectWithLongProcessingIsOlap()
        {
            Assert.Pass();
        }
    }
}