using Application.Model;
using Application.Services;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CommandTest
{
    class FileRegisterCommandsTest
    {
        int file = 0x_0f;
        int d0 = 0;
        int d1 = 1;

        Memory mem;
        CommandService com;
        SourceFileModel src;
        FileService fil;

        [SetUp]
        public void Setup()
        {
            mem = new Memory();
            src = new SourceFileModel();
            src.SourceFile = "";
            fil = new FileService();
            fil.CreateFileList(src);
            com = new CommandService(mem, src);
        }

        [Test]
        public void ADDWF_d0()
        {
            mem.RAM[file] = 10;
            mem.W_Reg = 6;

            com.OperationService.ADDWF(file, d0);

            Assert.AreEqual(16, mem.W_Reg);
        }


        [Test]
        public void ADDWF_d1_Overflow()
        {
            mem.RAM[file] = 10;
            mem.W_Reg = 250;

            com.OperationService.ADDWF(file, d1);

            Assert.AreEqual(4, mem.RAM[file]);
        }

        [Test]
        public void ANDWF_d0()
        {
            mem.RAM[file] = 10;
            mem.W_Reg = 6;

            com.OperationService.ANDWF(file, d0);

            Assert.AreEqual(2, mem.W_Reg);
        }


        [Test]
        public void ANDWF_d1()
        {
            mem.RAM[file] = 10;
            mem.W_Reg = 6;

            com.OperationService.ANDWF(file, d1);

            Assert.AreEqual(2, mem.RAM[file]);
        }

        [Test]
        public void CLRF()
        {
            mem.RAM[file] = 10;

            com.OperationService.CLRF(file);

            Assert.AreEqual(0, mem.RAM[file]);
        }

        [Test]
        public void CLRW()
        {
            mem.W_Reg = 17;

            com.OperationService.CLRW();

            Assert.AreEqual(0, mem.W_Reg);
        }

        [Test]
        public void COMF()
        {
            mem.RAM[file] = 0x_13;

            com.OperationService.COMF(file, d1);

            Assert.AreEqual(0x_EC, mem.RAM[file]);
        }

        [Test]
        public void DECF()
        {
            mem.RAM[file] = 13;

            com.OperationService.DECF(file, d1);

            Assert.AreEqual(12, mem.RAM[file]);
        }

        [Test]
        public void DECFSZ_wert()
        {
            mem.RAM[file] = 13;

            com.OperationService.DECFSZ(file, d1);

            Assert.AreEqual(12, mem.RAM[file]);
        }

        [Test]
        public void DECFSZ_noSkip()
        {
            mem.RAM[file] = 13;
            mem.RAM[Constants.PCL_B1] = 7;

            com.OperationService.DECFSZ(file, d1);

            Assert.AreEqual(8, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void DECFSZ_Skip()
        {
            mem.RAM[file] = 1;
            mem.RAM[Constants.PCL_B1] = 7;

            com.OperationService.DECFSZ(file, d1);

            Assert.AreEqual(9, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void INCF()
        {
            mem.RAM[file] = 13;

            com.OperationService.INCF(file, d1);

            Assert.AreEqual(14, mem.RAM[file]);
        }

        [Test]
        public void INCFSZ_wert()
        {
            mem.RAM[file] = 13;

            com.OperationService.INCFSZ(file, d1);

            Assert.AreEqual(14, mem.RAM[file]);
        }

        [Test]
        public void INCFSZ_noSkip()
        {
            mem.RAM[file] = 13;
            mem.RAM[Constants.PCL_B1] = 7;

            com.OperationService.INCFSZ(file, d1);

            Assert.AreEqual(8, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void INCFSZ_Skip()
        {
            mem.RAM[file] = 255;
            mem.RAM[Constants.PCL_B1] = 7;

            com.OperationService.INCFSZ(file, d1);

            Assert.AreEqual(9, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void IORWF()
        {
            mem.RAM[file] = 10;
            mem.W_Reg = 6;

            com.OperationService.IORWF(file, d1);

            Assert.AreEqual(14, mem.RAM[file]);
        }

        [Test]
        public void MOVF()
        {
            mem.RAM[file] = 10;

            com.OperationService.MOVF(file, d0);

            Assert.AreEqual(10, mem.W_Reg);
        }


        [Test]
        public void MOVWF()
        {
            mem.RAM[file] = 6;
            mem.W_Reg = 10;

            com.OperationService.MOVWF(file);

            Assert.AreEqual(10, mem.RAM[file]);
        }


        [Test]
        public void NOP()
        {
            mem.RAM[Constants.PCL_B1] = 10;

            com.OperationService.NOP();

            Assert.AreEqual(11, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void RLF_carry1()
        {
            mem.RAM[file] = 10;
            mem.RAM[Constants.STATUS_B1] = 1;

            com.OperationService.RLF(file, d1);

            Assert.AreEqual(21, mem.RAM[file]);
        }

        [Test]
        public void RLF_carry0()
        {
            mem.RAM[file] = 10;
            mem.RAM[Constants.STATUS_B1] = 0;

            com.OperationService.RLF(file, d1);

            Assert.AreEqual(20, mem.RAM[file]);
        }

        [Test]
        public void RLF_Overflow_Wert()
        {
            mem.RAM[file] = 138;
            mem.RAM[Constants.STATUS_B1] = 0;

            com.OperationService.RLF(file, d1);

            Assert.AreEqual(20, mem.RAM[file]);
        }


        [Test]
        public void RLF_Overflow_Carry()
        {
            mem.RAM[file] = 138;
            mem.RAM[Constants.STATUS_B1] = 0;

            com.OperationService.RLF(file, d1);

            Assert.AreEqual(1, mem.RAM[Constants.STATUS_B1]);
        }

        [Test]
        public void RRF_carry1()
        {
            mem.RAM[file] = 10;
            mem.RAM[Constants.STATUS_B1] = 1;

            com.OperationService.RRF(file, d1);

            Assert.AreEqual(133, mem.RAM[file]);
        }

        [Test]
        public void RRF_carry0()
        {
            mem.RAM[file] = 10;
            mem.RAM[Constants.STATUS_B1] = 0;

            com.OperationService.RRF(file, d1);

            Assert.AreEqual(5, mem.RAM[file]);
        }

        [Test]
        public void RRF_Overflow_Wert()
        {
            mem.RAM[file] = 11;
            mem.RAM[Constants.STATUS_B1] = 0;

            com.OperationService.RRF(file, d1);

            Assert.AreEqual(5, mem.RAM[file]);
        }


        [Test]
        public void RRF_Overflow_Carry()
        {
            mem.RAM[file] = 11;
            mem.RAM[Constants.STATUS_B1] = 0;

            com.OperationService.RRF(file, d1);

            Assert.AreEqual(1, mem.RAM[Constants.STATUS_B1]);
        }

        [Test]
        public void SUBWF()
        {
            mem.RAM[file] = 3;
            mem.W_Reg = 2;

            com.OperationService.SUBWF(file, d1);

            Assert.AreEqual(1, mem.RAM[file]);
        }

        [Test]
        public void SUBWF_Overflow()
        {
            mem.RAM[file] = 10;
            mem.W_Reg = 20;

            com.OperationService.SUBWF(file, d1);

            Assert.AreEqual(246, mem.RAM[file]);
        }

        [Test]
        public void SWAPF()
        {
            mem.RAM[file] = 0x_A5;

            com.OperationService.SWAPF(file, d1);

            Assert.AreEqual(0x_5A, mem.RAM[file]);
        }

        [Test]
        public void XORWF()
        {
            mem.RAM[file] = 0x_AF;
            mem.W_Reg = 0x_b5;

            com.OperationService.XORWF(file, d1);

            Assert.AreEqual(0x_1A, mem.RAM[file]);
        }
    }
}