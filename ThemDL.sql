
-- Lệnh thêm bến xe
SET IDENTITY_INSERT BenXe ON;
INSERT INTO BenXe (MaBenXe, TenBenXe, DiaChi, Sdt, ThanhPho) VALUES
(1, N'Bến xe Miền Đông', N'292 Đinh Bộ Lĩnh, Quận Bình Thạnh', N'0901234567', N'TP. Hồ Chí Minh'),
(2, N'Bến xe Đà Lạt', N'1 Tô Hiến Thành, Phường 3', N'0912345678', N'Đà Lạt'),
(3, N'Bến xe Trung tâm Đà Nẵng', N'201 Tôn Đức Thắng, Quận Liên Chiểu', N'0923456789', N'Đà Nẵng'),
(4, N'Bến xe Nha Trang', N'1 Yersin, Phường Vạn Thắng', N'0934567890', N'Nha Trang'),
(5, N'Bến xe Huế', N'97 An Dương Vương, Phường An Đông', N'0945678901', N'Huế'),
(6, N'Bến xe Giáp Bát', N'Km6 Đường Giải Phóng, Quận Hoàng Mai', N'0956789012', N'Hà Nội'),
(7, N'Bến xe Đồng Hới', N'Đường Trần Hưng Đạo, TP. Đồng Hới', N'0967890123', N'Quảng Bình'),
(8, N'Bến xe phía Nam Khánh Hòa', N'Đường 23/10, TP. Cam Ranh', N'0978901234', N'Khánh Hòa'),
(9, N'Bến xe Nước Ngầm', N'1 Ngọc Hồi, Quận Hoàng Mai', N'0989012345', N'Hà Nội'),
(10, N'Bến xe Mỹ Đình', N'20 Phạm Hùng, Quận Nam Từ Liêm', N'0990123456', N'Hà Nội');

SET IDENTITY_INSERT BenXe OFF;
UPDATE BenXe
SET ThanhPho = N'Thừa Thiên Huế'
WHERE MaBenXe = 5;
SELECT * FROM BenXe WHERE MaBenXe = 5;


-- Lệnh thêm tuyến xe 
SET IDENTITY_INSERT TuyenXe ON;

INSERT INTO TuyenXe (MaTuyen, DiemDi, DiemDen, SoNgayChayTrongTuan, GiaHienHanh, QuangDuong, MaBenXeDi, MaBenXeDen) VALUES
(1, N'TP. Hồ Chí Minh', N'Đà Lạt', 5, 290000, 305, 1, 2), -- HCM → Đà Lạt
(2, N'Đà Lạt', N'TP. Hồ Chí Minh', 5, 290000, 305, 2, 1), -- Đà Lạt → HCM
(3, N'TP. Hồ Chí Minh', N'Đà Nẵng', 4, 750000, 850, 1, 3), -- HCM → Đà Nẵng
(4, N'Đà Nẵng', N'TP. Hồ Chí Minh', 4, 750000, 850, 3, 1), -- Đà Nẵng → HCM
(5, N'TP. Hồ Chí Minh', N'Nha Trang', 6, 220000, 430, 1, 4), -- HCM → Nha Trang
(6, N'Nha Trang', N'TP. Hồ Chí Minh', 6, 220000, 430, 4, 1), -- Nha Trang → HCM
(7, N'TP. Hồ Chí Minh', N'Huế', 3, 800000, 1000, 1, 5), -- HCM → Huế
(8, N'Huế', N'TP. Hồ Chí Minh', 3, 800000, 1000, 5, 1), -- Huế → HCM
(9, N'TP. Hồ Chí Minh', N'Hà Nội', 7, 1200000, 1700, 1, 6), -- HCM → Hà Nội (Giáp Bát)
(10, N'Hà Nội', N'TP. Hồ Chí Minh', 7, 1200000, 1700, 6, 1), -- Hà Nội (Giáp Bát) → HCM
(11, N'Đà Lạt', N'Đà Nẵng', 4, 450000, 550, 2, 3), -- Đà Lạt → Đà Nẵng
(12, N'Đà Nẵng', N'Đà Lạt', 4, 450000, 550, 3, 2), -- Đà Nẵng → Đà Lạt
(13, N'Đà Nẵng', N'Hà Nội', 5, 600000, 770, 3, 9), -- Đà Nẵng → Hà Nội (Nước Ngầm)
(14, N'Hà Nội', N'Đà Nẵng', 5, 600000, 770, 9, 3), -- Hà Nội (Nước Ngầm) → Đà Nẵng
(15, N'Nha Trang', N'Huế', 3, 500000, 630, 4, 5), -- Nha Trang → Huế
(16, N'Huế', N'Nha Trang', 3, 500000, 630, 5, 4), -- Huế → Nha Trang
(17, N'Quảng Bình', N'Hà Nội', 4, 450000, 500, 7, 10), -- Quảng Bình → Hà Nội (Mỹ Đình)
(18, N'Hà Nội', N'Quảng Bình', 4, 450000, 500, 10, 7), -- Hà Nội (Mỹ Đình) → Quảng Bình
(19, N'Khánh Hòa', N'Đà Nẵng', 3, 300000, 400, 8, 3), -- Khánh Hòa → Đà Nẵng
(20, N'Đà Nẵng', N'Khánh Hòa', 3, 300000, 400, 3, 8); -- Đà Nẵng → Khánh Hòa
SET IDENTITY_INSERT TuyenXe off;
UPDATE TuyenXe
SET DiemDen = N'Thừa Thiên Huế'
WHERE MaTuyen IN (7, 15); -- Các tuyến có điểm đến là Huế

