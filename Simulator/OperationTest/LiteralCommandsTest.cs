using Application.Model;
using Application.Services;
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OperationTest
{
    [TestClass]
    public class LiteralCommandsTest
    {
        Memory mem;
        ApplicationService com;
        SourceFileModel src;
        FileService fil;

        [TestInitialize]
        public void Setup()
        {
            mem = new Memory();
            src = new SourceFileModel();
            src.SourceFile = "";
            fil = new FileService();
            fil.CreateFileList(src);
            com = new ApplicationService(mem, src);
        }

        [TestMethod]
        public void ADDLW()
        {
            int literal = 10;
            mem.W_Reg = 6;

            com.OperationService.ADDLW(literal);

            Assert.AreEqual(16, mem.W_Reg);
        }

        [TestMethod]
        public void ADDLW_Overflow()
        {
            int literal = 250;
            mem.W_Reg = 10;

            com.OperationService.ADDLW(literal);

            Assert.AreEqual(4, mem.W_Reg);
        }

        [TestMethod]
        public void AndLW()
        {
            int literal = 10;
            mem.W_Reg = 6;

            com.OperationService.ANDLW(literal);

            Assert.AreEqual(2, mem.W_Reg);
        }

        [TestMethod]
        public void Call_Adress()
        {
            int address = 0x_0f;
 
            com.OperationService.CALL(address);

            Assert.AreEqual(0x_0f, mem.RAM[Constants.PCL_B1]);
        }

        [TestMethod]
        public void Call_Stack()
        {
            int address = 0x_0f;
            int return_address = mem.RAM[Constants.PCL_B1] +1;
       
            com.OperationService.CALL(address);

            Assert.AreEqual(return_address, mem.PCStack.Pop());
        }

        [TestMethod]
        public void Goto()
        {
            int address = 0x_0f;

            com.OperationService.GOTO(address);

            Assert.AreEqual(0x_0f, mem.RAM[Constants.PCL_B1]);
        }

        [TestMethod]
        public void IORLW()
        {
            int literal = 10;
            mem.W_Reg = 6;

            com.OperationService.IORLW(literal);

            Assert.AreEqual(14, mem.W_Reg);
        }

        [TestMethod]
        public void MovLW()
        {
            int literal = 10;
            mem.W_Reg = 6;

            com.OperationService.MOVLW(literal);

            Assert.AreEqual(10, mem.W_Reg);
        }

        [TestMethod]
        public void RETFIE_Address()
        {
            mem.PCStack.Push(0x_0f);

            com.OperationService.RETFIE();

            Assert.AreEqual(0x_0f, mem.RAM[Constants.PCL_B1]);
        }

        [TestMethod]
        public void RETFIE_GIE()
        {
            mem.PCStack.Push(0x_0f);

            com.OperationService.RETFIE();

            int GIE = mem.RAM[Constants.INTCON_B1] & 0b_1000_0000;
            Assert.AreEqual(128, GIE);
        }

        [TestMethod]
        public void RETLW_Wreg()
        {
            mem.PCStack.Push(0x_0f);
            int literal = 10;

            com.OperationService.RETLW(literal);

            Assert.AreEqual(10, mem.W_Reg);
        }

        [TestMethod]
        public void RETLW_Address()
        {
            mem.PCStack.Push(0x_0f);
            int literal = 10;

            com.OperationService.RETLW(literal);

            Assert.AreEqual(0x_0f, mem.RAM[Constants.PCL_B1]);
        }

        [TestMethod]
        public void RETURN()
        {
            mem.PCStack.Push(0x_0f);

            com.OperationService.RETURN();

            Assert.AreEqual(0x_0f, mem.RAM[Constants.PCL_B1]);
        }

        [TestMethod]
        public void Sleep_PD()
        {
            com.OperationService.SLEEP();

            int pd = mem.RAM[Constants.STATUS_B1] & 0b_0000_1000;
            Assert.AreEqual(0, pd);
        }

        [TestMethod]
        public void Sleep_TO()
        {
            com.OperationService.SLEEP();

            int to = mem.RAM[Constants.STATUS_B1] & 0b_0001_0000;
            Assert.AreEqual(16, to);
        }

        [TestMethod]
        public void SUBLW()
        {
            int literal = 10;
            mem.W_Reg = 6;

            com.OperationService.SUBLW(literal);

            Assert.AreEqual(4, mem.W_Reg);
        }

        [TestMethod]
        public void SUBLW_Overflow()
        {
            int literal = 10;
            mem.W_Reg = 20;

            com.OperationService.SUBLW(literal);

            Assert.AreEqual(246, mem.W_Reg);
        }

        [TestMethod]
        public void XORLW()
        {
            int literal = 10;
            mem.W_Reg = 6;

            com.OperationService.XORLW(literal);

            Assert.AreEqual(12, mem.W_Reg);
        }
    }
}
