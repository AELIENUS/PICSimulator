using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Application.Models.CodeLogic
{
    public class SourceFileModel: ObservableObject, ISourceFileModel<ILineOfCode>
    {

        private string _sourceFile;

        public string SourceFile
        {
            get
            {
                return _sourceFile;
            }
            set
            {
                if (_sourceFile == value)
                {
                    return;
                }
                _sourceFile = value;
                //Notification an GUI
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ILineOfCode> _listOfCode;

        public ObservableCollection<ILineOfCode> ListOfCode
        {
            get
            {
                return _listOfCode;
            }
            set
            {
                if(_listOfCode == null)
                {
                    _listOfCode = new ObservableCollection<ILineOfCode>();
                }
                _listOfCode = value;
                RaisePropertyChanged();
            }
        }
        

        [IndexerName("LoC")]
        public ILineOfCode this[int commandIndex]
        {
            get
            {
                for (int i = 0; i < ListOfCode.Count; i++)
                {
                    if (commandIndex == ListOfCode[i].CommandIndex)
                    {
                        return ListOfCode[i];
                    }
                }
                //hier sollte man theoretisch nie hinkommen
                return new LineOfCode();
            }
            set
            {
                for (int i = 0; i < ListOfCode.Count; i++)
                {
                    if (commandIndex == ListOfCode[i].CommandIndex)
                    {
                        ListOfCode[i] = value;
                    }
                }
                RaisePropertyChanged("LoC");
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public SourceFileModel()
        {
        }
    }
}
