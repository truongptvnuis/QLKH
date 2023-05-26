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
using DevExpress.XtraTreeList;
using DevExpress.XtraGrid;
using DevExpress.Utils;

namespace QLKHOHANG
{
    public partial class frmKhachHang : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();

        public frmKhachHang()
        {
            InitializeComponent();
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            try
            {
                OnOffTextBox(true);
                LoadLuoi();
                OnOffSuaXoa();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmKhachHang_Shown(object sender, EventArgs e)
        {
            simpleButton_them.Focus();
        }

        private void OnOffSuaXoa()
        {
            if (gridControl_kh.DataSource != null && gridView_kh.DataRowCount > 0)
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

        private void LoadLuoi()
        {
            db = new DataClasses_QLKHOHANGDataContext();
            var data = from p in db.KhachHangs
                       where p.TinhTrang == true
                       select new
                       {
                           p.MaKH,
                           p.TenKH,
                           p.MST,
                           p.DiaChi,
                           p.SDT,
                           p.Email,
                           p.GhiChu,
                           p.TinhTrang
                       };
            if (data != null && data.ToList().Count > 0)
                gridControl_kh.DataSource = data;
            else gridControl_kh.DataSource = null;
        }

        private void ClearTextBox()
        {
            textEdit_madoituong.Text = "";
            textEdit_tendoituong.Text = "";
            textEdit_masothue.Text = "";
            textEdit_diachi.Text = "";
            textEdit_sdt.Text = "";
            textEdit_email.Text = "";
            textEdit_ghichu.Text = "";
        }

        private void gridView_kh_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gridView_kh.FocusedRowHandle != GridControl.AutoFilterRowHandle)
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

        private void LoadTextBox()
        {
            if (gridView_kh.DataRowCount > 0 && gridView_kh.FocusedRowHandle != GridControl.AutoFilterRowHandle && gridView_kh.GetFocusedRowCellValue("MaKH") != null)
            {
                textEdit_madoituong.Text = gridView_kh.GetFocusedRowCellValue("MaKH").ToString().Trim();

                if (gridView_kh.GetFocusedRowCellValue("TenKH") != null)
                    textEdit_tendoituong.Text = gridView_kh.GetFocusedRowCellValue("TenKH").ToString().Trim();
                else textEdit_tendoituong.Text = "";

                if (gridView_kh.GetFocusedRowCellValue("MST") != null)
                    textEdit_masothue.Text = gridView_kh.GetFocusedRowCellValue("MST").ToString().Trim();
                else textEdit_masothue.Text = "";

                if (gridView_kh.GetFocusedRowCellValue("DiaChi") != null)
                    textEdit_diachi.Text = gridView_kh.GetFocusedRowCellValue("DiaChi").ToString().Trim();
                else textEdit_diachi.Text = "";

                if (gridView_kh.GetFocusedRowCellValue("SDT") != null)
                    textEdit_sdt.Text = gridView_kh.GetFocusedRowCellValue("SDT").ToString().Trim();
                else textEdit_sdt.Text = "";

                if (gridView_kh.GetFocusedRowCellValue("Email") != null)
                    textEdit_email.Text = gridView_kh.GetFocusedRowCellValue("Email").ToString().Trim();
                else textEdit_email.Text = "";

                if (gridView_kh.GetFocusedRowCellValue("GhiChu") != null)
                    textEdit_ghichu.Text = gridView_kh.GetFocusedRowCellValue("GhiChu").ToString().Trim();
                else textEdit_ghichu.Text = "";
            }
            else
            {
                ClearTextBox();
            }
        }

        private void simpleButton_thoat_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Close();
            this.Cursor = Cursors.Default;
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

                    textEdit_madoituong.Focus();
                }
                else // LUU
                {
                    if (textEdit_madoituong.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập mã khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_madoituong.Focus();
                    }
                    else if (textEdit_tendoituong.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_tendoituong.Focus();
                    }
                    else // Save Database
                    {
                        db = new DataClasses_QLKHOHANGDataContext();

                        // 1. KTRA TRUNG MA KH
                        var data = from p in db.KhachHangs
                                   where p.MaKH.Trim().ToUpper() == textEdit_madoituong.Text.Trim().ToUpper()
                                   select p;
                        if (data.Count() > 0)
                        {
                            MessageBox.Show("Trùng mã, vui lòng nhập mã khách hàng khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textEdit_madoituong.Focus();
                        }
                        else
                        {
                            // 2. INSERT
                            KhachHang kh = new KhachHang();
                            kh.MaKH = textEdit_madoituong.Text.Trim();
                            kh.TenKH = textEdit_tendoituong.Text.Trim();
                            kh.MST = textEdit_masothue.Text.Trim();
                            kh.DiaChi = textEdit_diachi.Text.Trim();
                            kh.SDT = textEdit_sdt.Text.Trim();
                            kh.Email = textEdit_email.Text.Trim();
                            kh.GhiChu = textEdit_ghichu.Text.Trim();
                            kh.TinhTrang = true;

                            db.KhachHangs.InsertOnSubmit(kh);
                            db.SubmitChanges();

                            MessageBox.Show("Thêm dữ liệu thành công");
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
                MessageBox.Show("Thêm thất bại, vui lòng kiểm tra lại thông tin nhập vào! \n Mã lỗi: " + ex.Message.Trim(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void OnOffTextBox(bool _val)
        {
            textEdit_madoituong.Properties.ReadOnly = _val;
            textEdit_tendoituong.Properties.ReadOnly = _val;
            textEdit_masothue.Properties.ReadOnly = _val;
            textEdit_diachi.Properties.ReadOnly = _val;
            textEdit_email.Properties.ReadOnly = _val;
            textEdit_sdt.Properties.ReadOnly = _val;
            textEdit_ghichu.Properties.ReadOnly = _val;
        }

        string _makh = "";
        private void simpleButton_sua_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (simpleButton_sua.Text.ToUpper() == "SỬA")
                {
                    OnOffTextBox(false);
                    textEdit_madoituong.Properties.ReadOnly = true; // KHONG CHO SUA KHOA CHINH

                    simpleButton_them.Enabled = false;
                    simpleButton_refesh.Enabled = false;

                    simpleButton_sua.Text = "LƯU";
                    simpleButton_sua.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    simpleButton_sua.ForeColor = Color.Red;

                    simpleButton_xoa.Text = "HỦY";
                    simpleButton_xoa.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    simpleButton_xoa.ForeColor = Color.Red;

                    _makh = textEdit_madoituong.Text.Trim();
                }
                else // LUU
                {
                    if (textEdit_madoituong.Text.Trim() == "")
                    {
                        MessageBox.Show("Không xác định được đối tượng cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else // Save Database
                    {
                        db = new DataClasses_QLKHOHANGDataContext();

                        var kh = (from p in db.KhachHangs
                                  where p.MaKH.Trim().ToUpper() == textEdit_madoituong.Text.Trim().ToUpper()
                                  select p).FirstOrDefault();
                        if (kh != null)
                        {
                            kh.TenKH = textEdit_tendoituong.Text.Trim();
                            kh.MST = textEdit_masothue.Text.Trim();
                            kh.DiaChi = textEdit_diachi.Text.Trim();
                            kh.SDT = textEdit_sdt.Text.Trim();
                            kh.Email = textEdit_email.Text.Trim();
                            kh.GhiChu = textEdit_ghichu.Text.Trim();
                            kh.TinhTrang = true;

                            db.SubmitChanges();

                            OnOffTextBox(true);
                            LoadLuoi();
                            OnOffSuaXoa();

                            // FOCUS TAI DONG DA SUA
                            if (_makh != "")
                            {
                                int rowIndex = gridView_kh.LocateByDisplayText(0, gridView_kh.Columns["MaKH"], _makh);
                                gridControl_kh.Focus();
                                gridView_kh.FocusedRowHandle = rowIndex;
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
                    if (textEdit_madoituong.Text.Trim() != "")
                    {
                        DialogResult rs = MessageBox.Show("Bạn có chắc là muốn xóa đối tượng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (rs.ToString().Trim() == "Yes")
                        {
                            db = new DataClasses_QLKHOHANGDataContext();
                            KhachHang kh = db.KhachHangs.Single(t => t.MaKH.ToUpper().Trim() == textEdit_madoituong.Text.ToUpper().Trim());

                            db.KhachHangs.DeleteOnSubmit(kh);
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
    }
}