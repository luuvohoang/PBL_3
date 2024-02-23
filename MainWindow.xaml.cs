using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace ComputerShop
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=ComputerShop;Integrated Security=True;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnShowCustomers_Click(object sender, RoutedEventArgs e)
        {
            List<KhachHang> khachHangs = new List<KhachHang>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM KHACHHANG";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            KhachHang khachHang = new KhachHang
                            {
                                IDKhachHang = Convert.ToInt32(reader["IDKhachHang"]),
                                Ho = reader["Ho"].ToString(),
                                Ten = reader["Ten"].ToString(),
                                Email = reader["Email"].ToString(),
                                SDT = reader["SDT"].ToString()
                            };
                            khachHangs.Add(khachHang);
                        }
                    }
                }
            }
            dataGrid.ItemsSource = khachHangs;
        }

        private void btnShowHangMuc_Click(object sender, RoutedEventArgs e)
        {
            List<HangMuc> hangMucs = new List<HangMuc>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM HANGMUC";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            HangMuc hangMuc = new HangMuc
                            {
                                IDHangMuc = Convert.ToInt32(reader["IDHangMuc"]),
                                Ten = reader["Ten"].ToString(),
                                MoTa = reader["MoTa"].ToString()
                            };
                            hangMucs.Add(hangMuc);
                        }
                    }
                }
            }
            dataGrid.ItemsSource = hangMucs;
        }

        private void btnShowSanPham_Click(object sender, RoutedEventArgs e)
        {
            List<SanPham> sanPhams = new List<SanPham>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM SANPHAM";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SanPham sanPham = new SanPham
                            {
                                IDSanPham = Convert.ToInt32(reader["IDSanPham"]),
                                TenSanPham = reader["TenSanPham"].ToString(),
                                MoTa = reader["MoTa"].ToString(),
                                GiaThanh = Convert.ToDouble(reader["GiaThanh"]),
                                SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                IDNhaPhanPhoi = Convert.ToInt32(reader["IDNhaPhanPhoi"]),
                                IDNhanVien = Convert.ToInt32(reader["IDNhanVien"]),
                            };
                            sanPhams.Add(sanPham);
                        }
                    }
                }
            }
            dataGrid.ItemsSource = sanPhams;
        }

        private void btnAddHangMuc_Click(object sender, RoutedEventArgs e)
        {
            InputDialog dialog = new InputDialog(new List<string> { "IDHangMuc", "Ten", "MoTa" });

            if (dialog.ShowDialog() == true)
            {
                List<string> responses = dialog.GetResponses();
                int newIDHangMuc = Convert.ToInt32(responses[0]);
                string newTen = responses[1];
                string newMoTa = responses[2];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO HANGMUC (IDHangMuc, Ten, MoTa) VALUES (@IDHangMuc, @Ten, @MoTa)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDHangMuc", newIDHangMuc);
                        command.Parameters.AddWithValue("@Ten", newTen);
                        command.Parameters.AddWithValue("@MoTa", newMoTa);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Information updated", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            InputDialog dialog = new InputDialog(new List<string> { "IDKhachHang", "Ho", "Ten", "Email", "SDT" });

            if (dialog.ShowDialog() == true)
            {
                List<string> responses = dialog.GetResponses();
                int newIDKhachHang = Convert.ToInt32(responses[0]);
                string newHo = responses[1];
                string newTen = responses[2];
                string newEmail = responses[3];
                string newSDT = responses[4];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO KHACHHANG (IDKhachHang, Ho, Ten, Email, SDT) VALUES (@IDKhachHang, @Ho, @Ten, @Email, @SDT)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDKhachHang", newIDKhachHang);
                        command.Parameters.AddWithValue("@Ho", newHo);
                        command.Parameters.AddWithValue("@Ten", newTen);
                        command.Parameters.AddWithValue("@Email", newEmail);
                        command.Parameters.AddWithValue("@SDT", newSDT);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Information updated", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnAddSanPham_Click(object sender, RoutedEventArgs e)
        {
            
            InputDialog dialog = new InputDialog(new List<string> { "IDSanPham", "TenSanPham", "MoTa", "GiaThanh", "SoLuong", "IDNhaPhanPhoi", "IDNhanVien" });

            if (dialog.ShowDialog() == true)
            {
                List<string> responses = dialog.GetResponses();
                int newIDSanPham = Convert.ToInt32(responses[0]);
                string newTen = responses[1];
                string newMoTa = responses[2];
                double newGiaThanh = Convert.ToDouble(responses[3]);
                int newSoLuong = Convert.ToInt32(responses[4]);
                int newIDNhaPhanPhoi = Convert.ToInt32(responses[5]);
                int newIDNhanVien = Convert.ToInt32(responses[6]);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO SANPHAM (IDSanPham, TenSanPham, MoTa, GiaThanh, SoLuong, IDNhaPhanPhoi, IDNhanVien) " +
                        "VALUES (@IDSanPham, @TenSanPham, @MoTa, @GiaThanh, @SoLuong, @IDNhaPhanPhoi, @IDNhanVien)";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDSanPham", newIDSanPham);
                        command.Parameters.AddWithValue("@TenSanPham", newTen);
                        command.Parameters.AddWithValue("@MoTa", newMoTa);
                        command.Parameters.AddWithValue("@GiaThanh", newGiaThanh);
                        command.Parameters.AddWithValue("@SoLuong", newSoLuong);
                        command.Parameters.AddWithValue("@IDNhaPhanPhoi", newIDNhaPhanPhoi);
                        command.Parameters.AddWithValue("@IDNhanVien", newIDNhanVien);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Information updated", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDeleteKhachHang_Click(object sender, EventArgs e)
        {
            DeleteDialog dialog = new DeleteDialog("Enter IDKhachHang:");

            if (dialog.ShowDialog() == true)
            {
                int idDelete = Convert.ToInt32(dialog.Response);

                using (SqlConnection connection = new SqlConnection(connectionString)) 
                {
                    connection.Open();

                    string sql = "DELETE FROM KHACHHANG WHERE IDKhachHang = @IDKhachHang";

                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        command.Parameters.AddWithValue("@IDKhachHang", idDelete);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Table updated", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

