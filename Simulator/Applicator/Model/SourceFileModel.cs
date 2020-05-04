using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Application.Model
{
    public class SourceFileModel: INotifyPropertyChanged
    {
        #region PropertyChanged Teil TODO: gibt es das in MVVM light?
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if(handler!=null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

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

        /// <summary>
        /// ctor
        /// </summary>
        public SourceFileModel()
        {
        }
    }
}
