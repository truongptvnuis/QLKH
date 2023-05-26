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
    public partial class frmDVT : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();
        public frmDVT()
        {
            InitializeComponent();
        }

        private void frmDVT_Load(object sender, EventArgs e)
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
            textEdit_madvt.Properties.ReadOnly = _val;
            textEdit_diengiai.Properties.ReadOnly = _val;
        }

        private void LoadLuoi()
        {
            db = new DataClasses_QLKHOHANGDataContext();
            var data = from p in db.DonViTinhs
                       where p.TinhTrang == true
                       select new
                       {
                           p.MaDVT,
                           p.TenDVT
                       };
            if (data != null && data.ToList().Count > 0)
                gridControl_dvt.DataSource = data;
            else gridControl_dvt.DataSource = null;
        }

        private void OnOffSuaXoa()
        {
            if (gridControl_dvt.DataSource != null && gridView_dvt.DataRowCount > 0)
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
                if (gridView_dvt.FocusedRowHandle != GridControl.AutoFilterRowHandle)
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
            textEdit_madvt.Text = "";
            textEdit_diengiai.Text = "";
        }
        private void LoadTextBox()
        {
            if (gridView_dvt.DataRowCount > 0 && gridView_dvt.FocusedRowHandle != GridControl.AutoFilterRowHandle && gridView_dvt.GetFocusedRowCellValue("MaDVT") != null)
            {
                textEdit_madvt.Text = gridView_dvt.GetFocusedRowCellValue("MaDVT").ToString().Trim();
                if (gridView_dvt.GetFocusedRowCellValue("TenDVT") != null)
                    textEdit_diengiai.Text = gridView_dvt.GetFocusedRowCellValue("TenDVT").ToString().Trim();
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

                    textEdit_madvt.Focus();
                }
                else // LUU
                {
                    if (textEdit_madvt.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập mã đơn vị tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_madvt.Focus();
                    }
                    else if (textEdit_diengiai.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập tên đơn vị tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_diengiai.Focus();
                    }
                    else // Save Database
                    {
                        // 1. KTRA TRUNG MA DON VI TNIH
                        db = new DataClasses_QLKHOHANGDataContext();
                        var data = from p in db.DonViTinhs
                                   where p.MaDVT.Trim().ToUpper() == textEdit_madvt.Text.Trim().ToUpper()
                                   select p;
                        if (data.Count() > 0)
                        {
                            MessageBox.Show("Trùng mã đơn vị tính, vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textEdit_madvt.Focus();
                        }
                        else
                        {
                            DonViTinh dvt = new DonViTinh();
                            dvt.MaDVT = textEdit_madvt.Text.Trim();
                            dvt.TenDVT = textEdit_diengiai.Text.Trim();
                            dvt.TinhTrang = true;

                            db.DonViTinhs.InsertOnSubmit(dvt);
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
                    textEdit_madvt.Properties.ReadOnly = true; // KHONG CHO SUA KHOA CHINH
                    // ClearTextBox();

                    simpleButton_them.Enabled = false;
                    simpleButton_refesh.Enabled = false;

                    simpleButton_sua.Text = "LƯU";
                    simpleButton_sua.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    simpleButton_sua.ForeColor = Color.Red;

                    simpleButton_xoa.Text = "HỦY";
                    simpleButton_xoa.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    simpleButton_xoa.ForeColor = Color.Red;

                    _MaDVT = textEdit_madvt.Text.Trim();
                }
                else // LUU
                {
                    if (textEdit_madvt.Text.Trim() == "")
                    {
                        MessageBox.Show("Không xác định được đối tượng cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else // Save Database
                    {
                        db = new DataClasses_QLKHOHANGDataContext();

                        var loaihh = (from p in db.DonViTinhs
                                      where p.MaDVT.Trim().ToUpper() == textEdit_madvt.Text.Trim().ToUpper()
                                      select p).FirstOrDefault();
                        if (loaihh != null)
                        {
                            loaihh.TenDVT = textEdit_diengiai.Text.Trim();
                            loaihh.TinhTrang = true;

                            db.SubmitChanges();

                            OnOffTextBox(true);
                            LoadLuoi();
                            OnOffSuaXoa();

                            // FOCUS TAI DONG DA SUA
                            if (_MaDVT != "")
                            {
                                int rowIndex = gridView_dvt.LocateByDisplayText(0, gridView_dvt.Columns["MaDVT"], _MaDVT);
                                gridControl_dvt.Focus();
                                gridView_dvt.FocusedRowHandle = rowIndex;
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
                    if (textEdit_madvt.Text.Trim() != "")
                    {
                        DialogResult rs = MessageBox.Show("Bạn có chắc là muốn xóa đối tượng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (rs.ToString().Trim() == "Yes")
                        {
                            db = new DataClasses_QLKHOHANGDataContext();

                            DonViTinh dvt = db.DonViTinhs.Single(t => t.MaDVT.ToUpper().Trim() == textEdit_madvt.Text.ToUpper().Trim());
                            db.DonViTinhs.DeleteOnSubmit(dvt);
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

        private void frmDVT_FormClosing(object sender, FormClosingEventArgs e)
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