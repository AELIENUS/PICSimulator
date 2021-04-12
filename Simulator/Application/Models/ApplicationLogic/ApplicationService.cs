using System.Net.Mail;
using System.Threading;
using System.Windows.Interactivity;
using Application.Constants;
using Application.Models.CodeLogic;
using Application.Models.Memory;
using Application.Models.OperationLogic;

namespace Application.Models.ApplicationLogic
{
    public class ApplicationService
    {
        private MemoryService _memory;
        private SourceFileModel _srcModel;
        private short _command;
        private readonly OperationService _operationService;
        private OperationHelpers _operationHelpers;

        #region run
        public void Run ()
        {
            while (true)
            {
                BeginLoop();
                _command = _srcModel[_memory.RAM.PC_With_Clear].ProgramCode;
                Thread.Sleep(400);
                ResultInfo result = InvokeCommand(_command);
                //set current LoC to not executed
                _srcModel[_memory.RAM.PC_Without_Clear].IsExecuted = false;
                WriteToMemory(result);
            }
        }

        private void WriteToMemory(ResultInfo result)
        {
            if (result.JumpAddress != null)
                _operationHelpers.SetJumpAddress((int)result.JumpAddress);
            if (result.ClearISR)
                _memory.IsISR = false;
            if (result.OverflowInfo!=null && result.OperationResults.Count == 1)
                result.OperationResults[0].Value = _operationHelpers.Check_DC_C(result.OverflowInfo, result.OperationResults[0].Value);
            if (result.CheckZ && result.OperationResults.Count == 1)
                //Result is never null in this case
                _operationHelpers.CheckZ((int)result.OperationResults[0].Value);
            if (result.Cycles!=null)
                _operationHelpers.UpdateCycles((int)result.Cycles);
            if (result.OperationResults!=null)
                _operationHelpers.WriteOperationResults(result.OperationResults);
            if (result.PCIncrement != null)
                _operationHelpers.ChangePC_Fetch((int)result.PCIncrement);
            if (result.BeginLoop)
            {
                BeginLoop();
                _srcModel[_memory.RAM.PC_Without_Clear].IsExecuted = false;
                WriteToMemory(_operationService.NOP());
            }
        }

        private ResultInfo InvokeCommand(short command)
        {
            switch (_command)
            {
                case 0b_0000_0000_0000_1000:
                    return _operationService.RETURN(); //Return from Subroutine
                case 0b_0000_0000_0000_1001:
                    return _operationService.RETFIE(); //return from interrupt
                case 0b_0000_0000_0110_0011:
                    return _operationService.SLEEP(); //Go to standby mode
                case 0b_0000_0000_0110_0100:
                    return _operationService.CLRWDT(); //clear watchdog timer
                case 0b_0000_0000_0000_0000: // ab hier nop
                case 0b_0000_0000_0010_0000:
                case 0b_0000_0000_0100_0000:
                case 0b_0000_0000_0110_0000:
                    return _operationService.NOP(); //no operation 
                case short n when (n >= 0b_0000_0001_0000_0000 && n <= 0b_0000_0001_0111_1111):
                    return _operationService.CLRW(); //clear w
                default:
                    return AnalyzeNibble3();
            }
        }


        private ResultInfo AnalyzeNibble3()
        {
            int nibble3 = (int)_command & 0b_0011_0000_0000_0000; //bitoperation nur auf int ausführbar
            switch (nibble3)
            {
                case 0b_0010_0000_0000_0000:
                    int bit12 = (int)_command & 0b_0000_1000_0000_0000;
                    int address = (int)_command & 0b_0000_0111_1111_1111;
                    if (bit12 == 0b_0000_1000_0000_0000)
                    {
                        return _operationService.GOTO(address);
                    }
                    else
                    {
                        return _operationService.CALL(address);
                    }
                case 0b_0001_0000_0000_0000: //bit oriented operations
                    return AnalyzeBits11_12();
                case 0b_0011_0000_0000_0000: //literal operations
                    return AnalyzeNibble2Literal();
                case 0b_0000_0000_0000_0000: //byte oriented operations
                    return AnalyzeNibble2Byte();
                default:
                    return null;
            }
        }

        private ResultInfo AnalyzeBits11_12() //bit-oriented operations genauer analysieren
        {
            int bits11_12 = ((int)_command & 0b_0000_1100_0000_0000) >> 10;
            int bits = ((int)_command & 0b_0000_0011_1000_0000) >> 7;
            int file = (int)_command & 0b_0000_0000_0111_1111;
            if (file == 0x02) //wenn PCL beschrieben wird
            {
                _memory.RAM.PCL_was_Manipulated = true;
            }
            switch (bits11_12)
            {
                case 0:
                    return _operationService.BCF(file, bits);
                case 1:
                    return _operationService.BSF(file, bits);
                case 2:
                    return _operationService.BTFSC(file, bits);
                case 3:
                    return _operationService.BTFSS(file, bits);
                default:
                    return null;
            }
        }
    
        private ResultInfo AnalyzeNibble2Literal() 
        {
            int nibble2 = (int)_command & 0b_0000_1111_0000_0000;
            int literal = (int)_command & 0b_0000_0000_1111_1111;
            switch (nibble2)
            {
                case 0b_1001_0000_0000:
                    return _operationService.ANDLW(literal);
                case 0b_1000_0000_0000:
                    return _operationService.IORLW(literal);
                case 0b_1010_0000_0000:
                    return _operationService.XORLW(literal);
                case 0b_1110_0000_0000: //ab hier addlw
                case 0b_1111_0000_0000:
                    return _operationService.ADDLW(literal);
                case 0b_1100_0000_0000: //ab hier sublw
                case 0b_1101_0000_0000:
                    return _operationService.SUBLW(literal);
                case 0b_0000_0000_0000: //ab hier movlw
                case 0b_0001_0000_0000:
                case 0b_0010_0000_0000:
                case 0b_0011_0000_0000:
                    return _operationService.MOVLW(literal);
                case 0b_0100_0000_0000: //ab hier retlw
                case 0b_0101_0000_0000:
                case 0b_0110_0000_0000:
                case 0b_0111_0000_0000:
                    return _operationService.RETLW(literal);
                default:
                    return null;
            }
        }

