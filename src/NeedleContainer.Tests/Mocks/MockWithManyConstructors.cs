namespace Needle.Tests.Mocks
{
    using Attributes;

    public class MockWithManyConstructors
    {
        public MockWithManyConstructors(string parameter)
        {
        }
        
        [Constructor]
        public MockWithManyConstructors()
        {
            this.AttributedConstructorCalled = true;
        }

        public bool AttributedConstructorCalled
        {
            get; set;
        }
    }
}
