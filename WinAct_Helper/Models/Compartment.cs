using WinAct_Helper.Common;

namespace WinAct_Helper.Model
{
    public class Compartment: BaseViewModel
    {
        const int MAX_NAME_LENGTH = 10;
        private string _name = "";
        private double _a0;

        public string Name {
            get => _name;
            set {
                _name = (value.Length <= MAX_NAME_LENGTH) ? value : value.Substring(0, MAX_NAME_LENGTH);
                OnChanged();
            }
        }
        public double A0 { get => _a0; set { _a0 = value; OnChanged(); } }

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
