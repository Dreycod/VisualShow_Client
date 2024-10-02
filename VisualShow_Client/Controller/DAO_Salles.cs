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
    public class DAO_Salles
    {
        public async Task<List<Salles>> GetSalles()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getSalles");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    List<Salles> salles = JsonConvert.DeserializeObject<List<Salles>>(content);
                    return salles;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Salles> GetSalleById(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getSalleById/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Salles salle = JsonConvert.DeserializeObject<Salles>(content);
                    return salle;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Salles> GetSalleByName(string name)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getEtageByName/" + name);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Salles salles = JsonConvert.DeserializeObject<Salles>(content);
                    return salles;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Salles> AddSalle(string name)
        {
            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(new { name = name });
                MessageBox.Show(json);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                MessageBox.Show(content.ToString());
                HttpResponseMessage response = await client.PostAsync("https://drey.alwaysdata.net/AddSalle", content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Salles salle = JsonConvert.DeserializeObject<Salles>(result);
                    return salle;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Salles> UpdateSalle(string id, string name)
        {
            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(new { name = name });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://drey.alwaysdata.net/UpdateSalle/" + id, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Salles salle = JsonConvert.DeserializeObject<Salles>(result);
                    return salle;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<Salles> DeleteSalle(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync("https://drey.alwaysdata.net/DeleteSalle/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Salles salle = JsonConvert.DeserializeObject<Salles>(content);
                    return salle;

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
