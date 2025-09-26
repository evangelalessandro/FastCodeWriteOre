using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCodeWriteOre.Classes {
	internal class RowExcel {
		public DateTime Data { get; set; }
		public string DataString { get { return Data.ToString("ddMMYYYY"); } }
		public int WeekNumber { get; set; }

		public string Commessa { get; set; }

		public bool Cantiere { get; set; }

		public string SedeLavoro { get; set; }

		public decimal NumeroOre { get; set; } = decimal.Zero;
		public string Note { get; set; } = "";

	}
}
