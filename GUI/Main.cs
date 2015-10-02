using System;
using System.Windows.Forms;

namespace GUI
{
	public class Program
	{
		public static void Main (string[] args)
		{
			Application.EnableVisualStyles ();
			Application.Run (new MWindow ());
		}
	}
}

