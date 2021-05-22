using System.Collections.ObjectModel;

namespace Application.Models.CodeLogic
{
    public interface ISourceFileModel
    {
        string SourceFile { get; set; }
        ObservableCollection<LineOfCode> ListOfCode { get; set; }
        LineOfCode this[int commandIndex] { get; set; }
    }
}