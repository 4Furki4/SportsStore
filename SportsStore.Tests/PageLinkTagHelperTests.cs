﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Tests
{
    public class PageLinkTagHelperTests
    {
        [Fact]
        public void Can_Generate_Page_Links()
        {
            // Assert
            var UrlHelper = new Mock<IUrlHelper>();
            UrlHelper.SetupSequence(a => a.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");
            var UrlHelperFactory = new Mock<IUrlHelperFactory>();
            UrlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(UrlHelper.Object);

            var viewContext = new Mock<ViewContext>();
            PageLinkTagHelper Helper = new PageLinkTagHelper(UrlHelperFactory.Object) { PageModel = new PagingInfo 
            { 
                CurrentPage = 2, TotalItems = 28, ItemsPerPage = 10 
            }, 
            viewContext = viewContext.Object, 
            PageAction = "Test"  };

            TagHelperContext ctx = new(new TagHelperAttributeList(), new Dictionary<object, object>(), "");

            var content = new Mock<TagHelperContent>();
            TagHelperOutput output = new TagHelperOutput("div", new TagHelperAttributeList(), (cache, encoder) => Task.FromResult(content.Object));

            //Act 
            Helper.Process(ctx, output);

            // Assert 

            Assert.Equal(@"<a href=""Test/Page1"">1</a>"
                        + @"<a href=""Test/Page2"">2</a>"
                        + @"<a href=""Test/Page3"">3</a>",
            output.Content.GetContent());
        }
    }
}
