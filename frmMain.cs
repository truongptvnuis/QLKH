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

namespace QLKHOHANG
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public static string _user_id = "";
        public static string _user_name = "";

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            label_ngaygiohethong.Text = "     " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timer1.Start();
            if (_user_id != "")
                label_user_id.Text = "     " + _user_id.Trim() + " - " + _user_name.ToUpper().Trim();

            ShowForm(new frmBackGround());
        }

        #region Code chuc nang Show form con trong form cha

        private void ShowForm(Form fChild)
        {
            bool exist = false;
            foreach (Form item in this.MdiChildren)
            {
                if (item.Name == fChild.Name)
                {
                    exist = true;
                    item.Activate();
                    break;
                }
            }
            if (!exist)
            {
                fChild.MdiParent = this;
                fChild.Show();
            }
        }

        private void CLoseAndShowForm(Form fChild)
        {
            foreach (Form item in this.MdiChildren)
            {
                if (item.Name == fChild.Name)
                {
                    item.Close();
                }
            }

            fChild.MdiParent = this;
            fChild.Show();
        }

        #endregion

        private void barButtonItem_thoatform_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        private void barButtonItem_phieunhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.ShowForm(new frmPhieuNhap2());
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_phieuxuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.ShowForm(new frmPhieuXuat2());
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_hanghoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.ShowForm(new frmHangHoa());
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_khachhang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.ShowForm(new frmKhachHang());
            this.Cursor = Cursors.Default;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label_ngaygiohethong.Text = "     " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void barButtonItem_doimatkhau_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (_user_id != "")
            {
                frmDoiMatKhau2 frm = new frmDoiMatKhau2();
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Không xác định được tài khoản người dùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_danhmuc_hanghoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            frmHangHoa frm = new frmHangHoa();
            frm.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_danhmuc_nhanvien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.ShowForm(new frmNhanVien());
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_danhmuc_khachhang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            frmKhachHang frm = new frmKhachHang();
            frm.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_baocao_phieunhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.ShowForm(new frmPhieuNhap2());
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_baocao_phieuxuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.ShowForm(new frmPhieuXuat2());
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.ShowForm(new frmNhanVien());
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_danhmuc_loaihanghoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            frmLoaiHangHoa frm = new frmLoaiHangHoa();
            frm.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_danhmuc_nhacungcap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            frmNhaCC frm = new frmNhaCC();
            frm.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_danhmuc_donvitinh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            frmDVT frm = new frmDVT();
            frm.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_danhmuc_chucvu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            frmChucVu frm = new frmChucVu();
            frm.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_danhmuc_kho_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            frmKho frm = new frmKho();
            frm.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_tonkho_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ShowForm(new frmNXT());
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_baocao_tonkho_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ShowForm(new frmNXT());
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_huongdan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string path = Application.StartupPath + "\\TAI_LIEU_HD.docx";
            Help.ShowHelp(this, "file://" + path);
        }

        private void barButtonItem_doanhthu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ShowForm(new frmDoanhThu());
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_danhmuc_kh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            frmKhachHang frm = new frmKhachHang();
            frm.ShowDialog();
            this.Cursor = Cursors.Default;
        }
    }
}