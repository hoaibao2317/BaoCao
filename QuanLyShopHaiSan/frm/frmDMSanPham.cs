using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyShopHaiSan.Class;

namespace QuanLyShopHaiSan.frm
{
    public partial class frmDMSanPham : Form
    {
        public frmDMSanPham()
        {
            InitializeComponent();
        }
        DataTable tblH;

        private void frmDMSanPham_Load(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT * from tblLoaiSP";
            txtMaSP.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            LoadDataGridView();
            Functions.FillCombo(sql, cboMaLoaiSP, "MaLoaiSP", "TenSP");
            cboMaLoaiSP.SelectedIndex = -1;
            ResetValues();
        }
        private void ResetValues()
        {
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            cboMaLoaiSP.Text = "";
            txtSoLuong.Text = "0";
            txtDonGiaNhap.Text = "0";
            txtDonGiaBan.Text = "0";
            txtSoLuong.Enabled = true;
            txtDonGiaNhap.Enabled = false;
            txtDonGiaBan.Enabled = false;
            txtAnh.Text = "";
            picAnh.Image = null;
            txtGhichu.Text = "";
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT * from tblSanPham";
            tblH = Functions.GetDataToTable(sql);
            dgvHang.DataSource = tblH;
            dgvHang.Columns[0].HeaderText = "Mã sản phẩm";
            dgvHang.Columns[1].HeaderText = "Tên sản phẩm";
            dgvHang.Columns[2].HeaderText = "loại sản phẩm";
            dgvHang.Columns[3].HeaderText = "Số lượng";
            dgvHang.Columns[4].HeaderText = "Đơn giá nhập";
            dgvHang.Columns[5].HeaderText = "Đơn giá bán";
            dgvHang.Columns[6].HeaderText = "Ảnh";
            dgvHang.Columns[7].HeaderText = "Ghi chú";
            dgvHang.Columns[0].Width = 80;
            dgvHang.Columns[1].Width = 140;
            dgvHang.Columns[2].Width = 80;
            dgvHang.Columns[3].Width = 80;
            dgvHang.Columns[4].Width = 100;
            dgvHang.Columns[5].Width = 100;
            dgvHang.Columns[6].Width = 200;
            dgvHang.Columns[7].Width = 300;
            dgvHang.AllowUserToAddRows = false;
            dgvHang.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                string MaLoaiSP;
                string sql;
                if (btnThem.Enabled == false)
                {
                    MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaSP.Focus();
                    return;
                }
                if (tblH.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                txtMaSP.Text = dgvHang.CurrentRow.Cells["MaSP"].Value.ToString();
                txtTenSP.Text = dgvHang.CurrentRow.Cells["TenSP"].Value.ToString();
                MaLoaiSP = dgvHang.CurrentRow.Cells["MaLoaiSP"].Value.ToString();
                sql = "SELECT TenLoaiSP FROM tblLoaiSP WHERE MaLoaiSP=N'" + MaLoaiSP + "'";
                cboMaLoaiSP.Text = Functions.GetFieldValues(sql);
                txtSoLuong.Text = dgvHang.CurrentRow.Cells["SoLuong"].Value.ToString();
                txtDonGiaNhap.Text = dgvHang.CurrentRow.Cells["DonGiaNhap"].Value.ToString();
                txtDonGiaBan.Text = dgvHang.CurrentRow.Cells["DonGiaBan"].Value.ToString();
                sql = "SELECT Anh FROM tblSanPham WHERE MaSP=N'" + txtMaSP.Text + "'";
                txtAnh.Text = Functions.GetFieldValues(sql);
                picAnh.Image = Image.FromFile(txtAnh.Text);
                sql = "SELECT Ghichu FROM tblSanPham WHERE MaSP = N'" + txtMaSP.Text + "'";
                txtGhichu.Text = Functions.GetFieldValues(sql);
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnBoQua.Enabled = true;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnBoQua.Enabled = true;
                btnLuu.Enabled = true;
                btnThem.Enabled = false;
                ResetValues();
                txtMaSP.Enabled = true;
                txtMaSP.Focus();
                txtSoLuong.Enabled = true;
                txtDonGiaNhap.Enabled = true;
                txtDonGiaBan.Enabled = true;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            {
                string sql;
                if (txtMaSP.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập mã sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaSP.Focus();
                    return;
                }
                if (txtTenSP.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập tên sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTenSP.Focus();
                    return;
                }
                if (cboMaLoaiSP.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập loại sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaLoaiSP.Focus();
                    return;
                }
                if (txtAnh.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn phải chọn ảnh minh hoạ cho sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnOpen.Focus();
                    return;
                }
                sql = "SELECT MaSP FROM tblSanPham WHERE MaSP=N'" + txtMaSP.Text.Trim() + "'";
                if (Functions.CheckKey(sql))
                {
                    MessageBox.Show("Mã sản phẩm này đã tồn tại, bạn phải chọn mã sản phẩm khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaSP.Focus();
                    return;
                }
                sql = "INSERT INTO tblSanPham(MaSP,TenSP,MaLoaiSP,SoLuong,DonGiaNhap, DonGiaBan,Anh,Ghichu) VALUES(N'"
                    + txtMaSP.Text.Trim() + "',N'" + txtTenSP.Text.Trim() +
                    "',N'" + cboMaLoaiSP.SelectedValue.ToString() +
                    "'," + txtSoLuong.Text.Trim() + "," + txtDonGiaNhap.Text +
                    "," + txtDonGiaBan.Text + ",'" + txtAnh.Text + "',N'" + txtGhichu.Text.Trim() + "')";

                Functions.RunSQL(sql);
                LoadDataGridView();
                ResetValues();
                btnXoa.Enabled = true;
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnBoQua.Enabled = false;
                btnLuu.Enabled = false;
                txtMaSP.Enabled = false;
            }

        }

        private void btnSuu_Click(object sender, EventArgs e)
        
            {
                string sql;

                if (tblH == null || tblH.Rows == null || tblH.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Bạn chưa chọn bản ghi nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaSP.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTenSP.Text))
                {
                    MessageBox.Show("Bạn phải nhập tên sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTenSP.Focus();
                    return;
                }

                if (cboMaLoaiSP.SelectedValue == null || string.IsNullOrWhiteSpace(cboMaLoaiSP.SelectedValue.ToString()))
                {
                    MessageBox.Show("Bạn phải chọn loại sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaLoaiSP.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtAnh.Text))
                {
                    MessageBox.Show("Bạn phải chọn ảnh minh hoạ cho sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAnh.Focus();
                    return;
                }

                sql = "UPDATE tblSanPham SET TenSP=N'" + txtTenSP.Text.Trim() +
                    "',MaLoaiSP=N'" + cboMaLoaiSP.SelectedValue.ToString() +
                    "',SoLuong=" + txtSoLuong.Text +
                    ",Anh='" + txtAnh.Text + "',Ghichu=N'" + txtGhichu.Text + "' WHERE MaSP=N'" + txtMaSP.Text + "'";

                Functions.RunSQL(sql);
                LoadDataGridView();
                ResetValues();
                btnBoQua.Enabled = false;
            }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblH.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaSP.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xoá bản ghi này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE tblSanPham WHERE MaSP=N'" + txtMaSP.Text + "'";
                Functions.RunSqlDel(sql);
                LoadDataGridView();
                ResetValues();
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnThem.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaSP.Enabled = false;
        }

        private void btnMo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg)|*.jpg|GIF(*.gif)|*.gif|All files(*.*)|*.*";
            dlgOpen.FilterIndex = 2;
            dlgOpen.Title = "Chọn ảnh minh hoạ cho sản phẩm";
            const long maxImageSize = 10 * 1024 * 1024;
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                if (new FileInfo(dlgOpen.FileName).Length > maxImageSize)
                {
                    MessageBox.Show("Kích thước ảnh quá lớn. Vui lòng chọn ảnh khác.");
                }
                else
                {
                    
                    using (Image img = Image.FromFile(dlgOpen.FileName))
                    {
                        picAnh.Image = new Bitmap(img);
                        txtAnh.Text = dlgOpen.FileName;
                    }
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            {
                string sql;
                if ((txtMaSP.Text == "") && (txtTenSP.Text == "") && (cboMaLoaiSP.Text == ""))
                {
                    MessageBox.Show("Bạn hãy nhập điều kiện tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                sql = "SELECT * from tblSanPham WHERE 1=1";
                if (txtMaSP.Text != "")
                    sql += " AND MaSP LIKE N'%" + txtMaSP.Text + "%'";
                if (txtTenSP.Text != "")
                    sql += " AND TenSP LIKE N'%" + txtTenSP.Text + "%'";
                if (cboMaLoaiSP.Text != "")
                    sql += " AND MaLoaiSP LIKE N'%" + cboMaLoaiSP.SelectedValue + "%'";
                tblH = Functions.GetDataToTable(sql);
                if (tblH.Rows.Count == 0)
                    MessageBox.Show("Không có bản ghi thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else MessageBox.Show("Có " + tblH.Rows.Count + "  bản ghi thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvHang.DataSource = tblH;
                ResetValues();
            }
        }

        private void btnHienThi_Click(object sender, EventArgs e)
        {
            {
                string sql;
                sql = "SELECT MaSP,TenSP,MaLoaiSP,SoLuong,DonGiaNhap,DonGiaBan,Anh,Ghichu FROM tblSanPham";
                tblH = Functions.GetDataToTable(sql);
                dgvHang.DataSource = tblH;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
