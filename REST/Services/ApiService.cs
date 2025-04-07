using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using CosmosOdyssey.REST.Models;
using Newtonsoft.Json;

namespace CosmosOdyssey.REST.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PriceList> GetTravelPriceListAsync()
        {
            var response = await _httpClient.GetAsync("https://cosmosodyssey.azurewebsites.net/api/v1.0/TravelPrices");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var travelPriceList = JsonConvert.DeserializeObject<PriceList>(json);

            return travelPriceList;
        }
    }

}