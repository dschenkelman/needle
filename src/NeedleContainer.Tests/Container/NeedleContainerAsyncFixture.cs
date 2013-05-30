namespace Needle.Tests.Container
{
    using System.Threading;
    using System.Threading.Tasks;
    
    using Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Needle.Container;

    [TestClass]
    public class NeedleContainerAsyncFixture
    {
        [TestMethod]
        public async Task ShouldBeAbleToRetrieveTypeAsynchronously() 
        {
            var container = new NeedleContainer();
            var dependency = await container.GetAsync<MockDependency>();

            Assert.IsTrue(dependency.Constructed);

            dependency = (MockDependency)await container.GetAsync(typeof(MockDependency));

            Assert.IsTrue(dependency.Constructed);
        }
    }
}
