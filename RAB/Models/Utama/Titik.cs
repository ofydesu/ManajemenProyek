using RAB.Asset.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Models.Utama
{
    public class Titik
    {
        [DisplayName("No Grid")]
        public int TtkId { get; set; }
        [DisplayName("No Pola")]
        public int PolaId { get; set; }

        [Required(ErrorMessage = "No Urut harus ada dan UNIK")]
        [DisplayName("Urutan")]
        public int SumbuId { get; set; }
        public ESumbu Sumbu { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "Nama harus ada dan UNIK")]
        public string Nama { get; set; }
        public int Jarak { get; set; }
        public int PosAbs { get; set; }
        public int PosRel { get; set; }

        public virtual Pola Pola { get; set; }
        public virtual ICollection<Koordinat> TblKoordX { get; set; }
        public virtual ICollection<Koordinat> TblKoordY { get; set; }
        public virtual ICollection<KomponenGaris> TblKompGarisZ { get; set; }
        public virtual ICollection<KomponenKoordinat> TblKompKoorZatas { get; set; }
        public virtual ICollection<KomponenKoordinat> TblKompKoorZbawah { get; set; }

    }
}
