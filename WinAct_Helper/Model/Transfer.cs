using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAct_Helper.Model
{
    public class Transfer
    {
        const int MAX_FROM_LENGTH = 9;
        const int MAX_TO_LENGTH = 11;
        private string _from;
        private string _to;
        public string From
        {
            get => _from;
            set
            {
                _from = (value.Length <= MAX_FROM_LENGTH) ? value : value.Substring(0, MAX_FROM_LENGTH);
            }
        }
        public string To
        {
            get => _to;
            set
            {
                _to = (value.Length <= MAX_TO_LENGTH) ? value : value.Substring(0, MAX_TO_LENGTH);
            }
        }

        /// <summary>
        /// Transfer duration in days
        /// </summary>
        public double Time { get; set; } = 0.0;

        public Transfer(string from, string to):this(from, to, 0.0) { }

        public Transfer(string from, string to, double duration)
        {
            this.From = from;
            this.To = to;
            this.Time = duration;
        }

        public Transfer Copy()
        {
            return new Transfer(this.From, this.To, this.Time);
        }
    }
}
