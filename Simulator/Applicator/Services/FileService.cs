using Applicator.Services;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Application.Services
{
    class FileService : IFileService
    {
        public short[] ParseFile(string file, short[] array)
        {
            array = new short[Constants.PROGRAM_MEMORY_SIZE];
            //regex Pattern
            string pattern = @"^[0-9A-F]+\s[0-9A-F]+";
            RegexOptions options = RegexOptions.Multiline;

            foreach (Match m in Regex.Matches(file, pattern, options))
            {
                //ins array rein :D
                string com = m.Value;
                string adress = m.Value;
                com = com.Remove(0, 5); //erste 4 Zeichen (sogenannter Index looool) inklusive Leerzeichen
                adress = adress.Remove(4); // Ab Index 4 sind die Zeichen unbrauchbar
                short comAsInt = short.Parse(com, NumberStyles.HexNumber);
                short adressAsInt = short.Parse(adress, NumberStyles.HexNumber);
                array[adressAsInt] = comAsInt;
            }
            return array;
        }

        public FileService()
        {
           
        }
    }
}
