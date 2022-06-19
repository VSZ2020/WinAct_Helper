using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAct_Helper.Model
{
    public class Compartment
    {
        const int MAX_NAME_LENGTH = 10;
        private string _name = "";
        public string Name {
            get => _name;
            set {
                _name = (value.Length <= MAX_NAME_LENGTH) ? value : value.Substring(0,MAX_NAME_LENGTH);
            }
        }
        public double A0 { get; set; }

        public Compartment(string name, double InitA0)
        {
            Name = name;
            A0 = InitA0;
        }

        public Compartment(string name) : this(name, 0.0) { }

        public Compartment Copy()
        {
            return new Compartment(this.Name, this.A0);
        }
    }
}
