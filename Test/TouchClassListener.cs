using System;
using Leap;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Test
{
	public class TouchClassListener  : Listener
	{
		private Object thisLock = new Object();
		private TouchControlClass touchController;
		private Controller controllerr;
		
		private void SafeWriteLine(String line)
		{
			lock (thisLock)
			{
				Console.WriteLine(line);
			}
		}
		
		public override void OnInit(Controller controller)
		{

			this.controllerr = controller;
			touchController = new TouchControlClass(controller);
			SafeWriteLine("Initialized");
		}
		
		public override void OnConnect(Controller controller)
		{
			SafeWriteLine("Connected");
		}

		public int a;
		
		public override void OnDisconnect(Controller controller)
		{
			SafeWriteLine("Disconnected");
		}
		
		public override void OnExit(Controller controller)
		{
			SafeWriteLine("Exited");
		}
		public override void OnFrame (Controller controller)
		{
			Frame frame = controller.Frame ();
			
			touchController.undateTouches(frame.Pointables);
		}
	}
}

