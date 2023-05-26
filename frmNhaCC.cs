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
    public partial class frmNhaCC : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();

        public frmNhaCC()
        {
            InitializeComponent();
        }

        private void frmNhaCC_Load(object sender, EventArgs e)
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

        private void frmNCC_Shown(object sender, EventArgs e)
        {
            simpleButton_them.Focus();
        }

        private void OnOffSuaXoa()
        {
            if (gridControl_ncc.DataSource != null && gridView_ncc.DataRowCount > 0)
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
            var data = from p in db.NhaCCs
                       where p.TinhTrang == true
                       select new
                       {
                           p.MaNCC,
                           p.TenNCC,
                           p.DiaChi,
                           p.SDT,
                           p.Email,
                           p.GhiChu,
                           p.TinhTrang
                       };
            if (data != null && data.ToList().Count > 0)
                gridControl_ncc.DataSource = data;
            else gridControl_ncc.DataSource = null;
        }

        private void ClearTextBox()
        {
            textEdit_madoituong.Text = "";
            textEdit_tendoituong.Text = "";
            textEdit_diachi.Text = "";
            textEdit_sdt.Text = "";
            textEdit_email.Text = "";
            textEdit_ghichu.Text = "";
        }

        private void gridView_NCC_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gridView_ncc.FocusedRowHandle != GridControl.AutoFilterRowHandle)
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
            if (gridView_ncc.DataRowCount > 0 && gridView_ncc.FocusedRowHandle != GridControl.AutoFilterRowHandle && gridView_ncc.GetFocusedRowCellValue("MaNCC") != null)
            {
                textEdit_madoituong.Text = gridView_ncc.GetFocusedRowCellValue("MaNCC").ToString().Trim();

                if (gridView_ncc.GetFocusedRowCellValue("TenNCC") != null)
                    textEdit_tendoituong.Text = gridView_ncc.GetFocusedRowCellValue("TenNCC").ToString().Trim();
                else textEdit_tendoituong.Text = "";

                if (gridView_ncc.GetFocusedRowCellValue("DiaChi") != null)
                    textEdit_diachi.Text = gridView_ncc.GetFocusedRowCellValue("DiaChi").ToString().Trim();
                else textEdit_diachi.Text = "";

                if (gridView_ncc.GetFocusedRowCellValue("SDT") != null)
                    textEdit_sdt.Text = gridView_ncc.GetFocusedRowCellValue("SDT").ToString().Trim();
                else textEdit_sdt.Text = "";

                if (gridView_ncc.GetFocusedRowCellValue("Email") != null)
                    textEdit_email.Text = gridView_ncc.GetFocusedRowCellValue("Email").ToString().Trim();
                else textEdit_email.Text = "";

                if (gridView_ncc.GetFocusedRowCellValue("GhiChu") != null)
                    textEdit_ghichu.Text = gridView_ncc.GetFocusedRowCellValue("GhiChu").ToString().Trim();
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
                        MessageBox.Show("Vui lòng nhập mã nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_madoituong.Focus();
                    }
                    else if (textEdit_tendoituong.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_tendoituong.Focus();
                    }
                    else // Save Database
                    {
                        db = new DataClasses_QLKHOHANGDataContext();

                        // 1. KTRA TRUNG MA KH
                        var data = from p in db.NhaCCs
                                   where p.MaNCC.Trim().ToUpper() == textEdit_madoituong.Text.Trim().ToUpper()
                                   select p;
                        if (data.Count() > 0)
                        {
                            MessageBox.Show("Trùng mã nhà cung cấp, vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textEdit_madoituong.Focus();
                        }
                        else
                        {
                            // 2. INSERT
                            NhaCC ncc = new NhaCC();
                            ncc.MaNCC = textEdit_madoituong.Text.Trim();
                            ncc.TenNCC = textEdit_tendoituong.Text.Trim();
                            ncc.DiaChi = textEdit_diachi.Text.Trim();
                            ncc.SDT = textEdit_sdt.Text.Trim();
                            ncc.Email = textEdit_email.Text.Trim();
                            ncc.GhiChu = textEdit_ghichu.Text.Trim();
                            ncc.TinhTrang = true;

                            db.NhaCCs.InsertOnSubmit(ncc);
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
            textEdit_diachi.Properties.ReadOnly = _val;
            textEdit_email.Properties.ReadOnly = _val;
            textEdit_sdt.Properties.ReadOnly = _val;
            textEdit_ghichu.Properties.ReadOnly = _val;
        }

        string _MaNCC = "";
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

                    _MaNCC = textEdit_madoituong.Text.Trim();
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

                        var ncc = (from p in db.NhaCCs
                                  where p.MaNCC.Trim().ToUpper() == textEdit_madoituong.Text.Trim().ToUpper()
                                  select p).FirstOrDefault();
                        if (ncc != null)
                        {
                            ncc.TenNCC = textEdit_tendoituong.Text.Trim();
                            ncc.DiaChi = textEdit_diachi.Text.Trim();
                            ncc.SDT = textEdit_sdt.Text.Trim();
                            ncc.Email = textEdit_email.Text.Trim();
                            ncc.GhiChu = textEdit_ghichu.Text.Trim();
                            ncc.TinhTrang = true;

                            db.SubmitChanges();

                            OnOffTextBox(true);
                            LoadLuoi();
                            OnOffSuaXoa();

                            // FOCUS TAI DONG DA SUA
                            if (_MaNCC != "")
                            {
                                int rowIndex = gridView_ncc.LocateByDisplayText(0, gridView_ncc.Columns["MaNCC"], _MaNCC);
                                gridControl_ncc.Focus();
                                gridView_ncc.FocusedRowHandle = rowIndex;
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
                    if (textEdit_madoituong.Text.Trim() != "")
                    {
                        DialogResult rs = MessageBox.Show("Bạn có chắc là muốn xóa đối tượng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (rs.ToString().Trim() == "Yes")
                        {
                            db = new DataClasses_QLKHOHANGDataContext();
                            NhaCC ncc = db.NhaCCs.Single(t => t.MaNCC.ToUpper().Trim() == textEdit_madoituong.Text.ToUpper().Trim());
                            
                            db.NhaCCs.DeleteOnSubmit(ncc);
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
                MessageBox.Show("Thao tác thất bại! \n Mã lỗi: "+ex.Message.Trim(), "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
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