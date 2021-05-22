using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Threading;
using System.Windows.Shapes;
using Application.Constants;
using Application.Models.ApplicationLogic;
using Application.Models.CodeLogic;
using Application.Models.Memory;
using Application.Models.Utility;
using Application.Models.ViewLogic;

namespace Application.ViewModel
{
    /// <summary>
    /// MainViewModel Class that contains commands and properties for the MainViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region fields
        private IDialogService _dialogService;
        private IApplicationService _applicationService;
        private IFileService _fileService;
        private Thread _threadRun;
        #endregion

        #region Properties

        public short WReg
        {
            get
            {
                return _memory.WReg;
            }
            set
            {
                _memory.WReg = value;
                RaisePropertyChanged();
            }
        }

        public int ZFlag
        {
            get
            {
                return _memory.ZFlag;
            }
        }

        public int DCFlag
        {
            get
            {
                return _memory.DCFlag;
            }
        }

        public int CFlag
        {
            get
            {
                return _memory.CFlag;
            }
        }


        public IRAMModel RAM
        {
            get
            {
                return _memory.RAM;
            }
            set
            {
                _memory.RAM = value;
                RaisePropertyChanged();
            }
        }

        public byte TRISAValue
        {
            get
            {
                return _memory.TRISAValue;
            }
            set
            {
                _memory.TRISAValue = value;
                RaisePropertyChanged();
            }
        }

