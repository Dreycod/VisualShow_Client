using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VisualShow_Client.View;

namespace VisualShow_Client.Controller
{
    public class DAO_MQTT
    {
        string BrokerAddress = "raspberrypimatheo1";
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


        async Task ConnexionBroker(string ecran_name)
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
                ecran_name + "/humidity",
                ecran_name + "/temperature",
                ecran_name + "/decibels",
                ecran_name + "/air_quality",
                ecran_name + "/emergency"
            };
            foreach (var topic in topicsVoullus)
            {
                var filtre = new MqttTopicFilterBuilder().WithTopic(topic).Build();
                listeAbonnementsTopics.Add(filtre);
            }

            try
            {
                await clientmqtt.ConnectAsync(parametres_mqtt);
                Console.WriteLine("Connecté au broker", "Connexion");

                var optionsAbonnement = new MqttClientSubscribeOptions()
                {
                    TopicFilters = listeAbonnementsTopics
                };

                await clientmqtt.SubscribeAsync(optionsAbonnement);
                Console.WriteLine("Abonnée aux topics", "Connexion");
            }
            catch (Exception)
            {
                Console.WriteLine($"Echec de la configuration de la connexion");
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

                    break;
                case "KM103/temperature":
                    temperature = payload;
                    break;
                case "KM103/decibels":
                    decibels = payload;
                    break;
                case "KM103/air_quality":
                    air_quality = payload;
                    break;
                case "KM103/emergency":
                    emergency = payload;
                    MessageBox.Show(emergency, "Message de l'administrateur", MessageBoxButton.OK);

                    GestionUrgence(payload);

                    break;
            }
            return Task.CompletedTask;
        }

        public List<string> GetData()
        {
            MqttData.Clear();

            MqttData.Add(humidity);
            MqttData.Add(temperature);
            MqttData.Add(decibels);
            MqttData.Add(air_quality);
            MqttData.Add(emergency);

            return MqttData;
        }

        //gère les urgences et affiche la fenetre d'urgence correspondante
        public void GestionUrgence(string payload)
        {
            var valeursUrgentes = new List<string>
            {
                "FireAlert",
                "IntruderAlert",
                "GeneralEmergency"
            };
            string typeAlerte = payload;

            if (valeursUrgentes.Contains(payload))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var affichageUrgence = new Page_Urgence(typeAlerte);
                });

        public async Task InitializeAsync(string ecran_name)
        {
            await ConnexionBroker(ecran_name);
            try
            {
                // fonction qui appelle la fonction GestionMessage a chaque fois qu'un message est reçu
                clientmqtt.ApplicationMessageReceivedAsync += GestionMessage;
            }
            catch (Exception)
            {
                Console.WriteLine("Erreur lors de la réception des messages");

            }
        }
        
    }
}
