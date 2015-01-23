using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ConsoleManager
{
	class Program
	{
		private static MqttClient _client;
		private static string[] _topics;

		static void Main(string[] args)
		{
			//_client = new MqttClient("192.168.1.102");
			_client = new MqttClient("gnatmqsample.cloudapp.net");

			try
			{
				_client.Connect(Guid.NewGuid().ToString());

				//This manager doesn't need subscribe to any topic, only publish.
				//_client.Subscribe(_topics, qosLevels); 

				Console.WriteLine("## MANAGER ##");

				StartManager();
			}
			catch (MqttConnectionException ex)
			{
				ShowExceptionMessage(ex);
			}
			catch (Exception ex)
			{
				ShowExceptionMessage(ex);
			}
			finally
			{
				if (_client.IsConnected)
				{
					_client.Disconnect();	
				}
			}

			Console.WriteLine("Press any key to close...");
			Console.Read();

		}

		private static void ShowExceptionMessage(Exception ex, int deepLevel = 0)
		{
			if (ex.InnerException != null)
				ShowExceptionMessage(ex.InnerException, deepLevel + 1);

			System.Diagnostics.Debug.WriteLine("L{0}: {1}", deepLevel, ex.Message); //L -> level of inner exception (0 is top exception)
		}

		private static void StartManager()
		{
			var command = string.Empty;

			while (!command.Equals("exit"))
			{
				Console.Write("> ");
				command = Console.ReadLine();

				ProcessCommand(command);
			}
		}

		private static void ProcessCommand(string command)
		{
			//COMMAND -> s1:on | s1:off
			var cmd = command.Split(':');

			var topic = string.Format("sensor/{0}", cmd[0]);

			_client.Publish(topic, Encoding.UTF8.GetBytes(cmd[1]), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
		}
	}
}