        public byte PortAValue
        {
            get
            {
                return _memory.PortAValue;
            }
            set
            {
                _memory.PortAValue = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin0
        {
            get
            {
                return _memory.PortAPin0;
            }
            set
            {
                _memory.PortAPin0 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin1
        {
            get
            {
                return _memory.PortAPin1;
            }
            set
            {
                _memory.PortAPin1 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin2
        {
            get
            {
                return _memory.PortAPin2;
            }
            set
            {
                _memory.PortAPin2 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin3
        {
            get
            {
                return _memory.PortAPin3;
            }
            set
            {
                _memory.PortAPin3 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin4
        {
            get
            {
                return _memory.PortAPin4;
            }
            set
            {
                _memory.PortAPin4 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin5
        {
            get
            {
                return _memory.PortAPin5;
            }
            set
            {
                _memory.PortAPin5 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin6
        {
            get
            {
                return _memory.PortAPin6;
            }
            set
            {
                _memory.PortAPin6 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin7
        {
            get
            {
                return _memory.PortAPin7;
            }
            set
            {
                _memory.PortAPin7 = value;
                RaisePropertyChanged();
            }
        }

        public byte TRISBValue
        {
            get
            {
                return _memory.TRISBValue;
            }
            set
            {
                _memory.TRISBValue = value;
                RaisePropertyChanged();
            }
        }

        public byte PortBValue
        {
            get
            {
                return _memory.PortBValue;
            }
            set
            {
                _memory.PortBValue = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin0
        {
            get
            {
                return _memory.PortBPin0;
            }
            set
            {
                _memory.PortBPin0 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin1
        {
            get
            {
                return _memory.PortBPin1;
            }
            set
            {
                _memory.PortBPin1 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin2
        {
            get
            {
                return _memory.PortBPin2;
            }
            set
            {
                _memory.PortBPin2 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin3
        {
            get
            {
                return _memory.PortBPin3;
            }
            set
            {
                _memory.PortBPin3 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin4
        {
            get
            {
                return _memory.PortBPin4;
            }
            set
            {
                _memory.PortBPin4 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin5
        {
            get
            {
                return _memory.PortBPin5;
            }
            set
            {
                _memory.PortBPin5 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin6
        {
            get
            {
                return _memory.PortBPin6;
            }
            set
            {
                _memory.PortBPin6 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin7
        {
            get
            {
                return _memory.PortBPin7;
            }
            set
            {
                _memory.PortBPin7 = value;
                RaisePropertyChanged();
            }
        }

        public Stack<short> PCStack
        {
            get
            {
                return _memory.PCStack;
            }
        }

        public double Quartz
        {
            get
            {
                return _memory.Quartz;
            }
            set
            {
                _memory.Quartz = value;
                RaisePropertyChanged();
            }
        }

        public double Runtime
        {
            get
            {
                return _memory.Runtime;
            }
        }

        public int PCWithoutClear
        {
            get
            {
                return _memory.PCWithoutClear;
            }
        }

        public byte PCL
        {
            get
            {
                return _memory.PCL;
            }
        }

        public byte PCLATH
        {
            get
            {
                return _memory.PCLATH;
            }
        }

        private IMemoryService _memory;
        public IMemoryService Memory
        {
            get => _memory;
            set
            {
                _memory = value;
                RaisePropertyChanged();
            }
        }

        private ISourceFileModel<ILineOfCode> _srcFileModel;
        public ISourceFileModel<ILineOfCode> SrcFileModel
        {
            get => _srcFileModel;
            set 
            {
                _srcFileModel = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        private RelayCommand _openCommand;

        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand
                    ?? (_openCommand = new RelayCommand(
                        () =>
                        {
                            DebugCodes.Pause = true;
                            Memory.PowerReset();
                            SrcFileModel.SourceFile = _dialogService.Open();
                            //TODO: was passiert wenn ein SrcFileModel �berschrieben wird?
                            _fileService.CreateFileList(SrcFileModel);
                        }));
            }
        }

        private RelayCommand _runCommand;

        public RelayCommand RunCommand
        {
            get
            {
                return _runCommand
                    ?? (_runCommand = new RelayCommand(
                        () =>
                        {
                            if (SrcFileModel.ListOfCode != null)
                            {
                                SrcFileModel[Memory.RAM.PCWithoutClear].IsDebug = false;
                                DebugCodes.Pause = false;
                                if (_threadRun.ThreadState == System.Threading.ThreadState.Unstarted)
                                {
                                    _threadRun.Start();
                                }
                            }
                        }));
            }
        }

        private RelayCommand _pauseCommand;

        public RelayCommand PauseCommand
        {
            get
            {
                return _pauseCommand
                    ?? (_pauseCommand = new RelayCommand(
                        () =>
                        {
                            DebugCodes.Pause = true;
                            
                        }));
            }
        }

        private RelayCommand _singleStepCommand;

        public RelayCommand SingleStepCommand
        {
            get
            {
                return _singleStepCommand
                    ?? (_singleStepCommand = new RelayCommand(
                        () =>
                        {
                            if (SrcFileModel.ListOfCode != null)
                            {
                                // hier wird PC_without_Clear genommen, der die gestetzten Flags nicht l�scht 
                                if ((SrcFileModel[Memory.RAM.PCWithoutClear].ProgramCode & 0b_0011_0000_0000_0000) == 8192) //auf call & goto pr�fen
                                {
                                    //sprungadresse debug = true
                                    int adress = SrcFileModel[Memory.RAM.PCWithoutClear].ProgramCode & 0b0000_0111_1111_1111;
                                    SrcFileModel[adress].IsDebug = true;
                                }
                                else if (SrcFileModel[Memory.RAM.PCWithoutClear].ProgramCode == 0b0000_0000_0000_1001 //retfie
                                    || SrcFileModel[Memory.RAM.PCWithoutClear].ProgramCode == 0b0000_0000_0000_1000 //return
                                    || (SrcFileModel[Memory.RAM.PCWithoutClear].ProgramCode & 0b0011_1100_0000_0000) == 0b0011_0100_0000_0000) //retlw
                                {
                                    SrcFileModel[Memory.PCStack.Peek()].IsDebug = true;
                                }
                                //skip-befehle pr�fen
                                else if ((SrcFileModel[Memory.RAM.PCWithoutClear].ProgramCode & 0b_0011_1111_0000_0000) == 0b_0000_1011_0000_0000 //decfsz
                                    || (SrcFileModel[Memory.RAM.PCWithoutClear].ProgramCode & 0b_0011_1111_0000_0000) == 0b_0000_1111_0000_0000 //incfsz
                                    || (SrcFileModel[Memory.RAM.PCWithoutClear].ProgramCode & 0b_0011_1100_0000_0000) == 0b_0001_1000_0000_0000 //btfsc
                                    || (SrcFileModel[Memory.RAM.PCWithoutClear].ProgramCode & 0b_0011_1100_0000_0000) == 0b_0001_1000_0000_0000 //btfss
                                    ) 
                                {
                                    //nicht ganz korrekt, aber der einfachheit halber n�chste und �bern�chste adresse debuggen
                                    SrcFileModel[Memory.RAM.PCWithoutClear + 1].IsDebug = true;
                                    SrcFileModel[Memory.RAM.PCWithoutClear + 2].IsDebug = true;
                                }
                                //auf manipulation des PCL pr�fen (Ziel-File-Adresse der Operation == PCL && d-bit == 1)
                                else if ((SrcFileModel[Memory.RAM.PCWithoutClear].ProgramCode & 0b0000_0000_1000_0000) > 0
                                    && ( ((SrcFileModel[Memory.RAM.PCWithoutClear].ProgramCode & 0b0000_0000_0111_1111) == 0x_02) 
                                        || ((SrcFileModel[Memory.RAM.PCWithoutClear].ProgramCode & 0b0000_0000_0111_1111) == 0x_82))) //wenn die Ziel-File-Adresse der Operation der PCL ist
                                {
                                    //dann muss PC = PCLATH + PCL (nach operation) benutzt werden
                                    SrcFileModel[Memory.RAM[MemoryConstants.PCL_B1] + Memory.RAM[MemoryConstants.PCLATH_B1]].IsDebug = true; //PCL vor operation wird genommen -> falsch
                                }
                                else
                                {
                                    SrcFileModel[Memory.RAM.PCWithoutClear + 1].IsDebug = true;
                                }
                                _runCommand.Execute(null);
                            }
                           
                        }));
            }
        }

        private RelayCommand _resetCommand;

        public RelayCommand ResetCommand
        {
            get
            {
                return _resetCommand
                    ?? (_resetCommand = new RelayCommand(
                        () =>
                        {
                            if(SrcFileModel.ListOfCode!=null)
                            {
                                DebugCodes.Pause = true;

                                SrcFileModel[Memory.RAM.PCWithoutClear + 1].IsDebug = true;
                                _fileService.Reset(SrcFileModel);
                            }
                            Memory.PowerReset();
                        }));
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            // make dependency bag singleton
            IDependencyBag bag = new DependencyBag();
            bag.Create();
            _memory = bag.Memory;
            _srcFileModel = bag.SourceFile;
            _fileService = bag.FileService;
            _dialogService = bag.DialogService;
            _applicationService = bag.ApplicationService;

            DebugCodes.Pause = false;
            DebugCodes.Reset = false;

            ThreadStart start = new ThreadStart(_applicationService.Run);
            _threadRun = new Thread(start);
            this._memory.PropertyChanged += _memory_PropertyChanged;
           
        }

        private void _memory_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }
    }
}