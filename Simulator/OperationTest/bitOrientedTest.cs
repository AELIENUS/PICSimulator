using System.Collections.Generic;
using Application.Constants;
using Application.Models.CodeLogic;
using Application.Models.Memory;
using Application.Models.OperationLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OperationTest
{
    [TestClass]
    public class BitOrientedTest
    {
        MemoryService mem;
        OperationService opService;
        SourceFileModel src;
        FileService fil;

        [TestInitialize]
        public void Setup()
        {
            mem = new MemoryService();
            src = new SourceFileModel
            {
                SourceFile = ""
            };
            fil = new FileService();
            fil.CreateFileList(src);
            opService = new OperationService(mem, src);
        }

        [TestMethod]
        public void BCF()
        {
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 3;

            ResultInfo op_resultInfo = opService.BCF(file, bit);

            Assert.AreEqual(1, op_resultInfo.PCIncrement);
            Assert.AreEqual(1, op_resultInfo.Cycles);
            Assert.AreEqual(file, op_resultInfo.OperationResults[0].Address);
            Assert.AreEqual(2, op_resultInfo.OperationResults[0].Value);
        }

        [TestMethod]
        public void BSF()
        {
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 2;

            ResultInfo op_result = opService.BSF(file, bit);

            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.AreEqual(file, op_result.OperationResults[0].Address);
            Assert.AreEqual(3, op_result.OperationResults[0].Value);
        }

        [TestMethod]
        public void BTFSC_NoSkip()
        {
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 3;

            ResultInfo op_result =  opService.BTFSC(file, bit);

            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.IsFalse(op_result.BeginLoop);
        }

        [TestMethod]
        public void BTFSC_Skip()
        {
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 2;

            ResultInfo op_result = opService.BTFSC(file, bit);

            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.IsTrue(op_result.BeginLoop);
        }

        [TestMethod]
        public void BTFSS_Skip()
        {
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 3;

            ResultInfo op_result = opService.BTFSS(file, bit);

            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.IsTrue(op_result.BeginLoop);
        }

        [TestMethod]
        public void BTFSS_NoSkip()
        {
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 2;

            ResultInfo op_result = opService.BTFSS(file, bit);

            Assert.AreEqual(1, op_result.PCIncrement);
            Assert.AreEqual(1, op_result.Cycles);
            Assert.IsFalse(op_result.BeginLoop);
        }
    }
}
