using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAct_Helper.Model
{
    internal interface IValidationErrors
    {
        bool IsValid { get; }
        void AddError(string RecordName, string Message);
    }
}
