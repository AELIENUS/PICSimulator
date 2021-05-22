using System.Windows.Shapes;

namespace Application.Models.CodeLogic
{
    public interface IFileService
    {
        void CreateFileList(ISourceFileModel<ILineOfCode> src);
        void Reset(ISourceFileModel<ILineOfCode> src);
    }
}
