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
    public class DAO_Ecrans
    {
        public async Task<List<Ecrans>> GetEcrans()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getEcrans");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    List<Ecrans> ecrans = JsonConvert.DeserializeObject<List<Ecrans>>(content);
                    return ecrans;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Ecrans> GetEcranById(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getEcranById/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Ecrans ecran = JsonConvert.DeserializeObject<Ecrans>(content);
                    return ecran;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Ecrans> GetEcranByName(string name)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getEcranByName/" + name);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Ecrans ecran = JsonConvert.DeserializeObject<Ecrans>(content);
                    return ecran;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Ecrans> AddEcran(string name, DateTime date, string IsOn, string id_salle)
        {
            try
            {
                HttpClient client = new HttpClient();
                string formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");
                string json = JsonConvert.SerializeObject(new { name = name, lastUpdate = formattedDate, IsOn = IsOn, id_salle = id_salle });
                MessageBox.Show(json);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                MessageBox.Show(content.ToString());
                HttpResponseMessage response = await client.PostAsync("https://drey.alwaysdata.net/AddEcran", content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Ecrans ecran = JsonConvert.DeserializeObject<Ecrans>(result);
                    return ecran;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Ecrans> UpdateEcran(string id, string name, DateTime date, string IsOn, string id_salle)
        {
            try
            {
                HttpClient client = new HttpClient();
                string formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");
                string json = JsonConvert.SerializeObject(new { name = name, lastUpdate = formattedDate, IsOn = IsOn, id_salle = id_salle });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://drey.alwaysdata.net/UpdateEcran/" + id, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Ecrans ecran = JsonConvert.DeserializeObject<Ecrans>(result);
                    return ecran;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<Ecrans> DeleteEcran(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync("https://drey.alwaysdata.net/DeleteEcran/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Ecrans ecran = JsonConvert.DeserializeObject<Ecrans>(content);
                    return ecran;

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
