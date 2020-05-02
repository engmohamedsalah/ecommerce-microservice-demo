﻿using System;
using System.Threading.Tasks;
using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Search.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;
        
        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
           
        }

        [HttpPost]
        public async Task<IActionResult> SearchAsync([FromBody]SearchTerm Term)
        {
            var result = await searchService.SearchAsync(Term.CustomerId);
             
            if (result.IsSuccess)
                return Ok(result.SearchResult);

            return NotFound();
        }
    }
}
