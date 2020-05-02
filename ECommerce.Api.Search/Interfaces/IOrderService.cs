using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Interfaces
{
    public interface IOrdersService
    {
        Task<(bool IsScuccess, dynamic Orders, string ErrorMessage)> GetOrdersAsync(int CustomerId);
    }
}
