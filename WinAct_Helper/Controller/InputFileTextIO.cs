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
            string[] inputFileComment = { "", ""};
            Radionuclide rNuclide = DefaultDataController.GetDefaultRadionuclide();
            List<Compartment> compartments = new List<Compartment>();
            List<Transfer> transfers = new List<Transfer>(); 
            using (StreamReader rd = new StreamReader(fileName))
            {
                string? line = "";
                int lineCounter = 0;
                while ((line = rd.ReadLine()) != null)
                {
                    lineCounter++;
                    if (lineCounter == 1)
                    {
                        //Read first comment
                        inputFileComment[0] = line;
                        continue;
                    }
                    if (lineCounter == 2)
                    {
                        //Read first comment
                        inputFileComment[1] = line;
                        continue;
                    }
                    if (line.IndexOf("THALF") > -1)
                    {
                        var splittedLine = line.Split(" ");
                        double defaultHalfTime = 0.0;
                        if (splittedLine.Length > 1 && double.TryParse(splittedLine[1], out defaultHalfTime))
                        {
                            rNuclide = GetFromDatabase(
                                defaultHalfTime, 
                                new RadionuclideTextReader(), 
                                Radionuclides.RadionuclidesFilePath);
                        }
                        else
                        {
                            Trace.WriteLine($"Incorrect data in line {line}. Check Half-Time value!");
                        }
                    }
                        
                    if (line.IndexOf("COMPARTMENTS") > -1)
                        compartments = ReadCompartments(rd);
                    if (line.IndexOf("TRANSFERS") > -1)
                        transfers = ReadTransfers(rd);
                }
            }
            return new InputFile(fileName, rNuclide, compartments, transfers) { Comment_1 = inputFileComment[0], Comment_2 = inputFileComment[1] };
        }

        public void WriteFile(string outFileName, InputFile file)
        {
            //if (!File.Exists(outFileName))
            //{
            //    File.CreateText(outFileName).Close();
            //}
            
            using (StreamWriter wr = new StreamWriter(outFileName))
            {
                StringBuilder lines = new StringBuilder();
                lines.AppendLine(file.Comment_1);
                lines.AppendLine(file.Comment_2);
                lines.AppendLine(String.Format("THALF {0:E4}",file.Radionuclide.HalfTime));
                lines.AppendLine("COMPARTMENTS  A(0)                     <- Delimiter for initial condition block");
                for (int i = 0; i < file.Compartments.Count; i++)
                {
                    lines.AppendLine(String.Format(
                        "{0,-11}{1,-10:E3}", 
                        file.Compartments[i].Name, 
                        file.Compartments[i].A0));
                }
                lines.AppendLine(
                    "END LIST                               <- Delimiter for end of block\n"+
                    "TRANSFERS                 (/d)         <- Delimiter for start of transfer data");
                for (int i = 0; i < file.Transfers.Count; i++)
                {
                    lines.AppendLine(String.Format(
                        "{0,-10}->{1,-11}{2,-10:E4}",
                        file.Transfers[i].From,
                        file.Transfers[i].To,
                        file.Transfers[i].Time));
                }
                lines.AppendLine("EOF Data");
                wr.Write(lines);
                wr.Close();
            }

        }

        private Radionuclide GetFromDatabase(double halfTime, IRadionuclideReader reader, string dbPath)
        {
            var nuclideList = reader.ReadFile(dbPath);
            if (nuclideList != null)
                for (int i = 0; i < nuclideList.Count; i++)
                {
                    if (Math.Abs(nuclideList[i].HalfTime - halfTime) < 1E-6)
                    {
                        return nuclideList[i];
                    }
                }
            Trace.WriteLine($"One can't find radionuclide from user file. It will be added to database");
            return new Radionuclide("User-defined", halfTime);
            //return DefaultDataController.GetDefaultRadionuclide();
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
        private List<Transfer> ReadTransfers(StreamReader sr)
        {
            string line = "";
            int lineCounter = 0;
            var list = new List<Transfer>();
            while ((line = sr.ReadLine()).IndexOf("EOF Data") == -1)
            {
                lineCounter++;
                if (string.IsNullOrEmpty(line))
                    continue;

                var splittedString = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (splittedString.Length < 3)
                {
                    //Log error information
                    Trace.WriteLine($"Incorrect transfer data at line {line}. This line was skipped.");
                    continue;
                }
                double transferValue = 0.0;
                var transferFrom = splittedString[0].Trim();
                var transferTo = splittedString[1].Trim().Substring(2, splittedString[1].Length-2);
                if (double.TryParse(splittedString[2], out transferValue))
                {
                    var transfer = new Transfer(transferFrom, transferTo, transferValue);
                    list.Add(transfer);
                }
                else
                {
                    //Log error information
                    Trace.WriteLine($"Incorrect transfer {transferFrom} -> {transferTo} value at {lineCounter} position. This value was skipped.");
                    //TODO: Add trace listener to show user in UI
                }
            }
            return list;
        }
    }
}
