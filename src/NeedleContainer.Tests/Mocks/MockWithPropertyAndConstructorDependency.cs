namespace Needle.Tests.Mocks
{
    using Attributes;

    public class MockWithPropertyAndConstructorDependency
    {
        [Constructor]
        public MockWithPropertyAndConstructorDependency(MockDependency dependency)
        {
            this.ConstructorDependency = dependency;
        }

        [Dependency]
        public MockDependency PropertyDependency { get; set; }

        public MockDependency ConstructorDependency { get; private set; }
    }
}