UPDATE TuyenXe
SET DiemDi = N'Thừa Thiên Huế'
WHERE MaTuyen IN (8, 16); -- Các tuyến có điểm đi là Huế


SET IDENTITY_INSERT Nhanvien ON;
-- Lệnh thêm Nhân viên
INSERT INTO NhanVien (MaNhanVien, HoTen, Sdt, DiaChi, VaiTro, Cccd) VALUES
-- 10 nhân viên cũ
(1, N'Nguyễn Văn An', '0901234567', N'123 Đường Lê Lợi, TP. Hồ Chí Minh', N'Tài xế', 123456789012),
(2, N'Trần Thị Bé', N'0912345678', N'456 Đường Nguyễn Huệ, Đà Lạt', N'Lơ xe', 234567890123),
(3, N'Lê Văn Cường', N'0923456789', N'789 Đường Tôn Đức Thắng, Đà Nẵng', N'Nhân viên hỗ trợ', 345678901234),
(4, N'Phạm Thị Duyên', N'0934567890', N'321 Đường Yersin, Nha Trang', N'Tài xế', 456789012345),
(5, N'Hoàng Văn Em', N'0945678901', N'654 Đường An Dương Vương, Thừa Thiên Huế', N'Lơ xe', 345678931234),
(6, N'Ngô Thị Hoa', N'0956789012', N'987 Đường Giải Phóng, Hà Nội', N'Nhân viên hỗ trợ', 567890123456),
(7, N'Võ Văn Khang', N'0967890123', N'147 Đường Trần Hưng Đạo, Quảng Bình', N'Tài xế', 678901234567),
(8, N'Bùi Thị Lan', N'0978901234', N'258 Đường 23/10, Khánh Hòa', N'Lơ xe', 789012345678),
(9, N'Đỗ Văn Minh', N'0989012345', N'369 Đường Ngọc Hồi, Hà Nội', N'Tài xế', 890123456789),
(10, N'Nguyễn Thị Ngọc', N'0990123456', N'741 Đường Phạm Hùng, Hà Nội', N'Nhân viên hỗ trợ', 345128901234),
-- 10 nhân viên mới
(11, N'Trương Văn Bình', N'0902345678', N'852 Đường Nguyễn Trãi, TP. Hồ Chí Minh', N'Tài xế', 901234567890),
(12, N'Phan Thị Cẩm', N'0913456789', N'963 Đường Bà Triệu, Đà Lạt', N'Lơ xe', 123456789123),
(13, N'Vũ Văn Dũng', N'0924567890', N'147 Đường Lê Đại Hành, Đà Nẵng', N'Nhân viên hỗ trợ', 234567891234),
(14, N'Lý Thị Hồng', N'0935678901', N'258 Đường Nguyễn Văn Cừ, Nha Trang', N'Tài xế', 345678912345),
(15, N'Đinh Văn Kiên', N'0946789012', N'369 Đường Lê Hồng Phong, Thừa Thiên Huế', N'Lơ xe', 3451678901234),
(16, N'Hà Thị Mai', N'0957890123', N'741 Đường Nguyễn Lương Bằng, Hà Nội', N'Nhân viên hỗ trợ', 456789123456),
(17, N'Nguyễn Văn Nam', N'0968901234', N'852 Đường Lý Thường Kiệt, Quảng Bình', N'Tài xế', 567891234567),
(18, N'Trần Thị Oanh', N'0979012345', N'963 Đường Nguyễn Tất Thành, Khánh Hòa', N'Lơ xe', 678912345678),
(19, N'Lê Văn Phong', N'0980123456', N'147 Đường Hoàng Quốc Việt, Hà Nội', N'Tài xế', 789123456789),
(20, N'Phạm Thị Quyên', N'0991234567', N'258 Đường Cầu Giấy, Hà Nội', N'Nhân viên văn phòng', 1235678901234),
(21, N'Trần Văn Hùng', N'0903456789', N'159 Đường Trần Phú, TP. Hồ Chí Minh', N'Tài xế', 123456789013);
SET IDENTITY_INSERT Nhanvien OFF;
-- Loại xe
INSERT INTO Loaixe (ID_LOAI, Tenloai, Soghe) VALUES
(1, N'Xe ghế ngồi thường', 29),    -- Phù hợp tuyến ngắn như HCM ↔ Đà Lạt
(2, N'Xe ghế ngồi cao cấp', 25),   -- Tuyến trung bình như Đà Lạt ↔ Đà Nẵng
(3, N'Xe giường nằm 40 chỗ', 40),  -- Phù hợp tuyến dài như HCM ↔ Hà Nội
(4, N'Xe giường nằm 34 chỗ', 34),  -- Tuyến dài như HCM ↔ Thừa Thiên Huế
(5, N'Xe limousine 16 chỗ', 16),   -- Tuyến ngắn, cao cấp như HCM ↔ Nha Trang
(6, N'Xe limousine 22 chỗ', 22),   -- Tuyến trung bình, cao cấp như Đà Nẵng ↔ Hà Nội
(7, N'Xe ghế ngồi 45 chỗ', 45),    -- Tuyến dài, đông khách như HCM ↔ Hà Nội
(8, N'Xe giường nằm cao cấp', 36), -- Tuyến dài như Quảng Bình ↔ Hà Nội
(9, N'Xe ghế ngồi 16 chỗ', 16),    -- Tuyến ngắn như Khánh Hòa ↔ Đà Nẵng
(10, N'Xe giường nằm đôi 20 chỗ', 20); -- Tuyến trung bình, cao cấp như Nha Trang ↔ Thừa Thiên Huế

