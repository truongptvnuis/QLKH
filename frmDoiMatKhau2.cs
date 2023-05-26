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
    public partial class frmDoiMatKhau2 : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();
        public frmDoiMatKhau2()
        {
            InitializeComponent();
        }

        private void frmDoiMatKhau2_Load(object sender, EventArgs e)
        {
            textEdit_manv.Text = frmMain._user_id;
            textEdit_tennv.Text = frmMain._user_name;

            textEdit_matkhau_dangdung.Focus();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (textEdit_manv.Text.Trim() == "")
                {
                    MessageBox.Show("Không thể xác định được người dùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (textEdit_matkhau_dangdung.Text.Trim() == "" || textEdit_matkhau_moi.Text.Trim() == "" || textEdit_nhaplai_matkhaumoi.Text.Trim() == "")
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (textEdit_matkhau_moi.Text.Trim().ToUpper() != textEdit_nhaplai_matkhaumoi.Text.Trim().ToUpper())
                    {
                        MessageBox.Show("Mật khẩu mới không trùng khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textEdit_nhaplai_matkhaumoi.Focus();
                    }
                    else
                    {
                        db = new DataClasses_QLKHOHANGDataContext();
                        var data = (from p in db.NhanViens
                                    where p.MaNV.Trim().ToUpper() == textEdit_manv.Text.Trim().ToUpper()
                                    select p).FirstOrDefault();
                        if (data != null)
                        {
                            if (data.MatKhau.Trim().ToUpper() != textEdit_matkhau_dangdung.Text.Trim().ToUpper())
                            {
                                MessageBox.Show("Sai mật khẩu người dùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                data.MatKhau = textEdit_matkhau_moi.Text.Trim();
                                db.SubmitChanges();
                                MessageBox.Show("Cập nhật mật khẩu thành công!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không thể xác định được người dùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.Trim());
            }
            this.Cursor = Cursors.Default;
        }
    }
}