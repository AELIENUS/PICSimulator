using Application.Model;
using Applicator.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Application.ViewModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows;

//todo: methoden wieder private!!!!!!!!! (für tests public gestellt)

public class CommandService : ICommandService
{
    #region byte-oriented file register operations
    public void ADDWF(Memory memory, int file, int d) //add w and f 
    {
        int result = memory.RAM[file] + memory.W_Reg;

        if (result > 255)
        {
            result = result - 256;
        }

        check_DC_C(memory, memory.RAM[file], memory.W_Reg, "+");
        checkZ(memory, result);

        StoreSwitchedOnD(memory, file, result, d);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void ANDWF(Memory memory, int file, int d) //and w and f
    {
        int result = memory.W_Reg & memory.RAM[file];

        checkZ(memory, result);
        StoreSwitchedOnD(memory, file, result, d);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));
    }

    public void CLRF(Memory memory, int file) //clear f
    {
        memory.RAM[file] = 0;

        // z flag setzen
        checkZ(memory, 0);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));
    }

    public void CLRW(Memory memory) // clear w
    {
        memory.W_Reg = 0;

        // z flag setzen
        checkZ(memory, 0);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));
    }

    public void COMF(Memory memory, int file, int d) //complement f
    {
        int result = memory.RAM[file] ^ 0b_1111_1111;

        checkZ(memory, result);
        StoreSwitchedOnD(memory, file, result, d);
        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void DECF(Memory memory, int file, int d) //Decrement f
    {
        int result = memory.RAM[file] - 1;

        if (result < 0)
        {
            result = result + 256;
        }
        checkZ(memory, result);
        StoreSwitchedOnD(memory, file, result, d);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void DECFSZ(Memory memory, int file, int d) //Decrement f, Skip if 0
    {
        int result = memory.RAM[file] - 1;

        if (result < 0)
        {
            result = result + 256;
        }
        StoreSwitchedOnD(memory, file, result, d);

        int summand = 1;
        if (result == 0)
        {
            summand++;
        }

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + summand));

        //cycles: 1, bei überspringen 2
    }

    public void INCF(Memory memory, int file, int d) //increment f
    {
        int result = memory.RAM[file] + 1;
        if (result > 255)
        {
            result = result - 256;
        }
        checkZ(memory, result);
        StoreSwitchedOnD(memory, file, result, d);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void INCFSZ(Memory memory, int file, int d) //increment f, skip if zero
    {
        int result = memory.RAM[file] + 1;
        if (result > 255)
        {
            result = result - 256;
        }
        StoreSwitchedOnD(memory, file, result, d);

        int summand = 1;
        if (result == 0)
        {
            summand++;
        }

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + summand));

    }

    public void IORWF(Memory memory, int file, int d) // inclusive OR w with f
    {
        int result = (int)memory.W_Reg | (int)memory.RAM[file];

        checkZ(memory, result);

        StoreSwitchedOnD(memory, file, result, d);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void MOVF(Memory memory, int file, int d) // move f
    {
        int result = memory.RAM[file];

        checkZ(memory, result);

        StoreSwitchedOnD(memory, file, result, d);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void MOVWF(Memory memory, int file) // move w to f
    {
        memory.RAM[file] = Convert.ToByte(memory.W_Reg);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void NOP(Memory memory) // no operation
    {
        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void RLF(Memory memory, int file, int d) // rotate left through carry
    {
        int result = memory.RAM[file] << 1;
        int carry = memory.RAM[Constants.STATUS_B1] & 0b_0000_0001;

        result += carry;

        if (result > 255)
        {
            result = result - 256;
            //carry setzen
            ChangeBoth(memory, Constants.STATUS_B1, Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0000_0001));
        }
        else
        {
            // carry löschen
            ChangeBoth(memory, Constants.STATUS_B1, Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_1110));
        }

        StoreSwitchedOnD(memory, file, result, d);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void RRF(Memory memory, int file, int d) // rotate right through carry
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
            ChangeBoth(memory, Constants.STATUS_B1, Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0000_0001));
        }
        else
        {
            // carry löschen
            ChangeBoth(memory, Constants.STATUS_B1, Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_1110));
        }

        StoreSwitchedOnD(memory, file, result, d);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void SUBWF(Memory memory, int file, int d) // substract w from f
    {
        int result = memory.RAM[file] - memory.W_Reg; // literal in f ändern, auf d prüfen

        if (result < 0)
        {
            result = result + 256;
        }

        check_DC_C(memory, memory.RAM[file], memory.W_Reg, "-");
        checkZ(memory, result);

        StoreSwitchedOnD(memory, file, result, d);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void SWAPF(Memory memory, int file, int d) // swap nibbles in f
    {
        int newUpper = (memory.RAM[file] << 4) & 0b_1111_0000;
        int newLower = (memory.RAM[file] >> 4) & 0b_0000_1111;

        int result = newLower + newUpper;

        StoreSwitchedOnD(memory, file, result, d);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void XORWF(Memory memory, int file, int d) //exclusive OR w with f
    {

        int result = memory.W_Reg ^ memory.RAM[file];

        checkZ(memory, result);

        StoreSwitchedOnD(memory, file, result, d);

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    #endregion byte-oriented file register operations

    #region bit-oriented file register operations
    public async Task BCF(Memory memory, int file, int bit) //bit clear f
    {
        switch (bit)
        {
            case 0:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] & 0b_1111_1110);
                break;
            case 1:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] & 0b_1111_1101);
                break;
            case 2:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] & 0b_1111_1011);
                break;
            case 3:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] & 0b_1111_0111);
                break;
            case 4:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] & 0b_1110_1111);
                break;
            case 5:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] & 0b_1101_1111);
                break;
            case 6:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] & 0b_1011_1111);
                break;
            default: // bit = 7
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] & 0b_0111_1111);
                break;
        }

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles = 1
    }

    public async Task BSF(Memory memory, int file, int bit)//bit set f
    {
        switch (bit)
        {
            case 0:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] | 0b_0000_0001);
                break;
            case 1:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] | 0b_0000_0010);
                break;
            case 2:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] | 0b_0000_0100);
                break;
            case 3:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] | 0b_0000_1000);
                break;
            case 4:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] | 0b_0001_0000);
                break;
            case 5:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] | 0b_0010_0000);
                break;
            case 6:
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] | 0b_0100_0000);
                break;
            default: // bit = 7
                memory.RAM[file] = Convert.ToByte(memory.RAM[file] | 0b_1000_0000);
                break;
        }

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles = 1
    }

    public async Task BTFSC(Memory memory, int file, int bit) //bit test f, skip if clear
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
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + summand));

        //cycles: 1, bei skip 2
    }

    public async Task BTFSS(Memory memory, int file, int bit) //bit test f, skip if set
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
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + summand));

        //cycles: 1, bei skip 2
    }


    #endregion

    #region literal and control operations
    public void ADDLW(Memory memory, int literal) //add literal and w -> fertig
    {
        int result = literal + memory.W_Reg;

        if (result > 255)
        {
            result = result - 256;
        }

        check_DC_C(memory, literal, memory.W_Reg, "+");
        checkZ(memory, result);

        memory.W_Reg = (short)result;

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void ANDLW(Memory memory, int literal) //and literal and w -> fertig
    {

        int result;

        result = memory.W_Reg & literal;

        checkZ(memory, result);
        memory.W_Reg = (short)result;


        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void CALL(Memory memory, int address) // call subroutine -> fertig
    {
        int returnAdress = memory.RAM[Constants.PCL_B1] + 1;
        memory.PCStack.Push((short)returnAdress);

        GOTO(memory, address);


        // 2 cycles
    }

    public void CLRWDT(Memory memory) //clear watchdog timer -> wdt fehlt
    {
        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));
    }

    public async Task GOTO(Memory memory, int address) //go to address -> fertig
    {
        //bits 0-10 für PCL:
        int pcl_low = (int)address & 0b_0000_0000_1111_1111; //kommt in PCL
        int pcl_high = ((int)address & 0b_0000_0111_0000_0000) >> 8; //kommt in PCLATH

        //bits 11-12 für PCL aus PCLATH 3,4:
        pcl_high = pcl_high + (memory.RAM[Constants.PCLATH_B1] & 0b_0001_1000);

        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(pcl_low));
        ChangeBoth(memory, Constants.PCLATH_B1, Convert.ToByte(pcl_high));

        //cycles:2
    }

    public void IORLW(Memory memory, int literal) //inclusive OR literal with w   -> fertig
    {
        int result;

        result = memory.W_Reg | literal;

        checkZ(memory, result);
        memory.W_Reg = (short)result;
        ;
        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void MOVLW(Memory memory, int literal) //move literal to w -> fertig
    {
        memory.W_Reg = Convert.ToByte(literal);
        ///PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void RETFIE(Memory memory) //return from interrupt -> fertig
    {
        //top of stack in PC
        int returnAddress = memory.PCStack.Pop();
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(returnAddress));

        //GlobalInterruptEnable setzen
        int wert = memory.RAM[Constants.INTCON_B1] | 0b_1000_0000;
        ChangeBoth(memory, Constants.INTCON_B1, Convert.ToByte(wert));

        //cycles: 2
    }

    public void RETLW(Memory memory, int literal) //return with literal in w -> fertig
    {
        memory.W_Reg = Convert.ToByte(literal);

        // top of stack in PC
        int returnAddress = memory.PCStack.Pop();
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(returnAddress));

        //cycles: 2
    }

    public void RETURN(Memory memory) //return from subroutine -> fertig
    {
        //top of stack in PC
        int returnAddress = memory.PCStack.Pop();
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(returnAddress));

        //cycles: 2

    }

    public void SLEEP(Memory memory) // go into standby mode -> wdt fehlt, sleep modus
    {
        //clear power down status
        int PD = Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_0111);
        ChangeBoth(memory, Constants.STATUS_B1, Convert.ToByte(PD));

        //set time out status
        int TO = Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0001_0000);
        ChangeBoth(memory, Constants.STATUS_B1, Convert.ToByte(TO));

        //clear Watchdog and prescaler

        //processor in sleep mode, oscilatoor stopped,  page 14.8

    }

    public void SUBLW(Memory memory, int literal) // subtract w from literal -> fertig
    {
        int result = literal - memory.W_Reg;
        check_DC_C(memory, literal, memory.W_Reg, "-");
        checkZ(memory, result);

        if (result < 0)
        {
            result = result + 256;
        }

        memory.W_Reg = (short)result;

        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }

    public void XORLW(Memory memory, int literal) //exclusive OR literal with w -> fertig
    {
        int result;

        result = memory.W_Reg ^ literal;

        checkZ(memory, result);
        memory.W_Reg = (short)result;
        ;
        //PC hochzählen
        ChangeBoth(memory, Constants.PCL_B1, Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1));

        //cycles: 1
    }
    #endregion


    public async Task Run(Memory memory, List<int> breakpointList)
    {
        while (true)

        {
            int PC = memory.RAM[Constants.PCL_B1] + memory.RAM[Constants.PCLATH_B1];
            if (PC == 0x7ff)
            {
                memory.RAM[Constants.PCL_B1] = 0;
                memory.RAM[Constants.PCLATH_B1] = 0;
            }
            switch (memory.Program[PC])
            {
                case 0b_0000_0000_0000_1000:
                    RETURN(memory); //Return from Subroutine
                    break;
                case 0b_0000_0000_0000_1001:
                    RETFIE(memory); //return from interrupt
                    break;
                case 0b_0000_0000_0110_0011:
                    SLEEP(memory); //Go to standby mode
                    break;
                case 0b_0000_0000_0110_0100:
                    CLRWDT(memory); //clear watchdog timer
                    break;
                case 0b_0000_0000_0000_0000: // ab hier nop
                case 0b_0000_0000_0010_0000:
                case 0b_0000_0000_0100_0000:
                case 0b_0000_0000_0110_0000:
                    NOP(memory); //no operation 
                    break;
                case short n when (n >= 0b_0000_0001_0000_0000 && n <= 0b_0000_0001_0111_1111):
                    CLRW(memory); //clear w
                    break;
                default:
                    AnalyzeNibble3(memory);
                    break;
            }
        }
    }

    public async Task AnalyzeNibble3(Memory memory)
    {
        int PC = memory.RAM[Constants.PCL_B1] + memory.RAM[Constants.PCLATH_B1];
        short befehl = memory.Program[PC];
        int nibble3 = (int)befehl & 0b_0011_0000_0000_0000; //bitoperation nur auf int ausführbar
        switch (nibble3)
        {
            case 0b_0010_0000_0000_0000:
                int bit12 = (int)befehl & 0b_0000_1000_0000_0000;
                int address = (int)befehl & 0b_0000_0111_1111_1111;
                if (bit12 == 0b_0000_1000_0000_0000)
                {
                    await GOTO(memory, address);
                }
                else
                {
                    CALL(memory, address);
                }
                break;
            case 0b_0001_0000_0000_0000: //bit oriented operations
                await AnalyzeBits11_12(memory);
                break;
            case 0b_0011_0000_0000_0000: //literal operations
                await AnalyzeNibble2Literal(memory);
                break;
            case 0b_0000_0000_0000_0000: //byte oriented operations
                await AnalyzeNibble2Byte(memory);
                break;
            default:
                break;
        }
    }

    public async Task AnalyzeBits11_12(Memory memory) //bit-oriented operations genauer analysieren
    {
        int PC = memory.RAM[Constants.PCL_B1] + memory.RAM[Constants.PCLATH_B1];
        short befehl = memory.Program[PC];
        int bits11_12 = (int)befehl & 0b_0000_1100_0000_0000;
        int bits = (int)befehl & 0b_0000_0011_1000_0000;
        int file = (int)befehl & 0b_0000_0000_0111_1111;
        switch (bits11_12)
        {
            case 0:
                await BCF(memory, file, bits);
                break;
            case 1:
                await BSF(memory, file, bits);
                break;
            case 2:
                await BTFSC(memory, file, bits);
                break;
            case 3:
                await BTFSS(memory, file, bits);
                break;
            default:
                break;
        }
    }
    public async Task AnalyzeNibble2Literal(Memory memory)
    {
        int PC = memory.RAM[Constants.PCL_B1] + memory.RAM[Constants.PCLATH_B1];
        short befehl = memory.Program[PC];
        int nibble2 = (int)befehl & 0b_0000_1111_0000_0000;
        int literal = (int)befehl & 0b_0000_0000_1111_1111;
        switch (nibble2)
        {
            case 0b_1001_0000_0000:
                ANDLW(memory, literal);
                break;
            case 0b_1000_0000_0000:
                IORLW(memory, literal);
                break;
            case 0b_1010_0000_0000:
                XORLW(memory, literal);
                break;
            case 0b_1110_0000_0000: //ab hier addlw
            case 0b_1111_0000_0000:
                ADDLW(memory, literal);
                break;
            case 0b_1100_0000_0000: //ab hier sublw
            case 0b_1101_0000_0000:
                SUBLW(memory, literal);
                break;
            case 0b_0000_0000_0000: //ab hier movlw
            case 0b_0001_0000_0000:
            case 0b_0010_0000_0000:
            case 0b_0011_0000_0000:
                MOVLW(memory, literal);
                break;
            case 0b_0100_0000_0000: //ab hier retlw
            case 0b_0101_0000_0000:
            case 0b_0110_0000_0000:
            case 0b_0111_0000_0000:
                RETLW(memory, literal);
                break;
            default:
                break;
        }
    }
    public async Task AnalyzeNibble2Byte(Memory memory)
    {
        int PC = memory.RAM[Constants.PCL_B1] + memory.RAM[Constants.PCLATH_B1];
        short befehl = memory.Program[PC];
        int nibble2 = (int)befehl & 0b_0000_1111_0000_0000;
        int file = (int)befehl & 0b_0000_0000_0111_1111;
        int d = (int)befehl & 0b_0000_0000_1000_0000;
        switch (nibble2)
        {
            case 0b_0000_0000_0000_0000:
                MOVWF(memory, file);
                break;
            case 0b_0000_0001_0000_0000:
                CLRF(memory, file);
                break;
            case 0b_0000_0010_0000_0000:
                SUBWF(memory, file, d);
                break;
            case 0b_0000_0011_0000_0000:
                DECF(memory, file, d);
                break;
            case 0b_0000_0100_0000_0000:
                IORWF(memory, file, d);
                break;
            case 0b_0000_0101_0000_0000:
                ANDWF(memory, file, d);
                break;
            case 0b_0000_0110_0000_0000:
                XORWF(memory, file, d);
                break;
            case 0b_0000_0111_0000_0000:
                ADDWF(memory, file, d);
                break;
            case 0b_0000_1000_0000_0000:
                MOVF(memory, file, d);
                break;
            case 0b_0000_1001_0000_0000:
                COMF(memory, file, d);
                break;
            case 0b_0000_1010_0000_0000:
                INCF(memory, file, d);
                break;
            case 0b_0000_1011_0000_0000:
                DECFSZ(memory, file, d);
                break;
            case 0b_0000_1100_0000_0000:
                RRF(memory, file, d);
                break;
            case 0b_0000_1101_0000_0000:
                RLF(memory, file, d);
                break;
            case 0b_0000_1110_0000_0000:
                SWAPF(memory, file, d);
                break;
            case 0b_0000_1111_0000_0000:
                INCFSZ(memory, file, d);
                break;
            default:
                break;
        }
    }

    public void ChangeBoth(Memory memory, int index, byte wert)
    {
        switch (index)
        {
            case 0x00:
                memory.RAM[Constants.INDF_B1] = wert;
                memory.RAM[Constants.INDF_B2] = wert;
                break;
            case 0x02:
                memory.RAM[Constants.PCL_B1] = wert;
                memory.RAM[Constants.PCL_B2] = wert;
                break;
            case 0x03:
                memory.RAM[Constants.STATUS_B1] = wert;
                memory.RAM[Constants.STATUS_B2] = wert;
                break;
            case 0x04:
                memory.RAM[Constants.FSR_B1] = wert;
                memory.RAM[Constants.FSR_B2] = wert;
                break;
            case 0x0A:
                memory.RAM[Constants.PCLATH_B1] = wert;
                memory.RAM[Constants.PCLATH_B2] = wert;
                break;
            case 0x0B:
                memory.RAM[Constants.INTCON_B1] = wert;
                memory.RAM[Constants.INTCON_B2] = wert;
                break;
            default:
                break;
        }
    }

    public void checkZ(Memory memory, int result)
    {
        if (result == 0)
        {
            byte wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0000_0100);
            ChangeBoth(memory, Constants.STATUS_B1, wert);
        }
        else
        {
            byte wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_1011);
            ChangeBoth(memory, Constants.STATUS_B1, wert);
        }
    }

    public void check_DC_C(Memory memory, int literal1, int literal2, string op)
    {
        int literal1low = literal1 & 0b_0000_1111;
        int literal2low = literal2 & 0b_0000_1111;
        if (op == "+")
        {
            if ((literal1low + literal2low) > 15) //DigitCarry prüfen
            {
                byte wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0000_0010);
                ChangeBoth(memory, Constants.STATUS_B1, wert);
            }
            else
            {
                byte wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_1101);
                ChangeBoth(memory, Constants.STATUS_B1, wert);

            }
            if ((literal1 + literal2) > 255) // Carry prüfen
            {
                byte wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0000_0001);
                ChangeBoth(memory, Constants.STATUS_B1, wert);
            }
            else
            {
                byte wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_1110);
                ChangeBoth(memory, Constants.STATUS_B1, wert);
            }
        }
        //dann darf nur noch "-" im operanden stehen (wenn richtig aufgerufen)
        //hier hat der pic umgekehrte Logik, da die Entwickler ein invertieren vergessen haben (siehe Themenblatt)
        if ((literal1low - literal2low) < 0) // DigitCarry prüfen
        {
            byte wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_1101);
            ChangeBoth(memory, Constants.STATUS_B1, wert);

        }
        else
        {
            byte wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0000_0010);
            ChangeBoth(memory, Constants.STATUS_B1, wert);

        }
        if ((literal1 - literal2) < 0) //Carry prüfen
        {
            byte wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] & 0b_1111_1110);
            ChangeBoth(memory, Constants.STATUS_B1, wert);

        }
        else
        {
            byte wert = Convert.ToByte(memory.RAM[Constants.STATUS_B1] | 0b_0000_0001);
            ChangeBoth(memory, Constants.STATUS_B1, wert);

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

    public void StoreSwitchedOnD(Memory memory, int file, int result, int d)
    {
        if (d == 0)
        {
            //result stored in w
            memory.W_Reg = Convert.ToByte(result);
        }
        else
        {
            //result stored in f
            memory.RAM[file] = Convert.ToByte(result);
        }
    }

    public CommandService()
    {

    }

}