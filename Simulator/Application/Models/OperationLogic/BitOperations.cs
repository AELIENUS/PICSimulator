using System.Collections.Generic;
using Application.Models.Memory;

namespace Application.Models.OperationLogic
{
    public class BitOperations
    {
        private IMemoryService _memory;

        public BitOperations(IMemoryService memory)
        {
            _memory = memory;
        }

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

        private int BitTest(int content, int skipIf)
        { // wenn skipIf = 0 soll bei clear geskipped werden, ist skipIf = 1, soll bei set geskipped werden
            if (skipIf == 0)
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