using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace UltrasonicSensorHC_SR04
{
	public class Program
	{
		private static InterruptPort _echoPort;
		private static OutputPort _triggerPort;
		private static long _endTick;
		private static long _beginTick;
		private static long _minTicks;

		public static void Main()
		{
			_echoPort = new InterruptPort(Pins.GPIO_PIN_D4, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeLow);
			_triggerPort = new OutputPort(Pins.GPIO_PIN_D5, false);

			_echoPort.OnInterrupt += EchoPortOnOnInterrupt;
			
			while (true)
				Run();
		}

		private static void Run()
		{
			_endTick = 0L; // Reset the end time.
			_beginTick = DateTime.Now.Ticks; // Time when the signal is thrown.

			_triggerPort.Write(true); // Start to emit signal
			
			Thread.Sleep(2); // Time emiting the signal
			
			_triggerPort.Write(false); // Stop to emit signal

			Thread.Sleep(50); // Sleep to give time to the EchoPortOnOnInterrupt event to throw it.

			if (_endTick > 0L)
			{
				var elapsed = _endTick - _beginTick; // Calculate the round trip time from trigger to echo port after bouncing

				Debug.Print(CalculateDistance(elapsed).ToString());
			}
			else
			{
				Debug.Print("-1");
			}

			Thread.Sleep(250);
		}

		// Echo received the signal after bouncing
		private static void EchoPortOnOnInterrupt(uint data1, uint data2, DateTime time)
		{
			_endTick = time.Ticks;
		}

		/// <summary>
		/// Calculate distance from delay time.
		/// </summary>
		/// <param name="time">Time in Ticks since the signal is thrown from the Trigger to received into Echo</param>
		/// <returns></returns>
		private static float CalculateDistance(long time)
		{
			// 343 m/s -> 34.3 cm/ms --> http://hyperphysics.phy-astr.gsu.edu/hbase/sound/souspe.html
			// 2 -> time round trip

			return (float)(34.3*TimeSpan.FromTicks(time).Milliseconds)/2;
		}
	}
}
