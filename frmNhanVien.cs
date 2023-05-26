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
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.Skins;
using DevExpress.XtraTreeList.Columns;

namespace QLKHOHANG
{
    public partial class frmNhanVien : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();
        string _manv = ""; // Use cho Them, Sua

        public frmNhanVien()
        {
            InitializeComponent();

            Skin skin = GridSkins.GetSkin(treeList_nhanvien.LookAndFeel);
            skin.Properties[GridSkins.OptShowTreeLine] = true;
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            try
            {
                LoadGioiTinh();
                LoadChucVu();
                LoadRole();

                InitTreeList(treeList_nhanvien);
                CreateNodesNhanVien(treeList_nhanvien);

                OnOffTextBox(true);
                LoadLuoi("ALL"); // load tat ca nhan vien cho lan dau
                OnOffSuaXoa();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadGioiTinh()
        {
            comboBoxEdit_gioitinh.Properties.Items.Insert(0, "Nam");
            comboBoxEdit_gioitinh.Properties.Items.Insert(1, "Nữ");
        }

        private void LoadChucVu()
        {
            var data = from p in db.ChucVus
                       select p;
            searchLookUpEdit_chucvu.Properties.DataSource = data;
            searchLookUpEdit_chucvu.Properties.ValueMember = "MaCV";
            searchLookUpEdit_chucvu.Properties.DisplayMember = "TenCV";
        }

        private void LoadRole()
        {
            var data = from p in db.Roles
                       select p;
            searchLookUpEdit_role.Properties.DataSource = data;
            searchLookUpEdit_role.Properties.ValueMember = "ID";
            searchLookUpEdit_role.Properties.DisplayMember = "DG";
        }

        #region Phan xu ly Treelist
        private void InitTreeList(TreeList tl)
        {
            // Create columns.
            tl.BeginUpdate();

            tl.Columns.Clear();
            tl.Nodes.Clear();

            TreeListColumn col1 = tl.Columns.Add();
            col1.Caption = "Sơ đồ tổ chức";
            col1.VisibleIndex = 0;
            col1.OptionsFilter.AutoFilterCondition = DevExpress.XtraTreeList.Columns.AutoFilterCondition.Contains;
            col1.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            col1.AppearanceHeader.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            col1.AppearanceHeader.ForeColor = Color.FromArgb(192, 0, 0);

            TreeListColumn col2 = tl.Columns.Add();
            col2.Caption = "ID";
            col2.VisibleIndex = 1;
            col2.OptionsFilter.AutoFilterCondition = DevExpress.XtraTreeList.Columns.AutoFilterCondition.Contains;
            col2.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            col2.Visible = false;

            tl.EndUpdate();
        }
        private void CreateNodesNhanVien(TreeList tl)
        {
            tl.FocusedNodeChanged -= treeList_nhanvien_FocusedNodeChanged;
            tl.Nodes.Clear();
            tl.BeginUnboundLoad();

            // Create a root node .
            TreeListNode parentForRootNodes = tl.AppendNode(new object[] { "Tất cả" }, null);
            parentForRootNodes.SetValue(1, "ALL");

            var dschucvu = from p in db.ChucVus
                           select p;

            if (dschucvu != null && dschucvu.ToList().Count > 0)
            {
                foreach (var q in dschucvu.ToList())
                {
                    var cv = db.NhanViens.Where(t => t.MaChucVu.ToUpper().Trim() == q.MaCV.ToUpper().Trim()).Count();

                    TreeListNode rootNode = tl.AppendNode(new object[] { q.TenCV.Trim() + "  (" + cv.ToString() + ")" }, parentForRootNodes);
                    rootNode.Tag = 0;
                    rootNode.SetValue(1, q.MaCV);
                }
            }

            tl.EndUnboundLoad();
            tl.ExpandAll();
            tl.FocusedNodeChanged += treeList_nhanvien_FocusedNodeChanged;
        }

        private void treeList_nhanvien_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                ClearTextBox();
                if (e.Node != null)
                    LoadLuoi(e.Node.GetDisplayText(1).Trim());
            }
            catch { }
        }

