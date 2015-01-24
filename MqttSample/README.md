The purpose of this sample is connect a Netduino Plus 2 to Azure using the Mqtt protocol.

The GnatMQ broker is hosted in Azure and it is who deliver the messages to the subscribers, the Netduino and a console application.

Projects
--------
* ConsoleManager:
	Send commands to the broker through "sensor/s1" topic. For instance:
	> s1:on
	> s1:off

* NetduinoClient:
	* Turn on / off the on board led and send the led status to the console application when the switch is pressed.
	* To retrieve data is through "sensor/s1" topic and to send the status is through "sensor/info" topic.
	* Parse the data. For "on" command the led will turn on and for "off" command will turn off.
	* NOTE: Well, actually a two characters command will turn on the led, otherwise will turn off :-P

* SensorInfoStatus:
	Only read data from "sensor/info" topic and show it in console.

Build errors
------------
Build error: Description  0x80131700 | File MMP
[https://netmf.codeplex.com/workitem/221](https://netmf.codeplex.com/workitem/221)

Copy MetaDataProcessor.exe.config file in %ProgramFiles%\Microsoft .NET Micro Framework\v4.3\Tools

Content MetaDataProcessor.exe.config file:

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<startup uselegacyv2runtimeactivationpolicy="true">
		<supportedruntime version="v4.0.30319" />
	</startup>
</configuration>
```
