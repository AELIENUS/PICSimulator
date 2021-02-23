using System;
using System.Threading;
using Application.Services;
using Application.Model;

public class OperationService
{
    private ApplicationService _commandService;
    private readonly OperationHelpers _operationHelpers;
    private Memory _memory;
    private SourceFileModel _srcModel;

    public OperationService(ApplicationService applicationService, Memory memory, SourceFileModel srcModel)
    {
        _commandService = applicationService;
        _memory = memory;
        _srcModel = srcModel;
        _operationHelpers = new OperationHelpers(_memory, _srcModel);
    }

    public OperationHelpers OperationHelpers
    {
        get { return _operationHelpers; }
    }

    public ApplicationService ApplicationService
    {
        set { _commandService = value; }
        get { return _commandService; }
    }

    #region byte-oriented file register operations
    public void ADDWF(int file, int d) //add w and f 
    {
        int result = _memory.RAM[file] + _memory.W_Reg;

        if (result > 255)
        {
            result = result - 256;
        }

        OperationHelpers.check_DC_C(_memory.RAM[file], _memory.W_Reg, "+");
        OperationHelpers.checkZ(result);

        OperationHelpers.StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void ANDWF (int file, int d) //and w and f
    {
        int result = _memory.W_Reg & _memory.RAM[file];

        OperationHelpers.checkZ(result);
        OperationHelpers.StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void CLRF (int file) //clear f
    {
        _memory.RAM[file] = 0;

        // z flag setzen
        OperationHelpers.checkZ(0);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void CLRW() // clear w
    {
        _memory.W_Reg = 0;
        // z flag setzen
        OperationHelpers.checkZ(0);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void COMF(int file, int d) //complement f
    {
        int result = _memory.RAM[file] ^ 0b_1111_1111;

        OperationHelpers.checkZ(result);
        OperationHelpers.StoreSwitchedOnD(file, result, d);
        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void DECF(int file, int d) //Decrement f
    {
        int result = _memory.RAM[file] - 1;

        if (result < 0)
        {
            result = result + 256;
        }

        OperationHelpers.checkZ(result);
        OperationHelpers.StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void DECFSZ(int file, int d) //Decrement f, Skip if 0
    {
        int result = _memory.RAM[file] - 1;
        if (result < 0)
        {
            result = result + 256;
        }

        OperationHelpers.StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;

        if (result == 0) //bei überspringen ein nop ausführen (damit breakpunkte etc funktionieren, muss der erste teil der Run-while schleife ausgeführt werden
        {
            _commandService.BeginLoop();
            NOP();
        }
    }

    public void INCF (int file, int d) //increment f
    {
        int result = _memory.RAM[file] + 1;
        if (result>255)
        {
            result = result - 256;
        }

        OperationHelpers.checkZ(result);
        OperationHelpers.StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void INCFSZ(int file, int d) //increment f, skip if zero
    {
        int result = _memory.RAM[file] + 1;
        if (result > 255)
        {
            result = result - 256;
        }

        OperationHelpers.StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;

        if (result == 0) //bei überspringen ein nop ausführen (damit breakpunkte etc funktionieren, muss der erste teil der Run-while schleife ausgeführt werden
        {
            _commandService.BeginLoop();
            NOP();
        }
    }

    public void IORWF (int file, int d) // inclusive OR w with f
    {
        int result = (int) _memory.W_Reg | (int) _memory.RAM[file];

        OperationHelpers.checkZ(result);

        OperationHelpers.StoreSwitchedOnD(file, result, d);
        
        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void MOVF (int file, int d) // move f
    {
        int result = _memory.RAM[file];

        OperationHelpers.checkZ(result);

        OperationHelpers.StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void MOVWF (int file) // move w to f
    {
        _memory.RAM[file] = Convert.ToByte(_memory.W_Reg);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void NOP () // no operation
    {
        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void RLF (int file, int d) // rotate left through carry
    {
        int result = _memory.RAM[file] << 1;
        int carry = _memory.RAM[Constants.STATUS_B1] & 0b_0000_0001;

        result += carry;

        if (result > 255)
        {
            result = result - 256;
            //carry setzen
            _memory.RAM[Constants.STATUS_B1] |= 0b_0000_0001; 
        }
        else
        {
            // carry löschen
            _memory.RAM[Constants.STATUS_B1] &= 0b_1111_1110;
        }

        OperationHelpers.StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void RRF (int file, int d) // rotate right through carry
    {
        int newCarry = _memory.RAM[file] & 0b_0000_0001;
        int result = _memory.RAM[file] >> 1;
        int carry = _memory.RAM[Constants.STATUS_B1] & 0b_0000_0001;

        if (carry == 1)
        {
            result = result + 128;
        }
        if (newCarry == 1)
        {
            //carry setzen
            _memory.RAM[Constants.STATUS_B1] |= 0b_0000_0001;
        }
        else
        {
            // carry löschen
            _memory.RAM[Constants.STATUS_B1] &= 0b_1111_1110;
        }

        OperationHelpers.StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void SUBWF (int file, int d) // substract w from f
    {
        int result = _memory.RAM[file] - _memory.W_Reg; // literal in f ändern, auf d prüfen

        if (result < 0)
        {
            result = result + 256;
        }

        OperationHelpers.check_DC_C(_memory.RAM[file], _memory.W_Reg, "-");
        OperationHelpers.checkZ(result);

        OperationHelpers.StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void SWAPF(int file, int d) // swap nibbles in f
    {
        int newUpper = (_memory.RAM[file] << 4) & 0b_1111_0000;
        int newLower = (_memory.RAM[file] >> 4) & 0b_0000_1111;

        int result  = newLower + newUpper;

        OperationHelpers.StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void XORWF (int file, int d) //exclusive OR w with f
    {

        int result = _memory.W_Reg ^ _memory.RAM[file];

        OperationHelpers.checkZ(result);

        OperationHelpers.StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    #endregion byte-oriented file register operations

    #region bit-oriented file register operations

    public void BCF (int file, int bit) //bit clear f
    {
        switch (bit)
        {
            case 0:
                _memory.RAM[file] &= 0b_1111_1110;
                break;
            case 1:
                _memory.RAM[file] &= 0b_1111_1101;
                break;
            case 2:
                _memory.RAM[file] &= 0b_1111_1011;
                break;
            case 3:
                _memory.RAM[file] &= 0b_1111_0111;
                break;
            case 4:
                _memory.RAM[file] &= 0b_1110_1111;
                break;
            case 5:
                _memory.RAM[file] &= 0b_1101_1111;
                break;
            case 6:
                _memory.RAM[file] &= 0b_1011_1111;
                break;
            default: // bit = 7
                _memory.RAM[file] &= 0b_0111_1111;
                break;
        }

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);
        //cycles: 1
        _memory.CycleCounter++;
    }

    public void BSF (int file, int bit)//bit set f
    {
        switch (bit)
        {
            case 0:
                _memory.RAM[file] |= 0b_0000_0001;
                break;
            case 1:
                _memory.RAM[file] |= 0b_0000_0010;
                break;
            case 2:
                _memory.RAM[file] |= 0b_0000_0100;
                break;
            case 3:
                _memory.RAM[file] |= 0b_0000_1000;
                break;
            case 4:
                _memory.RAM[file] |= 0b_0001_0000;
                break;
            case 5:
                _memory.RAM[file] |= 0b_0010_0000;
                break;
            case 6:
                _memory.RAM[file] |= 0b_0100_0000;
                break;
            default: // bit = 7
                _memory.RAM[file] |= 0b_1000_0000;
                break;
        }

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);
        //cycles: 1
        _memory.CycleCounter++;
    }

    public void BTFSC (int file, int bit) //bit test f, skip if clear
    {
        int summand;
        switch (bit)
        {
            case 0:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0000_0001, 0);
                break;
            case 1:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0000_0010, 0);
                break;
            case 2:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0000_0100, 0);
                break;
            case 3:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0000_1000, 0);
                break;
            case 4:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0001_0000, 0);
                break;
            case 5:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0010_0000, 0);
                break;
            case 6:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0100_0000, 0);
                break;
            default: // bit = 7
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_1000_0000, 0);
                break;
        }
        //PC hochzählen
        OperationHelpers.ChangePC_Fetch((byte)summand);

        //cycles: 1
        _memory.CycleCounter++;

        if (summand == 2) //bei überspringen ein nop ausführen (damit breakpunkte etc funktionieren, muss der erste teil der Run-while schleife ausgeführt werden
        {
            _commandService.BeginLoop();
            //weiterer cycle (theoretisch im nop)
            _memory.CycleCounter++;
        }
    }

    public void BTFSS (int file, int bit) //bit test f, skip if set
    {
        int summand;
        switch (bit)
        {
            case 0:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0000_0001, 1);
                break;
            case 1:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0000_0010, 1);
                break;
            case 2:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0000_0100, 1);
                break;
            case 3:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0000_1000, 1);
                break;
            case 4:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0001_0000, 1);
                break;
            case 5:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0010_0000, 1);
                break;
            case 6:
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_0100_0000, 1);
                break;
            default: // bit = 7
                summand = OperationHelpers.bitTest(_memory.RAM[file] & 0b_1000_0000, 1);
                break;
        }
        //PC hochzählen
        OperationHelpers.ChangePC_Fetch((byte)summand);

