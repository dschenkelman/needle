namespace Needle.Tests.Mocks
{
    using Attributes;

    public class MockWithObjectDependency
    {
        [Constructor]
        public MockWithObjectDependency(MockDependency dependency)
        {
            this.Dependency = dependency;
        }

        public MockDependency Dependency
        {
            get; private set;
        }
    }
}
