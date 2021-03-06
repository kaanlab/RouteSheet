using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RouteSheet.Data.Exceptions;
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
    public class LessonsControllerTest
    {

        [Fact(DisplayName ="Getting 5 lessons should return Ok, be typeof<List> and count 5")]
        public void Test_001()
        {
            // Arrange
            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.AllLessons())
                .Returns(GetTestLessons());
            var controller = new LessonsController(mockRepo.Object);

            // Act
            var cut = controller.GetLessons();
            var result = cut.Result as OkObjectResult;
            var value = result.Value as IList<Lesson>;

            result.StatusCode.Should().Be(200);
            result.Value.Should().NotBeNull();
            value.Should().BeOfType<List<Lesson>>();
            value.Count.Should().Be(5);
        }

        [Fact(DisplayName = "Getting empty list of lessons should return Ok, be typeof<List> and count 0")]
        public void Test_002()
        {
            // Arrange
            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.AllLessons())
                .Returns(new List<Lesson>().AsQueryable());
            var controller = new LessonsController(mockRepo.Object);

            // Act
            var cut = controller.GetLessons();
            var result = cut.Result as OkObjectResult;
            var value = result.Value as IList<Lesson>;

            result.StatusCode.Should().Be(200);
            value.Should().BeOfType<List<Lesson>>();
            value.Count.Should().Be(0);
        }

        //[Fact(DisplayName = "Adding new lesson should return created lesson")]
        //public async Task Test_003()
        //{
        //    // Arrange
        //    var testLesson = Testlesson();
        //    var newLesson = new Lesson()
        //    {
        //        AppUser = testLesson.AppUser,
        //        Cadet = testLesson.Cadet,
        //        Date = testLesson.Date,
        //        Hour = testLesson.Hour,
        //        Prioriy = testLesson.Prioriy,
        //        Title = testLesson.Title,
        //    };
        //    var mockRepo = new Mock<IAppRepository>();
        //    mockRepo.Setup(repo => repo.AddLesson(newLesson))
        //        .ReturnsAsync(testLesson);
        //    var controller = new LessonsController(mockRepo.Object);

        //    // Act
        //    var cut = await controller.Add(newLesson);
        //    var result = cut.Result as OkObjectResult;
        //    var value = result.Value as Lesson;

        //    result.StatusCode.Should().Be(200);
        //    value.Should().BeOfType<Lesson>();
        //    value.Id.Should().Be(testLesson.Id);
        //}

        //[Fact(DisplayName = "Updating existing lesson should return lesson with updated fields")]
        //public async Task Test_004()
        //{
        //    // Arrange
        //    var lessonInDb = Testlesson();
        //    var updatedLesson = new Lesson()
        //    {
        //        Id = lessonInDb.Id,
        //        AppUserId = lessonInDb.AppUserId,
        //        AppUser = lessonInDb.AppUser,
        //        CadetId = lessonInDb.CadetId,
        //        Cadet = lessonInDb.Cadet,
        //        Date = lessonInDb.Date,
        //        Hour = lessonInDb.Hour,
        //        Prioriy = lessonInDb.Prioriy,
        //        Title = "new Title",
        //    };

        //    var mockRepo = new Mock<IAppRepository>();
        //    mockRepo.Setup(repo => repo.FindLessonById(lessonInDb.Id))
        //        .ReturnsAsync(lessonInDb);
        //    mockRepo.Setup(repo => repo.UpdateLesson(updatedLesson))
        //        .ReturnsAsync(updatedLesson);
        //    var controller = new LessonsController(mockRepo.Object);

        //    // Act
        //    var cut = await controller.Update(updatedLesson);
        //    var result = cut.Result as OkObjectResult;
        //    var value = result.Value as Lesson;

        //    result.StatusCode.Should().Be(200);
        //    value.Should().BeOfType<Lesson>();
        //    value.Title.Should().Be("new Title");
        //}

        [Fact(DisplayName = "Updating not existing lesson should return Problem")]
        public async Task Test_005()
        {
            // Arrange
            var testLesson = Testlesson();
            
            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.FindLessonById(testLesson.Id))
                .ReturnsAsync((Lesson)null);
            var controller = new LessonsController(mockRepo.Object);

            // Act
            var cut = await controller.Update(testLesson);
            var result = cut.Result as ObjectResult;
            var value = result.Value as ProblemDetails;

            result.StatusCode.Should().Be(500);
            value.Title.Should().Be("Api layer problems, see details for more info");
        }

        [Fact(DisplayName = "Delete existing lesson should return NoContent")]
        public async Task Test_006()
        {
            // Arrange
            var lessonInDb = Testlesson();

            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.DeleteLesson(lessonInDb.Id))
                .ReturnsAsync(true);
            var controller = new LessonsController(mockRepo.Object);

            // Act
            var cut = await controller.Delete(lessonInDb.Id);
            var result = cut as NoContentResult;

            result.StatusCode.Should().Be(204);
        }

        [Fact(DisplayName = "Delete not existing lesson should return BadRequest")]
        public async Task Test_007()
        {
            // Arrange
            var lessonInDb = Testlesson();

            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.DeleteLesson(lessonInDb.Id))
                .ReturnsAsync(false);
            var controller = new LessonsController(mockRepo.Object);

            // Act
            var cut = await controller.Delete(lessonInDb.Id);
            var result = cut as BadRequestResult;

            result.StatusCode.Should().Be(400);
        }

        private static Lesson Testlesson()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return fixture.Create<Lesson>();
        }


        private static IQueryable<Lesson> GetTestLessons()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return fixture.CreateMany<Lesson>(5).AsQueryable();
        }
    }
}
