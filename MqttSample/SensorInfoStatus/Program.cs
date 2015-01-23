using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SensorInfoStatus
{
	class Program
	{
		private static MqttClient _client;
		private static string[] _topics;

		static void Main(string[] args)
		{
			//_client = new MqttClient("192.168.1.102");
			_client = new MqttClient("gnatmqsample.cloudapp.net");

			_topics = new[] {"sensor/info"};
			byte[] qosLevels = new[] {MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE};

			_client.MqttMsgPublishReceived += (sender, eventArgs) => Console.WriteLine(Encoding.UTF8.GetString(eventArgs.Message));

			_client.Connect(Guid.NewGuid().ToString());
			_client.Subscribe(_topics, qosLevels);

			Console.WriteLine("## INFO STATUS ##");
			Console.Read();
		}
	}
}
