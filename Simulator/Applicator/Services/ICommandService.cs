using Application.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICommandService
{
    Task AnalyzeCommand (Memory Memory, List<int> breakpointList);
}
