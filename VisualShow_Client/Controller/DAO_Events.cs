using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VisualShow_Admin.Model;

namespace VisualShow_Admin.Controller
{
    public class DAO_Events
    {
        public async Task<List<Events>> GetEvents()
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
        public async Task<Events> GetEventById(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getEventById/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Events events = JsonConvert.DeserializeObject<Events>(content);
                    return events;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Events> GetEventByName(string name)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getEventByName/" + name);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Events events = JsonConvert.DeserializeObject<Events>(content);
                    return events;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Events> AddEvent(string name, DateTime date)
        {
            try
            {
                HttpClient client = new HttpClient();
                string formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");
                string json = JsonConvert.SerializeObject(new { name = name, date = formattedDate });
                MessageBox.Show(json);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                MessageBox.Show(content.ToString());
                HttpResponseMessage response = await client.PostAsync("https://drey.alwaysdata.net/AddEvent", content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Events events = JsonConvert.DeserializeObject<Events>(result);
                    return events;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Events> UpdateEvent(string id, string name, DateTime date)
        {
            try
            {
                HttpClient client = new HttpClient();
                string formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");
                string json = JsonConvert.SerializeObject(new { name = name, date = formattedDate });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://drey.alwaysdata.net/UpdateEvent/" + id, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Events events = JsonConvert.DeserializeObject<Events>(result);
                    return events;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<Events> DeleteEvent(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync("https://drey.alwaysdata.net/DeleteEvent/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Events events = JsonConvert.DeserializeObject<Events>(content);
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
