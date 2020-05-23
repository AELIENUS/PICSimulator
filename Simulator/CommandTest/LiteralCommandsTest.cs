using Application.Model;
using Application.Services;
using Applicator.Services;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CommandTest
{
    class LiteralCommandsTest
    {
        Memory mem = new Memory();
        CommandService com = new CommandService();

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ADDLW()
        {
            int literal = 10;
            mem.W_Reg = 6;

            com.ADDLW(mem, literal);

            Assert.AreEqual(16, mem.W_Reg);
        }

        [Test]
        public void ADDLW_Overflow()
        {
            int literal = 250;
            mem.W_Reg = 10;

            com.ADDLW(mem, literal);

            Assert.AreEqual(4, mem.W_Reg);
        }

        [Test]
        public void AndLW()
        {
            int literal = 10;
            mem.W_Reg = 6;

            com.ANDLW(mem, literal);

            Assert.AreEqual(2, mem.W_Reg);
        }

        [Test]
        public void Call_Adress()
        {
            int address = 0x_0f;
 
            com.CALL(mem, address);

            Assert.AreEqual(0x_0f, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void Call_Stack()
        {
            int address = 0x_0f;
            int return_address = mem.RAM[Constants.PCL_B1] +1;
       
            com.CALL(mem, address);

            Assert.AreEqual(return_address, mem.PCStack.Pop());
        }

        [Test]
        public void Goto()
        {
            int address = 0x_0f;

            com.GOTO(mem, address);

            Assert.AreEqual(0x_0f, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void IORLW()
        {
            int literal = 10;
            mem.W_Reg = 6;

            com.IORLW(mem, literal);

            Assert.AreEqual(14, mem.W_Reg);
        }

        [Test]
        public void MovLW()
        {
            int literal = 10;
            mem.W_Reg = 6;

            com.MOVLW(mem, literal);

            Assert.AreEqual(10, mem.W_Reg);
        }

        [Test]
        public void RETFIE_Address()
        {
            mem.PCStack.Push(0x_0f);

            com.RETFIE(mem);

            Assert.AreEqual(0x_0f, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void RETFIE_GIE()
        {
            mem.PCStack.Push(0x_0f);

            com.RETFIE(mem);

            int GIE = mem.RAM[Constants.INTCON_B1] & 0b_1000_0000;
            Assert.AreEqual(128, GIE);
        }

        [Test]
        public void RETLW_Wreg()
        {
            mem.PCStack.Push(0x_0f);
            int literal = 10;

            com.RETLW(mem, literal);

            Assert.AreEqual(10, mem.W_Reg);
        }

        [Test]
        public void RETLW_Address()
        {
            mem.PCStack.Push(0x_0f);
            int literal = 10;

            com.RETLW(mem, literal);

            Assert.AreEqual(0x_0f, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void RETURN()
        {
            mem.PCStack.Push(0x_0f);

            com.RETURN(mem);

            Assert.AreEqual(0x_0f, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void Sleep_PD()
        {
            com.SLEEP(mem);

            int pd = mem.RAM[Constants.STATUS_B1] & 0b_0000_1000;
            Assert.AreEqual(0, pd);
        }

        [Test]
        public void Sleep_TO()
        {
            com.SLEEP(mem);

            int to = mem.RAM[Constants.STATUS_B1] & 0b_0001_0000;
            Assert.AreEqual(16, to);
        }

        [Test]
        public void SUBLW()
        {
            int literal = 10;
            mem.W_Reg = 6;

            com.SUBLW(mem, literal);

            Assert.AreEqual(4, mem.W_Reg);
        }

        [Test]
        public void SUBLW_Overflow()
        {
            int literal = 10;
            mem.W_Reg = 20;

            com.SUBLW(mem, literal);

            Assert.AreEqual(246, mem.W_Reg);
        }

        [Test]
        public void XORLW()
        {
            int literal = 10;
            mem.W_Reg = 6;

            com.XORLW(mem, literal);

            Assert.AreEqual(12, mem.W_Reg);
        }
    }
}
