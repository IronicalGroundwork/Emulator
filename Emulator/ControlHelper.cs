﻿using System;
using System.Windows.Forms;

namespace Emulator
{
	public static class ControlHelper
	{
		public static void InvokeEx(this Control control, Action action)
		{
			if (control.InvokeRequired)
				control.Invoke(action);
			else
				action();
		}
	}
}