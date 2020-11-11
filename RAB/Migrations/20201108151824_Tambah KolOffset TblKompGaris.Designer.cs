﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RAB.Data;

namespace RAB.Migrations
{
    [DbContext(typeof(RabContext))]
    [Migration("20201108151824_Tambah KolOffset TblKompGaris")]
    partial class TambahKolOffsetTblKompGaris
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("RAB.Models.Utama.Garis", b =>
                {
                    b.Property<int>("GarisId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AkhirId")
                        .HasColumnType("int");

                    b.Property<int>("Arah")
                        .HasColumnType("int");

                    b.Property<int?>("AwalId")
                        .HasColumnType("int");

                    b.HasKey("GarisId");

                    b.HasIndex("AkhirId");

                    b.HasIndex("AwalId", "AkhirId")
                        .IsUnique()
                        .HasFilter("[AwalId] IS NOT NULL AND [AkhirId] IS NOT NULL");

                    b.ToTable("TblGaris");
                });

            modelBuilder.Entity("RAB.Models.Utama.KomponenGaris", b =>
                {
                    b.Property<int>("KompGrsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GarisId")
                        .HasColumnType("int");

                    b.Property<int>("KompId")
                        .HasColumnType("int");

                    b.Property<int>("OffsetKeKanan")
                        .HasColumnType("int");

                    b.Property<int>("PosRelX")
                        .HasColumnType("int");

                    b.Property<int>("PosRelY")
                        .HasColumnType("int");

                    b.Property<int>("PosRelatif")
                        .HasColumnType("int");

                    b.Property<int>("PosRelatifDiZ")
                        .HasColumnType("int");

                    b.Property<int>("PosZId")
                        .HasColumnType("int");

                    b.Property<int?>("TitikZTtkId")
                        .HasColumnType("int");

                    b.HasKey("KompGrsId");

                    b.HasIndex("KompId");

                    b.HasIndex("TitikZTtkId");

                    b.HasIndex("GarisId", "KompId")
                        .IsUnique();

                    b.ToTable("TblKomponenGaris");
                });

            modelBuilder.Entity("RAB.Models.Utama.KomponenPola", b =>
                {
                    b.Property<int>("KomPolId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("KompId")
                        .HasColumnType("int");

                    b.Property<int?>("PolaId")
                        .HasColumnType("int");

                    b.HasKey("KomPolId");

                    b.HasIndex("KompId");

                    b.HasIndex("PolaId", "KompId")
                        .IsUnique()
                        .HasFilter("[PolaId] IS NOT NULL AND [KompId] IS NOT NULL");

                    b.ToTable("TblKomponenPola");
                });

            modelBuilder.Entity("RAB.Models.Utama.Koordinat", b =>
                {
                    b.Property<int>("KoordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("KiriBawahSemu")
                        .HasColumnType("bit");

                    b.Property<bool>("LengkungAtas")
                        .HasColumnType("bit");

                    b.Property<bool>("LengkungBawah")
                        .HasColumnType("bit");

                    b.Property<bool>("MiringAtas")
                        .HasColumnType("bit");

                    b.Property<bool>("MiringBawah")
                        .HasColumnType("bit");

                    b.Property<bool>("SbgAkhirX")
                        .HasColumnType("bit");

                    b.Property<bool>("SbgAkhirY")
                        .HasColumnType("bit");

                    b.Property<bool>("SbgAwalX")
                        .HasColumnType("bit");

                    b.Property<bool>("SbgAwalY")
                        .HasColumnType("bit");

                    b.Property<bool>("TidakKeX")
                        .HasColumnType("bit");

                    b.Property<bool>("TidakKeY")
                        .HasColumnType("bit");

                    b.Property<int?>("Xid")
                        .HasColumnType("int");

                    b.Property<int?>("Yid")
                        .HasColumnType("int");

                    b.HasKey("KoordId");

                    b.HasIndex("Yid");

                    b.HasIndex("Xid", "Yid")
                        .IsUnique()
                        .HasFilter("[Xid] IS NOT NULL AND [Yid] IS NOT NULL");

                    b.ToTable("TblKoordinat");
                });

            modelBuilder.Entity("RAB.Models.Utama.Pola", b =>
                {
                    b.Property<int>("PolaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nama")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<int>("SatuanPenyusun")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("PolaId");

                    b.HasIndex("Nama")
                        .IsUnique();

                    b.ToTable("TblPola");
                });

            modelBuilder.Entity("RAB.Models.Utama.Titik", b =>
                {
                    b.Property<int>("TtkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Jarak")
                        .HasColumnType("int");

                    b.Property<string>("Nama")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("PolaId")
                        .HasColumnType("int");

                    b.Property<int>("PosAbs")
                        .HasColumnType("int");

                    b.Property<int>("PosRel")
                        .HasColumnType("int");

                    b.Property<int>("Sumbu")
                        .HasColumnType("int");

                    b.Property<int>("SumbuId")
                        .HasColumnType("int");

                    b.HasKey("TtkId");

                    b.HasIndex("PolaId", "Sumbu", "SumbuId")
                        .IsUnique();

                    b.ToTable("TblTitik");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RAB.Models.Utama.Garis", b =>
                {
                    b.HasOne("RAB.Models.Utama.Koordinat", "KoordAkhir")
                        .WithMany("TblGarisAkhir")
                        .HasForeignKey("AkhirId");

                    b.HasOne("RAB.Models.Utama.Koordinat", "KoordAwal")
                        .WithMany("TblGarisAwal")
                        .HasForeignKey("AwalId");
                });

            modelBuilder.Entity("RAB.Models.Utama.KomponenGaris", b =>
                {
                    b.HasOne("RAB.Models.Utama.Garis", "Garis")
                        .WithMany("TblKompGaris")
                        .HasForeignKey("GarisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RAB.Models.Utama.KomponenPola", "Komponen")
                        .WithMany("TblKompGaris")
                        .HasForeignKey("KompId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RAB.Models.Utama.Titik", "TitikZ")
                        .WithMany()
                        .HasForeignKey("TitikZTtkId");
                });

            modelBuilder.Entity("RAB.Models.Utama.KomponenPola", b =>
                {
                    b.HasOne("RAB.Models.Utama.Pola", "Komponen")
                        .WithMany("KompDiTblPola")
                        .HasForeignKey("KompId");

                    b.HasOne("RAB.Models.Utama.Pola", "Pola")
                        .WithMany("TblKompPola")
                        .HasForeignKey("PolaId");
                });

            modelBuilder.Entity("RAB.Models.Utama.Koordinat", b =>
                {
                    b.HasOne("RAB.Models.Utama.Titik", "TitikX")
                        .WithMany("TblKoordX")
                        .HasForeignKey("Xid");

                    b.HasOne("RAB.Models.Utama.Titik", "TitikY")
                        .WithMany("TblKoordY")
                        .HasForeignKey("Yid");
                });

            modelBuilder.Entity("RAB.Models.Utama.Titik", b =>
                {
                    b.HasOne("RAB.Models.Utama.Pola", "Pola")
                        .WithMany("TblTitik")
                        .HasForeignKey("PolaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
