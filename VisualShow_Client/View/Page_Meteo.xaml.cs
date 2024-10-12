using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace VisualShow_Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Meteo.xaml
    /// </summary>
    public partial class Page_Meteo : UserControl
    {
        public Page_Meteo()
        {
            InitializeComponent();
        }
        private const string BrokerAddress = "172.31.254.100";
        private const int BrokerPort = 1883;
        private const string Username = "matheo";
        private const string Password = "matheo";
        private string ClientID = Guid.NewGuid().ToString();
        private IMqttClient _client;
        private async void ConnectToBrokerAsync()
        {
            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();
            var optionsBuilder = new MqttClientOptionsBuilder()
                .WithClientId(ClientID)
                .WithTcpServer(BrokerAddress, BrokerPort)
                .WithCredentials(Username, Password)
                .WithCleanSession();
            var options = optionsBuilder.Build();

            try
            {
                await _client.ConnectAsync(options);
                MessageBox.Show("Connecté au broker");

                var topicFilters = new List<MqttTopicFilter>();

                List<string> topics = new List<string>()
                {
                "topic1",
                "topic2",
                "topic3"
                };

                foreach (var topic in topics)
                {
                    var filter = new MqttTopicFilterBuilder().WithTopic(topic).Build();
                    topicFilters.Add(filter);
                }

                var subscriptionOptions = new MqttClientSubscribeOptions()
                {
                    TopicFilters = topicFilters
                };

                await _client.SubscribeAsync(subscriptionOptions);
                MessageBox.Show("connected to topic");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Échec de connexion : {ex.Message}");
            }
            factory.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e => MqttOnNewMessage(e));
            factory.ConnectedHandler = new MqttClientConnectedHandlerDelegate(e => MqttOnConnected(e));
            factory.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(e => MqttOnDisconnected(e));

            await factory.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).WithExactlyOnceQoS().Build());
            await factory.StartAsync(mqttClientOptions);

        }

        private static void MqttOnNewMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            // Do something with each incoming message from the topic

            Console.WriteLine($"MQTT Client: OnNewMessage Topic: {e.ApplicationMessage.Topic} / Message: {e.ApplicationMessage.Payload}");
        }

        private static void MqttOnConnected(MqttClientConnectedEventArgs e) => Console.WriteLine($"MQTT Client: Connected with result: {e.ConnectResult.ResultCode}");

        private static void MqttOnDisconnected(MqttClientDisconnectedEventArgs e) => Console.WriteLine($"MQTT Client: Broker connection lost with reason: {e.Reason}.");

    }
}

