using System.IO;
using Microsoft.Win32;

namespace Application.Models.ViewLogic
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
