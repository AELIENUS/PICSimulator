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
    public class ByteOperationsTest
    {
        private Mock<IMemoryService> _mem;
        private Mock<IRAMModel> _ram;
        private ByteOperations _opService;

        private readonly int _file = 0x_0f;
        private readonly int _d0 = 0;
        private readonly int _d1 = 1;

        

        [TestInitialize]
        public void Setup()
        {
            _mem = new Mock<IMemoryService>().SetupAllProperties();
            _ram = new Mock<IRAMModel>().SetupAllProperties();
            _mem.Setup(m => m.RAM).Returns(_ram.Object);
            _opService = new ByteOperations(_mem.Object);
        }

        [TestMethod]
        public void ADDWF_d0()
        {
            _ram.SetupGet(p => p[_file]).Returns(10);
            _mem.SetupGet(p => p.WReg).Returns(6);

            ResultInfo op_result = _opService.ADDWF(_file, _d0);

            Assert.AreEqual(10, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(6, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(16, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _mem.Verify(p => p.WReg);
        }


        [TestMethod]
        public void ADDWF_d1_Overflow()
        {
            _ram.SetupGet(p => p[_file]).Returns(10);
            _mem.SetupGet(p => p.WReg).Returns(250);

            ResultInfo op_result = _opService.ADDWF(_file, _d1);

            Assert.AreEqual(10, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(250, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(260, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _mem.Verify(p => p.WReg);
        }

        [TestMethod]
        public void ANDWF_d0()
        {
            _ram.SetupGet(p => p[_file]).Returns(10);
            _mem.SetupGet(p => p.WReg).Returns(6);

            ResultInfo op_result = _opService.ANDWF(_file, _d0);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(2, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _mem.Verify(p => p.WReg);
        }


        [TestMethod]
        public void ANDWF_d1()
        {
            _ram.SetupGet(p => p[_file]).Returns(10);
            _mem.SetupGet(p => p.WReg).Returns(6);

            ResultInfo op_result = _opService.ANDWF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(2, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _mem.Verify(p => p.WReg);
        }

        [TestMethod]
        public void CLRF()
        {
            ResultInfo op_result = _opService.CLRF(_file);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(0, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void CLRW()
        {
            ResultInfo op_result = _opService.CLRW();

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(0, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void COMF()
        {
            _ram.SetupGet(p => p[_file]).Returns(0x_13);

            ResultInfo op_result = _opService.COMF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(0x_EC, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
        }

        [TestMethod]
        public void DECF()
        {
            _ram.SetupGet(p => p[_file]).Returns(13);

            ResultInfo op_result = _opService.DECF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(12, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
        }

        [TestMethod]
        public void DECFSZ_wert()
        {
            _ram.SetupGet(p => p[_file]).Returns(13);

            ResultInfo op_result = _opService.DECFSZ(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(12, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);
            Assert.IsFalse(op_result.BeginLoop);

            _ram.Verify(p => p[_file]);
        }

        [TestMethod]
        public void DECFSZ_Skip()
        {
            _ram.SetupGet(p => p[_file]).Returns(1);

            ResultInfo op_result = _opService.DECFSZ(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(0, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);
            Assert.IsTrue(op_result.BeginLoop);

            _ram.Verify(p => p[_file]);
        }

        [TestMethod]
        public void INCF()
        {
            _ram.SetupGet(p => p[_file]).Returns(13);

            ResultInfo op_result = _opService.INCF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(14, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
        }

        [TestMethod]
        public void INCFSZ_noSkip()
        {
            _ram.SetupGet(p => p[_file]).Returns(13);

            ResultInfo op_result = _opService.INCFSZ(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(14, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);
            Assert.IsFalse(op_result.BeginLoop);

            _ram.Verify(p => p[_file]);
        }

        [TestMethod]
        public void INCFSZ_Skip()
        {
            _ram.SetupGet(p => p[_file]).Returns(255);

            ResultInfo op_result = _opService.INCFSZ(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(0, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);
            Assert.IsTrue(op_result.BeginLoop);

            _ram.Verify(p => p[_file]);
        }

        [TestMethod]
        public void IORWF()
        {
            _ram.SetupGet(p => p[_file]).Returns(10);
            _mem.SetupGet(p => p.WReg).Returns(6);

            ResultInfo op_result = _opService.IORWF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(14, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _mem.Verify(p => p.WReg);
        }

        [TestMethod]
        public void MOVF()
        {
            _ram.SetupGet(p => p[_file]).Returns(10);

            ResultInfo op_result = _opService.MOVF(_file, _d0);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(10, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
        }


        [TestMethod]
        public void MOVWF()
        {
            _mem.SetupGet(p => p.WReg).Returns(10);

            ResultInfo op_result = _opService.MOVWF(_file);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(10, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _mem.Verify(p => p.WReg);
        }


        [TestMethod]
        public void NOP()
        {
            ResultInfo op_result = _opService.NOP();

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
        }

        [TestMethod]
        public void RLF_carry1()
        {
            _ram.SetupGet(p => p[_file]).Returns(10);
            _ram.SetupGet(p => p[MemoryConstants.STATUS_B1]).Returns(1);

            ResultInfo op_result = _opService.RLF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(21 ,op_result.OverflowInfo.Operand1);
            Assert.AreEqual(0, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(21, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _ram.Verify(p => p[MemoryConstants.STATUS_B1]);
        }

        [TestMethod]
        public void RLF_carry0()
        {
            _ram.SetupGet(p => p[_file]).Returns(10);
            _ram.SetupGet(p => p[MemoryConstants.STATUS_B1]).Returns(0);

            ResultInfo op_result = _opService.RLF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(20, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(0, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(20, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _ram.Verify(p => p[MemoryConstants.STATUS_B1]);
        }

        [TestMethod]
        public void RLF_Overflow()
        {
            _ram.SetupGet(p => p[_file]).Returns(138);
            _ram.SetupGet(p => p[MemoryConstants.STATUS_B1]).Returns(0);

            ResultInfo op_result = _opService.RLF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(276, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(0, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(276, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _ram.Verify(p => p[MemoryConstants.STATUS_B1]);
        }

        [TestMethod]
        public void RRF_carry1()
        {
            _ram.SetupGet(p => p[_file]).Returns(10);
            _ram.SetupGet(p => p[MemoryConstants.STATUS_B1]).Returns(1);

            ResultInfo op_result = _opService.RRF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(133, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(0, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(133, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _ram.Verify(p => p[MemoryConstants.STATUS_B1]);
        }

        [TestMethod]
        public void RRF_carry0()
        {
            _ram.SetupGet(p => p[_file]).Returns(10);
            _ram.SetupGet(p => p[MemoryConstants.STATUS_B1]).Returns(0);

            ResultInfo op_result = _opService.RRF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(5, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(0, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(5, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _ram.Verify(p => p[MemoryConstants.STATUS_B1]);
        }

        [TestMethod]
        public void RRF_Overflow()
        {
            _ram.SetupGet(p => p[_file]).Returns(11);
            _ram.SetupGet(p => p[MemoryConstants.STATUS_B1]).Returns(0);

            ResultInfo op_result = _opService.RRF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(5, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(256, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(5, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _ram.Verify(p => p[MemoryConstants.STATUS_B1]);
        }

        [TestMethod]
        public void SUBWF()
        {
            _ram.SetupGet(p => p[_file]).Returns(3);
            _mem.SetupGet(p => p.WReg).Returns(2);

            ResultInfo op_result = _opService.SUBWF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(3, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(2, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("-", op_result.OverflowInfo.Operator);
            Assert.AreEqual(1, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _mem.Verify(p => p.WReg);
        }

        [TestMethod]
        public void SUBWF_Overflow()
        {
            _ram.SetupGet(p => p[_file]).Returns(10);
            _mem.SetupGet(p => p.WReg).Returns(20);

            ResultInfo op_result = _opService.SUBWF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(10, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(20, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("-", op_result.OverflowInfo.Operator);
            Assert.AreEqual(-10, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _mem.Verify(p => p.WReg);
        }

        [TestMethod]
        public void SWAPF()
        {
            _ram.SetupGet(p => p[_file]).Returns(0x_A5);

            ResultInfo op_result = _opService.SWAPF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(0x_5A, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
        }

        [TestMethod]
        public void XORWF()
        {
            _ram.SetupGet(p => p[_file]).Returns(0x_AF);
            _mem.SetupGet(p => p.WReg).Returns(0x_B5);

            ResultInfo op_result = _opService.XORWF(_file, _d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(0x_1A, op_result.OperationResults[0].Value);
            Assert.AreEqual(_file, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[_file]);
            _mem.Verify(p => p.WReg);
        }
    }
}