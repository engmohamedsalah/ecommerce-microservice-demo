using System;
using Xunit;
using ECommerce.Api.Products;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Db;
using System.Threading.Tasks;
using System.Linq;


namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        ProductsDbContext dbContext;
        public ProductsServiceTest()
        {
            var contextOption = new DbContextOptionsBuilder<ProductsDbContext>()
              .UseInMemoryDatabase(nameof(GetProductsReturnAllProductsAsync));
            dbContext = new ProductsDbContext(contextOption.Options);
            CreateProducts(dbContext);
        }
        [Fact]
        public async Task GetProductsReturnAllProductsAsync()
        {
            //Arrange
          

            var profile = new ProductProfile();
            var mapperConfig = new MapperConfiguration(a => a.AddProfile(profile));
            var mapper = new Mapper(mapperConfig);
            var productsProvider = new ProductsProvider(dbContext,null,mapper);
            //act
            var produsts = await productsProvider.GetProductsAsync();
            //Assert
            Assert.True(produsts.IsSuccess);
            Assert.True(produsts.Products.Any());
            Assert.Null(produsts.ErrorMessage);

        }
        [Fact]
        public async Task GetProductReturnProductUsingValidIdAsync()
        {
            //Arrange
            
            var profile = new ProductProfile();
            var mapperConfig = new MapperConfiguration(a => a.AddProfile(profile));
            var mapper = new Mapper(mapperConfig);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            //act
            var produsts = await productsProvider.GetProductAsync(1);
            //Assert
            Assert.True(produsts.IsSuccess);
            Assert.NotNull(produsts.Product);
            Assert.Null(produsts.ErrorMessage);

        }
        [Fact]
        public async Task GetProductReturnProductUsingInValidIdAsync()
        {
            //Arrange

            var profile = new ProductProfile();
            var mapperConfig = new MapperConfiguration(a => a.AddProfile(profile));
            var mapper = new Mapper(mapperConfig);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            //act
            var produsts = await productsProvider.GetProductAsync(-1);
            //Assert
            Assert.False(produsts.IsSuccess);
            Assert.Null(produsts.Product);
            Assert.NotNull(produsts.ErrorMessage);

        }

        private void CreateProducts(ProductsDbContext dbContext)
        {
            if (dbContext.Products.Any()) return;
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Products.Add(new Product() {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory  = i+10,
                    Price = (decimal)(i*3.14)
                });

            }
            dbContext.SaveChanges();
        }
    }
}
