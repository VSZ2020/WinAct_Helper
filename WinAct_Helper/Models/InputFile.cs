using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using WinAct_Helper.Services;

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

        public string FullPath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = !string.IsNullOrEmpty(value) ? value: _filePath;
            }
        }
        
        public double HalfLive { get; private set; }

        /// <summary>
        /// Note at the start of WinAct file (first 2 lines)
        /// </summary>
        public string? Comment_1 { get; set; }
        public string? Comment_2 { get; set; }

        /// <summary>
        /// Basic class constructor
        /// </summary>
        /// <param name="FilePath">Path to input file</param>
        /// <param name="halfLive">Radionuclide Half-Live</param>
        /// <param name="comps">List of compartments</param>
        /// <param name="transfs">List of transfers</param>
        public InputFile(string FilePath, double halfLive, List<Compartment> comps, List<Transfer> transfs)
        {
            _filePath = FilePath;
            FileName = GetFileName(FilePath);
            HalfLive = halfLive;
            Compartments = comps;
            Transfers = transfs;
        }

        private string GetFileName(string path)
        {
            if (path.Length > 3)
            {
                return Path.GetFileNameWithoutExtension(path);
            }
            return path;
        }

        public InputFile Copy()
        {
            var compartments = new List<Compartment>(this.Compartments.Count);
            var transfers = new List<Transfer>(this.Transfers.Count);
            for (int i = 0; i < Compartments.Count; i++)
            {
                compartments.Add(this.Compartments[i]);
            }
            for (int j = 0; j < Transfers.Count; j++)
            {
                transfers.Add(this.Transfers[j]);
            }
            return new InputFile(
                this._filePath,
                this.HalfLive,
                compartments,
                transfers);
        }
    }
}
