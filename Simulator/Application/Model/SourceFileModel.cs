using Applicator.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace Application.Model
{
    public class SourceFileModel: ObservableObject
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

        private ObservableCollection<LineOfCode> _ListOfCode;

        public ObservableCollection<LineOfCode> ListOfCode
        {
            get
            {
                return _ListOfCode;
            }
            set
            {
                if(_ListOfCode == null)
                {
                    _ListOfCode = new ObservableCollection<LineOfCode>();
                }
                _ListOfCode = value;
                RaisePropertyChanged();
            }
        }
        

        [IndexerName("lol")]
        public LineOfCode this[int commandIndex]
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
                RaisePropertyChanged("lol");
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
