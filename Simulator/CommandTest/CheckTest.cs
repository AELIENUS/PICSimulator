using Application.Model;
using Application.Services;
using Applicator.Services;
using NUnit.Framework;
using System;

namespace CommandTest
{
    public class CheckTests
    {
        Memory mem = new Memory();
        CommandService com = new CommandService();

        [SetUp]
        public void Setup()
        {

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
    }
}