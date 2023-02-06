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

/* Имя файла: "Охранное агенство" 
   Выполнил: Киреева Сабрина
   Версия языка: Visual Studio 2022 LTSC 17.0 */
namespace Охранное_агенство
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            //Выход с формы авторизации и закрытие программного продукта 
            DialogResult result = MessageBox.Show("Вы точно хотети выйти?", "Выход", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
                Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Подключение базы данных SQl к Visual Studio
            SqlConnection sql = new SqlConnection("Data Source=SQL;Initial Catalog=УП01_Киреева_Сабрина;Integrated Security=True");
            sql.Open();
            //Объявление переменных и присваивание их к компонентам формы
            string login = textBox1.Text;
            string password = textBox2.Text;
            string role="";
            bool input = false;
            //Отправка запроса в базу данных на считывание данных из таблицы "Авторизация" в базе данных
            SqlCommand command = new SqlCommand("SELECT * FROM [Авторизация]", sql);
            SqlDataReader reader = command.ExecuteReader();
            //Проверка логина и пароля по параметру "Роль" из таблицы "Авторизация"
            while (reader.Read())
            {
                if (reader["Логин"].ToString() == login && reader["Пароль"].ToString() == password)
                {
                    input = true;
                    role = reader["Роль"].ToString();
                    if (role == "Администратор")
                    {
                        Администратор a = new Администратор();
                        a.Show();
                        this.Hide();
                    }
                    else if (role == "Директор")
                    {
                        Директор a = new Директор();
                        a.Show();
                        this.Hide();
                    }
                    else if (role == "Диспетчер")
                    {
                        Диспетчер a = new Диспетчер();
                        a.Show();
                        this.Hide();
                    }
                    else if (role == "Менеджер")
                    {
                        Менеджер a = new Менеджер();
                        a.Show();
                        this.Hide();
                    }
                    sql.Close();
                    input = true;
                    break;
                }
            }
            if (!input)
            {
                MessageBox.Show("Вы ввели неверный логин или пароль!");
            }
        }
    }
}
