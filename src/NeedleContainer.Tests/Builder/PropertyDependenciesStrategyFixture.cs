namespace Needle.Tests.Builder
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Needle.Builder.Strategies;
    using Needle.Container;

    /// <summary>
    /// Summary description for PropertyDependenciesStrategyFixture
    /// </summary>
    [TestClass]
    public class PropertyDependenciesStrategyFixture
    {
        private PropertyDependenciesStrategy strategy;

        [TestInitialize]
        public void Initialize()
        {
            this.strategy = new PropertyDependenciesStrategy();
        }

        [TestMethod]
        public void DependenciesStrategyStepIsDependenciesResolution()
        {
            Assert.AreEqual(BuildingStep.PropertyDependenciesResolution, this.strategy.BuildingStep);
        }

        [TestMethod]
        public void StrategyOrderIsCorrectInComparisonToOtherStrategies()
        {
            var mockConstructorStrategy = new Mock<IBuilderStrategy>();
            var mockPreBuildStrategy = new Mock<IBuilderStrategy>();
            var mockConstructorResolutionstrategy = new Mock<IBuilderStrategy>();

            mockPreBuildStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.PreBuilding);
            mockConstructorStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.ConstructorDetermination);
            mockConstructorResolutionstrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.ConstructorDependenciesResolution);

            Assert.IsTrue(this.strategy.CompareTo(mockConstructorStrategy.Object) > 0);
            Assert.IsTrue(this.strategy.CompareTo(mockPreBuildStrategy.Object) > 0);
            Assert.IsTrue(this.strategy.CompareTo(mockConstructorResolutionstrategy.Object) > 0);
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
            new PropertyDependenciesStrategy().CompareTo(null);
        }
    }
}
