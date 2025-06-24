using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asp.netWebDatVe.Migrations
{
    public partial class KhanhAn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BenXe",
                columns: table => new
                {
                    MaBenXe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenBenXe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sdt = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ThanhPho = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BenXe__436ED7BA873F7318", x => x.MaBenXe);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMai",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhanTramGiam = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "date", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "date", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KhuyenMa__6F56B3BD77E05726", x => x.MaKhuyenMai);
                });

            migrationBuilder.CreateTable(
                name: "LienHe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoVaTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayGui = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Sdt = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LienHe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "loaixe",
                columns: table => new
                {
                    ID_LOAI = table.Column<int>(type: "int", nullable: false),
                    TENLOAI = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SOGHE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__loaixe__994CB9EAC7B987C6", x => x.ID_LOAI);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    MaNhanVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sdt = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValueSql: "(N'Nhân Viên')"),
                    Cccd = table.Column<long>(type: "bigint", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhanVien__77B2CA47CD53549F", x => x.MaNhanVien);
                });

            migrationBuilder.CreateTable(
                name: "PhanQuyen",
                columns: table => new
                {
                    MaQuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenQuyen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhanQuye__1D4B7ED45F686F58", x => x.MaQuyen);
                });

            migrationBuilder.CreateTable(
                name: "TuyenXe",
                columns: table => new
                {
                    MaTuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiemDi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiemDen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoNgayChayTrongTuan = table.Column<int>(type: "int", nullable: false),
                    GiaHienHanh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuangDuong = table.Column<int>(type: "int", nullable: false),
                    MaBenXeDi = table.Column<int>(type: "int", nullable: false),
                    MaBenXeDen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TuyenXe__B45760204FE10EA9", x => x.MaTuyen);
                    table.ForeignKey(
                        name: "fk_MaBenXeDen",
                        column: x => x.MaBenXeDen,
                        principalTable: "BenXe",
                        principalColumn: "MaBenXe");
                    table.ForeignKey(
                        name: "fk_MaBenXeDi",
                        column: x => x.MaBenXeDi,
                        principalTable: "BenXe",
                        principalColumn: "MaBenXe");
                });

            migrationBuilder.CreateTable(
                name: "PhieuDatVe",
                columns: table => new
                {
                    MaPhieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgayDat = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VnpTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaKhuyenMai = table.Column<int>(type: "int", nullable: true),
                    MoMoTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuDat__2660BFE00BC5E6E3", x => x.MaPhieu);
                    table.ForeignKey(
                        name: "FK_PhieuDatVe_KhuyenMai",
                        column: x => x.MaKhuyenMai,
                        principalTable: "KhuyenMai",
                        principalColumn: "MaKhuyenMai");
                });

            migrationBuilder.CreateTable(
                name: "xe",
                columns: table => new
                {
                    BIENSO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ID_LOAI = table.Column<int>(type: "int", nullable: false),
                    TENXE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__xe__2B719FCC31E3F52F", x => x.BIENSO);
                    table.ForeignKey(
                        name: "FK__xe__ID_LOAI__4316F928",
                        column: x => x.ID_LOAI,
                        principalTable: "loaixe",
                        principalColumn: "ID_LOAI");
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "date", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaQuyen = table.Column<int>(type: "int", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChuThich = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.Id);
                    table.ForeignKey(
                        name: "FK__NguoiDung__MaQuy__656C112C",
                        column: x => x.MaQuyen,
                        principalTable: "PhanQuyen",
                        principalColumn: "MaQuyen");
                });

            migrationBuilder.CreateTable(
                name: "ThanhToan",
                columns: table => new
                {
                    MaThanhToan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhieu = table.Column<int>(type: "int", nullable: false),
                    PhuongThuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    MaGiaoDich = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ThanhToa__D4B25844477DF218", x => x.MaThanhToan);
                    table.ForeignKey(
                        name: "FK__ThanhToan__MaPhi__7D0E9093",
                        column: x => x.MaPhieu,
                        principalTable: "PhieuDatVe",
                        principalColumn: "MaPhieu");
                });

            migrationBuilder.CreateTable(
                name: "ChuyenXe",
                columns: table => new
                {
                    MaChuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTuyen = table.Column<int>(type: "int", nullable: false),
                    ThoiDiemKhoiHanh = table.Column<DateTime>(type: "datetime", nullable: false),
                    ThoiDiemDenDuKien = table.Column<DateTime>(type: "datetime", nullable: false),
                    GiaVe = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BienSoXe = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TenChuyenXe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNhanVien = table.Column<int>(type: "int", nullable: true),
                    MaTaiXe = table.Column<int>(type: "int", nullable: true),
                    MaNhanVien1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChuyenXe__0ED32A472A7B00B1", x => x.MaChuyen);
                    table.ForeignKey(
                        name: "FK__ChuyenXe__MaTuye__3C69FB99",
                        column: x => x.MaTuyen,
                        principalTable: "TuyenXe",
                        principalColumn: "MaTuyen",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChuyenXe_BienSoXe",
                        column: x => x.BienSoXe,
                        principalTable: "xe",
                        principalColumn: "BIENSO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChuyenXe_NhanVien_NhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNhanVien");
                    table.ForeignKey(
                        name: "FK_ChuyenXe_NhanVien_NhanVien1",
                        column: x => x.MaNhanVien1,
                        principalTable: "NhanVien",
                        principalColumn: "MaNhanVien");
                    table.ForeignKey(
                        name: "FK_ChuyenXe_NhanVien_TaiXe",
                        column: x => x.MaTaiXe,
                        principalTable: "NhanVien",
                        principalColumn: "MaNhanVien");
                });

            migrationBuilder.CreateTable(
                name: "Vitrighe",
                columns: table => new
                {
                    IdVitri = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bienso = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Tenvitri = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Trangthai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Vitrighe__5965B3AB6EFE223C", x => x.IdVitri);
                    table.ForeignKey(
                        name: "FK_Vitrighe_Xe",
                        column: x => x.Bienso,
                        principalTable: "xe",
                        principalColumn: "BIENSO");
                });

            migrationBuilder.CreateTable(
                name: "VeXe",
                columns: table => new
                {
                    MaVe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhieu = table.Column<int>(type: "int", nullable: true),
                    MaChuyen = table.Column<int>(type: "int", nullable: true),
                    TenVe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TenKH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayDat = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    SĐT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdVitri = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VeXe__2725100FC96E40BB", x => x.MaVe);
                    table.ForeignKey(
                        name: "FK__VeXe__MaChuyen__60A75C0F",
                        column: x => x.MaChuyen,
                        principalTable: "ChuyenXe",
                        principalColumn: "MaChuyen");
                    table.ForeignKey(
                        name: "FK__VeXe__MaPhieu__5FB337D6",
                        column: x => x.MaPhieu,
                        principalTable: "PhieuDatVe",
                        principalColumn: "MaPhieu");
                    table.ForeignKey(
                        name: "FK_VeXe_Vitrighe",
                        column: x => x.IdVitri,
                        principalTable: "Vitrighe",
                        principalColumn: "IdVitri");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenXe_BienSoXe",
                table: "ChuyenXe",
                column: "BienSoXe");

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenXe_MaNhanVien",
                table: "ChuyenXe",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenXe_MaNhanVien1",
                table: "ChuyenXe",
                column: "MaNhanVien1");

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenXe_MaTaiXe",
                table: "ChuyenXe",
                column: "MaTaiXe");

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenXe_MaTuyen",
                table: "ChuyenXe",
                column: "MaTuyen");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_MaQuyen",
                table: "NguoiDung",
                column: "MaQuyen");

            migrationBuilder.CreateIndex(
                name: "UQ__NguoiDun__A9D10534EAA83623",
                table: "NguoiDung",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDatVe_MaKhuyenMai",
                table: "PhieuDatVe",
                column: "MaKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_MaPhieu",
                table: "ThanhToan",
                column: "MaPhieu");

            migrationBuilder.CreateIndex(
                name: "IX_TuyenXe_MaBenXeDen",
                table: "TuyenXe",
                column: "MaBenXeDen");

            migrationBuilder.CreateIndex(
                name: "IX_TuyenXe_MaBenXeDi",
                table: "TuyenXe",
                column: "MaBenXeDi");

            migrationBuilder.CreateIndex(
                name: "IX_VeXe_IdVitri",
                table: "VeXe",
                column: "IdVitri");

            migrationBuilder.CreateIndex(
                name: "IX_VeXe_MaChuyen",
                table: "VeXe",
                column: "MaChuyen");

            migrationBuilder.CreateIndex(
                name: "IX_VeXe_MaPhieu",
                table: "VeXe",
                column: "MaPhieu");

            migrationBuilder.CreateIndex(
                name: "IX_Vitrighe_Bienso",
                table: "Vitrighe",
                column: "Bienso");

            migrationBuilder.CreateIndex(
                name: "IX_xe_ID_LOAI",
                table: "xe",
                column: "ID_LOAI");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LienHe");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropTable(
                name: "VeXe");

            migrationBuilder.DropTable(
                name: "PhanQuyen");

            migrationBuilder.DropTable(
                name: "ChuyenXe");

            migrationBuilder.DropTable(
                name: "PhieuDatVe");

            migrationBuilder.DropTable(
                name: "Vitrighe");

            migrationBuilder.DropTable(
                name: "TuyenXe");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "xe");

            migrationBuilder.DropTable(
                name: "BenXe");

            migrationBuilder.DropTable(
                name: "loaixe");
        }
    }
}
