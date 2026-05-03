using System.Collections.Generic;
using MyApi.Entity;

namespace MyApi.Data
{
    public class ProductsRepository
    {
        public List<Product> GetAllProducts()
        {
            List<Product> productList = new List<Product>();

            for(int i = 1; i <= 100; i++)
            {
                productList.Add(new Product
                {
                    Id = i,
                    Name = $"Product {i}",
                    Price = 10.0m * i
                });
            }

            return productList;
        }   
    }
}
