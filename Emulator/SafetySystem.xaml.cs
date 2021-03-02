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
	/// Логика взаимодействия для SafetySystem.xaml
	/// </summary>
	public partial class SafetySystem : UserControl
	{
		public SafetySystem()
		{
			InitializeComponent();
			try
			{
				foreach (string line in File.ReadLines("LastUserParameters.txt"))
				{
					string[] separator = new string[] { ";" };
					string[] strArray = line.Split(separator, StringSplitOptions.None);
					if (strArray.Length >= 4 && strArray[0] == "SafetySystem")
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
				if (strArray.Length >= 4 && strArray2[0] == "SafetySystem")
				{
					strArray[i] = "SafetySystem;" + Server + ";" + Key + ";" + Security + ";" + Thing + ";" + Service + ";";
					tmp = false;
					i++;
				}
				else if(strArray2[0].Length > 0)
				{
					strArray[i] = line;
					i++;
				}
			}
			if (tmp)
			{
				strArray[i] = "SafetySystem;" + Server + ";" + Key + ";" + Security + ";" + Thing + ";" + Service + ";";
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
			TbLogSafetySystem.Clear();
		}

		#endregion

		#region SafetySystem

		public static string d1 = "0", d2 = "0", d3 = "0";

		private void BtnTest1_Click(object sender, RoutedEventArgs e)
		{
			d1 = TbRange1.Text = "320";
			d2 = TbRange2.Text = "320";
			d3 = TbRange3.Text = "320";
		}

		private void BtnTest2_Click(object sender, RoutedEventArgs e)
		{
			d1 = TbRange1.Text = "50";
			d2 = TbRange2.Text = "50";
			d3 = TbRange3.Text = "50";
		}

		private void BtnTest3_Click(object sender, RoutedEventArgs e)
		{
			d1 = TbRange1.Text = "50";
			d2 = TbRange2.Text = "180";
			d3 = TbRange3.Text = "180";
		}

		private void BtnTest4_Click(object sender, RoutedEventArgs e)
		{
			d1 = TbRange1.Text = "200";
			d2 = TbRange2.Text = "120";
			d3 = TbRange3.Text = "70";
		}

		public void TbLogSafetySystem_Add(string newline)
		{
			TbLogSafetySystem.Text += "\r\n\r\n" + newline;
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

		private void TbRange1_TextChanged(object sender, TextChangedEventArgs e)
		{
			d1 = ((TextBox)sender).Text;
		}

		private void TbRange2_TextChanged(object sender, TextChangedEventArgs e)
		{
			d2 = ((TextBox)sender).Text;
		}

		private void TbRange3_TextChanged(object sender, TextChangedEventArgs e)
		{
			d3 = ((TextBox)sender).Text;
		}

		private void CbSecurity_Checked(object sender, RoutedEventArgs e)
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
				TbLogSafetySystem_Add("Не удаётся считать параметры пользователей");
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

						TbThing.Text = strArray2[10];
						TbService.Text = strArray2[11];
					}
				}
				catch (Exception exc)
				{
					TbLogSafetySystem_Add("Ошибка установки параметров пользователя");
				}
			}
			else
			{
				TbLogSafetySystem_Add("Ошибка установки параметров пользователя");
			}
		}

		Worker _SafetySystem;

		public void SafetySystem_WorkCompleted(bool cancelled)
		{
			Action action = () =>
			{
				TbLogSafetySystem_Add("Отправка данных остановлена");
				BtnConnect.Content = "Подключиться";
			};

			this.Dispatcher.Invoke(action);
		}

		private void SafetySystem_ProcessChanged()
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
					_SafetySystem.refresh_time = (int)NudRefreshTime.Value * 1000;
					var httpWebRequest = (HttpWebRequest)WebRequest.Create(security + "://" + Server + "/Thingworx/Things/" + Thing + "/Services/" + Service + "?method=post&appKey=" + Key + "&d1=" + d1 + "&d2=" + d2 + "&d3=" + d3);
					httpWebRequest.ContentType = "application/json";
					httpWebRequest.Accept = "application/json";
					httpWebRequest.Method = "POST";
					var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.ASCII))
					{
						TbLogSafetySystem_Add("Отправленные данные:  " + "{\"d1\":" + d1 + ",\"d2\":" + d2 + ",\"d3\":" + d3 + "}");
					}
				}
				catch (WebException ex)
				{
					TbLogSafetySystem_Add(ex.Message);
					_SafetySystem.Cancel();
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
					TbLogSafetySystem_Add("Ошибка! Сервер не указан");
					error = true;
				}
				if (TbKey.Text == "")
				{
					TbLogSafetySystem_Add("Ошибка! AppKey не указан");
					error = true;
				}
				if (TbThing.Text == "")
				{
					TbLogSafetySystem_Add("Ошибка! Вещь не указана");
					error = true;
				}
				if (TbService.Text == "")
				{
					TbLogSafetySystem_Add("Ошибка! Сервис не указан");
					error = true;
				}
				if (error)
				{
					return;
				}
				_SafetySystem = new Worker();
				_SafetySystem.ProcessChanged += SafetySystem_ProcessChanged;
				_SafetySystem.WorkCompleted += SafetySystem_WorkCompleted;

				BtnConnect.Content = "Отключиться";

				TbLogSafetySystem_Add("Отправка данных начата");

				Thread thread = new Thread(_SafetySystem.Work);
				thread.Start();
			}
			else
			{
				if (_SafetySystem != null)
				{
					_SafetySystem.Cancel();
					BtnConnect.Content = "Подключиться";
				}
			}
		}

		#endregion
	}
}
