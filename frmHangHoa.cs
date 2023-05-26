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
using DevExpress.XtraGrid;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace QLKHOHANG
{
    public partial class frmHangHoa : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();

        public frmHangHoa()
        {
            InitializeComponent();
        }

        private void frmHangHoa_Load(object sender, EventArgs e)
        {
            try
            {
                LoadLoaiHH();

                OnOffTextBox(true);
                LoadLuoi();
                OnOffSuaXoa();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadLoaiHH()
        {
            var data = from p in db.LoaiHangHoas
                       where p.TinhTrang == true
                       select p;

            searchLookUpEdit_loaihanghoa.Properties.DataSource = data;
            searchLookUpEdit_loaihanghoa.Properties.ValueMember = "MaLoaiHH";
            searchLookUpEdit_loaihanghoa.Properties.DisplayMember = "TenLoaiHH";
        }

        private void OnOffTextBox(bool _val)
        {
            textEdit_mahh.Properties.ReadOnly = _val;
            textEdit_tenhanghoa.Properties.ReadOnly = _val;
            searchLookUpEdit_loaihanghoa.Properties.ReadOnly = _val;

            textEdit_xuatxu.Properties.ReadOnly = _val;
            textEdit_mausac.Properties.ReadOnly = _val;
            textEdit_kichthuoc.Properties.ReadOnly = _val;

            spinEdit_slton_toithieu.Properties.ReadOnly = _val;
            spinEdit_slton_toida.Properties.ReadOnly = _val;
        }

        private void LoadLuoi()
        {
            db = new DataClasses_QLKHOHANGDataContext();
            var data = from p in db.HangHoas
                       where p.TinhTrang == true
                       select new
                       {
                           p.MaHH,
                           p.TenHH,
                           p.MaLoaiHH,
                           p.XuatXu,
                           p.QuyCach,
                           p.MauSac,
                           p.KichThuoc,
                           p.SLTonToiThieu,
                           p.SLTonToiDa,
                           p.GhiChu,
                           p.TinhTrang
                       };
            if (data != null && data.ToList().Count > 0)
                gridControl_hanghoa.DataSource = data;
            else gridControl_hanghoa.DataSource = null;
        }

        private void OnOffSuaXoa()
        {
            if (gridControl_hanghoa.DataSource != null && gridView_hanghoa.DataRowCount > 0)
            {
                simpleButton_sua.Enabled = true;
                simpleButton_xoa.Enabled = true;
            }
            else
            {
                simpleButton_sua.Enabled = false;
                simpleButton_xoa.Enabled = false;
            }
        }
        private void simpleButton_thoat_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Close();
            this.Cursor = Cursors.Default;
        }

        private void gridView_hh_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gridView_hanghoa.FocusedRowHandle != GridControl.AutoFilterRowHandle)
                {
                    ClearTextBox();
                    LoadTextBox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ClearTextBox()
        {
            textEdit_mahh.Text = "";
            textEdit_tenhanghoa.Text = "";
            searchLookUpEdit_loaihanghoa.EditValue = null;

            textEdit_xuatxu.Text = "";
            textEdit_mausac.Text = "";
            textEdit_kichthuoc.Text = "";

            spinEdit_slton_toida.EditValue = null;
            spinEdit_slton_toithieu.EditValue = null;
        }
        private void LoadTextBox()
        {
            if (gridView_hanghoa.DataRowCount > 0 && gridView_hanghoa.FocusedRowHandle != GridControl.AutoFilterRowHandle && gridView_hanghoa.GetFocusedRowCellValue("MaHH") != null)
            {
                textEdit_mahh.Text = gridView_hanghoa.GetFocusedRowCellValue("MaHH").ToString().Trim();

                if (gridView_hanghoa.GetFocusedRowCellValue("TenHH") != null)
                    textEdit_tenhanghoa.Text = gridView_hanghoa.GetFocusedRowCellValue("TenHH").ToString().Trim();
                else textEdit_tenhanghoa.Text = "";

                searchLookUpEdit_loaihanghoa.EditValue = gridView_hanghoa.GetFocusedRowCellValue("MaLoaiHH");

                if (gridView_hanghoa.GetFocusedRowCellValue("XuatXu") != null)
                    textEdit_xuatxu.Text = gridView_hanghoa.GetFocusedRowCellValue("XuatXu").ToString().Trim();
                else textEdit_xuatxu.Text = "";

                if (gridView_hanghoa.GetFocusedRowCellValue("MauSac") != null)
                    textEdit_mausac.Text = gridView_hanghoa.GetFocusedRowCellValue("MauSac").ToString().Trim();
                else textEdit_mausac.Text = "";

                if (gridView_hanghoa.GetFocusedRowCellValue("KichThuoc") != null)
                    textEdit_kichthuoc.Text = gridView_hanghoa.GetFocusedRowCellValue("KichThuoc").ToString().Trim();
                else textEdit_kichthuoc.Text = "";


                spinEdit_slton_toithieu.EditValue = gridView_hanghoa.GetFocusedRowCellValue("SLTonToiThieu");
                spinEdit_slton_toida.EditValue = gridView_hanghoa.GetFocusedRowCellValue("SLTonToiDa");
            }
            else
            {
                ClearTextBox();
            }
        }

        private void simpleButton_them_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (simpleButton_them.Text.ToUpper() == "THÊM")
                {
                    OnOffTextBox(false);
                    ClearTextBox();

                    simpleButton_sua.Enabled = false;
                    simpleButton_refesh.Enabled = false;

                    simpleButton_them.Text = "LƯU";
                    simpleButton_them.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    simpleButton_them.ForeColor = Color.Red;

                    simpleButton_xoa.Enabled = true;
                    simpleButton_xoa.Text = "HỦY";
                    simpleButton_xoa.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    simpleButton_xoa.ForeColor = Color.Red;

                    textEdit_mahh.Focus();
                }
                else // LUU
                {
                    if (textEdit_mahh.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập mã hàng hóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_mahh.Focus();
                    }
                    else if (textEdit_tenhanghoa.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập tên hàng hóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_tenhanghoa.Focus();
                    }
                    else if (searchLookUpEdit_loaihanghoa.EditValue == null || searchLookUpEdit_loaihanghoa.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập tên loại hàng hóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        searchLookUpEdit_loaihanghoa.Focus();
                    }
                    else // Save Database
                    {
                        // 1. KTRA TRUNG MA HANG HOA
                        db = new DataClasses_QLKHOHANGDataContext();
                        var data = from p in db.HangHoas
                                   where p.MaHH.Trim().ToUpper() == textEdit_mahh.Text.Trim().ToUpper()
                                   select p;
                        if (data.Count() > 0)
                        {
                            MessageBox.Show("Trùng mã hàng hóa, vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textEdit_mahh.Focus();
                        }
                        else
                        {
                            HangHoa hh = new HangHoa();
                            hh.MaHH = textEdit_mahh.Text.Trim();

                            if (searchLookUpEdit_loaihanghoa.EditValue != null)
                                hh.MaLoaiHH = searchLookUpEdit_loaihanghoa.EditValue.ToString().Trim();
                            else hh.MaLoaiHH = null;

                            hh.TenHH = textEdit_tenhanghoa.Text.Trim();
                            hh.XuatXu = textEdit_xuatxu.Text.Trim();
                            hh.MauSac = textEdit_mausac.Text.Trim();
                            hh.KichThuoc = textEdit_kichthuoc.Text.Trim();

                            if (spinEdit_slton_toithieu.EditValue != null)
                                hh.SLTonToiThieu = Convert.ToDouble(spinEdit_slton_toithieu.EditValue.ToString());
                            else hh.SLTonToiThieu = null;

                            if (spinEdit_slton_toida.EditValue != null)
                                hh.SLTonToiDa = Convert.ToDouble(spinEdit_slton_toida.EditValue.ToString());
                            else hh.SLTonToiDa = null;

                            hh.TinhTrang = true;
                            db.HangHoas.InsertOnSubmit(hh);

                            db.SubmitChanges();

                            OnOffTextBox(true);
                            LoadLuoi();
                            OnOffSuaXoa();
                            LoadTextBox();

                            simpleButton_sua.Enabled = true;
                            simpleButton_refesh.Enabled = true;

                            simpleButton_them.Text = "Thêm";
                            simpleButton_them.Font = new Font("Tahoma", 11, FontStyle.Regular);
                            simpleButton_them.ForeColor = Color.FromArgb(0, 0, 192);

                            simpleButton_xoa.Text = "Xóa";
                            simpleButton_xoa.Font = new Font("Tahoma", 11, FontStyle.Regular);
                            simpleButton_xoa.ForeColor = Color.FromArgb(0, 0, 192);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm thất bại! Mã lỗi: \n" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        string _mahh = "";
        private void simpleButton_sua_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (simpleButton_sua.Text.ToUpper() == "SỬA")
                {
                    OnOffTextBox(false);
                    textEdit_mahh.Properties.ReadOnly = true; // KHONG CHO SUA KHOA CHINH
                    // ClearTextBox();

                    simpleButton_them.Enabled = false;
                    simpleButton_refesh.Enabled = false;

                    simpleButton_sua.Text = "LƯU";
                    simpleButton_sua.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    simpleButton_sua.ForeColor = Color.Red;

                    simpleButton_xoa.Text = "HỦY";
                    simpleButton_xoa.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    simpleButton_xoa.ForeColor = Color.Red;

                    _mahh = textEdit_mahh.Text.Trim();
                }
                else // LUU
                {
                    if (textEdit_mahh.Text.Trim() == "")
                    {
                        MessageBox.Show("Không xác định được đối tượng cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (searchLookUpEdit_loaihanghoa.EditValue == null || searchLookUpEdit_loaihanghoa.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập tên loại hàng hóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        searchLookUpEdit_loaihanghoa.Focus();
                    }
                    else if (textEdit_tenhanghoa.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập tên hàng hóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_tenhanghoa.Focus();
                    }
                    else // Save Database
                    {
                        db = new DataClasses_QLKHOHANGDataContext();

                        var hh = (from p in db.HangHoas
                                  where p.MaHH.Trim().ToUpper() == textEdit_mahh.Text.Trim().ToUpper()
                                  select p).FirstOrDefault();
                        if (hh != null)
                        {
                            hh.TenHH = textEdit_tenhanghoa.Text.Trim();

                            if (searchLookUpEdit_loaihanghoa.EditValue != null)
                                hh.MaLoaiHH = searchLookUpEdit_loaihanghoa.EditValue.ToString().Trim();
                            else hh.MaLoaiHH = null;

                            hh.XuatXu = textEdit_xuatxu.Text.Trim();
                            hh.MauSac = textEdit_mausac.Text.Trim();
                            hh.KichThuoc = textEdit_kichthuoc.Text.Trim();

                            if (spinEdit_slton_toithieu.EditValue != null)
                                hh.SLTonToiThieu = Convert.ToDouble(spinEdit_slton_toithieu.EditValue.ToString());
                            else hh.SLTonToiThieu = null;

                            if (spinEdit_slton_toida.EditValue != null)
                                hh.SLTonToiDa = Convert.ToDouble(spinEdit_slton_toida.EditValue.ToString());
                            else hh.SLTonToiDa = null;

                            db.SubmitChanges();

                            OnOffTextBox(true);
                            LoadLuoi();
                            OnOffSuaXoa();

                            // FOCUS TAI DONG DA SUA
                            if (_mahh != "")
                            {
                                int rowIndex = gridView_hanghoa.LocateByDisplayText(0, gridView_hanghoa.Columns["MaHH"], _mahh);
                                gridControl_hanghoa.Focus();
                                gridView_hanghoa.FocusedRowHandle = rowIndex;
                            }

                            LoadTextBox();

                            simpleButton_them.Enabled = true;
                            simpleButton_refesh.Enabled = true;

                            simpleButton_sua.Text = "Sửa";
                            simpleButton_sua.Font = new Font("Tahoma", 11, FontStyle.Regular);
                            simpleButton_sua.ForeColor = Color.FromArgb(0, 0, 192);

                            simpleButton_xoa.Text = "Xóa";
                            simpleButton_xoa.Font = new Font("Tahoma", 11, FontStyle.Regular);
                            simpleButton_xoa.ForeColor = Color.FromArgb(0, 0, 192);
                        }
                        else
                        {
                            MessageBox.Show("Không xác định được đối tượng cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa thất bại! Mã lỗi: \n" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void simpleButton_xoa_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (simpleButton_xoa.Text.ToUpper() == "HỦY")
                {
                    LoadTextBox();
                    OnOffTextBox(true);

                    simpleButton_them.Enabled = true;
                    simpleButton_refesh.Enabled = true;
                    OnOffSuaXoa();

                    simpleButton_them.Text = "Thêm";
                    simpleButton_sua.Text = "Sửa";
                    simpleButton_xoa.Text = "Xóa";

                    simpleButton_them.Font = new Font("Tahoma", 11, FontStyle.Regular);
                    simpleButton_sua.Font = new Font("Tahoma", 11, FontStyle.Regular);
                    simpleButton_xoa.Font = new Font("Tahoma", 11, FontStyle.Regular);

                    simpleButton_them.ForeColor = Color.FromArgb(0, 0, 192);
                    simpleButton_sua.ForeColor = Color.FromArgb(0, 0, 192);
                    simpleButton_xoa.ForeColor = Color.FromArgb(0, 0, 192);

                }
                else // XOA
                {
                    if (textEdit_mahh.Text.Trim() != "")
                    {
                        DialogResult rs = MessageBox.Show("Bạn có chắc là muốn xóa đối tượng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (rs.ToString().Trim() == "Yes")
                        {
                            db = new DataClasses_QLKHOHANGDataContext();

                            HangHoa hh = db.HangHoas.Single(t => t.MaHH.ToUpper().Trim() == textEdit_mahh.Text.ToUpper().Trim());
                            db.HangHoas.DeleteOnSubmit(hh);
                            db.SubmitChanges();

                            LoadLuoi();
                            OnOffSuaXoa();
                            LoadTextBox();
                        }
                    }
                    else // KHONG XAC DINH
                    {
                        MessageBox.Show("Không xác định được đối tượng cần xóa!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Thao tác thất bại! \n Mã lỗi: " + ex.Message.Trim(), "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void simpleButton_refesh_Click(object sender, EventArgs e)
        {
            using (WaitDialogForm fwait = new WaitDialogForm("Vui lòng chờ trong giây lát", "Đang load dữ liệu..."))
            {
                try
                {
                    OnOffTextBox(true);
                    LoadLuoi();
                    OnOffSuaXoa();
                    LoadTextBox();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Thao tác thất bại! \n Mã lỗi: " + ex.Message.Trim(), "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                }
            }
        }

        private void frmHangHoa_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (simpleButton_them.Text.ToUpper() == "LƯU" || simpleButton_sua.Text.ToUpper() == "LƯU")
            {
                DialogResult rs = MessageBox.Show("Bạn có chắc là muốn đóng cửa sổ chương trình trong khi chưa hoàn tất thao tác ?!", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (rs.ToString().Trim() == "No")
                {
                    e.Cancel = true;
                }
            }
        }

        #region Phan xu ly nhap vao so >=0 trong spinEdit

        private void spinEdit_slton_toithieu_Validating(object sender, CancelEventArgs e)
        {
            if (spinEdit_slton_toithieu.EditValue != null && spinEdit_slton_toithieu.Value < 0)
            {
                e.Cancel = true;
            }
        }

        private void spinEdit_slton_toithieu_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Vui lòng nhập vào số dương >= 0";
        }

        private void spinEdit_slton_toida_Validating(object sender, CancelEventArgs e)
        {
            if (spinEdit_slton_toida.EditValue != null && spinEdit_slton_toida.Value < 0)
            {
                e.Cancel = true;
            }
        }

        private void spinEdit_slton_toida_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Vui lòng nhập vào số dương >= 0";
        }
        #endregion
    }
}