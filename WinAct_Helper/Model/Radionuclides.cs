using System.Collections.Generic;
using System;

using WinAct_Helper.Controller;

namespace WinAct_Helper.Model
{
    public class Radionuclides
    {
        public const string RadionuclidesFilePath = "data\\Radionuclides.txt";
        private List<Radionuclide> _radionuclides;

        public Radionuclide this[string name]
        {
            get
            {
                for (int i = 0; i < _radionuclides.Count; i++)
                    if (_radionuclides[i].Name == name)
                        return _radionuclides[i];
                return DefaultDataController.GetDefaultRadionuclide();
            }
        }
        //IRadionuclideReader Reader;
        public List<Radionuclide> Nuclides { 
            get => _radionuclides;
            set
            {
                if (value != null)
                    _radionuclides = value;
                else
                    throw new ArgumentNullException("Assigned object can't be NULL");
            }
        }
        public Radionuclides()
        {
            _radionuclides = new List<Radionuclide>();
        }

        public Radionuclide GetByIndex(int index)
        {
            return (_radionuclides.Count > 0 && index < _radionuclides.Count) ? _radionuclides[index] : DefaultDataController.GetDefaultRadionuclide();
        }

        public void ReadFromFile(IRadionuclideReader reader, string Filename)
        {
            var files = reader.ReadFile(Filename);
            _radionuclides = (files == null) ? new List<Radionuclide>() : files;
        }
    }
}
