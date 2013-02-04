using System;
using TCD.System.TouchInjection;
using Leap;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
	public class TouchControlClass
	{

		[DllImport("user32.dll")]
		public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);


		private PointerTouchInfo[] touchPoints;

		private Controller controllerr;
		private int noOfTouch;
		private int noOfFinger;
		private int[] xPos;
		private int[] yPos;

		public TouchControlClass (Controller controller)
		{
			noOfTouch = 0;
			noOfFinger = 0;

			xPos = new int[20];
			yPos = new int[20];


			bool s = TouchInjector.InitializeTouchInjection ();
			touchPoints = new PointerTouchInfo[20];
			controllerr = new Controller();
			controllerr = controller;
		}

		public void undateTouches (PointableList fingerlist)
		{
			noOfFinger = 0;
			
			ScreenList screenList = this.controllerr.CalibratedScreens;

			for (int i = 0; i < fingerlist.Count; i++) {

				Leap.Screen screen = screenList.ClosestScreenHit(fingerlist[i]);
				Vector intersection = screen.Intersect(fingerlist[i], true);
				float screenw = screen.WidthPixels;
				float screenh = screen.HeightPixels;
				
				int posx = (int)(intersection.x * screenw)  ;    // actual x position on your screen
				int posy = (int)(screenh - intersection.y * screenh) ;     // actual y position on your screen


				//Console.WriteLine(fingerlist[0].TipPosition.z);
				Console.WriteLine(posx+"   "+posy+"  "+i);
				if(fingerlist[i].TipPosition.z < 0)
				{	
					xPos[noOfFinger] = posx;
					yPos[noOfFinger] = posy;
					//Console.WriteLine(posx+"   "+posy+"  "+i);
					noOfFinger++;
				}
				else
				{
					using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
					{

						//		min		max
						//old	150		0   
						//new	30		2
						//NewValue = (((OldValue - OldMin) * (NewMax - NewMin)) / (OldMax - OldMin)) + NewMin
						
						int radius = ((((int)(fingerlist[i].TipPosition.z) - 150) * (2 - 30)) / (0 - 150)) + 30;

						//g.DrawRectangle(Pens.Black, posx - (radius/2), posy - (radius/2), radius, radius);

						//g.DrawEllipse(Pens.Black, posx - (radius/2), posy - (radius/2), radius, radius);
						//InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);
					}
				}
			}
			//Console.WriteLine(noOfFinger.ToString()+"    "+noOfTouch.ToString());

			if (noOfFinger != noOfTouch) {
				updateData(fingerlist);
			} else {
				for (int i = 0; i < noOfTouch; i++) {
					touchPoints[i].PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;
					touchPoints[i].Move((int)(xPos[i]), (int)(yPos[i]));
					int moveX = xPos[i] - touchPoints[0].PointerInfo.PtPixelLocation.X;
					int moveY = yPos[i] - touchPoints[0].PointerInfo.PtPixelLocation.Y;
					touchPoints[i].Move((int)(moveX), (int)(moveY));
					//Console.WriteLine(xPos[i]+"   "+yPos[i]);
				}
				TouchInjector.InjectTouchInput(noOfTouch, touchPoints);
			}
		}

		private void updateData (PointableList fingerlist)
		{
			for (int i = 0; i < noOfTouch; i++) {
				touchPoints[i].PointerInfo.PointerFlags = PointerFlags.UP;
			}

			TouchInjector.InjectTouchInput(noOfTouch, touchPoints);

			for(int i = 0; i < noOfFinger; i++)
			{
				touchPoints[i] = MakePointerTouchInfo(xPos[i], yPos[i], 10, (uint)(i+1));
			}

			TouchInjector.InjectTouchInput(noOfFinger, touchPoints);

			noOfTouch = noOfFinger;
			//Console.WriteLine(noOfTouch.ToString());
		}

		private PointerTouchInfo MakePointerTouchInfo(int x, int y, int radius, uint id, uint orientation = 90, uint pressure = 32000)
		{
			PointerTouchInfo contact = new PointerTouchInfo();
			contact.PointerInfo.pointerType = PointerInputType.TOUCH;
			contact.TouchFlags = TouchFlags.NONE;
			contact.Orientation = orientation;
			contact.Pressure = pressure;
			contact.PointerInfo.PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;
			contact.TouchMasks = TouchMask.CONTACTAREA | TouchMask.ORIENTATION | TouchMask.PRESSURE;
			contact.PointerInfo.PtPixelLocation.X = x;
			contact.PointerInfo.PtPixelLocation.Y = y;
			contact.PointerInfo.PointerId = id;
			contact.ContactArea.left = x - radius;
			contact.ContactArea.right = x + radius;
			contact.ContactArea.top = y - radius;
			contact.ContactArea.bottom = y + radius;
			return contact;
		}
	}
}

