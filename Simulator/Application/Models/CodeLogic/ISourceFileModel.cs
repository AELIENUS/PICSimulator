using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Application.Models.CodeLogic
{
    public interface ISourceFileModel<T> : ICodeToExecute<T>
    {
        string SourceFile { get; set; }
        ObservableCollection<T> ListOfCode { get; set; }
    }
}