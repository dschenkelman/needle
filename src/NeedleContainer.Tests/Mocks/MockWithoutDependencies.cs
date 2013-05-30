namespace Needle.Tests.Mocks
{
    using Attributes;

    internal class MocksWithoutDependencies 
    {
        internal class MockWithoutDependencies : IMockWithoutDependencies
        {
            [Constructor]
            public MockWithoutDependencies()
            {
                this.Constructed = true;
            }

            public bool Constructed
            {
                get;
                private set;
            }
        }

        internal class MockWithoutDependenciesDefaultConstructor : IMockWithoutDependencies
        {
            public bool Constructed
            {
                get { return true; }
            }
        }
    }
}
