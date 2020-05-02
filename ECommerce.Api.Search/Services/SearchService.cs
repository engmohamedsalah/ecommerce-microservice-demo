using System;
using System.Threading.Tasks;
using ECommerce.Api.Search.Interfaces;
using System.Linq;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordesrService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchService(IOrdersService ordesrService,IProductsService productsService, ICustomersService customersService)
        {
            this.ordesrService = ordesrService;
            this.productsService = productsService;
            this.customersService = customersService;
        }

        public async Task<(bool IsSuccess, dynamic SearchResult)> SearchAsync(int CustomerId)
        {
            var result = await ordesrService.GetOrdersAsync(CustomerId);
            var Products = await productsService.GetProductsAsync();
            var customers = await customersService.GetCustomerAsync(CustomerId);

            foreach (var order in result.Orders)
            {
                order.CustomerName = customers.Customer.Name;
                order.CustomerAddress = customers.Customer.Address;

                foreach (var orderItem in order.Items)
                {
                    orderItem.ProductName = Products.IsSuccess?
                        Products.Products.FirstOrDefault(a => a.Id == orderItem.ProductId)?.Name:"Product Name os not Available";
                }
            }
            
            if (result.IsScuccess)
                return (true, result.Orders);
            else
                return (false, null);
           
           
        }
    }
}
