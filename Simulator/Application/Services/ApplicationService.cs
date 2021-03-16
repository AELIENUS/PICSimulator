using System.Threading;
using System.Windows.Interactivity;
using Application.Model;
using Applicator.Model;

namespace Application.Services
{
    public class ApplicationService
    {
        private Memory _memory;
        private SourceFileModel _srcModel;
        private short _command;
        private readonly OperationService _operationService;

        public OperationService OperationService
        {
            get { return _operationService; }
        }

        public Memory Memory
        {
            set { _memory = value; }
            get { return _memory; }
        }

        public SourceFileModel SrcModel
        {
            set { _srcModel = value; }
            get { return _srcModel; }
        }

        #region run
        public void Run ()
        {
            while (true)
            {
                BeginLoop();
                _command = _srcModel[_memory.RAM.PC_With_Clear].ProgramCode;
                Thread.Sleep(200);
                InvokeCommand(_command);

            }
        }

        private void InvokeCommand(short command)
        {
            switch (_command)
            {
                case 0b_0000_0000_0000_1000:
                    OperationService.RETURN(); //Return from Subroutine
                    break;
                case 0b_0000_0000_0000_1001:
                    OperationService.RETFIE(); //return from interrupt
                    break;
                case 0b_0000_0000_0110_0011:
                    OperationService.SLEEP(); //Go to standby mode
                    break;
                case 0b_0000_0000_0110_0100:
                    OperationService.CLRWDT(); //clear watchdog timer
                    break;
                case 0b_0000_0000_0000_0000: // ab hier nop
                case 0b_0000_0000_0010_0000:
                case 0b_0000_0000_0100_0000:
                case 0b_0000_0000_0110_0000:
                    OperationService.NOP(); //no operation 
                    break;
                case short n when (n >= 0b_0000_0001_0000_0000 && n <= 0b_0000_0001_0111_1111):
                    OperationService.CLRW(); //clear w
                    break;
                default:
                    AnalyzeNibble3();
                    break;
            }
        }


        private void AnalyzeNibble3()
        {
            int nibble3 = (int)_command & 0b_0011_0000_0000_0000; //bitoperation nur auf int ausführbar
            switch (nibble3)
            {
                case 0b_0010_0000_0000_0000:
                    int bit12 = (int)_command & 0b_0000_1000_0000_0000;
                    int address = (int)_command & 0b_0000_0111_1111_1111;
                    if (bit12 == 0b_0000_1000_0000_0000)
                    {
                        OperationService.GOTO(address);
                    }
                    else
                    {
                        OperationService.CALL(address);
                    }
                    break;
                case 0b_0001_0000_0000_0000: //bit oriented operations
                    AnalyzeBits11_12();
                    break;
                case 0b_0011_0000_0000_0000: //literal operations
                    AnalyzeNibble2Literal();
                    break;
                case 0b_0000_0000_0000_0000: //byte oriented operations
                    AnalyzeNibble2Byte();
                    break;
                default:
                    break;
            }
        }

