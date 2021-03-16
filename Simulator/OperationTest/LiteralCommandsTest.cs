using Application.Model;
using Application.Services;
using System;
using System.Threading.Tasks;
using Moq;
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
            //we have to mock the Observable

            //the memory object is so basic that there is simply no point of mocking it, since it is much harder
            //to mock it than using the actual thing. That is, because you would have to set e.g. the PCStack
            //Property to return an object of type ObservableStack, which is just a stack, and this
            //is not possible since it is no non-overridable property/member.
            mem = new Memory(){ PCStack = new ObservableStack<short>()};
            //mock stack
            src = new SourceFileModel {SourceFile = ""};
            fil = new FileService();
            fil.CreateFileList(src);
            com = new ApplicationService(mem, src);
        }

        [TestMethod]
        public void ADDLW()
        {
            //arrange
            int literal = 10;
            mem.WReg = 6;

            //act
            com.OperationService.ADDLW(literal);

            //assert
            Assert.AreEqual(16, mem.WReg);
        }

        [TestMethod]
        public void ADDLW_Overflow()
        {
            int literal = 250;
            mem.WReg = 10;

            com.OperationService.ADDLW(literal);

            Assert.AreEqual(4, mem.WReg);
        }

        [TestMethod]
        public void ANDLW()
        {
            int literal = 10;
            mem.WReg = 6;

            com.OperationService.ANDLW(literal);

            Assert.AreEqual(2, mem.WReg);
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
            int return_address = mem.RAM[Constants.PCL_B1] + 1;

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
            mem.WReg = 6;

            com.OperationService.IORLW(literal);

            Assert.AreEqual(14, mem.WReg);
        }

        [TestMethod]
        public void MovLW()
        {
            int literal = 10;
            mem.WReg = 6;

            com.OperationService.MOVLW(literal);

            Assert.AreEqual(10, mem.WReg);
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

            Assert.AreEqual(10, mem.WReg);
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
            mem.WReg = 6;

            com.OperationService.SUBLW(literal);

            Assert.AreEqual(4, mem.WReg);
        }

        [TestMethod]
        public void SUBLW_Overflow()
        {
            int literal = 10;
            mem.WReg = 20;

            com.OperationService.SUBLW(literal);

            Assert.AreEqual(246, mem.WReg);
        }

        [TestMethod]
        public void XORLW()
        {
            int literal = 10;
            mem.WReg = 6;

            com.OperationService.XORLW(literal);

            Assert.AreEqual(12, mem.WReg);
        }
    }
}
