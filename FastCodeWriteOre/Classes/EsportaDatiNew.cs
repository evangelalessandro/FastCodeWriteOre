using FastCodeWriteOre.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastCodeWriteOre
{
    public class EsportaDatiClass
    {
        private Impostazioni _impostazioni;

        public EsportaDatiClass(Impostazioni impostazioni)
        {
            _impostazioni = impostazioni;
        }

        public void Elabora()
        {
            var dataOutput = new List<RowExcel>();

            using (var excelRaccoltaDati = new ClosedXML.Excel.XLWorkbook(_impostazioni.RaccoltaDati.File))
            {
                using (var excelDiario = new ClosedXML.Excel.XLWorkbook(_impostazioni.DiarioCantiere.File))
                {
                    var rowDiarioLast = excelDiario.Worksheet(1).Range("B:B").LastCellUsed().Address.RowNumber;

                    var list = new List<OreCantiere>();
                    ClosedXML.Excel.IXLWorksheet meseSheet = excelRaccoltaDati.Worksheet(1);

                    var listDate = DatePeriodo();
                    EstraiDatiDiario(excelDiario, rowDiarioLast, list, listDate);

                    var listGR = list.GroupBy(a => new { a.Commessa, a.Cantiere, a.Data }).ToList();

                    var listOre = listGR.Select(a => new { a.Key.Cantiere, a.Key.Commessa, a.Key.Data, SommaOre = a.Sum(b => b.Ore) }).ToList();

                    dataOutput = listOre.Select(a => new RowExcel() { Data = a.Data, Commessa = a.Commessa, NumeroOre = a.SommaOre, Cantiere = a.Cantiere }).ToList();

                    var SommePerData = dataOutput.GroupBy(a => a.Data).Select(a => new { DataRif = a.Key, SommaOra = a.Sum(b => b.NumeroOre) }).Where(a => a.SommaOra != 8).ToList();

                    foreach (var item in SommePerData)
                    {
                        var itemToUpdate = dataOutput.Where(a => a.Data == item.DataRif).OrderByDescending(a => a.NumeroOre).First();
                        if (item.SommaOra > 8)
                        {
                            itemToUpdate.Note = $"Ore totali >8 in data {item.DataRif.ToString("dd/MM/yyyy")}; mettere {item.SommaOra - 8} di straordinari";
                        }
                        else if (item.SommaOra < 8)
                        {
                            itemToUpdate.Note = $"Solo {item.SommaOra} ore in data {item.DataRif.ToString("dd/MM/yyyy")}; mettere Rol nelle note";
                        }
                    }

                    AggiungiTipoSede(dataOutput);

                    RipulisciFoglio(meseSheet);

                    ScriviOre(meseSheet, dataOutput);

                    excelRaccoltaDati.Save();
                }
            }

            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = _impostazioni.RaccoltaDati.File,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
            //MessageBox.Show("Operazione conclusa con successo", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            static void EstraiDatiDiario(ClosedXML.Excel.XLWorkbook excelDiario, int rowDiarioLast, List<OreCantiere> list, List<DateTime> listDate)
            {
                for (int i = 6; i <= rowDiarioLast; i++)
                {
                    var dataDiario = DateTime.Parse(excelDiario.Worksheet(1).Cell(i, 2).Value.ToString()).Date;
                    if (listDate.Contains(dataDiario))
                    {
                        var diarioItem =
                        (new OreCantiere()
                        {
                            Commessa = excelDiario.Worksheet(1).Cell(i, 3).Value.ToString().Trim(),
                            Data = dataDiario,
                            Ore = decimal.Parse(excelDiario.Worksheet(1).Cell(i, 7).Value.ToString()),
                            Cantiere = excelDiario.Worksheet(1).Cell(i, 5).Value.ToString() == "C",
                        });
                        list.Add(diarioItem);
                    }
                }
            }

            void AggiungiTipoSede(List<RowExcel> dataOutput)
            {
                foreach (var itemOut in dataOutput.Where(a => a.Cantiere || _impostazioni.TrasfertaEstera || !string.IsNullOrEmpty(a.Commessa)).ToList())
                {
                    string opt = "";

                    if (string.IsNullOrEmpty(itemOut.Commessa) && _impostazioni.TrasfertaEstera)
                    {
                        opt = "Giorno non lavorativo cantiere Estero";
                    }
                    else if (itemOut.Cantiere)
                    {
                        if (_impostazioni.TrasfertaEstera)
                        {
                            opt = "CANTIERE EUROPA";
                        }
                        else
                        {
                            opt = "CANTIERE ITALIA";
                        }
                    }
                    else if (!string.IsNullOrEmpty(itemOut.Commessa))
                    {
                        opt = "SEDE";
                    }
                    itemOut.SedeLavoro = opt;
                }
            }
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
            meseSheet.Range("A2:Z100").Clear(ClosedXML.Excel.XLClearOptions.Contents);
        }

        private void ScriviOre(
            ClosedXML.Excel.IXLWorksheet meseSheet, List<RowExcel> dataOutput)
        {
            int row = 2;

            while (dataOutput.Count() > 0)
            {
                var itemRowOutput = dataOutput.Select(a => a).OrderBy(a => a.Data).FirstOrDefault();
                dataOutput.Remove(itemRowOutput);

                meseSheet.Cell(row, 1).Value = itemRowOutput.Data.ToString("dd/MM/yyyy");
                meseSheet.Cell(row, 2).Value = System.Globalization.ISOWeek.GetWeekOfYear(itemRowOutput.Data);
                meseSheet.Cell(row, 3).Value = "Alessandro Evangelisti";
                meseSheet.Cell(row, 4).Value = "CONS. AREA SVILUPPO SW (PLC-PC)";
                meseSheet.Cell(row, 5).Value = itemRowOutput.SedeLavoro;
                meseSheet.Cell(row, 7).Value = itemRowOutput.Commessa.Split(" ").Where(a => a.Length > 0).Last();
                meseSheet.Cell(row, 8).Value = itemRowOutput.Commessa;

                meseSheet.Cell(row, 10).Value = itemRowOutput.NumeroOre;

                meseSheet.Cell(row, 17).Value = itemRowOutput.Note;

                row++;
            }
        }

        private static void Pernotto(ClosedXML.Excel.IXLWorksheet meseSheet, int row)
        {
            meseSheet.Cell(row, 13).Value = "x";
            meseSheet.Cell(row, 14).Value = "x";
            meseSheet.Cell(row, 15).Value = "x";
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