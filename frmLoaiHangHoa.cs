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

namespace QLKHOHANG
{
    public partial class frmLoaiHangHoa : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();
        public frmLoaiHangHoa()
        {
            InitializeComponent();
        }

        private void frmLoaiHangHoa_Load(object sender, EventArgs e)
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

        private void OnOffTextBox(bool _val)
        {
            textEdit_maloaihh.Properties.ReadOnly = _val;
            textEdit_diengiai.Properties.ReadOnly = _val;
        }

        private void LoadLuoi()
        {
            db = new DataClasses_QLKHOHANGDataContext();
            var data = from p in db.LoaiHangHoas
                       where p.TinhTrang == true
                       select new
                       {
                           p.MaLoaiHH,
                           p.TenLoaiHH
                       };
            if (data != null && data.ToList().Count > 0)
                gridControl_loaihanghoa.DataSource = data;
            else gridControl_loaihanghoa.DataSource = null;
        }

        private void OnOffSuaXoa()
        {
            if (gridControl_loaihanghoa.DataSource != null && gridView_loaihanghoa.DataRowCount > 0)
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

        private void gridView_loaihh_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gridView_loaihanghoa.FocusedRowHandle != GridControl.AutoFilterRowHandle)
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
            textEdit_maloaihh.Text = "";
            textEdit_diengiai.Text = "";
        }
        private void LoadTextBox()
        {
            if (gridView_loaihanghoa.DataRowCount > 0 && gridView_loaihanghoa.FocusedRowHandle != GridControl.AutoFilterRowHandle && gridView_loaihanghoa.GetFocusedRowCellValue("MaLoaiHH") != null)
            {
                textEdit_maloaihh.Text = gridView_loaihanghoa.GetFocusedRowCellValue("MaLoaiHH").ToString().Trim();
                if (gridView_loaihanghoa.GetFocusedRowCellValue("TenLoaiHH") != null)
                    textEdit_diengiai.Text = gridView_loaihanghoa.GetFocusedRowCellValue("TenLoaiHH").ToString().Trim();
                else textEdit_diengiai.Text = "";
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

                    textEdit_maloaihh.Focus();
                }
                else // LUU
                {
                    if (textEdit_maloaihh.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập mã loại hàng hóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_maloaihh.Focus();
                    }
                    else if (textEdit_diengiai.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập tên loại hàng hóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_diengiai.Focus();
                    }
                    else // Save Database
                    {
                        // 1. KTRA TRUNG MA HANG HOA
                        db = new DataClasses_QLKHOHANGDataContext();
                        var data = from p in db.LoaiHangHoas
                                   where p.MaLoaiHH.Trim().ToUpper() == textEdit_maloaihh.Text.Trim().ToUpper()
                                   select p;
                        if (data.Count() > 0)
                        {
                            MessageBox.Show("Trùng mã loại hàng hóa, vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textEdit_maloaihh.Focus();
                        }
                        else
                        {
                            LoaiHangHoa loaihh = new LoaiHangHoa();
                            loaihh.MaLoaiHH = textEdit_maloaihh.Text.Trim();
                            loaihh.TenLoaiHH = textEdit_diengiai.Text.Trim();
                            loaihh.TinhTrang = true;

                            db.LoaiHangHoas.InsertOnSubmit(loaihh);
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
                MessageBox.Show("Thêm thất bại, vui lòng kiểm tra lại thông tin nhập vào! \n Mã lỗi: " + ex.Message.Trim(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        string _maloaihh = "";
        private void simpleButton_sua_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (simpleButton_sua.Text.ToUpper() == "SỬA")
                {
                    OnOffTextBox(false);
                    textEdit_maloaihh.Properties.ReadOnly = true; // KHONG CHO SUA KHOA CHINH
                    // ClearTextBox();

                    simpleButton_them.Enabled = false;
                    simpleButton_refesh.Enabled = false;

                    simpleButton_sua.Text = "LƯU";
                    simpleButton_sua.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    simpleButton_sua.ForeColor = Color.Red;

                    simpleButton_xoa.Text = "HỦY";
                    simpleButton_xoa.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    simpleButton_xoa.ForeColor = Color.Red;

                    _maloaihh = textEdit_maloaihh.Text.Trim();
                }
                else // LUU
                {
                    if (textEdit_maloaihh.Text.Trim() == "")
                    {
                        MessageBox.Show("Không xác định được đối tượng cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else // Save Database
                    {
                        db = new DataClasses_QLKHOHANGDataContext();

                        var loaihh = (from p in db.LoaiHangHoas
                                      where p.MaLoaiHH.Trim().ToUpper() == textEdit_maloaihh.Text.Trim().ToUpper()
                                      select p).FirstOrDefault();
                        if (loaihh != null)
                        {
                            loaihh.TenLoaiHH = textEdit_diengiai.Text.Trim();
                            loaihh.TinhTrang = true;

                            db.SubmitChanges();

                            OnOffTextBox(true);
                            LoadLuoi();
                            OnOffSuaXoa();

                            // FOCUS TAI DONG DA SUA
                            if (_maloaihh != "")
                            {
                                int rowIndex = gridView_loaihanghoa.LocateByDisplayText(0, gridView_loaihanghoa.Columns["MaLoaiHH"], _maloaihh);
                                gridControl_loaihanghoa.Focus();
                                gridView_loaihanghoa.FocusedRowHandle = rowIndex;
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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thao tác thất bại! \n Mã lỗi: " + ex.Message.Trim(), "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
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
                    if (textEdit_maloaihh.Text.Trim() != "")
                    {
                        DialogResult rs = MessageBox.Show("Bạn có chắc là muốn xóa đối tượng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (rs.ToString().Trim() == "Yes")
                        {
                            db = new DataClasses_QLKHOHANGDataContext();

                            LoaiHangHoa loaihh = db.LoaiHangHoas.Single(t => t.MaLoaiHH.ToUpper().Trim() == textEdit_maloaihh.Text.ToUpper().Trim());
                            db.LoaiHangHoas.DeleteOnSubmit(loaihh);
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

        private void frmLoaiHangHoa_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (simpleButton_them.Text.ToUpper() == "LƯU" || simpleButton_sua.Text.ToUpper() == "LƯU")
            {
                DialogResult rs = MessageBox.Show("Bạn có muốn thoát form khi chưa thực hiện xong thao tác?", "Xác nhận đóng cửa sổ", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (rs.ToString().Trim() == "No")
                {
                    e.Cancel = true;
                }
            }
        }
    }
}