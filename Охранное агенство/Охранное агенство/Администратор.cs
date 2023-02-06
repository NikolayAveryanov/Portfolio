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
    public partial class Администратор : Form
    {
        public Администратор()
        {
            InitializeComponent();
        }
        SqlConnection sql = new SqlConnection(@"Data Source = sql; Initial Catalog = УП01_Киреева_Сабрина; Integrated Security = True");
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Администратор_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "уП01_Киреева_СабринаDataSet.Авторизация". При необходимости она может быть перемещена или удалена.
            this.авторизацияTableAdapter.Fill(this.уП01_Киреева_СабринаDataSet.Авторизация);

        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection sql = new SqlConnection(@"Data Source=sql;Initial Catalog=УП01_Киреева_Сабрина;Integrated Security=True");
            if (textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "" || textBox10.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                sql.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO [Авторизация] (Фамилия, Имя, Отчество, Логин, Пароль, Роль) VALUES (@a, @b, @c, @d, @e, @f)", sql))
                {
                    command.Parameters.AddWithValue("@a", textBox6.Text);
                    command.Parameters.AddWithValue("@b", textBox7.Text);
                    command.Parameters.AddWithValue("@c", textBox8.Text);
                    command.Parameters.AddWithValue("@d", textBox9.Text);
                    command.Parameters.AddWithValue("@e", textBox10.Text);
                    command.Parameters.AddWithValue("@f", comboBox1.Text);
                    command.ExecuteScalar();
                    sql.Close();
                    textBox6.Clear();
                    textBox7.Clear();
                    textBox8.Clear();
                    textBox9.Clear();
                    textBox10.Clear();
                    textBox6.Focus();
                    MessageBox.Show("Данные нового пользователя сохранены!");
                }
                SqlDataAdapter adapter;
                sql.Open();
                SqlCommand cmd = new SqlCommand("SELECT * From [Авторизация]", sql);
                adapter = new SqlDataAdapter(cmd);
                уП01_Киреева_СабринаDataSet.Clear();
                adapter.Fill(уП01_Киреева_СабринаDataSet, "Авторизация");
                sql.Close();
                dataGridView1.DataSource = уП01_Киреева_СабринаDataSet.Tables["Авторизация"];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sql.State != ConnectionState.Open)
                sql.Open();
            SqlCommand command = new SqlCommand("UPDATE [Авторизация] set [Логин]=@d, [Пароль]=@e WHERE [Фамилия]=@f", sql);
            command.Parameters.AddWithValue("d", textBox4.Text);
            command.Parameters.AddWithValue("e", textBox5.Text);
            command.Parameters.AddWithValue("f", textBox1.Text);
            try
            {
                command.ExecuteNonQuery();
                this.авторизацияTableAdapter.Fill(this.уП01_Киреева_СабринаDataSet.Авторизация);
                MessageBox.Show("Ваша запись была изменена", "Изменение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        string id;

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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int k = dataGridView1.SelectedRows[0].Index;
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    textBox1.Text = dataGridView1[0, k].Value.ToString();
                    textBox2.Text = dataGridView1[1, k].Value.ToString();
                    textBox3.Text = dataGridView1[2, k].Value.ToString();
                    textBox4.Text = dataGridView1[3, k].Value.ToString();
                    textBox5.Text = dataGridView1[4, k].Value.ToString();
                }
            }
            catch
            { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnection sql = new SqlConnection(@"Data Source = sql; Initial Catalog = УП01_Киреева_Сабрина; Integrated Security = True");
            if (textBox11.Text == "")
            {
                MessageBox.Show("Поле фамилия пользователя не заполнено!");
            }
            else
            {
                try
                {
                    sql.Open();
                    using (SqlCommand command = new SqlCommand("delete from [Авторизация] where [Фамилия] = " + "@a", sql))
                    {
                        command.Parameters.AddWithValue("@a", textBox11.Text);
                        command.ExecuteNonQuery();
                        sql.Close();
                    }
                    textBox11.Clear();
                    MessageBox.Show("Запись удалена!");

                    SqlDataAdapter adapter;
                    sql.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * From [Авторизация]", sql);
                    adapter = new SqlDataAdapter(cmd);
                    уП01_Киреева_СабринаDataSet.Clear();
                    adapter.Fill(уП01_Киреева_СабринаDataSet, "Авторизация");
                    sql.Close();
                    dataGridView1.DataSource = уП01_Киреева_СабринаDataSet.Tables["Авторизация"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка, данная запись связана с другими записями таблицы!");
                }
            }
            textBox2.Clear();
        }
    }
}
