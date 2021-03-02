using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Emulator
{
	/// <summary>
	/// Логика взаимодействия для SmartCamera.xaml
	/// </summary>
	public partial class SmartCamera : UserControl
	{
		public SmartCamera()
		{
			InitializeComponent();
			try
			{
				foreach (string line in File.ReadLines("LastUserParameters.txt"))
				{
					string[] separator = new string[] { ";" };
					string[] strArray = line.Split(separator, StringSplitOptions.None);
					if (strArray.Length >= 4 && strArray[0] == "SmartCamera")
					{
						TbServer.Text = strArray[1];
						TbKey.Text = strArray[2];

						Security = strArray[3];
						CbSecurity.IsChecked = bool.Parse(strArray[3]);

						TbThing.Text = strArray[4];
						TbService.Text = strArray[5];
						return;
					}
				}
			}
			catch (Exception exc)
			{

			}
		}

		public static void Closing()
		{
			string[] strArray = new string[4];
			int i = 0;
			bool tmp = true;
			foreach (string line in File.ReadLines("LastUserParameters.txt"))
			{
				string[] separator = new string[] { ";" };
				string[] strArray2 = line.Split(separator, StringSplitOptions.None);
				if (strArray.Length >= 4 && strArray2[0] == "SmartCamera")
				{
					strArray[i] = "SmartCamera;" + Server + ";" + Key + ";" + Security + ";" + Thing + ";" + Service + ";";
					tmp = false;
					i++;
				}
				else if (strArray2[0].Length > 0)
				{
					strArray[i] = line;
					i++;
				}
			}
			if (tmp)
			{
				strArray[i] = "SmartCamera;" + Server + ";" + Key + ";" + Security + ";" + Thing + ";" + Service + ";";
			}
			File.WriteAllLines("LastUserParameters.txt", strArray);
		}

		#region General

		private void Tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			int val;
			if (!Int32.TryParse(e.Text, out val))
			{
				e.Handled = true;
			}
		}

		private void Tb_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Space)
			{
				e.Handled = true;
			}
		}

		private void TbLog_TextChanged(object sender, TextChangedEventArgs e)
		{
			((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
			((TextBox)sender).ScrollToEnd();
			MenuItemClear.IsEnabled = ((TextBox)sender).Text.Length > 0;
		}

		private void MenuItemClear_Click(object sender, RoutedEventArgs e)
		{
			TbLogSmartCamera.Clear();
		}

		#endregion

		#region SmartCamera

		public static string code = "0";

		private void BtnCode0_Click(object sender, RoutedEventArgs e)
		{
			TbProductCode.Text = "0";
		}

		private void BtnCode1_Click(object sender, RoutedEventArgs e)
		{
			TbProductCode.Text = TbCode1.Text;
		}

		private void BtnCode2_Click(object sender, RoutedEventArgs e)
		{
			TbProductCode.Text = TbCode2.Text;
		}

		private void BtnCode3_Click(object sender, RoutedEventArgs e)
		{
			TbProductCode.Text = TbCode3.Text;
		}

		private void BtnCode4_Click(object sender, RoutedEventArgs e)
		{
			TbProductCode.Text = TbCode4.Text;
		}

		private void BtnClear_Click(object sender, RoutedEventArgs e)
		{
			TbProductCode.Text = "";
		}

		public void TbLogSmartCamera_Add(string newline)
		{
			TbLogSmartCamera.Text += "\r\n\r\n" + newline;
		}

		private void TbProductCode_TextChanged(object sender, TextChangedEventArgs e)
		{
			code = TbProductCode.Text;
		}

		#endregion

		#region Connecction 

		static string Server = "", Key = "", Security = "", Thing = "", Service = "";

		private void TbServer_TextChanged(object sender, TextChangedEventArgs e)
		{
			Server = ((TextBox)sender).Text;
		}

		private void TbKey_TextChanged(object sender, TextChangedEventArgs e)
		{
			Key = ((TextBox)sender).Text;
		}

		private void TbThing_TextChanged(object sender, TextChangedEventArgs e)
		{
			Thing = ((TextBox)sender).Text;
		}

		private void TbService_TextChanged(object sender, TextChangedEventArgs e)
		{
			Service = ((TextBox)sender).Text;
		}

		private void CbSecurity_Click(object sender, RoutedEventArgs e)
		{
			Security = CbSecurity.IsChecked.ToString();
		}

		private void BtnLoadSettings_Click(object sender, RoutedEventArgs e)
		{
			string path = "UserParameters.txt";
			LbUserParameters.Items.Clear();
			try
			{
				string[] strArray = File.ReadAllLines(path);
				foreach (string str in strArray)
				{
					if (str.Trim() != "")
					{
						LbUserParameters.Items.Add(str.Trim());
					}
				}
				LbUserParameters.SelectedIndex = 0;
			}
			catch (Exception exc)
			{
				TbLogSmartCamera_Add("Не удаётся считать параметры пользователей");
			}
		}

		private void BtnSetSettings_Click(object sender, RoutedEventArgs e)
		{
			int selectedIndex = this.LbUserParameters.SelectedIndex;
			if (selectedIndex >= 0)
			{
				string str = this.LbUserParameters.Items[selectedIndex].ToString();
				string[] separator = new string[] { ";" };
				try
				{
					string[] strArray2 = str.Split(separator, StringSplitOptions.None);
					if (strArray2.Length >= 6)
					{
						TbServer.Text = strArray2[1];
						TbKey.Text = strArray2[2];

						CbSecurity.IsChecked = bool.Parse(strArray2[3]);

						TbThing.Text = strArray2[8];
						TbService.Text = strArray2[9];
					}
				}
				catch (Exception exc)
				{
					TbLogSmartCamera_Add("Ошибка установки параметров пользователя");
				}
			}
			else
			{
				TbLogSmartCamera_Add("Ошибка установки параметров пользователя");
			}
		}

		private void CbSecurity_Checked(object sender, RoutedEventArgs e)
		{
			Security = CbSecurity.IsChecked.ToString();
		}

		Worker _SmartCamera;

		public void SmartCamera_WorkCompleted(bool cancelled)
		{
			Action action = () =>
			{
				TbLogSmartCamera_Add("Отправка данных остановлена");
				BtnConnect.Content = "Подключиться";
			};

			this.Dispatcher.Invoke(action);
		}

		private void SmartCamera_ProcessChanged()
		{
			Action action = () =>
			{
				try
				{
					string security = "http";
					if (CbSecurity.IsChecked == true)
					{
						security += "s";
					}
					_SmartCamera.refresh_time = (int)NudRefreshTime.Value * 1000;
					var httpWebRequest = (HttpWebRequest)WebRequest.Create(security + "://" + Server + "/Thingworx/Things/" + Thing + "/Services/" + Service + "?method=post&appKey=" + Key + "&code=" + code);
					httpWebRequest.ContentType = "application/json";
					httpWebRequest.Method = "POST";
					var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.ASCII))
					{
						TbLogSmartCamera_Add("Отправленные данные:  {\"code\":" + code + "}");
					}
				}
				catch (WebException ex)
				{
					TbLogSmartCamera_Add(ex.Message);
					_SmartCamera.Cancel();
				}
			};

			this.Dispatcher.Invoke(action);
		}

		private void BtnConnect_Click(object sender, RoutedEventArgs e)
		{
			if (BtnConnect.Content.ToString() == "Подключиться")
			{
				bool error = false;
				if (TbServer.Text == "")
				{
					TbLogSmartCamera_Add("Ошибка! Сервер не указан");
					error = true;
				}
				if (TbKey.Text == "")
				{
					TbLogSmartCamera_Add("Ошибка! AppKey не указан");
					error = true;
				}
				if (TbThing.Text == "")
				{
					TbLogSmartCamera_Add("Ошибка! Вещь не указана");
					error = true;
				}
				if (TbService.Text == "")
				{
					TbLogSmartCamera_Add("Ошибка! Сервис не указан");
					error = true;
				}
				if (error)
				{
					return;
				}
				_SmartCamera = new Worker();
				_SmartCamera.ProcessChanged += SmartCamera_ProcessChanged;
				_SmartCamera.WorkCompleted += SmartCamera_WorkCompleted;

				BtnConnect.Content = "Отключиться";

				TbLogSmartCamera_Add("Отправка данных начата");

				Thread thread = new Thread(_SmartCamera.Work);
				thread.Start();
			}
			else
			{
				if (_SmartCamera != null)
				{
					_SmartCamera.Cancel();
					BtnConnect.Content = "Подключиться";
				}
			}
		}

		#endregion
	}
}
