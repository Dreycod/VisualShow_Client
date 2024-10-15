using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VisualShow_Admin.Model;

namespace VisualShow_Client.Controller
{
    public class DAO_Offres
    {
        public async Task<List<Events>> GetOffres()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getEvents");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    List<Events> events = JsonConvert.DeserializeObject<List<Events>>(content);
                    return events;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
