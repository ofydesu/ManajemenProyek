using RAB.Models.Utama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Asset.Obyek
{
 
    public class OKoordTambahan :Koordinat
    {

        public bool PermintaanDariGambar { get; set; }
        public decimal Skala { get; set; }
        public string PubNama { get; set; }
        public int IsiPolaId { get; set; }

    }
}
