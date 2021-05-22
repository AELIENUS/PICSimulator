namespace Application.Models.CodeLogic
{
    public interface IFileService
    {
        void CreateFileList(ISourceFileModel src);
        void Reset(ISourceFileModel src);
    }
}
