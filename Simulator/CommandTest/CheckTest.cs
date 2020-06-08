using Application.Model;
using Application.Services;
using NUnit.Framework;
using System;

namespace CommandTest
{
    public class CheckTests
    {
        Memory mem;
        CommandService com;
        SourceFileModel src;

        [SetUp]
        public void Setup()
        {
            mem = new Memory();
            src = new SourceFileModel();
            com = new CommandService(mem, src);
        }

        [Test]
        public void CheckZ_true()
        {
            int literal = 0;
            
            com.checkZ(mem, literal);

            int z = mem.RAM[Constants.STATUS_B1] & 0b_0000_0100;
            Assert.AreEqual(4, z);
        }

        [Test]
        public void CheckZ_false()
        {
            int literal = 3;

            com.checkZ(mem, literal);

            int z = mem.RAM[Constants.STATUS_B1] & 0b_0000_0100;
            Assert.AreEqual(0, z);
        }

        #region Check dc, c plus
        [Test]
        public void Check_DC_falsePlus()
        {
            int lit1 = 0;
            int lit2 = 6;

            com.check_DC_C(mem, lit1, lit2, "+");


            int dc = mem.RAM[Constants.STATUS_B1] & 0b_0000_0010;

            Assert.AreEqual(0, dc);
        }

        [Test]
        public void Check_C_falsePlus()
        {
            int lit1 = 0;
            int lit2 = 6;

            com.check_DC_C(mem, lit1, lit2, "+");

            int c = mem.RAM[Constants.STATUS_B1] & 0b_0000_0001;
            Assert.AreEqual(0, c);
        }

        [Test]
        public void Check_DC_truePlus()
        {
            int lit1 = 136;

            com.check_DC_C(mem, lit1, lit1, "+");


            int dc = mem.RAM[Constants.STATUS_B1] & 0b_0000_0010;

            Assert.AreEqual(2, dc);

        }

        [Test]
        public void Check_C_truePlus()
        {
            int lit1 = 136;

            com.check_DC_C(mem, lit1, lit1, "+");



            int c = mem.RAM[Constants.STATUS_B1] & 0b_0000_0001;

            Assert.AreEqual(1, c);
        }

        #endregion

        #region Check DC, C Minus

        [Test]
        public void Check_DC_falseMinus()
        {
            int lit1 = 6;
            int lit2 = 2;

            com.check_DC_C(mem, lit1, lit2, "-");

            int dc = mem.RAM[Constants.STATUS_B1] & 0b_0000_0010;

            //umgekehrte logik
            Assert.AreEqual(2, dc);
        }

        [Test]
        public void Check_C_falseMinus()
        {
            int lit1 = 6;
            int lit2 = 2;

            com.check_DC_C(mem, lit1, lit2, "-");

            int c = mem.RAM[Constants.STATUS_B1] & 0b_0000_0001;

            //umgekehrte logik
            Assert.AreEqual(1, c);
        }

        [Test]
        public void Check_DC_trueMinus()
        {
            int lit1 = 68;
            int lit2 = 136;

            com.check_DC_C(mem, lit1, lit2, "-");


            int dc = mem.RAM[Constants.STATUS_B1] & 0b_0000_0010;

            //umgekehrte logik
            Assert.AreEqual(0, dc);
        }

        [Test]
        public void Check_C_trueMinus()
        {
            int lit1 = 68;
            int lit2 = 136;

            com.check_DC_C(mem, lit1, lit2, "-");

            int c = mem.RAM[Constants.STATUS_B1] & 0b_0000_0001;

            //umgekehrte logik
            Assert.AreEqual(0, c);
        }

        #endregion

        [Test]
        public void TestBit_SkipClear_True()
        {
            int lit1 = 0;

            int result = com.bitTest(lit1, 0);

            Assert.AreEqual(2, result);
        }

        [Test]
        public void StoreSwitchedOnD_0()
        {
            int file = 0x_0f;
            int result = 15;
            int d = 0;

            com.StoreSwitchedOnD(mem, file, result, d);

            Assert.AreEqual(15, mem.W_Reg);
        }

        [Test]
        public void StoreSwitchedOnD_1()
        {
            int file = 0x_0f;
            int result = 15;
            int d = 1;

            com.StoreSwitchedOnD(mem, file, result, d);

            Assert.AreEqual(15, mem.RAM[file]);
        }
    }
}