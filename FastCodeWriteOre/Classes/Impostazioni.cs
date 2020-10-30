using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastCodeWriteOre
{
    public class Impostazioni
    {
        public Impostazioni()
        {
            dataInizio = DateTime.Now.AddMonths(-1);
            dataInizio = dataInizio.AddDays(-dataInizio.Day + 1 + 20);

            dataFine = dataInizio.AddMonths(1);
            dataFine = dataFine.AddDays(-dataFine.Day + 1 + 20);

            while (dataInizio.DayOfWeek != DayOfWeek.Monday)
            {
                dataInizio = dataInizio.AddDays(1);
            }
            while (dataFine.DayOfWeek != DayOfWeek.Sunday)
            {
                dataFine = dataFine.AddDays(+1);
            }
        }
        public FileCartella DiarioCantiere { get; set; } = new FileCartella();
        public FileCartella  RaccoltaDati { get; set; } = new FileCartella();
 
        public DateTime dataInizio { get; set; }
        public DateTime dataFine { get; set; }

        public class FileCartella
        {
            public string File { get; set; }

            public string Cartella { get; set; }


        }
        public static Impostazioni Read()
        {
            var setting = Path.Combine(Application.StartupPath, "Setting.json");
            try
            {
                return JsonConvert.DeserializeObject<Impostazioni>(File.ReadAllText(setting));

            }
            catch (Exception)
            {
                return new Impostazioni();
            }
        }
        public static void Save(Impostazioni impostazioni)
        {
            var setting = Path.Combine(Application.StartupPath, "Setting.json");
            try
            {
                File.WriteAllText(setting, JsonConvert.SerializeObject(impostazioni));

            }
            catch (Exception ex)
            {

            }
        }

    }

}
