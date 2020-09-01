using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LABA1
{
    public partial class MyDBContext : DbContext
    {
        public MyDBContext()
        {
        }

        public MyDBContext(DbContextOptions<MyDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bodies> Bodies { get; set; }
        public virtual DbSet<Cars> Cars { get; set; }
        public virtual DbSet<Colors> Colors { get; set; }
        public virtual DbSet<Contracts> Contracts { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Labels> Labels { get; set; }
        public virtual DbSet<Landlords> Landlords { get; set; }
        public virtual DbSet<Renters> Renters { get; set; }
        public virtual DbSet<Transmissions> Transmissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=WIN-H1JKN582NVP\\SQLEXPRESS; Database=MyDB; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bodies>(entity =>
            {
                entity.HasKey(e => e.BodyId)
                    .HasName("PK_Body");

                entity.Property(e => e.Body)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Cars>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Body)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.BodyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cars_Body");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cars_Colors");

                entity.HasOne(d => d.Label)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.LabelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cars_Labels");

                entity.HasOne(d => d.Landlord)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.LandlordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cars_Landlords");

                entity.HasOne(d => d.Transmission)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.TransmissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cars_Transmissions");
            });

            modelBuilder.Entity<Colors>(entity =>
            {
                entity.HasKey(e => e.ColorId);

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Contracts>(entity =>
            {
                entity.Property(e => e.DayPrice).HasColumnType("money");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contracts_Cars1");

                entity.HasOne(d => d.Renter)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.RenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contracts_Renters");
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasKey(e => e.CountryId)
                    .HasName("PK_Country");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Labels>(entity =>
            {
                entity.HasKey(e => e.LableId);

                entity.Property(e => e.Lable)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Landlords>(entity =>
            {
                entity.Property(e => e.ContartPerson)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Landlords)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Landlords_Country");
            });

            modelBuilder.Entity<Renters>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DriveExperience)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Passport)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Transmissions>(entity =>
            {
                entity.HasKey(e => e.TransmissionId);

                entity.Property(e => e.Trasmission)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
