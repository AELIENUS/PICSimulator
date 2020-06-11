using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.ComponentModel;
using Application.Model;
using Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Threading;
using Applicator.Model;
using System;

namespace Application.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IDialogService _dialogService;
        private ICommandService _commandService;
        private IFileService _fileService;
        private Thread ThreadRun;
        #region Properties

        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        private CancellationToken cancellationToken = tokenSource.Token;

        private Memory _memory;

        public Memory Memory 
        {
            get
            {
                return _memory;
            }
            set
            {
                if (_memory.Equals(value))
                {
                    return;
                }
                _memory = value;
                RaisePropertyChanged();
            }
        }

        private PortA _PortA;

        public PortA PortA
        {
            get
            {
                return _PortA;
            }
            set
            {
                if(_PortA.Equals(value))
                {
                    return;
                }
                _PortA = value;
                RaisePropertyChanged();
            }
        }

        private PortB _PortB;

        public PortB PortB
        {
            get
            {
                return _PortB;
            }
            set
            {
                if (_PortB.Equals(value))
                {
                    return;
                }
                _PortB = value;
                RaisePropertyChanged();
            }
        }

        private SourceFileModel _srcFileModel;

        public SourceFileModel SrcFileModel
        {
            get 
            { 
                return _srcFileModel; 
            }
            set 
            {
                if (_srcFileModel.Equals(value))
                {
                    return;
                }
                _srcFileModel = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        //jetzt werden Commands vorbereitet
        private RelayCommand _openCommand;

        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand
                    ?? (_openCommand = new RelayCommand(
                        () =>
                        {
                            SrcFileModel.SourceFile = _dialogService.Open();
                            //TODO: was passiert wenn ein SrcFileModel überschrieben wird?
                            _fileService.CreateFileList(SrcFileModel);
                            //Memory reset um Fehler vorzubeugen
                            Memory.PowerReset();
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
                                SrcFileModel[Memory.PC].IsDebug = false;
                                DebugCodes.Pause = false;
                                if (ThreadRun.ThreadState == System.Threading.ThreadState.Unstarted)
                                {
                                    ThreadRun.Start();
                                }
                            }
                        }));
            }
        }

        private RelayCommand _PauseCommand;

        public RelayCommand PauseCommand
        {
            get
            {
                return _PauseCommand
                    ?? (_PauseCommand = new RelayCommand(
                        () =>
                        {
                            DebugCodes.Pause = true;
                            
                        }));
            }
        }

        private RelayCommand _SingleStepCommand;

        public RelayCommand SingleStepCommand
        {
            get
            {
                return _SingleStepCommand
                    ?? (_SingleStepCommand = new RelayCommand(
                        () =>
                        {
                            if (SrcFileModel.ListOfCode != null)
                            {
                                if ((SrcFileModel[Memory.PC].ProgramCode & 0b_0011_0000_0000_0000) == 8192) //auf call & goto prüfen
                                {
                                    int pc = SrcFileModel[Memory.PC].ProgramCode & 0b0000_0111_1111_1111; //sprungadresse debug = treu
                                    SrcFileModel[pc].IsDebug = true;
                                }
                                else if (SrcFileModel[Memory.PC].ProgramCode == 0b0000_0000_0000_1001 //retfie
                                    | SrcFileModel[Memory.PC].ProgramCode == 0b0000_0000_0000_1000 //return
                                    | (SrcFileModel[Memory.PC].ProgramCode & 0b0011_1100_0000_0000) == 0b0011_0100_0000_0000) //retlw
                                {
                                    SrcFileModel[Memory.PCStack.Peek()].IsDebug = true;
                                }
                                //skip-befehle prüfen
                                else if ((SrcFileModel[Memory.PC].ProgramCode & 0b_0011_1111_0000_0000) == 0b_0000_1011_0000_0000 //decfsz
                                    | (SrcFileModel[Memory.PC].ProgramCode & 0b_0011_1111_0000_0000) == 0b_0000_1111_0000_0000 //incfsz
                                    | (SrcFileModel[Memory.PC].ProgramCode & 0b_0011_1100_0000_0000) == 0b_0001_1000_0000_0000 //btfsc
                                    | (SrcFileModel[Memory.PC].ProgramCode & 0b_0011_1100_0000_0000) == 0b_0001_1000_0000_0000 //btfss
                                    ) 
                                {
                                    //nicht ganz korrekt, aber der einfachheit halber nächste und übernächste adresse debuggen
                                    SrcFileModel[Memory.PC + 1].IsDebug = true;
                                    SrcFileModel[Memory.PC + 2].IsDebug = true;
                                }
                                else
                                {
                                    SrcFileModel[Memory.PC + 1].IsDebug = true;
                                }
                                _runCommand.Execute(null);
                            }
                           
                         }));
            }
        }

        private RelayCommand _ResetCommand;

        public RelayCommand ResetCommand
        {
            get
            {
                return _ResetCommand
                    ?? (_ResetCommand = new RelayCommand(
                        () =>
                        {
                            if(SrcFileModel.ListOfCode!=null)
                            {
                                DebugCodes.Pause = true;

                                SrcFileModel[Memory.PC + 1].IsDebug = true;
                                _fileService.Reset(SrcFileModel);
                            }
                            Memory.PowerReset();
                        }));
            }
        }
        #endregion

        #region Methoden
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(
            SourceFileModel sourceFileModel,
            Memory memory,
            IFileService fileService,
            IDialogService dialogService
            /* hier werden services injected-> Service ist Interface im Helpers ordner*/)
        {
            _memory = memory;
            _srcFileModel = sourceFileModel;
            _fileService = fileService;
            _dialogService = dialogService;
            _commandService = new CommandService(memory, SrcFileModel);
            _PortA = new PortA(memory);
            _PortB = new PortB(memory);

            DebugCodes.Pause = false;
            DebugCodes.Reset = false;

            ThreadStart Start = new ThreadStart(_commandService.Run);
            ThreadRun = new Thread(Start);
           
        }

        public MainViewModel() : 
            this(new SourceFileModel(), new Memory(), new FileService(), new DialogService()) 
        {

        }
    }
}