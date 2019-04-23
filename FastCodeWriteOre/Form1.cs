using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace FastCodeWriteOre
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			dtExportStart.Value = DateTime.Now.AddMonths(-1);
			dtExportStart.Value = dtExportStart.Value.AddDays(-dtExportStart.Value.Day + 1 + 20);

			dtExportEnd.Value = dtExportStart.Value.AddMonths(1);
			dtExportEnd.Value = dtExportEnd.Value.AddDays(-dtExportEnd.Value.Day + 1 + 20);

			while (dtExportStart.Value.DayOfWeek!=DayOfWeek.Monday)
			{
				dtExportStart.Value=dtExportStart.Value.AddDays(1);
			}
			while (dtExportEnd.Value.DayOfWeek != DayOfWeek.Sunday)
			{
				dtExportEnd.Value=dtExportEnd.Value.AddDays(+1);
			}
			
		}
		public string DiarioCantiere { get; set; }
		public string FileRaccoltaDati { get; set; }
		private void button1_Click(object sender, EventArgs e)
		{
			//DiarioCantiere = @"C:\Users\fastcode13042017\Desktop\OrariCantieri\FASTCODE-EVANGELISTI_ALESSANDRO-Diario_Sede_Cantiere.xlsm";
			//FileRaccoltaDati = @"C:\Users\fastcode13042017\Desktop\RaccoltaDati x dip Cantieri Evangelisti Alessandro_Dic_2019.xlsx";

			SelectFile();
		}
		private void SelectFile()
		{
			using (OpenFileDialog res = new OpenFileDialog())
			{
				if (DiarioCantiere == null)
				{
					res.Title = "Seleziona file diario cantiere";
					//Filter
					res.Filter = "File excel|*fastcode*.xlsm;|Tutti i file|*.*";

					res.Multiselect = false;
					//When the user select the file
					if (res.ShowDialog() == DialogResult.OK)
					{
						DiarioCantiere = res.FileName;

					}
				}
				if (FileRaccoltaDati == null)
				{
					res.Title = "Seleziona file RaccoltaDati x dip Cantieri";
					//Filter
					res.Filter = "File excel|*.xls;*.xlsx|Tutti i file|*.*";

					res.Multiselect = false;
					//When the user select the file
					if (res.ShowDialog() == DialogResult.OK)
					{
						FileRaccoltaDati = res.FileName;

					}
				}
				if (FileRaccoltaDati != null && DiarioCantiere != null)
				{
					Elabora();
				}
			}

		}

		private void Elabora()
		{


			using (var excelRaccoltaDati = new ClosedXML.Excel.XLWorkbook(FileRaccoltaDati))
			{
				using (var excelDiario = new ClosedXML.Excel.XLWorkbook(DiarioCantiere))
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

			MessageBox.Show("Operazione conclusa con successo");


		}

		private List<DateTime> DatePeriodo()
		{
			return Enumerable
			.Range(0, int.MaxValue)
			.Select(index => new DateTime?(dtExportStart.Value.AddDays(index)))
			.TakeWhile(date => date <= dtExportEnd.Value)
			.Select(a=>a.Value.Date)
			.ToList();
		}

		private void RipulisciFoglio(ClosedXML.Excel.IXLWorksheet meseSheet)
		{
			if (checkClearAll.Checked)
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

		private void dateTimePicker2_Validated(object sender, EventArgs e)
		{
			CalcolaDiff();
		}

		private void CalcolaDiff()
		{
			dateTimePicker3.Value = new DateTime(dateTimePicker2.Value.Date.Ticks + (dateTimePicker2.Value - dateTimePicker1.Value).Ticks);
		}

		private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
		{

		}

		private void dateTimePicker1_Validated(object sender, EventArgs e)
		{
			CalcolaDiff();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			CalcolaDiff();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}
	}
}
