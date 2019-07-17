using HackerNews.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNews
{
    class HackerNewsClient
    {
        const string BasePath = "https://hacker-news.firebaseio.com/v0";

        public Task<int[]> GetTopItems()
        {
            return GetAsync<int[]>("topstories");
        }

        public Task<int[]> GetIncomingItems()
        {
            return  GetAsync<int[]>("newstories");
        }

        public Task<int[]> GetPopularItems()
        {
            return GetAsync<int[]>("beststories");
        }

        public async Task<Item> GetItem(int id)
        {
            return await GetAsync<Item>($"item/{id}");
        }

        private async Task<T> GetAsync<T>(string path)
        {
            path = $"{BasePath}/{path}.json";
            
            using (var client = new HttpClient())
            {
                // Make the request
                var response = await client.GetAsync(path);
                response.EnsureSuccessStatusCode(); 

                // Convert the JSON response to the desired type
                var jsonText = await response.Content.ReadAsStringAsync();
                using (var sReader = new StringReader(jsonText))
                {
                    using (var jReader = new JsonTextReader(sReader))
                    {
                        var ser = new JsonSerializer();
                        return ser.Deserialize<T>(jReader);
                    }
                }
            }
        }
    }
}
