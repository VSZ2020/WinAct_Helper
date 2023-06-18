using Microsoft.Win32;
using System;
using System.Windows;

using WinAct_Helper.Controller;
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

            btnMenuCreate.Click += BtnMenuCreate_Click;
            btnMenuOpen.Click += ButtonOpenClick;
            btnMenuSave.Click += ButtonSaveClick;
            btnMenuSaveAs.Click += ButtonSaveClick;

            btnMenuExit.Click += BtnMenuExit_Click;
            this.Closing += MainWindow_Closing;

            btnMenuAbout.Click += BtnMenuAbout_Click;
        }

        

        private void UpdateModifiedStatus(bool IsModified)
        {
            InputFileProperty.IsModified = IsModified;
            btnMenuSave.IsEnabled = IsModified;

            var path = InputFileProperty.FullPath;
            this.Title = String.Concat(
                IsModified ? "* " : "",
                "WinAct Input Helper ",
                string.IsNullOrWhiteSpace(path) ?"" : (path.Length < 65 ? "- " + path : "- ..." + path.Substring(path.Length - 65, 65)));
            
        }

        private void CreateWinActFile()
        {
            if (InputFileProperty != null && InputFileProperty.IsModified)
            {
                var result = MessageBox.Show(
                    "File was changed. Do you want to save changes?",
                    "Save changes",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel)
                    return;
                if (result == MessageBoxResult.Yes)
                {
                    SaveWinActFile(false);
                }

                LoadDefaultInput();
            }
        }

        private void OpenWinActFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "WinAct Input File (*.inp)|*.inp";
            dialog.DefaultExt = "*.inp";
            dialog.Multiselect = false;
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value == true)
            {
                var bufferFile = InputFileProperty.Copy();
                if (bufferFile.ReadFromFile(new FileDefaultService(new ValidationErrorsKeeper()), dialog.FileName))
                {
                    this.DataContext = null;
                    InputFileProperty = bufferFile;
                    this.DataContext = this;
                    SelectCorrespondingNuclide(InputFileProperty.Radionuclide);
                    //lvCompartments.Items.Refresh();
                    //lvTransfers.Items.Refresh();
                    UpdateModifiedStatus(true);
                }
                else
                {
                    MessageBox.Show(
                        $"Can't read current file {dialog.FileName}.\nCheck it!",
                        "Warning",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
        }

        private void SaveWinActFile(bool IsSaveAs)
        {
            string savePath = "";

            if (IsSaveAs || string.IsNullOrEmpty(InputFileProperty.FullPath))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.AddExtension = true;
                dialog.Filter = "WinAct Input File (*.inp)|*.inp";
                dialog.DefaultExt = "*.inp";
                var result = dialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    savePath = dialog.FileName;
                }
                else if (!result.Value) //In case of save Cancelling
                    return;
            }
            var validator = new ValidationErrorsKeeper();
            InputFileProperty.SaveTofile(new FileDefaultService(validator), savePath);
            if (validator.IsValid)
            {
                InputFileProperty.FullPath = savePath;
                UpdateModifiedStatus(false);
            }
            else
            {
                MessageBox.Show(
                    $"Can't write current file to path {savePath}.\nErrors: {validator.PrintErrors()}",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void SelectCorrespondingNuclide(Radionuclide nuclide)
        {
            bool InList = false;
            for (int i = 0; i < RadionuclidesProperty.Nuclides.Count; i++)
            {
                if (nuclide == RadionuclidesProperty.Nuclides[i])
                {
                    InList = true;
                    cBoxRadionuclides.SelectedItem = RadionuclidesProperty.Nuclides;
                    break;
                }
            }
            if (!InList)
            {
                RadionuclidesProperty.Nuclides.Add(nuclide);
                cBoxRadionuclides.Items.Refresh();
                cBoxRadionuclides.SelectedItem = nuclide;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns true if the program should be closed</returns>
        private bool ExitProgramController()
        {
            if (InputFileProperty.IsModified)
            {
                var result = MessageBox.Show(
                    "Do you want to save changes?",
                    "Question",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes: { SaveWinActFile(false); break; }
                    case MessageBoxResult.No: { break; }
                    case MessageBoxResult.Cancel: { return false; }
                }
            }
            return true;
        }
    }
}
