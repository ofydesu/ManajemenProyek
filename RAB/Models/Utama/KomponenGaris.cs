using RAB.Asset.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Models.Utama
{
    public class KomponenGaris
    {
        public int KompGrsId { get; set; }
        public int GarisId { get; set; }
        [DisplayName("Komponen")]
        public int KompId { get; set; }
        [DisplayName("Posisi Relatif Thd Z")]
        public EPosKompDiZ PosRelatifDiZ { get; set; } = EPosKompDiZ.Bawah;
        [DisplayName("Posisi Sumbu Z (vertikal)")]
        public int PosZId { get; set; }
        [DisplayName("Posisi Relatif Thd Awal Garis")]
        public EPosKompDiGaris PosRelatif { get; set; } = EPosKompDiGaris.Kiri;
        [DisplayName("Posisi Relatif X Pilihan")]
        public int PosRelX { get; set; }
        [DisplayName("Posisi Relatif Y Pilihan")]
        public int PosRelY { get; set; }

        [DisplayName("Offset Ke Kanan")]
        public int OffsetKeKanan { get; set; }

        public virtual Garis Garis { get; set; }
        public virtual KomponenPola PolaKomponen { get; set; }
        public virtual Titik TitikZ { get; set; }
    }
}
