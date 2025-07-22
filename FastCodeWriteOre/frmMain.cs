using System;
using System.IO;
using System.Windows.Forms;

namespace FastCodeWriteOre {
  public partial class frmMain : Form {
    Impostazioni _impostazioni;


    public frmMain() {
      InitializeComponent();
      _impostazioni = Impostazioni.Read();

      if (_impostazioni.dataInizio < DateTime.Now.AddMonths(-3))
        _impostazioni.dataInizio = DateTime.Now.AddMonths(-2);
      if (_impostazioni.dataFine < DateTime.Now.AddMonths(-2))
        _impostazioni.dataFine = DateTime.Now.AddMonths(-1);

      dtExportStart.Value = _impostazioni.dataInizio;
      dtExportEnd.Value = _impostazioni.dataFine;


      dtExportStart.DataBindings.Add("Value", _impostazioni, "dataInizio");
      dtExportEnd.DataBindings.Add("Value", _impostazioni, "dataFine");
      chkEstero.DataBindings.Add("Checked", _impostazioni, "TrasfertaEstera");


    }

    private void button1_Click(object sender, EventArgs e) {

      SelectFile();
    }
    private void SelectFile() {
      if (_impostazioni.dataInizio >= _impostazioni.dataFine) {
        MessageBox.Show("Data inizio maggiore di quella di fine!!", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      var domanda = (MessageBox.Show("Vuoi usare i file selezionati l'ultima volta?", "Domanda",
          MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question));
      if (domanda == DialogResult.Cancel) {
        return;
      }
      if (domanda == DialogResult.No) {
        _impostazioni.RaccoltaDati.File = "";
        _impostazioni.DiarioCantiere.File = "";
      }
      _impostazioni.RaccoltaDati.File = "";
      _impostazioni.TrasfertaEstera = chkEstero.Checked;

      using (OpenFileDialog res = new OpenFileDialog()) {
        if (string.IsNullOrEmpty(_impostazioni.DiarioCantiere.File)) {
          res.Title = "Seleziona file diario cantiere";
          //Filter
          res.Filter = "File excel|*fastcode*.xlsm;|Tutti i file|*.*";

          res.Multiselect = false;
          if (!string.IsNullOrEmpty(_impostazioni.DiarioCantiere.Cartella))
            res.InitialDirectory = _impostazioni.DiarioCantiere.Cartella;
          //When the user select the file
          if (res.ShowDialog() == DialogResult.OK) {
            _impostazioni.DiarioCantiere.File = res.FileName;
            _impostazioni.DiarioCantiere.Cartella = new FileInfo(_impostazioni.DiarioCantiere.File).DirectoryName;
          }
        }
        if (string.IsNullOrEmpty(_impostazioni.RaccoltaDati.File)) {
          res.Title = "Seleziona file RaccoltaDati x dip Cantieri";
          //Filter
          res.Filter = "File excel|*.xls;*.xlsx|Tutti i file|*.*";

          res.Multiselect = false;
          if (!string.IsNullOrEmpty(_impostazioni.RaccoltaDati.Cartella))
            res.InitialDirectory = _impostazioni.RaccoltaDati.Cartella;
          //When the user select the file
          if (res.ShowDialog() == DialogResult.OK) {
            _impostazioni.RaccoltaDati.File = res.FileName;
            _impostazioni.RaccoltaDati.Cartella = new FileInfo(_impostazioni.RaccoltaDati.File).DirectoryName;
          }
        }
        if (!string.IsNullOrEmpty(_impostazioni.RaccoltaDati.File)
            && !string.IsNullOrEmpty(_impostazioni.DiarioCantiere.File)) {
          var elab = new EsportaDatiClass(_impostazioni);
          elab.Elabora();
          Impostazioni.Save(_impostazioni);
        }
      }

    }




    private void dateTimePicker2_Validated(object sender, EventArgs e) {
      CalcolaDiff();
    }

    private void CalcolaDiff() {
      dateTimePicker3.Value = new DateTime(dateTimePicker2.Value.Date.Ticks + (dateTimePicker2.Value - dateTimePicker1.Value).Ticks);
    }


    private void dateTimePicker1_Validated(object sender, EventArgs e) {
      CalcolaDiff();
    }

    private void button2_Click(object sender, EventArgs e) {
      CalcolaDiff();
    }

    private void btnAddMese_Click(object sender, EventArgs e) {
      btnAddMese.Focus();


      var dt = dtExportEnd.Value.AddDays(1);
      _impostazioni.dataInizio = dt;
      _impostazioni.dataFine = dt.AddDays(30);
      dtExportStart.Value = _impostazioni.dataInizio;
      dtExportEnd.Value = _impostazioni.dataFine;
      dtExportStart.Refresh();
      dtExportEnd.Refresh();

      //dtExportStart.DataBindings.Add("Value", _impostazioni, "dataInizio");
      //dtExportEnd.DataBindings.Add("Value", _impostazioni, "dataFine");
    }
  }
}
