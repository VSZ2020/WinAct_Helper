using System.Collections.Generic;
using WinAct_Helper.Model;

namespace WinAct_Helper.Controller
{
    internal class ValidationErrorsKeeper : IValidationErrors
    {
        private Dictionary<string, string> _errorsDict;
        public bool IsValid { get => _errorsDict.Count == 0; }

        public void AddError(string RecordName, string Message)
        {
            if (!_errorsDict.ContainsKey(RecordName))
                _errorsDict.Add(RecordName, Message);
            else
                _errorsDict[RecordName] = Message;
        }

        public void ClearErrors()
        {
            _errorsDict.Clear();
        }

        public ValidationErrorsKeeper()
        {
            _errorsDict = new Dictionary<string, string>();
        }
    }
}
