﻿namespace FastCodeWriteOre {
  partial class frmMain {
    /// <summary>
    /// Variabile di progettazione necessaria.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Pulire le risorse in uso.
    /// </summary>
    /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Codice generato da Progettazione Windows Form

    /// <summary>
    /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
    /// il contenuto del metodo con l'editor di codice.
    /// </summary>
    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
      button1 = new System.Windows.Forms.Button();
      dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
      dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
      dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
      button2 = new System.Windows.Forms.Button();
      groupBox1 = new System.Windows.Forms.GroupBox();
      dtExportStart = new System.Windows.Forms.DateTimePicker();
      dtExportEnd = new System.Windows.Forms.DateTimePicker();
      chkEstero = new System.Windows.Forms.CheckBox();
      btnAddMese = new System.Windows.Forms.Button();
      groupBox1.SuspendLayout();
      SuspendLayout();
      // 
      // button1
      // 
      button1.FlatAppearance.BorderSize = 2;
      button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(192, 255, 192);
      button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(255, 255, 128);
      button1.Image = (System.Drawing.Image)resources.GetObject("button1.Image");
      button1.Location = new System.Drawing.Point(39, 366);
      button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      button1.Name = "button1";
      button1.Size = new System.Drawing.Size(313, 87);
      button1.TabIndex = 0;
      button1.Text = "Esporta ore";
      button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      button1.UseVisualStyleBackColor = false;
      button1.Click += button1_Click;
      // 
      // dateTimePicker1
      // 
      dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
      dateTimePicker1.Location = new System.Drawing.Point(71, 45);
      dateTimePicker1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      dateTimePicker1.Name = "dateTimePicker1";
      dateTimePicker1.Size = new System.Drawing.Size(265, 27);
      dateTimePicker1.TabIndex = 1;
      dateTimePicker1.Validated += dateTimePicker1_Validated;
      // 
      // dateTimePicker2
      // 
      dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
      dateTimePicker2.Location = new System.Drawing.Point(71, 85);
      dateTimePicker2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      dateTimePicker2.Name = "dateTimePicker2";
      dateTimePicker2.Size = new System.Drawing.Size(265, 27);
      dateTimePicker2.TabIndex = 2;
      dateTimePicker2.Validated += dateTimePicker2_Validated;
      // 
      // dateTimePicker3
      // 
      dateTimePicker3.Enabled = false;
      dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Time;
      dateTimePicker3.Location = new System.Drawing.Point(71, 125);
      dateTimePicker3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      dateTimePicker3.Name = "dateTimePicker3";
      dateTimePicker3.Size = new System.Drawing.Size(265, 27);
      dateTimePicker3.TabIndex = 3;
      // 
      // button2
      // 
      button2.Location = new System.Drawing.Point(71, 162);
      button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      button2.Name = "button2";
      button2.Size = new System.Drawing.Size(265, 37);
      button2.TabIndex = 4;
      button2.Text = "Calcola Differenza orari";
      button2.UseVisualStyleBackColor = true;
      button2.Click += button2_Click;
      // 
      // groupBox1
      // 
      groupBox1.Controls.Add(dateTimePicker1);
      groupBox1.Controls.Add(button2);
      groupBox1.Controls.Add(dateTimePicker2);
      groupBox1.Controls.Add(dateTimePicker3);
      groupBox1.Location = new System.Drawing.Point(16, 19);
      groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      groupBox1.Name = "groupBox1";
      groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      groupBox1.Size = new System.Drawing.Size(389, 218);
      groupBox1.TabIndex = 5;
      groupBox1.TabStop = false;
      groupBox1.Text = "Calcola differenae orarie";
      // 
      // dtExportStart
      // 
      dtExportStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      dtExportStart.Location = new System.Drawing.Point(39, 260);
      dtExportStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      dtExportStart.Name = "dtExportStart";
      dtExportStart.Size = new System.Drawing.Size(163, 27);
      dtExportStart.TabIndex = 6;
      // 
      // dtExportEnd
      // 
      dtExportEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      dtExportEnd.Location = new System.Drawing.Point(39, 297);
      dtExportEnd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      dtExportEnd.Name = "dtExportEnd";
      dtExportEnd.Size = new System.Drawing.Size(163, 27);
      dtExportEnd.TabIndex = 7;
      // 
      // chkEstero
      // 
      chkEstero.AutoSize = true;
      chkEstero.Location = new System.Drawing.Point(70, 333);
      chkEstero.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      chkEstero.Name = "chkEstero";
      chkEstero.Size = new System.Drawing.Size(132, 24);
      chkEstero.TabIndex = 8;
      chkEstero.Text = "Trasferta Estera";
      chkEstero.UseVisualStyleBackColor = true;
      // 
      // btnAddMese
      // 
      btnAddMese.Image = (System.Drawing.Image)resources.GetObject("btnAddMese.Image");
      btnAddMese.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      btnAddMese.Location = new System.Drawing.Point(222, 260);
      btnAddMese.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      btnAddMese.Name = "btnAddMese";
      btnAddMese.Size = new System.Drawing.Size(130, 64);
      btnAddMese.TabIndex = 9;
      btnAddMese.Text = "Mese successivo";
      btnAddMese.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      btnAddMese.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      btnAddMese.UseVisualStyleBackColor = true;
      btnAddMese.Click += btnAddMese_Click;
      // 
      // frmMain
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(414, 476);
      Controls.Add(btnAddMese);
      Controls.Add(chkEstero);
      Controls.Add(dtExportEnd);
      Controls.Add(dtExportStart);
      Controls.Add(groupBox1);
      Controls.Add(button1);
      Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
      ImeMode = System.Windows.Forms.ImeMode.Off;
      Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      Name = "frmMain";
      Text = "Form1";
      groupBox1.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.DateTimePicker dateTimePicker1;
    private System.Windows.Forms.DateTimePicker dateTimePicker2;
    private System.Windows.Forms.DateTimePicker dateTimePicker3;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.DateTimePicker dtExportStart;
    private System.Windows.Forms.DateTimePicker dtExportEnd;
    private System.Windows.Forms.CheckBox chkEstero;
    private System.Windows.Forms.Button btnAddMese;
  }
}