-- Xe
INSERT INTO Xe (Bienso, ID_LOAI, Tenxe, HinhAnh) VALUES
('51B-12345', 1, N'Xe ghế ngồi 29 chỗ 001', NULL),  -- Xe ghế ngồi thường
('43A-56789', 2, N'Xe ghế ngồi cao cấp 002', NULL), -- Xe ghế ngồi cao cấp
('51H-98765', 3, N'Xe giường nằm 40 chỗ 003', NULL), -- Xe giường nằm 40 chỗ
('79B-45678', 4, N'Xe giường nằm 34 chỗ 004', NULL), -- Xe giường nằm 34 chỗ
('50K-23456', 5, N'Xe limousine 16 chỗ 005', NULL),  -- Xe limousine 16 chỗ
('36A-78901', 6, N'Xe limousine 22 chỗ 006', NULL),  -- Xe limousine 22 chỗ
('51G-34567', 7, N'Xe ghế ngồi 45 chỗ 007', NULL),  -- Xe ghế ngồi 45 chỗ
('92C-67890', 8, N'Xe giường nằm cao cấp 008', NULL), -- Xe giường nằm cao cấp
('54D-89012', 9, N'Xe ghế ngồi 16 chỗ 009', NULL),  -- Xe ghế ngồi 16 chỗ
('63B-12345', 10, N'Xe giường nằm đôi 010', NULL);  -- Xe giường nằm đôi 20 chỗ

