﻿using Application.Model;
using Application.Services;
using Applicator.Services;
using NUnit.Framework;
using System;
using System.IO;

namespace CommandTest
{
    class bitOrientedTest
    {
        Memory mem = new Memory();
        CommandService com = new CommandService();

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void BCF()
        {
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 3;

            com.BCF(mem, file, bit);

            Assert.AreEqual(2, mem.RAM[file]);
        }

        [Test]
        public void BSF()
        {
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 2;

            com.BSF(mem, file, bit);

            Assert.AreEqual(3, mem.RAM[file]);
        }

        [Test]
        public void BTFSC_NoSkip()
        {
            mem.RAM[Constants.PCL_B1] = 1;
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 3;

            com.BTFSC(mem, file, bit);

            Assert.AreEqual(2, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void BTFSC_Skip()
        {
            mem.RAM[Constants.PCL_B1] = 1;
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 2;

            com.BTFSC(mem, file, bit);

            Assert.AreEqual(3, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void BTFSS_NoSkip()
        {
            mem.RAM[Constants.PCL_B1] = 1;
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 3;

            com.BTFSS(mem, file, bit);

            Assert.AreEqual(3, mem.RAM[Constants.PCL_B1]);
        }

        [Test]
        public void BTFSS_Skip()
        {
            mem.RAM[Constants.PCL_B1] = 1;
            int file = 0x_0f;
            int bit = 0;
            mem.RAM[file] = 2;

            com.BTFSS(mem, file, bit);

            Assert.AreEqual(2, mem.RAM[Constants.PCL_B1]);
        }
    }
}