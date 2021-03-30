using System;
using System.Collections.Generic;
using System.Threading;
using Application.Constants;
using Application.Models.CodeLogic;
using Application.Models.Memory;

namespace Application.Models.OperationLogic
{
    public class OperationService
    {
        private MemoryService _memory;
        private SourceFileModel _srcModel;

        public OperationService(MemoryService memory, SourceFileModel srcModel)
        {
            _memory = memory;
            _srcModel = srcModel;
        }

        #region byte-oriented file register operations

        public ResultInfo ADDWF(int file, int d) //add w and f 
        {
            int result = _memory.RAM[file] + _memory.WReg;
            return new ResultInfo()
            {
                OverflowInfo = new OverflowInfo() { Operand1 = _memory.RAM[file], Operand2 = _memory.WReg, Operator = "+" },
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
            };
        }

        public ResultInfo ANDWF(int file, int d) //and w and f
        {
            int result = _memory.WReg & _memory.RAM[file];
            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
            };
        }

        public ResultInfo CLRF(int file) //clear f
        {
            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = 0, Address = file}
            },
            };
        }

        public ResultInfo CLRW() // clear w
        {
            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = 0, Address = MemoryConstants.WRegPlaceholder}
            },
            };
        }

        public ResultInfo COMF(int file, int d) //complement f
        {
            int result = _memory.RAM[file] ^ 0b_1111_1111;
            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
            };
        }

        public ResultInfo DECF(int file, int d) //Decrement f
        {
            int result = _memory.RAM[file] - 1;
            //DC and C flag are not affected by this operation, but an underflow can still be expected.
            if (result < 0)
            {
                result = result + 256;
            }
            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
            };
        }

        public ResultInfo DECFSZ(int file, int d) //Decrement f, Skip if 0
        {
            int result = _memory.RAM[file] - 1;
            //DC and C flag are not affected by this operation, but an underflow can still be expected.
            if (result < 0)
            {
                result = result + 256;
            }
            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
                BeginLoop = !Convert.ToBoolean(result)
            };
        }

        public ResultInfo INCF(int file, int d) //increment f
        {
            int result = _memory.RAM[file] + 1;
            //DC and C flag are not affected by this operation, but an overflow can still be expected.
            if (result > 255)
            {
                result = result - 256;
            }
            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
            };
        }

        public ResultInfo INCFSZ(int file, int d) //increment f, skip if zero
        {
            int result = _memory.RAM[file] + 1;
            //DC and C flag are not affected by this operation, but an overflow can still be expected.
            if (result > 255)
            {
                result = result - 256;
            }
            return new ResultInfo()
            {
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
                BeginLoop = !Convert.ToBoolean(result)
            };
        }

        public ResultInfo IORWF(int file, int d) // inclusive OR w with f
        {
            int result = (int)_memory.WReg | (int)_memory.RAM[file];
            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
            };
        }

        public ResultInfo MOVF(int file, int d) // move f
        {
            int result = _memory.RAM[file];
            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
            };
        }

        public ResultInfo MOVWF(int file) // move w to f
        {
            return new ResultInfo()
            {
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = _memory.WReg, Address = file}
            },
            };
        }

        public ResultInfo NOP() // no operation
        {
            return new ResultInfo() { PCIncrement = 1, Cycles = 1 };
        }

        public ResultInfo RLF(int file, int d) // rotate left through carry
        {
            int result = _memory.RAM[file] << 1;
            int carry = _memory.RAM[MemoryConstants.STATUS_B1] & 0b_0000_0001;
            result += carry;
            return new ResultInfo()
            {
                OverflowInfo = new OverflowInfo() { Operand1 = result, Operand2 = 0, Operator = "+" },
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
            };
        }

        public ResultInfo RRF(int file, int d) // rotate right through carry
        {
            int newCarry = _memory.RAM[file] & 0b_0000_0001;
            int result = _memory.RAM[file] >> 1;
            int carry = _memory.RAM[MemoryConstants.STATUS_B1] & 0b_0000_0001;
            int operationResult = result;

            if (carry == 1)
            {
                operationResult = result + 128;
            }
            return new ResultInfo()
            {
                OverflowInfo = new OverflowInfo() { Operand1 = operationResult, Operand2 = Convert.ToBoolean(newCarry) ? 256 : 0, Operator = "+" },
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = operationResult, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
            };
        }

        public ResultInfo SUBWF(int file, int d) // substract w from f
        {
            int result = _memory.RAM[file] - _memory.WReg; // literal in f ändern, auf d prüfen
            return new ResultInfo()
            {
                CheckZ = true,
                OverflowInfo = new OverflowInfo() { Operand1 = _memory.RAM[file], Operand2 = _memory.WReg, Operator = "-" },
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
            };
        }

        public ResultInfo SWAPF(int file, int d) // swap nibbles in f
        {
            int newUpper = (_memory.RAM[file] << 4) & 0b_1111_0000;
            int newLower = (_memory.RAM[file] >> 4) & 0b_0000_1111;
            int result = newLower + newUpper;
            return new ResultInfo()
            {
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
            };
        }

        public ResultInfo XORWF(int file, int d) //exclusive OR w with f
        {
            int result = _memory.WReg ^ _memory.RAM[file];
            return new ResultInfo()
            {
                CheckZ = true,
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = Convert.ToBoolean(d) ? file : MemoryConstants.WRegPlaceholder}
            },
            };
        }
        #endregion byte-oriented file register operations

        #region bit-oriented file register operations
        public ResultInfo BCF(int file, int bit) //bit clear f
        {
            int result;
            switch (bit)
            {
                case 0:
                    result = _memory.RAM[file] & 0b_1111_1110;
                    break;
                case 1:
                    result = _memory.RAM[file] & 0b_1111_1101;
                    break;
                case 2:
                    result = _memory.RAM[file] & 0b_1111_1011;
                    break;
                case 3:
                    result = _memory.RAM[file] & 0b_1111_0111;
                    break;
                case 4:
                    result = _memory.RAM[file] & 0b_1110_1111;
                    break;
                case 5:
                    result = _memory.RAM[file] & 0b_1101_1111;
                    break;
                case 6:
                    result = _memory.RAM[file] & 0b_1011_1111;
                    break;
                default: // bit = 7
                    result = _memory.RAM[file] & 0b_0111_1111;
                    break;
            }
            return new ResultInfo()
            {
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = file}
            },
            };
        }

        public ResultInfo BSF(int file, int bit)//bit set f
        {
            int result;
            switch (bit)
            {
                case 0:
                    result = _memory.RAM[file] | 0b_0000_0001;
                    break;
                case 1:
                    result = _memory.RAM[file] | 0b_0000_0010;
                    break;
                case 2:
                    result = _memory.RAM[file] | 0b_0000_0100;
                    break;
                case 3:
                    result = _memory.RAM[file] | 0b_0000_1000;
                    break;
                case 4:
                    result = _memory.RAM[file] | 0b_0001_0000;
                    break;
                case 5:
                    result = _memory.RAM[file] | 0b_0010_0000;
                    break;
                case 6:
                    result = _memory.RAM[file] | 0b_0100_0000;
                    break;
                default: // bit = 7
                    result = _memory.RAM[file] | 0b_1000_0000;
                    break;
            }
            return new ResultInfo()
            {
                PCIncrement = 1,
                Cycles = 1,
                OperationResults = new List<OperationResult>()
            {
                new OperationResult()
                    {Value = result, Address = file}
            },
            };
        }

        public ResultInfo BTFSC(int file, int bit) //bit test f, skip if clear
        {
            int summand;
            switch (bit)
            {
                case 0:
                    summand = BitTest(_memory.RAM[file] & 0b_0000_0001, 0);
                    break;
                case 1:
                    summand = BitTest(_memory.RAM[file] & 0b_0000_0010, 0);
                    break;
                case 2:
                    summand = BitTest(_memory.RAM[file] & 0b_0000_0100, 0);
                    break;
                case 3:
                    summand = BitTest(_memory.RAM[file] & 0b_0000_1000, 0);
                    break;
                case 4:
                    summand = BitTest(_memory.RAM[file] & 0b_0001_0000, 0);
                    break;
                case 5:
                    summand = BitTest(_memory.RAM[file] & 0b_0010_0000, 0);
                    break;
                case 6:
                    summand = BitTest(_memory.RAM[file] & 0b_0100_0000, 0);
                    break;
                default: // bit = 7
                    summand = BitTest(_memory.RAM[file] & 0b_1000_0000, 0);
                    break;
            }
            return new ResultInfo()
            {
                PCIncrement = 1,
                Cycles = 1,
                BeginLoop = summand == 2,
            };
        }

        public ResultInfo BTFSS(int file, int bit) //bit test f, skip if set
        {
            int summand;
            switch (bit)
            {
                case 0:
                    summand = BitTest(_memory.RAM[file] & 0b_0000_0001, 1);
                    break;
                case 1:
                    summand = BitTest(_memory.RAM[file] & 0b_0000_0010, 1);
                    break;
                case 2:
                    summand = BitTest(_memory.RAM[file] & 0b_0000_0100, 1);
                    break;
                case 3:
                    summand = BitTest(_memory.RAM[file] & 0b_0000_1000, 1);
                    break;
                case 4:
                    summand = BitTest(_memory.RAM[file] & 0b_0001_0000, 1);
                    break;
                case 5:
                    summand = BitTest(_memory.RAM[file] & 0b_0010_0000, 1);
                    break;
                case 6:
                    summand = BitTest(_memory.RAM[file] & 0b_0100_0000, 1);
                    break;
                default: // bit = 7
                    summand = BitTest(_memory.RAM[file] & 0b_1000_0000, 1);
                    break;
            }
            return new ResultInfo()
            {
                PCIncrement = 1,
                Cycles = 1,
                BeginLoop = summand == 2,
            };
        }

        #endregion

        #region literal and control operations
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

        public ResultInfo ANDLW(int literal) //and literal and w -> fertig
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
            int returnAdress = _memory.RAM.RAMList[0].Byte2.Value + (_memory.RAM.RAMList[0].Byte10.Value << 8) + 1;
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
            int result = (int)_memory.WReg | literal;

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

        public ResultInfo MOVLW(int literal) //move literal to w -> fertig
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

        public ResultInfo RETFIE() //return from interrupt -> fertig
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

        public ResultInfo RETLW(int literal) //return with literal in w -> fertig
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

        public ResultInfo SUBLW(int literal) // subtract w from literal -> fertig
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

        public ResultInfo XORLW(int literal) //exclusive OR literal with w -> fertig
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
        #endregion

        private int BitTest(int content, int skip)
        { // wenn skip = 0 soll bei clear geskipped werden, ist skip = 1, soll bei set geskipped werden
            if (skip == 0)
            {
                if (content == 0)
                {
                    return 2;
                }
                return 1;
            }
            if (content == 0)
            {
                return 1;
            }
            return 2;
        }

    }
}
