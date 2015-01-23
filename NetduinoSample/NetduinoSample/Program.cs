using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NetduinoSample
{
	public class Program
	{
		static OutputPort _outLed;
		static OutputPort _outDig1;
		static bool _isOn;
		
		public static void Main()
		{
			_outLed = new OutputPort(Pins.ONBOARD_LED, false);
			_outDig1 = new OutputPort(Pins.GPIO_PIN_D0, false);
			_isOn = false;


			InterruptPort mySwicht = new InterruptPort(Pins.ONBOARD_SW1, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
			mySwicht.OnInterrupt += new NativeEventHandler(mySwicht_OnInterrupt);

			while (true)
			{
				if (_isOn)
				{
					_outLed.Write(true);
					_outDig1.Write(false);
					Thread.Sleep(500);
					_outLed.Write(false);
					_outDig1.Write(true);
					Thread.Sleep(500);
				}
				else
				{
					_outLed.Write(false);
					_outDig1.Write(false);
				}
			}
		}

		static void mySwicht_OnInterrupt(uint data1, uint data2, DateTime time)
		{
			if (data2 == 1)
				_isOn = !_isOn;
		}



	}
}
