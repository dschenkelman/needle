namespace Needle.Tests.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Mocks;

    using Moq;

    using Needle.Builder;
    using Needle.Builder.Strategies;
    using Needle.Container;

    using MockWithoutDependencies = Needle.Tests.Mocks.MocksWithoutDependencies.MockWithoutDependencies;

    [TestClass]
    public class GetAllStrategyFixture
    {
        private GetAllStrategy strategy;

        [TestInitialize]
        public void Initialize()
        {
            this.strategy = new GetAllStrategy();
        }

        [TestMethod]
        public void GetAllStrategyStepIPreBuilding()
        {
            Assert.AreEqual(BuildingStep.PreBuilding, this.strategy.BuildingStep);
        }

        [TestMethod]
        public void StrategyOrderIsCorrectInComparisonToOtherStrategies()
        {
            var mockDependenciesStrategy = new Mock<IBuilderStrategy>();
            var mockConstructorStrategy = new Mock<IBuilderStrategy>();
            mockConstructorStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.ConstructorDetermination);
            mockDependenciesStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.ConstructorDependenciesResolution);

            Assert.IsTrue(this.strategy.CompareTo(mockDependenciesStrategy.Object) < 0);
            Assert.IsTrue(this.strategy.CompareTo(mockConstructorStrategy.Object) < 0);
        }

        [TestMethod]
        public void MarksBuildStatusAsBuildCompleted()
        {
            var mockContainer = new Mock<INeedleContainer>();
            var mockBuildStatus = new Mock<IBuildStatus>();
            mockBuildStatus.SetupProperty<bool>(status => status.BuildCompleted);
            mockBuildStatus.SetupGet<Type>(status => status.TypeToBuild).Returns(typeof(IEnumerable<MockWithoutDependencies>));
            IBuildStatus buildStatus = mockBuildStatus.Object;
            INeedleContainer container = mockContainer.Object;

            this.strategy.ExecuteStrategy(buildStatus, container);

            Assert.IsTrue(buildStatus.BuildCompleted);
        }

        [TestMethod]
        public void DoesNothingIfBuildWasCompletedPriorToExecution()
        {
            var mockContainer = new Mock<INeedleContainer>();
            var mockBuildStatus = new Mock<IBuildStatus>();
            mockBuildStatus.SetupGet<bool>(status => status.BuildCompleted).Returns(true);
            IBuildStatus buildStatus = mockBuildStatus.Object;
            INeedleContainer container = mockContainer.Object;

            // note that there is no type provided for the build status, so if it did something it would fail
            this.strategy.ExecuteStrategy(buildStatus, container);
        }

        [TestMethod]
        public void StrategySetsBuildStatusWithDelegateToFuncOfTypeDesired()
        {
            var mockBuildStatus = new Mock<IBuildStatus>();
            mockBuildStatus.SetupGet<Type>(status => status.TypeToBuild).Returns(typeof(IEnumerable<MockWithoutDependencies>));
            mockBuildStatus.SetupProperty<Factory<object>>(status => status.FactoryMethod);
            var mockContainer = new Mock<INeedleContainer>();
            mockContainer.Setup(c => c.GetAll<MockWithoutDependencies>()).Returns(new MockWithoutDependencies[] { new MockWithoutDependencies(), new MockWithoutDependencies() });

            var buildStatus = mockBuildStatus.Object;

            this.strategy.ExecuteStrategy(mockBuildStatus.Object, mockContainer.Object);

            object obtainedObject = buildStatus.FactoryMethod.Invoke();
            Assert.IsInstanceOfType(obtainedObject, typeof(IEnumerable<MockWithoutDependencies>));
            Assert.AreEqual(2, ((IEnumerable<MockWithoutDependencies>)obtainedObject).Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CallingCompareToWithNullParameterThrows()
        {
            new GetAllStrategy().CompareTo(null);
        }
    }
}
