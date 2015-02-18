using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectToQueryBuilder.Test
{
    [TestClass]
    public class SampleModelRepositoryTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var repository = new SampleModelRepository();
            var items = repository.GetItems(x => x.Name == "TestName" && x.Description == "Description to test" || x.Count == 0);

            Assert.IsNotNull(items);
        }
    }
}
