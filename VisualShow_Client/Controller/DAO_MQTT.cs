using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VisualShow_Client.Controller
{
    public class DAO_MQTT
    {
        string BrokerAddress = "172.31.254.61";
        int BrokerPort = 1883;
        string Username = "matheo";
        string Password = "matheo";
        string ClientID = Guid.NewGuid().ToString();
        public IMqttClient clientmqtt;

        string humidity = "550";
        string temperature = "59";
        string decibels = "52";
        string air_quality = "10";
        string emergency = "";
        List<string> MqttData = new List<string>();

        async Task ConnexionBroker()
        {
            var mqttnet = new MqttFactory();
            clientmqtt = mqttnet.CreateMqttClient();

            var parametres_connexion_client = new MqttClientOptionsBuilder()
                .WithClientId(ClientID)
                .WithTcpServer(BrokerAddress, BrokerPort)
                .WithCredentials(Username, Password)
                .WithCleanSession();
            var parametres_mqtt = parametres_connexion_client.Build();

            var listeAbonnementsTopics = new List<MqttTopicFilter>();
            List<string> topicsVoullus = new List<string>()
            {
                "KM103/humidity",
                "KM103/temperature",
                "KM103/decibels",
                "KM103/air_quality",
                "KM103/emergency"
            };
            foreach (var topic in topicsVoullus)
            {
                var filtre = new MqttTopicFilterBuilder().WithTopic(topic).Build();
                listeAbonnementsTopics.Add(filtre);
            }

            try
            {
                await clientmqtt.ConnectAsync(parametres_mqtt);
                MessageBox.Show("Connecté au broker", "Connexion", MessageBoxButton.OK);

                var optionsAbonnement = new MqttClientSubscribeOptions()
                {
                    TopicFilters = listeAbonnementsTopics
                };

                await clientmqtt.SubscribeAsync(optionsAbonnement);
                MessageBox.Show("Abonnée aux topics", "Connexion", MessageBoxButton.OK);
            }
            catch (Exception)
            {
                MessageBox.Show($"Echec de la configuration de la connexion");
            }
        }

        public Task GestionMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

            //if (payload == "FireAlert")
            //{
                
            //}
            //else
            //{
            //}


            switch (topic)
            {
                case "KM103/humidity":
                    humidity = payload;
                    MqttData.Add(humidity);
                    break;
                case "KM103/temperature":
                    temperature = payload;
                    MqttData.Add(temperature);
                    break;
                case "KM103/decibels":
                    decibels = payload;
                    MqttData.Add(decibels);
                    break;
                case "KM103/air_quality":
                    air_quality = payload;
                    MqttData.Add(air_quality);
                    break;
                case "KM103/emergency":
                    emergency = payload;
                    MessageBox.Show(emergency, "Message de l'administrateur", MessageBoxButton.OK);
                    break;
            }
            MessageBox.Show($"Humidité : {humidity}\nTempérature : {temperature}\nDécibels : {decibels}\nQualité :{air_quality}, Emergency: {emergency}");
            return Task.CompletedTask;
        }

        public List<string> GetData()
        {
            // ceci sert à tester avec des données donnée tout en haut manuellement.
            MqttData.Add(humidity);
            MqttData.Add(temperature);
            MqttData.Add(decibels);
            MqttData.Add(air_quality);
            MqttData.Add(emergency);

            return MqttData;
        }
        public async Task InitializeAsync()
        {
            //await ConnexionBroker();
            //try
            //{
            //    // fonction qui appelle la fonction GestionMessage a chaque fois qu'un message est reçu
            //    clientmqtt.ApplicationMessageReceivedAsync += GestionMessage;
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Erreur lors de la réception des messages");
            //}
        }
        
    }
}
