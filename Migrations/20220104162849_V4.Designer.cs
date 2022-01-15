﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;

namespace WEB_projekat.Migrations
{
    [DbContext(typeof(BibliotekaContext))]
    [Migration("20220104162849_V4")]
    partial class V4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13");

            modelBuilder.Entity("BibliotekaKorisnik", b =>
                {
                    b.Property<int>("BibliotekeID")
                        .HasColumnType("int");

                    b.Property<int>("KorisniciID")
                        .HasColumnType("int");

                    b.HasKey("BibliotekeID", "KorisniciID");

                    b.HasIndex("KorisniciID");

                    b.ToTable("BibliotekaKorisnik");
                });

            modelBuilder.Entity("Models.Biblioteka", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Adresa")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Kontakt")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Biblioteke");
                });

            modelBuilder.Entity("Models.Iznajmljivanje", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("BibliotekaID")
                        .HasColumnType("int");

                    b.Property<int>("KnjigaID")
                        .HasColumnType("int");

                    b.Property<bool>("KnjigaVracena")
                        .HasColumnType("bit");

                    b.Property<int>("KorisnikID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("BibliotekaID");

                    b.HasIndex("KnjigaID");

                    b.HasIndex("KorisnikID");

                    b.ToTable("Iznajmljivanja");
                });

            modelBuilder.Entity("Models.Knjiga", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Autor")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("BrojOcenjivanja")
                        .HasColumnType("int");

                    b.Property<string>("Naslov")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Ocena")
                        .HasColumnType("float");

                    b.HasKey("ID");

                    b.ToTable("Knjige");
                });

            modelBuilder.Entity("Models.KnjigaBiblioteka", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("BibliotekaID")
                        .HasColumnType("int");

                    b.Property<int>("KnjigaID")
                        .HasColumnType("int");

                    b.Property<int>("Kolicina")
                        .HasColumnType("int");

                    b.Property<int>("Preostalo")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("BibliotekaID");

                    b.HasIndex("KnjigaID");

                    b.ToTable("KnjigeBiblioteke");
                });

            modelBuilder.Entity("Models.Korisnik", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("JMBG")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("Prezime")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("ID");

                    b.ToTable("Korisnici");
                });

            modelBuilder.Entity("BibliotekaKorisnik", b =>
                {
                    b.HasOne("Models.Biblioteka", null)
                        .WithMany()
                        .HasForeignKey("BibliotekeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Korisnik", null)
                        .WithMany()
                        .HasForeignKey("KorisniciID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Iznajmljivanje", b =>
                {
                    b.HasOne("Models.Biblioteka", "Biblioteka")
                        .WithMany("Iznajmljivanja")
                        .HasForeignKey("BibliotekaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Knjiga", "Knjiga")
                        .WithMany("Iznajmljivanja")
                        .HasForeignKey("KnjigaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Korisnik", "Korisnik")
                        .WithMany("Iznajmljivanja")
                        .HasForeignKey("KorisnikID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Biblioteka");

                    b.Navigation("Knjiga");

                    b.Navigation("Korisnik");
                });

            modelBuilder.Entity("Models.KnjigaBiblioteka", b =>
                {
                    b.HasOne("Models.Biblioteka", "Biblioteka")
                        .WithMany("Knjige")
                        .HasForeignKey("BibliotekaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Knjiga", "Knjiga")
                        .WithMany("Biblioteke")
                        .HasForeignKey("KnjigaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Biblioteka");

                    b.Navigation("Knjiga");
                });

            modelBuilder.Entity("Models.Biblioteka", b =>
                {
                    b.Navigation("Iznajmljivanja");

                    b.Navigation("Knjige");
                });

            modelBuilder.Entity("Models.Knjiga", b =>
                {
                    b.Navigation("Biblioteke");

                    b.Navigation("Iznajmljivanja");
                });

            modelBuilder.Entity("Models.Korisnik", b =>
                {
                    b.Navigation("Iznajmljivanja");
                });
#pragma warning restore 612, 618
        }
    }
}
