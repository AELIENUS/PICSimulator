﻿using System.Net.Mail;
using System.Threading;
using System.Windows.Interactivity;
using System.Windows.Shapes;
using Application.Constants;
using Application.Models.CodeLogic;
using Application.Models.Memory;
using Application.Models.OperationLogic;

namespace Application.Models.ApplicationLogic
{
    public class ApplicationService : IApplicationService
    {
        private IMemoryService _memory;
        private ISourceFileModel<ILineOfCode> _srcModel;
        private short _command;
        private readonly BitOperations _bitOperations;
        private readonly ByteOperations _byteOperations;
        private readonly LiteralControlOperations _literalControlOperations;
        private OperationHelpers _operationHelpers;

        #region run
        public void Run ()
        {
            while (true)
            {
                BeginLoop();
                _command = _srcModel[_memory.RAM.PCWithClear].ProgramCode;
                Thread.Sleep(400);
                ResultInfo result = InvokeCommand();
                //set current LoC to not executed
                _srcModel[_memory.RAM.PCWithoutClear].IsExecuted = false;
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
                _srcModel[_memory.RAM.PCWithoutClear].IsExecuted = false;
                WriteToMemory(_byteOperations.NOP());
            }
        }

        private ResultInfo InvokeCommand()
        {
            switch (_command)
            {
                case 0b_0000_0000_0000_1000:
                    return _literalControlOperations.RETURN(); //Return from Subroutine
                case 0b_0000_0000_0000_1001:
                    return _literalControlOperations.RETFIE(); //return from interrupt
                case 0b_0000_0000_0110_0011:
                    return _literalControlOperations.SLEEP(); //Go to standby mode
                case 0b_0000_0000_0110_0100:
                    return _literalControlOperations.CLRWDT(); //clear watchdog timer
                case 0b_0000_0000_0000_0000: // ab hier nop
                case 0b_0000_0000_0010_0000:
                case 0b_0000_0000_0100_0000:
                case 0b_0000_0000_0110_0000:
                    return _byteOperations.NOP(); //no operation 
                case short n when (n >= 0b_0000_0001_0000_0000 && n <= 0b_0000_0001_0111_1111):
                    return _byteOperations.CLRW(); //clear w
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
                        return _literalControlOperations.GOTO(address);
                    }
                    else
                    {
                        return _literalControlOperations.CALL(address);
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
                _memory.RAM.PCLWasManipulated = true;
            }
            switch (bits11_12)
            {
                case 0:
                    return _bitOperations.BCF(file, bits);
                case 1:
                    return _bitOperations.BSF(file, bits);
                case 2:
                    return _bitOperations.BTFSC(file, bits);
                case 3:
                    return _bitOperations.BTFSS(file, bits);
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
                    return _literalControlOperations.ANDLW(literal);
                case 0b_1000_0000_0000:
                    return _literalControlOperations.IORLW(literal);
                case 0b_1010_0000_0000:
                    return _literalControlOperations.XORLW(literal);
                case 0b_1110_0000_0000: //ab hier addlw
                case 0b_1111_0000_0000:
                    return _literalControlOperations.ADDLW(literal);
                case 0b_1100_0000_0000: //ab hier sublw
                case 0b_1101_0000_0000:
                    return _literalControlOperations.SUBLW(literal);
                case 0b_0000_0000_0000: //ab hier movlw
                case 0b_0001_0000_0000:
                case 0b_0010_0000_0000:
                case 0b_0011_0000_0000:
                    return _literalControlOperations.MOVLW(literal);
                case 0b_0100_0000_0000: //ab hier retlw
                case 0b_0101_0000_0000:
                case 0b_0110_0000_0000:
                case 0b_0111_0000_0000:
                    return _literalControlOperations.RETLW(literal);
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
                    return _byteOperations.MOVWF(file);
                case 0b_0000_0001_0000_0000:
                    return _byteOperations.CLRF(file);
                case 0b_0000_0010_0000_0000:
                    return _byteOperations.SUBWF(file, d);
                case 0b_0000_0011_0000_0000:
                    return _byteOperations.DECF(file, d);
                case 0b_0000_0100_0000_0000:
                    return _byteOperations.IORWF(file, d);
                case 0b_0000_0101_0000_0000:
                    return _byteOperations.ANDWF(file, d);
                case 0b_0000_0110_0000_0000:
                    return _byteOperations.XORWF(file, d);
                case 0b_0000_0111_0000_0000:
                    return _byteOperations.ADDWF(file, d);
                case 0b_0000_1000_0000_0000:
                    return _byteOperations.MOVF(file, d);
                case 0b_0000_1001_0000_0000:
                    return _byteOperations.COMF(file, d);
                case 0b_0000_1010_0000_0000:
                    return _byteOperations.INCF(file, d);
                case 0b_0000_1011_0000_0000:
                    return _byteOperations.DECFSZ(file, d);
                case 0b_0000_1100_0000_0000:
                    return _byteOperations.RRF(file, d);
                case 0b_0000_1101_0000_0000:
                    return _byteOperations.RLF(file, d);
                case 0b_0000_1110_0000_0000:
                    return _byteOperations.SWAPF(file, d);
                case 0b_0000_1111_0000_0000:
                    return _byteOperations.INCFSZ(file, d);
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
            _memory.PCStack.Push((short)_memory.RAM.PCWithoutClear);
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
            if (_memory.RAM.PCWithoutClear == 0x7ff)
            {
                _memory.RAM[MemoryConstants.PCL_B1] = 0;
                _memory.RAM[MemoryConstants.PCLATH_B1] = 0;
            }
            _srcModel[_memory.RAM.PCWithoutClear].IsExecuted = true;
            while (true)
            {
                if (DebugCodes.Pause || _srcModel[_memory.RAM.PCWithoutClear].IsDebug)
                {
                    //loop forever
                }
                else
                {
                    break;
                }
            }
        }
    

        public ApplicationService(IMemoryService memory, ISourceFileModel<ILineOfCode> srcModel, OperationHelpers operationHelpers, BitOperations bitOperations, ByteOperations byteOperations, LiteralControlOperations literalControlOperations)
        {
            this._memory = memory;
            this._srcModel = srcModel;
            _bitOperations = bitOperations;
            _byteOperations = byteOperations;
            _literalControlOperations = literalControlOperations;
            _operationHelpers = operationHelpers;
        }

    }
}