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

namespace Охранное_агенство
{
    public partial class Диспетчер : Form
    {
        public Диспетчер()
        {
            InitializeComponent();
        }

        SqlConnection sql = new SqlConnection(@"Data Source=sql;Initial Catalog=УП01_Киреева_Сабрина;Integrated Security=True");
        private void Диспетчер_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "уП01_Киреева_СабринаDataSet.Договор". При необходимости она может быть перемещена или удалена.
            this.договорTableAdapter.Fill(this.уП01_Киреева_СабринаDataSet.Договор);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "уП01_Киреева_СабринаDataSet.Квартира". При необходимости она может быть перемещена или удалена.
            this.квартираTableAdapter.Fill(this.уП01_Киреева_СабринаDataSet.Квартира);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "уП01_Киреева_СабринаDataSet.Экипаж". При необходимости она может быть перемещена или удалена.
            this.экипажTableAdapter.Fill(this.уП01_Киреева_СабринаDataSet.Экипаж);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "уП01_Киреева_СабринаDataSet.Вызов". При необходимости она может быть перемещена или удалена.
            this.вызовTableAdapter.Fill(this.уП01_Киреева_СабринаDataSet.Вызов);
            //заполнение адреса
            string query = "select Договор.Код_договора, Квартира.Адрес_квартиры " +
                "from Договор join Квартира on Договор.Код_квартиры = Квартира.Код_квартиры";
            if (sql.State != ConnectionState.Open)
                sql.Open();
            using (SqlCommand command = new SqlCommand(query, sql))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox2.Items.Add(reader[0] + "." + reader[1]);
                    }
                }
            }

            query = "SELECT Код_экипажа, Командир_экипажа FROM Экипаж";
            if (sql.State != ConnectionState.Open)
                sql.Open();
            using (SqlCommand command = new SqlCommand(query, sql))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader[0] + "-" + reader[1]);
                    }
                }
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Сменить пользователя?", "Выход", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                Form1 a = new Form1();
                a.Show();
                this.Hide();
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
            
            }    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection sql = new SqlConnection(@"Data Source=sql;Initial Catalog=УП01_Киреева_Сабрина;Integrated Security=True");
            if (comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                sql.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO [Вызов] (Код_договора, Код_экипажа, Вызов_ложный, Величина_штрафа, Дата_время_вызова) VALUES (@a, @b, @c, @d, @e)", sql))
                {
                    command.Parameters.AddWithValue("@a", comboBox2.Text.Substring(0, comboBox2.Text.IndexOf('.')));
                    command.Parameters.AddWithValue("@b", comboBox1.Text.Substring(0, comboBox1.Text.IndexOf('-')));
                    if (checkBox1.Checked)
                        command.Parameters.AddWithValue("@c", "1");
                    else
                        command.Parameters.AddWithValue("@c", "0");
                    command.Parameters.AddWithValue("@d", textBox1.Text);
                    command.Parameters.AddWithValue("@e", Convert.ToDateTime(dateTimePicker1.Value));
                   
                    command.ExecuteScalar();
                    sql.Close();
                    MessageBox.Show("Данные вызова сохранены!");
                }
                SqlDataAdapter adapter;
                sql.Open();
                SqlCommand cmd = new SqlCommand("SELECT * From [Вызов]", sql);
                adapter = new SqlDataAdapter(cmd);
                уП01_Киреева_СабринаDataSet.Clear();
                adapter.Fill(уП01_Киреева_СабринаDataSet, "Вызов");
                sql.Close();
                dataGridView1.DataSource = уП01_Киреева_СабринаDataSet.Tables["Вызов"];
            }
        }
    }
}
