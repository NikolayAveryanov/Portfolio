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
using System.Data.Sql;
using System.IO;

/* Имя файла: "Охранное агенство" 
   Выполнил: Киреева Сабрина
   Версия языка: Visual Studio 2022 LTSC 17.0 */
namespace Охранное_агенство
{
    public partial class Менеджер : Form
    {
        public Менеджер()
        {
            InitializeComponent();
        }
        //Подключение базы данных SQL к Visual Studio 
        SqlConnection sql = new SqlConnection(@"Data Source=sql;Initial Catalog=УП01_Киреева_Сабрина;Integrated Security=True");

        private void Менеджер_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "уП01_Киреева_СабринаDataSet.Квартира". При необходимости она может быть перемещена или удалена.
            this.квартираTableAdapter.Fill(this.уП01_Киреева_СабринаDataSet.Квартира);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "уП01_Киреева_СабринаDataSet.Клиент". При необходимости она может быть перемещена или удалена.
            this.клиентTableAdapter.Fill(this.уП01_Киреева_СабринаDataSet.Клиент);
            //Заполнение списка владельцов
            comboBox1.Items.Clear();
            string query = "SELECT Регистрационный_номер, Фамилия, Имя, Отчество FROM Клиент";
            if (sql.State != ConnectionState.Open)
                sql.Open();
            using (SqlCommand command = new SqlCommand(query, sql))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader[0] + "-" + reader[1] + " " + reader[2] + " " + reader[3]);
                    }
                }
            }
            //Заполнение скиска ФИО
            query = "SELECT CONCAT(Фамилия, ' ', Имя, ' ', Отчество) FROM Клиент";
            using (SqlCommand command = new SqlCommand(query, sql))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox5.Items.Add(reader[0]);
                    }
                }
            }
            //Заполнение списка адресов
            query = "SELECT Адрес_квартиры FROM Квартира";
            using (SqlCommand command = new SqlCommand(query, sql))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox6.Items.Add(reader[0]);
                    }
                }
            }
            sql.Close();

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            //Переход с формы менеджера на форму авторизации
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
            //Подключение базы данных SQl
            SqlConnection sql = new SqlConnection(@"Data Source=sql;Initial Catalog=УП01_Киреева_Сабрина;Integrated Security=True");
            //Считывание данных с компонентов формы
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                sql.Open();
                //Запрос на добавление клиента
                using (SqlCommand command = new SqlCommand("INSERT INTO [Клиент] (Адрес_клиента, Фамилия, Имя, Отчество, Телефон) VALUES (@a, @b, @c, @d, @e)", sql))
                {
                    //Построчная запись в базу данных
                    command.Parameters.AddWithValue("@a", textBox1.Text);
                    command.Parameters.AddWithValue("@b", textBox2.Text);
                    command.Parameters.AddWithValue("@c", textBox3.Text);
                    command.Parameters.AddWithValue("@d", textBox4.Text);
                    command.Parameters.AddWithValue("@e", textBox5.Text);
                    command.ExecuteScalar();
                    sql.Close();
                    //Очистка полей
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox1.Focus();
                    MessageBox.Show("Данные нового клиента сохранены!");
                }
                //Запрос в базу данных на вывод данных на форму 
                SqlDataAdapter adapter;
                sql.Open();
                SqlCommand cmd = new SqlCommand("SELECT * From [Клиент]", sql);
                adapter = new SqlDataAdapter(cmd);
                уП01_Киреева_СабринаDataSet.Clear();
                adapter.Fill(уП01_Киреева_СабринаDataSet, "Клиент");
                sql.Close();
                dataGridView1.DataSource = уП01_Киреева_СабринаDataSet.Tables["Клиент"];
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Блокировка всех символов на клавиатуре, кроме букв 
            if (!Char.IsDigit(e.KeyChar))
            { return; }
            else
            { e.Handled = true; }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Блокировка всех символов на клавиатуре, кроме букв 
            if (!Char.IsDigit(e.KeyChar))
            { return; }
            else
            { e.Handled = true; }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Блокировка всех символов на клавиатуре, кроме букв 
            if (!Char.IsDigit(e.KeyChar))
            { return; }
            else
            { e.Handled = true; }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Блокировка всех букв на клавиатуре
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
                e.Handled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Подключение базы данных SQL 
            SqlConnection sql = new SqlConnection(@"Data Source = sql; Initial Catalog = УП01_Киреева_Сабрина; Integrated Security = True");
            //Считывание данных с компонента формы 
            if (textBox11.Text == "")
            {
                MessageBox.Show("Поле код клиента не заполнено!");
            }
            else
            {
                try
                {
                    sql.Open();
                    //Запрос на удаление пользователей 
                    using (SqlCommand command = new SqlCommand("delete from [Клиент] where [Регистрационный_номер] = " + "@a", sql))
                    {
                        command.Parameters.AddWithValue("@a", textBox11.Text);
                        command.ExecuteNonQuery();
                        sql.Close();
                    }
                    textBox11.Clear();
                    MessageBox.Show("Запись удалена!");
                    //Запрос на вывод данных на форму с базы данных таблицы Клиент
                    SqlDataAdapter adapter;
                    sql.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * From [Клиент]", sql);
                    adapter = new SqlDataAdapter(cmd);
                    уП01_Киреева_СабринаDataSet.Clear();
                    adapter.Fill(уП01_Киреева_СабринаDataSet, "Клиент");
                    sql.Close();
                    dataGridView1.DataSource = уП01_Киреева_СабринаDataSet.Tables["Клиент"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка, данная запись связана с другими записями таблицы!");
                }
            }
            textBox2.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sql.Open();
            //Запрос на изменение данных в базе данных таблицы Клиент
            SqlCommand command = new SqlCommand("UPDATE [Клиент] set [Адрес_клиента]=@a, [Фамилия]=@b, [Имя]=@c, [Отчество]=@d, [Телефон]=@e WHERE [Регистрационный_номер]=@f", sql);
            //Построчное считывание данных с компонентов формы
            command.Parameters.AddWithValue("a", textBox6.Text);
            command.Parameters.AddWithValue("b", textBox7.Text);
            command.Parameters.AddWithValue("c", textBox8.Text);
            command.Parameters.AddWithValue("d", textBox9.Text);
            command.Parameters.AddWithValue("e", textBox10.Text);
            command.Parameters.AddWithValue("f", id);
            try
            {
                command.ExecuteNonQuery();
                this.клиентTableAdapter.Fill(this.уП01_Киреева_СабринаDataSet.Клиент);
                MessageBox.Show("Ваша запись была изменена", "Изменение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Data.SqlClient.SqlException)
            { }
        }
        string id;

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Блокировка всех символов клавиатуры, кроме букв
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
            //Блокировка всех символов клавиатуры, кроме букв
            if (!Char.IsDigit(e.KeyChar))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Блокировка всех символов клавиатуры, кроме букв
            if (!Char.IsDigit(e.KeyChar))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Блокировка букв на клавиатуре
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
                e.Handled = true;
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Блокировка букв на клавиатуре
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
                e.Handled = true;
        }

        private void dataGridView1_SelectionChanged_1(object sender, EventArgs e)
        {
            //Обработка двойного нажатия на строку в таблицу и вывод данных на компоненты формы
            try
            {
                int k = dataGridView1.SelectedRows[0].Index;
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    id = dataGridView1[0, k].Value.ToString();
                    textBox6.Text = dataGridView1[1, k].Value.ToString();
                    textBox7.Text = dataGridView1[2, k].Value.ToString();
                    textBox8.Text = dataGridView1[3, k].Value.ToString();
                    textBox9.Text = dataGridView1[4, k].Value.ToString();
                    textBox10.Text = dataGridView1[5, k].Value.ToString();
                }
            }
            catch
            { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Объявление массива байт
            byte[] img = null;
            //Подключение базы данных SQL 
            SqlConnection sql = new SqlConnection(@"Data Source=sql;Initial Catalog=УП01_Киреева_Сабрина;Integrated Security=True");
            //Считывание данных с компонентов формы 
            if (comboBox1.Text == "" || textBox12.Text == "" || comboBox2.Text == "" || comboBox3.Text == "" || comboBox4.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                sql.Open();
                using (MemoryStream memory = new MemoryStream()) //преобразование фото в массив байт
                {
                    pictureBox3.Image.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                    img = memory.ToArray();
                }
                //Запрос на добавление данных в базу данных таблицу Квартира 
                using (SqlCommand command = new SqlCommand("INSERT INTO [Квартира] (Регистрационный_номер, Адрес_квартиры, " +
                    "Наличие_код_замка, Количество_этажей, Этаж_квартиры, Тип_дома, Тип_двери, Наличие_балкона, Тип_балкона, План_квартиры) " +
                    "VALUES (@a, @b, @c, @d, @e, @f, @g, @h, @i, @q)", sql))
                {
                    //Построчное считывание данных с компонентов формы 
                    command.Parameters.AddWithValue("@a", comboBox1.Text.Substring(0, comboBox1.Text.IndexOf('-')));
                    command.Parameters.AddWithValue("@b", textBox12.Text);
                    if(checkBox1.Checked)
                        command.Parameters.AddWithValue("@c", "1");
                    else
                        command.Parameters.AddWithValue("@c", "0");
                    command.Parameters.AddWithValue("@d", numericUpDown1.Text);
                    command.Parameters.AddWithValue("@e", numericUpDown2.Text);
                    command.Parameters.AddWithValue("@f", comboBox2.Text);
                    command.Parameters.AddWithValue("@g", comboBox3.Text);
                    if(checkBox2.Checked)
                        command.Parameters.AddWithValue("@h", "1");
                    else
                        command.Parameters.AddWithValue("@h", "0");
                    command.Parameters.AddWithValue("@i", comboBox4.Text);
                    command.Parameters.AddWithValue("@q", img);

                    command.ExecuteScalar();
                    sql.Close();
                    textBox12.Clear();
                    MessageBox.Show("Данные квартиры сохранены!");
                }
                //Запрос на изменение данных в таблице на форме 
                SqlDataAdapter adapter;
                sql.Open();
                SqlCommand cmd = new SqlCommand("SELECT * From [Квартира]", sql);
                adapter = new SqlDataAdapter(cmd);
                уП01_Киреева_СабринаDataSet.Clear();
                adapter.Fill(уП01_Киреева_СабринаDataSet, "Квартира");
                sql.Close();
                dataGridView1.DataSource = уП01_Киреева_СабринаDataSet.Tables["Квартира"];
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Подключение класса и считывание файла "Договор"
            var helper = new Document("Макет договора.docx");
            var items = new Dictionary<string, string>
            {
                //Обработка тегов
                {"<FIO>", comboBox5.SelectedItem.ToString()},
                {"<ADDRESS>",comboBox6.SelectedItem.ToString() },
                {"<PAYMENT> ", textBox13.Text },
                {"<COMPENSATION> ", textBox14.Text },
                {"<STARTDATE>", dateTimePicker1.Value.ToString("dd.MM.yyyy")},
                {"<ENDDATE>", dateTimePicker2.Value.ToString("dd.MM.yyyy") },
                {"<Date>", dateTimePicker3.Value.ToString("dd.MM.yyyy") },
            };
            helper.Process(items, "Договор");
            //Открытие Word с макетом и заполнеными данными с тегов
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Открытие проводчика и загрузка фотографии из памяти ПК на форму
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы изображений (*.jpg, *.png)|*.jpg;*.png;*.jpeg| Все файлы (*.*)|*.*";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                pictureBox3.ImageLocation = openFileDialog.FileName;
            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Обработка двойного нажатия на сторку в таблице и вывод данных на компоненты формы 
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                int k = dataGridView2.SelectedRows[0].Index;
                string id = dataGridView2[0, k].Value.ToString();
                //Запрос на считывание данных из базы данных таблицы Квартира
                string query = "SELECT * FROM Квартира WHERE Код_квартиры=@id";
                try
                {
                    if (sql.State != ConnectionState.Open)
                        sql.Open();
                    using (SqlCommand command = new SqlCommand(query, sql))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            if (reader.HasRows)
                            {
                                string client = reader[1].ToString();
                                for (int i = 0; i < comboBox1.Items.Count; i++)
                                {
                                    if (comboBox1.Items[i].ToString().Substring(0, comboBox1.Items[i].ToString().IndexOf('-')) == client)
                                    {
                                        comboBox1.SelectedItem = comboBox1.Items[i];
                                        break;
                                    }
                                }   
                                    textBox12.Text = reader[2].ToString();
                                    if (reader[3].ToString() == "True")
                                        checkBox1.Checked = true;
                                    else
                                        checkBox1.Checked = false;
                                    numericUpDown1.Value = Convert.ToDecimal(reader[4].ToString());
                                    numericUpDown2.Value = Convert.ToDecimal(reader[5].ToString());
                                for (int i = 0; i < comboBox2.Items.Count; i++)
                                {
                                    if (comboBox2.Items[i].ToString() == reader[6].ToString())
                                    {
                                        comboBox2.SelectedItem = comboBox2.Items[i];
                                        break;
                                    }
                                }
                                for (int i = 0; i < comboBox3.Items.Count; i++)
                                {
                                    if (comboBox3.Items[i].ToString() == reader[7].ToString())
                                    {
                                        comboBox3.SelectedItem = comboBox3.Items[i];
                                        break;
                                    }
                                }
                                    if (reader[8].ToString() == "True")
                                        checkBox2.Checked = true;
                                    else
                                        checkBox2.Checked = false;
                                for (int i = 0; i < comboBox4.Items.Count; i++)
                                {
                                    if (comboBox4.Items[i].ToString() == reader[9].ToString())
                                    {
                                        comboBox4.SelectedItem = comboBox4.Items[i];
                                        break;
                                    }
                                }
                                    byte[] img = (byte[])(reader[10]);
                                    using (MemoryStream ms = new MemoryStream(img)) //преобразовать массив байт в картинку
                                    {
                                        pictureBox3.Image = Image.FromStream(ms);
                                    }

                                }
                            }

                        }
                    }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sql.Close();
                }

            }
        }
    }
}
