using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Asp.netWebDatVe.Models
{
    public partial class QLDatVeContext : DbContext
    {
        public QLDatVeContext()
        {
        }

        public QLDatVeContext(DbContextOptions<QLDatVeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BenXe> BenXes { get; set; } = null!;
        public virtual DbSet<BenXeDen> BenXeDens { get; set; } = null!;
        public virtual DbSet<ChuyenXe> ChuyenXes { get; set; } = null!;
        public virtual DbSet<Loaixe> Loaixes { get; set; } = null!;
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; } = null!;
        public virtual DbSet<PhanQuyen> PhanQuyens { get; set; } = null!;
        public virtual DbSet<PhieuDatVe> PhieuDatVes { get; set; } = null!;
        public virtual DbSet<TuyenXe> TuyenXes { get; set; } = null!;
        public virtual DbSet<VeXe> VeXes { get; set; } = null!;
        public virtual DbSet<Vitrighe> Vitrighes { get; set; } = null!;
        public virtual DbSet<Xe> Xes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DINHNGUYEN;Database=QLDatVe;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BenXe>(entity =>
            {
                entity.HasKey(e => e.MaBenXe)
                    .HasName("PK__BenXe__436ED7BADC83220E");

                entity.ToTable("BenXe");

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(20)
                    .HasColumnName("SDT");

                entity.Property(e => e.TenBenXe).HasMaxLength(100);
            });

            modelBuilder.Entity<BenXeDen>(entity =>
            {
                entity.HasKey(e => e.MaBenXeDen)
                    .HasName("PK__BenXeDen__FA9521107644CDCA");

                entity.ToTable("BenXeDen");

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(20)
                    .HasColumnName("SDT");

                entity.Property(e => e.TenBenXeDen).HasMaxLength(100);
            });

            modelBuilder.Entity<ChuyenXe>(entity =>
            {
                entity.HasKey(e => e.MaChuyen)
                    .HasName("PK__ChuyenXe__0ED32A472A7B00B1");

                entity.ToTable("ChuyenXe");

                entity.Property(e => e.BienSoXe).HasMaxLength(15);

                entity.Property(e => e.GiaVe).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TenChuyenXe).HasMaxLength(100);

                entity.Property(e => e.ThoiDiemDenDuKien).HasColumnType("datetime");

                entity.Property(e => e.ThoiDiemKhoiHanh).HasColumnType("datetime");

                entity.HasOne(d => d.BienSoXeNavigation)
                    .WithMany(p => p.ChuyenXes)
                    .HasForeignKey(d => d.BienSoXe)
                    .HasConstraintName("FK_ChuyenXe_BienSoXe");

                entity.HasOne(d => d.MaTuyenNavigation)
                    .WithMany(p => p.ChuyenXes)
                    .HasForeignKey(d => d.MaTuyen)
                    .HasConstraintName("FK__ChuyenXe__MaTuye__3C69FB99");
            });

            modelBuilder.Entity<Loaixe>(entity =>
            {
                entity.HasKey(e => e.IdLoai)
                    .HasName("PK__loaixe__994CB9EAC7B987C6");

                entity.ToTable("loaixe");

                entity.Property(e => e.IdLoai)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_LOAI");

                entity.Property(e => e.Soghe).HasColumnName("SOGHE");

                entity.Property(e => e.Tenloai)
                    .HasMaxLength(50)
                    .HasColumnName("TENLOAI");
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.ToTable("NguoiDung");

                entity.HasIndex(e => e.Email, "UQ__NguoiDun__A9D10534EAA83623")
                    .IsUnique();

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.HinhAnh).HasMaxLength(50);

                entity.Property(e => e.HoTen).HasMaxLength(100);

                entity.Property(e => e.MatKhau).HasMaxLength(255);

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.Sdt)
                    .HasMaxLength(20)
                    .HasColumnName("SDT");

                entity.HasOne(d => d.MaQuyenNavigation)
                    .WithMany(p => p.NguoiDungs)
                    .HasForeignKey(d => d.MaQuyen)
                    .HasConstraintName("FK__NguoiDung__MaQuy__656C112C");
            });

            modelBuilder.Entity<PhanQuyen>(entity =>
            {
                entity.HasKey(e => e.MaQuyen)
                    .HasName("PK__PhanQuye__1D4B7ED45F686F58");

                entity.ToTable("PhanQuyen");

                entity.Property(e => e.TenQuyen).HasMaxLength(50);
            });

            modelBuilder.Entity<PhieuDatVe>(entity =>
            {
                entity.HasKey(e => e.MaPhieu)
                    .HasName("PK__PhieuDat__2660BFE00BC5E6E3");

                entity.ToTable("PhieuDatVe");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.NgayDat)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TrangThai).HasMaxLength(50);
            });

            modelBuilder.Entity<TuyenXe>(entity =>
            {
                entity.HasKey(e => e.MaTuyen)
                    .HasName("PK__TuyenXe__B45760204FE10EA9");

                entity.ToTable("TuyenXe");

                entity.Property(e => e.DiemDen).HasMaxLength(100);

                entity.Property(e => e.DiemDi).HasMaxLength(100);

                entity.Property(e => e.GiaHienHanh).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.MaBenXeNavigation)
                    .WithMany(p => p.TuyenXes)
                    .HasForeignKey(d => d.MaBenXe)
                    .HasConstraintName("FK_TuyenXe_BenXe");

                entity.HasOne(d => d.MaBenXeDenNavigation)
                    .WithMany(p => p.TuyenXes)
                    .HasForeignKey(d => d.MaBenXeDen)
                    .HasConstraintName("FK_TuyenXe_BenXeDen");
            });

            modelBuilder.Entity<VeXe>(entity =>
            {
                entity.HasKey(e => e.MaVe)
                    .HasName("PK__VeXe__2725100FC96E40BB");

                entity.ToTable("VeXe");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.GhiChu).HasMaxLength(200);

                entity.Property(e => e.IdVitri).HasColumnName("ID_VITRI");

                entity.Property(e => e.TenKh)
                    .HasMaxLength(50)
                    .HasColumnName("TenKH");

                entity.Property(e => e.TenVe).HasMaxLength(50);

                entity.Property(e => e.TrangThai).HasMaxLength(50);

                entity.HasOne(d => d.IdVitriNavigation)
                    .WithMany(p => p.VeXes)
                    .HasForeignKey(d => d.IdVitri)
                    .HasConstraintName("FK_VeXe_Vitrighe");

                entity.HasOne(d => d.MaChuyenNavigation)
                    .WithMany(p => p.VeXes)
                    .HasForeignKey(d => d.MaChuyen)
                    .HasConstraintName("FK__VeXe__MaChuyen__60A75C0F");

                entity.HasOne(d => d.MaPhieuNavigation)
                    .WithMany(p => p.VeXes)
                    .HasForeignKey(d => d.MaPhieu)
                    .HasConstraintName("FK__VeXe__MaPhieu__5FB337D6");
            });

            modelBuilder.Entity<Vitrighe>(entity =>
            {
                entity.HasKey(e => e.IdVitri)
                    .HasName("PK__vitrighe__121D67D0DAE6F32D");

                entity.ToTable("vitrighe");

                entity.Property(e => e.IdVitri)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_VITRI");

                entity.Property(e => e.Bienso)
                    .HasMaxLength(15)
                    .HasColumnName("BIENSO");

                entity.Property(e => e.Tenvitri)
                    .HasMaxLength(50)
                    .HasColumnName("TENVITRI");

                entity.Property(e => e.Trangthai).HasColumnName("TRANGTHAI");

                entity.HasOne(d => d.BiensoNavigation)
                    .WithMany(p => p.Vitrighes)
                    .HasForeignKey(d => d.Bienso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__vitrighe__BIENSO__45F365D3");
            });

            modelBuilder.Entity<Xe>(entity =>
            {
                entity.HasKey(e => e.Bienso)
                    .HasName("PK__xe__2B719FCC31E3F52F");

                entity.ToTable("xe");

                entity.Property(e => e.Bienso)
                    .HasMaxLength(15)
                    .HasColumnName("BIENSO");

                entity.Property(e => e.HinhAnh).HasMaxLength(50);

                entity.Property(e => e.IdLoai).HasColumnName("ID_LOAI");

                entity.Property(e => e.Tenxe)
                    .HasMaxLength(50)
                    .HasColumnName("TENXE");

                entity.HasOne(d => d.IdLoaiNavigation)
                    .WithMany(p => p.Xes)
                    .HasForeignKey(d => d.IdLoai)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__xe__ID_LOAI__4316F928");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
