﻿using FastCodeWriteOre.Classes;
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
        Impostazioni _impostazioni;
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
                    ClosedXML.Excel.IXLWorksheet meseSheet = excelRaccoltaDati.Worksheet(2);



                    var listDate = DatePeriodo();



                    for (int i = 6; i <= rowDiarioLast; i++)
                    {
                        var dataDiario = DateTime.Parse(excelDiario.Worksheet(1).Cell(i, 2).Value.ToString()).Date;
                        if (listDate.Contains(dataDiario))
                        {
                            var diarioItem =
                            (new OreCantiere()
                            {
                                Commessa = excelDiario.Worksheet(1).Cell(i, 3).Value.ToString(),
                                Data = dataDiario,
                                Ore = decimal.Parse(excelDiario.Worksheet(1).Cell(i, 7).Value.ToString()),
                                Cantiere = excelDiario.Worksheet(1).Cell(i, 5).Value.ToString() == "C",

                            });
                            list.Add(diarioItem);
                        }
                    }

                    var listGR = list.GroupBy(a => new { a.Commessa, a.Cantiere, a.Data }).ToList();



                    var listOre = listGR.Select(a => new { a.Key.Cantiere, a.Key.Commessa, a.Key.Data, SommaOre = a.Sum(b => b.Ore) }).ToList();


                    dataOutput = listOre.Select(a => new RowExcel() { Data = a.Data, Commessa = a.Commessa, NumeroOre = a.SommaOre, Cantiere = a.Cantiere }).ToList();




                    /*aggiungo le date mancanti del periodo selezionato*/
                    foreach (var itemData in listDate )
                    {
                        if (dataOutput.Where(a => a.Data == itemData).Count() == 0)
                        {
                            var itemRow = (new RowExcel() { Data = itemData });


                            dataOutput.Add(itemRow);
                        }
                    }
                    foreach (var itemOut in dataOutput.Where(a => a.Cantiere || _impostazioni.TrasfertaEstera).ToList())
                    {
                        string opt = "";
                        if (string.IsNullOrEmpty(itemOut.Commessa) && _impostazioni.TrasfertaEstera)
                        {

                            opt = "Giorno non lavorativo cantiere Estero";
                        }
                        else
                        {
                            if (_impostazioni.TrasfertaEstera)
                            {
                                opt = "Cantiere UE";
                            }
                            else
                            {
                                opt = "Cantiere Italia";
                            }
                        }
                        itemOut.Pasto = true;
                        itemOut.SedeLavoro = opt;
                    }



                    RipulisciFoglio(meseSheet);


                    ScriviOre(  meseSheet, dataOutput);

                    excelRaccoltaDati.Save();
                }

            }

           
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = _impostazioni.RaccoltaDati.File,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
            MessageBox.Show("Operazione conclusa con successo", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);



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

            meseSheet.Range("A3:Z100").Clear(ClosedXML.Excel.XLClearOptions.Contents);

        }


        private void ScriviOre(  
            ClosedXML.Excel.IXLWorksheet meseSheet, List<RowExcel> dataOutput)
        {
            int row = 3;

            var it = new CultureInfo("it-IT");
            while (dataOutput.Count() > 0)
            {

                var itemRowOutput = dataOutput.Select(a => a).OrderBy(a => a.Data).FirstOrDefault();
                dataOutput.Remove(itemRowOutput);

                if (!meseSheet.Cell(row, 4).IsEmpty())
                {

                    meseSheet.Row(row).InsertRowsBelow(1);
                    row++;
                }
                meseSheet.Cell(row, 1).Value = itemRowOutput.Data.ToString("dd/MM/yyyy");
                meseSheet.Cell(row, 2).Value = itemRowOutput.Data.ToString("ddd", it);
                meseSheet.Cell(row, 3).Value = System.Globalization.ISOWeek.GetWeekOfYear(itemRowOutput.Data);


                meseSheet.Cell(row, 4).Value = itemRowOutput.Commessa;
                meseSheet.Cell(row, 6).Value = itemRowOutput.NumeroOre;

                meseSheet.Cell(row, 5).Value = itemRowOutput.SedeLavoro;

                if (itemRowOutput.Pasto)
                    Pernotto(meseSheet, row);



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
