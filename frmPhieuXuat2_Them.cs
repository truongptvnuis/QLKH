using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace QLKHOHANG
{
    public partial class frmPhieuXuat2_Them : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();

        public static string _tieude = "PHIẾU XUẤT";
        public static string _sopx = "";

        DataTable dt_ctpx;
        DataTable dt_ktton_ctpx;

        public frmPhieuXuat2_Them()
        {
            InitializeComponent();
        }

        private void frmPhieuXuat2_Them_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = _tieude;

                CreateTable_CTPX();
                InitTable_CTPX();

                LoadKhachHang();
                LoadKho();
                LoadHangHoa();
                LoadDVT();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.Trim());
            }
        }

        private void frmPhieuXuat2_Them_Shown(object sender, EventArgs e)
        {
            if (_sopx.Trim() != "")
            {
                // Chuc nang SUA
                lbl_tieude_chucnang.Text = "SỬA DỮ LIỆU";
                LoadTextBox();
                textEdit_sophieuxuat.Enabled = false;
                simpleButton_thoat.Focus();

            }
            else
            {
                // Chuc nang THEM
                lbl_tieude_chucnang.Text = "THÊM MỚI";
                textEdit_sophieuxuat.Enabled = true;
                ClearTextBox();
                dateEdit_ngayxuat.EditValue = DateTime.Now;
                textEdit_lydoxuat.Text = "Bán hàng";

                textEdit_sophieuxuat.Focus();
            }
        }

        private void LoadTextBox()
        {
            var px = (from p in db.PhieuXuats
                      where p.SoPX.Trim().ToUpper() == _sopx.Trim().ToUpper()
                      select p).FirstOrDefault();

            if (px != null)
            {
                #region 1. LOAD PHIEU XUAT
                textEdit_sophieuxuat.Text = px.SoPX.Trim(); ;

                dateEdit_ngayxuat.EditValue = px.NgayXuat;

                searchLookupEdit_khachhang.EditValue = px.MaKH;

                if (px.KhachHang != null)
                {
                    if (px.KhachHang.TenKH != null)
                        textEdit_tenKH.Text = px.KhachHang.TenKH;
                    else textEdit_tenKH.Text = "";

                    if (px.KhachHang.DiaChi != null)
                        textEdit_diachiKH.Text = px.KhachHang.DiaChi;
                    else textEdit_diachiKH.Text = "";
                }

                if (px.MaNV != null)
                    textEdit_manv.Text = px.MaNV.ToString().Trim();
                else textEdit_manv.Text = "";

                textEdit_lydoxuat.Text = px.LyDoXuat;
                searchLookUpEdit_kho.EditValue = px.MaKho;
                textEdit_ghichu.Text = px.GhiChu;

                textEdit_tongsl.EditValue = px.TongSL;
                textEdit_tongtrigia.EditValue = px.TongTien;
                #endregion

                #region 2. LOAD CTPX
                var ctpn = (from q in db.ChiTietPXes
                            join h in db.HangHoas on q.MaHH equals h.MaHH
                            where q.SoPX.Trim().ToUpper() == _sopx.Trim().ToUpper()
                            select new
                            {
                                q.MaHH,
                                h.TenHH,
                                q.MaDVT,
                                q.SoLuong,
                                q.DonGia,
                                q.ThanhTien,
                                q.GhiChu
                            }).ToList();
                if (ctpn != null && ctpn.Count > 0)
                {
                    DataRow r;
                    dt_ctpx.Rows.Clear();
                    foreach (var t in ctpn)
                    {
                        r = dt_ctpx.NewRow();
                        r["MaHH"] = t.MaHH;
                        r["TenHH"] = t.TenHH;
                        r["MaDVT"] = t.MaDVT;

                        if (t.SoLuong != null)
                            r["SoLuong"] = t.SoLuong;
                        else r["SoLuong"] = DBNull.Value;

                        if (t.DonGia != null)
                            r["DonGia"] = t.DonGia;
                        else r["DonGia"] = DBNull.Value;

                        if (t.ThanhTien != null)
                            r["ThanhTien"] = t.ThanhTien;
                        else r["ThanhTien"] = DBNull.Value;

                        r["GhiChu"] = t.GhiChu;
                        dt_ctpx.Rows.Add(r);
                    }

                    if (ctpn.Count < 10)
                    {
                        for (int i = 0; i < 10 - ctpn.Count; i++)
                        {
                            r = dt_ctpx.NewRow();
                            dt_ctpx.Rows.Add(r);
                        }
                    }
                }
                gridControl_ctpx.DataSource = dt_ctpx;
                #endregion
            }
        }

        private void ClearTextBox()
        {
            textEdit_sophieuxuat.Text = "";

            searchLookupEdit_khachhang.EditValue = null;
            textEdit_tenKH.Text = "";
            textEdit_diachiKH.Text = "";

            textEdit_lydoxuat.Text = "";
            searchLookUpEdit_kho.EditValue = null;
            dateEdit_ngayxuat.EditValue = null;
            textEdit_ghichu.Text = "";

            textEdit_tongsl.Text = "0";
            textEdit_tongtrigia.Text = "0";
            textEdit_manv.Text = frmMain._user_id;
        }

        private void CreateTable_CTPX()
        {
            dt_ctpx = new DataTable();

            dt_ctpx.Columns.Add("MaHH", typeof(string));
            dt_ctpx.Columns.Add("TenHH", typeof(string));
            dt_ctpx.Columns.Add("MaDVT", typeof(string));
            dt_ctpx.Columns.Add("SoLuong", typeof(float));
            dt_ctpx.Columns.Add("DonGia", typeof(float));
            dt_ctpx.Columns.Add("ThanhTien", typeof(float));
            dt_ctpx.Columns.Add("GhiChu", typeof(string));
        }

        private void CreateTable_KTTon_CTPX()
        {
            dt_ktton_ctpx = new DataTable();

            dt_ktton_ctpx.Columns.Add("MaHH", typeof(string));
            dt_ktton_ctpx.Columns.Add("MaKho", typeof(string));
            dt_ktton_ctpx.Columns.Add("SoLuong", typeof(float));
            dt_ktton_ctpx.Columns.Add("Status", typeof(bool));
        }

        private void InitTable_CTPX()
        {
            DataRow r;
            for (int i = 0; i < 10; i++)
            {
                r = dt_ctpx.NewRow();
                dt_ctpx.Rows.Add(r);
            }

            gridControl_ctpx.DataSource = dt_ctpx;
        }

        private void LoadKhachHang()
        {
            var data = from p in db.KhachHangs
                       where p.TinhTrang == true
                       select new
                       {
                           p.MaKH,
                           p.TenKH,
                           p.DiaChi
                       };

            searchLookupEdit_khachhang.Properties.DataSource = data;
            searchLookupEdit_khachhang.Properties.ValueMember = "MaKH";
            searchLookupEdit_khachhang.Properties.DisplayMember = "MaKH";
        }

        private void LoadKho()
        {
            var data = from p in db.Khos
                       select new
                       {
                           p.MaKho,
                           p.TenKho,
                           p.DiaChi
                       };

            searchLookUpEdit_kho.Properties.DataSource = data;
            searchLookUpEdit_kho.Properties.ValueMember = "MaKho";
            searchLookUpEdit_kho.Properties.DisplayMember = "MaKho";
        }

        private void LoadHangHoa()
        {
            var data = from p in db.HangHoas
                       where p.TinhTrang == true
                       select new
                       {
                           p.MaHH,
                           p.TenHH,
                           p.LoaiHangHoa.TenLoaiHH
                       };

            repositoryItemLookUpEdit_mahh.DataSource = data;
            repositoryItemLookUpEdit_mahh.ValueMember = "MaHH";
            repositoryItemLookUpEdit_mahh.DisplayMember = "MaHH";
        }

        private void LoadTextBox_KhachHang()
        {
            if (searchLookupEdit_khachhang.Properties.View.GetFocusedRowCellValue("TenKH") != null)
                textEdit_tenKH.Text = searchLookupEdit_khachhang.Properties.View.GetFocusedRowCellValue("TenKH").ToString().Trim();
            else textEdit_tenKH.Text = "";

            if (searchLookupEdit_khachhang.Properties.View.GetFocusedRowCellValue("DiaChi") != null)
                textEdit_diachiKH.Text = searchLookupEdit_khachhang.Properties.View.GetFocusedRowCellValue("DiaChi").ToString().Trim();
            else textEdit_diachiKH.Text = "";
        }

        private void LoadDVT()
        {
            var data = from p in db.DonViTinhs
                       where p.TinhTrang == true
                       select new
                       {
                           p.MaDVT,
                           p.TenDVT
                       };

            repositoryItemLookUpEdit_dvt.DataSource = data;
            repositoryItemLookUpEdit_dvt.ValueMember = "MaDVT";
            repositoryItemLookUpEdit_dvt.DisplayMember = "TenDVT";
        }

        ////////////////////////////////////////////////////
        private void searchLookUpEdit_khachhang_EditValueChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (searchLookupEdit_khachhang.EditValue != null)
            {
                LoadTextBox_KhachHang();
            }
            else
            {
                textEdit_tenKH.Text = "";
                textEdit_diachiKH.Text = "";
            }

            textEdit_lydoxuat.Focus();
            this.Cursor = Cursors.Default;
        }

        private void searchLookUpEdit_khachhang_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            textEdit_tenKH.Text = "";
            textEdit_diachiKH.Text = "";
        }

        private void searchLookUpEdit_khachhang_Leave(object sender, EventArgs e)
        {
            if (searchLookupEdit_khachhang.Text.Trim() == "")
            {
                searchLookupEdit_khachhang.EditValue = null;
                textEdit_tenKH.Text = "";
                textEdit_diachiKH.Text = "";
            }
        }

        private void searchLookUpEdit_kho_EditValueChanged(object sender, EventArgs e)
        {
            dateEdit_ngayxuat.Focus();
        }

        private void textEdit_ghichu_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                gridControl_ctpx.Focus();
                gridView_ctpx.FocusedRowHandle = 0;
                gridView_ctpx.FocusedColumn = gridView_ctpx.Columns["MaHH"];
                gridView_ctpx.ShowEditor();
            }
        }

        ///////////////////////////////////////////////////

        private void repositoryItemLookUpEdit_mahh_EditValueChanged(object sender, EventArgs e)
        {
            gridView_ctpx.SetFocusedRowCellValue("TenHH", "");

            LookUpEdit lookupedit = sender as LookUpEdit;
            if (lookupedit != null && lookupedit.EditValue != null)
            {

                var kh = (from p in db.HangHoas
                          where p.MaHH.Trim().ToUpper() == lookupedit.EditValue.ToString().Trim().ToUpper()
                          select new
                          {
                              p.MaHH,
                              p.TenHH,
                          }).FirstOrDefault();
                if (kh != null)
                {
                    if (kh.TenHH != null)
                        gridView_ctpx.SetFocusedRowCellValue("TenHH", kh.TenHH);
                }
            }
        }

        private void repositoryItemLookUpEdit_mahh_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            gridView_ctpx.SetFocusedRowCellValue("TenHH", "");
            gridView_ctpx.SetFocusedRowCellValue("MaDVT", "");
        }

        private void repositoryItemLookUpEdit_dvt_Leave(object sender, EventArgs e)
        {
            LookUpEdit l = sender as LookUpEdit;
            if (l.Text.Trim() == "")
            {
                l.EditValue = null;
            }
        }

        ////////////////////////////////////////////////////

        private void gridView_ctpn_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column == gridView_ctpx.Columns["SoLuong"])
            {
                if (e.Value != null && e.Value.ToString().Trim() != "")
                {
                    if (gridView_ctpx.GetFocusedRowCellValue("DonGia") != null && gridView_ctpx.GetFocusedRowCellValue("DonGia").ToString().Trim() != "")
                        gridView_ctpx.SetFocusedRowCellValue("ThanhTien", Convert.ToDouble(Convert.ToDouble(e.Value) * Convert.ToDouble(gridView_ctpx.GetFocusedRowCellValue("DonGia").ToString().Trim())));
                    else gridView_ctpx.SetFocusedRowCellValue("ThanhTien", null);
                }
                else
                {
                    gridView_ctpx.SetFocusedRowCellValue("ThanhTien", null);
                }
            }
            else if (e.Column == gridView_ctpx.Columns["DonGia"])
            {
                if (e.Value != null && e.Value.ToString().Trim() != "")
                {
                    if (gridView_ctpx.GetFocusedRowCellValue("SoLuong") != null && gridView_ctpx.GetFocusedRowCellValue("SoLuong").ToString().Trim() != "")
                        gridView_ctpx.SetFocusedRowCellValue("ThanhTien", Convert.ToDouble(Convert.ToDouble(e.Value) * Convert.ToDouble(gridView_ctpx.GetFocusedRowCellValue("SoLuong").ToString().Trim())));
                    else gridView_ctpx.SetFocusedRowCellValue("ThanhTien", null);
                }
                else
                {
                    gridView_ctpx.SetFocusedRowCellValue("ThanhTien", null);
                }
            }
        }

        private void gridView_ctpx_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            UpdateTongSo();
        }

        private void UpdateTongSo()
        {
            double _tongslxuat = 0.0;
            double _tonggtxuat = 0.0;

            for (int i = 0; i < gridView_ctpx.DataRowCount; i++)
            {
                if (gridView_ctpx.GetDataRow(i)["MaHH"] != null && gridView_ctpx.GetDataRow(i)["MaHH"].ToString().Trim() != "")
                {
                    if (gridView_ctpx.GetDataRow(i)["SoLuong"] != null && gridView_ctpx.GetDataRow(i)["SoLuong"].ToString().Trim() != "")
                        _tongslxuat += Convert.ToDouble(gridView_ctpx.GetDataRow(i)["SoLuong"].ToString().Trim());

                    if (gridView_ctpx.GetDataRow(i)["ThanhTien"] != null && gridView_ctpx.GetDataRow(i)["ThanhTien"].ToString().Trim() != "")
                        _tonggtxuat += Convert.ToDouble(gridView_ctpx.GetDataRow(i)["ThanhTien"].ToString().Trim());
                }
            }

            textEdit_tongsl.EditValue = _tongslxuat;
            textEdit_tongtrigia.EditValue = _tonggtxuat;
        }

        /// <summary>
        /// ////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton_luu_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            using (DataClasses_QLKHOHANGDataContext _db = new DataClasses_QLKHOHANGDataContext()) // su dung cuc bo - db duoc su dung trong pham vi using
            {
                System.Data.Common.DbTransaction trans = null;
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;
                try
                {
                    if (_sopx.Trim() == "")
                    {
                        #region Them moi
                        if (textEdit_sophieuxuat.Text.Trim() == "")
                        {
                            MessageBox.Show("Vui lòng nhập số phiếu xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textEdit_sophieuxuat.Focus();

                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else if (searchLookupEdit_khachhang.EditValue == null || searchLookupEdit_khachhang.EditValue.ToString().Trim() == "")
                        {
                            MessageBox.Show("Vui lòng nhập thông tin khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            searchLookupEdit_khachhang.Focus();

                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else if (searchLookUpEdit_kho.EditValue == null || searchLookUpEdit_kho.EditValue.ToString().Trim() == "")
                        {
                            MessageBox.Show("Vui lòng nhập thông tin kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            searchLookUpEdit_kho.Focus();

                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else if (dateEdit_ngayxuat.EditValue == null || dateEdit_ngayxuat.Text.Trim() == "")
                        {
                            MessageBox.Show("Vui lòng nhập ngày xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dateEdit_ngayxuat.Focus();

                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else // SAVE
                        {
                            // 1. KIEM TRA TRUNG MA
                            var data = from p in _db.PhieuXuats
                                       where p.SoPX.Trim().ToUpper() == textEdit_sophieuxuat.Text.Trim().ToUpper()
                                       select p;
                            if (data.Count() > 0)
                            {
                                MessageBox.Show("Trùng số phiếu xuất, vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                textEdit_sophieuxuat.Focus();

                                this.Cursor = Cursors.Default;
                                return;
                            }
                            else
                            {
                                #region 1. INSERT PHIEU XUAT
                                PhieuXuat px = new PhieuXuat();
                                px.SoPX = textEdit_sophieuxuat.Text.Trim();

                                if (searchLookupEdit_khachhang.EditValue != null)
                                    px.MaKH = searchLookupEdit_khachhang.EditValue.ToString().Trim();
                                else px.MaKH = null;

                                if (searchLookUpEdit_kho.EditValue != null)
                                    px.MaKho = searchLookUpEdit_kho.EditValue.ToString().Trim();
                                else px.MaKho = null;

                                px.LyDoXuat = textEdit_lydoxuat.Text.Trim();

                                if (dateEdit_ngayxuat.EditValue != null && dateEdit_ngayxuat.Text.Trim() != "")
                                    px.NgayXuat = dateEdit_ngayxuat.DateTime;
                                else px.NgayXuat = DateTime.Now;

                                if (textEdit_tongsl.Text.Trim() != "")
                                    px.TongSL = Convert.ToDouble(textEdit_tongsl.Text.Trim());
                                else px.TongSL = 0;

                                if (textEdit_tongtrigia.Text.Trim() != "")
                                    px.TongTien = Convert.ToDouble(textEdit_tongtrigia.Text.Trim());
                                else px.TongTien = 0;

                                px.TrangThai = 1;

                                px.MaNV = frmMain._user_id.Trim();
                                px.GhiChu = textEdit_ghichu.Text.Trim();

                                _db.PhieuXuats.InsertOnSubmit(px);
                                #endregion

                                #region 2. INSERT CTPX
                                for (int i = 0; i < gridView_ctpx.DataRowCount; i++)
                                {
                                    if (gridView_ctpx.GetRowCellValue(i, "MaHH").ToString().Trim() != "")
                                    {
                                        ChiTietPX ctpx = new ChiTietPX();

                                        ctpx.SoPX = px.SoPX;
                                        ctpx.MaHH = gridView_ctpx.GetRowCellValue(i, "MaHH").ToString().Trim();

                                        if (gridView_ctpx.GetRowCellValue(i, "MaDVT") != null && gridView_ctpx.GetRowCellValue(i, "MaDVT").ToString().Trim() != "")
                                            ctpx.MaDVT = gridView_ctpx.GetRowCellValue(i, "MaDVT").ToString().Trim();
                                        else ctpx.MaDVT = null;

                                        if (gridView_ctpx.GetRowCellValue(i, "SoLuong") != null && gridView_ctpx.GetRowCellValue(i, "SoLuong").ToString().Trim() != "")
                                            ctpx.SoLuong = Convert.ToDouble(gridView_ctpx.GetRowCellValue(i, "SoLuong").ToString().Trim());
                                        else ctpx.SoLuong = 0;

                                        if (gridView_ctpx.GetRowCellValue(i, "DonGia") != null && gridView_ctpx.GetRowCellValue(i, "DonGia").ToString().Trim() != "")
                                            ctpx.DonGia = Convert.ToDouble(gridView_ctpx.GetRowCellValue(i, "DonGia").ToString().Trim());
                                        else ctpx.DonGia = 0;

                                        ctpx.GhiChu = "";

                                        _db.ChiTietPXes.InsertOnSubmit(ctpx);
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region Sua du lieu
                        if (_sopx.Trim() == "" || textEdit_sophieuxuat.Text.Trim() == "")
                        {
                            MessageBox.Show("Không xác định được đối tượng cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else if (searchLookupEdit_khachhang.EditValue == null || searchLookupEdit_khachhang.EditValue.ToString().Trim() == "")
                        {
                            MessageBox.Show("Vui lòng nhập thông tin khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            searchLookupEdit_khachhang.Focus();

                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else if (searchLookUpEdit_kho.EditValue == null || searchLookUpEdit_kho.EditValue.ToString().Trim() == "")
                        {
                            MessageBox.Show("Vui lòng nhập thông tin kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            searchLookUpEdit_kho.Focus();

                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else if (dateEdit_ngayxuat.EditValue == null || dateEdit_ngayxuat.Text.Trim() == "")
                        {
                            MessageBox.Show("Vui lòng nhập ngày xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dateEdit_ngayxuat.Focus();

                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else // SAVE
                        {
                            #region 1. UPDATE PHIEU XUAT
                            var px = (from p in _db.PhieuXuats
                                      where p.SoPX.Trim().ToUpper() == _sopx.Trim().ToUpper()
                                      select p).FirstOrDefault();
                            if (px != null)
                            {
                                // px.SoPX = textEdit_sophieuxuat.Text.Trim();

                                if (searchLookupEdit_khachhang.EditValue != null)
                                    px.MaKH = searchLookupEdit_khachhang.EditValue.ToString().Trim();
                                else px.MaKH = null;

                                if (searchLookUpEdit_kho.EditValue != null)
                                    px.MaKho = searchLookUpEdit_kho.EditValue.ToString().Trim();
                                else px.MaKho = null;

                                px.LyDoXuat = textEdit_lydoxuat.Text.Trim();

                                if (dateEdit_ngayxuat.EditValue != null && dateEdit_ngayxuat.Text.Trim() != "")
                                    px.NgayXuat = dateEdit_ngayxuat.DateTime;
                                else px.NgayXuat = DateTime.Now;

                                if (textEdit_tongsl.Text.Trim() != "")
                                    px.TongSL = Convert.ToDouble(textEdit_tongsl.Text.Trim());
                                else px.TongSL = 0;

                                if (textEdit_tongtrigia.Text.Trim() != "")
                                    px.TongTien = Convert.ToDouble(textEdit_tongtrigia.Text.Trim());
                                else px.TongTien = 0;

                                px.MaNV = frmMain._user_id.Trim();
                                px.GhiChu = textEdit_ghichu.Text.Trim();

                                #region 2. DELETE CTPX OLD

                                var ctpx_old = from p in _db.ChiTietPXes
                                               where p.SoPX == px.SoPX
                                               select p;
                                if (ctpx_old != null)
                                    _db.ChiTietPXes.DeleteAllOnSubmit(ctpx_old);

                                #endregion

                                #region 3. INSERT CTPX NEW
                                for (int i = 0; i < gridView_ctpx.DataRowCount; i++)
                                {
                                    if (gridView_ctpx.GetRowCellValue(i, "MaHH").ToString().Trim() != "")
                                    {
                                        ChiTietPX ctpx = new ChiTietPX();

                                        ctpx.SoPX = px.SoPX;
                                        ctpx.MaHH = gridView_ctpx.GetRowCellValue(i, "MaHH").ToString().Trim();

                                        if (gridView_ctpx.GetRowCellValue(i, "MaDVT") != null && gridView_ctpx.GetRowCellValue(i, "MaDVT").ToString().Trim() != "")
                                            ctpx.MaDVT = gridView_ctpx.GetRowCellValue(i, "MaDVT").ToString().Trim();
                                        else ctpx.MaDVT = null;

                                        if (gridView_ctpx.GetRowCellValue(i, "SoLuong") != null && gridView_ctpx.GetRowCellValue(i, "SoLuong").ToString().Trim() != "")
                                            ctpx.SoLuong = Convert.ToDouble(gridView_ctpx.GetRowCellValue(i, "SoLuong").ToString().Trim());
                                        else ctpx.SoLuong = 0;

                                        if (gridView_ctpx.GetRowCellValue(i, "DonGia") != null && gridView_ctpx.GetRowCellValue(i, "DonGia").ToString().Trim() != "")
                                            ctpx.DonGia = Convert.ToDouble(gridView_ctpx.GetRowCellValue(i, "DonGia").ToString().Trim());
                                        else ctpx.DonGia = 0;

                                        ctpx.GhiChu = "";

                                        _db.ChiTietPXes.InsertOnSubmit(ctpx);
                                    }
                                }
                                #endregion

                            }
                            else
                            {
                                MessageBox.Show("Không xác định được đối tượng cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            #endregion
                        }
                        #endregion
                    }

                    _db.SubmitChanges();
                    trans.Commit();
                    MessageBox.Show("Lưu dữ liệu thành công");
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();

                    MessageBox.Show("Thao tác thất bại!. \n Mã lỗi: " + ex.Message.Trim(), "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    _db.Connection.Close();
                }
            }
            this.Cursor = Cursors.Default;
        }
        private void simpleButton_thoat_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Close();
            this.Cursor = Cursors.Default;
        }

        private void repositoryItemSpinEdit_soluong_Validating(object sender, CancelEventArgs e)
        {
            SpinEdit spin = sender as SpinEdit;
            if (spin.EditValue != null && spin.Value < 0)
            {
                e.Cancel = true;
            }
        }

        private void repositoryItemSpinEdit_dongia_Validating(object sender, CancelEventArgs e)
        {
            SpinEdit spin = sender as SpinEdit;
            if (spin.EditValue != null && spin.Value < 0)
            {
                e.Cancel = true;
            }
        }
    }
}