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
    public partial class frmPhieuNhap2_Them : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();

        public static string _tieude = "PHIẾU NHẬP";
        public static string _sopn = "";

        DataTable dt_ctpn;
        DataTable dt_ktton_ctpn;

        public frmPhieuNhap2_Them()
        {
            InitializeComponent();
        }

        private void frmPhieuNhap_Them_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = _tieude;

                CreateTable_CTPN();
                InitTable_CTPN();

                LoadNhaCC();
                LoadKho();
                LoadHangHoa();
                LoadDVT();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.Trim());
            }
        }

        private void frmPhieuNhap_Them_Shown(object sender, EventArgs e)
        {
            if (_sopn.Trim() != "")
            {
                // Chuc nang SUA
                lbl_tieude_chucnang.Text = "SỬA DỮ LIỆU";
                LoadTextBox();
                textEdit_sophieunhap.Enabled = false;
                simpleButton_thoat.Focus();

            }
            else
            {
                // Chuc nang THEM
                lbl_tieude_chucnang.Text = "THÊM MỚI";
                textEdit_sophieunhap.Enabled = true;
                ClearTextBox();
                dateEdit_ngaynhap.EditValue = DateTime.Now;
                textEdit_lydonhap.Text = "Nhập kho";

                textEdit_sophieunhap.Focus();
            }
        }

        private void LoadTextBox()
        {
            var pn = (from p in db.PhieuNhaps
                      where p.SoPN.Trim().ToUpper() == _sopn.Trim().ToUpper() && p.TinhTrang == true
                      select new
                     {
                         p.SoPN,
                         p.MaNCC,
                         p.NgayNhap,
                         p.NhaCC.TenNCC,
                         p.NhaCC.DiaChi,
                         p.LyDoNhap,
                         p.MaKho,
                         p.MaNV,
                         p.TongSL,
                         p.TongTriGia,
                         p.GhiChu,
                         p.TinhTrang
                     }).FirstOrDefault();

            if (pn != null)
            {
                #region 1. LOAD PHIEU NHAP
                textEdit_sophieunhap.Text = pn.SoPN.Trim(); ;

                dateEdit_ngaynhap.EditValue = pn.NgayNhap;

                searchLookupEdit_nhacungcap.EditValue = pn.MaNCC;
                textEdit_tenNCC.Text = pn.TenNCC;
                textEdit_diachiNCC.Text = pn.DiaChi;

                if (pn.MaNV != null)
                    textEdit_manv.Text = pn.MaNV.ToString().Trim();
                else textEdit_manv.Text = "";

                textEdit_lydonhap.Text = pn.LyDoNhap;
                searchLookUpEdit_kho.EditValue = pn.MaKho;
                textEdit_ghichu.Text = pn.GhiChu;

                textEdit_tongsl.EditValue = pn.TongSL;
                textEdit_tonggiatri.EditValue = pn.TongTriGia;
                #endregion

                #region 2. LOAD CTPN
                var ctpn = (from q in db.ChiTietPNs
                            join h in db.HangHoas on q.MaHH equals h.MaHH
                            where q.SoPN.Trim().ToUpper() == _sopn.Trim().ToUpper()
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
                    dt_ctpn.Rows.Clear();
                    foreach (var t in ctpn)
                    {
                        r = dt_ctpn.NewRow();
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
                        dt_ctpn.Rows.Add(r);
                    }

                    if (ctpn.Count < 10)
                    {
                        for (int i = 0; i < 10 - ctpn.Count; i++)
                        {
                            r = dt_ctpn.NewRow();
                            dt_ctpn.Rows.Add(r);
                        }
                    }
                }
                gridControl_ctpn.DataSource = dt_ctpn;
                #endregion
            }
        }

        private void ClearTextBox()
        {
            textEdit_sophieunhap.Text = "";

            searchLookupEdit_nhacungcap.EditValue = null;
            textEdit_tenNCC.Text = "";
            textEdit_diachiNCC.Text = "";

            textEdit_lydonhap.Text = "";
            searchLookUpEdit_kho.EditValue = null;
            dateEdit_ngaynhap.EditValue = null;
            textEdit_ghichu.Text = "";

            textEdit_tongsl.Text = "0";
            textEdit_tonggiatri.Text = "0";
            textEdit_manv.Text = frmMain._user_id;
        }

        private void CreateTable_CTPN()
        {
            dt_ctpn = new DataTable();

            dt_ctpn.Columns.Add("MaHH", typeof(string));
            dt_ctpn.Columns.Add("TenHH", typeof(string));
            dt_ctpn.Columns.Add("MaDVT", typeof(string));
            dt_ctpn.Columns.Add("SoLuong", typeof(float));
            dt_ctpn.Columns.Add("DonGia", typeof(float));
            dt_ctpn.Columns.Add("ThanhTien", typeof(float));
            dt_ctpn.Columns.Add("GhiChu", typeof(string));
        }

        private void CreateTable_KTTon_CTPX()
        {
            dt_ktton_ctpn = new DataTable();

            dt_ktton_ctpn.Columns.Add("MaHH", typeof(string));
            dt_ktton_ctpn.Columns.Add("MaKho", typeof(string));
            dt_ktton_ctpn.Columns.Add("SoLuong", typeof(float));
            dt_ktton_ctpn.Columns.Add("Status", typeof(bool));
        }

        private void InitTable_CTPN()
        {
            DataRow r;
            for (int i = 0; i < 10; i++)
            {
                r = dt_ctpn.NewRow();
                dt_ctpn.Rows.Add(r);
            }

            gridControl_ctpn.DataSource = dt_ctpn;
        }

        private void LoadNhaCC()
        {
            var data = from p in db.NhaCCs
                       where p.TinhTrang == true
                       select new
                       {
                           p.MaNCC,
                           p.TenNCC,
                           p.DiaChi
                       };

            searchLookupEdit_nhacungcap.Properties.DataSource = data;
            searchLookupEdit_nhacungcap.Properties.ValueMember = "MaNCC";
            searchLookupEdit_nhacungcap.Properties.DisplayMember = "MaNCC";
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

        private void LoadTextBox_NhaCC()
        {
            if (searchLookupEdit_nhacungcap.Properties.View.GetFocusedRowCellValue("TenNCC") != null)
                textEdit_tenNCC.Text = searchLookupEdit_nhacungcap.Properties.View.GetFocusedRowCellValue("TenNCC").ToString().Trim();
            else textEdit_tenNCC.Text = "";

            if (searchLookupEdit_nhacungcap.Properties.View.GetFocusedRowCellValue("DiaChi") != null)
                textEdit_diachiNCC.Text = searchLookupEdit_nhacungcap.Properties.View.GetFocusedRowCellValue("DiaChi").ToString().Trim();
            else textEdit_diachiNCC.Text = "";
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
        private void searchLookUpEdit_nhacungcap_EditValueChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (searchLookupEdit_nhacungcap.EditValue != null)
            {
                LoadTextBox_NhaCC();
            }
            else
            {
                textEdit_tenNCC.Text = "";
                textEdit_diachiNCC.Text = "";
            }

            textEdit_lydonhap.Focus();
            this.Cursor = Cursors.Default;
        }

        private void searchLookUpEdit_nhacungcap_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            textEdit_tenNCC.Text = "";
            textEdit_diachiNCC.Text = "";
        }

        private void searchLookUpEdit_nhacungcap_Leave(object sender, EventArgs e)
        {
            if (searchLookupEdit_nhacungcap.Text.Trim() == "")
            {
                searchLookupEdit_nhacungcap.EditValue = null;
                textEdit_tenNCC.Text = "";
                textEdit_diachiNCC.Text = "";
            }
        }

        //////////////////////////////////////////////////
        private void searchLookUpEdit_kho_EditValueChanged(object sender, EventArgs e)
        {
            dateEdit_ngaynhap.Focus();
        }

        private void textEdit_ghichu_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                gridControl_ctpn.Focus();
                gridView_ctpn.FocusedRowHandle = 0;
                gridView_ctpn.FocusedColumn = gridView_ctpn.Columns["MaHH"];
                gridView_ctpn.ShowEditor();
            }
        }

        ///////////////////////////////////////////////////

        private void repositoryItemLookUpEdit_mahh_EditValueChanged(object sender, EventArgs e)
        {
            gridView_ctpn.SetFocusedRowCellValue("TenHH", "");

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
                        gridView_ctpn.SetFocusedRowCellValue("TenHH", kh.TenHH);
                }
            }
        }

        private void repositoryItemLookUpEdit_mahh_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            gridView_ctpn.SetFocusedRowCellValue("TenHH", "");
            gridView_ctpn.SetFocusedRowCellValue("MaDVT", "");
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
            if (e.Column == gridView_ctpn.Columns["SoLuong"])
            {
                if (e.Value != null && e.Value.ToString().Trim() != "")
                {
                    if (gridView_ctpn.GetFocusedRowCellValue("DonGia") != null && gridView_ctpn.GetFocusedRowCellValue("DonGia").ToString().Trim() != "")
                        gridView_ctpn.SetFocusedRowCellValue("ThanhTien", Convert.ToDouble(Convert.ToDouble(e.Value) * Convert.ToDouble(gridView_ctpn.GetFocusedRowCellValue("DonGia").ToString().Trim())));
                    else gridView_ctpn.SetFocusedRowCellValue("ThanhTien", null);
                }
                else
                {
                    gridView_ctpn.SetFocusedRowCellValue("ThanhTien", null);
                }
            }
            else if (e.Column == gridView_ctpn.Columns["DonGia"])
            {
                if (e.Value != null && e.Value.ToString().Trim() != "")
                {
                    if (gridView_ctpn.GetFocusedRowCellValue("SoLuong") != null && gridView_ctpn.GetFocusedRowCellValue("SoLuong").ToString().Trim() != "")
                        gridView_ctpn.SetFocusedRowCellValue("ThanhTien", Convert.ToDouble(Convert.ToDouble(e.Value) * Convert.ToDouble(gridView_ctpn.GetFocusedRowCellValue("SoLuong").ToString().Trim())));
                    else gridView_ctpn.SetFocusedRowCellValue("ThanhTien", null);
                }
                else
                {
                    gridView_ctpn.SetFocusedRowCellValue("ThanhTien", null);
                }
            }
        }

        private void gridView_ctpn_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            UpdateTongSo();
        }

        private void UpdateTongSo()
        {
            double _tongslnhap = 0.0;
            double _tonggtnhap = 0.0;

            for (int i = 0; i < gridView_ctpn.DataRowCount; i++)
            {
                if (gridView_ctpn.GetDataRow(i)["MaHH"] != null && gridView_ctpn.GetDataRow(i)["MaHH"].ToString().Trim() != "")
                {
                    if (gridView_ctpn.GetDataRow(i)["SoLuong"] != null && gridView_ctpn.GetDataRow(i)["SoLuong"].ToString().Trim() != "")
                        _tongslnhap += Convert.ToDouble(gridView_ctpn.GetDataRow(i)["SoLuong"].ToString().Trim());

                    if (gridView_ctpn.GetDataRow(i)["ThanhTien"] != null && gridView_ctpn.GetDataRow(i)["ThanhTien"].ToString().Trim() != "")
                        _tonggtnhap += Convert.ToDouble(gridView_ctpn.GetDataRow(i)["ThanhTien"].ToString().Trim());
                }
            }


            textEdit_tongsl.EditValue = _tongslnhap;
            textEdit_tonggiatri.EditValue = _tonggtnhap;
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
                    if (_sopn.Trim() == "")
                    {
                        #region Them moi
                        if (textEdit_sophieunhap.Text.Trim() == "")
                        {
                            MessageBox.Show("Vui lòng nhập số phiếu nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textEdit_sophieunhap.Focus();

                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else if (searchLookupEdit_nhacungcap.EditValue == null || searchLookupEdit_nhacungcap.EditValue.ToString().Trim() == "")
                        {
                            MessageBox.Show("Vui lòng nhập thông tin nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            searchLookupEdit_nhacungcap.Focus();

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
                        else if (dateEdit_ngaynhap.EditValue == null || dateEdit_ngaynhap.Text.Trim() == "")
                        {
                            MessageBox.Show("Vui lòng nhập ngày nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dateEdit_ngaynhap.Focus();

                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else // SAVE
                        {
                            // 1. KIEM TRA TRUNG MA
                            var data = from p in _db.PhieuNhaps
                                       where p.SoPN.Trim().ToUpper() == textEdit_sophieunhap.Text.Trim().ToUpper()
                                       select p;
                            if (data.Count() > 0)
                            {
                                MessageBox.Show("Trùng số phiếu nhập, vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                textEdit_sophieunhap.Focus();

                                this.Cursor = Cursors.Default;
                                return;
                            }
                            else
                            {
                                #region 1. INSERT PHIEU NHAP
                                PhieuNhap pn = new PhieuNhap();
                                pn.SoPN = textEdit_sophieunhap.Text.Trim();

                                if (searchLookupEdit_nhacungcap.EditValue != null)
                                    pn.MaNCC = searchLookupEdit_nhacungcap.EditValue.ToString().Trim();
                                else pn.MaNCC = null;

                                if (searchLookUpEdit_kho.EditValue != null)
                                    pn.MaKho = searchLookUpEdit_kho.EditValue.ToString().Trim();
                                else pn.MaKho = null;

                                pn.LyDoNhap = textEdit_lydonhap.Text.Trim();

                                if (dateEdit_ngaynhap.EditValue != null && dateEdit_ngaynhap.Text.Trim() != "")
                                    pn.NgayNhap = dateEdit_ngaynhap.DateTime;
                                else pn.NgayNhap = DateTime.Now;

                                if (textEdit_tongsl.Text.Trim() != "")
                                    pn.TongSL = Convert.ToDouble(textEdit_tongsl.Text.Trim());
                                else pn.TongSL = null;

                                if (textEdit_tonggiatri.Text.Trim() != "")
                                    pn.TongTriGia = Convert.ToDouble(textEdit_tonggiatri.Text.Trim());
                                else pn.TongTriGia = null;

                                pn.MaNV = frmMain._user_id.Trim();
                                pn.GhiChu = textEdit_ghichu.Text.Trim();
                                pn.TinhTrang = true;

                                _db.PhieuNhaps.InsertOnSubmit(pn);
                                #endregion

                                #region 2. INSERT CTPN
                                for (int i = 0; i < gridView_ctpn.DataRowCount; i++)
                                {
                                    if (gridView_ctpn.GetRowCellValue(i, "MaHH").ToString().Trim() != "")
                                    {
                                        ChiTietPN ctpn = new ChiTietPN();

                                        ctpn.SoPN = pn.SoPN;
                                        ctpn.MaHH = gridView_ctpn.GetRowCellValue(i, "MaHH").ToString().Trim();

                                        if (gridView_ctpn.GetRowCellValue(i, "MaDVT") != null && gridView_ctpn.GetRowCellValue(i, "MaDVT").ToString().Trim() != "")
                                            ctpn.MaDVT = gridView_ctpn.GetRowCellValue(i, "MaDVT").ToString().Trim();
                                        else ctpn.MaDVT = null;

                                        if (gridView_ctpn.GetRowCellValue(i, "SoLuong") != null && gridView_ctpn.GetRowCellValue(i, "SoLuong").ToString().Trim() != "")
                                            ctpn.SoLuong = Convert.ToDouble(gridView_ctpn.GetRowCellValue(i, "SoLuong").ToString().Trim());
                                        else ctpn.SoLuong = 0;

                                        if (gridView_ctpn.GetRowCellValue(i, "DonGia") != null && gridView_ctpn.GetRowCellValue(i, "DonGia").ToString().Trim() != "")
                                            ctpn.DonGia = Convert.ToDouble(gridView_ctpn.GetRowCellValue(i, "DonGia").ToString().Trim());
                                        else ctpn.DonGia = 0;

                                        ctpn.GhiChu = "";

                                        _db.ChiTietPNs.InsertOnSubmit(ctpn);
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
                        if (_sopn.Trim() == "" || textEdit_sophieunhap.Text.Trim() == "")
                        {
                            MessageBox.Show("Không xác định được đối tượng cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else if (searchLookupEdit_nhacungcap.EditValue == null || searchLookupEdit_nhacungcap.EditValue.ToString().Trim() == "")
                        {
                            MessageBox.Show("Vui lòng nhập thông tin nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            searchLookupEdit_nhacungcap.Focus();

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
                        else if (dateEdit_ngaynhap.EditValue == null || dateEdit_ngaynhap.Text.Trim() == "")
                        {
                            MessageBox.Show("Vui lòng nhập ngày nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dateEdit_ngaynhap.Focus();

                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else // SAVE
                        {
                            #region 1. UPDATE PHIEU NHAP
                            var pn = (from p in _db.PhieuNhaps
                                      where p.SoPN.Trim().ToUpper() == _sopn.Trim().ToUpper()
                                      select p).FirstOrDefault();
                            if (pn != null)
                            {
                                // pn.SoPN = textEdit_sophieunhap.Text.Trim();

                                if (searchLookupEdit_nhacungcap.EditValue != null)
                                    pn.MaNCC = searchLookupEdit_nhacungcap.EditValue.ToString().Trim();
                                else pn.MaNCC = null;

                                if (searchLookUpEdit_kho.EditValue != null)
                                    pn.MaKho = searchLookUpEdit_kho.EditValue.ToString().Trim();
                                else pn.MaKho = null;

                                pn.LyDoNhap = textEdit_lydonhap.Text.Trim();

                                if (dateEdit_ngaynhap.EditValue != null && dateEdit_ngaynhap.Text.Trim() != "")
                                    pn.NgayNhap = dateEdit_ngaynhap.DateTime;
                                else pn.NgayNhap = DateTime.Now;

                                if (textEdit_tongsl.Text.Trim() != "")
                                    pn.TongSL = Convert.ToDouble(textEdit_tongsl.Text.Trim());
                                else pn.TongSL = null;

                                if (textEdit_tonggiatri.Text.Trim() != "")
                                    pn.TongTriGia = Convert.ToDouble(textEdit_tonggiatri.Text.Trim());
                                else pn.TongTriGia = null;

                                pn.MaNV = frmMain._user_id.Trim();
                                pn.GhiChu = textEdit_ghichu.Text.Trim();
                                pn.TinhTrang = true;

                                #region 2. DELETE CTPN OLD

                                var ctpn_old = from p in _db.ChiTietPNs
                                               where p.SoPN == pn.SoPN
                                               select p;
                                if (ctpn_old != null)
                                    _db.ChiTietPNs.DeleteAllOnSubmit(ctpn_old);

                                #endregion

                                #region 3. INSERT CTPN NEW
                                for (int i = 0; i < gridView_ctpn.DataRowCount; i++)
                                {
                                    if (gridView_ctpn.GetRowCellValue(i, "MaHH").ToString().Trim() != "")
                                    {
                                        ChiTietPN ctpn = new ChiTietPN();

                                        ctpn.SoPN = pn.SoPN;
                                        ctpn.MaHH = gridView_ctpn.GetRowCellValue(i, "MaHH").ToString().Trim();

                                        if (gridView_ctpn.GetRowCellValue(i, "MaDVT") != null && gridView_ctpn.GetRowCellValue(i, "MaDVT").ToString().Trim() != "")
                                            ctpn.MaDVT = gridView_ctpn.GetRowCellValue(i, "MaDVT").ToString().Trim();
                                        else ctpn.MaDVT = null;

                                        if (gridView_ctpn.GetRowCellValue(i, "SoLuong") != null && gridView_ctpn.GetRowCellValue(i, "SoLuong").ToString().Trim() != "")
                                            ctpn.SoLuong = Convert.ToDouble(gridView_ctpn.GetRowCellValue(i, "SoLuong").ToString().Trim());
                                        else ctpn.SoLuong = 0;

                                        if (gridView_ctpn.GetRowCellValue(i, "DonGia") != null && gridView_ctpn.GetRowCellValue(i, "DonGia").ToString().Trim() != "")
                                            ctpn.DonGia = Convert.ToDouble(gridView_ctpn.GetRowCellValue(i, "DonGia").ToString().Trim());
                                        else ctpn.DonGia = 0;

                                        ctpn.GhiChu = "";

                                        _db.ChiTietPNs.InsertOnSubmit(ctpn);
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