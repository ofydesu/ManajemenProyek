using RAB.Asset.Enum;
using RAB.Asset.Interface.Obyek;
using RAB.Models.Utama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Asset.Obyek
{
    public class OGaris //: IGaris
    {
        public Koordinat Awal { get; set; }
        public Koordinat Akhir { get; set; }
        public ESumbu Arah { get; set; }
        public string Nama
        {
            get
            {
                return Awal.Nama + " - " + Akhir.Nama;
            }
        }
        public int Panjang
        {
            get
            {
                int pjg = 0;
                if (Arah == ESumbu.X)
                {
                    pjg = Akhir.TitikX.PosAbs - Awal.TitikX.PosAbs;
                }
                else
                {
                    pjg = Akhir.TitikY.PosAbs - Awal.TitikY.PosAbs;
                }
                return pjg;
            }
        }
    }
}
