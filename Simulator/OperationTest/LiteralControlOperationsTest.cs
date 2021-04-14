using System.Collections.Generic;
using Application.Constants;
using Application.Models.CodeLogic;
using Application.Models.CustomDatastructures;
using Application.Models.Memory;
using Application.Models.OperationLogic;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OperationTest
{
    [TestClass]
    public class LiteralControlOperationsTest
    {
        private Mock<IMemoryService> _mem;
        private Mock<IRAMModel> _ram;
        LiteralControlOperations _opService;

        [TestInitialize]
        public void Setup()
        {
            _mem = new Mock<IMemoryService>().SetupAllProperties();
            _ram = new Mock<IRAMModel>().SetupAllProperties();
            _mem.Setup(m => m.RAM).Returns(_ram.Object);
            Stack<short> _pcStack = new Stack<short>();
            _mem.Setup(m => m.PCStack).Returns(_pcStack);
            _opService = new LiteralControlOperations(_mem.Object);
        }

        [TestMethod]
        public void ADDLW()
        {
            _mem.SetupGet(p => p.WReg).Returns(6);

            //arrange
            int literal = 10;

            //act
            ResultInfo op_result = _opService.ADDLW(literal);

            //assert
            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(literal, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(6, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(16, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);

            _mem.Verify(p => p.WReg);
        }

        [TestMethod]
        public void ADDLW_Overflow()
        {
            _mem.SetupGet(p => p.WReg).Returns(10);

            int literal = 250;

            ResultInfo op_result = _opService.ADDLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(literal, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(10, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(260, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);

            _mem.Verify(p => p.WReg);
        }

        [TestMethod]
        public void ANDLW()
        {
            _mem.SetupGet(p => p.WReg).Returns(6);

            int literal = 10;

            ResultInfo op_result = _opService.ANDLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(2, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);

            _mem.Verify(p => p.WReg);
        }

        [TestMethod]
        public void Call()
        {
            _ram.SetupGet(p => p[MemoryConstants.PCL_B1]).Returns(1);
            _ram.SetupGet(p => p[MemoryConstants.PCLATH_B1]).Returns(0);

            int address = 0x_0f;
            int return_address = 2;
            int newPc = address;

            ResultInfo op_result = _opService.CALL(address);

            Assert.AreEqual(return_address, _mem.Object.PCStack.Pop());
            Assert.AreEqual(newPc, op_result.JumpAddress);
            Assert.AreEqual(2, op_result.Cycles);
            Assert.AreEqual(0x_0f, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.PCL_B1, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[MemoryConstants.PCL_B1]);
            _ram.Verify(p => p[MemoryConstants.PCLATH_B1]);
        }

        [TestMethod]
        public void Goto()
        {
            _ram.SetupGet(p => p[MemoryConstants.PCLATH_B1]).Returns(8);

            int address = 0x_0f;
            int newPc = address + (8 << 8);

            ResultInfo op_result = _opService.GOTO(address);

            Assert.AreEqual(newPc, op_result.JumpAddress);
            Assert.AreEqual(2, op_result.Cycles);
            Assert.AreEqual(0x_0f, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.PCL_B1, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[MemoryConstants.PCLATH_B1]);
        }

        [TestMethod]
        public void IORLW()
        {
            _mem.SetupGet(p => p.WReg).Returns(6);

            int literal = 10;

            ResultInfo op_result = _opService.IORLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(14, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);

            _mem.Verify(p => p.WReg);
        }

        [TestMethod]
        public void MovLW()
        {
            int literal = 10;

            ResultInfo op_result = _opService.MOVLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(10, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void RETFIE_Address()
        {
            _ram.SetupGet(p => p[MemoryConstants.INTCON_B1]).Returns(5);

            _mem.Object.PCStack.Push(0x_0f);
            int value = 5 | 0b_1000_0000;

            ResultInfo op_result = _opService.RETFIE();

            Assert.AreEqual(2, op_result.Cycles);
            Assert.IsTrue(op_result.ClearISR);
            Assert.AreEqual(0x_0f, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.PCL_B1, op_result.OperationResults[0].Address);
            Assert.AreEqual(value, op_result.OperationResults[1].Value);
            Assert.AreEqual(MemoryConstants.INTCON_B1, op_result.OperationResults[1].Address);

            _ram.Verify(p => p[MemoryConstants.INTCON_B1]);
        }

        [TestMethod]
        public void RETLW_Wreg()
        {
            _mem.Object.PCStack.Push(0x_0f);
            int literal = 10;

            ResultInfo op_result = _opService.RETLW(literal);

            Assert.AreEqual(2, op_result.Cycles);
            Assert.AreEqual(literal, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
            Assert.AreEqual(0x_0f, op_result.OperationResults[1].Value);
            Assert.AreEqual(MemoryConstants.PCL_B1, op_result.OperationResults[1].Address);
        }

        [TestMethod]
        public void RETURN()
        {
            _mem.Object.PCStack.Push(0x_0f);

            ResultInfo op_result = _opService.RETURN();

            Assert.AreEqual(2, op_result.Cycles);
            Assert.AreEqual(0x_0f, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.PCL_B1, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void Sleep_PD()
        {
            _ram.SetupGet(p => p[MemoryConstants.STATUS_B1]).Returns(0);

            ResultInfo op_result = _opService.SLEEP();

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(16, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.STATUS_B1, op_result.OperationResults[0].Address);

            _ram.Verify(p => p[MemoryConstants.STATUS_B1]);
        }

        [TestMethod]
        public void SUBLW()
        {
            _mem.SetupGet(p => p.WReg).Returns(6);

            int literal = 10;

            ResultInfo op_result = _opService.SUBLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(literal, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(6, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("-", op_result.OverflowInfo.Operator);
            Assert.AreEqual(4, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);

            _mem.Verify(p => p.WReg);
        }

        [TestMethod]
        public void SUBLW_Overflow()
        {
            _mem.SetupGet(p => p.WReg).Returns(20);

            int literal = 10;

            ResultInfo op_result = _opService.SUBLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(literal, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(20, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("-", op_result.OverflowInfo.Operator);
            Assert.AreEqual(-10, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);

            _mem.Verify(p => p.WReg);
        }

        [TestMethod]
        public void XORLW()
        {
            _mem.SetupGet(p => p.WReg).Returns(6);

            int literal = 10;

            ResultInfo op_result = _opService.XORLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(12, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);

            _mem.Verify(p => p.WReg);
        }
    }
}
