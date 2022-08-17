using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinAct_Helper.Controller;

namespace WinAct_Helper.Model
{
    internal class FileService : IFileService
    {
        private IFileIO _fileIOController;
        private Dictionary<string, string> ValidationErrors;

        public FileService(IFileIO fileController)
        {
            _fileIOController = fileController;
        }

        public bool ValidateFields(InputFile input)
        {
            for (int i = 0; i < input.Compartments.Count; i++)
            {
                if (input.Compartments[i].A0 < 0.0)
                    ValidationErrors.Add($"Compartment #{i + 1}", $"Error in compartment '{input.Compartments[i].Name}' with value {input.Compartments[i].A0}");
            }
            for (int i = 0; i < input.Transfers.Count; i++)
            {
                if (input.Transfers[i].Time < 0.0)
                    ValidationErrors.Add($"Transfer #{i + 1}", $"Error in transfer '{input.Transfers[i].From} -> {input.Transfers[i].To}' with value {input.Transfers[i].Time}");
            }
            return ValidationErrors.Count == 0;
        }

        #region IFileService methods
        public InputFile ReadFile(string fileName)
        {
            //HACK: There is can be placed validation procedure
            return _fileIOController.ReadFile(fileName);
        }

        public bool WriteFile(string outFileName, InputFile file)
        {
            if (!ValidateFields(file))
                return false;
            _fileIOController.WriteFile(outFileName, file);
            return true;
        }
        #endregion
    }
}
