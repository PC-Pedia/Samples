using System;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace NetduinoClient
{
	public class Program
	{
		private static MqttClient _client;

		private static OutputPort _ledPort;

		public static void Main()
		{
			try
			{
				//Mqtt configuration

				//_client = new MqttClient("192.168.1.102");
				_client = new MqttClient("gnatmqsample.cloudapp.net");

				_client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

				_client.Connect(Guid.NewGuid().ToString());
				_client.Subscribe(new[] { "sensor/s1" }, new[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });

				//Board configuration
				_ledPort = new OutputPort(Pins.ONBOARD_LED, false);
				InterruptPort switchPort = new InterruptPort(Pins.ONBOARD_SW1,
					false,
					Port.ResistorMode.Disabled,
					Port.InterruptMode.InterruptEdgeBoth);

				switchPort.OnInterrupt += SwitchPortOnOnInterrupt;

				Thread.Sleep(Timeout.Infinite);
			}
			catch (Exception ex)
			{
				Debug.Print(ex.Message);
			}
		}

		private static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
		{
			if (e.Topic.Equals("sensor/s1"))
			{
				SetStatus(e.Message);
			}
		}

		private static void SetStatus(byte[] message)
		{
			var command = Encoding.UTF8.GetChars(message);

			if (command.Length == 2) //ON
				_ledPort.Write(true);
			else //OFF :-P
				_ledPort.Write(false);
		}

		private static void SwitchPortOnOnInterrupt(uint data1, uint data2, DateTime time)
		{
			var message = "NetDuino's led is " + _ledPort.Read();

			if (data2 == 1)
				_client.Publish("sensor/info", Encoding.UTF8.GetBytes(message));
		}
	}
}
