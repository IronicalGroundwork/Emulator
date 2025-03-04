﻿using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Emulator
{
	public class RootObjectRemoteTerminal
	{
		public float L1 { get; set; }
		public float L2 { get; set; }
		public float L3 { get; set; }
		public float L4 { get; set; }
	}

	/// <summary>
	/// Логика взаимодействия для RemoteTerminal.xaml
	/// </summary>
	public partial class RemoteTerminal : UserControl
	{
		public RemoteTerminal()
		{
			InitializeComponent();
			try
			{
				foreach (string line in File.ReadLines("LastUserParameters.txt"))
				{
					string[] separator = new string[] { ";" };
					string[] strArray = line.Split(separator, StringSplitOptions.None);
					if (strArray.Length >= 4 && strArray[0] == "RemoteTerminal")
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
				if (strArray.Length >= 4 && strArray2[0] == "RemoteTerminal")
				{
					strArray[i] = "RemoteTerminal;" + Server + ";" + Key + ";" + Security + ";" + Thing + ";" + Service + ";";
					tmp = false;
					i++;
				}
				else if(strArray2[0].Length>0)
				{
					strArray[i] = line;
					i++;
				}				
			}
			if (tmp)
			{
				 strArray[i] = "RemoteTerminal;" + Server + ";" + Key + ";" + Security + ";" + Thing + ";" + Service + ";";
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
			TbLogRemoteTerminal.Clear();
		}

		#endregion

		#region RemoteTerminal

		public static int b1 = 0, b2 = 0, b3 = 0;

		private void CountingButton1_OnClick(object sender, RoutedEventArgs e)
		{
			if (CountingBadge1.Badge == null || Equals(CountingBadge1.Badge, ""))
				CountingBadge1.Badge = 0;

			var next = int.Parse(CountingBadge1.Badge.ToString()) + 1;

			b1 = next < 1001 ? next : 0;

			CountingBadge1.Badge = next < 1001 ? (object)next : null;
		}

		private void CountingButton2_OnClick(object sender, RoutedEventArgs e)
		{
			if (CountingBadge2.Badge == null || Equals(CountingBadge2.Badge, ""))
				CountingBadge2.Badge = 0;

			var next = int.Parse(CountingBadge2.Badge.ToString()) + 1;

			b2 = next < 1001 ? next : 0;

			CountingBadge2.Badge = next < 1001 ? (object)next : null;
		}

		private void CountingButton3_OnClick(object sender, RoutedEventArgs e)
		{
			if (CountingBadge3.Badge == null || Equals(CountingBadge3.Badge, ""))
				CountingBadge3.Badge = 0;

			var next = int.Parse(CountingBadge3.Badge.ToString()) + 1;

			b3 = next < 1001 ? next : 0;

			CountingBadge3.Badge = next < 1001 ? (object)next : null;
		}

		private void ResetButtons_OnClick(object sender, RoutedEventArgs e)
		{
			CountingBadge1.Badge = CountingBadge2.Badge = CountingBadge3.Badge = null;
			b1 = b2 = b3 = 0;
		}

		public void TbLogRemoteTerminal_Add(string newline)
		{
			TbLogRemoteTerminal.Text += "\r\n\r\n" + newline;
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
				TbLogRemoteTerminal_Add("Не удаётся считать параметры пользователей");
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

						TbThing.Text = strArray2[6];
						TbService.Text = strArray2[7];
					}
				}
				catch (Exception exc)
				{
					TbLogRemoteTerminal_Add("Ошибка установки параметров пользователя");
				}
			}
			else
			{
				TbLogRemoteTerminal_Add("Ошибка установки параметров пользователя");
			}
		}

		Worker _RemoteTerminal;

		public void RemoteTerminal_WorkCompleted(bool cancelled)
		{
			Action action = () =>
			{
				TbLogRemoteTerminal_Add("Отправка данных остановлена");
				BtnConnect.Content = "Подключиться";
			};

			this.Dispatcher.Invoke(action);
		}

		private void RemoteTerminal_ProcessChanged()
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
					_RemoteTerminal.refresh_time = (int)NudRefreshTime.Value * 1000;
					var httpWebRequest = (HttpWebRequest)WebRequest.Create(security + "://" + Server + "/Thingworx/Things/" + Thing + "/Services/" + Service + "?method=post&appKey=" + Key + "&p=" + DeadMenSwitch.Value + "&b1=" + b1 + "&b2=" + b2 + "&b3=" + b3);
					httpWebRequest.ContentType = "application/json";
					httpWebRequest.Accept = "application/json";
					httpWebRequest.Method = "POST";
					var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.ASCII))
					{
						var result = streamReader.ReadToEnd();
						RootObjectRemoteTerminal answer = JsonConvert.DeserializeObject<RootObjectRemoteTerminal>(result);
						TbLogRemoteTerminal_Add("Полученные данные:  " + result + "\r\nОтправленные данные:  " + "{\"p\":" + DeadMenSwitch.Value + ",\"b1\":" + b1 + ",\"b2\":" + b2 + ",\"b3\":" + b3 + "}");
						var converter = new System.Windows.Media.BrushConverter();
						LED1.Foreground = LED1text.Foreground = LED2.Foreground = LED2text.Foreground = LED3.Foreground = LED3text.Foreground = LED4.Foreground = LED4text.Foreground = (Brush)converter.ConvertFromString("#FF455A64");
						LED1.Kind = LED2.Kind = LED3.Kind = LED4.Kind = PackIconKind.LightbulbOnOutline;
						if (answer.L1 == 1)
						{
							LED1.Foreground = LED1text.Foreground = Brushes.Blue;
							LED1.Kind = PackIconKind.LightbulbOn;
						}
						if (answer.L2 == 1)
						{
							LED2.Foreground = LED2text.Foreground = Brushes.Red;
							LED2.Kind = PackIconKind.LightbulbOn;
						}
						if (answer.L3 == 1)
						{
							LED3.Foreground = LED3text.Foreground = Brushes.Yellow;
							LED3.Kind = PackIconKind.LightbulbOn;
						}
						if (answer.L4 == 1)
						{
							LED4.Foreground = LED4text.Foreground = Brushes.ForestGreen;
							LED4.Kind = PackIconKind.LightbulbOn;
						}
					}
				}
				catch (WebException ex)
				{
					TbLogRemoteTerminal_Add(ex.Message);
					_RemoteTerminal.Cancel();
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
					TbLogRemoteTerminal_Add("Ошибка! Сервер не указан");
					error = true;
				}
				if (TbKey.Text == "")
				{
					TbLogRemoteTerminal_Add("Ошибка! AppKey не указан");
					error = true;
				}
				if (TbThing.Text == "")
				{
					TbLogRemoteTerminal_Add("Ошибка! Вещь не указана");
					error = true;
				}
				if (TbService.Text == "")
				{
					TbLogRemoteTerminal_Add("Ошибка! Сервис не указан");
					error = true;
				}
				if (error)
				{
					return;
				}
				_RemoteTerminal = new Worker();
				_RemoteTerminal.ProcessChanged += RemoteTerminal_ProcessChanged;
				_RemoteTerminal.WorkCompleted += RemoteTerminal_WorkCompleted;

				BtnConnect.Content = "Отключиться";

				TbLogRemoteTerminal_Add("Отправка данных начата");

				Thread thread = new Thread(_RemoteTerminal.Work);
				thread.Start();
			}
			else
			{
				if (_RemoteTerminal != null)
				{
					_RemoteTerminal.Cancel();
					BtnConnect.Content = "Подключиться";
				}
			}
		}
		#endregion
	}
}
