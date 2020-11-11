using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Models.Utama
{
    public class KomponenKoordinat
    {
        public int KompKoorId { get; set; }
        [DisplayName("Koordinat")]
        public int KoorId { get; set; }
        [DisplayName("Komponen")]
        public int KompId { get; set; }
        [DisplayName("Posisi Atas")]
        public int? PosAtasId { get; set; }
        [DisplayName("Posisi Bawah")]
        public int? PosBawahId { get; set; }

        public virtual Koordinat Koordinat { get; set; }
        public virtual KomponenPola PolaKomponen { get; set; }
        public virtual Titik TitikZatas { get; set; }
        public virtual Titik TitikZbawah { get; set; }
    }
}
