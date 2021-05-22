using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Constants;
using Application.Models.ApplicationLogic;
using Application.Models.CodeLogic;
using Application.Models.CustomDatastructures;
using Application.Models.Memory;
using Application.Models.OperationLogic;
using Application.Models.ViewLogic;

namespace Application.Models.Utility
{
    public interface IDependencyBag
    {
        IMemoryService Memory { get; }
        ISourceFileModel SourceFile { get; }
        IFileService FileService { get; }
        IDialogService DialogService { get; }
        IApplicationService ApplicationService { get; }
        IRAMModel RAM { get; }
        IPort PortA { get; }
        IPort PortB { get; }
        Stack<short> PCStack { get; }
        void Create();
    }

    public class DependencyBag : IDependencyBag
    {
        public IMemoryService Memory
        {
            get;
            private set;
        }
        public ISourceFileModel SourceFile
        {
            get;
            private set;
        }
        public IFileService FileService
        {
            get;
            private set;
        }
        public IDialogService DialogService
        {
            get;
            private set;
        }
        public IApplicationService ApplicationService
        {
            get;
            private set;
        }
        public IRAMModel RAM
        {
            get;
            private set;
        }
        public IPort PortA
        {
            get;
            private set;
        }
        public IPort PortB
        {
            get;
            private set;
        }
        public Stack<short> PCStack
        {
            get;
            private set;
        }
        private BitOperations BitOperations;
        private ByteOperations ByteOperations;
        private LiteralControlOperations LiteralControlOperations;
        private OperationHelpers OperationHelpers;

        public void Create()
        {
            //TODO: set this to timer port when it is implemented
            PortA = new Port();
            PortB = new Port();
            RAM = new RAMModel(PortA, PortB);
            PCStack = new ObservableStack<short>(new Stack<short>(MemoryConstants.PC_STACK_CAPACITY));
            Memory = new MemoryService(RAM,PCStack);
            SourceFile = new SourceFileModel();
            FileService = new FileService();
            DialogService = new DialogService();
            OperationHelpers = new OperationHelpers(Memory);
            BitOperations = new BitOperations(Memory);
            ByteOperations = new ByteOperations(Memory);
            LiteralControlOperations = new LiteralControlOperations(Memory);
            ApplicationService = new ApplicationService(Memory, SourceFile, OperationHelpers, BitOperations, ByteOperations, LiteralControlOperations);
        }
    }
}
