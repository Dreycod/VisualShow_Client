using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using VisualShow_Admin.Model;

namespace VisualShow_Admin.Controller
{
    public class DAO_TempHum
    {
        public async Task<List<Temp_Hum>> GetTemp_Hum(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getTemp_Hum/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    List<Temp_Hum> temp_hum = JsonConvert.DeserializeObject<List<Temp_Hum>>(content);
                    return temp_hum;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Temp_Hum> AddTemp_Hum(string id, string temperature, string humidite, DateTime date)
        {
            try
            {
                HttpClient client = new HttpClient();
                string formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");
                string json = JsonConvert.SerializeObject(new { temperature = temperature, humidite = humidite, date = formattedDate }); MessageBox.Show(json);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                MessageBox.Show(content.ToString());
                HttpResponseMessage response = await client.PostAsync("https://drey.alwaysdata.net/AddTemp_Hum/" + id, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Temp_Hum temp_hum = JsonConvert.DeserializeObject<Temp_Hum>(result);
                    return temp_hum;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Temp_Hum> UpdateTemp_Hum(string id, string temperature, string humidite, DateTime date)
        {
            try
            {
                HttpClient client = new HttpClient();
                string formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");
                string json = JsonConvert.SerializeObject(new { temperature = temperature, humidite = humidite, date = formattedDate });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://drey.alwaysdata.net/UpdateTemp_Hum/" + id, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Temp_Hum temp_hum = JsonConvert.DeserializeObject<Temp_Hum>(result);
                    return temp_hum;
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
