using Application.Model;
using Applicator.Model;
using Applicator.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Application.Services
{
    class FileService : IFileService
    {
        private string pattern = @"^[0-9A-F]+\s[0-9A-F]+";

        public void CreateFileList(SourceFileModel src)
        {
            src.ListOfCode = new ObservableCollection<LineOfCode>();
            string temp = src.SourceFile;
            string[] lines = temp.Split(
            new[] { Environment.NewLine },
            StringSplitOptions.None
            );
            for (int i = 0; i<lines.Length; i++)
            {
                Match m = Regex.Match(lines[i], pattern);
                var LoC = new LineOfCode(lines[i]);
                if(m.Success == true)
                {
                    //ins array rein :D
                    string com = m.Value;
                    string adress = m.Value;
                    com = com.Remove(0, 5); //erste 4 Zeichen (sogenannter Index looool) inklusive Leerzeichen
                    adress = adress.Remove(4); // Ab Index 4 sind die Zeichen unbrauchbar
                    short comAsInt = short.Parse(com, NumberStyles.HexNumber);
                    short adressAsInt = short.Parse(adress, NumberStyles.HexNumber);
                    LoC.ProgramCode = comAsInt;
                    LoC.CommandIndex = adressAsInt;
                }
                src.ListOfCode.Add(LoC);
            }
        }

        public FileService()
        {
           
        }
    }
}
