namespace Needle.Tests.Builder
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Needle.Builder;
    using Needle.Builder.Strategies;
    using Needle.Container;

    [TestClass]
    public class ConstructorDependenciesStrategyFixture
    {
        private ConstructorDependenciesStrategy strategy;

        [TestInitialize]
        public void Initialize()
        {
            this.strategy = new ConstructorDependenciesStrategy();
        }

        [TestMethod]
        public void DependenciesStrategyStepIsDependenciesResolution()
        {
            Assert.AreEqual(BuildingStep.ConstructorDependenciesResolution, this.strategy.BuildingStep);
        }

        [TestMethod]
        public void StrategyOrderIsCorrectInComparisonToOtherStrategies()
        {
            var mockConstructorStrategy = new Mock<IBuilderStrategy>();
            var mockPreBuildStrategy = new Mock<IBuilderStrategy>();

            mockPreBuildStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.PreBuilding);
            mockConstructorStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.ConstructorDetermination);

            Assert.IsTrue(this.strategy.CompareTo(mockConstructorStrategy.Object) > 0);
            Assert.IsTrue(this.strategy.CompareTo(mockPreBuildStrategy.Object) > 0);
        }

        [TestMethod]
        public void DoesNothingIfBuildWasCompletedPriorToExecution()
        {
            var mockContainer = new Mock<INeedleContainer>();
            var mockBuildStatus = new Mock<IBuildStatus>();
            mockBuildStatus.SetupGet<bool>(status => status.BuildCompleted).Returns(true);
            IBuildStatus buildStatus = mockBuildStatus.Object;
            INeedleContainer container = mockContainer.Object;

            // note that there is no type provided, so if it did something it would fail
            this.strategy.ExecuteStrategy(buildStatus, container);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsIfExecutingWithNullBuildStatus()
        {
            this.strategy.ExecuteStrategy(null, new Mock<INeedleContainer>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CallingCompareToWithNullParameterThrows()
        {
            new ConstructorDependenciesStrategy().CompareTo(null);
        }
    }
}