        private void AnalyzeBits11_12() //bit-oriented operations genauer analysieren
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
                    OperationService.BCF(file, bits);
                    break;
                case 1:
                    OperationService.BSF(file, bits);
                    break;
                case 2:
                    OperationService.BTFSC(file, bits);
                    break;
                case 3:
                    OperationService.BTFSS(file, bits);
                    break;
                default:
                    break;

            }
        }
    
        private void AnalyzeNibble2Literal() 
        {
            int nibble2 = (int)_command & 0b_0000_1111_0000_0000;
            int literal = (int)_command & 0b_0000_0000_1111_1111;
            switch (nibble2)
            {
                case 0b_1001_0000_0000:
                    OperationService.ANDLW(literal);
                    break;
                case 0b_1000_0000_0000:
                    OperationService.IORLW(literal);
                    break;
                case 0b_1010_0000_0000:
                    OperationService.XORLW(literal);
                    break;
                case 0b_1110_0000_0000: //ab hier addlw
                case 0b_1111_0000_0000:
                    OperationService.ADDLW(literal); 
                    break;
                case 0b_1100_0000_0000: //ab hier sublw
                case 0b_1101_0000_0000:
                    OperationService.SUBLW(literal);
                    break;
                case 0b_0000_0000_0000: //ab hier movlw
                case 0b_0001_0000_0000:
                case 0b_0010_0000_0000:
                case 0b_0011_0000_0000:
                    OperationService.MOVLW(literal);
                    break;
                case 0b_0100_0000_0000: //ab hier retlw
                case 0b_0101_0000_0000:
                case 0b_0110_0000_0000:
                case 0b_0111_0000_0000:
                    OperationService.RETLW(literal);
                    break;
                default:
                    break;
            }
        }

        private void AnalyzeNibble2Byte()
        {
            int nibble2 = (int)_command & 0b_0000_1111_0000_0000;
            int file = (int)_command & 0b_0000_0000_0111_1111;
            int d = ((int)_command & 0b_0000_0000_1000_0000)>>7;
            switch (nibble2)
            {
                case 0b_0000_0000_0000_0000:
                    OperationService.MOVWF(file);
                    break;
                case 0b_0000_0001_0000_0000:
                    OperationService.CLRF(file); 
                    break;
                case 0b_0000_0010_0000_0000:
                    OperationService.SUBWF(file, d);
                    break;
                case 0b_0000_0011_0000_0000:
                    OperationService.DECF(file, d);
                    break;
                case 0b_0000_0100_0000_0000:
                    OperationService.IORWF(file, d);
                    break;
                case 0b_0000_0101_0000_0000:
                    OperationService.ANDWF(file, d);
                    break;
                case 0b_0000_0110_0000_0000:
                    OperationService.XORWF(file, d);
                    break;
                case 0b_0000_0111_0000_0000:
                    OperationService.ADDWF(file, d);
                    break;
                case 0b_0000_1000_0000_0000:
                    OperationService.MOVF(file, d);
                    break;
                case 0b_0000_1001_0000_0000:
                    OperationService.COMF(file, d);
                    break;
                case 0b_0000_1010_0000_0000:
                    OperationService.INCF(file, d);
                    break;
                case 0b_0000_1011_0000_0000:
                    OperationService.DECFSZ(file, d);
                    break;
                case 0b_0000_1100_0000_0000:
                    OperationService.RRF(file, d);
                    break;
                case 0b_0000_1101_0000_0000:
                    OperationService.RLF(file, d);
                    break;
                case 0b_0000_1110_0000_0000:
                    OperationService.SWAPF(file, d);
                    break;
                case 0b_0000_1111_0000_0000:
                    OperationService.INCFSZ(file, d);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Interrupts

        private void CheckForInterrupts() //INTCON DA MAIN MAN
        {
            //GIE gesetzt?
            if((_memory.RAM[Constants.INTCON_B1] & 0b1000_0000)>0)
            {
                //GIE gesetzt
                //T0IE gesetzt?
                if ((_memory.RAM[Constants.INTCON_B1] & 0b0010_0000) > 0)
                {
                    //T0IE gesetzt
                    CheckForTimer0Interrupt();
                }
                //EEIE gesetzt?
                if ((_memory.RAM[Constants.INTCON_B1] & 0b0100_0000) > 0)
                {
                    //EEIE gesetzt
                    CheckForEEPROMWrittenInterrupt();
                }
                //INTE gesetzt?
                if ((_memory.RAM[Constants.INTCON_B1] & 0b0001_0000) > 0)
                {
                    //INTE gesetzt
                    CheckForRB0Interrupt();
                }
                //RBIE gesetzt
                if ((_memory.RAM[Constants.INTCON_B1] & 0b0000_1000) > 0)
                {
                    //RBIE gesetzt
                    CheckForPortBInterrupt();
                }
            }
        }

        private void CheckForTimer0Interrupt()
        {
            //T0IF gesetzt
            if((_memory.RAM[Constants.INTCON_B1] & 0b0000_0100) > 0)
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
            if((_memory.RAM[Constants.INTCON_B1] & 0b0000_0010) >0)
            {
                Interrupt();
            }
        }

        private void CheckForPortBInterrupt()
        {
            if ((_memory.RAM[Constants.INTCON_B1] & 0b0000_0001) > 0)
            {
                Interrupt();
            }
        }

        private void Interrupt()
        {
            //Push PC to Stack
            _memory.PCStack.Push((short)_memory.RAM.PC_Without_Clear);
            //Set PC to Interrupt Vector
            _memory.RAM[Constants.PCL_B1] = (byte)Constants.PERIPHERAL_INTERRUPT_VECTOR_ADDRESS;
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
                _memory.RAM[Constants.PCL_B1] = 0;
                _memory.RAM[Constants.PCLATH_B1] = 0;
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
    

        public ApplicationService(Memory memory, SourceFileModel srcModel)
        {
            this._memory = memory;
            this._srcModel = srcModel;
            /// diese sehr schlecht, da das Objekt selbst übergeben wird
            _operationService = new OperationService(this, _memory, _srcModel);
        }

        public ApplicationService()
        {
            _operationService = new OperationService(this, _memory, _srcModel);
        }

    }
}