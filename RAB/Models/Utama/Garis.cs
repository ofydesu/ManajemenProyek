using RAB.Asset.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Models.Utama
{
    public class Garis
    {
        public int GarisId { get; set; }
        public int? AwalId { get; set; }
        public int? AkhirId { get; set; }

        public ESumbu Arah { get; set; }
        public string Nama
        {
            get
            {
                string nama = "";
                if (KoordAwal != null && KoordAkhir != null)
                {
                    nama = KoordAwal.Nama + " - " + KoordAkhir.Nama;
                }
                return nama;
            }
        }
        public int Panjang
        {
            get
            {
                int pjg;    // = 0;
                if (Arah == ESumbu.X)
                {
                    pjg = KoordAkhir.TitikX.PosAbs - KoordAwal.TitikX.PosAbs;
                }
                else
                {
                    pjg = KoordAkhir.TitikY.PosAbs - KoordAwal.TitikY.PosAbs;
                }
                return pjg;
            }
        }
        public ESat1D Satuan
        {
            get
            {
                ESat1D sat = ESat1D.M;
                try
                {
                   sat = KoordAwal.TitikX.Pola.SatuanPenyusun;
                }
                catch { }
                return sat;
            }
        }


        public virtual Koordinat KoordAwal { get; set; }
        public virtual Koordinat KoordAkhir { get; set; }

        public virtual ICollection<KomponenGaris> TblKompGaris { get; set; }
    }
}
