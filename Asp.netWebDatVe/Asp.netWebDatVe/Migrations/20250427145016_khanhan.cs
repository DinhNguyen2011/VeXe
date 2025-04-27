using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asp.netWebDatVe.Migrations
{
    public partial class khanhan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BenXe",
                columns: table => new
                {
                    MaBenXe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenBenXe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BenXe__436ED7BADC83220E", x => x.MaBenXe);
                });

            migrationBuilder.CreateTable(
                name: "BenXeDen",
                columns: table => new
                {
                    MaBenXeDen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenBenXeDen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BenXeDen__FA9521107644CDCA", x => x.MaBenXeDen);
                });

            migrationBuilder.CreateTable(
                name: "loaixe",
                columns: table => new
                {
                    ID_LOAI = table.Column<int>(type: "int", nullable: false),
                    TENLOAI = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SOGHE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__loaixe__994CB9EAC7B987C6", x => x.ID_LOAI);
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
                name: "PhieuDatVe",
                columns: table => new
                {
                    MaPhieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgayDat = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuDat__2660BFE00BC5E6E3", x => x.MaPhieu);
                });

            migrationBuilder.CreateTable(
                name: "TuyenXe",
                columns: table => new
                {
                    MaTuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiemDi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiemDen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoNgayChayTrongTuan = table.Column<int>(type: "int", nullable: true),
                    GiaHienHanh = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QuangDuong = table.Column<int>(type: "int", nullable: true),
                    MaBenXe = table.Column<int>(type: "int", nullable: true),
                    MaBenXeDen = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TuyenXe__B45760204FE10EA9", x => x.MaTuyen);
                    table.ForeignKey(
                        name: "FK_TuyenXe_BenXe",
                        column: x => x.MaBenXe,
                        principalTable: "BenXe",
                        principalColumn: "MaBenXe");
                    table.ForeignKey(
                        name: "FK_TuyenXe_BenXeDen",
                        column: x => x.MaBenXeDen,
                        principalTable: "BenXeDen",
                        principalColumn: "MaBenXeDen");
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
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "date", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaQuyen = table.Column<int>(type: "int", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
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
                name: "ChuyenXe",
                columns: table => new
                {
                    MaChuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTuyen = table.Column<int>(type: "int", nullable: true),
                    ThoiDiemKhoiHanh = table.Column<DateTime>(type: "datetime", nullable: true),
                    ThoiDiemDenDuKien = table.Column<DateTime>(type: "datetime", nullable: true),
                    GiaVe = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BienSoXe = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    TenChuyenXe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChuyenXe__0ED32A472A7B00B1", x => x.MaChuyen);
                    table.ForeignKey(
                        name: "FK__ChuyenXe__MaTuye__3C69FB99",
                        column: x => x.MaTuyen,
                        principalTable: "TuyenXe",
                        principalColumn: "MaTuyen");
                    table.ForeignKey(
                        name: "FK_ChuyenXe_BienSoXe",
                        column: x => x.BienSoXe,
                        principalTable: "xe",
                        principalColumn: "BIENSO");
                });

            migrationBuilder.CreateTable(
                name: "vitrighe",
                columns: table => new
                {
                    ID_VITRI = table.Column<int>(type: "int", nullable: false),
                    BIENSO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TENVITRI = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TRANGTHAI = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__vitrighe__121D67D0DAE6F32D", x => x.ID_VITRI);
                    table.ForeignKey(
                        name: "FK__vitrighe__BIENSO__45F365D3",
                        column: x => x.BIENSO,
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
                    ID_VITRI = table.Column<int>(type: "int", nullable: true)
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
                        column: x => x.ID_VITRI,
                        principalTable: "vitrighe",
                        principalColumn: "ID_VITRI");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenXe_BienSoXe",
                table: "ChuyenXe",
                column: "BienSoXe");

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
                name: "IX_TuyenXe_MaBenXe",
                table: "TuyenXe",
                column: "MaBenXe");

            migrationBuilder.CreateIndex(
                name: "IX_TuyenXe_MaBenXeDen",
                table: "TuyenXe",
                column: "MaBenXeDen");

            migrationBuilder.CreateIndex(
                name: "IX_VeXe_ID_VITRI",
                table: "VeXe",
                column: "ID_VITRI");

            migrationBuilder.CreateIndex(
                name: "IX_VeXe_MaChuyen",
                table: "VeXe",
                column: "MaChuyen");

            migrationBuilder.CreateIndex(
                name: "IX_VeXe_MaPhieu",
                table: "VeXe",
                column: "MaPhieu");

            migrationBuilder.CreateIndex(
                name: "IX_vitrighe_BIENSO",
                table: "vitrighe",
                column: "BIENSO");

            migrationBuilder.CreateIndex(
                name: "IX_xe_ID_LOAI",
                table: "xe",
                column: "ID_LOAI");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "VeXe");

            migrationBuilder.DropTable(
                name: "PhanQuyen");

            migrationBuilder.DropTable(
                name: "ChuyenXe");

            migrationBuilder.DropTable(
                name: "PhieuDatVe");

            migrationBuilder.DropTable(
                name: "vitrighe");

            migrationBuilder.DropTable(
                name: "TuyenXe");

            migrationBuilder.DropTable(
                name: "xe");

            migrationBuilder.DropTable(
                name: "BenXe");

            migrationBuilder.DropTable(
                name: "BenXeDen");

            migrationBuilder.DropTable(
                name: "loaixe");
        }
    }
}
