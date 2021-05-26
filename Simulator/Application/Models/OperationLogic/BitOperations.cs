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
            return BXF(file, bit, false);
        }

        public ResultInfo BSF(int file, int bit)//bit set f
        {
            return BXF(file, bit, true);
        }

        private ResultInfo BXF(int file, int bit, bool set)
        {
            int result;
            byte mask = 0b0000_0001;
            mask = (byte) (mask << bit);
            if (set)
            {
                result = _memory.RAM[file] | mask;
            }
            else
            {
                mask = (byte)~mask;
                result = _memory.RAM[file] & mask;
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
            summand = BTFSX(file, bit, 0);
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
            summand = BTFSX(file, bit, 1);
            return new ResultInfo()
            {
                PCIncrement = 1,
                Cycles = 1,
                BeginLoop = summand == 2,
            };
        }

        private int BTFSX(int file, int bit, int skipIf)
        {
            byte mask = 0b0000_0001;
            mask = (byte)(mask << bit);
            if (skipIf == 0)
            {
                if ((_memory.RAM[file] & mask) == 0)
                {
                    return 2;
                }
                return 1;
            }
            if ((_memory.RAM[file] & mask) == 0)
            {
                return 1;
            }
            return 2;
        }

    }
}