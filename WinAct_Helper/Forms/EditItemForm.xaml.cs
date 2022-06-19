using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WinAct_Helper.Model;

namespace WinAct_Helper.Forms
{
    /// <summary>
    /// Логика взаимодействия для EditItemForm.xaml
    /// </summary>
    public partial class EditItemForm : Window
    {
        private Compartment _compartment;
        private Transfer _transfer;
        public Compartment? compartment { get => _compartment;
            set
            {
                _compartment = value;
                SetCompartmentEditMode();
            }
        }
        public Transfer? transfer { get => _transfer;
            set
            {
                _transfer = value;
                SetTransferEditMode();
            }
        }
        /// <summary>
        /// Check if Compartment and Transfer classes are null. If It's true, then nothing to edit and operation is cancelled.
        /// </summary>
        public bool IsCanceled
        {
            get
            {
                return _compartment == null && _transfer == null;
            }
        }

        private bool DataAccepted = false;

        public EditItemForm()
        {
            InitializeComponent();
            SetButtonsHandler();
        }

        private void SetCompartmentEditMode()
        {
            textFrom.Text = "Compartment name";
            textTo.IsEnabled = false;
            tboxTo.IsEnabled = false;
            textTime.Text = "A0";

            tboxFrom.Text = _compartment.Name;
            tboxTime.Text = _compartment.A0.ToString();
        }

        private void SetTransferEditMode()
        {
            textTo.IsEnabled = true;
            tboxTo.IsEnabled = true;
            textTime.Text = "Time (days)";

            tboxFrom.Text = _transfer.From;
            tboxTo.Text = _transfer.To;
            tboxTime.Text = _transfer.Time.ToString();
        }

        private void SetButtonsHandler()
        {
            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (object sender, RoutedEventArgs e) => this.Close();
            this.Closed += EditItemForm_Closed;
        }

        private void EditItemForm_Closed(object? sender, EventArgs e)
        {
            if (!DataAccepted)
                CancelEditing();
        }

        private void CancelEditing()
        {
            if (_compartment != null)
                _compartment = null;
            if (_transfer != null)
                _transfer = null;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInputs())
            {
                DataAccepted = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(
                    "Check input fields! There are errors.", 
                    "Warning", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning);
            }
        }

        private bool CheckInputs()
        {
            double buf_double = 0;
            if (!Double.TryParse(tboxTime.Text, out buf_double) || buf_double < 0.0)
                return false;
            if (_compartment != null)
            {
                _compartment.Name = tboxFrom.Text;
                _compartment.A0 = buf_double;
            } else if (_transfer != null)
            {
                _transfer.From = tboxFrom.Text;
                _transfer.To = tboxTo.Text;
                _transfer.Time = buf_double;
            }
            return true;
        }
    }
}
