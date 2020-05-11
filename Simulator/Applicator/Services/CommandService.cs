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
    private void ADDWF(Memory memory, int file, int d) //add w and f
    {
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

    private void ANDWF (Memory memory, int file, int d) //and w and f
    {

    }

    private void CLRF (Memory memory, int file) //clear f
    {

    }

    private void CLRW(Memory memory) // clear w
    {

    }

    private void COMF(Memory memory, int file, int d) //complement f
    {

    }

    private void DECF(Memory memory, int file, int d) //Decrement f
    {

    }

    private void DECFSZ(Memory memory, int file, int d) //Decrement f, Skip if 0
    {

    }

    private void INCF (Memory memory, int file, int d) //increment f
    {

    }

    private void INCFSZ(Memory memory, int file, int d) //increment f, skip if zero
    {

    }

    private void IORWF (Memory memory, int file, int d) // inclusive OR w with f
    {

    }

    private void MOVF (Memory memory, int file, int d) // move f
    {

    }

    private void MOVWF (Memory memory,int file) // move w to f
    {

    }

    private void NOP (Memory memory) // no operation
    {
        Console.WriteLine("nop");
    }

    private void RLF (Memory memory, int file, int d) // rotate left through carry
    {

    }

    private void RRF (Memory memory,int file, int d) // rotate right through carry
    {

    }

    private void SUBWF (Memory memory,int file, int d) // substract w from f
    {
        /* int result = literal - memory.W_reg; // literal in f ändern, auf d prüfen
        check_DC_C(memory, literal, memory.W_reg, "-");        
        checkZ(memory, result);

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

    private void SWAPF(Memory memory,int file, int d) // swap nibbles in f
    {

    }

    private void XORWF (Memory memory,int file, int d) //exclusive OR w with f
    {

    }

    #endregion byte-oriented file register operations

    #region bit-oriented file register operations
    private async Task BCF (Memory memory, int file, int bits) //bit clear f
    {

    }

    private async Task BSF (Memory memory, int file, int bits)//bit set f
    {

    }

    private async Task BTFSC (Memory memory, int file, int bits) //bit test f, skip if clear
    {

    }

    private async Task BTFSS (Memory memory, int file, int bits) //bit test f, skip if set
    {

    }

    #endregion

    #region literal and control operations
    private void ADDLW (Memory memory, int literal) //add literal and w
    {
        int result = literal + memory.W_reg;
        check_DC_C(memory, literal, memory.W_reg, "+");        
        checkZ(memory, result);

        if (result > 255)
	    {
            result = result -255;
	    }
        memory.W_reg = result;

        //cycles: 1
    }

    private void ANDLW(Memory memory, int literal) //and literal and w
    {

    }

    private void CALL(Memory memory, int address) // call subroutine
    {
        //TODO stack  = Memory.RAM[Constants.PCL_B1] + 1;

        GOTO (memory, address);


       // 2 cycles
    }

    private void CLRWDT(Memory memory) //clear watchdog timer
    {

    }

    private async Task GOTO (Memory memory, int address) //go to address
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

    private void IORLW (Memory memory, int literal) //inclusive OR literal with w
    {

    }

    private void MOVLW (Memory memory, int literal) //move literal to w
    {
        Console.WriteLine("movlw {0}", literal);
        byte wert = Convert.ToByte(memory.RAM[Constants.PCL_B1] +1);
        ChangeBoth(memory, Constants.PCL_B1, wert);
    }

    private void RETFIE(Memory memory) //return from interrupt
    {

    }

    private void RETLW (Memory memory, int literal) //return with literal in w 
    {
        Console.WriteLine("retlw {0}", literal);
    }

    private async Task RETURN(Memory memory) //return from subroutine
    {
        Console.WriteLine("return");
    }

    private void SLEEP(Memory memory) // go into standby mode
    {

    }

    private void SUBLW(Memory memory, int literal) // subtract w from literal
    {
        int result = literal - memory.W_reg; 
        check_DC_C(memory, literal, memory.W_reg, "-");        
        checkZ(memory, result);

        if (result < 0)
	    {
            result = result + 255;
	    }

        memory.W_reg = result;

        //cycles: 1
    }

    private void XORLW (Memory memory, int literal) //exclusive OR literal with w
    {

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
                    await RETURN(memory); //Return from Subroutine
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

    private async Task AnalyzeNibble3(Memory memory)
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

    private async Task AnalyzeBits11_12(Memory memory) //bit-oriented operations genauer analysieren
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
    private async Task AnalyzeNibble2Literal(Memory memory) 
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
    private async Task AnalyzeNibble2Byte(Memory memory) 
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

    private void checkZ (Memory memory, int result)
    {
        if (result == 0)
	    {
            byte  wert = Convert.ToByte( memory.RAM[Constants.STATUS_B1] | 0b_0000_0100);
            ChangeBoth(memory, Constants.STATUS_B1, wert);
	    }
        else
	    {
            byte  wert = Convert.ToByte( memory.RAM[Constants.STATUS_B1] & 0b_1111_1011);
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
                byte  wert = Convert.ToByte( memory.RAM[Constants.STATUS_B1] | 0b_0000_0010);
                ChangeBoth(memory, Constants.STATUS_B1, wert);
	        }
            else
	        {
                byte  wert = Convert.ToByte( memory.RAM[Constants.STATUS_B1] & 0b_1111_1101);
                ChangeBoth(memory, Constants.STATUS_B1, wert);

	        }
            if ((literal1 + literal2) > 255) // Carry prüfen
	        {
                byte  wert = Convert.ToByte( memory.RAM[Constants.STATUS_B1] | 0b_0000_0001);
                ChangeBoth(memory, Constants.STATUS_B1, wert);
	        }
            else
	        {
                byte  wert = Convert.ToByte( memory.RAM[Constants.STATUS_B1] & 0b_1111_1110);
                ChangeBoth(memory, Constants.STATUS_B1, wert);
	        }
	    }
        //dann darf nur noch "-" im operanden stehen (wenn richtig aufgerufen)
        //hier hat der pic umgekehrte Logik, da die Entwickler ein invertieren vergessen haben (siehe Themenblatt)
        if ((literal1low-literal2low) < 0) // DigitCarry prüfen
	    {
            byte  wert = Convert.ToByte( memory.RAM[Constants.STATUS_B1] & 0b_1111_1101);
            ChangeBoth(memory, Constants.STATUS_B1, wert);

	    }
        else
	    {
            byte  wert = Convert.ToByte( memory.RAM[Constants.STATUS_B1] | 0b_0000_0010);
            ChangeBoth(memory, Constants.STATUS_B1, wert);
 
	    }
        if ((literal1 - literal2) < 0) //Carry prüfen
	    {
            byte  wert = Convert.ToByte( memory.RAM[Constants.STATUS_B1] & 0b_1111_1110);
            ChangeBoth(memory, Constants.STATUS_B1, wert);

	    }
        else
	    {
            byte  wert = Convert.ToByte( memory.RAM[Constants.STATUS_B1] | 0b_0000_0001);
            ChangeBoth(memory, Constants.STATUS_B1, wert);

	    }
    }

    public CommandService()
    {

    }

}
