namespace Application.Models.CodeLogic
{
    public interface ILineOfCode
    {
        string Line { get; set; }
        bool IsExecuted { get; set; }
        bool IsDebug { get; set; }
        short ProgramCode { get; set; }
        int CommandIndex { get; set; }
    }
}