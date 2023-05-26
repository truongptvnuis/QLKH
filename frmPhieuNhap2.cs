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

namespace QLKHOHANG
{
    public partial class frmPhieuNhap2 : DevExpress.XtraEditors.XtraForm
    {
        DataClasses_QLKHOHANGDataContext db = new DataClasses_QLKHOHANGDataContext();

        public frmPhieuNhap2()
        {
            InitializeComponent();
        }

        private void frmPhieuNhap2_Load(object sender, EventArgs e)
        {
            dateEdit_tungay.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dateEdit_denngay.EditValue = DateTime.Now.Date;

            TongHop();
        }

        private void barButtonItem_tonghop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            TongHop();

            if (gridView_phieunhap.DataRowCount <= 0)
                MessageBox.Show("Không tìm thấy dữ liệu thỏa yêu cầu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


            this.Cursor = Cursors.Default;
        }

        private void TongHop()
        {
            try
            {
                if (dateEdit_tungay.EditValue != null && dateEdit_denngay.EditValue != null)
                {
                    if (DateTime.Compare(Convert.ToDateTime(dateEdit_tungay.EditValue).Date, Convert.ToDateTime(dateEdit_denngay.EditValue).Date) <= 0)
                    {
                        LoadLuoi(Convert.ToDateTime(dateEdit_tungay.EditValue).Date, Convert.ToDateTime(dateEdit_denngay.EditValue).Date);
                    }
                    else
                    {
                        MessageBox.Show("Từ ngày không được vượt quá đến ngày!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        bar_controls.ItemLinks[0].Focus();

                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    if (dateEdit_tungay.EditValue == null)
                    {
                        MessageBox.Show("Vui lòng nhập thời gian bắt đầu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        bar_controls.ItemLinks[0].Focus();

                        this.Cursor = Cursors.Default;
                        return;
                    }
                    else if (dateEdit_denngay.EditValue == null)
                    {
                        MessageBox.Show("Vui lòng nhập thời gian kết thúc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        bar_controls.ItemLinks[1].Focus();

                        this.Cursor = Cursors.Default;
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadLuoi(DateTime _tungay, DateTime _denngay)
        {
            gridControl_phieunhap.DataSource = null;

            db = new DataClasses_QLKHOHANGDataContext();
            var phieunhap = from p in db.PhieuNhaps
                            where p.NgayNhap.Value.Date >= _tungay && p.NgayNhap.Value.Date <= _denngay && p.TinhTrang == true
                            select new
                            {
                                p.SoPN,
                                p.MaNCC,
                                p.NhaCC.TenNCC,
                                p.LyDoNhap,
                                p.NgayNhap,
                                p.MaKho,
                                p.TongSL,
                                p.TongTriGia,
                                p.TinhTrang,
                                TrangThai = (p.TinhTrang == true ? "Đã duyệt" : "Chưa duyệt"),
                                p.MaNV
                            };
            gridControl_phieunhap.DataSource = phieunhap;

            gridView_phieunhap.ExpandAllGroups();
            if (phieunhap.ToList().Count > 0)
            {
                gridView_phieunhap.FocusedRowHandle = 0;
            }
        }

        private void barButtonItem_them_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            frmPhieuNhap2_Them._tieude = "PHIẾU NHẬP - THÊM MỚI DỮ LIỆU";
            frmPhieuNhap2_Them._sopn = "";
            frmPhieuNhap2_Them frm = new frmPhieuNhap2_Them();
            frm.ShowDialog();

            if (dateEdit_tungay.EditValue != null && dateEdit_denngay.EditValue != null && dateEdit_tungay.EditValue.ToString().Trim() != "" && dateEdit_denngay.EditValue.ToString().Trim() != "")
                LoadLuoi(Convert.ToDateTime(dateEdit_tungay.EditValue).Date, Convert.ToDateTime(dateEdit_denngay.EditValue).Date);

            this.Cursor = Cursors.Default;
        }

        private void dateEdit_tungay_EditValueChanged(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                bar_controls.ItemLinks[1].Focus();
            }));
        }

        private void dateEdit_denngay_EditValueChanged(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                bar_controls.ItemLinks[2].Focus();
            }));
        }

        private void gridView_phieunhap_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gridView_phieunhap.FocusedRowHandle != GridControl.AutoFilterRowHandle)
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
            gridControl_ctpn.DataSource = null;
        }

        private void LoadTextBox()
        {
            if (gridView_phieunhap.DataRowCount > 0 && gridView_phieunhap.FocusedRowHandle != GridControl.AutoFilterRowHandle && gridView_phieunhap.GetFocusedRowCellValue("SoPN") != null)
            {
                var data = from p in db.ChiTietPNs
                           where p.SoPN.Trim().ToUpper() == gridView_phieunhap.GetFocusedRowCellValue("SoPN").ToString().Trim().ToUpper()
                           select new
                           {
                               p.MaCTPN,
                               p.SoPN,
                               p.MaHH,
                               p.HangHoa.TenHH,
                               p.MaDVT,
                               p.DonViTinh.TenDVT,
                               p.SoLuong,
                               p.DonGia,
                               p.ThanhTien,
                               p.GhiChu
                           };
                gridControl_ctpn.DataSource = data;
            }
            else
            {
                ClearTextBox();
            }
        }