        private ResultInfo AnalyzeNibble2Byte()
        {
            int nibble2 = (int)_command & 0b_0000_1111_0000_0000;
            int file = (int)_command & 0b_0000_0000_0111_1111;
            int d = ((int)_command & 0b_0000_0000_1000_0000)>>7;
            switch (nibble2)
            {
                case 0b_0000_0000_0000_0000:
                    return _operationService.MOVWF(file);
                case 0b_0000_0001_0000_0000:
                    return _operationService.CLRF(file);
                case 0b_0000_0010_0000_0000:
                    return _operationService.SUBWF(file, d);
                case 0b_0000_0011_0000_0000:
                    return _operationService.DECF(file, d);
                case 0b_0000_0100_0000_0000:
                    return _operationService.IORWF(file, d);
                case 0b_0000_0101_0000_0000:
                    return _operationService.ANDWF(file, d);
                case 0b_0000_0110_0000_0000:
                    return _operationService.XORWF(file, d);
                case 0b_0000_0111_0000_0000:
                    return _operationService.ADDWF(file, d);
                case 0b_0000_1000_0000_0000:
                    return _operationService.MOVF(file, d);
                case 0b_0000_1001_0000_0000:
                    return _operationService.COMF(file, d);
                case 0b_0000_1010_0000_0000:
                    return _operationService.INCF(file, d);
                case 0b_0000_1011_0000_0000:
                    return _operationService.DECFSZ(file, d);
                case 0b_0000_1100_0000_0000:
                    return _operationService.RRF(file, d);
                case 0b_0000_1101_0000_0000:
                    return _operationService.RLF(file, d);
                case 0b_0000_1110_0000_0000:
                    return _operationService.SWAPF(file, d);
                case 0b_0000_1111_0000_0000:
                    return _operationService.INCFSZ(file, d);
                default:
                    return null;
            }
        }
        #endregion

        #region Interrupts

        private void CheckForInterrupts() //INTCON DA MAIN MAN
        {
            //GIE gesetzt?
            if((_memory.RAM[MemoryConstants.INTCON_B1] & 0b1000_0000)>0)
            {
                //GIE gesetzt
                //T0IE gesetzt?
                if ((_memory.RAM[MemoryConstants.INTCON_B1] & 0b0010_0000) > 0)
                {
                    //T0IE gesetzt
                    CheckForTimer0Interrupt();
                }
                //EEIE gesetzt?
                if ((_memory.RAM[MemoryConstants.INTCON_B1] & 0b0100_0000) > 0)
                {
                    //EEIE gesetzt
                    CheckForEEPROMWrittenInterrupt();
                }
                //INTE gesetzt?
                if ((_memory.RAM[MemoryConstants.INTCON_B1] & 0b0001_0000) > 0)
                {
                    //INTE gesetzt
                    CheckForRB0Interrupt();
                }
                //RBIE gesetzt
                if ((_memory.RAM[MemoryConstants.INTCON_B1] & 0b0000_1000) > 0)
                {
                    //RBIE gesetzt
                    CheckForPortBInterrupt();
                }
            }
        }

        private void CheckForTimer0Interrupt()
        {
            //T0IF gesetzt
            if((_memory.RAM[MemoryConstants.INTCON_B1] & 0b0000_0100) > 0)
            {
                Interrupt();
            }
        }

        private void CheckForEEPROMWrittenInterrupt()
        {

        }

        private void CheckForRB0Interrupt()
        {
            //INTF Interrupt Flag gesetzt?
            if((_memory.RAM[MemoryConstants.INTCON_B1] & 0b0000_0010) >0)
            {
                Interrupt();
            }
        }

        private void CheckForPortBInterrupt()
        {
            if ((_memory.RAM[MemoryConstants.INTCON_B1] & 0b0000_0001) > 0)
            {
                Interrupt();
            }
        }

        private void Interrupt()
        {
            //Push PC to Stack
            _memory.PCStack.Push((short)_memory.RAM.PC_Without_Clear);
            //Set PC to Interrupt Vector
            _memory.RAM[MemoryConstants.PCL_B1] = (byte)MemoryConstants.PERIPHERAL_INTERRUPT_VECTOR_ADDRESS;
            _memory.IsISR = true;
        }
        #endregion

        public void BeginLoop()
        {
            if (_memory.IsISR == false)
            {
                CheckForInterrupts();
            }
            if (_memory.RAM.PC_Without_Clear == 0x7ff)
            {
                _memory.RAM[MemoryConstants.PCL_B1] = 0;
                _memory.RAM[MemoryConstants.PCLATH_B1] = 0;
            }
            _srcModel[_memory.RAM.PC_Without_Clear].IsExecuted = true;
            while (true)
            {
                if (DebugCodes.Pause || _srcModel[_memory.RAM.PC_Without_Clear].IsDebug)
                {
                    //loop forever
                }
                else
                {
                    break;
                }
            }
        }
    

        public ApplicationService(MemoryService memory, SourceFileModel srcModel, OperationHelpers operationHelpers, OperationService operationService)
        {
            this._memory = memory;
            this._srcModel = srcModel;
            _operationService = operationService;
            _operationHelpers = operationHelpers;
        }

    }
}