--vị trí ghế 
-- Xe 51B-12345 (29 ghế)
-- Chèn vị trí ghế cho xe 51B-12345 (29 ghế)
INSERT INTO Vitrighe (ID_VITRI, Bienso, Tenvitri, Trangthai)
VALUES
(1, '51B-12345', 'G1', 0), (2, '51B-12345', 'G2', 0), (3, '51B-12345', 'G3', 0), (4, '51B-12345', 'G4', 0), 
(5, '51B-12345', 'G5', 0), (6, '51B-12345', 'G6', 0), (7, '51B-12345', 'G7', 0), (8, '51B-12345', 'G8', 0), 
(9, '51B-12345', 'G9', 0), (10, '51B-12345', 'G10', 0), (11, '51B-12345', 'G11', 0), (12, '51B-12345', 'G12', 0), 
(13, '51B-12345', 'G13', 0), (14, '51B-12345', 'G14', 0), (15, '51B-12345', 'G15', 0), (16, '51B-12345', 'G16', 0), 
(17, '51B-12345', 'G17', 0), (18, '51B-12345', 'G18', 0), (19, '51B-12345', 'G19', 0), (20, '51B-12345', 'G20', 0), 
(21, '51B-12345', 'G21', 0), (22, '51B-12345', 'G22', 0), (23, '51B-12345', 'G23', 0), (24, '51B-12345', 'G24', 0), 
(25, '51B-12345', 'G25', 0), (26, '51B-12345', 'G26', 0), (27, '51B-12345', 'G27', 0), (28, '51B-12345', 'G28', 0), 
(29, '51B-12345', 'G29', 0);

-- Chèn vị trí ghế cho xe 43A-56789 (25 ghế)
INSERT INTO Vitrighe (ID_VITRI, Bienso, Tenvitri, Trangthai) VALUES 
(30, '43A-56789', 'G1', 0), (31, '43A-56789', 'G2', 0), (32, '43A-56789', 'G3', 0), (33, '43A-56789', 'G4', 0), 
(34, '43A-56789', 'G5', 0), (35, '43A-56789', 'G6', 0), (36, '43A-56789', 'G7', 0), (37, '43A-56789', 'G8', 0), 
(38, '43A-56789', 'G9', 0), (39, '43A-56789', 'G10', 0), (40, '43A-56789', 'G11', 0), (41, '43A-56789', 'G12', 0), 
(42, '43A-56789', 'G13', 0), (43, '43A-56789', 'G14', 0), (44, '43A-56789', 'G15', 0), (45, '43A-56789', 'G16', 0), 
(46, '43A-56789', 'G17', 0), (47, '43A-56789', 'G18', 0), (48, '43A-56789', 'G19', 0), (49, '43A-56789', 'G20', 0), 
(50, '43A-56789', 'G21', 0), (51, '43A-56789', 'G22', 0), (52, '43A-56789', 'G23', 0), (53, '43A-56789', 'G24', 0), 
(54, '43A-56789', 'G25', 0);
-- Chèn vị trí ghế cho xe 51H-98765 (40 ghế)
-- Chèn vị trí ghế cho xe 51H-98765 (G1 đến G40)
INSERT INTO Vitrighe (ID_VITRI, Bienso, Tenvitri, Trangthai) VALUES 
(55, '51H-98765', 'G1', 0), (56, '51H-98765', 'G2', 0), (57, '51H-98765', 'G3', 0), (58, '51H-98765', 'G4', 0), 
(59, '51H-98765', 'G5', 0), (60, '51H-98765', 'G6', 0), (61, '51H-98765', 'G7', 0), (62, '51H-98765', 'G8', 0), 
(63, '51H-98765', 'G9', 0), (64, '51H-98765', 'G10', 0), (65, '51H-98765', 'G11', 0), (66, '51H-98765', 'G12', 0), 
(67, '51H-98765', 'G13', 0), (68, '51H-98765', 'G14', 0), (69, '51H-98765', 'G15', 0), (70, '51H-98765', 'G16', 0), 
(71, '51H-98765', 'G17', 0), (72, '51H-98765', 'G18', 0), (73, '51H-98765', 'G19', 0), (74, '51H-98765', 'G20', 0), 
(75, '51H-98765', 'G21', 0), (76, '51H-98765', 'G22', 0), (77, '51H-98765', 'G23', 0), (78, '51H-98765', 'G24', 0), 
(79, '51H-98765', 'G25', 0), (80, '51H-98765', 'G26', 0), (81, '51H-98765', 'G27', 0), (82, '51H-98765', 'G28', 0), 
(83, '51H-98765', 'G29', 0), (84, '51H-98765', 'G30', 0), (85, '51H-98765', 'G31', 0), (86, '51H-98765', 'G32', 0), 
(87, '51H-98765', 'G33', 0), (88, '51H-98765', 'G34', 0), (89, '51H-98765', 'G35', 0), (90, '51H-98765', 'G36', 0), 
(91, '51H-98765', 'G37', 0), (92, '51H-98765', 'G38', 0), (93, '51H-98765', 'G39', 0), (94, '51H-98765', 'G40', 0);

