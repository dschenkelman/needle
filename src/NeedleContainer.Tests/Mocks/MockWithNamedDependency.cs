namespace Needle.Tests.Mocks
{
    using Attributes;

    internal class MockWithNamedDependency
    {
        [Constructor]
        public MockWithNamedDependency([Dependency("Mock")] MockDependency dependency)
        {
            this.Dependency = dependency;
        }

        public MockDependency Dependency
        {
            get; private set;
        }
    }
}
