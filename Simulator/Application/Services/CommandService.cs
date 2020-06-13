using Application.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Application.ViewModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows;
using Applicator.Model;
using System.Threading;
using System.ComponentModel;
using Application.Services;

//todo: methoden wieder private!!!!!!!!! (für tests public gestellt)

public class CommandService : ICommandService
{
    private Memory memory;
    private SourceFileModel SrcModel;
    private 
    short befehl;

    #region byte-oriented file register operations
    public void ADDWF(int file, int d) //add w and f 
    {
        int result = memory.RAM[file] + memory.W_Reg;

        if (result > 255)
        {
            result = result - 256;
        }

        check_DC_C(memory.RAM[file], memory.W_Reg, "+");
        checkZ(result);

        StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }
    
    public void ANDWF (int file, int d) //and w and f
    {
        int result = memory.W_Reg & memory.RAM[file];

        checkZ(result);
        StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void CLRF (int file) //clear f
    {
        memory.RAM[file] = 0;

        // z flag setzen
        checkZ(0);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void CLRW() // clear w
    {
        memory.W_Reg = 0;
        // z flag setzen
        checkZ(0);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void COMF(int file, int d) //complement f
    {
        int result = memory.RAM[file] ^ 0b_1111_1111;

        checkZ(result);
        StoreSwitchedOnD(file, result, d);
        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void DECF(int file, int d) //Decrement f
    {
        int result = memory.RAM[file] - 1;

        if (result < 0)
        {
            result = result + 256;
        }
        checkZ(result);
        StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void DECFSZ(int file, int d) //Decrement f, Skip if 0
    {
        int result = memory.RAM[file] - 1;
        if (result < 0)
        {
            result = result + 256;
        }
        StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;

        if (result == 0) //bei überspringen ein nop ausführen (damit breakpunkte etc funktionieren, muss der erste teil der Run-while schleife ausgeführt werden
        {
            BeginLoop();
            NOP();
        }
    }

    public void INCF (int file, int d) //increment f
    {
        int result = memory.RAM[file] + 1;
        if (result>255)
        {
            result = result - 256;
        }
        checkZ(result);
        StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void INCFSZ(int file, int d) //increment f, skip if zero
    {
        int result = memory.RAM[file] + 1;
        if (result > 255)
        {
            result = result - 256;
        }
        StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;

        if (result == 0) //bei überspringen ein nop ausführen (damit breakpunkte etc funktionieren, muss der erste teil der Run-while schleife ausgeführt werden
        {
            BeginLoop();
            NOP();
        }
    }

    public void IORWF (int file, int d) // inclusive OR w with f
    {
        int result = (int)memory.W_Reg | (int)memory.RAM[file];

        checkZ(result);

        StoreSwitchedOnD(file, result, d);
        
        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void MOVF (int file, int d) // move f
    {
        int result = memory.RAM[file];

        checkZ(result);

        StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void MOVWF (int file) // move w to f
    {
        memory.RAM[file] = Convert.ToByte(memory.W_Reg);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void NOP () // no operation
    {
        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void RLF (int file, int d) // rotate left through carry
    {
        int result = memory.RAM[file] << 1;
        int carry = memory.RAM[Constants.STATUS_B1] & 0b_0000_0001;

        result += carry;

        if (result > 255)
        {
            result = result - 256;
            //carry setzen
            memory.RAM[Constants.STATUS_B1] |= 0b_0000_0001; 
        }
        else
        {
            // carry löschen
            memory.RAM[Constants.STATUS_B1] &= 0b_1111_1110;
        }
        // carry für GUI updaten
        memory.C_Flag = (short)(memory.RAM[Constants.STATUS_B1] & 0b_0000_0001); //Wert für GUI updaten

        StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void RRF (int file, int d) // rotate right through carry
    {
        int newCarry = memory.RAM[file] & 0b_0000_0001;
        int result = memory.RAM[file] >> 1;
        int carry = memory.RAM[Constants.STATUS_B1] & 0b_0000_0001;

        if (carry == 1)
        {
            result = result + 128;
        }
        if (newCarry == 1)
        {
            //carry setzen
            memory.RAM[Constants.STATUS_B1] |= 0b_0000_0001;
        }
        else
        {
            // carry löschen
            memory.RAM[Constants.STATUS_B1] &= 0b_1111_1110;
        }
        // carry für GUI updaten
        memory.C_Flag = (short)(memory.RAM[Constants.STATUS_B1] & 0b_0000_0001); //Wert für GUI updaten

        StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void SUBWF (int file, int d) // substract w from f
    {
        int result = memory.RAM[file] - memory.W_Reg; // literal in f ändern, auf d prüfen

        if (result < 0)
	    {
            result = result + 256;
	    }

        check_DC_C(memory.RAM[file], memory.W_Reg, "-");
        checkZ(result);

        StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void SWAPF(int file, int d) // swap nibbles in f
    {
        int newUpper = (memory.RAM[file] << 4) & 0b_1111_0000;
        int newLower = (memory.RAM[file] >> 4) & 0b_0000_1111;

        int result  = newLower + newUpper;

        StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void XORWF (int file, int d) //exclusive OR w with f
    {

        int result = memory.W_Reg ^ memory.RAM[file];

        checkZ(result);

        StoreSwitchedOnD(file, result, d);

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    #endregion byte-oriented file register operations

    #region bit-oriented file register operations
    public void BCF (int file, int bit) //bit clear f
    {
        switch (bit)
        {
            case 0:
                memory.RAM[file] &= 0b_1111_1110;
                break;
            case 1:
                memory.RAM[file] &= 0b_1111_1101;
                break;
            case 2:
                memory.RAM[file] &= 0b_1111_1011;
                break;
            case 3:
                memory.RAM[file] &= 0b_1111_0111;
                break;
            case 4:
                memory.RAM[file] &= 0b_1110_1111;
                break;
            case 5:
                memory.RAM[file] &= 0b_1101_1111;
                break;
            case 6:
                memory.RAM[file] &= 0b_1011_1111;
                break;
            default: // bit = 7
                memory.RAM[file] &= 0b_0111_1111;
                break;
        }

        //PC hochzählen
        ChangePC_Fetch(1);
        //cycles: 1
        memory.CycleCounter++;
    }

    public void BSF (int file, int bit)//bit set f
    {
        switch (bit)
        {
            case 0:
                memory.RAM[file] |= 0b_0000_0001;
                break;
            case 1:
                memory.RAM[file] |= 0b_0000_0010;
                break;
            case 2:
                memory.RAM[file] |= 0b_0000_0100;
                break;
            case 3:
                memory.RAM[file] |= 0b_0000_1000;
                break;
            case 4:
                memory.RAM[file] |= 0b_0001_0000;
                break;
            case 5:
                memory.RAM[file] |= 0b_0010_0000;
                break;
            case 6:
                memory.RAM[file] |= 0b_0100_0000;
                break;
            default: // bit = 7
                memory.RAM[file] |= 0b_1000_0000;
                break;
        }

        //PC hochzählen
        ChangePC_Fetch(1);
        //cycles: 1
        memory.CycleCounter++;
    }

    public void BTFSC (int file, int bit) //bit test f, skip if clear
    {
        int summand;
        switch (bit)
        {
            case 0:
                summand = bitTest(memory.RAM[file] & 0b_0000_0001, 0);
                break;
            case 1:
                summand = bitTest(memory.RAM[file] & 0b_0000_0010, 0);
                break;
            case 2:
                summand = bitTest(memory.RAM[file] & 0b_0000_0100, 0);
                break;
            case 3:
                summand = bitTest(memory.RAM[file] & 0b_0000_1000, 0);
                break;
            case 4:
                summand = bitTest(memory.RAM[file] & 0b_0001_0000, 0);
                break;
            case 5:
                summand = bitTest(memory.RAM[file] & 0b_0010_0000, 0);
                break;
            case 6:
                summand = bitTest(memory.RAM[file] & 0b_0100_0000, 0);
                break;
            default: // bit = 7
                summand = bitTest(memory.RAM[file] & 0b_1000_0000, 0);
                break;
        }
        //PC hochzählen
        ChangePC_Fetch((byte)summand);

        //cycles: 1
        memory.CycleCounter++;

        if (summand == 2) //bei überspringen ein nop ausführen (damit breakpunkte etc funktionieren, muss der erste teil der Run-while schleife ausgeführt werden
        {
            BeginLoop();
            //weiterer cycle (theoretisch im nop)
            memory.CycleCounter++;
        }
    }

    public void BTFSS (int file, int bit) //bit test f, skip if set
    {
        int summand;
        switch (bit)
        {
            case 0:
                summand = bitTest(memory.RAM[file] & 0b_0000_0001, 1);
                break;
            case 1:
                summand = bitTest(memory.RAM[file] & 0b_0000_0010, 1);
                break;
            case 2:
                summand = bitTest(memory.RAM[file] & 0b_0000_0100, 1);
                break;
            case 3:
                summand = bitTest(memory.RAM[file] & 0b_0000_1000, 1);
                break;
            case 4:
                summand = bitTest(memory.RAM[file] & 0b_0001_0000, 1);
                break;
            case 5:
                summand = bitTest(memory.RAM[file] & 0b_0010_0000, 1);
                break;
            case 6:
                summand = bitTest(memory.RAM[file] & 0b_0100_0000, 1);
                break;
            default: // bit = 7
                summand = bitTest(memory.RAM[file] & 0b_1000_0000, 1);
                break;
        }
        //PC hochzählen
        ChangePC_Fetch((byte)summand);

        //cycles: 1
        memory.CycleCounter++;

        if (summand == 2) //bei überspringen ein nop ausführen (damit breakpunkte etc funktionieren, muss der erste teil der Run-while schleife ausgeführt werden
        {
            BeginLoop();
            //weiterer cycle (theoretisch im nop)
            memory.CycleCounter++;
        }
    }

 
    #endregion

    #region literal and control operations
    public void ADDLW (int literal) //add literal and w -> fertig
    {
        int result = literal + memory.W_Reg;

        if (result > 255)
	    {
            result = result -256;
	    }


        check_DC_C(literal, memory.W_Reg, "+");
        checkZ(result);

        memory.W_Reg = (short)result;

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void ANDLW(int literal) //and literal and w -> fertig
    {
    
        int result;

        result = memory.W_Reg & literal;

        checkZ(result);
        memory.W_Reg = (short)result;


        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void CALL(int address) // call subroutine -> fertig
    {
        //ganzen 13 bit PC auf Stack pushen
        int returnAdress = memory.RAM.RAMList[0].Byte2.Value + (memory.RAM.RAMList[0].Byte10.Value << 8) +1;
        memory.PCStack.Push((short)returnAdress);

        //cycles: 2, aber beide in GOTO
        
        GOTO(address);
    }

    public void CLRWDT() //clear watchdog timer -> wdt fehlt
    {
        //PC hochzählen
        ChangePC_Fetch(1);
        //cycles: 1
        memory.CycleCounter++;
    }

    public void GOTO (int address) //go to address -> fertig
    {
        //bit 0-10 aus address, bit 11-12 aus PCLATH <3,4>
        int new_pc = address + ((memory.RAM[Constants.PCLATH_B1] & 0b_0001_1000) << 11);

        //execution löschen
        SrcModel[memory.RAM.PC_Without_Clear].IsExecuted = false;

        //PCL setzen
        int newPCL = address & 0b_0111_1111;
        memory.RAM[Constants.PCL_B1] = Convert.ToByte(newPCL);

        //cycles: 2
        memory.CycleCounter++;
        memory.CycleCounter++;

        memory.RAM.lol = true;
        memory.RAM.PC_JumpAdress = new_pc;
    }

    public void IORLW (int literal) //inclusive OR literal with w   -> fertig
    {
        int result;

        result = (int)memory.W_Reg | literal;

        checkZ (result);
        memory.W_Reg = (short)result;

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void MOVLW (int literal) //move literal to w -> fertig
    {
        memory.W_Reg = Convert.ToByte(literal);
        ///PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void RETFIE() //return from interrupt -> fertig
    {
        SrcModel[memory.RAM.PC_Without_Clear].IsExecuted = false;

        //top of stack in PC
        int returnAddress = memory.PCStack.Pop();
        returnAddress &= 0b_1111_1111;
        memory.RAM[Constants.PCL_B1] = Convert.ToByte(returnAddress);

        //GlobalInterruptEnable setzen
        int wert = memory.RAM[Constants.INTCON_B1] | 0b_1000_0000;
        memory.RAM[Constants.INTCON_B1] = Convert.ToByte(wert); 

        //cycles: 2
        memory.CycleCounter++;
        memory.CycleCounter++;
        memory.IsISR = false;
    }

    public void RETLW (int literal) //return with literal in w -> fertig
    {
        memory.W_Reg = Convert.ToByte(literal);

        SrcModel[memory.RAM.PC_Without_Clear].IsExecuted = false;

        // top of stack in PC
        int returnAddress = memory.PCStack.Pop();
        returnAddress &= 0b_1111_1111;
        memory.RAM[Constants.PCL_B1]= Convert.ToByte(returnAddress);

        //cycles: 2
        memory.CycleCounter++;
        memory.CycleCounter++;
    }

    public void RETURN() //return from subroutine -> fertig
    {
        SrcModel[memory.RAM.PC_Without_Clear].IsExecuted = false;
        //top of stack in PC
        int returnAddress = memory.PCStack.Pop();
        returnAddress &= 0b_1111_1111;
        memory.RAM[Constants.PCL_B1] = Convert.ToByte(returnAddress);

        //cycles: 2
        memory.CycleCounter++;
        memory.CycleCounter++;

    }

    public void SLEEP() // go into standby mode -> wdt fehlt, sleep modus
    {
        //clear power down status
        int PD = Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_0111);
        memory.RAM[Constants.STATUS_B1] = Convert.ToByte(PD);

        //set time out status
        int TO = Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0001_0000);
        memory.RAM[Constants.STATUS_B1] = Convert.ToByte(TO);

        //clear Watchdog and prescaler

        //processor in sleep mode, oscilatoor stopped,  page 14.8

        //cycles: 1
        memory.CycleCounter++;
    }

    public void SUBLW(int literal) // subtract w from literal -> fertig
    {
        int result = literal - memory.W_Reg; 
        check_DC_C(literal, memory.W_Reg, "-");        
        checkZ(result);

        if (result < 0)
	    {
            result = result + 256;
	    }

        memory.W_Reg = (short)result;

        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }

    public void XORLW (int literal) //exclusive OR literal with w -> fertig
    {
        int result;

        result = memory.W_Reg ^ literal;

        checkZ(result);
        memory.W_Reg = (short)result;
        
        //PC hochzählen
        ChangePC_Fetch(1);

        //cycles: 1
        memory.CycleCounter++;
    }
    #endregion

    #region run
    public void Run ()
    {
        while (true)
        {
            BeginLoop();
            befehl = SrcModel[memory.RAM.PC_With_Clear].ProgramCode;
            Thread.Sleep(200);
            
            switch (befehl)
            {
                case 0b_0000_0000_0000_1000:
                    RETURN(); //Return from Subroutine
                    break;
                case 0b_0000_0000_0000_1001:
                    RETFIE(); //return from interrupt
                    break;
                case 0b_0000_0000_0110_0011:
                    SLEEP(); //Go to standby mode
                    break;
                case 0b_0000_0000_0110_0100:
                    CLRWDT(); //clear watchdog timer
                    break;
                case 0b_0000_0000_0000_0000: // ab hier nop
                case 0b_0000_0000_0010_0000:
                case 0b_0000_0000_0100_0000:
                case 0b_0000_0000_0110_0000:
                    NOP(); //no operation 
                    break;
                case short n when (n >= 0b_0000_0001_0000_0000 && n <= 0b_0000_0001_0111_1111):
                    CLRW(); //clear w
                    break;
                default:
                    AnalyzeNibble3();
                    break;
            }

        }
    }

    public void AnalyzeNibble3()
    {
        int nibble3 = (int)befehl & 0b_0011_0000_0000_0000; //bitoperation nur auf int ausführbar
        switch (nibble3)
        {
            case 0b_0010_0000_0000_0000:
                int bit12 = (int)befehl & 0b_0000_1000_0000_0000;
                int address = (int)befehl & 0b_0000_0111_1111_1111;
                if (bit12 == 0b_0000_1000_0000_0000)
                {
                    GOTO(address);
                }
                else
                {
                     CALL(address);
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

    public void AnalyzeBits11_12() //bit-oriented operations genauer analysieren
    {
        int bits11_12 = ((int)befehl & 0b_0000_1100_0000_0000) >> 10;
        int bits = ((int)befehl & 0b_0000_0011_1000_0000) >> 7;
        int file = (int)befehl & 0b_0000_0000_0111_1111;
        if (file == 0x02) //wenn PCL beschrieben wird
        {
            memory.RAM.PCL_was_Manipulated = true;
        }
        switch (bits11_12)
        {
            case 0:
                BCF(file, bits);
                break;
            case 1:
                BSF(file, bits);
                break;
            case 2:
                BTFSC(file, bits);
                break;
            case 3:
                BTFSS(file, bits);
                break;
            default:
                break;
        }
    }
    
    public void AnalyzeNibble2Literal() 
    {
        int nibble2 = (int)befehl & 0b_0000_1111_0000_0000;
        int literal = (int)befehl & 0b_0000_0000_1111_1111;
        switch (nibble2)
        {
            case 0b_1001_0000_0000:
                ANDLW(literal);
                break;
            case 0b_1000_0000_0000:
                IORLW(literal);
                break;
            case 0b_1010_0000_0000:
                XORLW(literal);
                break;
            case 0b_1110_0000_0000: //ab hier addlw
            case 0b_1111_0000_0000:
                ADDLW(literal); 
                break;
            case 0b_1100_0000_0000: //ab hier sublw
            case 0b_1101_0000_0000:
                SUBLW(literal);
                break;
            case 0b_0000_0000_0000: //ab hier movlw
            case 0b_0001_0000_0000:
            case 0b_0010_0000_0000:
            case 0b_0011_0000_0000:
                MOVLW(literal);
                break;
            case 0b_0100_0000_0000: //ab hier retlw
            case 0b_0101_0000_0000:
            case 0b_0110_0000_0000:
            case 0b_0111_0000_0000:
                RETLW(literal);
                break;
            default:
                break;
        }
    }

    public void AnalyzeNibble2Byte() 
    {
        int nibble2 = (int)befehl & 0b_0000_1111_0000_0000;
        int file = (int)befehl & 0b_0000_0000_0111_1111;
        int d = ((int)befehl & 0b_0000_0000_1000_0000)>>7;
        switch (nibble2)
        {
            case 0b_0000_0000_0000_0000:
                MOVWF(file);
                break;
            case 0b_0000_0001_0000_0000:
                CLRF(file); 
                break;
            case 0b_0000_0010_0000_0000:
                SUBWF(file, d);
                break;
            case 0b_0000_0011_0000_0000:
                DECF(file, d);
                break;
            case 0b_0000_0100_0000_0000:
                IORWF(file, d);
                break;
            case 0b_0000_0101_0000_0000:
                ANDWF(file, d);
                break;
            case 0b_0000_0110_0000_0000:
                XORWF(file, d);
                break;
            case 0b_0000_0111_0000_0000:
                ADDWF(file, d);
                break;
            case 0b_0000_1000_0000_0000:
                MOVF(file, d);
                break;
            case 0b_0000_1001_0000_0000:
                COMF(file, d);
                break;
            case 0b_0000_1010_0000_0000:
                INCF(file, d);
                break;
            case 0b_0000_1011_0000_0000:
                DECFSZ(file, d);
                break;
            case 0b_0000_1100_0000_0000:
                RRF(file, d);
                break;
            case 0b_0000_1101_0000_0000:
                RLF(file, d);
                break;
            case 0b_0000_1110_0000_0000:
                SWAPF(file, d);
                break;
            case 0b_0000_1111_0000_0000:
                INCFSZ(file, d);
                break;
            default:
                break;
        }
    }
    #endregion

    #region helpers

    public void checkZ(int result)
    {
        byte wert;
        if (result == 0)
        {
            wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0000_0100);
        }
        else
        {
            wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_1011);
        }
        memory.RAM[Constants.STATUS_B1] = wert;
        memory.Z_Flag = (short)((wert & 0b_0000_0100)>>2); //wert für GUI updaten
    }

    public void check_DC_C(int literal1, int literal2, string op)
    {
        int literal1low = literal1 & 0b_0000_1111;
        int literal2low = literal2 & 0b_0000_1111;
        byte wert;
        if (op == "+")
        {
            if ((literal1low + literal2low) > 15) //DigitCarry prüfen
            {
                wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0000_0010);
            }
            else
            {
                wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_1101);
            }
            memory.RAM[Constants.STATUS_B1] = wert;
            memory.DC_Flag = (short)((wert & 0b_0000_0010) >> 1); //Wert für GUI updaten
            if ((literal1 + literal2) > 255) // Carry prüfen
            {
                wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0000_0001);
            }
            else
            {
                wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_1110);
            }
            memory.RAM[Constants.STATUS_B1] = wert;
            memory.C_Flag = (short)(wert & 0b_0000_0001); //Wert für GUI updaten
        }

        else //op = "-"
        {
            //hier hat der pic umgekehrte Logik, da die Entwickler ein invertieren vergessen haben (siehe Themenblatt)
            if ((literal1low - literal2low) < 0) // DigitCarry prüfen
            {
                wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_1101);
            }
            else
            {
                wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0000_0010);
            }
            memory.RAM[Constants.STATUS_B1] = wert;
            memory.DC_Flag = (short)((wert & 0b_0000_0010) >> 1); //Wert für GUI updaten
            if ((literal1 - literal2) < 0) //Carry prüfen
            {
                wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_1110);
            }
            else
            {
                wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0000_0001);
            }
            memory.RAM[Constants.STATUS_B1] = wert;
            memory.C_Flag = (short)(wert & 0b_0000_0001); //Wert für GUI updaten
        }
    }

    public int bitTest(int content, int skip)
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

    public void StoreSwitchedOnD(int file, int result, int d)
    {
        if (d == 0)
        {
            //result stored in w
            memory.W_Reg = Convert.ToByte(result);
        }
        else
        {
            if (file == 0x02)
            {
                SrcModel.ListOfCode[memory.RAM.PC_Without_Clear].IsExecuted = false;
                memory.RAM.PCL_was_Manipulated = true;
            }
            //result stored in f
            memory.RAM[file] = Convert.ToByte(result);
        }
    }

    private void ChangePC_Fetch (byte wert)
    {
        SrcModel[memory.RAM.PC_Without_Clear].IsExecuted = false;
        if (memory.RAM[Constants.PCL_B1] == 0b_1111_1111)
        {
            memory.RAM[Constants.PCL_B1] = (byte)(0 + wert);
            //PCLATH erhöhen
            memory.RAM[Constants.PCLATH_B1] += 1;
        }
        else
        {
            memory.RAM[Constants.PCL_B1] += wert;
        }
    }


    private void CheckForInterrupts() //INTCON DA MAIN MAN
    {
        //GIE gesetzt?
        if((memory.RAM[Constants.INTCON_B1] & 0b1000_0000)>0)
        {
            //GIE gesetzt
            //T0IE gesetzt?
            if ((memory.RAM[Constants.INTCON_B1] & 0b0010_0000) > 0)
            {
                //T0IE gesetzt
                CheckForTimer0Interrupt();
            }
            //EEIE gesetzt?
            if ((memory.RAM[Constants.INTCON_B1] & 0b0100_0000) > 0)
            {
                //EEIE gesetzt
                CheckForEEPROMWrittenInterrupt();
            }
            //INTE gesetzt?
            if ((memory.RAM[Constants.INTCON_B1] & 0b0001_0000) > 0)
            {
                //INTE gesetzt
                CheckForRB0Interrupt();
            }
            //RBIE gesetzt
            if ((memory.RAM[Constants.INTCON_B1] & 0b0000_1000) > 0)
            {
                //RBIE gesetzt
                CheckForPortBInterrupt();
            }
        }
    }

    private void CheckForTimer0Interrupt()
    {
        //T0IF gesetzt
        if((memory.RAM[Constants.INTCON_B1] & 0b0000_0100) > 0)
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
        if((memory.RAM[Constants.INTCON_B1] & 0b0000_0010) >0)
        {
            Interrupt();
        }
    }

    private void CheckForPortBInterrupt()
    {
        if ((memory.RAM[Constants.INTCON_B1] & 0b0000_0001) > 0)
        {
            Interrupt();
        }
    }

    private void Interrupt()
    {
        //Push PC to Stack
        memory.PCStack.Push((short)memory.RAM.PC_Without_Clear);
        //Set PC to Interrupt Vector
        memory.RAM[Constants.PCL_B1] = (byte)Constants.PERIPHERAL_INTERRUPT_VECTOR_ADDRESS;
        memory.IsISR = true;
    }

    private void BeginLoop()
    {
        if (memory.IsISR == false)
        {
            CheckForInterrupts();
        }
        if (memory.RAM.PC_Without_Clear == 0x7ff)
        {
            memory.RAM[Constants.PCL_B1] = 0;
            memory.RAM[Constants.PCLATH_B1] = 0;
        }
        SrcModel[memory.RAM.PC_Without_Clear].IsExecuted = true;
        while (true)
        {
            if (DebugCodes.Pause || SrcModel[memory.RAM.PC_Without_Clear].IsDebug)
            {
                //loop forever
            }
            else
            {
                break;
            }
        }
    }
    #endregion

    public CommandService(Memory memory, SourceFileModel SrcModel)
    {
        this.memory = memory;
        this.SrcModel = SrcModel;
    }

}