using RAB.Asset.Enum;
using RAB.Asset.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Models.Utama
{
    public class Pola : IPola
    {
        public int PolaId { get; set; }
        [Column(TypeName ="varchar(100)")]
        [Required(ErrorMessage = "Nama harus ada dan UNIK")]
        public string Nama { get; set; }
        [Required(ErrorMessage = "Status harus ada dan UNIK")]
        public EStatusPola Status {get; set ; }

        [Required(ErrorMessage = "Satuan harus diisi")]
        [DisplayName("Satuan Penyusun")]
        public ESat1D SatuanPenyusun { get; set; }
        [DisplayName("Posisi dalam 3 Dimensi")]
        public EPosisi3D Posisi3D { get; set; } = EPosisi3D.Denah;

        public virtual ICollection<Titik> TblTitik{ get; set; }
        public virtual ICollection<KomponenPola> TblKompPola { get; set; }
        public virtual ICollection<KomponenPola> KompDiTblPola { get; set; }

    }
}
