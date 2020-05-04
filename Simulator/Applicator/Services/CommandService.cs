using Application.Model;
using Applicator.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

public class CommandService : ICommandService
{
    #region byte-oriented file register operations
    private void ADDWF(int file, int d) //add w and f
    {
        
    }

    private void ANDWF (int file, int d) //and w and f
    {

    }

    private void CLRF (int file) //clear f
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

    private void INCF (int file, int d) //increment f
    {

    }

    private void INCFSZ(int file, int d) //increment f, skip if zero
    {

    }

    private void IORWF (int file, int d) // inclusive OR w with f
    {

    }

    private void MOVF (int file, int d) // move f
    {

    }

    private void MOVWF (int file) // move w to f
    {

    }

    private void NOP () // no operation
    {
        Console.WriteLine("nop");
    }

    private void RLF (int file, int d) // rotate left through carry
    {

    }

    private void RRF (int file, int d) // rotate right through carry
    {

    }

    private void SUBWF (int file, int d) // substract w from f
    {

    }

    private void SWAPF(int file, int d) // swap nibbles in f
    {

    }

    private void XORWF (int file, int d) //exclusive OR w with f
    {

    }

    #endregion byte-oriented file register operations

    #region bit-oriented file register operations
    private void BCF (int file, int bits) //bit clear f
    {

    }

    private void BSF (int file, int bits)//bit set f
    {

    }

    private void BTFSC (int file, int bits) //bit test f, skip if clear
    {

    }

    private void BTFSS (int file, int bits) //bit test f, skip if set
    {

    }

    #endregion

    #region literal and control operations
    private void ADDLW (int literal) //add literal and w
    {

    }

    private void ANDLW(int literal) //and literal and w
    {

    }

    private void CALL(int address) // call subroutine
    {

    }

    private void CLRWDT() //clear watchdog timer
    {

    }

    private void GOTO (int address) //go to address
    {

    }

    private void IORLW (int literal) //inclusive OR literal with w
    {

    }

    private void MOVLW (int literal) //move literal to w
    {

    }

    private void RETFIE() //return from interrupt
    {

    }

    private void RETLW (int literal) //return with literal in w 
    {

    }

    private void RETURN() //return from subroutine
    {

    }

    private void SLEEP() // go into standby mode
    {

    }

    private void SUBLW(int literal) // subtract w from literal
    {

    }

    private void XORLW (int literal) //exclusive OR literal with w
    {

    }
    #endregion


    public async Task Run(Memory Memory, List<int> breakpointList)
    {
        for (int i = 0; i < Constants.PROGRAM_MEMORY_SIZE; i++)
        {
            switch (Memory.Program[i])
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
                if (bit12 == 1)
                {
                    GOTO(address);
                }
                else
                {
                    CALL(address);
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
    private void AnalyzeNibble2Literal(short befehl)
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
    private void AnalyzeNibble2Byte(short befehl)
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

    
    public CommandService()
    {

    }

}
