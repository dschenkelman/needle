namespace Needle.Tests.Mocks
{
    using Attributes;

    internal class MockWithThreeDependencies
    {
        [Constructor]
        public MockWithThreeDependencies(MockDependency dep1, MockDependency dep2, MockDependency dep3)
        {
            this.Dependency1 = dep1;
            this.Dependency2 = dep2;
            this.Dependency3 = dep3;
        }

        public MockDependency Dependency1 { get; set; }

        public MockDependency Dependency2 { get; set; }

        public MockDependency Dependency3 { get; set; }
    }
}
