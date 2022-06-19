using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WinAct_Helper.Forms;
using WinAct_Helper.Model;

namespace WinAct_Helper
{
    partial class MainWindow: Window
    {
        private void SetMainWindowBindings()
        {
            this.DataContext = this;
            if (RadionuclidesProperty.Nuclides.Count > 0)
                cBoxRadionuclides.SelectedIndex = 0;
        }

        private void SetButtonEventHandlers()
        {
            btnAddCompartment.Click += ButtonAddClick;
            btnAddTransfer.Click += ButtonAddClick;
            btnMenuAddCompartment.Click += ButtonAddClick;
            btnMenuAddTransfer.Click += ButtonAddClick;

            btnEditCompartment.Click += ButtonEditClick;
            btnEditTransfer.Click += ButtonEditClick;
            btnMenuEditCompartment.Click += ButtonEditClick;
            btnMenuEditTransfer.Click += ButtonEditClick;

            btnRemoveCompartment.Click += ButtonRemoveClick;
            btnRemoveTransfer.Click += ButtonRemoveClick;
            btnMenuRemoveCompartment.Click += ButtonRemoveClick;
            btnMenuRemoveTransfer.Click += ButtonRemoveClick;
        }
        /// <summary>
        /// Arises when button Add (for compartment or transfer) clicked
        /// </summary>
        /// <param name="sender">Invoker</param>
        /// <param name="e"></param>
        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            EditItemForm editForm = new EditItemForm();
            if (sender == btnAddCompartment || sender == btnMenuAddCompartment)
            {
                var comp = new Compartment("Undefined", 0.0);
                editForm.compartment = comp;
                editForm.ShowDialog();
                if (!editForm.IsCanceled)
                    InputFileProperty.Compartments.Add(editForm.compartment);
                
            }
            if (sender == btnAddTransfer || sender == btnMenuAddTransfer)
            {
                var trans = new Transfer("Undefined", "Undefined", 0.0);
                editForm.transfer = trans;
                editForm.ShowDialog();
                if (!editForm.IsCanceled)
                    InputFileProperty.Transfers.Add(editForm.transfer);
                
            }
            lvCompartments.Items?.Refresh();
            lvTransfers.Items?.Refresh();
        }
        /// <summary>
        /// Arises when button Edit (for compartment or transfer) clicked
        /// </summary>
        /// <param name="sender">Invoker</param>
        /// <param name="e"></param>
        private void ButtonEditClick(object sender, RoutedEventArgs e)
        {
            EditItemForm editForm = new EditItemForm();
            if (sender == btnEditCompartment || sender == btnMenuEditCompartment)
            {
                if (lvCompartments.SelectedIndex == -1)
                {
                    MessageBox.Show("Nothing to edit", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                var index = lvCompartments.SelectedIndex;
                editForm.compartment = ((Compartment)lvCompartments.SelectedItem).Copy();
                editForm.ShowDialog();
                if (!editForm.IsCanceled)
                    InputFileProperty.Compartments[index] = editForm.compartment;
            }
            if (sender == btnEditTransfer || sender == btnMenuEditTransfer)
            {
                if (lvTransfers.SelectedIndex == -1)
                {
                    MessageBox.Show("Nothing to edit", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                var index = lvTransfers.SelectedIndex;
                editForm.transfer = ((Transfer)lvTransfers.SelectedItem).Copy();
                editForm.ShowDialog();
                if (!editForm.IsCanceled)
                    InputFileProperty.Transfers[index] = editForm.transfer;
            }
            lvCompartments.Items?.Refresh();
            lvTransfers.Items?.Refresh();
        }
        /// <summary>
        /// Arises when button Remove (for compartment or transfer) clicked
        /// </summary>
        /// <param name="sender">Invoker</param>
        /// <param name="e"></param>
        private void ButtonRemoveClick(object sender, RoutedEventArgs e)
        {
            if (sender == btnRemoveCompartment || sender == btnMenuRemoveCompartment)
            {
                var index = lvCompartments.SelectedIndex;
                if (index == -1 || lvCompartments.Items?.Count == 0)
                {
                    MessageBox.Show("Nothing to remove", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                this.InputFileProperty.Compartments.RemoveAt(index);
            }
            if (sender == btnRemoveTransfer || sender == btnMenuRemoveTransfer)
            {
                var index = lvTransfers.SelectedIndex;
                if (index == -1 || lvTransfers.Items?.Count == 0)
                {
                    MessageBox.Show("Nothing to remove", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                this.InputFileProperty.Transfers.RemoveAt(index);
            }
            lvCompartments.Items?.Refresh();
            lvTransfers.Items?.Refresh();
        }
    }
}
