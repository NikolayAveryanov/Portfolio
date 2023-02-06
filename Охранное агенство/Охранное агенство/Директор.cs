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
    public partial class Директор : Form
    {
        public Директор()
        {
            InitializeComponent();
        }
        SqlConnection sql = new SqlConnection(@"Data Source=sql;Initial Catalog=УП01_Киреева_Сабрина;Integrated Security=True");
        private void Директор_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "уП01_Киреева_СабринаDataSet.Экипаж". При необходимости она может быть перемещена или удалена.
            this.экипажTableAdapter.Fill(this.уП01_Киреева_СабринаDataSet.Экипаж);
            if (sql.State != ConnectionState.Open)
                sql.Open();
            comboBox1.Items.Clear();
            //список адресов
            string query = "SELECT Код_квартиры, Адрес_квартиры FROM Квартира";
            using (SqlCommand command = new SqlCommand(query, sql))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader[0]+"."+reader[1]);
                    }
                }
            }
            sql.Close();

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

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection sql = new SqlConnection(@"Data Source=sql;Initial Catalog=УП01_Киреева_Сабрина;Integrated Security=True");
            sql.Open();
            SqlCommand command = new SqlCommand("UPDATE [Экипаж] set [Командир_экипажа]=@a, [Марка_автомобиля]=@b WHERE [Код_экипажа]=@f", sql);
            command.Parameters.AddWithValue("a", textBox1.Text);
            command.Parameters.AddWithValue("b", textBox2.Text);
            command.Parameters.AddWithValue("f", id);
            try
            {

                command.ExecuteNonQuery();
                this.экипажTableAdapter.Fill(this.уП01_Киреева_СабринаDataSet.Экипаж);
                MessageBox.Show("Ваша запись была изменена", "Изменение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        string id;

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int k = dataGridView1.SelectedRows[0].Index;
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    id = dataGridView1[0, k].Value.ToString();
                    textBox1.Text = dataGridView1[1, k].Value.ToString();
                    textBox2.Text = dataGridView1[2, k].Value.ToString();
                }
            }
            catch
            { }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            { return; }
            else
            { e.Handled = true; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var helper = new Document("Отчёт о квартирах.docx");
            var items = new Dictionary<string, string>
            {
                {"<ADDRESS>", comboBox1.SelectedItem.ToString().Substring(comboBox1.SelectedItem.ToString().IndexOf('.')+1)},
                {"<FIO>",textBox5.Text.Substring(textBox5.Text.IndexOf('-')+1)},
                {"<TOTAL>", textBox3.Text },
                {"<FALSE>", textBox4.Text },
                {"<DATE>", dateTimePicker3.Value.ToString("dd.MM.yyyy")},
            };
            helper.Process(items, "Отчёт о квартирах");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sql.State != ConnectionState.Open)
                sql.Open();
            //заполнение количества вызовов
            string query = String.Format("SELECT Квартира.Адрес_квартиры, COUNT(Вызов.Код_договора) AS Общее_кол_во_вызовов " +
                "FROM Вызов JOIN(Квартира JOIN Договор ON Квартира.Код_квартиры = Договор.Код_квартиры) " +
                "ON Вызов.Код_договора = Договор.Код_договора where Квартира.Адрес_квартиры='{0}' " +
                "GROUP BY Квартира.Адрес_квартиры", comboBox1.SelectedItem.ToString().Substring(comboBox1.SelectedItem.ToString().IndexOf('.')+1));
            using (SqlCommand command = new SqlCommand(query, sql))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    if (reader.HasRows)
                    {
                        textBox3.Text = reader[1].ToString();
                    }
                }
            }
            //заполнение клиента
            query = String.Format("select Квартира.Регистрационный_номер, concat(Клиент.Фамилия, ' ', Клиент.Имя, ' ', Клиент.Отчество) " +
                "from Квартира join Клиент on Квартира.Регистрационный_номер = Клиент.Регистрационный_номер " +
                "where Квартира.Адрес_квартиры = '{0}'", comboBox1.SelectedItem.ToString().Substring(comboBox1.SelectedItem.ToString().IndexOf('.') + 1));
            using (SqlCommand command = new SqlCommand(query, sql))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    if (reader.HasRows)
                    {
                        textBox5.Text = reader[0].ToString()+"-"+reader[1].ToString();
                    }
                }
            }
            //заполнение ложных вызовов
            query = String.Format("select Квартира.Адрес_квартиры, COUNT(Вызов.Вызов_ложный) " +
                "from Вызов join (Договор join Квартира on Договор.Код_квартиры = Квартира.Код_квартиры) on Вызов.Код_договора = Договор.Код_договора" +
                " where Вызов.Вызов_ложный = 'True' AND Квартира.Адрес_квартиры = '{0}'" +
                " group by Квартира.Адрес_квартиры", comboBox1.SelectedItem.ToString().Substring(comboBox1.SelectedItem.ToString().IndexOf('.') + 1));
            using (SqlCommand command = new SqlCommand(query, sql))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    if (reader.HasRows)
                    {
                        textBox4.Text = reader[1].ToString();
                    }
                }
            }

        }
    }
}
