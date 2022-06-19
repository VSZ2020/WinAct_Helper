using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WinAct_Helper.Model;

namespace WinAct_Helper.Model
{
    internal class DataKeeper
    {
        private List<Compartment> _compartments;
        private List<Transfer> _transfers;


        public DataKeeper(int compartmentsCount, int transfersCount)
        {
            _compartments = new List<Compartment>(compartmentsCount);
            _transfers = new List<Transfer>(transfersCount);
        }

        public DataKeeper(List<Compartment> compartments, List<Transfer> transfers)
        {
            if (compartments == null)
                throw new ArgumentNullException("Compartments list can't be NULL");
            if (transfers == null)
                throw new ArgumentNullException("Transfers list can't be NULL");

            this._compartments = compartments;
            this._transfers = transfers;
        }
    }
}
