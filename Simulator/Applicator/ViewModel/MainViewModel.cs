using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.ComponentModel;
using Application.Model;
using Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Collections.ObjectModel;

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

        private IDialogService _dialogService;
        private ICommandService _commandService;
        private IFileService _fileService;

        #endregion

        #region StyleInfo
        private ObservableCollection<string> RowStyleInfo
        {
            get
            {
                if (RowStyleInfo ==null)
                {
                    string[] arr =
                    {
                        "0x0C", "Ox1C", "0x2C", "0x3C", "0x4C", "0x5C", "0x6C", "0x7C", "0x8C", "0x9C", "0xAC", "0xBC", "0xCC", "0xDC", "0xEC", "0xFC" 
                    };
                    return new ObservableCollection<string>(arr);
                }
                return RowStyleInfo;
            }
        }
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
                            Task.Run(() => _commandService.Run(Memory, new List<int>()));
                        }));
            }
        }

        private RelayCommand _stopCommand;

        public RelayCommand StopCommand
        {
            get
            {
                return _stopCommand
                    ?? (_stopCommand = new RelayCommand(
                        () =>
                        {
                            //TODO: was passiert?
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
            IDialogService dialogService,
            ICommandService commandService
            /* hier werden services injected-> Service ist Interface im Helpers ordner*/)
        {
            _memory = memory;
            _srcFileModel = sourceFileModel;
            _fileService = fileService;
            _dialogService = dialogService;
            _commandService = commandService;
        }

        public MainViewModel() : 
            this(new SourceFileModel(), new Memory(), new FileService(), new DialogService(), new CommandService()) 
        {

        }
    }
}