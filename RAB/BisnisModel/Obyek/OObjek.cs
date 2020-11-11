using RAB.Asset.Enum;
using RAB.Asset.Interface.Obyek;
using RAB.Asset.OlahanModel;
using RAB.Models.Utama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Asset.Obyek
{
    public class AKoord
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public string Nama { get; set; }
    }

    public class AGaris
    {
        public AKoord KoorAwal { get; set; }
        public AKoord KoorAkhir { get; set; }
        public string Nama { get {
                return KoorAwal.Nama + " " + KoorAkhir.Nama;
            } }
        public ESumbu Arah
        {
            get
            {
                if(KoorAwal.PosY == KoorAkhir.PosY)
                {
                    return ESumbu.X;
                }
                return ESumbu.Y;
            }
        }
        public int Panjang { get {
                if(Arah == ESumbu.X)
                {
                    return KoorAkhir.PosX - KoorAwal.PosX;
                }
                return KoorAkhir.PosY - KoorAwal.PosY;
            }
        }

    }
    /*
    public class OKoord : IKoord
    {
        //public int Id { get; set; }
        public OTitikBerposisi X { get; set; }
        public OTitikBerposisi Y { get; set; }

        public string Nama
        {
            get {
                string nama = "";
                try
                {
                    nama = X.Nama + " " + Y.Nama;
                }
                catch { }
                return nama;
            }
        }
    }


    public class OPersegi : IPersegi
    {
        public OGaris Datar { get; set; }
        public OGaris Tegak { get; set; }

        public string Nama {
            get
            {
                string kiB = Datar.Awal.Nama;
                string kaB = Datar.Akhir.Nama;
                string kiA = Tegak.Akhir.Nama;
                string kaA = Datar.Akhir.TitikX.Nama + Tegak.Akhir.TitikY.Nama;

                return kiB + "-"+ kaB + "-" + kiA + "-" + kaA;
            }
        }
    }
    */
}
