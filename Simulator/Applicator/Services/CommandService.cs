using Application.Model;
using Applicator.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Application.ViewModel;

public class CommandService : ICommandService
{
    #region byte-oriented file register operations
    private void ADDWF(int file, int d) //add w and f
    {
<<<<<<< Updated upstream
        
    }

    private void ANDWF (int file, int d) //and w and f
=======
        /* int result = literal + memory.W_reg; // literal in f ändern, auf d prüfen
        check_DC_C(memory, literal, memory.W_reg, "+");        
        checkZ(memory, result);

        if (result > 255)
        {
            result = result -255;
        }

        if (d==0)
        {
            memory.W_reg = result;
        }
        //result stored in f
        */

        //cycles: 1
    }

    private void ANDWF(Memory memory, int file, int d) //and w and f
>>>>>>> Stashed changes
    {

    }

<<<<<<< Updated upstream
    private void CLRF (int file) //clear f
=======
    private void CLRF(Memory memory, int file) //clear f
>>>>>>> Stashed changes
    {

    }

    private void CLRW() // clear w
    {

    }

    private void COMF(int file, int d) //complement f
    {

    }

    private void DECF(int file, int d) //Decrement f
    {

    }

    private void DECFSZ(int file, int d) //Decrement f, Skip if 0
    {

    }

<<<<<<< Updated upstream
    private void INCF (int file, int d) //increment f
=======
    private void INCF(Memory memory, int file, int d) //increment f
>>>>>>> Stashed changes
    {

    }

    private void INCFSZ(int file, int d) //increment f, skip if zero
    {

    }

<<<<<<< Updated upstream
    private void IORWF (int file, int d) // inclusive OR w with f
=======
    private void IORWF(Memory memory, int file, int d) // inclusive OR w with f
>>>>>>> Stashed changes
    {

    }

<<<<<<< Updated upstream
    private void MOVF (int file, int d) // move f
=======
    private void MOVF(Memory memory, int file, int d) // move f
>>>>>>> Stashed changes
    {

    }

<<<<<<< Updated upstream
    private void MOVWF (int file) // move w to f
=======
    private void MOVWF(Memory memory, int file) // move w to f
>>>>>>> Stashed changes
    {

    }

<<<<<<< Updated upstream
    private void NOP () // no operation
=======
    private void NOP(Memory memory) // no operation
>>>>>>> Stashed changes
    {
        Console.WriteLine("nop");
    }

<<<<<<< Updated upstream
    private void RLF (int file, int d) // rotate left through carry
=======
    private void RLF(Memory memory, int file, int d) // rotate left through carry
>>>>>>> Stashed changes
    {

    }

<<<<<<< Updated upstream
    private void RRF (int file, int d) // rotate right through carry
=======
    private void RRF(Memory memory, int file, int d) // rotate right through carry
>>>>>>> Stashed changes
    {

    }

<<<<<<< Updated upstream
    private void SUBWF (int file, int d) // substract w from f
=======
    private void SUBWF(Memory memory, int file, int d) // substract w from f
>>>>>>> Stashed changes
    {

<<<<<<< Updated upstream
    }

    private void SWAPF(int file, int d) // swap nibbles in f
=======
        if (result < 0)
        {
            result = result + 255;
        }

        if (d==0)
        {
            memory.W_reg = result;
        }
        //result stored in f
        */

        //cycles: 1
    }

    private void SWAPF(Memory memory, int file, int d) // swap nibbles in f
>>>>>>> Stashed changes
    {

    }

<<<<<<< Updated upstream
    private void XORWF (int file, int d) //exclusive OR w with f
=======
    private void XORWF(Memory memory, int file, int d) //exclusive OR w with f
>>>>>>> Stashed changes
    {

    }

    #endregion byte-oriented file register operations

    #region bit-oriented file register operations
<<<<<<< Updated upstream
    private void BCF (int file, int bits) //bit clear f
=======
    private async Task BCF(Memory memory, int file, int bits) //bit clear f
>>>>>>> Stashed changes
    {

    }

<<<<<<< Updated upstream
    private void BSF (int file, int bits)//bit set f
=======
    private async Task BSF(Memory memory, int file, int bits)//bit set f
>>>>>>> Stashed changes
    {

    }

<<<<<<< Updated upstream
    private void BTFSC (int file, int bits) //bit test f, skip if clear
=======
    private async Task BTFSC(Memory memory, int file, int bits) //bit test f, skip if clear
>>>>>>> Stashed changes
    {

    }

<<<<<<< Updated upstream
    private void BTFSS (int file, int bits) //bit test f, skip if set
=======
    private async Task BTFSS(Memory memory, int file, int bits) //bit test f, skip if set
>>>>>>> Stashed changes
    {

    }

    #endregion

    #region literal and control operations
