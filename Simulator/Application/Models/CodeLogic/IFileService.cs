namespace Application.Models.CodeLogic
{
    public interface IFileService
    {
        void CreateFileList(SourceFileModel src);
        void Reset(SourceFileModel src);
    }
}
