using RAB.Asset.Obyek;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Models.Utama
{
    public class Koordinat
    {
        public int KoordId { get; set; }
        //public int PolaId { get; set; }
        [DisplayName("Sumbu X")]
        public int? Xid { get; set; }
        [DisplayName("Sumbu X")]
        public int? Yid { get; set; }

        [DisplayName("Tidak Ke X")]
        public bool TidakKeX { get; set; }

        [DisplayName("Sebagai Awal di X")]
        public bool SbgAwalX { get; set; }
        [DisplayName("Sebagai Awal di Y")]
        public bool SbgAwalY { get; set; }

        [DisplayName("Sebagai Akhir di X")]
        public bool SbgAkhirX { get; set; }
        [DisplayName("Sebagai akhir di Y")]
        public bool SbgAkhirY { get; set; }

        [DisplayName("Tidak Ke Y")]
        public bool TidakKeY { get; set; }

        [DisplayName("Miring Ke Atas")]
        public bool MiringAtas { get; set; }
        [DisplayName("Miring Ke Bawah")]
        public bool MiringBawah { get; set; }

        [DisplayName("Melengkung Ke Atas")]
        public bool LengkungAtas { get; set; }
        [DisplayName("Melengkung Ke Bawah")]
        public bool LengkungBawah { get; set; }

        [DisplayName("KiriBawah Semu")]
        public bool KiriBawahSemu { get; set; }

        public String Nama
        {
            get{
                var nama = "";
                if(TitikX != null || TitikY != null) { 
                    nama = TitikX.Nama + " " + TitikY.Nama;
                }
                return nama;
            }
        }
        public int PolaId { 
            get {
                int id = 0;
                if(TitikX != null)
                {
                    id = TitikX.PolaId;
                }
                return id;
            } 
        }

        public virtual Titik TitikX { get; set; }
        public virtual Titik TitikY { get; set; }

        public virtual ICollection<Garis> TblGarisAwal { get; set; }
        public virtual ICollection<Garis> TblGarisAkhir { get; set; }
        public virtual ICollection<KomponenKoordinat> TblKompKoor { get; set; }

    }
}
