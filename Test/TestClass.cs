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
		private int[] hiren;
		private Controller controllerr;
		private int noOfTouch;
		
		public TouchControlClass (Controller controller)
		{
			noOfTouch = 1;
			
			bool s = TouchInjector.InitializeTouchInjection ();
			touchPoints = new PointerTouchInfo[20];
			controllerr = new Controller();
			controllerr = controller;
			hiren = new int[noOfTouch];
			
			for (int i=0; i<noOfTouch; i++) {
				hiren[i] = 0;
				touchPoints[i] = MakePointerTouchInfo(200, 200, 10, (uint)(i+1));
				//touchPoints[i].PointerInfo.PointerFlags = PointerFlags.UP;
			}
			bool sucess = TouchInjector.InjectTouchInput(noOfTouch, touchPoints);
			for (int i=0; i<2; i++) {
				touchPoints[i].PointerInfo.PointerFlags = PointerFlags.UP;
			}
			sucess = TouchInjector.InjectTouchInput(noOfTouch, touchPoints);
			Console.WriteLine(sucess.ToString()+"   "+"TouchInit");
		}
		
		public void undateTouches (FingerList fingerlist)
		{
			
			ScreenList screenList = this.controllerr.CalibratedScreens;
			
			
			for(int ii = 0; ii < fingerlist.Count; ii++)
			{
				Console.Write(fingerlist[ii].Id+"  ");
			}
			
			Console.WriteLine();
			
			
			
			for (int i = 0; i < fingerlist.Count; i++) {
				Leap.Screen screen = screenList.ClosestScreenHit(fingerlist[i]);
				Vector intersection = screen.Intersect(fingerlist[i], true);
				float screenw = screen.WidthPixels;
				float screenh = screen.HeightPixels;
				
				int posx = (int)(intersection.x * screenw)  ;    // actual x position on your screen
				int posy = (int)(screenh - intersection.y * screenh) ;     // actual y position on your screen
				
				/*if(i == fingerlist[i].Id)
				{
					if(hiren[i] == 0)
					{
						if(fingerlist[i].TipPosition.z < 0)
						{
							touchPoints[i] = MakePointerTouchInfo(posx, posy, 20, (uint)(i+1));
							touchPoints[i].PointerInfo.PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;
							hiren[i] = 1;
						}
						else
						{
							for(int ii = 0; ii < fingerlist.Count; ii++)
							{
								Console.Write(fingerlist[ii].Id+"  ");
							}
							drawCircle(posx, posy);
						}
					}
					else
					{
						if(fingerlist[i].TipPosition.z > 0)
						{
							touchPoints[i].PointerInfo.PointerFlags = PointerFlags.UP;
							hiren[i] = 0;
						}
						else
						{
							touchPoints[i].PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;
							int moveX = posx - touchPoints[0].PointerInfo.PtPixelLocation.X;
							int moveY = posy - touchPoints[0].PointerInfo.PtPixelLocation.Y;
							touchPoints[0].Move((int)(moveX), (int)(moveY));
						}
					}
				}
				else
				{
					touchPoints[i].PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;
					touchPoints[i].Move(0, 0);
				}*/
				bool s = TouchInjector.InjectTouchInput(noOfTouch, touchPoints);
			}
		}
		
		private void testFunc (int touchID, int xx, int yy)
		{
			
			//touchPoints[0].PointerInfo.PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;
			//touchPoints[0].Pressure = (uint)(0);
			
			touchPoints[0].PointerInfo.PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;
			touchPoints[1].PointerInfo.PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;
			touchPoints[2].PointerInfo.PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;
			bool sucess = TouchInjector.InjectTouchInput(1, touchPoints);
			
			
			Console.WriteLine(sucess+"   "+"Update DOWN");
			
			for(int a = 0; a < 5; a++)
			{
				touchPoints[0].PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;
				touchPoints[0].Move (1, 1);
				
				touchPoints[1].PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;
				touchPoints[1].Move (1, 0);
				
				sucess = TouchInjector.InjectTouchInput (1, touchPoints);
				
				Console.WriteLine(sucess+"   "+"Update UPDATE");
			}
			
			touchPoints[0].PointerInfo.PointerFlags = PointerFlags.UP;
			touchPoints[1].PointerInfo.PointerFlags = PointerFlags.UP;
			touchPoints[2].PointerInfo.PointerFlags = PointerFlags.UP;
			sucess = TouchInjector.InjectTouchInput(1, touchPoints);
			
			Console.WriteLine(sucess+"   "+"Update  UP");
		}
		
		private void drawCircle(int xx,int yy)
		{
			using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
			{
				g.DrawEllipse(Pens.Black, xx - 10, yy - 10, 20, 20);
				InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);
			}
		}
		
		/*
		private void addTouch (int touchID, int xx, int yy)
		{
			Console.WriteLine ("Down");

			for (int i = 0; i < 2; i++) {
				if (i = touchID) {
					touchPoints [i] = MakePointerTouchInfo (xx, yy, 10, 1);
					touchPoints[i].PointerInfo.PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;;
				} else {
					touchPoints[i].PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;
					touchPoints[i].Move(0, 0);
				}	
			}

			bool success = TouchInjector.InjectTouchInput(2, touchPoints);

			Console.WriteLine(success+"   "+"Update  DOWN");
		}
		
		private PointerTouchInfo getTouchInfoFromID (int touchID)
		{
			for(int i = 0; i < 20; i++)
			{
				if(touchPoints[i].PointerInfo.PointerId == touchID)
				{
					return touchPoints[i];
				}
			}
			return touchPoints[0];
		}

		private void removeTouch(int touchID)
		{

			touchPoints[0].PointerInfo.PointerFlags = PointerFlags.UP;
			bool success = TouchInjector.InjectTouchInput(1, touchPoints);
			Console.WriteLine(success+"Up");
		}

		private void updateTouch(int touchID, int xx, int yy)
		{
			touchPoints[0].PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;
			int moveX = xx - touchPoints[0].PointerInfo.PtPixelLocation.X;
			int moveY = yy - touchPoints[0].PointerInfo.PtPixelLocation.Y;
			touchPoints[0].Move((int)(moveX), (int)(moveY));
			//touchPoints[touchID].Move (xx - touchPoints[touchID].PointerInfo.PtPixelLocation.X, yy - touchPoints[touchID].PointerInfo.PtPixelLocation.Y);
			bool success = TouchInjector.InjectTouchInput (1, touchPoints);

			Console.WriteLine(success+"  UPDATE  "+moveX+"  "+xx+ "  "+touchPoints[0].PointerInfo.PtPixelLocation.X);
		}
*/
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

