using System;
using TCD.System.TouchInjection;
using Leap;



namespace Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// Create a sample listener and controller
			TouchClassListener listener = new TouchClassListener();
			Controller controller = new Controller();
			
			// Have the sample listener receive events from the controller
			controller.AddListener(listener);


			
			// Keep this process running until Enter is pressed
			Console.WriteLine("Press Enter to quit...");
			Console.ReadLine();
			
			// Remove the sample listener when done
			controller.RemoveListener(listener);
			controller.Dispose();


		}




		public static PointerTouchInfo MakePointerTouchInfo(int x, int y, int radius, uint id, uint orientation = 90, uint pressure = 32000)
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
