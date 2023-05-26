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
    public partial class frmKho : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();
        public frmKho()
        {
            InitializeComponent();
        }

        private void frmKho_Load(object sender, EventArgs e)
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
            textEdit_makho.Properties.ReadOnly = _val;
            textEdit_diengiai.Properties.ReadOnly = _val;
            textEdit_diachi.Properties.ReadOnly = _val;
            textEdit_sdt.Properties.ReadOnly = _val;
            textEdit_ghichu.Properties.ReadOnly = _val;
        }

        private void LoadLuoi()
        {
            db = new DataClasses_QLKHOHANGDataContext();
            var data = from p in db.Khos
                       where p.TinhTrang == true
                       select new
                       {
                           p.MaKho,
                           p.TenKho,
                           p.DiaChi,
                           p.SDT,
                           p.GhiChu
                       };
            if (data != null && data.ToList().Count > 0)
                gridControl_kho.DataSource = data;
            else gridControl_kho.DataSource = null;
        }

        private void OnOffSuaXoa()
        {
            if (gridControl_kho.DataSource != null && gridView_kho.DataRowCount > 0)
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

        private void gridView_dvt_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gridView_kho.FocusedRowHandle != GridControl.AutoFilterRowHandle)
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
            textEdit_makho.Text = "";
            textEdit_diengiai.Text = "";
            textEdit_diachi.Text = "";
            textEdit_sdt.Text = "";
            textEdit_ghichu.Text = "";
        }
        private void LoadTextBox()
        {
            if (gridView_kho.DataRowCount > 0 && gridView_kho.FocusedRowHandle != GridControl.AutoFilterRowHandle && gridView_kho.GetFocusedRowCellValue("MaKho") != null)
            {
                textEdit_makho.Text = gridView_kho.GetFocusedRowCellValue("MaKho").ToString().Trim();
                if (gridView_kho.GetFocusedRowCellValue("TenKho") != null)
                    textEdit_diengiai.Text = gridView_kho.GetFocusedRowCellValue("TenKho").ToString().Trim();
                else textEdit_diengiai.Text = "";

                if (gridView_kho.GetFocusedRowCellValue("DiaChi") != null)
                    textEdit_diachi.Text = gridView_kho.GetFocusedRowCellValue("DiaChi").ToString().Trim();
                else textEdit_diachi.Text = "";

                if (gridView_kho.GetFocusedRowCellValue("SDT") != null)
                    textEdit_sdt.Text = gridView_kho.GetFocusedRowCellValue("SDT").ToString().Trim();
                else textEdit_sdt.Text = "";

                if (gridView_kho.GetFocusedRowCellValue("GhiChu") != null)
                    textEdit_ghichu.Text = gridView_kho.GetFocusedRowCellValue("GhiChu").ToString().Trim();
                else textEdit_ghichu.Text = "";
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

                    textEdit_makho.Focus();
                }
                else // LUU
                {
                    if (textEdit_makho.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập mã kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_makho.Focus();
                    }
                    else if (textEdit_diengiai.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập tên kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_diengiai.Focus();
                    }
                    else // Save Database
                    {
                        // 1. KTRA TRUNG MA DON VI TNIH
                        db = new DataClasses_QLKHOHANGDataContext();
                        var data = from p in db.Khos
                                   where p.MaKho.Trim().ToUpper() == textEdit_makho.Text.Trim().ToUpper()
                                   select p;
                        if (data.Count() > 0)
                        {
                            MessageBox.Show("Trùng mã kho, vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textEdit_makho.Focus();
                        }
                        else
                        {
                            Kho k = new Kho();
                            k.MaKho = textEdit_makho.Text.Trim();
                            k.TenKho = textEdit_diengiai.Text.Trim();
                            k.DiaChi = textEdit_diachi.Text.Trim();
                            k.SDT = textEdit_sdt.Text.Trim();
                            k.GhiChu = textEdit_ghichu.Text.Trim();
                            k.TinhTrang = true;

                            db.Khos.InsertOnSubmit(k);
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

        string _MaDVT = "";
        private void simpleButton_sua_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (simpleButton_sua.Text.ToUpper() == "SỬA")
                {
                    OnOffTextBox(false);
                    textEdit_makho.Properties.ReadOnly = true; // KHONG CHO SUA KHOA CHINH
                    // ClearTextBox();

                    simpleButton_them.Enabled = false;
                    simpleButton_refesh.Enabled = false;

                    simpleButton_sua.Text = "LƯU";
                    simpleButton_sua.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    simpleButton_sua.ForeColor = Color.Red;

                    simpleButton_xoa.Text = "HỦY";
                    simpleButton_xoa.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    simpleButton_xoa.ForeColor = Color.Red;

                    _MaDVT = textEdit_makho.Text.Trim();
                }
                else // LUU
                {
                    if (textEdit_makho.Text.Trim() == "")
                    {
                        MessageBox.Show("Không xác định được đối tượng cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else // Save Database
                    {
                        db = new DataClasses_QLKHOHANGDataContext();

                        var k = (from p in db.Khos
                                      where p.MaKho.Trim().ToUpper() == textEdit_makho.Text.Trim().ToUpper()
                                      select p).FirstOrDefault();
                        if (k != null)
                        {
                            k.TenKho = textEdit_diengiai.Text.Trim();
                            k.DiaChi = textEdit_diachi.Text.Trim();
                            k.SDT = textEdit_sdt.Text.Trim();
                            k.GhiChu = textEdit_ghichu.Text.Trim();
                            k.TinhTrang = true;

                            db.SubmitChanges();

                            OnOffTextBox(true);
                            LoadLuoi();
                            OnOffSuaXoa();

                            // FOCUS TAI DONG DA SUA
                            if (_MaDVT != "")
                            {
                                int rowIndex = gridView_kho.LocateByDisplayText(0, gridView_kho.Columns["MaKho"], _MaDVT);
                                gridControl_kho.Focus();
                                gridView_kho.FocusedRowHandle = rowIndex;
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
                    if (textEdit_makho.Text.Trim() != "")
                    {
                        DialogResult rs = MessageBox.Show("Bạn có chắc là muốn xóa đối tượng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (rs.ToString().Trim() == "Yes")
                        {
                            db = new DataClasses_QLKHOHANGDataContext();

                            Kho k = db.Khos.Single(t => t.MaKho.ToUpper().Trim() == textEdit_makho.Text.ToUpper().Trim());
                            db.Khos.DeleteOnSubmit(k);
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

        private void frmKho_FormClosing(object sender, FormClosingEventArgs e)
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