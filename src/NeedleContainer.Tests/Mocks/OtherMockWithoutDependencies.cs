namespace Needle.Tests.Mocks
{
    using Attributes;

    public class OtherMockWithoutDependencies : IMockWithoutDependencies
    {
        private readonly bool constructed;
        
        [Constructor]
        public OtherMockWithoutDependencies()
        {
            this.constructed = true;
        }

        public bool Constructed
        {
            get { return this.constructed; }
        }
    }
}
