using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Application.Services
{
    public class DialogService : IDialogService
    {
        public string Open()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                //SourceFile holen
                string file = File.ReadAllText(ofd.FileName);
                return file;
            }
            else
            {
                return "OFD failed";
            }
        }
    }
}
