﻿using QuanLyShopHaiSan.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using COMExcel = Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyShopHaiSan.frm
{
    public partial class frmHoaDonBan : Form
    {
        public frmHoaDonBan()
        {
            InitializeComponent();
        }
        DataTable tblCTHDB;
        public void frmHoaDonBan_Load(object sender, EventArgs e)
        
            {
                btnThem.Enabled = true;
                btnLuu.Enabled = false;
                btnXoa.Enabled = false;
                btnInHoaDon.Enabled = false;
                txtMaHDBan.ReadOnly = true;
                txtTenNhanVien.ReadOnly = true;
                txtTenKhach.ReadOnly = true;
                txtDiaChi.ReadOnly = true;
                txtDienThoai.ReadOnly = true;
                txtTenSP.ReadOnly = true;
                txtDonGiaBan.ReadOnly = true;
                txtThanhTien.ReadOnly = true;
                txtTongTien.ReadOnly = true;
                txtGiamGia.Text = "0";
                txtTongTien.Text = "0";
                Functions.FillCombo("SELECT MaKhach, TenKhach FROM tblKhach", cboMaKhach, "MaKhach", "MaKhach");
                cboMaKhach.SelectedIndex = -1;
                Functions.FillCombo("SELECT MaNhanVien, TenNhanVien FROM tblNhanVien", cboMaNhanVien, "MaNhanVien", "TenKhach");
                cboMaNhanVien.SelectedIndex = -1;
                Functions.FillCombo("SELECT MaSP, TenSP FROM tblSanPham", cboMaSP, "MaSP", "MaSP");
                cboMaSP.SelectedIndex = -1;
                //Hiển thị thông tin của một hóa đơn được gọi từ form tìm kiếm
                if (txtMaHDBan.Text != "")
                {
                    LoadInfoHoaDon();
                    btnXoa.Enabled = true;
                    btnInHoaDon.Enabled = true;
                }
                LoadDataGridView();
            }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT a.MaSP, b.TenSP, a.SoLuong, b.DonGiaBan, a.GiamGia,a.ThanhTien FROM tblChiTietHD AS a, tblSanPham AS b WHERE a.MaHDBan = N'" + txtMaHDBan.Text + "' AND a.MaSP=b.MaSP";
            tblCTHDB = Functions.GetDataToTable(sql);
            dgvHDBanHang.DataSource = tblCTHDB;
            dgvHDBanHang.Columns[0].HeaderText = "Mã sản phẩm";
            dgvHDBanHang.Columns[1].HeaderText = "Tên sản phẩm";
            dgvHDBanHang.Columns[2].HeaderText = "Số lượng";
            dgvHDBanHang.Columns[3].HeaderText = "Đơn giá";
            dgvHDBanHang.Columns[4].HeaderText = "Giảm giá %";
            dgvHDBanHang.Columns[5].HeaderText = "Thành tiền";
            dgvHDBanHang.Columns[0].Width = 80;
            dgvHDBanHang.Columns[1].Width = 130;
            dgvHDBanHang.Columns[2].Width = 80;
            dgvHDBanHang.Columns[3].Width = 90;
            dgvHDBanHang.Columns[4].Width = 90;
            dgvHDBanHang.Columns[5].Width = 90;
            dgvHDBanHang.AllowUserToAddRows = false;
            dgvHDBanHang.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
        private void LoadInfoHoaDon()
        {
            string str;
            str = "SELECT NgayBan FROM tblHDBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
            txtNgayBan.Text = Functions.ConvertDateTime(Functions.GetFieldValues(str));
            str = "SELECT MaNhanVien FROM tblHDBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
            cboMaNhanVien.Text = Functions.GetFieldValues(str);
            str = "SELECT MaKhach FROM tblHDBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
            cboMaKhach.Text = Functions.GetFieldValues(str);
            str = "SELECT TongTien FROM tblHDBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
            txtTongTien.Text = Functions.GetFieldValues(str);
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            {
                btnXoa.Enabled = false;
                btnLuu.Enabled = true;
                btnInHoaDon.Enabled = false;
                btnThem.Enabled = false;
                ResetValues();
                txtMaHDBan.Text = Functions.CreateKey("HDB");
                LoadDataGridView();
            }
        }
        private void ResetValues()
        {
            txtMaHDBan.Text = "";
            txtNgayBan.Text = DateTime.Now.ToShortDateString();
            cboMaNhanVien.Text = "";
            cboMaKhach.Text = "";
            txtTongTien.Text = "0";
            cboMaSP.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            double sl, SLcon, tong, Tongmoi;
            sql = "SELECT MaHDBan FROM tblHDBan WHERE MaHDBan=N'" + txtMaHDBan.Text + "'";
            if (!Functions.CheckKey(sql))
            {
                // Mã hóa đơn chưa có, tiến hành lưu các thông tin chung
                // Mã HDBan được sinh tự động do đó không có trường hợp trùng khóa
                if (txtNgayBan.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập ngày bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNgayBan.Focus();
                    return;
                }
                if (cboMaNhanVien.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaNhanVien.Focus();
                    return;
                }
                if (cboMaKhach.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaKhach.Focus();
                    return;
                }
                sql = "INSERT INTO tblHDBan(MaHDBan, NgayBan, MaNhanVien, MaKhach, TongTien) VALUES (N'" + txtMaHDBan.Text.Trim() + "','" +
                        Functions.ConvertDateTime(txtNgayBan.Text.Trim()) + "',N'" + cboMaNhanVien.SelectedValue + "',N'" +
                        cboMaKhach.SelectedValue + "'," + txtTongTien.Text + ")";
                Functions.RunSQL(sql);
            }
            // Lưu thông tin của các mặt hàng
            if (cboMaSP.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập Mã sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaSP.Focus();
                return;
            }
            if ((txtSoLuong.Text.Trim().Length == 0) || (txtSoLuong.Text == "0"))
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            if (txtGiamGia.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập giảm giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGiamGia.Focus();
                return;
            }
            sql = "SELECT MaSP FROM tblChiTietHD WHERE MaSP=N'" + cboMaSP.SelectedValue + "' AND MaHDBan = N'" + txtMaHDBan.Text.Trim() + "'";
            if (Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã sản phẩm này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValuesHang();
                cboMaSP.Focus();
                return;
            }
            // Kiểm tra xem số lượng hàng trong kho còn đủ để cung cấp không?
            sl = Convert.ToDouble(Functions.GetFieldValues("SELECT SoLuong FROM tblSanPham WHERE MaSP = N'" + cboMaSP.SelectedValue + "'"));
            if (Convert.ToDouble(txtSoLuong.Text) > sl)
            {
                MessageBox.Show("Số lượng mặt hàng này chỉ còn " + sl, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            sql = "INSERT INTO tblChiTietHD(MaHDBan,MaSP,SoLuong,DonGia, GiamGia,ThanhTien) VALUES(N'" + txtMaHDBan.Text.Trim() + "',N'" + cboMaSP.SelectedValue + "'," + txtSoLuong.Text + "," + txtDonGiaBan.Text + "," + txtGiamGia.Text + "," + txtThanhTien.Text + ")";
            Functions.RunSQL(sql);
            LoadDataGridView();
            // Cập nhật lại số lượng của mặt hàng vào bảng tblSanPham
            SLcon = sl - Convert.ToDouble(txtSoLuong.Text);
            sql = "UPDATE tblSanPham SET SoLuong =" + SLcon + " WHERE MaSP= N'" + cboMaSP.SelectedValue + "'";
            Functions.RunSQL(sql);
            // Cập nhật lại tổng tiền cho hóa đơn bán
            tong = Convert.ToDouble(Functions.GetFieldValues("SELECT TongTien FROM tblHDBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'"));
            Tongmoi = tong + Convert.ToDouble(txtThanhTien.Text);
            sql = "UPDATE tblHDBan SET TongTien =" + Tongmoi + " WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
            Functions.RunSQL(sql);
            txtTongTien.Text = Tongmoi.ToString();
            ResetValuesHang();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnInHoaDon.Enabled = true;
        }
        private void ResetValuesHang()
        {
            cboMaSP.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            {
                double sl, slcon, slxoa;
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sql = "SELECT MaSP,SoLuong FROM tblChiTietHD WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
                    DataTable tblSanPham = Functions.GetDataToTable(sql);
                    for (int hang = 0; hang <= tblSanPham.Rows.Count - 1; hang++)
                    {
                        // Cập nhật lại số lượng cho các mặt hàng
                        sl = Convert.ToDouble(Functions.GetFieldValues("SELECT SoLuong FROM tblSanPham WHERE MaSP = N'" + tblSanPham.Rows[hang][0].ToString() + "'"));
                        slxoa = Convert.ToDouble(tblSanPham.Rows[hang][1].ToString());
                        slcon = sl + slxoa;
                        sql = "UPDATE tblSanPham SET SoLuong =" + slcon + " WHERE MaSP= N'" + tblSanPham.Rows[hang][0].ToString() + "'";
                        Functions.RunSQL(sql);
                    }

                    //Xóa chi tiết hóa đơn
                    sql = "DELETE tblChiTietHD WHERE MaHDBan=N'" + txtMaHDBan.Text + "'";
                    Functions.RunSqlDel(sql);

                    //Xóa hóa đơn
                    sql = "DELETE tblHDBan WHERE MaHDBan=N'" + txtMaHDBan.Text + "'";
                    Functions.RunSqlDel(sql);
                    ResetValues();
                    LoadDataGridView();
                    btnXoa.Enabled = false;
                    btnInHoaDon.Enabled = false;
                }
            }
        }

        private void cboMaNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaNhanVien.Text == "")
                txtTenNhanVien.Text = "";
            // Khi chọn Mã nhân viên thì tên nhân viên tự động hiện ra
            str = "Select TenNhanVien from tblNhanVien where MaNhanVien =N'" + cboMaNhanVien.SelectedValue + "'";
            txtTenNhanVien.Text = Functions.GetFieldValues(str);
        }

        private void cboMaKhach_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                string str;
                if (cboMaKhach.Text == "")
                {
                    txtTenKhach.Text = "";
                    txtDiaChi.Text = "";
                    txtDienThoai.Text = "";
                }
                //Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
                str = "Select TenKhach from tblKhach where MaKhach = N'" + cboMaKhach.SelectedValue + "'";
                txtTenKhach.Text = Functions.GetFieldValues(str);
                str = "Select DiaChi from tblKhach where MaKhach = N'" + cboMaKhach.SelectedValue + "'";
                txtDiaChi.Text = Functions.GetFieldValues(str);
                str = "Select DienThoai from tblKhach where MaKhach= N'" + cboMaKhach.SelectedValue + "'";
                txtDienThoai.Text = Functions.GetFieldValues(str);
            }
        }

        private void cboMaSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                string str;
                if (cboMaSP.Text == "")
                {
                    txtTenSP.Text = "";
                    txtDonGiaBan.Text = "";
                }
                // Khi chọn Mã sản phẩm thì các thông tin về hàng hiện ra
                str = "SELECT TenSP FROM tblSanPham WHERE MaSP =N'" + cboMaSP.SelectedValue + "'";
                txtTenSP.Text = Functions.GetFieldValues(str);
                str = "SELECT DonGiaBan FROM tblSanPham WHERE MaSP =N'" + cboMaSP.SelectedValue + "'";
                txtDonGiaBan.Text = Functions.GetFieldValues(str);
            }
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            {
                //Khi thay đổi số lượng thì thực hiện tính lại thành tiền
                double tt, sl, dg, gg;
                if (txtSoLuong.Text == "")
                    sl = 0;
                else
                    sl = Convert.ToDouble(txtSoLuong.Text);
                if (txtGiamGia.Text == "")
                    gg = 0;
                else
                    gg = Convert.ToDouble(txtGiamGia.Text);
                if (txtDonGiaBan.Text == "")
                    dg = 0;
                else
                    dg = Convert.ToDouble(txtDonGiaBan.Text);
                tt = sl * dg - sl * dg * gg / 100;
                txtThanhTien.Text = tt.ToString();
            }
        }
        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else e.Handled = true;
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            {
                //Khi thay đổi giảm giá thì tính lại thành tiền
                double tt, sl, dg, gg;
                if (txtSoLuong.Text == "")
                    sl = 0;
                else
                    sl = Convert.ToDouble(txtSoLuong.Text);
                if (txtGiamGia.Text == "")
                    gg = 0;
                else
                    gg = Convert.ToDouble(txtGiamGia.Text);
                if (txtDonGiaBan.Text == "")
                    dg = 0;
                else
                    dg = Convert.ToDouble(txtDonGiaBan.Text);
                tt = sl * dg - sl * dg * gg / 100;
                txtThanhTien.Text = tt.ToString();
            }
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            {
                // Khởi động chương trình Excel
                COMExcel.Application exApp = new COMExcel.Application();
                COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
                COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet
                COMExcel.Range exRange;
                string sql;
                int hang = 0, cot = 0;
                DataTable tblThongtinHD, tblThongtinHang;
                exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
                exSheet = exBook.Worksheets[1];
                // Định dạng chung
                exRange = exSheet.Cells[1, 1];
                exRange.Range["A1:Z300"].Font.Name = "Times new roman"; //Font chữ
                exRange.Range["A1:B3"].Font.Size = 10;
                exRange.Range["A1:B3"].Font.Bold = true;
                exRange.Range["A1:B3"].Font.ColorIndex = 5; //Màu xanh da trời
                exRange.Range["A1:A1"].ColumnWidth = 7;
                exRange.Range["B1:B1"].ColumnWidth = 15;
                exRange.Range["A1:B1"].MergeCells = true;
                exRange.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["A1:B1"].Value = "Shop Hải Sản";
                exRange.Range["A2:B2"].MergeCells = true;
                exRange.Range["A2:B2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["A2:B2"].Value = "Long An";
                exRange.Range["A3:B3"].MergeCells = true;
                exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["A3:B3"].Value = "Điện thoại: (04)38526419";
                exRange.Range["C2:E2"].Font.Size = 16;
                exRange.Range["C2:E2"].Font.Bold = true;
                exRange.Range["C2:E2"].Font.ColorIndex = 3; //Màu đỏ
                exRange.Range["C2:E2"].MergeCells = true;
                exRange.Range["C2:E2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["C2:E2"].Value = "HÓA ĐƠN BÁN";
                // Biểu diễn thông tin chung của hóa đơn bán
                sql = "SELECT a.MaHDBan, a.NgayBan, a.TongTien, b.TenKhach, b.DiaChi, b.DienThoai, c.TenNhanVien FROM tblHDBan AS a, tblKhach AS b, tblNhanVien AS c WHERE a.MaHDBan = N'" + txtMaHDBan.Text + "' AND a.MaKhach = b.MaKhach AND a.MaNhanVien = c.MaNhanVien";
                tblThongtinHD = Functions.GetDataToTable(sql);
                exRange.Range["B6:C9"].Font.Size = 12;
                exRange.Range["B6:B6"].Value = "Mã hóa đơn:";
                exRange.Range["C6:E6"].MergeCells = true;
                exRange.Range["C6:E6"].Value = tblThongtinHD.Rows[0][0].ToString();
                exRange.Range["B7:B7"].Value = "Khách hàng:";
                exRange.Range["C7:E7"].MergeCells = true;
                exRange.Range["C7:E7"].Value = tblThongtinHD.Rows[0][3].ToString();
                exRange.Range["B8:B8"].Value = "Địa chỉ:";
                exRange.Range["C8:E8"].MergeCells = true;
                exRange.Range["C8:E8"].Value = tblThongtinHD.Rows[0][4].ToString();
                exRange.Range["B9:B9"].Value = "Điện thoại:";
                exRange.Range["C9:E9"].MergeCells = true;
                exRange.Range["C9:E9"].Value = tblThongtinHD.Rows[0][5].ToString();
                //Lấy thông tin các mặt hàng
                sql = "SELECT b.TenSP, a.SoLuong, b.DonGiaBan, a.GiamGia, a.ThanhTien " +
                      "FROM tblChiTietHD AS a , tblSanPham AS b WHERE a.MaHDBan = N'" +
                      txtMaHDBan.Text + "' AND a.MaSP = b.MaSP";
                tblThongtinHang = Functions.GetDataToTable(sql);
                //Tạo dòng tiêu đề bảng
                exRange.Range["A11:F11"].Font.Bold = true;
                exRange.Range["A11:F11"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["C11:F11"].ColumnWidth = 12;
                exRange.Range["A11:A11"].Value = "STT";
                exRange.Range["B11:B11"].Value = "Tên sản phẩm";
                exRange.Range["C11:C11"].Value = "Số lượng";
                exRange.Range["D11:D11"].Value = "Đơn giá";
                exRange.Range["E11:E11"].Value = "Giảm giá";
                exRange.Range["F11:F11"].Value = "Thành tiền";
                for (hang = 0; hang < tblThongtinHang.Rows.Count; hang++)
                {
                    //Điền số thứ tự vào cột 1 từ dòng 12
                    exSheet.Cells[1][hang + 12] = hang + 1;
                    for (cot = 0; cot < tblThongtinHang.Columns.Count; cot++)
                    //Điền thông tin hàng từ cột thứ 2, dòng 12
                    {
                        exSheet.Cells[cot + 2][hang + 12] = tblThongtinHang.Rows[hang][cot].ToString();
                        if (cot == 3) exSheet.Cells[cot + 2][hang + 12] = tblThongtinHang.Rows[hang][cot].ToString() + "%";
                    }
                }
                exRange = exSheet.Cells[cot][hang + 14];
                exRange.Font.Bold = true;
                exRange.Value2 = "Tổng tiền:";
                exRange = exSheet.Cells[cot + 1][hang + 14];
                exRange.Font.Bold = true;
                exRange.Value2 = tblThongtinHD.Rows[0][2].ToString();
                exRange = exSheet.Cells[1][hang + 15]; //Ô A1 
                exRange.Range["A1:F1"].MergeCells = true;
                exRange.Range["A1:F1"].Font.Bold = true;
                exRange.Range["A1:F1"].Font.Italic = true;
                exRange.Range["A1:F1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignRight;
                exRange = exSheet.Cells[4][hang + 17]; //Ô A1 
                exRange.Range["A1:C1"].MergeCells = true;
                exRange.Range["A1:C1"].Font.Italic = true;
                exRange.Range["A1:C1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                DateTime d = Convert.ToDateTime(tblThongtinHD.Rows[0][1]);
                exRange.Range["A1:C1"].Value = "Hà Nội, ngày " + d.Day + " tháng " + d.Month + " năm " + d.Year;
                exRange.Range["A2:C2"].MergeCells = true;
                exRange.Range["A2:C2"].Font.Italic = true;
                exRange.Range["A2:C2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["A2:C2"].Value = "Nhân viên bán hàng";
                exRange.Range["A6:C6"].MergeCells = true;
                exRange.Range["A6:C6"].Font.Italic = true;
                exRange.Range["A6:C6"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["A6:C6"].Value = tblThongtinHD.Rows[0][6];
                exSheet.Name = "Hóa đơn nhập";
                exApp.Visible = true;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            {
                if (cboMaHD.Text == "")
                {
                    MessageBox.Show("Bạn phải chọn một mã hóa đơn để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaHD.Focus();
                    return;
                }
                txtMaHDBan.Text = cboMaHD.Text;
                LoadInfoHoaDon();
                LoadDataGridView();
                btnXoa.Enabled = true;
                btnLuu.Enabled = true;
                btnInHoaDon.Enabled = true;
                cboMaHD.SelectedIndex = -1;
            }
        }

        

        private void dgvHDBanHang_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string MaSPxoa, sql;
            Double ThanhTienxoa, SoLuongxoa, sl, slcon, tong, tongmoi;
            if (tblCTHDB.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                //Xóa hàng và cập nhật lại số lượng hàng 
                MaSPxoa = dgvHDBanHang.CurrentRow.Cells["MaSP"].Value.ToString();
                SoLuongxoa = Convert.ToDouble(dgvHDBanHang.CurrentRow.Cells["SoLuong"].Value.ToString());
                ThanhTienxoa = Convert.ToDouble(dgvHDBanHang.CurrentRow.Cells["ThanhTien"].Value.ToString());
                sql = "DELETE tblChiTietHD WHERE MaHDBan=N'" + txtMaHDBan.Text + "' AND MaSP = N'" + MaSPxoa + "'";
                Functions.RunSQL(sql);
                // Cập nhật lại số lượng cho các mặt hàng
                sl = Convert.ToDouble(Functions.GetFieldValues("SELECT SoLuong FROM tblSanPham WHERE MaSP = N'" + MaSPxoa + "'"));
                slcon = sl + SoLuongxoa;
                sql = "UPDATE tblSanPham SET SoLuong =" + slcon + " WHERE MaSP= N'" + MaSPxoa + "'";
                Functions.RunSQL(sql);
                // Cập nhật lại tổng tiền cho hóa đơn bán
                tong = Convert.ToDouble(Functions.GetFieldValues("SELECT TongTien FROM tblHDBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'"));
                tongmoi = tong - ThanhTienxoa;
                sql = "UPDATE tblHDBan SET TongTien =" + tongmoi + " WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
                Functions.RunSQL(sql);
                txtTongTien.Text = tongmoi.ToString();
                LoadDataGridView();
            }
        }

        private void cboMaSP_SelectedIndexChanged_1(object sender, EventArgs e)
        {            
                string str;
                if (cboMaSP.Text == "")
                {
                    txtTenSP.Text = "";
                    txtDonGiaBan.Text = "";
                }
                // Khi chọn mã hàng thì các thông tin về hàng hiện ra
                str = "SELECT TenSP FROM tblSanPham WHERE MaSP =N'" + cboMaSP.SelectedValue + "'";
                txtTenSP.Text = Functions.GetFieldValues(str);
                str = "SELECT DonGiaBan FROM tblSanPham WHERE MaSP =N'" + cboMaSP.SelectedValue + "'";
                txtDonGiaBan.Text = Functions.GetFieldValues(str);
            
        }

        private void cboMaHD_DropDown(object sender, EventArgs e)
        {
            {
                Functions.FillCombo("SELECT MaHDBan FROM tblHDBan", cboMaHD, "MaHDBan", "MaHDBan");
                cboMaHD.SelectedIndex = -1;
            }

        }
    }

}
     

