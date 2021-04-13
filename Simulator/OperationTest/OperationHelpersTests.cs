using System.Collections.Generic;
using Application.Constants;
using Application.Models.ApplicationLogic;
using Application.Models.CodeLogic;
using Application.Models.OperationLogic;
using Application.Models.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OperationTest
{
    [TestClass]
    public class OperationHelpersTests
    {
        MemoryService mem;
        SourceFileModel src;
        private OperationHelpers opHelpers;

        [TestInitialize]
        public void Setup()
        {
            mem = new MemoryService(new RAMModel(), new Stack<short>(MemoryConstants.PC_STACK_CAPACITY));
            src = new SourceFileModel();
            opHelpers = new OperationHelpers(mem, src);
        }

        [TestMethod]
        public void CheckZ_true()
        {
            int literal = 0;
            
            opHelpers.CheckZ(literal);

            int z = mem.RAM[MemoryConstants.STATUS_B1] & 0b_0000_0100;
            Assert.AreEqual(4, z);
        }

        [TestMethod]
        public void CheckZ_false()
        {
            int literal = 3;

            opHelpers.CheckZ(literal);

            int z = mem.RAM[MemoryConstants.STATUS_B1] & 0b_0000_0100;
            Assert.AreEqual(0, z);
        }

        #region Check dc, c plus
        [TestMethod]
        public void Check_DC_falsePlus()
        {
            int lit1 = 0;
            int lit2 = 6;
            OverflowInfo overflowInfo = new OverflowInfo() {Operand1 = lit1, Operand2 = lit2, Operator = "+"};

            opHelpers.Check_DC_C(overflowInfo, lit1+lit2);


            int dc = mem.RAM[MemoryConstants.STATUS_B1] & 0b_0000_0010;

            Assert.AreEqual(0, dc);
        }

        [TestMethod]
        public void Check_C_falsePlus()
        {
            int lit1 = 0;
            int lit2 = 6;
            OverflowInfo overflowInfo = new OverflowInfo() { Operand1 = lit1, Operand2 = lit2, Operator = "+" };
            opHelpers.Check_DC_C(overflowInfo, lit1 + lit2);

            int c = mem.RAM[MemoryConstants.STATUS_B1] & 0b_0000_0001;
            Assert.AreEqual(0, c);
        }

        [TestMethod]
        public void Check_DC_truePlus()
        {
            int lit1 = 136;
            OverflowInfo overflowInfo = new OverflowInfo() { Operand1 = lit1, Operand2 = lit1, Operator = "+" };
            int result = opHelpers.Check_DC_C(overflowInfo, lit1 + lit1);

            int dc = mem.RAM[MemoryConstants.STATUS_B1] & 0b_0000_0010;

            Assert.AreEqual(2, dc);
        }

        [TestMethod]
        public void Check_C_truePlus()
        {
            int lit1 = 136;
            OverflowInfo overflowInfo = new OverflowInfo() { Operand1 = lit1, Operand2 = lit1, Operator = "+" };
            int result = opHelpers.Check_DC_C(overflowInfo, lit1 + lit1);

            int c = mem.RAM[MemoryConstants.STATUS_B1] & 0b_0000_0001;

            Assert.AreEqual(1, c);
            Assert.AreEqual(16, result);
        }

        #endregion

        #region Check DC, C Minus

        [TestMethod]
        public void Check_DC_falseMinus()
        {
            int lit1 = 6;
            int lit2 = 2;
            OverflowInfo overflowInfo = new OverflowInfo() { Operand1 = lit1, Operand2 = lit2, Operator = "-" };
            opHelpers.Check_DC_C(overflowInfo, lit1 - lit2);

            int dc = mem.RAM[MemoryConstants.STATUS_B1] & 0b_0000_0010;

            //umgekehrte logik
            Assert.AreEqual(2, dc);
        }

        [TestMethod]
        public void Check_C_falseMinus()
        {
            int lit1 = 6;
            int lit2 = 2;
            OverflowInfo overflowInfo = new OverflowInfo() { Operand1 = lit1, Operand2 = lit2, Operator = "-" };
            opHelpers.Check_DC_C(overflowInfo, lit1 - lit2);

            int c = mem.RAM[MemoryConstants.STATUS_B1] & 0b_0000_0001;

            //umgekehrte logik
            Assert.AreEqual(1, c);
        }

        [TestMethod]
        public void Check_DC_trueMinus()
        {
            int lit1 = 68;
            int lit2 = 136;
            OverflowInfo overflowInfo = new OverflowInfo() { Operand1 = lit1, Operand2 = lit2, Operator = "-" };
            opHelpers.Check_DC_C(overflowInfo, lit1 - lit2);

            int dc = mem.RAM[MemoryConstants.STATUS_B1] & 0b_0000_0010;

            //umgekehrte logik
            Assert.AreEqual(0, dc);
        }

        [TestMethod]
        public void Check_C_trueMinus()
        {
            int lit1 = 68;
            int lit2 = 136;
            OverflowInfo overflowInfo = new OverflowInfo() { Operand1 = lit1, Operand2 = lit2, Operator = "-" };
            opHelpers.Check_DC_C(overflowInfo, lit1 - lit2);

            int c = mem.RAM[MemoryConstants.STATUS_B1] & 0b_0000_0001;

            //umgekehrte logik
            Assert.AreEqual(0, c);
        }
        #endregion

        [TestMethod]
        public void StoreOperationResult_W()
        {
            int result = 15;
            List<OperationResult> results = new List<OperationResult>();
            results.Add(new OperationResult() {Value = result, Address = MemoryConstants.WRegPlaceholder});

            opHelpers.WriteOperationResults(results);

            Assert.AreEqual(15, mem.WReg);
        }
        //2 results, file, w 

        [TestMethod]
        public void StoreOperationResult_file()
        {
            int file = 0x_0f;
            int result = 15;
            List<OperationResult> results = new List<OperationResult>();
            results.Add(new OperationResult() { Value = result, Address = file });

            opHelpers.WriteOperationResults(results);

            Assert.AreEqual(15, mem.RAM[file]);
        }

        [TestMethod]
        public void StoreOperationResult_twoResults()
        {
            int file = 0x_0f;
            int result = 15;
            List<OperationResult> results = new List<OperationResult>();
            results.Add(new OperationResult() { Value = result, Address = MemoryConstants.WRegPlaceholder });
            results.Add(new OperationResult() { Value = result, Address = file });

            opHelpers.WriteOperationResults(results);

            Assert.AreEqual(15, mem.WReg);
            Assert.AreEqual(15, mem.RAM[file]);
        }

        [TestMethod]
        public void setJumpAdress()
        { 
            int address = 0x_0f;

            opHelpers.SetJumpAddress(address);

            Assert.AreEqual(0x_0f, mem.RAM.PCJumpAdress);
            Assert.IsTrue(mem.RAM.PCWasJump);
        }

        [TestMethod]
        public void updateCycles()
        {
            //CycleCounter is incremented by 1 every time, the setter is called
            //mem.CycleCounter starts at 0

            opHelpers.UpdateCycles(1);

            Assert.AreEqual(1, mem.CycleCounter);
        }

        [TestMethod]
        public void updateCycles_2()
        {
            //CycleCounter is incremented by 1 every time, the setter is called
            //mem.CycleCounter starts at 0

            opHelpers.UpdateCycles(2);

            Assert.AreEqual(2, mem.CycleCounter);
        }

        [TestMethod]
        public void changePCFetch_overflow()
        {
            mem.RAM[MemoryConstants.PCL_B1] = 0b_1111_1111;
            mem.RAM[MemoryConstants.PCLATH_B1] = 0;

            opHelpers.ChangePC_Fetch(1);

            Assert.AreEqual(1, mem.RAM[MemoryConstants.PCLATH_B1]);
            Assert.AreEqual(1, mem.RAM[MemoryConstants.PCL_B1]);
        }

        [TestMethod]
        public void changePCFetch()
        {
            mem.RAM[MemoryConstants.PCL_B1] = 1;
            mem.RAM[MemoryConstants.PCLATH_B1] = 0;

            opHelpers.ChangePC_Fetch(1);

            Assert.AreEqual(0, mem.RAM[MemoryConstants.PCLATH_B1]);
            Assert.AreEqual(2, mem.RAM[MemoryConstants.PCL_B1]);
        }
    }
}
