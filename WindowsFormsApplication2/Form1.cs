using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        //luu bien
        int selectedRow;
        string CorrectFileName = null;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = "C:\\";
            open.Filter = "Image Files (*.jpg)|*.jpg|All Files(*.*)|*.*";
            open.FilterIndex = 1;

            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (open.CheckFileExists)
                {
                    string paths = Application.StartupPath.Substring(0, Application.StartupPath.Length - 10);
                    CorrectFileName = System.IO.Path.GetFileName(open.FileName);
                    System.IO.File.Copy(open.FileName, paths + "\\Images\\" + CorrectFileName);
                    
                    pic.Image = Image.FromFile(paths + "\\Images\\" + CorrectFileName);
                    MessageBox.Show("Tải ảnh thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy ảnh!");
                }
            }
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMSSV.Text == "")
            {
                MessageBox.Show("Nhập đầy đủ thông tin!");
                return;
            }
            if (txtHoTen.Text == "")
            {
                MessageBox.Show("Nhập đầy đủ thông tin!");
                return;
            }
            if (KiemTraMSSV(txtHoTen.Text))
            {
                MessageBox.Show("Tài khoản đã tồn tại! Vui lòng nhập MSSV khác");
                return;
            }

            string connectionString = @"Data Source=DESKTOP-G44GFH4\SQLEXPRESS;Initial Catalog=SINHVIEN;Integrated Security=True";
            //tao dt ket noi
            SqlConnection conn = new SqlConnection(connectionString);


            //mo ket noi
            conn.Open();

            //xu ly sql
            string sql = "INSERT INTO SINHVIEN (MSSV, HoTen, NgaySinh, GioiTinh, DiemTB, Path) VALUES ('{0}', N'{1}', '{2}', '{3}', '{4}', '{5}')";
            //truyen doi tuong
            string sqlFormat = string.Format(sql, txtMSSV.Text, txtHoTen.Text, dtpNgaySinh.Value.ToString("yyyy-MM-dd"), radNam.Checked ? 1 : 0, nudDiemTB.Value, "\\Images\\" + CorrectFileName);

            SqlCommand cmd = new SqlCommand(sqlFormat, conn); //câu lệnh cần thực thi và kết nổi
            
            //sử dụng Nonquery để select
            int count = cmd.ExecuteNonQuery();

            if (count > 0)
            {
                MessageBox.Show("Thêm thành công");
                Form1_Load(sender, e);
                clearInput();
            }
            else
            {
                MessageBox.Show("Thêm thất bại");
            }
            
            
            //dong ket noi
            conn.Close();
        }
        public void clearInput()
        {
            pic.Image = null;
            txtMSSV.Text = null;
            txtHoTen.Text = null;
            dtpNgaySinh.Value = DateTime.Now;
            radNam.Checked = true;
            radNu.Checked = false;
            nudDiemTB.Value = 0;
        }

        public bool KiemTraMSSV(string mssv)
        {
            string connectionString = @"Data Source=DESKTOP-G44GFH4\SQLEXPRESS;Initial Catalog=SINHVIEN;Integrated Security=True";
            //tao dt ket noi
            SqlConnection conn = new SqlConnection(connectionString);

            //mo ket noi
            conn.Open();

            //xu ly sql
            string sql = "SELECT * FROM SINHVIEN WHERE MSSV = '{0}'";
            //truyen doi tuong
            string sqlFormat = string.Format(sql, txtMSSV.Text);

            SqlCommand cmd = new SqlCommand(sqlFormat, conn); //câu lệnh cần thực thi và kết nổi

            //sử dụng Nonquery để select
            SqlDataReader data = cmd.ExecuteReader();

            //while(data.Read()) 
            //{

            //}

            if (data.HasRows)
            {
                return true;
            }

            
            //dong ket noi
            conn.Close();

            return false;
        }

        private void btnSLSV_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=DESKTOP-G44GFH4\SQLEXPRESS;Initial Catalog=SINHVIEN;Integrated Security=True";
            //tao ket noi
            SqlConnection conn = new SqlConnection(connectionString);

            //mo ket noi 
            conn.Open();

            //xu ly sql
            string sql = "SELECT COUNT(*) FROM SINHVIEN";

            SqlCommand cmd = new SqlCommand(sql, conn); //câu lệnh cần thực thi và kết nổi

            object data = cmd.ExecuteScalar();
            MessageBox.Show("Số lượng SV: " + data);

            //dong ket noi
            conn.Close();
        }

        //set lại cập nhật
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (txtMSSV.Text == "")
            {
                MessageBox.Show("Nhập MSSV để cập nhật!");
                return;
            }

            string connectionString = @"Data Source=DESKTOP-G44GFH4\SQLEXPRESS;Initial Catalog=SINHVIEN;Integrated Security=True";
            //tao dt ket noi
            SqlConnection conn = new SqlConnection(connectionString);



            //mo ket noi
            conn.Open();

            //xu ly sql
            string sql = "UPDATE SINHVIEN SET HoTen = N'{0}', NgaySinh = '{1}', GioiTinh = '{2}', DiemTB = '{3}', Path = '{4}' WHERE MSSV = '{5}'";
            //truyen doi tuong
            string sqlFormat = string.Format(sql, txtHoTen.Text, dtpNgaySinh.Value.ToString("yyyy-MM-dd"), radNam.Checked ? 1 : 0, nudDiemTB.Value, "\\Images\\" + CorrectFileName, txtMSSV.Text);

            SqlCommand cmd = new SqlCommand(sqlFormat, conn); //câu lệnh cần thực thi và kết nổi

            //sử dụng Nonquery để delete
            int count = cmd.ExecuteNonQuery();

            if (count > 0)
            {
                MessageBox.Show("Cập nhật thành công!");
                clearInput();
                Form1_Load(sender, e);
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại");
            }

            //dong ket noi
            conn.Close();

        }
        private void dgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = dgvSearch.Rows[selectedRow];
            txtMSSV.Text = row.Cells[0].Value.ToString();
            txtHoTen.Text = row.Cells[1].Value.ToString();
            dtpNgaySinh.Value = Convert.ToDateTime(row.Cells[2].Value.ToString());
            bool gt = Convert.ToBoolean(row.Cells[3].Value.ToString());
            if (gt)
            {
                radNam.Checked = true;
                radNu.Checked = false;
            }
            else
            {
                radNam.Checked = false;
                radNu.Checked = true;
            }
            nudDiemTB.Value = Convert.ToDecimal(row.Cells[4].Value.ToString());
            string paths = Application.StartupPath.Substring(0, Application.StartupPath.Length - 10);
            pic.Image = Image.FromFile(paths + row.Cells[5].Value.ToString());
            
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMSSV.Text == "")
            {
                MessageBox.Show("Bạn phải nhập MSSV!");
                return;
            }
            string message = "Bạn muốn xóa dữ liệu này ?";
            string title = "Delete";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                
                string connectionString = @"Data Source=DESKTOP-G44GFH4\SQLEXPRESS;Initial Catalog=SINHVIEN;Integrated Security=True";
                //tao dt ket noi
                SqlConnection conn = new SqlConnection(connectionString);

                //mo ket noi
                conn.Open();

                //xu ly sql
                string sql = "DELETE FROM SINHVIEN WHERE MSSV = '{0}'";
                //truyen doi tuong
                string sqlFormat = string.Format(sql, txtMSSV.Text);
                
                SqlCommand cmd = new SqlCommand(sqlFormat, conn); //câu lệnh cần thực thi và kết nổi

                //xoa file trong Images
                


                

                //sử dụng Nonquery để delete
                int count = cmd.ExecuteNonQuery();

                if (count > 0)
                {
                    
                    MessageBox.Show("Xóa thành công " + count + " đối tượng");
                    clearInput();
                    Form1_Load(sender, e);
                }
                else
                {
                    MessageBox.Show("Xóa thất bại");
                }

                //dong ket noi
                conn.Close();
            }
            else
            {
                return;
            }  
            
        }
       

        private void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=DESKTOP-G44GFH4\SQLEXPRESS;Initial Catalog=SINHVIEN;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            string sql = "SELECT * FROM SINHVIEN";
     
            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter(sql, conn);
            dap.Fill(ds);
            dgvSearch.DataSource = ds.Tables[0];
            dgvSearch.Update();
            dgvSearch.Refresh();

            conn.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=DESKTOP-G44GFH4\SQLEXPRESS;Initial Catalog=SINHVIEN;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            string sql = "SELECT * FROM SINHVIEN WHERE HoTen LIKE N'%{0}%'";
            string sqlFormat = string.Format(sql, txtSearch.Text);

            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter(sqlFormat, conn);
            dap.Fill(ds);
            dgvSearch.DataSource = ds.Tables[0];
            dgvSearch.Update();
            dgvSearch.Refresh();


            conn.Close();

        }

        

        private void dgvSearch_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            clearInput();
        }



        

        

        

        

       

         

        

        

        
    }
}
