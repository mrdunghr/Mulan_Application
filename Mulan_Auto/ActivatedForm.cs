using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Mulan_Auto
{
    public partial class ActivatedForm : Form
    {
        public ActivatedForm()
        {
            InitializeComponent();
        }
        private void ActivatedForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string enteredKey = txtInput.Text;

            // Kết nối đến cơ sở dữ liệu
            string constr = "server=localhost;user=root;password=1111;database=mulan_auto;port=3306;";
            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                conn.Open();

                Key key = ReadKeyFromDatabase(conn, enteredKey);

                if (key != null)
                {
                    // Kiểm tra key hết hạn trước
                    if (IsKeyExpired(key))
                    {
                        lbTrangThai.Text = "Key hết hạn";
                    }
                    else
                    {
                        // Key hợp lệ, kiểm tra tên máy tính và địa chỉ IP
                        string computerName = GetComputerName();
                        string ipAddress = GetIPAddress();

                        if (key.ComputerName.Equals(computerName) && key.IPAddress.Equals(ipAddress))
                        {
                            // Tên máy tính và địa chỉ IP khớp
                            // Thực hiện các thao tác tiếp theo ở đây
                            Mulan mulan = new Mulan();
                            mulan.Show();
                            lbTrangThai.Text = "Key của bạn chính xác";
                        }
                        else
                        {
                            // Tên máy tính hoặc địa chỉ IP không khớp
                            lbTrangThai.Text = "Bạn không có quyền sử dụng key này";
                        }
                    }
                }
                else
                {
                    // Key không hợp lệ
                    lbTrangThai.Text = "Key của bạn không chính xác";
                }

                conn.Close();
            }
        }
        private Key ReadKeyFromDatabase(MySqlConnection conn, string enteredKey)
        {
            string selectDetailQuery = "SELECT * FROM key_gen WHERE code_key = @enteredKey";
            using (MySqlCommand selectDetailCmd = new MySqlCommand(selectDetailQuery, conn))
            {
                selectDetailCmd.Parameters.AddWithValue("@enteredKey", enteredKey);

                using (MySqlDataReader reader = selectDetailCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Key key = new Key
                        {
                            CodeKey = reader["code_key"].ToString(),
                            Price = float.Parse(reader["price"].ToString()),
                            StartDate = DateTime.Parse(reader["start_date"].ToString()),
                            EndDate = DateTime.Parse(reader["end_date"].ToString()),
                            ComputerName = reader["computer_name"].ToString(),
                            IPAddress = reader["ip_address"].ToString(),
                            MACAddress = reader["mac_address"].ToString()
                        };

                        return key;
                    }
                }
            }

            return null; // Trả về null nếu không tìm thấy key
        }
        private bool IsKeyExpired(Key key)
        {
            if (key != null)
            {
                DateTime expirationDate = key.EndDate;
                DateTime currentDate = DateTime.Now;
                return expirationDate != null && currentDate.CompareTo(expirationDate) > 0;
            }

            return false; // Trả về false nếu key là null
        }
        private string GetComputerName()
        {
            return Environment.MachineName;
        }
        private string GetIPAddress()
        {
            try
            {
                string hostName = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

                foreach (IPAddress ip in hostEntry.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
            }

            return null; // Trả về null nếu không tìm thấy địa chỉ IP hợp lệ
        }
    }
}
