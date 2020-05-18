using Application.Model;
using Application.Services;
using Applicator.Services;
using NUnit.Framework;
using System;

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

            Assert.AreEqual(5, mem.W_Reg);
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
            int act_address = mem.RAM[Constants.PCL_B1];
       
            com.CALL(mem, address);

            //stacktest
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
    }
}
