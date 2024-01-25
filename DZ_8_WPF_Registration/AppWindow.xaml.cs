using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _15._01
{
    /// <summary>
    /// Логика взаимодействия для AppWindow.xaml
    /// </summary>
    public partial class AppWindow : Window
    {
        string userLogin = string.Empty;
        public AppWindow(string userLogin)
        {
            InitializeComponent();
            this.userLogin = userLogin;
            userLabel.Content = $"Добро пожаловать, {userLogin}!";
            initNotes();
        }

        void initNotes()
        {
            string path = $"{userLogin}.txt";
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        TextBox box = new TextBox();
                        box.Text = s;
                        userNotes.Children.Add(box);
                    }
                }
            } else
            {
                File.Create(path);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            userNotes.Children.Add(new TextBox());
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            using (StreamWriter sw = File.CreateText($"{userLogin}.txt"))
            {
                foreach(var box in userNotes.Children)
                {
                    TextBox textBox = (TextBox)box;
                    if(textBox.Text != string.Empty)
                    {
                        sw.WriteLine(textBox.Text);
                    }
                }
            }
        }
    }
}
