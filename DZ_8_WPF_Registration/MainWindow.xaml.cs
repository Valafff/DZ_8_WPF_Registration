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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _15._01
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		RegistrationForm registration;
		AurizationForm aurization;
		public MainWindow()
		{
			InitializeComponent();
			registration = new RegistrationForm(
				registrationLogin, registrationPassword
				);

			aurization = new AurizationForm(
				autorizationLogin, autorizationPassword
				);
		}

		private void executeRegistration(object sender, RoutedEventArgs e)
		{
			if (registration.execute("users.txt"))
			{
				MessageBox.Show("Удачная регистрация");
			}
			else
			{
				MessageBox.Show("Регистрация не удалась");
			}
		}

		private void executeAutorization(object sender, RoutedEventArgs e)
		{
			if (aurization.execute("users.txt"))
			{
				new AppWindow(autorizationLogin.Text).Show();
				Close();
			}
			else
			{
				MessageBox.Show("Неверно введенные данные");
			}
		}
	}

	abstract class UserForm
	{
		protected TextBox login;
		protected PasswordBox password;

		public UserForm(TextBox login, PasswordBox password)
		{
			this.login = login;
			this.password = password;
		}

		abstract public bool execute(string path);
	}

	class RegistrationForm : UserForm
	{
		public RegistrationForm(TextBox login, PasswordBox password) :
			base(login, password)
		{ }

		public override bool execute(string path)
		{
			string textLogin = login.Text;

			if (File.Exists(path))
			{
				using (StreamReader sr = File.OpenText(path))
				{
					string s;
					while ((s = sr.ReadLine()) != null)
					{
						string log = s.Split(' ')[0];
						if (log == textLogin)
							return false;

					}
				}
			}

			int hashPassword = password.Password.GetHashCode();
			using (StreamWriter sw = File.AppendText(path))
			{
				sw.WriteLine($"{textLogin} {hashPassword}");
			}
			return true;
		}
	}

	class AurizationForm : UserForm
	{
		public AurizationForm(TextBox login, PasswordBox password) :
		   base(login, password)
		{ }

		public override bool execute(string path)
		{
			string textLogin = login.Text;
			string textHashPassword = password.Password.GetHashCode().ToString();
			if (File.Exists(path))
			{
				using (StreamReader sr = File.OpenText(path))
				{
					string s;
					while ((s = sr.ReadLine()) != null)
					{
						string[] data = s.Split(' ');
						if (data[0] == textLogin && data[1] == textHashPassword)
							return true;

					}

					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}
}
