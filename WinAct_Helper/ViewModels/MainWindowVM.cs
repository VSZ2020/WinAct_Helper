using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinAct_Helper.Common;
using WinAct_Helper.Controller;
using WinAct_Helper.Forms;
using WinAct_Helper.Model;
using WinAct_Helper.Services;

namespace WinAct_Helper.ViewModels
{
    public class MainWindowVM: BaseValidationViewModel
    {
        public MainWindowVM()
        {
            Compartments = new();
            Transfers = new();
            CreateNewFile();
        }

        #region Commands
        private RelayCommand addCompartment;
        private RelayCommand editCompartment;
        private RelayCommand removeCompartment;
        private RelayCommand addTransfer;
        private RelayCommand editTransfer;
        private RelayCommand removeTransfer;

        private RelayCommand newFile;
        private RelayCommand openFile;
        private RelayCommand saveFile;

        public RelayCommand AddCompartmentCommand => addCompartment ?? (addCompartment = new RelayCommand(o => AddCompartment()));
        public RelayCommand EditCompartmentCommand => editCompartment ?? (editCompartment = new RelayCommand(o => EditCompartment(), o => _selectedCompartment != null));
        public RelayCommand RemoveCompartmentCommand => removeCompartment ?? (removeCompartment = new RelayCommand(o => RemoveCompartment(), o => Compartments.Count > 0 && _selectedCompartment != null));
        
        public RelayCommand AddTransferCommand => addTransfer ?? (addTransfer = new RelayCommand(o => AddTransfer()));
        public RelayCommand EditTransferCommand => editTransfer ?? (editTransfer = new RelayCommand(o => EditTransfer(), o => _selectedTransfer != null));
        public RelayCommand RemoveTransferCommand => removeTransfer ?? (removeTransfer = new RelayCommand(o => RemoveTransfer(), o => Transfers.Count > 0 && _selectedTransfer != null));
        
        public RelayCommand NewFileCommand => newFile ?? (newFile = new RelayCommand(o => CreateNewFile()));
        public RelayCommand OpenFileCommand => openFile ?? (openFile = new RelayCommand(o => OpenInputFile()));
        public RelayCommand SaveFileCommand => saveFile ?? (saveFile = new RelayCommand(o => SaveInputFile(), o => IsFileModified));
        #endregion

        private Compartment? _selectedCompartment;
        private Transfer? _selectedTransfer;
        private string filePath;
        private double halfLive;
        private string commentLine1;
        private string commentLine2;

        private bool _isFileModified;

        public Compartment? SelectedCompartment { get => _selectedCompartment; set { _selectedCompartment = value; OnChanged(); } }
        public Transfer? SelectedTransfer { get => _selectedTransfer; set { _selectedTransfer = value; OnChanged(); } }
        public ObservableCollection<Compartment> Compartments { get; private set; }
        public ObservableCollection<Transfer> Transfers { get; private set; }

        public string FilePath { get => filePath; set { filePath = value; OnChanged(); } }
        public double HalfLive { get => halfLive; set { halfLive = value; OnChanged(); } }
        public string CommentLine1 { get => commentLine1; set { commentLine1 = value;OnChanged(); } }
        public string CommentLine2 { get => commentLine2; set { commentLine2 = value; OnChanged(); } }

        public bool IsFileModified { get => _isFileModified; set { _isFileModified = value; OnChanged(); } }


        public void AddCompartment()
        {
            var wnd = new EditCompartmentForm();
            wnd.ShowDialog();
            if (wnd.IsSuccess)
            {
                Compartments.Add(wnd.Compartment);
                IsFileModified = true;
            }
        }

        public void EditCompartment()
        {
            var wnd = new EditCompartmentForm(SelectedCompartment);
            wnd.ShowDialog();
            if (wnd.IsSuccess)
            {
                var index =  Compartments.IndexOf(SelectedCompartment!);
                Compartments.Remove(SelectedCompartment!);
                Compartments.Insert(index, wnd.Compartment);
                IsFileModified = true;
            }
        }

        public void RemoveCompartment() 
        {
            Compartments.Remove(SelectedCompartment!);
            IsFileModified = true;
        }

        public void AddTransfer()
        {
            var wnd = new EditTransferForm();
            wnd.ShowDialog();
            if (wnd.IsSuccess)
            {
                Transfers.Add(wnd.Transfer);
                IsFileModified = true;
            }
        }

        public void EditTransfer()
        {
            var wnd = new EditTransferForm(SelectedTransfer);
            wnd.ShowDialog();
            if (wnd.IsSuccess)
            {
                var index = Transfers.IndexOf(SelectedTransfer!);
                Transfers.Remove(SelectedTransfer!);
                Transfers.Insert(index, wnd.Transfer);
                IsFileModified = true;
            }
        }

        public void RemoveTransfer()
        {
            Transfers.Remove(SelectedTransfer!);
            IsFileModified = true;
        }


        public bool ValidateView()
        {
            base.ClearValidationMessages();

            if (halfLive <= 0)
                base.AddErrorMessage("Half live is zero or negative", "Half-Live");

            if (Transfers.Count == 0)
                base.AddErrorMessage("Transfers list is empty", "Transfers");

            return base.IsValid;
        }

        

        private void ApplyInputFileToViewModel(InputFile file)
        {
            this.HalfLive = file.HalfLive;
            this.FilePath = file.FullPath;
            this.CommentLine1 = file.Comment_1;
            this.CommentLine2 = file.Comment_2;
            this.Compartments.Clear();
            this.Transfers.Clear();

            foreach (var comp in file.Compartments)
                Compartments.Add(comp);

            foreach (var t in file.Transfers)
                Transfers.Add(t);
        }

        private void CreateNewFile()
        {
            if (ControlChanges())
            {
                var inputFile = DefaultDataProvider.CreateDefaultInput();
                ApplyInputFileToViewModel(inputFile);
                IsFileModified = true;
            }
        }

        private void OpenInputFile()
        {
            if (ControlChanges())
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "WinAct Input File (*.inp)|*.inp";
                dialog.DefaultExt = "*.inp";
                dialog.Multiselect = false;

                var result = dialog.ShowDialog();
                if (result.HasValue && result.Value == true)
                {
                    var fileName = dialog.FileName;
                    try
                    {
                        var inputFile = InputFileService.ReadFile(fileName);
                        ApplyInputFileToViewModel(inputFile);
                        IsFileModified = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Can't read current file {dialog.FileName}.\nError message {ex.Message}",
                            "Warning",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
            }
        }

        public bool SaveInputFile()
        {
            if (ValidateView() && IsFilePathAssigned())
            {
                var inputFile = new InputFile(filePath, HalfLive, Compartments.ToList(), Transfers.ToList()) { Comment_1 = this.CommentLine1, Comment_2 = this.CommentLine2 };
                InputFileService.WriteFile(filePath, inputFile);
                return true;
            }

            return false;
        }

        private bool IsFilePathAssigned()
        {
            if (string.IsNullOrEmpty(filePath))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.AddExtension = true;
                dialog.Filter = "WinAct Input File (*.inp)|*.inp";
                dialog.DefaultExt = "*.inp";
                var result = dialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    FilePath = dialog.FileName;
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool ControlChanges()
        {
            if (IsFileModified)
            {
                var result = MessageBox.Show(
                    "File has changes. Do you want to save them?",
                    "Save changes",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                    return false;

                if (result == MessageBoxResult.Yes)
                    return SaveInputFile();
            }
            return true;
        }

    }
}
