using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinAct_Helper.Model;

namespace WinAct_Helper.Controller
{
    public class FileDefaultService : IFileService
    {
        private IFileIO _fileIOController;
        private IValidationErrors _validationErrors;

        public FileDefaultService(IValidationErrors validationErrors) : this(new InputFileTextIO(), validationErrors)
        { }

        public FileDefaultService(IFileIO fileController, IValidationErrors validationErrors)
        {
            _fileIOController = fileController;
            _validationErrors = validationErrors;
        }

        public bool ValidateFields(InputFile input)
        {
            for (int i = 0; i < input.Compartments.Count; i++)
            {
                if (input.Compartments[i].A0 < 0.0)
                    _validationErrors.AddError($"Compartment #{i + 1}", $"Error in compartment '{input.Compartments[i].Name}' with value {input.Compartments[i].A0}");
            }
            for (int i = 0; i < input.Transfers.Count; i++)
            {
                if (input.Transfers[i].Time < 0.0)
                    _validationErrors.AddError($"Transfer #{i + 1}", $"Error in transfer '{input.Transfers[i].From} -> {input.Transfers[i].To}' with value {input.Transfers[i].Time}");
            }
            return _validationErrors.IsValid;
        }

        #region IFileService methods realization
        public InputFile ReadFile(string fileName)
        {
            //HACK: There is can be placed validation procedure
            return _fileIOController.ReadFile(fileName);
        }

        public bool WriteFile(string outFileName, InputFile file)
        {
            if (!ValidateFields(file))
            {
                Trace.WriteLine("Errors occured during validation procedure!");
                return false;
            }
            _fileIOController.WriteFile(outFileName, file);
            return true;
        }
        #endregion
    }
}
