using System;
using Application.Model;
using Application.Services;

public class OperationHelpers
{
    private Memory _memory;
    private SourceFileModel _srcModel;

    public OperationHelpers(Memory memory, SourceFileModel srcModel)
    {
        _memory = memory;
        _srcModel = srcModel;
    }

    public void checkZ(int result)
    {
        byte wert;
        if (result == 0)
        {
            wert = Convert.ToByte(_memory.RAM[Constants.STATUS_B1] | 0b_0000_0100);
        }
        else
        {
            wert = Convert.ToByte(_memory.RAM[Constants.STATUS_B1] & 0b_1111_1011);
        }

        _memory.RAM[Constants.STATUS_B1] = wert;
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
                wert = Convert.ToByte(_memory.RAM[Constants.STATUS_B1] | 0b_0000_0010);
            }
            else
            {
                wert = Convert.ToByte(_memory.RAM[Constants.STATUS_B1] & 0b_1111_1101);
            }

            _memory.RAM[Constants.STATUS_B1] = wert;

            if ((literal1 + literal2) > 255) // Carry prüfen
            {
                wert = Convert.ToByte(_memory.RAM[Constants.STATUS_B1] | 0b_0000_0001);
            }
            else
            {
                wert = Convert.ToByte(_memory.RAM[Constants.STATUS_B1] & 0b_1111_1110);
            }

            _memory.RAM[Constants.STATUS_B1] = wert;
        }

        else //op = "-"
        {
            //hier hat der pic umgekehrte Logik, da die Entwickler ein invertieren vergessen haben (siehe Themenblatt)
            if ((literal1low - literal2low) < 0) // DigitCarry prüfen
            {
                wert = Convert.ToByte(_memory.RAM[Constants.STATUS_B1] & 0b_1111_1101);
            }
            else
            {
                wert = Convert.ToByte(_memory.RAM[Constants.STATUS_B1] | 0b_0000_0010);
            }

            _memory.RAM[Constants.STATUS_B1] = wert;

            if ((literal1 - literal2) < 0) //Carry prüfen
            {
                wert = Convert.ToByte(_memory.RAM[Constants.STATUS_B1] & 0b_1111_1110);
            }
            else
            {
                wert = Convert.ToByte(_memory.RAM[Constants.STATUS_B1] | 0b_0000_0001);
            }

            _memory.RAM[Constants.STATUS_B1] = wert;
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
            _memory.W_Reg = Convert.ToByte(result);
        }
        else
        {
            if (file == 0x02)
            {
                _srcModel.ListOfCode[_memory.RAM.PC_Without_Clear].IsExecuted = false;
                _memory.RAM.PCL_was_Manipulated = true;
            }
            //result stored in f
            _memory.RAM[file] = Convert.ToByte(result);
        }
    }

    public void ChangePC_Fetch (byte wert)
    {
        _srcModel[_memory.RAM.PC_Without_Clear].IsExecuted = false;
        if (_memory.RAM[Constants.PCL_B1] == 0b_1111_1111)
        {
            _memory.RAM[Constants.PCL_B1] = (byte)(0 + wert);
            //PCLATH erhöhen
            _memory.RAM[Constants.PCLATH_B1] += 1;
        }
        else
        {
            _memory.RAM[Constants.PCL_B1] += wert;
        }
    }
}