namespace Needle.Tests.Mocks
{
    using Attributes;

    public class MockDependency
    {
        [Constructor]
        public MockDependency()
        {
            this.Constructed = true;
        }

        public bool Constructed
        {
            get; private set;
        }
    }
}
