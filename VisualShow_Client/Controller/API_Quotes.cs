using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using WeatherView.Service;

namespace VisualShow_Client.Controller
{
    public class QuotesRoot
    {
        public string quote { get; set; }
        public string author { get; set; }
        public string category { get; set; }
    }

    public class API_Quotes
    {
        public API_Quotes() 
        { }

        public async Task<List<QuotesRoot>> GetQuotesAsync(string category)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Use the category passed to the method
                    string apiUrl = $"https://api.api-ninjas.com/v1/quotes?category={category}";

                    // Add your API key
                    client.DefaultRequestHeaders.Add("X-Api-Key", "DUFbFlpVVJWtOXJc0DwQnQ==xpjHNAkZ3F3CIXNO");

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        // Check if the response content contains an error message
                        if (content.Contains("error"))
                        {
                            return null;
                        }

                        // Deserialize the JSON response into a list of QuotesRoot objects
                        List<QuotesRoot> quotes = JsonConvert.DeserializeObject<List<QuotesRoot>>(content);

                        return quotes; // Return the list of quotes
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return null;
            }
        }
    }
}
