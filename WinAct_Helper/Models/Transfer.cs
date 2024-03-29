using System.ComponentModel;
using WinAct_Helper.Common;

namespace WinAct_Helper.Model
{
    public class Transfer : BaseViewModel
    {
        const int MAX_FROM_LENGTH = 9;
        const int MAX_TO_LENGTH = 11;
        private string _from;
        private string _to;
        private double _constant;

        public string From
        {
            get => _from;
            set
            {
                _from = (value.Length <= MAX_FROM_LENGTH) ? value : value.Substring(0, MAX_FROM_LENGTH);
                OnChanged();
            }
        }
        public string To
        {
            get => _to;
            set
            {
                _to = (value.Length <= MAX_TO_LENGTH) ? value : value.Substring(0, MAX_TO_LENGTH);
                OnChanged();
            }
        }

        /// <summary>
        /// Transfer duration in days
        /// </summary>
        public double Constant { get => _constant; set { _constant = value; OnChanged(); } }

        public Transfer(string from, string to):this(from, to, 0.0) { }

        public Transfer(string from, string to, double constant)
        {
            this.From = from;
            this.To = to;
            this.Constant = constant;
        }

        public Transfer Copy()
        {
            return new Transfer(this.From, this.To, this.Constant);
        }
    }
}
