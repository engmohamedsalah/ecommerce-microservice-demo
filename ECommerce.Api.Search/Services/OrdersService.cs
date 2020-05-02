using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Search.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<OrdersService> logger;
        public OrdersService(IHttpClientFactory httpClientFactory,ILogger<OrdersService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<(bool IsScuccess, dynamic Orders, string ErrorMessage)> GetOrdersAsync(int CustomerId)
        {
            try
            {
                var client = httpClientFactory.CreateClient("OrdersService");
                var response = await client.GetAsync($"api/Orders/{CustomerId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<IEnumerable<Orders>>(content, option);
                    return (true, result, null);
                }
                else
                    return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
