using System.Windows;
using WinAct_Helper.Model;
using WinAct_Helper.ViewModels;

namespace WinAct_Helper.Forms
{
    public partial class EditCompartmentForm : Window
    {
        public EditCompartmentForm(Compartment? compartment = null)
        {
            vm = new CompartmentWindowVM(compartment);
            DataContext = vm;

            InitializeComponent();

            this.Title = compartment != null ? "Edit compartment" : "New compartment";
        }
        private CompartmentWindowVM vm;

        public Compartment Compartment { get; private set; }

        public bool IsSuccess = false;

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (vm.ValidateInputs())
            {
                IsSuccess = true;
                this.Compartment = vm.Compartment;
                this.Close();
            }
        }
    }
}
