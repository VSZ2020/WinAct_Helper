using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using WinAct_Helper.Model;

namespace WinAct_Helper.Controller
{
    internal class DefaultDataController
    {
        private Radionuclides _radionuclides;

        public DefaultDataController(Radionuclides LoadedRadionuclides)
        {
            _radionuclides = LoadedRadionuclides;
        }

        public InputFile CreateDefaultInput()
        {
            var input = new InputFile(
                String.Concat(
                    System.Environment.CurrentDirectory,
                    "Untitled.",
                    DefaultExtensions.INPUT_FILE_EXTENSION),
                _radionuclides.GetByIndex(0),
                GetDefaultCompartments(5),
                GetDefaultTransfers(5)){ Comment = "Default input file", IsModified = true };
            return input;
        }

        public static List<Compartment> GetDefaultCompartments(int count)
        {
            List<Compartment> comps = new List<Compartment>(count);
            for (int i = 0; i < count; i++)
                comps.Add(new Compartment(string.Concat("Compart ", i+1)));
            return comps;
        }

        public static List<Transfer> GetDefaultTransfers(int count)
        {
            List<Transfer> transf = new List<Transfer>(count);
            for (int i = 0; i < count; i++)
                transf.Add(new Transfer(string.Concat("From ", i+1), string.Concat("To ", i+1)));
            return transf;
        }

        public static Radionuclide GetDefaultRadionuclide()
        {
            return new Radionuclide("", 0.0);
        }
    }
}
