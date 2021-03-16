using Application.Model;
using Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OperationTest
{
    [TestClass]
    public class BitOrientedTest
    {
        Memory mem;
        ApplicationService com;
        SourceFileModel src;
        FileService fil;

        [TestInitialize]
        public void Setup()
        {
            mem = new Memory();
            src = new SourceFileModel
            {
                SourceFile = ""
            };
            fil = new FileService();
            fil.CreateFileList(src);
            com = new ApplicationService(mem, src);
        }

        [TestMethod]
        public void BCF()
        {
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 3;

            com.OperationService.BCF(file, bit);

            Assert.AreEqual(2, mem.RAM[file]);
        }

        [TestMethod]
        public void BSF()
        {
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 2;

            com.OperationService.BSF(file, bit);

            Assert.AreEqual(3, mem.RAM[file]);
        }

        [TestMethod]
        public void BTFSC_NoSkip()
        {
            mem.RAM[Constants.PCL_B1] = 1;
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 3;

            com.OperationService.BTFSC(file, bit);

            Assert.AreEqual(2, mem.RAM[Constants.PCL_B1]);
        }

        [TestMethod]
        public void BTFSC_Skip()
        {
            mem.RAM[Constants.PCL_B1] = 1;
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 2;

            com.OperationService.BTFSC(file, bit);

            Assert.AreEqual(3, mem.RAM[Constants.PCL_B1]);
        }

        [TestMethod]
        public void BTFSS_NoSkip()
        {
            mem.RAM[Constants.PCL_B1] = 1;
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 3;

            com.OperationService.BTFSS(file, bit);

            Assert.AreEqual(3, mem.RAM[Constants.PCL_B1]);
        }

        [TestMethod]
        public void BTFSS_Skip()
        {
            mem.RAM[Constants.PCL_B1] = 1;
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 2;

            com.OperationService.BTFSS(file, bit);

            Assert.AreEqual(2, mem.RAM[Constants.PCL_B1]);
        }
    }
}
