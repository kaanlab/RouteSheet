using Microsoft.AspNetCore.Mvc;
using Moq;
using RouteSheet.Data.Repositories;
using RouteSheet.Server.Controllers;
using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RouteSheet.Server.Tests
{
    public class LessonControllerTest
    {

        [Fact]
        public async Task GetLessons_ReturnsAViewResult_WithAListOfBrainstormSessions()
        {
            // Arrange
            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.GetLessons())
                .Returns(GetTestLessons());
            var controller = new LessonsController(mockRepo.Object);

            // Act
            var cut = controller.GetLessons();
            var result = cut.Result as OkResult;
            var v = cut.Value;
            //var contentResult = result
            // Assert
            //var viewResult = Assert.IsType<ViewResult>(result);
            //var model = Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(
            //    viewResult.ViewData.Model);
            //Assert.Equal(2, model.Count());
            //Assert.IsType<>
        }


        private IQueryable<Lesson> GetTestLessons()
        {
            return new List<Lesson>
            {
                new Lesson()
                {
                    Id = 1,

                },
                new Lesson()
                {
                    Id = 2,

                }

            }.AsQueryable();
        }

    }
}
