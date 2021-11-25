using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
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

        [Fact(DisplayName ="GetLessons: getting 5 lessons should return Ok, be typeof<List> and count 5")]
        public void GetLessons_001()
        {
            // Arrange
            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.GetLessons())
                .Returns(GetTestLessons());
            var controller = new LessonsController(mockRepo.Object);

            // Act
            var cut = controller.GetLessons();
            var result = cut.Result as OkObjectResult;
            var value = result.Value as IList<Lesson>;

            result.StatusCode.Should().Be(200);
            result.Value.Should().NotBeNull();
            value.Should().BeOfType<List<Lesson>>();
            value.Count().Should().Be(5);
        }

        [Fact(DisplayName = "GetLessons: getting empty list of lessons should return Ok, be typeof<List> and count 0")]
        public void GetLessons_002()
        {
            // Arrange
            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.GetLessons())
                .Returns(new List<Lesson>().AsQueryable());
            var controller = new LessonsController(mockRepo.Object);

            // Act
            var cut = controller.GetLessons();
            var result = cut.Result as OkObjectResult;
            var value = result.Value as IList<Lesson>;

            result.StatusCode.Should().Be(200);
            value.Should().BeOfType<List<Lesson>>();
            value.Count().Should().Be(0);
        }


        private IQueryable<Lesson> GetTestLessons()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return fixture.CreateMany<Lesson>(5).AsQueryable();
        }
    }
}
