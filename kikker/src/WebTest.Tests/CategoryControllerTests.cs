using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using WebTest.Controllers;
using WebTest.Services;
using Xunit;

namespace WebTest.Tests
{
    public class CategoryControllerTests
    {
		

		[Fact]
		public async Task Index_ReturnsAViewResult_WithAListOfCategory()
		{
			// Arrange
			var mockRepo = new Mock<IValue>();
			mockRepo.Setup(repo => repo.GetValuesAsync()).Returns(Task.FromResult(GetTestValues()));
			var controller = new HomeController(mockRepo.Object);

			// Act
			var result = await controller.Index();

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsType<HomeViewModel>(viewResult.ViewData.Model);
			Assert.Equal("value3", model.Name);			
		}

		private List<string> GetTestValues()
		{
			return new List<string> { "value3", "value4" };
		}
	}
}
