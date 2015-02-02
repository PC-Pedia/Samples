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

			_minTicks = 6200L;

			while (true)
			{
				_triggerPort.Write(true);
				Thread.Sleep(1);

				_endTick = 0L;
				_beginTick = DateTime.Now.Ticks;

				_triggerPort.Write(false);
				Thread.Sleep(50);

				if (_endTick > 0L)
				{
					var elapsed = _endTick - _beginTick;

					elapsed -= _minTicks;

					Debug.Print(elapsed.ToString());
				}
				else
				{
					Debug.Print("-1");
				}
			}
		}

		private static void EchoPortOnOnInterrupt(uint data1, uint data2, DateTime time)
		{
			_endTick = time.Ticks;
		}
	}
}
