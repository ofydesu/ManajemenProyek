using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAB.Models.Utama;

namespace RAB.Data
{
    public class RabContext : IdentityDbContext
    {
        public RabContext(DbContextOptions<RabContext> options)
            : base(options)
        {
        }

        public DbSet<Pola> TblPola { get; set; }
        public DbSet<Titik> TblTitik { get; set; }
        public DbSet<Koordinat> TblKoordinat{ get; set; }
        public DbSet<KomponenPola> TblKomponenPola{ get; set; }
        public DbSet<Garis> TblGaris { get; set; }
        public DbSet<KomponenGaris> TblKomponenGaris { get; set; }
        public DbSet<KomponenKoordinat> TblKomponenKoordinat { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region pola
            builder.Entity<Pola>()
                .HasKey(p => p.PolaId);
            builder.Entity<Pola>()
                .HasIndex(p => p.Nama)
                .IsUnique();
            #endregion

            #region Titik
            builder
                .Entity<Titik>()
                .HasKey(t => t.TtkId);
            builder
                .Entity<Titik>()
                .HasOne(t => t.Pola)
                .WithMany(p => p.TblTitik)
                .HasForeignKey(t => t.PolaId)
                //.OnDelete(DeleteBehavior.Restrict)
                ;
            builder
                .Entity<Titik>()
                .HasIndex(t => new {t.PolaId, t.Sumbu, t.SumbuId })
                .IsUnique();

            #endregion

            #region koordinat
            builder
                .Entity<Koordinat>()
                .HasKey(k => k.KoordId);
            builder
                .Entity<Koordinat>()
                .HasOne(k => k.TitikX)
                .WithMany(t => t.TblKoordX)
                .HasForeignKey(k => k.Xid);
                //.OnDelete(DeleteBehavior.SetNull);
            builder
                .Entity<Koordinat>()
                .HasOne(k => k.TitikY)
                .WithMany(t => t.TblKoordY)
                .HasForeignKey(k => k.Yid);
            //.OnDelete(DeleteBehavior.SetNull);
            builder
                .Entity<Koordinat>()
                .HasIndex(k => new { k.Xid, k.Yid })
                .IsUnique();
            #endregion

            #region Komponen Pola
            builder
                .Entity<KomponenPola>()
                .HasKey(k => k.KomPolaId);
            builder
                .Entity<KomponenPola>()
                .HasOne(k => k.Pola)
                .WithMany(t => t.TblKompPola)
                .HasForeignKey(k => k.PolaId);

            builder
                .Entity<KomponenPola>()
                .HasOne(k => k.Komponen)
                .WithMany(t => t.KompDiTblPola)
                .HasForeignKey(k => k.KompId);

            builder
                .Entity<KomponenPola>()
                .HasIndex(k => new { k.PolaId, k.KompId })
                .IsUnique();
            #endregion

            #region Garis
            builder
                .Entity<Garis>()
                .HasKey(k => k.GarisId);
            builder
                .Entity<Garis>()
                .HasOne(k => k.KoordAwal)
                .WithMany(t => t.TblGarisAwal)
                .HasForeignKey(k => k.AwalId);
            //.OnDelete(DeleteBehavior.SetNull);
            builder
                .Entity<Garis>()
                .HasOne(k => k.KoordAkhir)
                .WithMany(t => t.TblGarisAkhir)
                .HasForeignKey(k => k.AkhirId);
            //.OnDelete(DeleteBehavior.SetNull);
            builder
                .Entity<Garis>()
                .HasIndex(k => new { k.AwalId, k.AkhirId })
                .IsUnique();
            #endregion

            #region komponen Garis
            builder
                .Entity<KomponenGaris>()
                .HasKey(k => k.KompGrsId);
            builder
                .Entity<KomponenGaris>()
                .HasOne(k => k.Garis)
                .WithMany(t => t.TblKompGaris)
                .HasForeignKey(k => k.GarisId);
            //.OnDelete(DeleteBehavior.SetNull);
            builder
                .Entity<KomponenGaris>()
                .HasOne(k => k.PolaKomponen)
                .WithMany(t => t.TblKompGaris)
                .HasForeignKey(k => k.KompPolaId);
            //.OnDelete(DeleteBehavior.SetNull);
            builder
                .Entity<KomponenGaris>()
                .HasOne(k => k.TitikZ)
                .WithMany(t => t.TblKompGarisZ)
                .HasForeignKey(k => k.PosZId);
            builder
                .Entity<KomponenGaris>()
                .HasIndex(k => new { k.GarisId, k.KompPolaId, k.PosRelatif, k.PosRelX })
                .IsUnique();
            #endregion

            #region komponen Garis
            builder
                .Entity<KomponenKoordinat>()
                .HasKey(k => k.KompKoorId);
            builder
                .Entity<KomponenKoordinat>()
                .HasOne(k => k.Koordinat)
                .WithMany(t => t.TblKompKoor)
                .HasForeignKey(k => k.KoorId);
            //.OnDelete(DeleteBehavior.SetNull);
            builder
                .Entity<KomponenKoordinat>()
                .HasOne(k => k.PolaKomponen)
                .WithMany(t => t.TblKompPolaKoord)
                .HasForeignKey(k => k.KompId);
            //.OnDelete(DeleteBehavior.SetNull);
            builder
                .Entity<KomponenKoordinat>()
                .HasOne(k => k.TitikZatas)
                .WithMany(t => t.TblKompKoorZatas)
                .HasForeignKey(k => k.PosAtasId);
            builder
                .Entity<KomponenKoordinat>()
                .HasOne(k => k.TitikZbawah)
                .WithMany(t => t.TblKompKoorZbawah)
                .HasForeignKey(k => k.PosBawahId);
            builder
                .Entity<KomponenKoordinat>()
                .HasIndex(k => new { k.KoorId })
                .IsUnique();
            #endregion
        }
    }
}