        private void barButtonItem_thoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Close();
            this.Cursor = Cursors.Default;
        }

        private void gridView_phieunhap_DoubleClick(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);

            if ((info.InRow || info.InRowCell) && !info.InGroupRow && gridView_phieunhap.FocusedRowHandle != GridControl.AutoFilterRowHandle)
            {
                if (gridView_phieunhap.GetFocusedRowCellValue("SoPN") != null && gridView_phieunhap.GetFocusedRowCellValue("SoPN").ToString().Trim() != "")
                {
                    frmPhieuNhap2_Them._tieude = "PHIẾU NHẬP - SỬA DỮ LIỆU";
                    frmPhieuNhap2_Them._sopn = gridView_phieunhap.GetFocusedRowCellValue("SoPN").ToString().Trim();

                    frmPhieuNhap2_Them frm = new frmPhieuNhap2_Them();
                    frm.ShowDialog();

                    if (dateEdit_tungay.EditValue != null && dateEdit_denngay.EditValue != null && dateEdit_tungay.EditValue.ToString().Trim() != "" && dateEdit_denngay.EditValue.ToString().Trim() != "")
                        LoadLuoi(Convert.ToDateTime(dateEdit_tungay.EditValue).Date, Convert.ToDateTime(dateEdit_denngay.EditValue).Date);
                }
                else
                {

                    frmPhieuNhap2_Them._sopn = "";
                    MessageBox.Show("Không xác định được đối tượng cần sửa thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_in_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (dateEdit_tungay.EditValue != null && dateEdit_denngay.EditValue != null)
                {
                    if (Convert.ToDateTime(dateEdit_tungay.EditValue).Date == Convert.ToDateTime(dateEdit_denngay.EditValue).Date)
                        R_PHIEUNHAP._tieude = "Trong ngày " + Convert.ToDateTime(dateEdit_tungay.EditValue).Date.ToString("dd/MM/yyyy");
                    else R_PHIEUNHAP._tieude = "Từ ngày " + Convert.ToDateTime(dateEdit_tungay.EditValue).Date.ToString("dd/MM/yyyy") + " đến ngày " + Convert.ToDateTime(dateEdit_denngay.EditValue).Date.ToString("dd/MM/yyyy");
                }
                R_PHIEUNHAP R = new R_PHIEUNHAP();
                R.DataSource = gridView_phieunhap.DataSource;
                R.ShowPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_sua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (gridView_phieunhap.GetFocusedRowCellValue("SoPN") != null && gridView_phieunhap.GetFocusedRowCellValue("SoPN").ToString().Trim() != "")
            {
                frmPhieuNhap2_Them._tieude = "PHIẾU NHẬP - SỬA DỮ LIỆU";
                frmPhieuNhap2_Them._sopn = gridView_phieunhap.GetFocusedRowCellValue("SoPN").ToString().Trim();

                frmPhieuNhap2_Them frm = new frmPhieuNhap2_Them();
                frm.ShowDialog();

                if (dateEdit_tungay.EditValue != null && dateEdit_denngay.EditValue != null && dateEdit_tungay.EditValue.ToString().Trim() != "" && dateEdit_denngay.EditValue.ToString().Trim() != "")
                    LoadLuoi(Convert.ToDateTime(dateEdit_tungay.EditValue).Date, Convert.ToDateTime(dateEdit_denngay.EditValue).Date);
            }
            else
            {
                MessageBox.Show("Không xác định được đối tượng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Cursor = Cursors.Default;
        }

        private void barButtonItem_xoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (gridView_phieunhap.DataRowCount > 0 && gridView_phieunhap.GetFocusedRowCellValue("SoPN") != null && gridView_phieunhap.GetFocusedRowCellValue("SoPN").ToString().Trim() != "")
            {
                DialogResult result = MessageBox.Show("Bạn có chắc là muốn xóa đối tượng này.", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result.ToString().Trim().ToUpper() == "YES")
                {
                    using (DataClasses_QLKHOHANGDataContext _db = new DataClasses_QLKHOHANGDataContext()) // su dung cuc bo - db duoc su dung trong pham vi using
                    {
                        System.Data.Common.DbTransaction trans = null;
                        _db.Connection.Open();
                        trans = _db.Connection.BeginTransaction();
                        _db.Transaction = trans;

                        try
                        {
                            // 1. DELETE CTPN
                            var ctpn = from p in _db.ChiTietPNs
                                       where p.SoPN == gridView_phieunhap.GetFocusedRowCellValue("SoPN").ToString().Trim()
                                       select p;
                            _db.ChiTietPNs.DeleteAllOnSubmit(ctpn);

                            // 2. DELETE PHIEU NHAP
                            var pn = from q in _db.PhieuNhaps
                                     where q.SoPN == gridView_phieunhap.GetFocusedRowCellValue("SoPN").ToString().Trim()
                                     select q;
                            _db.PhieuNhaps.DeleteAllOnSubmit(pn);

                            _db.SubmitChanges();
                            trans.Commit();

                            if (dateEdit_tungay.EditValue != null && dateEdit_denngay.EditValue != null && dateEdit_tungay.EditValue.ToString().Trim() != "" && dateEdit_denngay.EditValue.ToString().Trim() != "")
                                LoadLuoi(Convert.ToDateTime(dateEdit_tungay.EditValue).Date, Convert.ToDateTime(dateEdit_denngay.EditValue).Date);

                            MessageBox.Show("Đã xóa thành công");
                        }
                        catch (Exception ex)
                        {
                            if (trans != null)
                                trans.Rollback();

                            MessageBox.Show("Không thể thực hiện thao tác! \n Mã lỗi: " + ex.Message.Trim(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            _db.Connection.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không xác định được đối tượng cần xóa dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }
    }
}