        //cycles: 1
        _memory.CycleCounter++;

        if (summand == 2) //bei überspringen ein nop ausführen (damit breakpunkte etc funktionieren, muss der erste teil der Run-while schleife ausgeführt werden
        {
            _commandService.BeginLoop();
            //weiterer cycle (theoretisch im nop)
            _memory.CycleCounter++;
        }
    }

    #endregion

    #region literal and control operations
    public void ADDLW (int literal) //add literal and w -> fertig
    {
        int result = literal + _memory.W_Reg;

        if (result > 255)
        {
            result = result -256;
        }


        OperationHelpers.check_DC_C(literal, _memory.W_Reg, "+");
        OperationHelpers.checkZ(result);

        _memory.W_Reg = (short)result;

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void ANDLW(int literal) //and literal and w -> fertig
    {
    
        int result;

        result = _memory.W_Reg & literal;

        OperationHelpers.checkZ(result);
        _memory.W_Reg = (short)result;


        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void CALL(int address) // call subroutine -> fertig
    {
        //ganzen 13 bit PC auf Stack pushen
        int returnAdress = _memory.RAM.RAMList[0].Byte2.Value + (_memory.RAM.RAMList[0].Byte10.Value << 8) +1;
        _memory.PCStack.Push((short)returnAdress);

        //cycles: 2, aber beide in GOTO
        
        GOTO(address);
    }

    public void CLRWDT() //clear watchdog timer -> wdt fehlt
    {
        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);
        //cycles: 1
        _memory.CycleCounter++;
    }

    public void GOTO (int address) //go to address -> fertig
    {
        //bit 0-10 aus address, bit 11-12 aus PCLATH <3,4>
        int new_pc = address + ((_memory.RAM[Constants.PCLATH_B1] & 0b_0001_1000) << 8);

        //execution löschen
        _srcModel[_memory.RAM.PC_Without_Clear].IsExecuted = false;

        //PCL setzen
        int newPCL = address & 0b_0111_1111;
        _memory.RAM[Constants.PCL_B1] = Convert.ToByte(newPCL);

        //cycles: 2
        _memory.CycleCounter++;
        _memory.CycleCounter++;

        _memory.RAM.PC_was_Jump = true;
        _memory.RAM.PC_JumpAdress = new_pc;
    }

    public void IORLW (int literal) //inclusive OR literal with w   -> fertig
    {
        int result;

        result = (int) _memory.W_Reg | literal;

        OperationHelpers.checkZ (result);
        _memory.W_Reg = (short)result;

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void MOVLW (int literal) //move literal to w -> fertig
    {
        _memory.W_Reg = Convert.ToByte(literal);
        ///PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void RETFIE() //return from interrupt -> fertig
    {
        _srcModel[_memory.RAM.PC_Without_Clear].IsExecuted = false;

        //top of stack in PC
        int returnAddress = _memory.PCStack.Pop();
        returnAddress &= 0b_1111_1111;
        _memory.RAM[Constants.PCL_B1] = Convert.ToByte(returnAddress);

        //GlobalInterruptEnable setzen
        int wert = _memory.RAM[Constants.INTCON_B1] | 0b_1000_0000;
        _memory.RAM[Constants.INTCON_B1] = Convert.ToByte(wert); 

        //cycles: 2
        _memory.CycleCounter++;
        _memory.CycleCounter++;
        _memory.IsISR = false;
    }

    public void RETLW (int literal) //return with literal in w -> fertig
    {
        _memory.W_Reg = Convert.ToByte(literal);

        _srcModel[_memory.RAM.PC_Without_Clear].IsExecuted = false;

        // top of stack in PC
        int returnAddress = _memory.PCStack.Pop();
        returnAddress &= 0b_1111_1111;
        _memory.RAM[Constants.PCL_B1]= Convert.ToByte(returnAddress);

        //cycles: 2
        _memory.CycleCounter++;
        _memory.CycleCounter++;
    }

    public void RETURN() //return from subroutine -> fertig
    {
        _srcModel[_memory.RAM.PC_Without_Clear].IsExecuted = false;
        //top of stack in PC
        int returnAddress = _memory.PCStack.Pop();
        returnAddress &= 0b_1111_1111;
        _memory.RAM[Constants.PCL_B1] = Convert.ToByte(returnAddress);

        //cycles: 2
        _memory.CycleCounter++;
        _memory.CycleCounter++;

    }

    public void SLEEP() // go into standby mode -> wdt fehlt, sleep modus
    {
        //clear power down status
        int PD = Convert.ToByte(_memory.RAM[Constants.STATUS_B1] & 0b_1111_0111);
        _memory.RAM[Constants.STATUS_B1] = Convert.ToByte(PD);

        //set time out status
        int TO = Convert.ToByte(_memory.RAM[Constants.STATUS_B1] | 0b_0001_0000);
        _memory.RAM[Constants.STATUS_B1] = Convert.ToByte(TO);

        //clear Watchdog and prescaler

        //processor in sleep mode, oscilatoor stopped,  page 14.8

        //wenn watchdog aktiviert: fpr 2,3sec schlafen
        Thread.Sleep(2300);

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void SUBLW(int literal) // subtract w from literal -> fertig
    {
        int result = literal - _memory.W_Reg;
        OperationHelpers.check_DC_C(literal, _memory.W_Reg, "-");
        OperationHelpers.checkZ(result);

        if (result < 0)
        {
            result = result + 256;
        }

        _memory.W_Reg = (short)result;

        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    public void XORLW (int literal) //exclusive OR literal with w -> fertig
    {
        int result;

        result = _memory.W_Reg ^ literal;

        OperationHelpers.checkZ(result);
        _memory.W_Reg = (short)result;
        
        //PC hochzählen
        OperationHelpers.ChangePC_Fetch(1);

        //cycles: 1
        _memory.CycleCounter++;
    }

    #endregion
}