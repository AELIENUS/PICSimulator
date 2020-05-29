using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IFileService
    {
        short[] ParseFile(string file, short[] array);
        void CreateFileList(SourceFileModel src);
    }
}
