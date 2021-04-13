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
        int file = 0x_0f;
        int d0 = 0;
        int d1 = 1;

        MemoryService mem;
        ByteOperations opService;
        SourceFileModel src;
        FileService fil;

        [TestInitialize]
        public void Setup()
        {
            mem = new MemoryService(new RAMModel(new Mock<Port>().Object, new Mock<Port>().Object), new Stack<short>(MemoryConstants.PC_STACK_CAPACITY));
            src = new SourceFileModel();
            src.SourceFile = "";
            fil = new FileService();
            fil.CreateFileList(src);
            opService = new ByteOperations(mem);
        }

        [TestMethod]
        public void ADDWF_d0()
        {
            mem.RAM[file] = 10;
            mem.WReg = 6;

            ResultInfo op_result = opService.ADDWF(file, d0);

            Assert.AreEqual(10, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(6, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(16, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }


        [TestMethod]
        public void ADDWF_d1_Overflow()
        {
            mem.RAM[file] = 10;
            mem.WReg = 250;

            ResultInfo op_result = opService.ADDWF(file, d1);

            Assert.AreEqual(10, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(250, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(260, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void ANDWF_d0()
        {
            mem.RAM[file] = 10;
            mem.WReg = 6;

            ResultInfo op_result = opService.ANDWF(file, d0);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(2, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }


        [TestMethod]
        public void ANDWF_d1()
        {
            mem.RAM[file] = 10;
            mem.WReg = 6;

            ResultInfo op_result = opService.ANDWF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(2, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void CLRF()
        {
            mem.RAM[file] = 10;

            ResultInfo op_result = opService.CLRF(file);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(0, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void CLRW()
        {
            mem.WReg = 17;

            ResultInfo op_result = opService.CLRW();

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(0, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void COMF()
        {
            mem.RAM[file] = 0x_13;

            ResultInfo op_result = opService.COMF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(0x_EC, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void DECF()
        {
            mem.RAM[file] = 13;

            ResultInfo op_result = opService.DECF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(12, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void DECFSZ_wert()
        {
            mem.RAM[file] = 13;

            ResultInfo op_result = opService.DECFSZ(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(12, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
            Assert.IsFalse(op_result.BeginLoop);
        }

        [TestMethod]
        public void DECFSZ_Skip()
        {
            mem.RAM[file] = 1;

            ResultInfo op_result = opService.DECFSZ(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(0, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
            Assert.IsTrue(op_result.BeginLoop);
        }

        [TestMethod]
        public void INCF()
        {
            mem.RAM[file] = 13;

            ResultInfo op_result = opService.INCF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(14, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void INCFSZ_noSkip()
        {
            mem.RAM[file] = 13;

            ResultInfo op_result = opService.INCFSZ(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(14, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
            Assert.IsFalse(op_result.BeginLoop);
        }

        [TestMethod]
        public void INCFSZ_Skip()
        {
            mem.RAM[file] = 255;

            ResultInfo op_result = opService.INCFSZ(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(0, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
            Assert.IsTrue(op_result.BeginLoop);
        }

        [TestMethod]
        public void IORWF()
        {
            mem.RAM[file] = 10;
            mem.WReg = 6;

            ResultInfo op_result = opService.IORWF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(14, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void MOVF()
        {
            mem.RAM[file] = 10;

            ResultInfo op_result = opService.MOVF(file, d0);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(10, op_result.OperationResults[0].Value);
            Assert.AreEqual(MemoryConstants.WRegPlaceholder, op_result.OperationResults[0].Address);
        }


        [TestMethod]
        public void MOVWF()
        {
            mem.RAM[file] = 6;
            mem.WReg = 10;

            ResultInfo op_result = opService.MOVWF(file);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(10, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }


        [TestMethod]
        public void NOP()
        {
            ResultInfo op_result = opService.NOP();

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
        }

        [TestMethod]
        public void RLF_carry1()
        {
            mem.RAM[file] = 10;
            mem.RAM[MemoryConstants.STATUS_B1] = 1;

            ResultInfo op_result = opService.RLF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(21 ,op_result.OverflowInfo.Operand1);
            Assert.AreEqual(0, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(21, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void RLF_carry0()
        {
            mem.RAM[file] = 10;
            mem.RAM[MemoryConstants.STATUS_B1] = 0;

            ResultInfo op_result = opService.RLF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(20, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(0, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(20, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void RLF_Overflow()
        {
            mem.RAM[file] = 138;
            mem.RAM[MemoryConstants.STATUS_B1] = 0;

            ResultInfo op_result = opService.RLF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(276, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(0, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(276, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void RRF_carry1()
        {
            mem.RAM[file] = 10;
            mem.RAM[MemoryConstants.STATUS_B1] = 1;

            ResultInfo op_result = opService.RRF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(133, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(0, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(133, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void RRF_carry0()
        {
            mem.RAM[file] = 10;
            mem.RAM[MemoryConstants.STATUS_B1] = 0;

            ResultInfo op_result = opService.RRF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(5, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(0, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(5, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void RRF_Overflow()
        {
            mem.RAM[file] = 11;
            mem.RAM[MemoryConstants.STATUS_B1] = 0;

            ResultInfo op_result = opService.RRF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(5, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(256, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("+", op_result.OverflowInfo.Operator);
            Assert.AreEqual(5, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void SUBWF()
        {
            mem.RAM[file] = 3;
            mem.WReg = 2;

            ResultInfo op_result = opService.SUBWF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(3, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(2, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("-", op_result.OverflowInfo.Operator);
            Assert.AreEqual(1, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void SUBWF_Overflow()
        {
            mem.RAM[file] = 10;
            mem.WReg = 20;

            ResultInfo op_result = opService.SUBWF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(10, op_result.OverflowInfo.Operand1);
            Assert.AreEqual(20, op_result.OverflowInfo.Operand2);
            Assert.AreEqual("-", op_result.OverflowInfo.Operator);
            Assert.AreEqual(-10, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void SWAPF()
        {
            mem.RAM[file] = 0x_A5;

            ResultInfo op_result = opService.SWAPF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(0x_5A, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }

        [TestMethod]
        public void XORWF()
        {
            mem.RAM[file] = 0x_AF;
            mem.WReg = 0x_b5;

            ResultInfo op_result = opService.XORWF(file, d1);

            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.IsTrue(op_result.CheckZ);
            Assert.AreEqual(0x_1A, op_result.OperationResults[0].Value);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
        }
    }
}