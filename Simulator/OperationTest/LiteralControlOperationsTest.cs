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
        MemoryService mem;
        LiteralControlOperations opService;
        FileService fil;

        [TestInitialize]
        public void Setup()
        {
            //mocking the ObservableStack would be more complicated than just using a normal stack
            //the Ports of the memory are mocked, because they contain logic, that is not needed for the operations to work
            mem = new MemoryService(new RAMModel(new Mock<Port>().Object, new Mock<Port>().Object), new Stack<short>(MemoryConstants.PC_STACK_CAPACITY));
            
            opService = new LiteralControlOperations(mem);
        }

        [TestMethod]
        public void ADDLW()
        {
            //arrange
            int literal = 10;
            mem.WReg = 6;

            //act
            ResultInfo op_result = opService.ADDLW(literal);

            //assert
            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(literal, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(6, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(16, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void ADDLW_Overflow()
        {
            int literal = 250;
            mem.WReg = 10;

            ResultInfo op_result = opService.ADDLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(literal, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(10, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(260, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void ANDLW()
        {
            int literal = 10;
            mem.WReg = 6;

            ResultInfo op_result = opService.ANDLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(2, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void Call()
        {
            int address = 0x_0f;
            int return_address = mem.RAM[MemoryConstants.PCL_B1] + 1;
            int newPc = address + ((mem.RAM[MemoryConstants.PCLATH_B1] & 0b_0001_1000) << 8);

            ResultInfo op_result = opService.CALL(address);

            Assert.AreEqual(return_address, mem.PCStack.Pop());
            Assert.AreEqual(newPc, op_result.JumpAddress);
            Assert.AreEqual(2, op_result.Cycles);
            Assert.AreEqual(0x_0f, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.PCL_B1, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void Goto()
        {
            int address = 0x_0f;
            int newPc = address + ((mem.RAM[MemoryConstants.PCLATH_B1] & 0b_0001_1000) << 8);

            ResultInfo op_result = opService.GOTO(address);

            Assert.AreEqual(newPc, op_result.JumpAddress);
            Assert.AreEqual(2, op_result.Cycles);
            Assert.AreEqual(0x_0f, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.PCL_B1, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void IORLW()
        {
            int literal = 10;
            mem.WReg = 6;

            ResultInfo op_result = opService.IORLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(14, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void MovLW()
        {
            int literal = 10;
            mem.WReg = 6;

            ResultInfo op_result = opService.MOVLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(10, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void RETFIE_Address()
        {
            mem.PCStack.Push(0x_0f);
            int value = mem.RAM[MemoryConstants.INTCON_B1] | 0b_1000_0000;

            ResultInfo op_result = opService.RETFIE();

            Assert.AreEqual(2, op_result.Cycles);
            Assert.IsTrue(op_result.ClearISR);
            Assert.AreEqual(0x_0f, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.PCL_B1, op_result.OperationResults[0].Address);
            Assert.AreEqual(value, op_result.OperationResults[1].Value);
            Assert.AreEqual(MemoryConstants.INTCON_B1, op_result.OperationResults[1].Address);
        }

        [TestMethod]
        public void RETLW_Wreg()
        {
            mem.PCStack.Push(0x_0f);
            int literal = 10;

            ResultInfo op_result = opService.RETLW(literal);

            Assert.AreEqual(2, op_result.Cycles);
            Assert.AreEqual(literal, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
            Assert.AreEqual(0x_0f, op_result.OperationResults[1].Value);
            Assert.AreEqual(MemoryConstants.PCL_B1, op_result.OperationResults[1].Address);
        }

        [TestMethod]
        public void RETURN()
        {
            mem.PCStack.Push(0x_0f);

            ResultInfo op_result = opService.RETURN();

            Assert.AreEqual(2, op_result.Cycles);
            Assert.AreEqual(0x_0f, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.PCL_B1, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void Sleep_PD()
        {
            mem.RAM[MemoryConstants.STATUS_B1] = 0;

            ResultInfo op_result = opService.SLEEP();

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(16, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.STATUS_B1, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void SUBLW()
        {
            int literal = 10;
            mem.WReg = 6;

            ResultInfo op_result = opService.SUBLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(literal, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(6, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("-", op_result.OverflowInfo.Operator);
            Assert.AreEqual(4, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void SUBLW_Overflow()
        {
            int literal = 10;
            mem.WReg = 20;

            ResultInfo op_result = opService.SUBLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(literal, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(20, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("-", op_result.OverflowInfo.Operator);
            Assert.AreEqual(-10, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void XORLW()
        {
            int literal = 10;
            mem.WReg = 6;

            ResultInfo op_result = opService.XORLW(literal);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(12, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }
    }
}
