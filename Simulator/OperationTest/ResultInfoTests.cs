using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Application.Models.OperationLogic;

namespace OperationTest
{
    [TestClass]
    public class ResultInfoTests
    {
        private ResultInfo res;

        [TestInitialize]
        public void Setup()
        {
            res = new ResultInfo();
        }

        [TestMethod]
        public void AreAllPropertiesNull()
        {
            res = new ResultInfo();

            Assert.IsFalse(res.CheckZ);
            Assert.IsFalse(res.BeginLoop);
            Assert.IsFalse(res.ClearISR);
            Assert.IsNull(res.Cycles);
            Assert.IsNull(res.JumpAddress);
            Assert.IsNull(res.OperationResults);
            Assert.IsNull(res.PCIncrement);
            Assert.IsNull(res.OverflowInfo);
        }
    }
}
