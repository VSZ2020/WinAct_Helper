using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using WinAct_Helper.Forms;
using WinAct_Helper.Model;
using WinAct_Helper.Controller;

namespace WinAct_Helper
{
    partial class MainWindow : Window
    {
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
                {
                    InputFileProperty.Compartments.Add(editForm.compartment);
                    UpdateModifiedStatus(true);
                }

            }
            if (sender == btnAddTransfer || sender == btnMenuAddTransfer)
            {
                var trans = new Transfer("Undefined", "Undefined", 0.0);
                editForm.transfer = trans;
                editForm.ShowDialog();
                if (!editForm.IsCanceled)
                {
                    InputFileProperty.Transfers.Add(editForm.transfer);
                    UpdateModifiedStatus(true);
                }

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
                var index = lvCompartments.SelectedIndex;
                if (index == -1 || index >= InputFileProperty.Compartments.Count)
                {
                    MessageBox.Show("Nothing to edit", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                editForm.compartment = ((Compartment)lvCompartments.SelectedItem).Copy();
                editForm.ShowDialog();
                if (!editForm.IsCanceled)
                {
                    InputFileProperty.Compartments[index] = editForm.compartment;
                    UpdateModifiedStatus(true);
                }
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
                {
                    InputFileProperty.Transfers[index] = editForm.transfer;
                    UpdateModifiedStatus(true);
                }
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
                InputFileProperty.Compartments.RemoveAt(index);
                UpdateModifiedStatus(true);
            }
            if (sender == btnRemoveTransfer || sender == btnMenuRemoveTransfer)
            {
                var index = lvTransfers.SelectedIndex;
                if (index == -1 || lvTransfers.Items?.Count == 0)
                {
                    MessageBox.Show("Nothing to remove", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                InputFileProperty.Transfers.RemoveAt(index);
                UpdateModifiedStatus(true);
            }
            lvCompartments.Items?.Refresh();
            lvTransfers.Items?.Refresh();
        }

        private void ButtonOpenClick(object sender, RoutedEventArgs e)
        {
            OpenWinActFile();
        }

        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            if (sender == btnMenuSave)
            {
                SaveWinActFile(false);
            }

            if (sender == btnMenuSaveAs)
            {
                SaveWinActFile(true);
            }
        }

        private void BtnMenuAbout_Click(object sender, RoutedEventArgs e)
        {
            About formAbout = new About();
            formAbout.ShowDialog();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ExitProgramController())
            {
                e.Cancel = true;
            }
        }

        private void BtnMenuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
