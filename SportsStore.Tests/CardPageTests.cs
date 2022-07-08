using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace SportsStore.Tests
{
    public class CardPageTests
    {
        [Fact]
        public void Can_Load_Cart()
        {
            // Arrange
            // - create a mock repository
            Product p1 = new Product { Id = 1, Name = "P1" };
            Product p2 = new Product { Id = 2, Name = "P2" };
            Mock<IStoreRepository> mockRepo = new();
            mockRepo.Setup(m => m.Products).Returns((new Product[] {
             p1, p2
             }).AsQueryable<Product>());
            // - create a cart

            Card testCard = new();
            testCard.AddItem(p1, 2);
            testCard.AddItem(p2, 1);

            // - create a mock page context and session

            // Action


            CartModel cartModel = new CartModel(mockRepo.Object, testCard);
            cartModel.OnGet("myUrl");


            //Assert
            Assert.Equal(2, cartModel.Card?.Lines.Count());
            Assert.Equal("myUrl", cartModel.ReturnUrl);
        }

        [Fact]
        public void Can_Update_Cart()
        {
            // Arrange
            // - create a mock repository
            Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns((new Product[] {
             new Product { Id = 1, Name = "P1" }
             }).AsQueryable<Product>());
            Card? testCart = new();
            // Action
            CartModel cartModel = new CartModel(mockRepo.Object, testCart);
            cartModel.OnPost(1, "myUrl");

            Assert.Single(testCart.Lines);
            Assert.Equal("P1", testCart.Lines.First().Product.Name);
            Assert.Equal(1, testCart.Lines.First().Quantity);
        }
    }       
}
