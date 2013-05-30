namespace Needle.Tests.Builder
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Needle.Builder;
    using Needle.Builder.Strategies;
    using Needle.Container;

    [TestClass]
    public class ConstructorStrategyFixture
    {
        private ConstructorStrategy strategy;

        [TestInitialize]
        public void Initialize() 
        {
            this.strategy = new ConstructorStrategy();   
        }

        [TestMethod]
        public void ConstructorStrategyStepIsConstructorDetermination() 
        {
            Assert.AreEqual(BuildingStep.ConstructorDetermination, this.strategy.BuildingStep);
        }

        [TestMethod]
        public void StrategyOrderIsCorrectInComparisonToOtherStrategies() 
        {
            var mockDependenciesStrategy = new Mock<IBuilderStrategy>();
            var mockPreBuildStrategy = new Mock<IBuilderStrategy>();

            mockPreBuildStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.PreBuilding);
            mockDependenciesStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.ConstructorDependenciesResolution);

            Assert.IsTrue(this.strategy.CompareTo(mockDependenciesStrategy.Object) < 0);
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
            new ConstructorStrategy().CompareTo(null);
        }
    }
}
