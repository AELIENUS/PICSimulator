using System.Collections.Generic;
using Application.Constants;
using Application.Models.CodeLogic;
using Application.Models.Memory;
using Application.Models.OperationLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace OperationTest
{
    [TestClass]
    public class BitOperationsTest
    {
        private Mock<IMemoryService> _mem;
        private Mock<IRAMModel> _ram;
        private BitOperations _opService;
        private int file = 0x_0f;
        private int bit = 0;

        [TestInitialize]
        public void Setup()
        {
            _mem = new Mock<IMemoryService>().SetupAllProperties();
            _ram = new Mock<IRAMModel>().SetupAllProperties();
            _mem.Setup(m => m.RAM).Returns(_ram.Object);
            _opService = new BitOperations(_mem.Object);
        }

        [TestMethod]
        public void BCF()
        {
            _ram.SetupGet(p => p[file]).Returns(3);

            ResultInfo op_resultInfo = _opService.BCF(file, bit);

            Assert.AreEqual(1, op_resultInfo.PCIncrement);
            Assert.AreEqual(1, op_resultInfo.Cycles);
            Assert.AreEqual(file, op_resultInfo.OperationResults[0].Address);
            Assert.AreEqual(2, op_resultInfo.OperationResults[0].Value);

            _ram.Verify(p => p[file]);
        }

        [TestMethod]
        public void BSF()
        {
            _ram.SetupGet(p => p[file]).Returns(2);

            ResultInfo op_result = _opService.BSF(file, bit);

            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
            Assert.AreEqual(3, op_result.OperationResults[0].Value);

            _ram.Verify(p => p[file]);
        }

        [TestMethod]
        public void BTFSC_NoSkip()
        {
            _ram.SetupGet(p => p[file]).Returns(3);

            ResultInfo op_result =  _opService.BTFSC(file, bit);

            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.IsFalse(op_result.BeginLoop);

            _ram.Verify(p => p[file]);
        }

        [TestMethod]
        public void BTFSC_Skip()
        {
            _ram.SetupGet(p => p[file]).Returns(2);

            ResultInfo op_result = _opService.BTFSC(file, bit);

            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.IsTrue(op_result.BeginLoop);

            _ram.Verify(p => p[file]);
        }

        [TestMethod]
        public void BTFSS_Skip()
        {
            _ram.SetupGet(p => p[file]).Returns(3);

            ResultInfo op_result = _opService.BTFSS(file, bit);

            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.IsTrue(op_result.BeginLoop);

            _ram.Verify(p => p[file]);
        }

        [TestMethod]
        public void BTFSS_NoSkip()
        {
            _ram.SetupGet(p => p[file]).Returns(2);

            ResultInfo op_result = _opService.BTFSS(file, bit);

            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.IsFalse(op_result.BeginLoop);

            _ram.Verify(p => p[file]);
        }
    }
}
