using System.Collections.Generic;
using WinAct_Helper.Common;
using WinAct_Helper.Model;
using WinAct_Helper.Models;

namespace WinAct_Helper.ViewModels
{
    public class TransferWindowVM: BaseValidationViewModel
    {
        public TransferWindowVM(Transfer? transfer = null)
        {
            this.Transfer = transfer != null ? transfer.Copy() : new Transfer("A", "B", 1);
            SelectedUnits = ConstantUnits.PerDay;
        }

        private Dictionary<ConstantUnits, string> _constantUnits = new()
        {
            { ConstantUnits.PerYear, "1/y" },
            { ConstantUnits.PerMonth, "1/m" },
            { ConstantUnits.PerDay, "1/d" },
            { ConstantUnits.PerHour, "1/h" },
            { ConstantUnits.PerMinute, "1/min" },
            { ConstantUnits.PerSecond, "1/s" },
        };

        public Transfer Transfer { get; private set; }
        public ConstantUnits SelectedUnits { get; set; }

        public Dictionary<ConstantUnits, string> ConstantUnitsDict => _constantUnits;

        public bool ValidateInputs()
        {
            base.ClearValidationMessages();

            if (Transfer.Constant < 0.0)
                base.AddErrorMessage("Incorrect transition constant", "Transition constant");

            if (Transfer.From.Contains(' ') || string.IsNullOrEmpty(Transfer.From))
                base.AddErrorMessage("Field contains whitespace or empty", "From");

            if (Transfer.To.Contains(' ') || string.IsNullOrEmpty(Transfer.To))
                base.AddErrorMessage("Field contains whitespace or empty", "To");

            return IsValid;
        }

        /// <summary>
        /// Convert custom units to 1/day
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public double ConvertUnits(ConstantUnits unitType)
        {
            return unitType switch
            {
                ConstantUnits.PerMonth => 1.0 / 30,
                ConstantUnits.PerYear => 1.0 / 365,
                ConstantUnits.PerHour => 24,
                ConstantUnits.PerMinute => 60 * 24,
                ConstantUnits.PerSecond => 3600 * 24,
                _ => 1,
            };
        }
    }
}
