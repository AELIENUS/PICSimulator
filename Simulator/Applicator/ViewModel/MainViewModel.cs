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

        private List<int> BreakpointList;

        private IDialogService _dialogService;
        private ICommandService _commandService;
        private IFileService _fileService;
        private Task taskRun;
        private Thread ThreadRun;




        #endregion

        #region Commands

        #region Change PortA
        private RelayCommand<bool> _PORTABit7;

        public RelayCommand<bool> PORTABit7
        {
            get
            {
                if(_PORTABit7 == null)
                {
                    _PORTABit7 = new RelayCommand<bool>(param => ChangePORTABit7(param));
                }
                return _PORTABit7;
            }
        }

        public void ChangePORTABit7(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if(convertedParameter)
            {
                Memory.RAM[5] |= 0b1000_0000;
                
            }
            else
            {
                Memory.RAM[5] &= 0b0111_1111;
            }
        }

        private RelayCommand<bool> _PORTABit6;

        public RelayCommand<bool> PORTABit6
        {
            get
            {
                if (_PORTABit6 == null)
                {
                    _PORTABit6 = new RelayCommand<bool>(param => ChangePORTABit6(param));
                }
                return _PORTABit6;
            }
        }

        public void ChangePORTABit6(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[5] |= 0b0100_0000;

            }
            else
            {
                Memory.RAM[5] &= 0b1011_1111;
            }
        }
        private RelayCommand<bool> _PORTABit5;

        public RelayCommand<bool> PORTABit5
        {
            get
            {
                if (_PORTABit5 == null)
                {
                    _PORTABit5 = new RelayCommand<bool>(param => ChangePORTABit5(param));
                }
                return _PORTABit5;
            }
        }

        public void ChangePORTABit5(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[5] |= 0b0010_0000;

            }
            else
            {
                Memory.RAM[5] &= 0b1101_1111;
            }
        }
        private RelayCommand<bool> _PORTABit4;

        public RelayCommand<bool> PORTABit4
        {
            get
            {
                if (_PORTABit4 == null)
                {
                    _PORTABit4 = new RelayCommand<bool>(param => ChangePORTABit4(param));
                }
                return _PORTABit4;
            }
        }

        public void ChangePORTABit4(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[5] |= 0b0001_0000;

            }
            else
            {
                Memory.RAM[5] &= 0b1110_1111;
            }
        }
        private RelayCommand<bool> _PORTABit3;

        public RelayCommand<bool> PORTABit3
        {
            get
            {
                if (_PORTABit3 == null)
                {
                    _PORTABit3 = new RelayCommand<bool>(param => ChangePORTABit3(param));
                }
                return _PORTABit3;
            }
        }

        public void ChangePORTABit3(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[5] |= 0b0000_1000;

            }
            else
            {
                Memory.RAM[5] &= 0b1111_0111;
            }
        }
        private RelayCommand<bool> _PORTABit2;

        public RelayCommand<bool> PORTABit2
        {
            get
            {
                if (_PORTABit2 == null)
                {
                    _PORTABit2 = new RelayCommand<bool>(param => ChangePORTABit2(param));
                }
                return _PORTABit2;
            }
        }

        public void ChangePORTABit2(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[5] |= 0b0000_0100;

            }
            else
            {
                Memory.RAM[5] &= 0b1111_1011;
            }
        }

        private RelayCommand<bool> _PORTABit1;

        public RelayCommand<bool> PORTABit1
        {
            get
            {
                if (_PORTABit1 == null)
                {
                    _PORTABit1 = new RelayCommand<bool>(param => ChangePORTABit1(param));
                }
                return _PORTABit1;
            }
        }

        public void ChangePORTABit1(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[5] |= 0b0000_0010;

            }
            else
            {
                Memory.RAM[5] &= 0b1111_1101;
            }
        }

        private RelayCommand<bool> _PORTABit0;

        public RelayCommand<bool> PORTABit0
        {
            get
            {
                if (_PORTABit0 == null)
                {
                    _PORTABit0 = new RelayCommand<bool>(param => ChangePORTABit0(param));
                }
                return _PORTABit0;
            }
        }

        public void ChangePORTABit0(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[5] |= 0b0000_0001;

            }
            else
            {
                Memory.RAM[5] &= 0b1111_1110;
            }
        }
        #endregion

        #region ChangePortB

        private RelayCommand<bool> _PORTBBit7;

        public RelayCommand<bool> PORTBBit7
        {
            get
            {
                if (_PORTBBit7 == null)
                {
                    _PORTBBit7 = new RelayCommand<bool>(param => ChangePORTBBit7(param));
                }
                return _PORTBBit7;
            }
        }

        public void ChangePORTBBit7(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[6] |= 0b1000_0000;

            }
            else
            {
                Memory.RAM[6] &= 0b0111_1111;
            }
        }

        private RelayCommand<bool> _PORTBBit6;

        public RelayCommand<bool> PORTBBit6
        {
            get
            {
                if (_PORTBBit6 == null)
                {
                    _PORTBBit6 = new RelayCommand<bool>(param => ChangePORTBBit6(param));
                }
                return _PORTBBit6;
            }
        }

        public void ChangePORTBBit6(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[6] |= 0b0100_0000;

            }
            else
            {
                Memory.RAM[6] &= 0b1011_1111;
            }
        }

        private RelayCommand<bool> _PORTBBit5;

        public RelayCommand<bool> PORTBBit5
        {
            get
            {
                if (_PORTBBit5 == null)
                {
                    _PORTBBit5 = new RelayCommand<bool>(param => ChangePORTBBit5(param));
                }
                return _PORTBBit5;
            }
        }

        public void ChangePORTBBit5(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[6] |= 0b0010_0000;

            }
            else
            {
                Memory.RAM[6] &= 0b1101_1111;
            }
        }
        private RelayCommand<bool> _PORTBBit4;

        public RelayCommand<bool> PORTBBit4
        {
            get
            {
                if (_PORTBBit4 == null)
                {
                    _PORTBBit4 = new RelayCommand<bool>(param => ChangePORTBBit4(param));
                }
                return _PORTBBit4;
            }
        }

        public void ChangePORTBBit4(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[6] |= 0b0001_0000;

            }
            else
            {
                Memory.RAM[6] &= 0b1110_1111;
            }
        }

        private RelayCommand<bool> _PORTBBit3;

        public RelayCommand<bool> PORTBBit3
        {
            get
            {
                if (_PORTBBit3 == null)
                {
                    _PORTBBit3 = new RelayCommand<bool>(param => ChangePORTBBit3(param));
                }
                return _PORTBBit3;
            }
        }

        public void ChangePORTBBit3(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[6] |= 0b0000_1000;

            }
            else
            {
                Memory.RAM[6] &= 0b1111_0111;
            }
        }
        private RelayCommand<bool> _PORTBBit2;

        public RelayCommand<bool> PORTBBit2
        {
            get
            {
                if (_PORTBBit2 == null)
                {
                    _PORTBBit2 = new RelayCommand<bool>(param => ChangePORTBBit2(param));
                }
                return _PORTBBit2;
            }
        }

        public void ChangePORTBBit2(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[6] |= 0b0000_0100;

            }
            else
            {
                Memory.RAM[6] &= 0b1111_1011;
            }
        }

        private RelayCommand<bool> _PORTBBit1;

        public RelayCommand<bool> PORTBBit1
        {
            get
            {
                if (_PORTBBit1 == null)
                {
                    _PORTBBit1 = new RelayCommand<bool>(param => ChangePORTBBit1(param));
                }
                return _PORTBBit1;
            }
        }

        public void ChangePORTBBit1(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[6] |= 0b0000_0010;

            }
            else
            {
                Memory.RAM[6] &= 0b1111_1101;
            }
        }

        private RelayCommand<bool> _PORTBBit0;

        public RelayCommand<bool> PORTBBit0
        {
            get
            {
                if (_PORTBBit0 == null)
                {
                    _PORTBBit0 = new RelayCommand<bool>(param => ChangePORTBBit0(param));
                }
                return _PORTBBit0;
            }
        }

        public void ChangePORTBBit0(object parameter)
        {
            bool convertedParameter = (bool)parameter;
            if (convertedParameter)
            {
                Memory.RAM[6] |= 0b0000_0001;

            }
            else
            {
                Memory.RAM[6] &= 0b1111_1110;
            }
        }


        #endregion

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
                            Memory.Program = _fileService.ParseFile(SrcFileModel.SourceFile, Memory.Program);
                            //TODO: was passiert wenn ein SrcFileModel überschrieben wird?
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
                            if (ThreadRun.ThreadState == System.Threading.ThreadState.Suspended)
                            {
                                ThreadRun.Resume();
                            }
                            else if (ThreadRun.ThreadState == System.Threading.ThreadState.Unstarted)
                            {
                                ThreadRun.Start();
                            }
                            
                            //taskRun.Start();
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
                            ThreadRun.Suspend();
                            
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
                            //nächstes PC in Breakpoint liste 
                            //taskRun.Start();
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
                            ThreadRun.Suspend();
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
            _commandService = new CommandService(memory, BreakpointList);

            object[] param = new object[2];

            ThreadStart Start = new ThreadStart(_commandService.Run);
            ThreadRun = new Thread(Start);
            //taskRun = new Task(() => { _commandService.Run(Memory, new List<int>()); }, tokenSource.Token);
        }

        public MainViewModel() : 
            this(new SourceFileModel(), new Memory(), new FileService(), new DialogService()) 
        {

        }
    }
}