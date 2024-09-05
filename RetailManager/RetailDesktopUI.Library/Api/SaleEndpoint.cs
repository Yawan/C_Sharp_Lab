using Newtonsoft.Json;
using RetailDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RetailDesktopUI.Library.Api
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private readonly IAPIHelper _apiHelper;

        public SaleEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task PostSale(SaleModel sale)
        {
            try
            {
                using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
                {
                    response.EnsureSuccessStatusCode();

                    // TODO: Log successful call
                    // 例如：_logger.LogInformation($"Sale posted successfully. Sale ID: {sale.Id}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"HTTP request failed when posting sale. Message: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Failed to serialize sale data to JSON", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred while posting sale", ex);
            }
        }
    }
}