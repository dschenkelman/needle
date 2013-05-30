namespace Needle.Tests.Mocks
{
    using System;
    using System.Collections.Generic;

    using Attributes;

    using MockWithoutDependencies = Needle.Tests.Mocks.MocksWithoutDependencies.MockWithoutDependencies;

    internal class MockWithPropertyDependencies
    {
        internal class MockWithPropertyDependency
        {
            [Dependency]
            public MockWithoutDependencies Dependency { get; set; }
        }

        internal class MockWithNamedPropertyDependency
        {
            [Dependency("DependencyId")]
            public MockWithoutDependencies Dependency { get; set; }
        }

        internal class MockWithMultiplePropertyDependencies
        {
            [Dependency]
            public MockWithoutDependencies FirstDependency { get; set; }

            [Dependency]
            public MockWithObjectDependency SecondDependency { get; set; }
        }

        internal class MockWithPropertyFactoryDependency
        {
            [Dependency]
            public Func<MockDependency> PropertyDependency { get; set; }
        }

        internal class MockWithPropertyGetAllDependency
        {
            [Dependency]
            public IEnumerable<MockDependency> PropertyDependency { get; set; }
        }
    }
}