<<<<<<< Updated upstream
    private void ADDLW (int literal) //add literal and w
    {
        Console.WriteLine("addlw {0}", literal);
=======
    private void ADDLW(Memory memory, int literal) //add literal and w
    {
        int result = literal + memory.W_Reg;
        check_DC_C(memory, literal, memory.W_Reg, "+");
        checkZ(memory, result);

        if (result > 255)
        {
            result = result - 255;
        }
        memory.W_Reg = (short)result;

        //cycles: 1
>>>>>>> Stashed changes
    }

    private void ANDLW(int literal) //and literal and w
    {

    }

    private void CALL(int address) // call subroutine
    {
        //TODO stack  = Memory.RAM[Constants.PCL_B1] + 1;
<<<<<<< Updated upstream
        /*MainViewModel.Memory
        Memory.RAM[Constants.PCL_B1] = (int)address & 0b_0000_0111_1111_1111;
        Memory.RAM[Constants.PCLATH_B1] = (int)address & 0b_0000_0111_1111_1111;*/
        Console.WriteLine("call {0}", address);
=======

        GOTO(memory, address);


        // 2 cycles
>>>>>>> Stashed changes
    }

    private void CLRWDT() //clear watchdog timer
    {

    }

<<<<<<< Updated upstream
    private void GOTO (int address) //go to address
    {
        Console.WriteLine("goto {0}", address);
        //cycles:2
    }

    private void IORLW (int literal) //inclusive OR literal with w
=======
    private async Task GOTO(Memory memory, int address) //go to address
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

    private void IORLW(Memory memory, int literal) //inclusive OR literal with w
>>>>>>> Stashed changes
    {

    }

<<<<<<< Updated upstream
    private void MOVLW (int literal) //move literal to w
    {
        Console.WriteLine("movlw {0}", literal);
=======
    private void MOVLW(Memory memory, int literal) //move literal to w
    {
        Console.WriteLine("movlw {0}", literal);
        byte wert = Convert.ToByte(memory.RAM[Constants.PCL_B1] + 1);
        ChangeBoth(memory, Constants.PCL_B1, wert);
>>>>>>> Stashed changes
    }

    private void RETFIE() //return from interrupt
    {

    }

<<<<<<< Updated upstream
    private void RETLW (int literal) //return with literal in w 
=======
    private void RETLW(Memory memory, int literal) //return with literal in w 
>>>>>>> Stashed changes
    {
        Console.WriteLine("retlw {0}", literal);
    }

    private async Task RETURN() //return from subroutine
    {
        Console.WriteLine("return");
    }

    private void SLEEP() // go into standby mode
    {

    }

    private void SUBLW(int literal) // subtract w from literal
    {
<<<<<<< Updated upstream
=======
        int result = literal - memory.W_Reg;
        check_DC_C(memory, literal, memory.W_Reg, "-");
        checkZ(memory, result);

        if (result < 0)
        {
            result = result + 255;
        }

        memory.W_Reg = (short)result;
>>>>>>> Stashed changes

    }

<<<<<<< Updated upstream
    private void XORLW (int literal) //exclusive OR literal with w
=======
    private void XORLW(Memory memory, int literal) //exclusive OR literal with w
>>>>>>> Stashed changes
    {

    }
    #endregion


    public async Task Run(Memory Memory, List<int> breakpointList)
    {
        int i = 0;
        while ( i < Constants.PROGRAM_MEMORY_SIZE) //i durch Memory.RAM[Constants.PCL_B1] ersetzen
        {
<<<<<<< Updated upstream
            switch (Memory.Program[i])
=======
            int PC = memory.RAM[Constants.PCL_B1] + memory.RAM[Constants.PCLATH_B1];
            if (PC == 0x7ff)
            {
                memory.RAM[Constants.PCL_B1] = 0;
                memory.RAM[Constants.PCLATH_B1] = 0;
            }
            switch (memory.Program[PC])
>>>>>>> Stashed changes
            {
                case 0b_0000_0000_0000_1000:
                    await RETURN(); //Return from Subroutine
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
<<<<<<< Updated upstream
                case short n when (n >= 0b_0000_0001_0000_0000 && n <= 0b_0000_0001_0111_1111): 
                    CLRW(); //clear w
=======
                case short n when (n >= 0b_0000_0001_0000_0000 && n <= 0b_0000_0001_0111_1111):
                    CLRW(memory); //clear w
>>>>>>> Stashed changes
                    break;
                default:
                    AnalyzeNibble3(Memory.Program[i]);
                    break;
            }
        }
    }

    private void AnalyzeNibble3(short befehl)
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
<<<<<<< Updated upstream
                    CALL(address);
=======
                    CALL(memory, address);
>>>>>>> Stashed changes
                }
                break;
            case 0b_0001_0000_0000_0000: //bit oriented operations
                AnalyzeBits11_12(befehl);
                break;
            case 0b_0011_0000_0000_0000: //literal operations
                AnalyzeNibble2Literal(befehl);
                break;
            case 0b_0000_0000_0000_0000: //byte oriented operations
                AnalyzeNibble2Byte(befehl);
                break;
            default:
                break;
        }
    }

    private void AnalyzeBits11_12(short befehl) //bit-oriented operations genauer analysieren
    {
        int bits11_12 = (int)befehl & 0b_0000_1100_0000_0000;
        int bits = (int)befehl & 0b_0000_0011_1000_0000;
        int file = (int)befehl & 0b_0000_0000_0111_1111;
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
<<<<<<< Updated upstream
    private void AnalyzeNibble2Literal(short befehl)
=======
    private async Task AnalyzeNibble2Literal(Memory memory)
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
                ADDLW(literal); 
=======
                ADDLW(memory, literal);
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
    private void AnalyzeNibble2Byte(short befehl)
=======
    private async Task AnalyzeNibble2Byte(Memory memory)
>>>>>>> Stashed changes
    {
        int nibble2 = (int)befehl & 0b_0000_1111_0000_0000;
        int file = (int)befehl & 0b_0000_0000_0111_1111;
        int d = (int)befehl & 0b_0000_0000_1000_0000;
        switch (nibble2)
        {
            case 0b_0000_0000_0000_0000:
                MOVWF(file);
                break;
            case 0b_0000_0001_0000_0000:
<<<<<<< Updated upstream
                CLRF(file);
=======
                CLRF(memory, file);
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
    
=======
    private void ChangeBoth(Memory memory, int index, byte wert)
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

    private void checkZ(Memory memory, int result)
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

    private void check_DC_C(Memory memory, int literal1, int literal2, string op)
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

>>>>>>> Stashed changes
    public CommandService()
    {

    }

}
