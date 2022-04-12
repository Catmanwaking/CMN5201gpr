//Author: Dominik Dohmeier
using _0h_h1_3D_Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleTests
{
    [TestClass]
    public class RuleTests
    {
        private readonly int[,] validArrayFull =
        {
            { 2, 1, 1, 2 },
            { 1, 1, 2, 2 },
            { 2, 2, 1, 1 },
            { 1, 2, 2, 1 }
        };

        private readonly int[,] validArrayFull_Big =
        {
            { 1, 2, 2, 1, 2, 2, 1, 1, 2, 1 },
            { 2, 1, 1, 2, 1, 2, 2, 1, 1, 2 },
            { 2, 1, 2, 1, 2, 1, 2, 2, 1, 1 },
            { 1, 2, 2, 1, 2, 1, 1, 2, 2, 1 },
            { 2, 1, 1, 2, 1, 2, 1, 1, 2, 2 },
            { 2, 2, 1, 2, 1, 1, 2, 2, 1, 1 },
            { 1, 1, 2, 1, 2, 2, 1, 1, 2, 2 },
            { 1, 2, 1, 2, 1, 1, 2, 2, 1, 2 },
            { 2, 1, 2, 1, 1, 2, 2, 1, 2, 1 },
            { 1, 2, 1, 2, 2, 1, 1, 2, 1, 2 }
        };

        private readonly int[,] validArrayPartial =
        {
            { 0, 1, 0, 2 },
            { 0, 1, 0, 1 },
            { 1, 2, 1, 2 },
            { 0, 0, 1, 0 }
        };

        private readonly int[,] invalidArrayFull_AdjacentFailOnly =
        {
            { 1, 2, 2, 2, 1, 1 },
            { 1, 1, 1, 2, 2, 2 },
            { 2, 1, 2, 1, 1, 2 },
            { 2, 2, 2, 1, 1, 1 },
            { 1, 2, 1, 1, 2, 2 },
            { 2, 1, 1, 2, 2, 1 }
        };

        private readonly int[,] invalidArrayFull_SameLineFailOnly =
        {
            { 1, 2, 2, 1 },
            { 2, 1, 1, 2 },
            { 1, 2, 2, 1 },
            { 2, 1, 1, 2 }
        };

        private readonly int[,] invalidArrayFull_EqualCountFailOnly =
        {
            { 1, 2, 2, 1 },
            { 2, 2, 1, 1 },
            { 1, 1, 2, 2 },
            { 1, 2, 1, 2 }
        };

        #region AdjacencyRule

        [TestMethod]
        public void AdjacentRule_WithVaildArrayFull()
        {
            Assert.IsTrue(Rules2D.AdjacencyRule(validArrayFull));
        }

        [TestMethod]
        public void AdjacentRule_WithVaildArrayFull_Big()
        {
            Assert.IsTrue(Rules2D.AdjacencyRule(validArrayFull_Big));
        }

        [TestMethod]
        public void AdjacentRule_WithVaildArrayPartial()
        {
            Assert.IsTrue(Rules2D.AdjacencyRule(validArrayPartial));
        }

        [TestMethod]
        public void AdjacentRule_WithInvaildArrayFull()
        {
            Assert.IsFalse(Rules2D.AdjacencyRule(invalidArrayFull_AdjacentFailOnly));
        }

        #endregion

        #region EqualCountRule

        [TestMethod]
        public void EqualCountRule_WithValidArrayFull()
        {
            Assert.IsTrue(Rules2D.EqualCountRule(validArrayFull));
        }

        [TestMethod]
        public void EqualCountRule_WithValidArrayFull_Big()
        {
            Assert.IsTrue(Rules2D.EqualCountRule(validArrayFull_Big));
        }

        [TestMethod]
        public void EqualCountRule_WithValidArrayPartial()
        {
            Assert.IsTrue(Rules2D.EqualCountRule(validArrayPartial));
        }

        [TestMethod]
        public void EqualCountRule_WithInvalidArrayFull()
        {
            Assert.IsFalse(Rules2D.EqualCountRule(invalidArrayFull_EqualCountFailOnly));
        }

        #endregion

        #region SameLineRule

        [TestMethod]
        public void SameLineRule_WithVaildArrayFull()
        {
            Assert.IsTrue(Rules2D.SameLineRule(validArrayFull));
        }

        [TestMethod]
        public void SameLineRule_WithVaildArrayFull_Big()
        {
            Assert.IsTrue(Rules2D.SameLineRule(validArrayFull_Big));
        }

        [TestMethod]
        public void SameLineRule_WithVaildArrayPartial()
        {
            Assert.IsTrue(Rules2D.SameLineRule(validArrayPartial));
        }

        [TestMethod]
        public void SameLineRule_WithInvaildArrayFull()
        {
            Assert.IsFalse(Rules2D.SameLineRule(invalidArrayFull_SameLineFailOnly));
        }

        #endregion
    }
}