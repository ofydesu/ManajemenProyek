using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace RAB.Models.Utama
{
    public class KomponenPola
    {
        public int KomPolId { get; set; }
        public int? PolaId { get; set; }
        [DisplayName("Komponen")]
        public int? KompId { get; set; }

        public virtual Pola Pola { get; set; }
        public virtual Pola Komponen { get; set; }

        public virtual ICollection<KomponenGaris> TblKompGaris { get; set; }
        public virtual ICollection<KomponenKoordinat> TblKompPolaKoord { get; set; }
    }
}
