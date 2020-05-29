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
            _commandService = new CommandService(memory, SrcFileModel, BreakpointList);

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