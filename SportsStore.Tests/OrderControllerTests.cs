using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Card()
        {
            //              Arrange Start
            Mock<IOrderRepository> mock = new();
            
            Card card = new Card();

            Order order = new Order();

            OrderController target = new(mock.Object, card);
            //              Arrange End

            //              Act Start
            ViewResult? result = target?.Checkout(order) as ViewResult;
            //              Act End

            //              Assert Start

            mock.Verify(m=>m.SaveOrder(It.IsAny<Order>()),Times.Never);
            Assert.True(string.IsNullOrEmpty(result?.ViewName));
            Assert.False(result?.ViewData.ModelState.IsValid);
            //              Assert End
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //              Arrange Start
            Mock<IOrderRepository> mock = new();

            Card card = new();

            Order order = new();

            card.AddItem(new Product(), 1);

            OrderController target = new(mock.Object, card);

            target.ModelState.AddModelError("error", "error");
            //              Arrange End

            //              Act Start
            ViewResult? result = target.Checkout(order) as ViewResult;
            //              Act End

            //              Assert Start
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            
            Assert.True(string.IsNullOrEmpty(result?.ViewName));

            Assert.False(result?.ViewData.ModelState.IsValid);
            //              Assert End
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            //              Arrange Start
            Mock<IOrderRepository> mock = new();

            Card card = new();

            Order order = new();

            card.AddItem(new Product(), 1);

            OrderController target = new(mock.Object, card);
            //              Arrange End

            //              Act Start
            RedirectToPageResult? result = target.Checkout(new()) as RedirectToPageResult;
            //              Act End

            //              Assert Start
            mock.Verify(m=>m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("/Completed", result?.PageName);
        }
    }
}