-- Chèn vị trí ghế cho xe 79B-45678 (G1 đến G34)
INSERT INTO Vitrighe (ID_VITRI, Bienso, Tenvitri, Trangthai) VALUES 
(95, '79B-45678', 'G1', 0), (96, '79B-45678', 'G2', 0), (97, '79B-45678', 'G3', 0), (98, '79B-45678', 'G4', 0), 
(99, '79B-45678', 'G5', 0), (100, '79B-45678', 'G6', 0), (101, '79B-45678', 'G7', 0), (102, '79B-45678', 'G8', 0), 
(103, '79B-45678', 'G9', 0), (104, '79B-45678', 'G10', 0), (105, '79B-45678', 'G11', 0), (106, '79B-45678', 'G12', 0), 
(107, '79B-45678', 'G13', 0), (108, '79B-45678', 'G14', 0), (109, '79B-45678', 'G15', 0), (110, '79B-45678', 'G16', 0), 
(111, '79B-45678', 'G17', 0), (112, '79B-45678', 'G18', 0), (113, '79B-45678', 'G19', 0), (114, '79B-45678', 'G20', 0), 
(115, '79B-45678', 'G21', 0), (116, '79B-45678', 'G22', 0), (117, '79B-45678', 'G23', 0), (118, '79B-45678', 'G24', 0), 
(119, '79B-45678', 'G25', 0), (120, '79B-45678', 'G26', 0), (121, '79B-45678', 'G27', 0), (122, '79B-45678', 'G28', 0), 
(123, '79B-45678', 'G29', 0), (124, '79B-45678', 'G30', 0), (125, '79B-45678', 'G31', 0), (126, '79B-45678', 'G32', 0), 
(127, '79B-45678', 'G33', 0), (128, '79B-45678', 'G34', 0);

-- Chèn vị trí ghế cho xe 36A-78901 (G1 đến G22)
INSERT INTO Vitrighe (ID_VITRI, Bienso, Tenvitri, Trangthai) VALUES 
(129, '36A-78901', 'G1', 0), (130, '36A-78901', 'G2', 0), (131, '36A-78901', 'G3', 0), (132, '36A-78901', 'G4', 0), 
(133, '36A-78901', 'G5', 0), (134, '36A-78901', 'G6', 0), (135, '36A-78901', 'G7', 0), (136, '36A-78901', 'G8', 0), 
(137, '36A-78901', 'G9', 0), (138, '36A-78901', 'G10', 0), (139, '36A-78901', 'G11', 0), (140, '36A-78901', 'G12', 0), 
(141, '36A-78901', 'G13', 0), (142, '36A-78901', 'G14', 0), (143, '36A-78901', 'G15', 0), (144, '36A-78901', 'G16', 0), 
(145, '36A-78901', 'G17', 0), (146, '36A-78901', 'G18', 0), (147, '36A-78901', 'G19', 0), (148, '36A-78901', 'G20', 0), 
(149, '36A-78901', 'G21', 0), (150, '36A-78901', 'G22', 0);

SET IDENTITY_INSERT ChuyenXe ON;

