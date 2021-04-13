using System;
using System.Collections.Generic;
using System.Threading;
using Application.Constants;
using Application.Models.Memory;

namespace Application.Models.OperationLogic
{
    public class LiteralControlOperations
    {
        private MemoryService _memory;

        public LiteralControlOperations(MemoryService memory)
        {
            _memory = memory;
        }

        public ResultInfo ADDLW(int literal) //add literal and w
        {
            int result = literal + _memory.WReg;
            return new ResultInfo()
            {
                CheckZ = true,
                OverflowInfo = new OverflowInfo() { Operand1 = literal, Operand2 = _memory.WReg, Operator = "+" },
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
                {
                    new OperationResult()
                        {Value = result, Address = MemoryConstants.WRegPlaceholder}
                },
            };
        }

        public ResultInfo ANDLW(int literal) //and literal and w 
        {
            int result = _memory.WReg & literal;
            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
                {
                    new OperationResult()
                        {Value = result, Address = MemoryConstants.WRegPlaceholder}
                },
            };
        }

        public ResultInfo CALL(int address) // call subroutine
        {
            //push whole PC to stack
            int returnAdress = _memory.RAM[2] + (_memory.RAM[10] << 8) + 1;
            _memory.PCStack.Push((short)returnAdress);
            return GOTO(address);
        }

        public ResultInfo CLRWDT() //clear watchdog timer -> wdt is not implemented
        {
            return new ResultInfo()
            {
                PCIncrement = 1,
                Cycles = 1,
            };
        }

        public ResultInfo GOTO(int address) //go to address
        {
            //bit 0-10 aus address, bit 11-12 aus PCLATH <3,4>
            int newPC = address + ((_memory.RAM[MemoryConstants.PCLATH_B1] & 0b_0001_1000) << 8);
            //Calculate PCL 
            int newPCL = address & 0b_0111_1111;
            return new ResultInfo()
            {
                JumpAddress = newPC,
                Cycles = 2,
                OperationResults = new List<OperationResult>()
                {
                    new OperationResult()
                        {Value = newPCL, Address = MemoryConstants.PCL_B1}
                },
            };
        }

        public ResultInfo IORLW(int literal) //inclusive OR literal with w
        {
            int result = (int) _memory.WReg | literal;

            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
                {
                    new OperationResult()
                        {Value = result, Address = MemoryConstants.WRegPlaceholder}
                },
            };
        }

        public ResultInfo MOVLW(int literal) //move literal to w 
        {
            int result = Convert.ToByte(literal);
            return new ResultInfo()
            {
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
                {
                    new OperationResult()
                        {Value = result, Address = MemoryConstants.WRegPlaceholder}
                },
            };
        }

        public ResultInfo RETFIE() //return from interrupt 
        {
            //top of stack in PC
            int returnAddress = _memory.PCStack.Pop();
            returnAddress &= 0b_1111_1111;
            //calculate GlobalInterruptEnable
            int value = _memory.RAM[MemoryConstants.INTCON_B1] | 0b_1000_0000;
            return new ResultInfo()
            {
                ClearISR = true,
                Cycles = 2,
                OperationResults = new List<OperationResult>()
                {
                    new OperationResult()
                        {Value = returnAddress, Address = MemoryConstants.PCL_B1},
                    new OperationResult()
                        {Value = value, Address = MemoryConstants.INTCON_B1}
                },
            };
        }

        public ResultInfo RETLW(int literal) //return with literal in w 
        {
            // top of stack in PC
            int returnAddress = _memory.PCStack.Pop();
            returnAddress &= 0b_1111_1111;
            return new ResultInfo()
            {
                Cycles = 2,
                OperationResults = new List<OperationResult>()
                {
                    new OperationResult()
                        {Value = literal, Address = MemoryConstants.WRegPlaceholder},
                    new OperationResult()
                        {Value = returnAddress, Address = MemoryConstants.PCL_B1}
                },
            };
        }

        public ResultInfo RETURN() //return from subroutine
        {
            //top of stack in PC
            int returnAddress = _memory.PCStack.Pop();
            returnAddress &= 0b_1111_1111;

            return new ResultInfo()
            {
                Cycles = 2,
                OperationResults = new List<OperationResult>()
                {
                    new OperationResult()
                        {Value = returnAddress, Address = MemoryConstants.PCL_B1}
                },
            };
        }

        /// <summary>
        /// Not fully implemented
        /// </summary>
        public ResultInfo SLEEP() // go into standby mode 
        {
            //clear power down status
            int PD = Convert.ToByte(_memory.RAM[MemoryConstants.STATUS_B1] & 0b_1111_0111);
            //set time out status
            int result = Convert.ToByte(PD | 0b_0001_0000);
            //clear Watchdog and prescaler
            //processor in sleep mode, oscilatoor stopped,  page 14.8
            //wenn watchdog aktiviert: fpr 2,3sec schlafen
            Thread.Sleep(2300);
            return new ResultInfo()
            {
                Cycles = 1,
                PCIncrement = 1,
                OperationResults = new List<OperationResult>()
                {
                    new OperationResult()
                        {Value = result, Address = MemoryConstants.STATUS_B1},
                },
            };
        }

        public ResultInfo SUBLW(int literal) // subtract w from literal 
        {
            int result = literal - _memory.WReg;
            return new ResultInfo()
            {
                OverflowInfo = new OverflowInfo() { Operand1 = literal, Operand2 = _memory.WReg, Operator = "-" },
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
                {
                    new OperationResult()
                        {Value = result, Address = MemoryConstants.WRegPlaceholder}
                },
            };
        }

        public ResultInfo XORLW(int literal) //exclusive OR literal with w 
        {
            int result = _memory.WReg ^ literal;
            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
                {
                    new OperationResult()
                        {Value = result, Address = MemoryConstants.WRegPlaceholder}
                },
            };
        }
    }
}