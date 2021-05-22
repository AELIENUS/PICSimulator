using System.Collections.Generic;
using Application.Models.ApplicationLogic;
using Application.Models.CodeLogic;
using Application.Models.Memory;
using Application.Models.ViewLogic;

namespace Application.Models.Utility
{
    public interface IDependencyBag
    {
        IMemoryService Memory { get; }
        ISourceFileModel<ILineOfCode> SourceFile { get; }
        IFileService FileService { get; }
        IDialogService DialogService { get; }
        IApplicationService ApplicationService { get; }
        IRAMModel RAM { get; }
        IPort PortA { get; }
        IPort PortB { get; }
        Stack<short> PCStack { get; }
        void Create();
    }
}