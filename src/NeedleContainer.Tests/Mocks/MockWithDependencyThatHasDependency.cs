namespace Needle.Tests.Mocks
{
    using Attributes;

    public class MockWithDependencyThatHasDependency
    {
        [Constructor]
        public MockWithDependencyThatHasDependency(MockWithObjectDependency dependency)
        {
            this.Dependency = dependency;
        }

        public MockWithObjectDependency Dependency
        {
            get; private set;
        }
    }
}
