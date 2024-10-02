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
    public class DAO_Air
    {
        public async Task<List<Air>> GetAir(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getAir/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    List<Air> air = JsonConvert.DeserializeObject<List<Air>>(content);
                    return air;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Air> AddAir(string id, string air, DateTime date)
        {
            try
            {
                HttpClient client = new HttpClient();
                string formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");
                string json = JsonConvert.SerializeObject(new { air = air, date = formattedDate });
                MessageBox.Show(json);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                MessageBox.Show(content.ToString());
                HttpResponseMessage response = await client.PostAsync("https://drey.alwaysdata.net/AddAir/" + id, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Air airs = JsonConvert.DeserializeObject<Air>(result);
                    return airs;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Air> UpdateAir(string id, string air, DateTime date)
        {
            try
            {
                HttpClient client = new HttpClient();
                string formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");
                string json = JsonConvert.SerializeObject(new { air = air, date = formattedDate });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://drey.alwaysdata.net/UpdateAir/" + id, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Air airs = JsonConvert.DeserializeObject<Air>(result);
                    return airs;
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
