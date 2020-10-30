using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastCodeWriteOre
{
	public class EsportaDati
    {
		Impostazioni _impostazioni;
		public EsportaDati(Impostazioni impostazioni)
		{
			_impostazioni = impostazioni;
		}
		public void Elabora()
		{


			using (var excelRaccoltaDati = new ClosedXML.Excel.XLWorkbook(_impostazioni.RaccoltaDati.File))
			{
				using (var excelDiario = new ClosedXML.Excel.XLWorkbook(_impostazioni.DiarioCantiere.File))
				{

					var row = 3;

					var rowDiarioLast = excelDiario.Worksheet(1).Range("B:B").LastCellUsed().Address.RowNumber;

					var list = new List<OreCantiere>();
					ClosedXML.Excel.IXLWorksheet meseSheet = excelRaccoltaDati.Worksheet(2);


					var listDate = DatePeriodo();

					for (int i = 6; i <= rowDiarioLast; i++)
					{
						var dataDiario = DateTime.Parse(excelDiario.Worksheet(1).Cell(i, 2).Value.ToString()).Date;
						if (listDate.Contains(dataDiario))
						{
							list.Add(new OreCantiere()
							{
								Commessa = excelDiario.Worksheet(1).Cell(i, 3).Value.ToString(),
								Data = dataDiario,
								Ore = decimal.Parse(excelDiario.Worksheet(1).Cell(i, 7).Value.ToString()),
								Cantiere = excelDiario.Worksheet(1).Cell(i, 5).Value.ToString() == "C",

							});
							System.Diagnostics.Debug.WriteLine(dataDiario);
						}
					}
					RipulisciFoglio(meseSheet);
					ScriviOre(excelRaccoltaDati, row, list, meseSheet);
				}

			}

			MessageBox.Show("Operazione conclusa con successo","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);


		}

		private List<DateTime> DatePeriodo()
		{
			return Enumerable
			.Range(0, int.MaxValue)
			.Select(index => new DateTime?(_impostazioni.dataInizio.AddDays(index)))
			.TakeWhile(date => date <= _impostazioni.dataFine)
			.Select(a => a.Value.Date)
			.ToList();
		}

		private void RipulisciFoglio(ClosedXML.Excel.IXLWorksheet meseSheet)
		{
			if (true)
			{
				meseSheet.Range("A3:Z100").Clear(ClosedXML.Excel.XLClearOptions.Contents);

				var row = 3;
				foreach (var item in DatePeriodo())
				{

					meseSheet.Cell(row, 1).Value = item.ToString("dd/MM/yyyy");
					meseSheet.Cell(row, 2).Value = item.ToString("ddd", new CultureInfo("it-IT"));
					row++;
				}
			}
		}

		private void ScriviOre(ClosedXML.Excel.XLWorkbook excelRaccoltaDati, int row, List<OreCantiere> list,
			ClosedXML.Excel.IXLWorksheet meseSheet)
		{
			var listGR = list.GroupBy(a => new { a.Commessa, a.Cantiere, a.Data }).ToList();



			var listOre = listGR.Select(a => new { a.Key.Cantiere, a.Key.Commessa, a.Key.Data, SommaOre = a.Sum(b => b.Ore) }).ToList();
			foreach (var item in listOre)
			{
				row = 3;
				while (meseSheet.Cell(row, 1).Value != null)
				{
					if (meseSheet.Cell(row, 1).Value.ToString() == "")
					{
						break;
					}
					var dataCel = DateTime.Parse(meseSheet.Cell(row, 1).Value.ToString()).Date;
					if (dataCel == item.Data)
					{
						if (meseSheet.Cell(row, 3).IsEmpty())
						{
							ScriviRiga(meseSheet, row, item.Commessa, item.SommaOre, item.Cantiere);
							break;
						}
						else
						{
							meseSheet.Row(row).InsertRowsBelow(1);
							row++;
							meseSheet.Cell(row, 1).Value = dataCel.ToString("dd/MM/yyyy");
							meseSheet.Cell(row, 2).Value = dataCel.ToString("ddd", new CultureInfo("it-IT"));

							ScriviRiga(meseSheet, row, item.Commessa, item.SommaOre, item.Cantiere);
							break;
						}

					}
					row++;
				}
			}
			excelRaccoltaDati.Save();
		}

		private void ScriviRiga(ClosedXML.Excel.IXLWorksheet meseSheet, int row, string commessa, decimal sommaOre, bool cantiere)
		{


			meseSheet.Cell(row, 3).Value = commessa;
			meseSheet.Cell(row, 5).Value = sommaOre;
			if (cantiere)
			{
				meseSheet.Cell(row, 4).Value = "Cantiere Italia";
				meseSheet.Cell(row, 12).Value = "x";
				meseSheet.Cell(row, 13).Value = "x";
				meseSheet.Cell(row, 14).Value = "x";
			}
			else
			{
				meseSheet.Cell(row, 4).Value = "Sede System o telework";
			}
		}
		internal class OreCantiere
		{
			public DateTime Data { get; set; }
			public string Commessa { get; set; }
			public decimal Ore { get; set; }
			public bool Cantiere { get; set; }
		}
	}
}