-- Thêm các chuyến xe với Mã nhân viên tương ứng và Biển số xe đã chỉnh sửa
INSERT INTO ChuyenXe (MaChuyen, MaTuyen, ThoiDiemKhoiHanh, ThoiDiemDenDuKien, GiaVe, BienSoXe, TenChuyenXe, GhiChu, MaNhanVien, MaTaiXe)
VALUES
(1, 1, '2025-08-16 08:00:00', '2025-08-17 10:30:00', 290000, '51B-12345', N'TP. Hồ Chí Minh → Đà Lạt', N'Chuyến mới sau ngày 15/5', 1, 1),  -- Tài xế: Nguyễn Văn An
(2, 2, '2025-08-16 09:00:00', '2025-08-17 11:30:00', 290000, '43A-56789', N'Đà Lạt → TP. Hồ Chí Minh', N'Chuyến mới sau ngày 15/5', 2, 2),  -- Lơ xe: Trần Thị Bé
(3, 3, '2025-08-16 10:00:00', '2025-08-17 12:30:00', 750000, '51H-98765', N'TP. Hồ Chí Minh → Đà Nẵng', N'Chuyến mới sau ngày 15/5', 3, 3),  -- Nhân viên hỗ trợ: Lê Văn Cường
(4, 4, '2025-08-16 11:00:00', '2025-08-17 13:30:00', 750000, '79B-45678', N'Đà Nẵng → TP. Hồ Chí Minh', N'Chuyến mới sau ngày 15/5', 4, 4),  -- Tài xế: Phạm Thị Duyên
(5, 5, '2025-08-16 12:00:00', '2025-08-17 14:30:00', 220000, '50K-23456', N'TP. Hồ Chí Minh → Nha Trang', N'Chuyến mới sau ngày 15/5', 5, 5),  -- Lơ xe: Hoàng Văn Em
(6, 6, '2025-08-16 13:00:00', '2025-08-17 15:30:00', 220000, '36A-78901', N'Nha Trang → TP. Hồ Chí Minh', N'Chuyến mới sau ngày 15/5', 6, 6),  -- Nhân viên hỗ trợ: Ngô Thị Hoa
(7, 7, '2025-08-16 14:00:00', '2025-08-17 16:30:00', 800000, '51G-34567', N'TP. Hồ Chí Minh → Huế', N'Chuyến mới sau ngày 15/5', 7, 7),  -- Tài xế: Võ Văn Khang
(8, 8, '2025-08-16 15:00:00', '2025-08-17 17:30:00', 800000, '92C-67890', N'Huế → TP. Hồ Chí Minh', N'Chuyến mới sau ngày 15/5', 8, 8),  -- Lơ xe: Bùi Thị Lan
(9, 9, '2025-08-16 16:00:00', '2025-08-17 18:30:00', 1200000, '54D-89012', N'TP. Hồ Chí Minh → Hà Nội', N'Chuyến mới sau ngày 15/5', 9, 9),  -- Tài xế: Đỗ Văn Minh
(10, 10, '2025-08-16 17:00:00', '2025-08-17 19:30:00', 1200000, '63B-12345', N'Hà Nội → TP. Hồ Chí Minh', N'Chuyến mới sau ngày 15/5', 10, 10);  -- Nhân viên hỗ trợ: Nguyễn Thị Ngọc

SET IDENTITY_INSERT ChuyenXe OFF;

SET IDENTITY_INSERT PhanQuyen ON;
INSERT INTO PhanQuyen (MaQuyen, TenQuyen)
VALUES
(1, N'Quản trị viên'),
(2, N'Nhân viên'),
(3, N'Khách hàng'),
(4, N'Tài xế'),
(5, N'Lơ xe');

SET IDENTITY_INSERT PhanQuyen OFF;


SET IDENTITY_INSERT NguoiDung ON;
INSERT INTO NguoiDung (Id, Email, Sdt, HoTen, MatKhau, NgaySinh, DiaChi, MaQuyen, HinhAnh, ChuThich)
VALUES
(1, 'admin@example.com', '0909000001', N'Nguyễn Văn Admin', '123456', '1990-01-01', N'TP. Hồ Chí Minh', 1, NULL, N'Tài khoản quản trị viên'),
(2, 'nhanvien1@example.com', '0909000002', N'Lê Thị Nhân Viên', '123456', '1992-05-10', N'Đà Nẵng', 2, NULL, N'Nhân viên văn phòng'),
(3, 'khach1@example.com', '0909000003', N'Phạm Văn Khách', '123456', '1995-07-20', N'Nha Trang', 3, NULL, N'Khách hàng thân thiết'),
(4, 'taixe1@example.com', '0909000004', N'Võ Văn Tài Xế', '123456', '1985-11-30', N'Cần Thơ', 4, NULL, N'Tài xế chính'),
(5, 'loxevien1@example.com', '0909000005', N'Nguyễn Thị Lơ Xe', '123456', '1988-04-15', N'Huế', 5, NULL, N'Lơ xe tuyến Bắc Nam');
SET IDENTITY_INSERT NguoiDung OFF;


INSERT INTO KhuyenMai (TenKhuyenMai, MoTa, PhanTramGiam, NgayBatDau, NgayKetThuc)
VALUES 
    (N'Giảm giá hè 2025', N'Ưu đãi cho tất cả chuyến xe tháng 5', 20, '2025-05-01', '2025-08-31'),
    (N'Khuyến mãi lễ 30/4', N'Giảm giá đặc biệt dịp lễ', 15, '2025-04-25', '2025-08-20'),
    (N'Mừng sinh nhật Khánh An', N'Giảm giá cho khách đặt vé tuần này', 10, '2025-05-10', '2025-08-25');
