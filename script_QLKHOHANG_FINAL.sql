USE [master]
GO
/****** Object:  Database [QLKHOHANG]    Script Date: 08/04/2023 9:38:08 PM ******/
CREATE DATABASE [QLKHOHANG]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QLKHOHANG', FILENAME = N'E:\Database\QLKHOHANG.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'QLKHOHANG_log', FILENAME = N'E:\Database\QLKHOHANG_log.ldf' , SIZE = 4224KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [QLKHOHANG] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QLKHOHANG].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QLKHOHANG] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QLKHOHANG] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QLKHOHANG] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QLKHOHANG] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QLKHOHANG] SET ARITHABORT OFF 
GO
ALTER DATABASE [QLKHOHANG] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QLKHOHANG] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QLKHOHANG] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QLKHOHANG] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QLKHOHANG] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QLKHOHANG] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QLKHOHANG] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QLKHOHANG] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QLKHOHANG] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QLKHOHANG] SET  DISABLE_BROKER 
GO
ALTER DATABASE [QLKHOHANG] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QLKHOHANG] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QLKHOHANG] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QLKHOHANG] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QLKHOHANG] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QLKHOHANG] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QLKHOHANG] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QLKHOHANG] SET RECOVERY FULL 
GO
ALTER DATABASE [QLKHOHANG] SET  MULTI_USER 
GO
ALTER DATABASE [QLKHOHANG] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QLKHOHANG] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QLKHOHANG] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QLKHOHANG] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [QLKHOHANG] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'QLKHOHANG', N'ON'
GO
ALTER DATABASE [QLKHOHANG] SET QUERY_STORE = OFF
GO
USE [QLKHOHANG]
GO
/****** Object:  Table [dbo].[ChiTietPN]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietPN](
	[MaCTPN] [int] IDENTITY(1,1) NOT NULL,
	[SoPN] [varchar](20) NULL,
	[MaHH] [varchar](20) NULL,
	[MaDVT] [varchar](20) NULL,
	[SoLuong] [float] NULL,
	[DonGia] [float] NULL,
	[ThanhTien]  AS ([SoLuong]*[DonGia]),
	[GhiChu] [nvarchar](200) NULL,
 CONSTRAINT [PK_ChiTietPN] PRIMARY KEY CLUSTERED 
(
	[MaCTPN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietPX]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietPX](
	[MaCTPX] [int] IDENTITY(1,1) NOT NULL,
	[SoPX] [varchar](20) NULL,
	[MaHH] [varchar](20) NULL,
	[MaDVT] [varchar](20) NULL,
	[SoLuong] [float] NULL,
	[DonGia] [float] NULL,
	[ThanhTien]  AS ([SoLuong]*[DonGia]),
	[GhiChu] [nvarchar](200) NULL,
 CONSTRAINT [PK_ChiTietPX] PRIMARY KEY CLUSTERED 
(
	[MaCTPX] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChucVu]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChucVu](
	[MaCV] [varchar](20) NOT NULL,
	[TenCV] [nvarchar](200) NULL,
	[TinhTrang] [bit] NULL,
 CONSTRAINT [PK_ChucVu] PRIMARY KEY CLUSTERED 
(
	[MaCV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonViTinh]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonViTinh](
	[MaDVT] [varchar](20) NOT NULL,
	[TenDVT] [nvarchar](200) NULL,
	[TinhTrang] [bit] NULL,
 CONSTRAINT [PK_DonViTinh] PRIMARY KEY CLUSTERED 
(
	[MaDVT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HangHoa]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HangHoa](
	[MaHH] [varchar](20) NOT NULL,
	[TenHH] [nvarchar](200) NULL,
	[MaLoaiHH] [varchar](20) NULL,
	[XuatXu] [nvarchar](200) NULL,
	[QuyCach] [nvarchar](200) NULL,
	[MauSac] [nvarchar](100) NULL,
	[KichThuoc] [nvarchar](50) NULL,
	[SLTonToiThieu] [float] NULL,
	[SLTonToiDa] [float] NULL,
	[GhiChu] [nvarchar](300) NULL,
	[TinhTrang] [bit] NULL,
 CONSTRAINT [PK_HangHoa] PRIMARY KEY CLUSTERED 
(
	[MaHH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhachHang]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhachHang](
	[MaKH] [varchar](20) NOT NULL,
	[TenKH] [nvarchar](200) NULL,
	[MST] [varchar](20) NULL,
	[DiaChi] [nvarchar](300) NULL,
	[SDT] [varchar](20) NULL,
	[Email] [nvarchar](200) NULL,
	[GhiChu] [nvarchar](300) NULL,
	[TinhTrang] [bit] NULL,
 CONSTRAINT [PK_KhachHang] PRIMARY KEY CLUSTERED 
(
	[MaKH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Kho]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kho](
	[MaKho] [varchar](20) NOT NULL,
	[TenKho] [nvarchar](200) NULL,
	[DiaChi] [nvarchar](300) NULL,
	[SDT] [varchar](20) NULL,
	[GhiChu] [nvarchar](300) NULL,
	[TinhTrang] [bit] NULL,
 CONSTRAINT [PK_Kho] PRIMARY KEY CLUSTERED 
(
	[MaKho] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoaiHangHoa]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoaiHangHoa](
	[MaLoaiHH] [varchar](20) NOT NULL,
	[TenLoaiHH] [nvarchar](200) NULL,
	[TinhTrang] [bit] NULL,
 CONSTRAINT [PK_LoaiHangHoa] PRIMARY KEY CLUSTERED 
(
	[MaLoaiHH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhaCC]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhaCC](
	[MaNCC] [varchar](20) NOT NULL,
	[TenNCC] [nvarchar](200) NULL,
	[DiaChi] [nvarchar](300) NULL,
	[SDT] [varchar](20) NULL,
	[Email] [nvarchar](200) NULL,
	[GhiChu] [nvarchar](300) NULL,
	[TinhTrang] [bit] NULL,
 CONSTRAINT [PK_NhaCC] PRIMARY KEY CLUSTERED 
(
	[MaNCC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[MaNV] [varchar](20) NOT NULL,
	[MaChucVu] [varchar](20) NULL,
	[HoTen] [nvarchar](200) NULL,
	[GioiTinh] [nvarchar](50) NULL,
	[NgaySinh] [date] NULL,
	[SDT] [varchar](20) NULL,
	[DiaChi] [nvarchar](300) NULL,
	[MatKhau] [varchar](200) NULL,
	[RoleID] [varchar](20) NULL,
	[TinhTrang] [bit] NULL,
 CONSTRAINT [PK_NhanVien] PRIMARY KEY CLUSTERED 
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuNhap]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuNhap](
	[SoPN] [varchar](20) NOT NULL,
	[MaNCC] [varchar](20) NULL,
	[MaKho] [varchar](20) NULL,
	[NgayNhap] [datetime] NULL,
	[LyDoNhap] [nvarchar](200) NULL,
	[TongSL] [float] NULL,
	[TongTriGia] [float] NULL,
	[MaNV] [varchar](20) NULL,
	[GhiChu] [nvarchar](300) NULL,
	[TinhTrang] [bit] NULL,
 CONSTRAINT [PK_PhieuNhap] PRIMARY KEY CLUSTERED 
(
	[SoPN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuXuat]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuXuat](
	[SoPX] [varchar](20) NOT NULL,
	[MaKH] [varchar](20) NULL,
	[MaKho] [varchar](20) NULL,
	[NgayXuat] [datetime] NULL,
	[LyDoXuat] [nvarchar](200) NULL,
	[MaNV] [varchar](20) NULL,
	[GhiChu] [nvarchar](200) NULL,
	[TongSL] [float] NULL,
	[TongTien] [float] NULL,
	[TrangThai] [int] NULL,
 CONSTRAINT [PK_PhieuXuat] PRIMARY KEY CLUSTERED 
(
	[SoPX] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [varchar](20) NOT NULL,
	[DG] [nvarchar](200) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ChiTietPN] ON 

INSERT [dbo].[ChiTietPN] ([MaCTPN], [SoPN], [MaHH], [MaDVT], [SoLuong], [DonGia], [GhiChu]) VALUES (9, N'PN002', N'HH001', N'DVT001', 10, 20000, N'')
INSERT [dbo].[ChiTietPN] ([MaCTPN], [SoPN], [MaHH], [MaDVT], [SoLuong], [DonGia], [GhiChu]) VALUES (10, N'PN002', N'HH002', N'DVT001', 20, 50000, N'')
INSERT [dbo].[ChiTietPN] ([MaCTPN], [SoPN], [MaHH], [MaDVT], [SoLuong], [DonGia], [GhiChu]) VALUES (11, N'PN001', N'HH001', N'DVT001', 10, 20000, N'')
SET IDENTITY_INSERT [dbo].[ChiTietPN] OFF
GO
SET IDENTITY_INSERT [dbo].[ChiTietPX] ON 

INSERT [dbo].[ChiTietPX] ([MaCTPX], [SoPX], [MaHH], [MaDVT], [SoLuong], [DonGia], [GhiChu]) VALUES (14, N'PX001', N'HH001', N'DVT001', 5, 25000, N'')
INSERT [dbo].[ChiTietPX] ([MaCTPX], [SoPX], [MaHH], [MaDVT], [SoLuong], [DonGia], [GhiChu]) VALUES (15, N'PX002', N'HH001', N'DVT001', 2, 60000, N'')
INSERT [dbo].[ChiTietPX] ([MaCTPX], [SoPX], [MaHH], [MaDVT], [SoLuong], [DonGia], [GhiChu]) VALUES (16, N'PX002', N'HH002', N'DVT001', 2, 60000, N'')
INSERT [dbo].[ChiTietPX] ([MaCTPX], [SoPX], [MaHH], [MaDVT], [SoLuong], [DonGia], [GhiChu]) VALUES (18, N'PX003', N'HH001', N'DVT001', 1, 50000, N'')
SET IDENTITY_INSERT [dbo].[ChiTietPX] OFF
GO
INSERT [dbo].[ChucVu] ([MaCV], [TenCV], [TinhTrang]) VALUES (N'CV01', N'Ban GĐ', 1)
INSERT [dbo].[ChucVu] ([MaCV], [TenCV], [TinhTrang]) VALUES (N'CV02', N'Hành chánh', 1)
INSERT [dbo].[ChucVu] ([MaCV], [TenCV], [TinhTrang]) VALUES (N'CV03', N'Kế toán', 1)
INSERT [dbo].[ChucVu] ([MaCV], [TenCV], [TinhTrang]) VALUES (N'CV04', N'Bán hàng', 1)
INSERT [dbo].[ChucVu] ([MaCV], [TenCV], [TinhTrang]) VALUES (N'CV05', N'Giao nhận', 1)
GO
INSERT [dbo].[DonViTinh] ([MaDVT], [TenDVT], [TinhTrang]) VALUES (N'DVT001', N'Cái', 1)
INSERT [dbo].[DonViTinh] ([MaDVT], [TenDVT], [TinhTrang]) VALUES (N'DVT002', N'Hộp', 1)
GO
INSERT [dbo].[HangHoa] ([MaHH], [TenHH], [MaLoaiHH], [XuatXu], [QuyCach], [MauSac], [KichThuoc], [SLTonToiThieu], [SLTonToiDa], [GhiChu], [TinhTrang]) VALUES (N'HH001', N'Tên hàng hóa 001', N'L001', N'Thái Bình, Việt Nam', NULL, N'Đỏ', N'M', 2, 100, NULL, 1)
INSERT [dbo].[HangHoa] ([MaHH], [TenHH], [MaLoaiHH], [XuatXu], [QuyCach], [MauSac], [KichThuoc], [SLTonToiThieu], [SLTonToiDa], [GhiChu], [TinhTrang]) VALUES (N'HH002', N'Tên hàng hóa 002', N'L002', N'Việt Nam', NULL, N'Xanh', N'S', 1, 10, NULL, 1)
GO
INSERT [dbo].[KhachHang] ([MaKH], [TenKH], [MST], [DiaChi], [SDT], [Email], [GhiChu], [TinhTrang]) VALUES (N'KH001', N'Khách hàng 001', N'0101010101', N'Hà Nội', N'1111111111111', N'kh001@gmail.com', N'Khách hàng thân thiết', 1)
INSERT [dbo].[KhachHang] ([MaKH], [TenKH], [MST], [DiaChi], [SDT], [Email], [GhiChu], [TinhTrang]) VALUES (N'KH002', N'Khách hàng 002', N'0202020202', N'TP. HCM', N'22222222222', N'kh002@gmail.com', N'', 1)
GO
INSERT [dbo].[Kho] ([MaKho], [TenKho], [DiaChi], [SDT], [GhiChu], [TinhTrang]) VALUES (N'KHO01', N'Kho số 01', N'TP. HCM', N'1111111111', N'Kho chính', 1)
INSERT [dbo].[Kho] ([MaKho], [TenKho], [DiaChi], [SDT], [GhiChu], [TinhTrang]) VALUES (N'KHO02', N'Kho số 02', N'Hà Nội', N'2222222222', N'', 1)
GO
INSERT [dbo].[LoaiHangHoa] ([MaLoaiHH], [TenLoaiHH], [TinhTrang]) VALUES (N'L001', N'Dụng cụ thể thao', 1)
INSERT [dbo].[LoaiHangHoa] ([MaLoaiHH], [TenLoaiHH], [TinhTrang]) VALUES (N'L002', N'Quần áo', 1)
GO
INSERT [dbo].[NhaCC] ([MaNCC], [TenNCC], [DiaChi], [SDT], [Email], [GhiChu], [TinhTrang]) VALUES (N'NCC01', N'Nhà cung cấp 01', N'TP. HCM', N'111111111d', N'nhacc01@gmail.com', N'Nhà cung cấp chính', 1)
INSERT [dbo].[NhaCC] ([MaNCC], [TenNCC], [DiaChi], [SDT], [Email], [GhiChu], [TinhTrang]) VALUES (N'NCC02', N'Nhà cung cấp 02', N'Hà Nội', N'22222222', N'nhacc02@gmail.com', N'', 1)
GO
INSERT [dbo].[NhanVien] ([MaNV], [MaChucVu], [HoTen], [GioiTinh], [NgaySinh], [SDT], [DiaChi], [MatKhau], [RoleID], [TinhTrang]) VALUES (N'NV001', N'CV01', N'NGUYỄN VĂN A', N'Nam', CAST(N'2000-01-01' AS Date), N'11111111111', N'TP. HCM', N'123', N'ADMIN', 1)
INSERT [dbo].[NhanVien] ([MaNV], [MaChucVu], [HoTen], [GioiTinh], [NgaySinh], [SDT], [DiaChi], [MatKhau], [RoleID], [TinhTrang]) VALUES (N'NV02', N'CV03', N'LÊ THỊ B', N'Nữ', CAST(N'2022-11-01' AS Date), N'2222222222', N'HÀ NỘI', N'123', NULL, 1)
GO
INSERT [dbo].[PhieuNhap] ([SoPN], [MaNCC], [MaKho], [NgayNhap], [LyDoNhap], [TongSL], [TongTriGia], [MaNV], [GhiChu], [TinhTrang]) VALUES (N'PN001', N'NCC01', N'KHO01', CAST(N'2023-04-08T15:41:13.413' AS DateTime), N'Nhập kho', 10, 200000, N'NV001', N'Thêm mới', 1)
INSERT [dbo].[PhieuNhap] ([SoPN], [MaNCC], [MaKho], [NgayNhap], [LyDoNhap], [TongSL], [TongTriGia], [MaNV], [GhiChu], [TinhTrang]) VALUES (N'PN002', N'NCC01', N'KHO02', CAST(N'2023-04-08T20:57:13.383' AS DateTime), N'Nhập kho', 30, 1200000, N'NV001', N'', 1)
GO
INSERT [dbo].[PhieuXuat] ([SoPX], [MaKH], [MaKho], [NgayXuat], [LyDoXuat], [MaNV], [GhiChu], [TongSL], [TongTien], [TrangThai]) VALUES (N'PX001', N'KH001', N'KHO01', CAST(N'2023-04-08T21:01:23.863' AS DateTime), N'Bán hàng', N'NV001', N'', 5, 125000, 1)
INSERT [dbo].[PhieuXuat] ([SoPX], [MaKH], [MaKho], [NgayXuat], [LyDoXuat], [MaNV], [GhiChu], [TongSL], [TongTien], [TrangThai]) VALUES (N'PX002', N'KH002', N'KHO02', CAST(N'2023-04-08T21:18:24.727' AS DateTime), N'Bán hàng', N'NV001', N'', 4, 240000, 1)
INSERT [dbo].[PhieuXuat] ([SoPX], [MaKH], [MaKho], [NgayXuat], [LyDoXuat], [MaNV], [GhiChu], [TongSL], [TongTien], [TrangThai]) VALUES (N'PX003', N'KH002', N'KHO02', CAST(N'2023-05-01T21:29:29.510' AS DateTime), N'Bán hàng', N'NV001', N'', 1, 50000, 1)
GO
INSERT [dbo].[Role] ([ID], [DG]) VALUES (N'ADMIN', N'ADMIN')
INSERT [dbo].[Role] ([ID], [DG]) VALUES (N'USER', N'USER')
GO
ALTER TABLE [dbo].[ChiTietPN]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPN_DonViTinh] FOREIGN KEY([MaDVT])
REFERENCES [dbo].[DonViTinh] ([MaDVT])
GO
ALTER TABLE [dbo].[ChiTietPN] CHECK CONSTRAINT [FK_ChiTietPN_DonViTinh]
GO
ALTER TABLE [dbo].[ChiTietPN]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPN_HangHoa] FOREIGN KEY([MaHH])
REFERENCES [dbo].[HangHoa] ([MaHH])
GO
ALTER TABLE [dbo].[ChiTietPN] CHECK CONSTRAINT [FK_ChiTietPN_HangHoa]
GO
ALTER TABLE [dbo].[ChiTietPN]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPN_PhieuNhap] FOREIGN KEY([SoPN])
REFERENCES [dbo].[PhieuNhap] ([SoPN])
GO
ALTER TABLE [dbo].[ChiTietPN] CHECK CONSTRAINT [FK_ChiTietPN_PhieuNhap]
GO
ALTER TABLE [dbo].[ChiTietPX]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPX_DonViTinh] FOREIGN KEY([MaDVT])
REFERENCES [dbo].[DonViTinh] ([MaDVT])
GO
ALTER TABLE [dbo].[ChiTietPX] CHECK CONSTRAINT [FK_ChiTietPX_DonViTinh]
GO
ALTER TABLE [dbo].[ChiTietPX]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPX_HangHoa] FOREIGN KEY([MaHH])
REFERENCES [dbo].[HangHoa] ([MaHH])
GO
ALTER TABLE [dbo].[ChiTietPX] CHECK CONSTRAINT [FK_ChiTietPX_HangHoa]
GO
ALTER TABLE [dbo].[ChiTietPX]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPX_PhieuXuat] FOREIGN KEY([SoPX])
REFERENCES [dbo].[PhieuXuat] ([SoPX])
GO
ALTER TABLE [dbo].[ChiTietPX] CHECK CONSTRAINT [FK_ChiTietPX_PhieuXuat]
GO
ALTER TABLE [dbo].[HangHoa]  WITH CHECK ADD  CONSTRAINT [FK_HangHoa_LoaiHangHoa] FOREIGN KEY([MaLoaiHH])
REFERENCES [dbo].[LoaiHangHoa] ([MaLoaiHH])
GO
ALTER TABLE [dbo].[HangHoa] CHECK CONSTRAINT [FK_HangHoa_LoaiHangHoa]
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [FK_NhanVien_ChucVu] FOREIGN KEY([MaChucVu])
REFERENCES [dbo].[ChucVu] ([MaCV])
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [FK_NhanVien_ChucVu]
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [FK_NhanVien_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([ID])
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [FK_NhanVien_Role]
GO
ALTER TABLE [dbo].[PhieuNhap]  WITH CHECK ADD  CONSTRAINT [FK_PhieuNhap_Kho] FOREIGN KEY([MaKho])
REFERENCES [dbo].[Kho] ([MaKho])
GO
ALTER TABLE [dbo].[PhieuNhap] CHECK CONSTRAINT [FK_PhieuNhap_Kho]
GO
ALTER TABLE [dbo].[PhieuNhap]  WITH CHECK ADD  CONSTRAINT [FK_PhieuNhap_NhaCC] FOREIGN KEY([MaNCC])
REFERENCES [dbo].[NhaCC] ([MaNCC])
GO
ALTER TABLE [dbo].[PhieuNhap] CHECK CONSTRAINT [FK_PhieuNhap_NhaCC]
GO
ALTER TABLE [dbo].[PhieuXuat]  WITH CHECK ADD  CONSTRAINT [FK_PhieuXuat_KhachHang] FOREIGN KEY([MaKH])
REFERENCES [dbo].[KhachHang] ([MaKH])
GO
ALTER TABLE [dbo].[PhieuXuat] CHECK CONSTRAINT [FK_PhieuXuat_KhachHang]
GO
ALTER TABLE [dbo].[PhieuXuat]  WITH CHECK ADD  CONSTRAINT [FK_PhieuXuat_Kho] FOREIGN KEY([MaKho])
REFERENCES [dbo].[Kho] ([MaKho])
GO
ALTER TABLE [dbo].[PhieuXuat] CHECK CONSTRAINT [FK_PhieuXuat_Kho]
GO
ALTER TABLE [dbo].[PhieuXuat]  WITH CHECK ADD  CONSTRAINT [FK_PhieuXuat_NhanVien] FOREIGN KEY([MaNV])
REFERENCES [dbo].[NhanVien] ([MaNV])
GO
ALTER TABLE [dbo].[PhieuXuat] CHECK CONSTRAINT [FK_PhieuXuat_NhanVien]
GO
/****** Object:  StoredProcedure [dbo].[sp_doanhthu]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_doanhthu]
(
	-- TINH SO LUONG NHAP XUAT TON HANG HOA THEO THANG NAM
	@tungay date,
	@denngay date,
	@makho varchar(20),
	@mahh varchar(20),
	@makh varchar(20)
)
AS
BEGIN

	SELECT PX.NgayXuat,PX.SoPX, PX.MaKho, CTPX.MaHH, H.TenHH, PX.MaKH, KH.TenKH, CTPX.SoLuong, CTPX.DonGia, CTPX.ThanhTien
	FROM PhieuXuat PX, ChiTietPX CTPX, HangHoa H, KhachHang KH
	WHERE PX.SoPX=CTPX.SoPX AND CTPX.MaHH = H.MaHH AND PX.MaKH=KH.MaKH
			AND CAST(PX.NgayXuat AS DATE) BETWEEN @tungay AND @denngay
			AND PX.MaKho = (CASE WHEN(@makho) is null then PX.MaKho ELSE @makho END)
			AND CTPX.MaHH = (CASE WHEN(@mahh) is null then CTPX.MaHH ELSE @mahh END)
			AND PX.MaKH = (CASE WHEN(@makh) is null then PX.MaKH ELSE @makh END)
END

GO
/****** Object:  StoredProcedure [dbo].[sp_in_ctpx]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[sp_in_ctpx]
(
	@sopx varchar(20)
)
AS
BEGIN
	SELECT PhieuXuat.SoPX, PhieuXuat.NgayXuat, PhieuXuat.MaKho, PhieuXuat.MaKH, KhachHang.TenKH, KhachHang.DiaChi, KhachHang.SDT, ChiTietPX.MaHH, HangHoa.TenHH, DonViTinh.TenDVT, ChiTietPX.SoLuong, 
					 ChiTietPX.DonGia, ChiTietPX.ThanhTien, PhieuXuat.TongTien, NhanVien.HoTen
	FROM   (ChiTietPX INNER JOIN
			PhieuXuat ON ChiTietPX.SoPX = PhieuXuat.SoPX )LEFT JOIN
			HangHoa ON ChiTietPX.MaHH = HangHoa.MaHH LEFT JOIN
			KhachHang ON PhieuXuat.MaKH = KhachHang.MaKH LEFT JOIN
			DonViTinh ON ChiTietPX.MaDVT = DonViTinh.MaDVT LEFT JOIN
			NhanVien ON PhieuXuat.MaNV = NhanVien.MaNV
	WHERE PhieuXuat.SoPX=@sopx
END

GO
/****** Object:  StoredProcedure [dbo].[sp_nxt_thang]    Script Date: 08/04/2023 9:38:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_nxt_thang]
(
	-- TINH SO LUONG NHAP XUAT TON HANG HOA THEO THANG NAM
	@thang int,
	@nam int,
	@makho varchar(20),
	@mahh varchar(20)
)
AS
BEGIN

	-- TINH SO LUONG TON DAU KY (CUA THANG TRUOC)
	SELECT pn.MaKho, ctpn.MaHH, sum(ctpn.SoLuong) as slnhap INTO #NA
	FROM PhieuNhap pn inner join ChiTietPN ctpn on pn.SoPN=ctpn.SoPN
	WHERE (YEAR(pn.NgayNhap)) < @nam OR (YEAR(pn.NgayNhap) = @nam and MONTH(pn.NgayNhap) < @thang)
	GROUP BY pn.MaKho, ctpn.MaHH

	SELECT px.MaKho, ctpx.MaHH, sum(ctpx.SoLuong) as slxuat INTO #XA
	FROM PhieuXuat px inner join ChiTietPX ctpx on px.SoPX=ctpx.SoPX
	WHERE (YEAR(px.NgayXuat)) < @nam OR (YEAR(px.NgayXuat) = @nam and MONTH(px.NgayXuat) < @thang)
	GROUP BY px.MaKho, ctpx.MaHH
	
	SELECT #NA.MAKHO, #NA.MAHH, (ISNULL(#NA.SLNHAP,0) - ISNULL(#XA.SLXUAT,0)) AS sltondauky INTO #TA
	FROM #NA INNER JOIN #XA ON #NA.MAKHO = #XA.MAKHO AND #NA.MAHH = #XA.MAHH 	

	SELECT * INTO #DK
	FROM
	(
		SELECT *
		FROM #TA
		UNION
		SELECT #NA.MAKHO, #NA.MAHH, ISNULL(#NA.SLNHAP,0) AS sltondauky
		FROM #NA
		WHERE (ISNULL(#NA.MAKHO,'')+ISNULL(#NA.MAHH,'')) NOT IN  (SELECT (ISNULL(#TA.MAKHO,'')+ISNULL(#TA.MAHH,'')) FROM #TA)
		UNION
		SELECT #XA.MAKHO, #XA.MAHH, ISNULL(#XA.SLXUAT,0)*(-1) AS sltondauky
		FROM #XA
		WHERE (ISNULL(#XA.MAKHO,'')+ISNULL(#XA.MAHH,'')) NOT IN  (SELECT (ISNULL(#TA.MAKHO,'')+ISNULL(#TA.MAHH,'')) FROM #TA)
	)DK

	-- TINH SO LUONG NHAP XUAT TRONG KY
	SELECT pn.MaKho, ctpn.MaHH, sum(ctpn.SoLuong) as slnhap into #NB
	FROM PhieuNhap pn inner join ChiTietPN ctpn on pn.SoPN=ctpn.SoPN
	WHERE YEAR(pn.NgayNhap) = @nam and MONTH(pn.NgayNhap) = @thang
	GROUP BY pn.MaKho, ctpn.MaHH

	SELECT px.MaKho, ctpx.MaHH, sum(ctpx.SoLuong) as slxuat INTO #XB
	FROM PhieuXuat px inner join ChiTietPX ctpx on px.SoPX=ctpx.SoPX
	WHERE YEAR(px.NgayXuat) = @nam and MONTH(px.NgayXuat) = @thang
	GROUP BY px.MaKho, ctpx.MaHH

	SELECT #NB.MAKHO, #NB.MAHH, ISNULL(#NB.SLNHAP,0) AS slnhaptrongky, ISNULL(#XB.SLXUAT,0) AS slxuattrongky INTO #TB
	FROM #NB INNER JOIN #XB ON #NB.MAKHO = #XB.MAKHO AND #NB.MAHH = #XB.MAHH 

	SELECT * INTO #TK
	FROM
	(
		SELECT *
		FROM #TB
		UNION
		SELECT #NB.MAKHO, #NB.MAHH, ISNULL(#NB.SLNHAP,0) AS slnhaptrongky, 0 as slxuattrongky
		FROM #NB
		WHERE (ISNULL(#NB.MAKHO,'')+ISNULL(#NB.MAHH,'')) NOT IN  (SELECT (ISNULL(#TB.MAKHO,'')+ISNULL(#TB.MAHH,'')) FROM #TB)
		UNION
		SELECT #XB.MAKHO, #XB.MAHH, 0 AS slnhaptrongky, ISNULL(#XB.SLXUAT,0) AS slxuattrongky
		FROM #XB
		WHERE (ISNULL(#XB.MAKHO,'')+ISNULL(#XB.MAHH,'')) NOT IN  (SELECT (ISNULL(#TB.MAKHO,'')+ISNULL(#TB.MAHH,'')) FROM #TB)
	)TK

	-- TINH SO LUONG NHAP XUAT TON
	SELECT #DK.MaKho, #DK.MaHH, ISNULL(#DK.sltondauky,0) AS sltondauky , ISNULL(#TK.slnhaptrongky,0) AS slnhaptrongky, ISNULL(#TK.slxuattrongky,0) AS slxuattrongky,
				(ISNULL(#DK.sltondauky,0) + ISNULL(#TK.slnhaptrongky,0) - ISNULL(#TK.slxuattrongky,0)) AS sltoncuoiky into #CK
	FROM #DK, #TK
	WHERE #DK.MaKho = #TK.MaKho AND #DK.MaHH=#TK.MaHH

	SELECT CK.*, HH.TenHH
	FROM
	(
		SELECT *
		FROM #CK
		UNION
		SELECT #DK.MAKHO, #DK.MAHH, ISNULL(#DK.sltondauky,0) AS sltondauky,  0 as slnhaptrongky, 0 as slxuattrongky,
				(ISNULL(#DK.sltondauky,0)) as sltoncuoiky
		FROM #DK
		WHERE (ISNULL(#DK.MAKHO,'')+ISNULL(#DK.MAHH,'')) NOT IN  (SELECT (ISNULL(#CK.MAKHO,'')+ISNULL(#CK.MAHH,'')) FROM #CK)
		UNION
		SELECT #TK.MAKHO, #TK.MAHH, 0 AS sltondauky,  ISNULL(#TK.slnhaptrongky,0) as slnhaptrongky, ISNULL(#TK.slxuattrongky,0) as slxuattrongky,
				(ISNULL(#TK.slnhaptrongky,0) - ISNULL(#TK.slxuattrongky,0)) as sltoncuoiky
		FROM #TK
		WHERE (ISNULL(#TK.MAKHO,'')+ISNULL(#TK.MAHH,'')) NOT IN  (SELECT (ISNULL(#CK.MAKHO,'')+ISNULL(#CK.MAHH,'')) FROM #CK)
	) CK INNER JOIN HangHoa HH ON CK.MaHH=HH.MaHH
	WHERE CK.MaKho = (CASE WHEN(@makho) is null then CK.MaKho ELSE @makho END)
			AND CK.MaHH = (CASE WHEN(@mahh) is null then CK.MaHH ELSE @mahh END)

END
GO
USE [master]
GO
ALTER DATABASE [QLKHOHANG] SET  READ_WRITE 
GO
