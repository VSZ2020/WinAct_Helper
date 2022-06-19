using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAct_Helper.Model
{
    public class Radionuclide
    {
        const int MAX_NAME_LENGTH = 10;
        private string _name = "";

        public string Name
        {
            get => _name;
            set
            {
                _name = (value.Length <= MAX_NAME_LENGTH) ? value : value.Substring(0, MAX_NAME_LENGTH);
            }
        }
        /// <summary>
        /// Nuclide Half-Life in days (d)
        /// </summary>
        public double HalfTime;

        public Radionuclide(string name, double halftime)
        {
            Name = name;
            HalfTime = halftime;
        }
    }
}
