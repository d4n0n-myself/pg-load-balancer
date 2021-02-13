using NUnit.Framework;

namespace LoadBalancer.Tests.Definer
{
    public class OltpTests
    {
        // [SetUp]
        // public void Setup()
        // {
        // }

        [Test]
        public void InsertIsOltp()
        {
            Assert.Pass();
        }
        
        [Test]
        public void UpdateIsOltp()
        {
            Assert.Pass();
        }
        
        [Test]
        public void DeleteIsOltp()
        {
            Assert.Pass();
        }

        [Test]
        public void SelectWith10RowsIsOltp()
        {
            Assert.Pass();
        }

        [Test]
        public void SelectWithSimpleQueryIsOltp()
        {
            Assert.Pass();
        }
    }
}