using CacheServiceApp.Model;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace CacheServiceApp.Services
{
    public class BirdsService : IBirdsService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;

        public BirdsService(HttpClient httpClient, IDistributedCache cache) => (_httpClient, _cache) = (httpClient, cache);

        public async Task<List<Bird>> Get()
        {
            string cacheKey = "listBirds";
            string serializedBirds;

            var redisBirds = await _cache.GetAsync(cacheKey);
            List<Bird> listBirds;

            if (redisBirds != null)
            {
                serializedBirds = Encoding.UTF8.GetString(redisBirds);
                listBirds = JsonConvert.DeserializeObject<List<Bird>>(serializedBirds);
            }
            else
            {
                listBirds = await _httpClient.GetFromJsonAsync<List<Bird>>(_httpClient.BaseAddress);
                serializedBirds = JsonConvert.SerializeObject(listBirds);
                redisBirds = Encoding.UTF8.GetBytes(serializedBirds);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                await _cache.SetAsync(cacheKey, redisBirds, options);
            }

            return listBirds;
        }
    }
}
