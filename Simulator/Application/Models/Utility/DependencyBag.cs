using System;
using System.Collections.Generic;
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
    public class DependencyBag
    {
        public MemoryService Memory;
        public SourceFileModel SourceFile;
        public FileService FileService;
        public DialogService DialogService;
        public ApplicationService ApplicationService;
        public BitOperations BitOperations;
        public ByteOperations ByteOperations;
        public LiteralControlOperations LiteralControlOperations;
        public OperationHelpers OperationHelpers;
        public ObservableStack<short> PCStack;
        public RAMModel RAM;
        public Port PortA;
        public Port PortB;

        public DependencyBag()
        {
                
        }

        public void Create()
        {
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
