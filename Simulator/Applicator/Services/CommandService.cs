using Applicator.Model;
using Applicator.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CommandService : ICommandService
{
    private void MOVLW ()
    {

    }

    private void RETFIE()
    {

    }

    public async Task Run(Memory Memory, List<int> breakpointList)
    {
        for (int i = 0; i < Constants.PROGRAM_MEMORY_SIZE; i++)
        {
            switch (Memory.Program[i])
            {
                case 9:
                    RETFIE(); //return from interrupt
                    break;
                default:
                    break;
            }
        }
    }

    public CommandService()
	{

	}
}
