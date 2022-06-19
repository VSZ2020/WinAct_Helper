using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using WinAct_Helper.Controller;

namespace WinAct_Helper.Model
{
    public class InputFile
    {
        private string _filePath;

        /// <summary>
        /// List of Compartments
        /// </summary>
        public List<Compartment> Compartments { get; private set; }
        /// <summary>
        /// List of Transfers
        /// </summary>
        public List<Transfer> Transfers { get; private set; }
        /// <summary>
        /// Short name of current file
        /// </summary>
        public string FileName { get; set; }
        
        public Radionuclide Radionuclide { get; private set; }
        /// <summary>
        /// Note at the start of WinAct file (first 2 lines)
        /// </summary>
        public string? Comment { get; set; }
        /// <summary>
        /// Flag for checking file fields modification
        /// </summary>
        public bool IsModified = false;

        public InputFile(string FilePath, Radionuclide nuclide) :
            this(FilePath,
                nuclide ?? DefaultDataController.GetDefaultRadionuclide(),
                new List<Compartment>(),
                new List<Transfer>())
        { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="FileReader"></param>
        public InputFile(string FilePath, IFileIO FileReader)
        {
            FileName = GetFileName(FilePath);
            ReadFromFile(FileReader, FilePath);
            //UNDONE: Add Try-Catch block for case, when the file is not exist
            //If file is not exist, then create default file
        }

        /// <summary>
        /// Basic class constructor
        /// </summary>
        /// <param name="FilePath">Path to input file</param>
        /// <param name="nuclide">Radionuclide</param>
        /// <param name="comps">List of compartments</param>
        /// <param name="transfs">List of transfers</param>
        public InputFile(string FilePath, Radionuclide nuclide, List<Compartment> comps, List<Transfer> transfs)
        {
            _filePath = FilePath;
            FileName = GetFileName(FilePath);
            Radionuclide = nuclide;
            Compartments = comps;
            Transfers = transfs;
        }

        public void ReadFromFile(IFileIO FileReader, string path)
        {
            if (File.Exists(path))
                throw new IOException($"File {path} doesn't exist");
            var file = FileReader.ReadFile(path);

            if (file.Radionuclide != null)
                this.Radionuclide = file.Radionuclide;
            if (file.Compartments != null)
                this.Compartments = file.Compartments;
            if (file.Transfers != null)
                this.Transfers = file.Transfers;
        }

        private string GetFileName(string path)
        {
            if (path.Length > 3)
            {
                return Path.GetFileNameWithoutExtension(path);
            }
            return path;
        }
    }
}
