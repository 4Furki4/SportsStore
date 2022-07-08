using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Tests
{
    public class CardTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            // Arrange
            Product p1 = new() { Id = 1, Name = "p1" };
            Product p2 = new() { Id = 2, Name = "p2" };
            Card target = new();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CardLine[] results = target.Lines.ToArray();

            // Assert

            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Line()
        {
            // Arrange
            Product p1 = new() { Id = 1, Name = "p1" };
            Product p2 = new() { Id = 2, Name = "p2" };
            Card target = new();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 9);
            target.AddItem(p2, 19);

            CardLine[] results = target.Lines.ToArray();

            // Assert
            
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(10, results[0].Quantity);

            Assert.Equal(p2, results[1].Product);
            Assert.Equal(20, results[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            // Arrange
            Product p1 = new() { Id = 1, Name = "p1" };
            Product p2 = new() { Id = 2, Name = "p2" };
            Card target = new();
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            // Act

            target.RemoveLine(p1);

            // Assert

            Assert.Empty( target.Lines.Where(p=>p.Product == p1) );
            Assert.Equal(1, target.Lines?.Count());
        }
        [Fact]
        public void Can_Calculate_Total_Value()
        {
            // Arrange
            Product p1 = new() { Id = 1, Name = "p1", Price = 50M };
            Product p2 = new() { Id = 2, Name = "p2", Price = 150M };
            Card target = new();
            target.AddItem(p1, 5);
            target.AddItem(p2, 3);

            // Act

            decimal total = target.ComputeTotalValue();

            // Assert

            Assert.Equal((50*5+150*3), total);
        }

        [Fact]
        public void Can_Purge_Card()
        {
            // Arrange

            Product p1 = new() { Id = 1, Name = "p1", Price = 50M };
            Product p2 = new() { Id = 2, Name = "p2", Price = 150M };
            Card target = new();
            target.AddItem(p1, 5);
            target.AddItem(p2, 3);

            // Act

            target.Clear();

            // Assert

            Assert.Empty(target.Lines);
        }
    }
}
