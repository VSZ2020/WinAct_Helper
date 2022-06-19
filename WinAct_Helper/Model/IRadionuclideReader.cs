using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAct_Helper.Model
{
    public interface IRadionuclideReader
    {
        List<Radionuclide> ReadFile(string fileName);
    }
}
