using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void CanUseRepository()
        {
            // Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{Id = 1, Name = "TestProduct1"},
                new Product{Id = 2, Name = "TestProduct2"}
            }).AsQueryable<Product>());
            HomeController controller = new(mock.Object);

            // Act
            ProductListViewModel result = controller.Index(null)?.ViewData.Model as ProductListViewModel ?? new();

            // Assert

            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("TestProduct1", prodArray[0].Name);
            Assert.Equal("TestProduct2", prodArray[1].Name);
        }

        [Fact]
        public void Can_Paginize()
        {
            // Arrange
            Mock<IStoreRepository> mock = new();
            mock.Setup(p => p.Products).Returns((new Product[]
            {
                new Product {Id = 1, Name = "P1"},
                new Product {Id = 2, Name = "P2"},
                new Product {Id = 3, Name = "P3"},
                new Product {Id = 4, Name = "P4"},
                new Product {Id = 5, Name = "P5"}
            }).AsQueryable<Product>());
            HomeController controller = new(mock.Object);
            controller.PageSize = 3;

            //Act
            ProductListViewModel result = (controller.Index(null,2)?.ViewData.Model) as ProductListViewModel ?? new();

            //Assert

            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] 
            {
                new Product {Id = 1, Name = "P1"},
                new Product {Id = 2, Name = "P2"},
                new Product {Id = 3, Name = "P3"},
                new Product {Id = 4, Name = "P4"},
                new Product {Id = 5, Name = "P5"}
            }).AsQueryable<Product>());
            HomeController controller = new(mock.Object) { PageSize = 3 };

            //Act
            var result = controller.Index(null,2)?.ViewData.Model as ProductListViewModel?? new();

            // Assert
            var prodArray = result.Products.ToArray();
            PagingInfo pageInfo = result.PagingInfo;
            Assert.True(prodArray.Length == 2);
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            // Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(p => p.Products).Returns((new Product[] {
             new Product {Id = 1, Name = "P1", Category = "Cat1"},
             new Product {Id = 2, Name = "P2", Category = "Cat2"},
             new Product {Id = 3, Name = "P3", Category = "Cat1"},
             new Product {Id = 4, Name = "P4", Category = "Cat2"},
             new Product {Id = 5, Name = "P5", Category = "Cat3"}
             }).AsQueryable<Product>());
            HomeController controller = new HomeController(mock.Object) { PageSize = 3 };

            // Act
            var result = (controller.Index("Cat2")?.ViewData.Model as ProductListViewModel ?? new()).Products.ToArray();

            // Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");

        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            //Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
             new Product {Id = 1, Name = "P1", Category = "Cat1"},
             new Product {Id= 2, Name = "P2", Category = "Cat2"},
             new Product {Id= 3, Name = "P3", Category = "Cat1"},
             new Product {Id= 4, Name = "P4", Category = "Cat2"},
             new Product {Id= 5, Name = "P5", Category = "Cat3"}
             }).AsQueryable<Product>());
            var controller = new HomeController(mock.Object) { PageSize = 3 };

            Func<ViewResult, ProductListViewModel?> GetModel = result => result?.ViewData?.Model as ProductListViewModel;

            //Act

            int? res1 = GetModel(controller.Index("Cat1"))?.PagingInfo.TotalItems;
            int? res2 = GetModel(controller.Index("Cat2"))?.PagingInfo.TotalItems;
            int? res3 = GetModel(controller.Index("Cat3"))?.PagingInfo.TotalItems;
            int? resAll = GetModel(controller.Index(null))?.PagingInfo.TotalItems;

            //Assert

            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }

    }
}
