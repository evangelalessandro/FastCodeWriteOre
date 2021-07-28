using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCodeWriteOre.Classes
{
    internal class RowExcel
    {
        public DateTime Data { get; set; }
        public string Day { get; set; }

        public int WeekName { get; set; }

        public string Commessa { get; set; }

        public bool Cantiere { get; set; }

        public string SedeLavoro { get; set; }

        public decimal NumeroOre { get; set; }

        public bool Pasto { get; set; }
    }
}
