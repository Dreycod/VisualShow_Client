using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VisualShow_Admin.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VisualShow_Admin.Controller
{
    public class DAO_Users
    {
        public async Task<List<Users>> GetUsers()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getUsers");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    List<Users> users = JsonConvert.DeserializeObject<List<Users>>(content);
                    return users;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Users> GetUserById(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getUserById/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Users user = JsonConvert.DeserializeObject<Users>(content);
                    return user;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Users> GetUserByName(string name)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getUserByName/" + name);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Users user = JsonConvert.DeserializeObject<Users>(content);
                    return user;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Users> AddUser(string name, string type, DateTime date_creation, string mdp)
        {
            try
            {
                HttpClient client = new HttpClient();
                string formattedDate = date_creation.ToString("yyyy-MM-dd HH:mm:ss");
                string json = JsonConvert.SerializeObject(new { name = name, type = type, date_creation = formattedDate, mdp = mdp });
                MessageBox.Show(json);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                MessageBox.Show(content.ToString());
                HttpResponseMessage response = await client.PostAsync("https://drey.alwaysdata.net/AddUser", content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Users user = JsonConvert.DeserializeObject<Users>(result);
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Users> UpdateUser(string id, string name, string type, DateTime date_creation, string mdp)
        {
            try
            {
                HttpClient client = new HttpClient();
                string formattedDate = date_creation.ToString("yyyy-MM-dd HH:mm:ss");
                string json = JsonConvert.SerializeObject(new { name = name, type = type, date_creation = formattedDate, mdp = mdp });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://drey.alwaysdata.net/UpdateUser/" + id, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("error"))
                    {
                        MessageBox.Show("Error: " + result);
                        return null;
                    }
                    Users user = JsonConvert.DeserializeObject<Users>(result);
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<Users> DeleteUser(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync("https://drey.alwaysdata.net/DeleteUser/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    Users user = JsonConvert.DeserializeObject<Users>(content);
                    return user;

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
