using System;
using System.Collections.Generic;
using Application.Constants;
using Application.Models.CodeLogic;
using Application.Models.Memory;

namespace Application.Models.OperationLogic
{
    public class OperationHelpers
    {
        private MemoryService _memory;
        private SourceFileModel _srcModel;

        public OperationHelpers(MemoryService memory, SourceFileModel srcModel)
        {
            _memory = memory;
            _srcModel = srcModel;
        }

        public void CheckZ(int result)
        {
            byte wert;
            if (result == 0)
            {
                wert = Convert.ToByte(_memory.RAM[MemoryConstants.STATUS_B1] | 0b_0000_0100);
            }
            else
            {
                wert = Convert.ToByte(_memory.RAM[MemoryConstants.STATUS_B1] & 0b_1111_1011);
            }

            _memory.RAM[MemoryConstants.STATUS_B1] = wert;
        }

        public int Check_DC_C(OverflowInfo parameters, int operationResult)
        {
            int literal1Low = parameters.Operand1 & 0b_0000_1111;
            int literal2Low = parameters.Operand2 & 0b_0000_1111;
            byte value;
            int modifiedResult = operationResult;
            if (parameters.Operator == "+")
            {
                if ((literal1Low + literal2Low) > 15) //DigitCarry prüfen
                {
                    value = Convert.ToByte(_memory.RAM[MemoryConstants.STATUS_B1] | 0b_0000_0010);
                }
                else
                {
                    value = Convert.ToByte(_memory.RAM[MemoryConstants.STATUS_B1] & 0b_1111_1101);
                }

                _memory.RAM[MemoryConstants.STATUS_B1] = value;

                if ((parameters.Operand1 + parameters.Operand2) > 255) // Carry prüfen
                {
                    value = Convert.ToByte(_memory.RAM[MemoryConstants.STATUS_B1] | 0b_0000_0001);
                    modifiedResult = operationResult - 256;
                }
                else
                {
                    value = Convert.ToByte(_memory.RAM[MemoryConstants.STATUS_B1] & 0b_1111_1110);
                }
            }

            else //op = "-"
            {
                //hier hat der pic umgekehrte Logik, da die Entwickler ein invertieren vergessen haben (siehe Themenblatt)
                if ((literal1Low - literal2Low) < 0) // DigitCarry prüfen
                {
                    value = Convert.ToByte(_memory.RAM[MemoryConstants.STATUS_B1] & 0b_1111_1101);
                }
                else
                {
                    value = Convert.ToByte(_memory.RAM[MemoryConstants.STATUS_B1] | 0b_0000_0010);
                }

                _memory.RAM[MemoryConstants.STATUS_B1] = value;

                if ((parameters.Operand1 - parameters.Operand2) < 0) //Carry prüfen
                {
                    value = Convert.ToByte(_memory.RAM[MemoryConstants.STATUS_B1] & 0b_1111_1110);
                    modifiedResult = operationResult + 256;
                }
                else
                {
                    value = Convert.ToByte(_memory.RAM[MemoryConstants.STATUS_B1] | 0b_0000_0001);
                }
            }
            _memory.RAM[MemoryConstants.STATUS_B1] = value;
            return modifiedResult;
        }

        public void ChangePC_Fetch(int value)
        {
            if (_memory.RAM[MemoryConstants.PCL_B1] == 0b_1111_1111)
            {
                _memory.RAM[MemoryConstants.PCL_B1] = (byte)(0 + value);
                //PCLATH erhöhen
                _memory.RAM[MemoryConstants.PCLATH_B1] += 1;
            }
            else
            {
                _memory.RAM[MemoryConstants.PCL_B1] += (byte)value;
            }
        }

        public void UpdateCycles(int cyclesToAdd)
        {
            while (cyclesToAdd > 0)
            {
                //CycleCounter is incremented by 1 every time, the setter is called
                _memory.CycleCounter += 1;
                cyclesToAdd -= 1;
            }
            
        }

        public void WriteOperationResults(List<OperationResult> operationResults)
        {
            foreach (var operationResult in operationResults)
            {
                if (operationResult.Address == MemoryConstants.WRegPlaceholder)
                    _memory.WReg = (short)operationResult.Value;
                else
                    _memory.RAM[operationResult.Address] = (byte)operationResult.Value;
            }
        }

        public void SetJumpAddress(int jumpAddress)
        {
            _memory.RAM.PC_was_Jump = true;
            _memory.RAM.PC_JumpAdress = jumpAddress;
        }
    }
}