namespace Needle.Tests.Mocks
{
    using Attributes;

    public class MockWithMultipleInjectionConstructors
    {
        [Constructor]
        public MockWithMultipleInjectionConstructors()
        {
        }

        [Constructor]
        public MockWithMultipleInjectionConstructors(int num)
        {
        }
    }
}
