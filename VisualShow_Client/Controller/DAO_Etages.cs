using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using VisualShow_Admin.Model;

namespace VisualShow_Admin.Controller
{
    public class DAO_Etages
    {
        public async Task<List<Etages>> GetEtages()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getEtages");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    List<Etages> etages = JsonConvert.DeserializeObject<List<Etages>>(content);
                    return etages;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Etages> GetEtageById(string id)
                    {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getEtageById/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Etages etage = JsonConvert.DeserializeObject<Etages>(content);
                    return etage;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Etages> GetEtageByName(string name)
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
                    Etages etage = JsonConvert.DeserializeObject<Etages>(content);
                    return etage;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Etages> AddEtage(string name)
        {
            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(new { name = name });
                MessageBox.Show(json);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                MessageBox.Show(content.ToString());
                HttpResponseMessage response = await client.PostAsync("https://drey.alwaysdata.net/AddEtage", content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Etages etage = JsonConvert.DeserializeObject<Etages>(result);
                    return etage;
                }
                return null;
            }  
            catch (Exception ex) 
            {
                return null;
            }   
        }
        public async Task<Etages> UpdateEtage(string id, string name)
        { 
            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(new { name = name });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://drey.alwaysdata.net/UpdateEtage/" + id, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Etages etage = JsonConvert.DeserializeObject<Etages>(result);
                    return etage;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        
        }
        public async Task<Etages> DeleteEtage(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync("https://drey.alwaysdata.net/DeleteEtage/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Etages etage = JsonConvert.DeserializeObject<Etages>(content);
                    return etage;

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
