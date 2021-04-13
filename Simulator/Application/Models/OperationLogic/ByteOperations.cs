using System;
using System.Collections.Generic;
using Application.Constants;
using Application.Models.Memory;

namespace Application.Models.OperationLogic
{
    public class ByteOperations
    {
        private MemoryService _memory;

        public ByteOperations(MemoryService memory)
        {
            _memory = memory;
        }

        public MemoryService Memory
        {
            set { _memory = value; }
            get { return _memory; }
        }

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
                result -= 256;
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
    }
}