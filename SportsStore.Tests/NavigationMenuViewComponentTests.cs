using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests 
    {
        [Fact]
        public void Can_Select_Categories()
        {
            // Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
             new Product {Id = 1, Name = "P1",
             Category = "Apples"},
             new Product {Id = 2, Name = "P2",
             Category = "Apples"},
             new Product {Id= 3, Name = "P3",
             Category = "Plums"},
             new Product {Id= 4, Name = "P4",
             Category = "Oranges"},
             }).AsQueryable<Product>());
            NavigationMenuViewComponent target =
            new NavigationMenuViewComponent(mock.Object);

            // Act = get the set of categories
            string[] results = ((target.Invoke()
            as ViewViewComponentResult)?.ViewData?.Model as IEnumerable<string>
            ?? Enumerable.Empty<string>()).ToArray();
            // Assert
            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples",
             "Oranges", "Plums" }, results));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            // Arrange
            string categoryToSelect = "Apples";
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
            new Product {Id = 1, Name = "P1", Category = "Apples"},
            new Product {Id = 4, Name = "P2", Category = "Oranges"},
            }).AsQueryable<Product>());
            NavigationMenuViewComponent target = new(mock.Object);

            target.ViewComponentContext = new ViewComponentContext { ViewContext = new() //RouteData Arranged
            { 
                RouteData = new Microsoft.AspNetCore.Routing.RouteData()} 
            };
            target.RouteData.Values["category"] = categoryToSelect;

            //Act

            string? result = (string?)(target.Invoke() as ViewViewComponentResult)?.ViewData?["SelectedCategory"];
            Assert.Equal(categoryToSelect, result);
        }
    }
}
