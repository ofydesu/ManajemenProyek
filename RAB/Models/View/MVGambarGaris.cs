using RAB.Asset.Obyek;
using RAB.Asset.OlahanModel;
using RAB.Models.Utama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Models.View
{
    public class MVGambarGaris
    {
        public int? PolaId { get; set; }
        public IEnumerable<Koordinat> LstKoord { get; set; }
        public IEnumerable< OKetGarisKoordinat> LstGrsKoord { get; set; }
        public IEnumerable<OGaris> LstGarisReal { get; set; }
        public IEnumerable<OGaris> LstGarisSemu { get; set; }
        public int TinggiMax { get; set; }
        public int LebarMax { get; set; }
        public decimal Skala { get; set; }
        public int SkalaBulat
        {
            get
            {
                return decimal.ToInt32(Skala * 100);
            }
        }
    }

    public class MVGambarKompGaris
    {
        public MVGambarGarisValid GarisValid { get; set; }
        public MVGambarGarisParsial GarisParsial { get; set; }
    }
    public class MVGambarGarisValid
    {
        public int? PolaId { get; set; }
        public IEnumerable<Koordinat> LstKoord { get; set; }
        public IEnumerable<OKetGarisKoordinat> LstGrsKoord { get; set; }
        public IEnumerable<Garis> LstGarisReal { get; set; }
        public IEnumerable<Garis> LstGarisSemu { get; set; }
        public int TinggiMax { get; set; }
        public int LebarMax { get; set; }
        public decimal Skala { get; set; }
        public int SkalaBulat
        {
            get
            {
                return decimal.ToInt32(Skala * 100);
            }
        }
    }
    public class MVGambarGarisParsial
    {
        public int? PolaId { get; set; }
        public IEnumerable<Koordinat> LstKoord { get; set; }
        public IEnumerable<OKetGarisKoordinat> LstGrsKoord { get; set; }
        public Garis GarisReal { get; set; }
        public Garis GarisSemu { get; set; }
        public List<AGaris> LstGarisKomp { get; set; }
        public int TinggiMax { get; set; }
        public int LebarMax { get; set; }
        public decimal Skala { get; set; }
        public int SkalaBulat
        {
            get
            {
                return decimal.ToInt32(Skala * 100);
            }
        }
    }

}
