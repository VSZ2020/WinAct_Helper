using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WinAct_Helper.Model;

namespace WinAct_Helper.Services
{
    internal class RadionuclideTextReader
    {
        public List<Radionuclide> ReadFile(string fileName)
        {
            var radionuclides = new List<Radionuclide>() { DefaultDataProvider.GetDefaultRadionuclide() };
            using (StreamReader reader = new StreamReader(fileName))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var buffer = line.Split(":");
                    double halftime = 0.0;
                    double.TryParse(buffer[1].Trim(), out halftime);
                    var nuclide = new Radionuclide(buffer[0], halftime);
                    radionuclides.Add(nuclide);
                }
            }
            return radionuclides;
        }
    }
}
