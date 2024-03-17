using WinAct_Helper.Common;
using WinAct_Helper.Model;

namespace WinAct_Helper.ViewModels
{
    public class TransferWindowVM: BaseValidationViewModel
    {
        public TransferWindowVM(Transfer? transfer = null)
        {
            this.Transfer = transfer != null ? transfer.Copy() : new Transfer("A", "B", 1);
        }

        public Transfer Transfer { get; private set; }

        public bool ValidateInputs()
        {
            base.ClearValidationMessages();

            if (Transfer.Time < 0.0)
                base.AddErrorMessage("Incorrect duration time", "Duration time");

            if (Transfer.From.Contains(' ') || string.IsNullOrEmpty(Transfer.From))
                base.AddErrorMessage("Field contains whitespace or empty", "From");

            if (Transfer.To.Contains(' ') || string.IsNullOrEmpty(Transfer.To))
                base.AddErrorMessage("Field contains whitespace or empty", "To");

            return IsValid;
        }
    }
}