        private void LoadLuoi(string _machucvu)
        {
            gridControl_nhanvien.DataSource = null;

            var data = from p in db.NhanViens
                       where p.MaChucVu.ToUpper() == (_machucvu.ToUpper() == "ALL" ? p.MaChucVu : _machucvu.ToUpper())
                       select p;

            if (data != null && data.ToList().Count() > 0)
            {
                gridControl_nhanvien.DataSource = data;
                gridControl_nhanvien.Focus();
                gridView_nhanvien.FocusedRowHandle = 0;
            }
        }

        #endregion

        private void OnOffTextBox(bool _val)
        {
            textEdit_maID.Properties.ReadOnly = _val;
            textEdit_hoten.Properties.ReadOnly = _val;
            dateEdit_ngaysinh.Properties.ReadOnly = _val;

            searchLookUpEdit_chucvu.Properties.ReadOnly = _val;
            comboBoxEdit_gioitinh.Properties.ReadOnly = _val;

            textEdit_diachi.Properties.ReadOnly = _val;
            textEdit_sdt.Properties.ReadOnly = _val;
            searchLookUpEdit_role.Properties.ReadOnly = _val;
        }

        private void OnOffSuaXoa()
        {
            if (gridControl_nhanvien.DataSource != null && gridView_nhanvien.DataRowCount > 0)
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

        private void gridView_doituong_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gridView_nhanvien.FocusedRowHandle != GridControl.AutoFilterRowHandle && gridView_nhanvien.FocusedRowHandle >= 0)
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
            textEdit_maID.Text = "";
            textEdit_hoten.Text = "";
            textEdit_diachi.Text = "";
            dateEdit_ngaysinh.EditValue = null;
            searchLookUpEdit_chucvu.EditValue = null;
            comboBoxEdit_gioitinh.SelectedIndex = 0; // Default GioiTinh
            textEdit_sdt.Text = "";
            searchLookUpEdit_role.EditValue = null;
        }

        private void LoadTextBox()
        {
            if (gridView_nhanvien.DataRowCount > 0 && gridView_nhanvien.FocusedRowHandle != GridControl.AutoFilterRowHandle && gridView_nhanvien.GetFocusedRowCellValue("MaNV") != null)
            {
                textEdit_maID.Text = gridView_nhanvien.GetFocusedRowCellValue("MaNV").ToString().Trim();

                if (gridView_nhanvien.GetFocusedRowCellValue("HoTen") != null)
                    textEdit_hoten.Text = gridView_nhanvien.GetFocusedRowCellValue("HoTen").ToString().Trim();
                else textEdit_hoten.Text = "";

                if (gridView_nhanvien.GetFocusedRowCellValue("MaChucVu") != null)
                    searchLookUpEdit_chucvu.EditValue = gridView_nhanvien.GetFocusedRowCellValue("MaChucVu");
                else searchLookUpEdit_chucvu.EditValue = null;

                if (gridView_nhanvien.GetFocusedRowCellValue("DiaChi") != null)
                    textEdit_diachi.EditValue = gridView_nhanvien.GetFocusedRowCellValue("DiaChi");
                else textEdit_diachi.EditValue = null;

                if (gridView_nhanvien.GetFocusedRowCellValue("GioiTinh") != null)
                    comboBoxEdit_gioitinh.EditValue = gridView_nhanvien.GetFocusedRowCellValue("GioiTinh");
                else comboBoxEdit_gioitinh.EditValue = null;

                if (gridView_nhanvien.GetFocusedRowCellValue("NgaySinh") != null)
                    dateEdit_ngaysinh.EditValue = gridView_nhanvien.GetFocusedRowCellValue("NgaySinh");
                else dateEdit_ngaysinh.EditValue = null;

                if (gridView_nhanvien.GetFocusedRowCellValue("SDT") != null)
                    textEdit_sdt.EditValue = gridView_nhanvien.GetFocusedRowCellValue("SDT");
                else textEdit_sdt.EditValue = null;

                if (gridView_nhanvien.GetFocusedRowCellValue("RoleID") != null)
                    searchLookUpEdit_role.EditValue = gridView_nhanvien.GetFocusedRowCellValue("RoleID");
                else searchLookUpEdit_role.EditValue = null;

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
                    _manv = "";
                    OnOffTextBox(false);
                    treeList_nhanvien.Enabled = false;
                    ClearTextBox();

                    if (treeList_nhanvien.FocusedNode != null)
                        searchLookUpEdit_chucvu.EditValue = treeList_nhanvien.FocusedNode.GetDisplayText(1).Trim();

                    simpleButton_sua.Enabled = false;
                    simpleButton_refesh.Enabled = false;

                    simpleButton_them.Text = "LƯU";
                    simpleButton_them.Font = new Font("Times New Roman", 12, FontStyle.Bold);
                    simpleButton_them.ForeColor = Color.Red;

                    simpleButton_xoa.Enabled = true;
                    simpleButton_xoa.Text = "HỦY";
                    simpleButton_xoa.Font = new Font("Times New Roman", 12, FontStyle.Bold);
                    simpleButton_xoa.ForeColor = Color.Red;

                    textEdit_maID.Focus();
                }
                else // LUU
                {
                    if (textEdit_maID.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập mã nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit_maID.Focus();
                    }
                    else if (CheckExist_NhanVien(textEdit_maID.Text)) // ton tai
                    {
                        MessageBox.Show("Trùng mã nhân viên, vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                        textEdit_maID.Focus();
                    }
                    else if (searchLookUpEdit_chucvu.EditValue == null || searchLookUpEdit_chucvu.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập chức vụ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        searchLookUpEdit_chucvu.Focus();
                    }
                    else // Save Database
                    {
                        db = new DataClasses_QLKHOHANGDataContext();

                        NhanVien nv = new NhanVien();

                        nv.MaNV = textEdit_maID.Text.Trim();
                        nv.HoTen = textEdit_hoten.Text.Trim();

                        if (dateEdit_ngaysinh.EditValue != null && dateEdit_ngaysinh.Text.Trim() != "")
                            nv.NgaySinh = Convert.ToDateTime(dateEdit_ngaysinh.EditValue).Date;
                        else nv.NgaySinh = null;

                        if (comboBoxEdit_gioitinh.EditValue != null && comboBoxEdit_gioitinh.Text.Trim() != "")
                            nv.GioiTinh = comboBoxEdit_gioitinh.EditValue.ToString().Trim();
                        else nv.GioiTinh = null;

                        nv.DiaChi = textEdit_diachi.Text.Trim();
                        nv.SDT = textEdit_sdt.Text.Trim();
                        nv.MaChucVu = searchLookUpEdit_chucvu.EditValue.ToString().Trim();

                        if (searchLookUpEdit_role.EditValue != null && searchLookUpEdit_role.Text.Trim() != "")
                            nv.RoleID = searchLookUpEdit_role.EditValue.ToString().Trim();
                        else nv.RoleID = null;

                        nv.TinhTrang = true;

                        db.NhanViens.InsertOnSubmit(nv);
                        _manv = nv.MaNV;

                        db.SubmitChanges();

                        OnOffTextBox(true);
                        treeList_nhanvien.Enabled = true;

                        CreateNodesNhanVien(treeList_nhanvien);

                        treeList_nhanvien.FocusedNodeChanged -= treeList_nhanvien_FocusedNodeChanged;
                        treeList_nhanvien.FocusedNode = treeList_nhanvien.FindNodeByFieldValue("ID", nv.MaChucVu);

                        OnOffSuaXoa();
                        LoadLuoi(nv.MaChucVu);

                        // FOCUS TAI DONG VUA THEM VAO
                        if (_manv != "")
                        {
                            int rowIndex = gridView_nhanvien.LocateByDisplayText(0, gridView_nhanvien.Columns["MaNV"], _manv);
                            gridControl_nhanvien.Focus();
                            gridView_nhanvien.FocusedRowHandle = rowIndex;
                        }

                        LoadTextBox();
                        treeList_nhanvien.FocusedNodeChanged += treeList_nhanvien_FocusedNodeChanged;

                        simpleButton_sua.Enabled = true;
                        simpleButton_refesh.Enabled = true;

                        simpleButton_them.Text = "Thêm";
                        simpleButton_them.Font = new Font("Times New Roman", 12, FontStyle.Regular);
                        simpleButton_them.ForeColor = Color.FromArgb(0, 0, 192);

                        simpleButton_xoa.Text = "Xóa";
                        simpleButton_xoa.Font = new Font("Times New Roman", 12, FontStyle.Regular);
                        simpleButton_xoa.ForeColor = Color.FromArgb(0, 0, 192);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm thất bại! Mã lỗi: \n" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private bool CheckExist_NhanVien(string _ma)
        {
            var data = from p in db.NhanViens
                       where p.MaNV.ToUpper().Trim() == _ma.ToUpper().Trim()
                       select p;
            if (data != null && data.ToList().Count > 0)
                return true;
            else
                return false;
        }

        private void simpleButton_sua_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (simpleButton_sua.Text.ToUpper() == "SỬA")
                {
                    _manv = "";
                    OnOffTextBox(false);
                    treeList_nhanvien.Enabled = false;
                    textEdit_maID.Properties.ReadOnly = true; // KHONG CHO SUA KHOA CHINH
                    // ClearTextBox();

                    simpleButton_them.Enabled = false;
                    simpleButton_refesh.Enabled = false;

                    simpleButton_sua.Text = "LƯU";
                    simpleButton_sua.Font = new Font("Times New Roman", 12, FontStyle.Bold);
                    simpleButton_sua.ForeColor = Color.Red;

                    simpleButton_xoa.Text = "HỦY";
                    simpleButton_xoa.Font = new Font("Times New Roman", 12, FontStyle.Bold);
                    simpleButton_xoa.ForeColor = Color.Red;

                    _manv = textEdit_maID.Text.Trim();
                }
                else // LUU
                {
                    if (textEdit_maID.Text.Trim() == "")
                    {
                        MessageBox.Show("Không xác định được đối tượng cần sửa dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (searchLookUpEdit_chucvu.EditValue == null || searchLookUpEdit_chucvu.Text.Trim() == "")
                    {
                        // Doi tuong chua xac dinh
                        MessageBox.Show("Vui lòng nhập chức vụ nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        searchLookUpEdit_chucvu.Focus();
                    }
                    else // Save Database
                    {
                        db = new DataClasses_QLKHOHANGDataContext();

                        NhanVien nv = db.NhanViens.Single(t => t.MaNV.ToUpper().Trim() == textEdit_maID.Text.ToUpper().Trim());

                        nv.HoTen = textEdit_hoten.Text.Trim();

                        if (dateEdit_ngaysinh.EditValue != null && dateEdit_ngaysinh.Text.Trim() != "")
                            nv.NgaySinh = Convert.ToDateTime(dateEdit_ngaysinh.EditValue).Date;
                        else nv.NgaySinh = null;

                        if (comboBoxEdit_gioitinh.EditValue != null && comboBoxEdit_gioitinh.Text.Trim() != "")
                            nv.GioiTinh = comboBoxEdit_gioitinh.EditValue.ToString().Trim();
                        else nv.GioiTinh = null;

                        nv.DiaChi = textEdit_diachi.Text.Trim();
                        nv.SDT = textEdit_sdt.Text.Trim();
                        nv.MaChucVu = searchLookUpEdit_chucvu.EditValue.ToString().Trim();

                        if (searchLookUpEdit_role.EditValue != null && searchLookUpEdit_role.Text.Trim() != "")
                            nv.RoleID = searchLookUpEdit_role.EditValue.ToString().Trim();
                        else nv.RoleID = null;

                        nv.TinhTrang = true;

                        db.SubmitChanges();

                        OnOffTextBox(true);
                        treeList_nhanvien.Enabled = true;

                        CreateNodesNhanVien(treeList_nhanvien);

                        treeList_nhanvien.FocusedNodeChanged -= treeList_nhanvien_FocusedNodeChanged;
                        treeList_nhanvien.FocusedNode = treeList_nhanvien.FindNodeByFieldValue("ID", nv.MaChucVu);

                        OnOffSuaXoa();
                        LoadLuoi(nv.MaChucVu);

                        // FOCUS TAI DONG DA SUA
                        if (_manv != "")
                        {
                            int rowIndex = gridView_nhanvien.LocateByDisplayText(0, gridView_nhanvien.Columns["MaNV"], _manv);
                            gridControl_nhanvien.Focus();
                            gridView_nhanvien.FocusedRowHandle = rowIndex;
                        }

                        LoadTextBox();
                        treeList_nhanvien.FocusedNodeChanged += treeList_nhanvien_FocusedNodeChanged;

                        simpleButton_them.Enabled = true;
                        simpleButton_refesh.Enabled = true;

                        simpleButton_sua.Text = "Sửa";
                        simpleButton_sua.Font = new Font("Times New Roman", 12, FontStyle.Regular);
                        simpleButton_sua.ForeColor = Color.FromArgb(0, 0, 192);

                        simpleButton_xoa.Text = "Xóa";
                        simpleButton_xoa.Font = new Font("Times New Roman", 12, FontStyle.Regular);
                        simpleButton_xoa.ForeColor = Color.FromArgb(0, 0, 192);
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
            string _thaotac = "";
            try
            {
                if (simpleButton_xoa.Text.ToUpper() == "HỦY")
                {
                    _thaotac = "hủy";
                    LoadTextBox();
                    OnOffTextBox(true);
                    treeList_nhanvien.Enabled = true;

                    simpleButton_them.Enabled = true;
                    simpleButton_refesh.Enabled = true;
                    OnOffSuaXoa();

                    simpleButton_them.Text = "Thêm";
                    simpleButton_sua.Text = "Sửa";
                    simpleButton_xoa.Text = "Xóa";

                    simpleButton_them.Font = new Font("Times New Roman", 12, FontStyle.Regular);
                    simpleButton_sua.Font = new Font("Times New Roman", 12, FontStyle.Regular);
                    simpleButton_xoa.Font = new Font("Times New Roman", 12, FontStyle.Regular);

                    simpleButton_them.ForeColor = Color.FromArgb(0, 0, 192);
                    simpleButton_sua.ForeColor = Color.FromArgb(0, 0, 192);
                    simpleButton_xoa.ForeColor = Color.FromArgb(0, 0, 192);

                }
                else // XOA
                {
                    _thaotac = "xóa";
                    if (textEdit_maID.Text.Trim() != "")
                    {
                        DialogResult rs = MessageBox.Show("Bạn có chắc là muốn xóa nhân viên này ?!", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (rs.ToString().Trim() == "Yes")
                        {
                            db = new DataClasses_QLKHOHANGDataContext();

                            OnOffSuaXoa();

                            NhanVien nv = db.NhanViens.Single(t => t.MaNV.ToUpper().Trim() == textEdit_maID.Text.ToUpper().Trim());
                            db.NhanViens.DeleteOnSubmit(nv);
                            db.SubmitChanges();

                            CreateNodesNhanVien(treeList_nhanvien);

                            treeList_nhanvien.FocusedNodeChanged -= treeList_nhanvien_FocusedNodeChanged;
                            treeList_nhanvien.FocusedNode = treeList_nhanvien.FindNodeByFieldValue("ID", nv.MaChucVu);

                            LoadLuoi(nv.MaChucVu);
                            LoadTextBox();

                            treeList_nhanvien.FocusedNodeChanged += treeList_nhanvien_FocusedNodeChanged;
                        }
                    }
                    else // KHONG XAC DINH
                    {
                        MessageBox.Show("Không thể xác định được đối tượng cần xóa dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thao tác " + _thaotac + " thất bại! Mã lỗi: \n" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    OnOffSuaXoa();

                    CreateNodesNhanVien(treeList_nhanvien);
                    LoadLuoi("ALL"); // load tat ca NV

                    treeList_nhanvien.FocusedNodeChanged -= treeList_nhanvien_FocusedNodeChanged;
                    LoadTextBox();
                    treeList_nhanvien.FocusedNodeChanged += treeList_nhanvien_FocusedNodeChanged;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể làm mới dữ liệu! Mã lỗi: \n" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmNhanVien_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}