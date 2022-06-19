using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAct_Helper.Model
{
    internal interface IFileService
    {
        bool WriteFile(string outFileName, InputFile file);

        InputFile ReadFile(string fileName);
    }
}
