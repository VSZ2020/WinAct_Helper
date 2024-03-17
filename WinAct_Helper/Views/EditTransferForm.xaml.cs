using System.Windows;
using WinAct_Helper.Model;
using WinAct_Helper.ViewModels;

namespace WinAct_Helper.Forms
{
    /// <summary>
    /// Логика взаимодействия для EditItemForm.xaml
    /// </summary>
    public partial class EditTransferForm : Window
    {
        public EditTransferForm(Transfer? transfer = null)
        {
            vm = new TransferWindowVM(transfer);
            DataContext = vm;

            InitializeComponent();

            this.Title = transfer != null ? "Edit transfer" : "New transfer";
        }
        private TransferWindowVM vm;

        public Transfer Transfer { get; private set; }

        public bool IsSuccess = false;

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (vm.ValidateInputs())
            {
                IsSuccess = true;
                this.Transfer = vm.Transfer;
                this.Close();
            }
        }
    }
}
