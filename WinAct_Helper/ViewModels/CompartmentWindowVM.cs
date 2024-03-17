using WinAct_Helper.Common;
using WinAct_Helper.Model;

namespace WinAct_Helper.ViewModels
{
    public class CompartmentWindowVM : BaseValidationViewModel
    {
        public CompartmentWindowVM(Compartment? compartment = null)
        {
            this.Compartment = compartment != null ? compartment.Copy() : new Compartment("Comp", 0);
        }

        public Compartment Compartment { get; private set; }

        public bool ValidateInputs()
        {
            base.ClearValidationMessages();

            if (Compartment.A0 < 0.0)
                base.AddErrorMessage("Incorrect activity", "A0");

            if (Compartment.Name.Contains(' ') || string.IsNullOrEmpty(Compartment.Name))
                base.AddErrorMessage("Field contains whitespaces or empty","Name");

            return IsValid;
        }
    }
}
