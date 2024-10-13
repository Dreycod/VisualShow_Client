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
        string BrokerAddress = "172.31.254.87";
        int BrokerPort = 1883;
        string Username = "matheo";
        string Password = "matheo";
        string ClientID = Guid.NewGuid().ToString();
        IMqttClient clientmqtt;

        string humidity = "550";
        string temperature = "59";
        string decibels = "52";
        string air_quality = "2";
        string emergency = "";
        int test = 0;
        List<string> MqttData = new List<string>();

        async void ConnexionBroker()
        {
            //constructeur de la connexion
            var mqttnet = new MqttFactory();
            //creation du client
            clientmqtt = mqttnet.CreateMqttClient();

            //constructeur des paramètres de connexion
            var parametres_connexion_client = new MqttClientOptionsBuilder()
            .WithClientId(ClientID)
            .WithTcpServer(BrokerAddress, BrokerPort)
            .WithCredentials(Username, Password)
            .WithCleanSession();
            var parametres_mqtt = parametres_connexion_client.Build();

            //liste de filtres mqtt
            var listeAbonnementsTopics = new List<MqttTopicFilter>();
            //liste ou on rentre les topics auxquels on veut s'abonner (attention mettre le chemin entier)
            List<string> topicsVoullus = new List<string>()
            {
            "KM103/humidity",
            "KM103/temperature",
            "KM103/decibels",
            "KM103/air_quality",
            "KM103/emergency"
            };
            //boucle qui crée un filtre pour chaque topic dans la liste topicsVoullus et qui l'ajoute a la liste des filtres
            foreach (var topic in topicsVoullus)
            {
                var filtre = new MqttTopicFilterBuilder().WithTopic(topic).Build();
                listeAbonnementsTopics.Add(filtre);
            }

            //tentative de connexion au broker mqtt avec les paramètres entrés puis d'abonnement aux topics selectionnés
            try
            {
                //tentative de connexion a un borker mqtt avec les paramètres entrés
                await clientmqtt.ConnectAsync(parametres_mqtt);
                MessageBox.Show("Connecté au broker", "Connexion", MessageBoxButton.OK);

                //constructeur des options de filtres (on donne en paramètre la liste des filtres)
                var optionsAbonnement = new MqttClientSubscribeOptions()
                {
                    TopicFilters = listeAbonnementsTopics
                };

                //fonction d'abonnement aux topics (avec en paramètres la liste des filtres créée)
                await clientmqtt.SubscribeAsync(optionsAbonnement);
                //fonction qui appelle la fonction GestionMessage a chaque fois qu'un message est reçu
                MessageBox.Show("Abonnée aux topics", "Connexion", MessageBoxButton.OK);
            }

            catch (Exception)
            {
                MessageBox.Show($"Echec de la configuration de la connexion");
            }
        }

        //fonction qui convertit le payload des messages attachés aux topics en chaine
        public Task GestionMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

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
                    MqttData.Add(emergency);
                    break;
            }
            MessageBox.Show($"Humidité : {humidity}\nTempérature : {temperature}\nDécibels : {decibels}\nQualité :{air_quality}, Emergency: {emergency}");
            return Task.CompletedTask;
        }
        
        public List<string> GetData()
        {
            // ceci est un test qui marche, faut etre connecté pour réellement avoir des vrais valeurs de mqtt
            //MqttData.Add(humidity);
            //MqttData.Add(temperature);
            //MqttData.Add(decibels);
            //MqttData.Add(air_quality);
            //MqttData.Add(emergency);

            return MqttData;
        }
        public DAO_MQTT()
        {
            //ConnexionBroker();
            //try
            //{
            //    //fonction qui appelle la fonction GestionMessage a chaque fois qu'un message est reçu
            //    clientmqtt.ApplicationMessageReceivedAsync += GestionMessage;
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Erreur lors de la réception des messages");
            //}
        }
    }
}
