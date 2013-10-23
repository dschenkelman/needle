namespace Needle.Tests.Builder
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Needle.Builder.Strategies;

    [TestClass]
    public class StrategyStepCollectionFixture
    {
        private BuilderStrategyCollection collection;

        [TestInitialize]
        public void Initialize()
        {
            this.collection = new BuilderStrategyCollection();
        }
        
        [TestMethod]
        public void CreatingBuilderStrategyCollectionCreatesWithoutStrategies()
        {
            Assert.AreEqual(0, this.collection.StrategyCount);
        }

        [TestMethod]
        public void CanAddStrategyToCollectionAndIncreaseCount() 
        {
            var mockStrategy = new Mock<IBuilderStrategy>();
            mockStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.ConstructorDetermination);
            
            this.collection.AddStrategy(mockStrategy.Object);
            Assert.AreEqual(1, this.collection.StrategyCount);
        }

        [TestMethod]
        public void CanRemoveStrategyFromCollectionAndDecreaseCount() 
        {
            var mockStrategy = new Mock<IBuilderStrategy>();
            mockStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.ConstructorDetermination);

            this.collection.AddStrategy(mockStrategy.Object);
            this.collection.RemoveStrategy(mockStrategy.Object);
            Assert.AreEqual(0, this.collection.StrategyCount);
        }

        [TestMethod]
        public void CanReplaceRegisteredStrategiesInCollection() 
        {
            var mockStrategy = new Mock<IBuilderStrategy>();
            mockStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.ConstructorDetermination);

            var secondMockStrategy = new Mock<IBuilderStrategy>();
            secondMockStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.ConstructorDetermination);

            this.collection.AddStrategy(mockStrategy.Object);
            this.collection.AddStrategy(secondMockStrategy.Object);

            Assert.AreSame(secondMockStrategy.Object, this.collection.GetStrategy(BuildingStep.ConstructorDetermination));
        }

        [TestMethod]
        public void AddedStrategiesAreOrderedInCollection() 
        {
            var mockConstructorStrategy = new Mock<IBuilderStrategy>();
            mockConstructorStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.ConstructorDetermination);

            var mockDependenciesStrategy = new Mock<IBuilderStrategy>();
            mockDependenciesStrategy.SetupGet<BuildingStep>(strategy => strategy.BuildingStep)
                .Returns(BuildingStep.ConstructorDependenciesResolution);

            mockDependenciesStrategy.Setup(s => s.CompareTo(mockConstructorStrategy.Object)).Returns(1).Verifiable();

            var strategies = new[] { mockConstructorStrategy.Object, mockDependenciesStrategy.Object };

            this.collection.AddStrategy(mockDependenciesStrategy.Object);
            this.collection.AddStrategy(mockConstructorStrategy.Object);

            int i = 0;
            foreach (var strategy in this.collection)
            {
                Assert.AreSame(strategies[i], strategy);
                i++;
            }

            mockDependenciesStrategy.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CallingAddStrategyWithNullStrategyThrows() 
        {
            this.collection.AddStrategy(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CallingRemoveStrategyWithNullStrategyThrows()
        {
            this.collection.RemoveStrategy(null);
        }
    }
}
