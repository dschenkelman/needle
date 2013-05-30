namespace Needle.Tests.Builder
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Needle.Builder;
    using Needle.Builder.Strategies;
    using Needle.Container;

    [TestClass]
    public class PreBuildCompositeBuilderStrategyFixture
    {
        private PreBuildCompositeBuilderStrategy strategy;

        [TestInitialize]
        public void Initialize()
        {
            this.strategy = new PreBuildCompositeBuilderStrategy();
        }

        [TestMethod]
        public void PreBuildCompositeBuilderStrategyStepIPreBuilding()
        {
            Assert.AreEqual(BuildingStep.PreBuilding, this.strategy.BuildingStep);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddingStrategyWithDifferentBuildingStepThrowsException() 
        {
            var mockStrategy = new Mock<IBuilderStrategy>();
            mockStrategy.SetupGet(s => s.BuildingStep).Returns(BuildingStep.ConstructorDetermination);
            this.strategy.AddStrategy(mockStrategy.Object);
        }

        [TestMethod]
        public void StrategyOrderIsCorrectInComparisonToOtherStrategies()
        {
            var mockDependenciesStrategy = new Mock<IBuilderStrategy>();
            var mockConstructorStrategy = new Mock<IBuilderStrategy>();
            mockConstructorStrategy.SetupGet<BuildingStep>(s => s.BuildingStep)
                .Returns(BuildingStep.ConstructorDetermination);
            mockDependenciesStrategy.SetupGet<BuildingStep>(s => s.BuildingStep)
                .Returns(BuildingStep.ConstructorDependenciesResolution);

            Assert.IsTrue(this.strategy.CompareTo(mockDependenciesStrategy.Object) < 0);
            Assert.IsTrue(this.strategy.CompareTo(mockConstructorStrategy.Object) < 0);
        }

        [TestMethod]
        public void AddingStrategyIncreasesCount() 
        {
            var mockStrategy = new Mock<IBuilderStrategy>();
            mockStrategy.SetupGet<BuildingStep>(s => s.BuildingStep)
                .Returns(BuildingStep.PreBuilding);

            this.strategy.AddStrategy(mockStrategy.Object);
            Assert.AreEqual(1, this.strategy.StrategyCount);
        }

        [TestMethod]
        public void CanRemoveStrategyFromCollectionAndDecreaseCount()
        {
            var mockStrategy = new Mock<IBuilderStrategy>();
            mockStrategy.SetupGet<BuildingStep>(s => s.BuildingStep)
                .Returns(BuildingStep.PreBuilding);

            this.strategy.AddStrategy(mockStrategy.Object);
            this.strategy.RemoveStrategy(mockStrategy.Object);
            Assert.AreEqual(0, this.strategy.StrategyCount);
        }

        [TestMethod]
        public void WhenBuildIsCompletedDoesNotExecuteStrategies() 
        {
            bool wasStrategyCalled = false;
            var mockContainer = new Mock<INeedleContainer>();
            var mockBuildStatus = new Mock<IBuildStatus>();
            mockBuildStatus.SetupGet<bool>(status => status.BuildCompleted).Returns(true);
            var mockStrategy = new Mock<IBuilderStrategy>();

            mockStrategy.SetupGet<BuildingStep>(s => s.BuildingStep)
                .Returns(BuildingStep.PreBuilding);
            mockStrategy.Setup(s => s.ExecuteStrategy(mockBuildStatus.Object, mockContainer.Object)).Callback(() => wasStrategyCalled = true);

            this.strategy.AddStrategy(mockStrategy.Object);

            Assert.IsFalse(wasStrategyCalled);
        }
    }
}
