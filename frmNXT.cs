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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using QLKHOHANG.Reports;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;

namespace QLKHOHANG
{
    public partial class frmNXT : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();

        public frmNXT()
        {
            InitializeComponent();
        }

        private void frmNXT_Load(object sender, EventArgs e)
        {
            dateEdit_thanglv.EditValue = DateTime.Now.Date;
            LoadHangHoa();
            LoadKho();
        }

        private void frmNXT_Shown(object sender, EventArgs e)
        {
            TongHop();
        }

        private void barButtonItem_tonghop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            TongHop();
            this.Cursor = Cursors.Default;
        }

        private void LoadHangHoa()
        {
            var data = from p in db.HangHoas
                       select p;

            repositoryItemLookUpEdit_mahanghoa.DataSource = data;
            repositoryItemLookUpEdit_mahanghoa.ValueMember = "MaHH";
            repositoryItemLookUpEdit_mahanghoa.DisplayMember = "MaHH";
        }

        private void LoadKho()
        {
            var data = from p in db.Khos
                       select new
                       {
                           p.MaKho,
                           p.TenKho,
                           p.DiaChi
                       };

            repositoryItemLookUpEdit_makho.DataSource = data;
            repositoryItemLookUpEdit_makho.ValueMember = "MaKho";
            repositoryItemLookUpEdit_makho.DisplayMember = "MaKho";
        }

        private void TongHop()
        {
            try
            {
                if (dateEdit_thanglv.EditValue == null || dateEdit_thanglv.EditValue.ToString().Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập tháng năm báo cáo!","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    bar_controls.ItemLinks[0].Focus();
                }
                else
                {

                    int thang = Convert.ToDateTime(dateEdit_thanglv.EditValue).Month;
                    int nam = Convert.ToDateTime(dateEdit_thanglv.EditValue).Year;

                    SqlConnection con = new SqlConnection();
                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = Program.constr;
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "sp_nxt_thang";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@THANG", thang);
                    cmd.Parameters.AddWithValue("@NAM", nam);

                    if(barEditItem_makho.EditValue != null && barEditItem_makho.EditValue.ToString().Trim() != "")
                        cmd.Parameters.AddWithValue("@MAKHO", barEditItem_makho.EditValue.ToString().Trim());
                    else cmd.Parameters.AddWithValue("@MAKHO", DBNull.Value);

                    if (barEditItem_mahanghoa.EditValue != null && barEditItem_mahanghoa.EditValue.ToString().Trim() != "")
                        cmd.Parameters.AddWithValue("@MAHH", barEditItem_mahanghoa.EditValue.ToString().Trim());
                    else cmd.Parameters.AddWithValue("@MAHH", DBNull.Value);

                    SqlDataAdapter adt = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adt.Fill(dt);

                    gridControl_nxt_thang.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void barEditItem_makho_EditValueChanged(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                bar_controls.ItemLinks[3].Focus();
            }));
        }

        private void barEditItem_mahanghoa_EditValueChanged(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                bar_controls.ItemLinks[4].Focus();
            }));
        }

        private void ClearTextBox()
        {
            gridControl_nxt_thang.DataSource = null;
        }


        private void barButtonItem_thoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Close();
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_in_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (dateEdit_thanglv.EditValue != null && dateEdit_thanglv.EditValue.ToString().Trim() != "")
                {
                    R_NXT._tieude = "Tháng " + Convert.ToDateTime(dateEdit_thanglv.EditValue).ToString("MM") + " năm " + Convert.ToDateTime(dateEdit_thanglv.EditValue).ToString("yyyy");
                }
                R_NXT R = new R_NXT();
                R.DataSource = gridView_tonkho.DataSource;
                R.ShowPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Cursor = Cursors.Default;
        }
    }
}