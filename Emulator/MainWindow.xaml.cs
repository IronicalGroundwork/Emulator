namespace Emulator
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SmartCamera.Closing();
			SafetySystem.Closing();
			RemoteTerminal.Closing();
			RobotSimulator.Closing();
		}
	}
}