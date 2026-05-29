using GeneticAlgorithm.Core.Genetics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.Core.Tests
{
    [TestClass]
    public class RouletteWheelSelectorTests
    {
        [TestMethod]
        public void SelectIndex_ReturnsValidIndex()
        {
            var cumulative = new[] { 10.0, 30.0, 60.0 };
            int index = RouletteWheelSelector.SelectIndex(cumulative, 60, 3, new System.Random(42));
            Assert.IsTrue(index >= 0 && index < 3);
        }
    }
}
