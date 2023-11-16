using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; 
using QuanLyShopHaiSan.Class;

namespace QuanLyShopHaiSan.frm
{
    public partial class frmDMLoaiSanPham : Form
    {
        public frmDMLoaiSanPham()
        {
            InitializeComponent();
        }
        DataTable tblLoaiSP;
        private void frmDMLoaiSanPham_Load(object sender, EventArgs e)
        {
            txtMaLoaiSanPham.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            LoadDataGridView(); //Hiển thị bảng tblLoaiSP
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT MaLoaiSP, TenLoaiSP FROM tblLoaiSP";
            tblLoaiSP = Class.Functions.GetDataToTable(sql); //Đọc dữ liệu từ bảng
            dgvLoaiSanPham.DataSource = tblLoaiSP; //Nguồn dữ liệu            
            dgvLoaiSanPham.Columns[0].HeaderText = "Mã sản phẩm";
            dgvLoaiSanPham.Columns[1].HeaderText = "Tên sản phẩm";
            dgvLoaiSanPham.Columns[0].Width = 100;
            dgvLoaiSanPham.Columns[1].Width = 300;
            dgvLoaiSanPham.AllowUserToAddRows = false; //Không cho người dùng thêm dữ liệu trực tiếp
            dgvLoaiSanPham.EditMode = DataGridViewEditMode.EditProgrammatically; //Không cho sửa dữ liệu trực tiếp      
        }

        private void dgvLoaiSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                if (btnThem.Enabled == false)
                {
                    MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaLoaiSanPham.Focus();
                    return;
                }
                if (tblLoaiSP.Rows.Count == 0) //Nếu không có dữ liệu
                {
                    MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                txtMaLoaiSanPham.Text = dgvLoaiSanPham.CurrentRow.Cells["MaLoaiSP"].Value.ToString();
                txtTenLoaiSanPham.Text = dgvLoaiSanPham.CurrentRow.Cells["TenLoaiSP"].Value.ToString();
                btnSuu.Enabled = true;
                btnXoa.Enabled = true;
                btnBoQua.Enabled = true;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSuu.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValue(); //Xoá trắng các textbox
            txtMaLoaiSanPham.Enabled = true; //cho phép nhập mới
            txtTenLoaiSanPham.Focus();
        }
        private void ResetValue()
        {
            txtMaLoaiSanPham.Text = "";
            txtTenLoaiSanPham.Text = "";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            {
                string sql; //Lưu lệnh sql
                if (txtMaLoaiSanPham.Text.Trim().Length == 0) //Nếu chưa nhập mã chất liệu
                {
                    MessageBox.Show("Bạn phải nhập mã sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaLoaiSanPham.Focus();
                    return;
                }
                if (txtTenLoaiSanPham.Text.Trim().Length == 0) //Nếu chưa nhập tên chất liệu
                {
                    MessageBox.Show("Bạn phải nhập tên sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTenLoaiSanPham.Focus();
                    return;
                }
                sql = "Select MaLoaiSP From tblLoaiSP where MaLoaiSP=N'" + txtMaLoaiSanPham.Text.Trim() + "'";
                if (Class.Functions.CheckKey(sql))
                {
                    MessageBox.Show("Mã sản phẩm này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaLoaiSanPham.Focus();
                    return;
                }

                sql = "INSERT INTO tblLoaiSP VALUES(N'" +
                    txtMaLoaiSanPham.Text + "',N'" + txtTenLoaiSanPham.Text + "')";
                Class.Functions.RunSQL(sql); //Thực hiện câu lệnh sql
                LoadDataGridView(); //Nạp lại DataGridView
                ResetValue();
                btnXoa.Enabled = true;
                btnThem.Enabled = true;
                btnSuu.Enabled = true;
                btnBoQua.Enabled = false;
                btnLuu.Enabled = false;
                txtMaLoaiSanPham.Enabled = false;
            }
        }

        private void btnSuu_Click(object sender, EventArgs e)
        {
            {
                string sql; //Lưu câu lệnh sql
                if (tblLoaiSP.Rows.Count == 0)
                {
                    MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtMaLoaiSanPham.Text == "") //nếu chưa chọn bản ghi nào
                {
                    MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtTenLoaiSanPham.Text.Trim().Length == 0) //nếu chưa nhập tên chất liệu
                {
                    MessageBox.Show("Bạn chưa nhập tên sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                sql = "UPDATE tblLoaiSP SET TenLoaiSP=N'" +
                    txtTenLoaiSanPham.Text.ToString() +
                    "' WHERE MaLoaiSP=N'" + txtMaLoaiSanPham.Text + "'";
                Class.Functions.RunSQL(sql);
                LoadDataGridView();
                ResetValue();

                btnBoQua.Enabled = false;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            {
                string sql;
                if (tblLoaiSP.Rows.Count == 0)
                {
                    MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtMaLoaiSanPham.Text == "") //nếu chưa chọn bản ghi nào
                {
                    MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (MessageBox.Show("Bạn có muốn xoá không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    sql = "DELETE tblLoaiSP WHERE MaLoaiSP=N'" + txtMaLoaiSanPham.Text + "'";
                    Class.Functions.RunSqlDel(sql);
                    LoadDataGridView();
                    ResetValue();
                }
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValue();
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSuu.Enabled = true;
            btnLuu.Enabled = false;
            txtMaLoaiSanPham.Enabled = false;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
