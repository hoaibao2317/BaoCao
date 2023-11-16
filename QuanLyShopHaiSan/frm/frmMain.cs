using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyShopHaiSan.frm
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private bool ExistTabPage(TabControl control, string tabName)
        {
            bool check = false;
            for (int i = 0; i < control.TabPages.Count; i++)
            {
                if (control.TabPages[i].Name == tabName)
                {
                    check = true;
                    break;

                }
            }
            return check;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Class.Functions.Connect(); 
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            Class.Functions.Disconnect(); 
            Application.Exit(); 
        }

        private void mnuSanPham_Click(object sender, EventArgs e)
        {
            {
                TabPage tabPage = new TabPage();
                tabPage.Text = "Sản phẩm";
                tabPage.Name = "mnuSanPham";
                tabPage.ImageIndex = 1;
                Form frm = new frmDMSanPham();
                frm.TopLevel = false;
                frm.Parent = tabPage;
                frm.Dock = DockStyle.Fill;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Show();
                tabPage.Controls.Add(frm);
                if (!ExistTabPage(tapSach, "mnuSanPham"))
                {
                    tapSach.TabPages.Add(tabPage);
                }
                tapSach.SelectedTab = tapSach.TabPages["mnuSanPham"];
            }
        }

        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            {
                TabPage tabPage = new TabPage();
                tabPage.Text = "Nhân viên";
                tabPage.Name = "mnuNhanVien";
                tabPage.ImageIndex = 1;
                Form frm = new frmNhanVien();
                frm.TopLevel = false;
                frm.Parent = tabPage;
                frm.Dock = DockStyle.Fill;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Show();
                tabPage.Controls.Add(frm);
                if (!ExistTabPage(tapSach, "mnuNhanVien"))
                {
                    tapSach.TabPages.Add(tabPage);
                }
                tapSach.SelectedTab = tapSach.TabPages["mnuNhanVien"];
            }
        }

        private void mnuKhachHang_Click(object sender, EventArgs e)
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = "Khách hàng";
            tabPage.Name = "mnuKhachHang";
            tabPage.ImageIndex = 1;
            Form frm = new frmDMKhachHang();
            frm.TopLevel = false;
            frm.Parent = tabPage;
            frm.Dock = DockStyle.Fill;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Show();
            tabPage.Controls.Add(frm);
            if (!ExistTabPage(tapSach, "mnuKhachHang"))
            {
                tapSach.TabPages.Add(tabPage);
            }
            tapSach.SelectedTab = tapSach.TabPages["mnuKhachHang"];
        }

        private void mnuHoaDonBan_Click(object sender, EventArgs e)
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = "Hóa đơn";
            tabPage.Name = "mnuHoaDonBan";
            tabPage.ImageIndex = 1;
            Form frm = new frmHoaDonBan();
            frm.TopLevel = false;
            frm.Parent = tabPage;
            frm.Dock = DockStyle.Fill;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Show();
            tabPage.Controls.Add(frm);
            if (!ExistTabPage(tapSach, "mnuHoaDonBan"))
            {
                tapSach.TabPages.Add(tabPage);
            }
            tapSach.SelectedTab = tapSach.TabPages["mnuHoaDonBan"];
        }

        private void mnuFindHoaDon_Click(object sender, EventArgs e)
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = "Tìm hóa đơn";
            tabPage.Name = "mnuFindHoaDon";
            tabPage.ImageIndex = 1;
            Form frm = new frmTimHDBan();
            frm.TopLevel = false;
            frm.Parent = tabPage;
            frm.Dock = DockStyle.Fill;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Show();
            tabPage.Controls.Add(frm);
            if (!ExistTabPage(tapSach, "mnuFindHoaDon"))
            {
                tapSach.TabPages.Add(tabPage);
            }
            tapSach.SelectedTab = tapSach.TabPages["mnuFindHoaDon"];
        }

        private void LoaiSanPham_Click(object sender, EventArgs e)
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = "Loại sản phẩm";
            tabPage.Name = "mnuLoaiSanPham";
            tabPage.ImageIndex = 1;
            Form frm = new frmDMLoaiSanPham();
            frm.TopLevel = false;
            frm.Parent = tabPage;
            frm.Dock = DockStyle.Fill;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Show();
            tabPage.Controls.Add(frm);
            if (!ExistTabPage(tapSach, "mnuLoaiSanPham"))
            {
                tapSach.TabPages.Add(tabPage);
            }
            tapSach.SelectedTab = tapSach.TabPages["mnuLoaiSanPham"];
        }
    }
}
