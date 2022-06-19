using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using WinAct_Helper.Model;


namespace WinAct_Helper.Controller
{
    internal class InputFileTextIO : IFileIO
    {
        public InputFile ReadFile(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException($"Can't find file at path {fileName}");
            using (StreamReader rd = new StreamReader(fileName))
            {
                string line = "";
                while ((line = rd.ReadLine()) != null)
                {

                }
            }
        }

        public void WriteFile(string outFileName, InputFile file)
        {
            if (!File.Exists(outFileName))
            {
                File.CreateText(outFileName).Close();
            }
            throw new NotImplementedException();
        }

        private List<Compartment> ReadCompartments(StreamReader sr)
        {
            int lineCounter = 0;
            string line = "";
            var list = new List<Compartment>();
            while ((line = sr.ReadLine()).IndexOf("END LIST") == -1)
            {
                lineCounter++;
                if (string.IsNullOrEmpty(line))
                    continue;
                
                var splittedString = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (splittedString.Length < 2)
                {
                    //Log error information
                    Trace.WriteLine($"Incorrect compartment data at line {line}. This line was skipped.");
                    continue;
                }
                double compartmentValue = 0.0;
                var compartmentName = splittedString[0].Trim();
                if (double.TryParse(splittedString[1], out compartmentValue))
                {
                    var compartment = new Compartment(compartmentName, compartmentValue);
                    list.Add(compartment);
                }
                else
                {
                    //Log error information
                    Trace.WriteLine($"Incorrect compartment {compartmentName} A0 value at {lineCounter} position. This value was skipped.");
                    //TODO: Add trace listener to show user in UI
                }
            }
            return list;
        }
    }
}
