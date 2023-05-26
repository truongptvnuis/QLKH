using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;

namespace QLKHOHANG
{
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();
        public string _username = "";
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUserID.Focus();
        }

        private void btthoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btlogin_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (txtUserID.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserID.Focus();
            }
            else if (txtPass.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPass.Focus();
            }
            else if (IsvalidUser(txtUserID.Text.Trim(), txtPass.Text.Trim()))
            {
                frmMain._user_id = txtUserID.Text.Trim();
                frmMain._user_name = _username;

                frmMain frm = new frmMain();
                this.Hide();
                frm.Show();
            }
            else
            {
                MessageBox.Show("Xem lại [Tên đăng nhập] và [Mật khẩu] !!!");
            }
            this.Cursor = Cursors.Default;
        }

        private bool IsvalidUser(string userID, string password)
        {
            try
            {
                var mylogin = from p in db.NhanViens
                              where (p.MaNV == userID && p.MatKhau == password)
                              select p;
                if (mylogin.Any())
                {
                    _username = mylogin.ToList()[0].HoTen;
                    return true;
                }
            }
            catch { }

            _username = "";
            return false;
        }
    }
}