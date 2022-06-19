using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinAct_Helper.Model;

namespace WinAct_Helper.Controller
{
    public interface IFileIO
    {
        InputFile ReadFile(string fileName);

        void WriteFile(string outFileName, InputFile file);
    }
}
